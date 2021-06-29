using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;

namespace InstrumentUI.DataAccess
{
    public static class SqlCeUpgrade
    {
        /// <summary>
        /// Possible SQL CE Versions
        /// </summary>
        private enum SqlCeVersion
        {
            SqlCe20 = 0,
            SqlCe30 = 1,
            SqlCe35 = 2,
            SqlCe40 = 3
        }

        
        /// <summary>
        /// If the current database is not SQL CE 4.0, then upgrade it
        /// </summary>
        /// <param name="engine">SQL CE Database engine</param>
        /// <param name="filename">Filename of the SQL CE Database</param>
        public static void EnsureVersion40(this SqlCeEngine engine, string filename)
        {
            var fileversion = DetermineVersion(filename);

            if (fileversion == SqlCeVersion.SqlCe20)
                throw new ApplicationException("Unable to upgrade from 2.0 to 4.0");
            
            if (SqlCeVersion.SqlCe40 > fileversion)
                engine.Upgrade();
        }


        /// <summary>
        /// Determine the SQL CE Version
        /// </summary>
        /// <param name="filename">Filename of the SQL CE Database</param>
        /// <returns>SQL CE Version</returns>
        private static SqlCeVersion DetermineVersion(string filename)
        {
            var versionDictionary = new Dictionary<int, SqlCeVersion>
                                        {
                                            {0x73616261, SqlCeVersion.SqlCe20},
                                            {0x002dd714, SqlCeVersion.SqlCe30},
                                            {0x00357b9d, SqlCeVersion.SqlCe35},
                                            {0x003d0900, SqlCeVersion.SqlCe40}
                                        };
            int versionLongword;

            using (var fs = new FileStream(filename, FileMode.Open))
            {
                fs.Seek(16, SeekOrigin.Begin);
                using (var reader = new BinaryReader(fs))
                {
                    versionLongword = reader.ReadInt32();
                }
            }

            if (versionDictionary.ContainsKey(versionLongword))
                return versionDictionary[versionLongword];

            throw new ApplicationException("Unable to determine database file version");
        }
    }
}
