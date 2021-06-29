using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using InstrumentUI_ATK.DataAccess.Model;

namespace InstrumentUI_ATK.DataAccess
{
    public class ScheduleRepository
    {
        /// <summary>
        /// File name that the underlying DataSet is save to
        /// </summary>
        private string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    _fileName = ConfigurationManager.AppSettings["ScheduleFilePath"];
                    if (string.IsNullOrEmpty(_fileName))
                        throw new ArgumentNullException("ScheduleFilePath", "Argument must be set in the appSettings section of the application's configuration file.");
                }
                return _fileName;
            }
        }
        private string _fileName = "";


        /// <summary>
        /// The underlying DataSet which stores the Schedule data
        /// </summary>
        private ScheduleDataSet Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new ScheduleDataSet();

                    if (File.Exists(FileName))
                        _data.ReadXml(FileName);
                }
                return _data;
            }
        }
        private ScheduleDataSet _data;


        /// <summary>
        /// Retrieves the Schedule
        /// </summary>
        /// <param name="userName">The UserName that the Schedule is associated with</param>
        /// <returns>Schedule</returns>
        public List<ScheduleItem> GetSchedule(string userName)
        {
            var scheduleList = new List<ScheduleItem>();
            var filter = string.Format("UserName = '{0}'", userName);
            foreach (ScheduleDataSet.ScheduleRow item in Data.Schedule.Select(filter))
            {
                var scheduleItem = new ScheduleItem
                                       {
                                           UserName = item.UserName,
                                           LocationNumber = item.LocationNumber,
                                           IsActive = false, //item.IsActive,
                                           MaterialId = item.MaterialId,
                                           Material = item.Material,
                                           ScansPerHour = item.ScansPerHour,
                                           UserField1 = item.UserField1,
                                           UserField2 = item.UserField2
                                       };
                scheduleList.Add(scheduleItem);
            }

            // Make sure that there is an entry for all 6 Sample Locations
            for (short i = 1; i <= 6; i++)
            {
                // Skip it, if it already exists
                if (scheduleList.Exists(s => s.LocationNumber == i)) continue;

                // Otherwise, create a new entry for the Location Number
                var scheduleItem = new ScheduleItem
                                       {
                                           UserName = userName,
                                           LocationNumber = i
                                       };
                scheduleList.Add(scheduleItem);
            }

            return scheduleList;
        }


        /// <summary>
        /// Save the Schedule
        /// </summary>
        /// <param name="schedule">Schedule to be saved</param>
        /// <returns>True, if save was successful; False, otherwise.</returns>
        public bool SaveSchedule(List<ScheduleItem> schedule)
        {
            foreach (var item in schedule)
            {
                ScheduleDataSet.ScheduleRow row; 
                var filter = string.Format("LocationNumber = {0} AND UserName = '{1}'", item.LocationNumber, item.UserName);
                var rows = Data.Schedule.Select(filter);
                if (rows.Length == 1)
                {
                    row = (ScheduleDataSet.ScheduleRow) rows[0];
                }
                else
                {
                    row = Data.Schedule.NewScheduleRow();
                    row.UserName = item.UserName;
                    row.LocationNumber = item.LocationNumber;
                    Data.Schedule.AddScheduleRow(row);
                }

                row.IsActive = item.IsActive;
                row.MaterialId = item.MaterialId;
                row.UserField1 = item.UserField1;
                row.UserField2 = item.UserField2;
                row.ScansPerHour = item.ScansPerHour;
            }

            Data.WriteXml(FileName);
            return true;
        }
    }
}
