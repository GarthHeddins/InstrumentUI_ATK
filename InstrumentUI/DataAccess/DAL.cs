using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.DataAccess
{
    public static class DAL
    {
        private const string ACTIVE_ONLY = "A";
        private const string DISABLED_ONLY = "D";

        private static readonly InstrumentUIDataContext DataContext;
        public static List<String> materialList;
        public static List<String> requestIDs;


        static DAL()
        {
            DataContext = new InstrumentUIDataContext(Helper.CONNECTION_STRING);
        }


        public static DataService.User AuthenticateUser(string username, string password)
        {
            var user = new DataService.User();

            IEnumerable<User> userDetails = from u in DataContext.Users
                                            where u.UserName == username && u.Password == password
                                            select u;

            try
            {
                foreach (var users in userDetails)
                {
                    Console.WriteLine(users.Password, users.UserName);
                    user.UserName = users.UserName;

                    user.Id = users.Id;
                    user.UserName = users.UserName;
                    user.FirstName = users.FirstName;
                    user.LastName = users.LastName;
                    user.UserRole = new DataService.UserRole {RoleName = users.Role};
                    string status = users.Status;
                    user.Status = status.Equals(ACTIVE_ONLY, StringComparison.InvariantCultureIgnoreCase)
                                      ? DataService.EnumStatus.ACTIVE
                                      : status.Equals(DISABLED_ONLY, StringComparison.InvariantCultureIgnoreCase)
                                            ? DataService.EnumStatus.DISABLED
                                            : DataService.EnumStatus.TERMINATED;

                    user.Location = new DataService.Location {LocationId = (int) users.LocationId, LocationName = users.LocationName};
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return user;
        }


        public static IEnumerable<Trait> GetAllTrait()
        {
            return from t in DataContext.Traits
                   select t;
        }


        /// <summary>
        /// Get all sample identifier attributes from the local cache based on the specified material id
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public static IEnumerable<SampleIdentifier> GetSampleIdentifiersByMaterial(int materialId)
        {
            Trace.WriteLine("Enter DAL.GetSampleIdentifierByMaterial()");
            var list = from s in DataContext.SampleIdentifiers
                       where s.MaterialId == materialId
                       orderby s.DisplayOrder
                       select s;
            Trace.WriteLine("Exit DAL.GetSampleIdentifierByMaterial()");
            return list;
        }


        public static IEnumerable<RecordedSampleIdentifier> GetPreviousSampleIdentifiersByMaterial(int materialId)
        {
            return from r in DataContext.RecordedSampleIdentifiers
                   where r.MaterialId == materialId
                   select r;
        }


        public static IEnumerable<SampleClass> GetSampleClasses()
        {
            return from s in DataContext.SampleClasses
                   select s;
        }


        public static SpectrometerType GetSpectrometerType()
        {
            return (from s in DataContext.SpectrometerTypes
                    select s).FirstOrDefault();
        }


        public static User GetUser()
        {
            return (from s in DataContext.Users
                    select s).FirstOrDefault();
        }

        public static AdminPreference GetAdminPreference()
        {
            return DataContext.GetTable<AdminPreference>().FirstOrDefault();
        }


        public static void InsertAdminPreference(string language, bool autoPrint, bool autoSampleId, bool soundsOn, bool speedMode, bool speedModeDualScan)
        {
            Table<AdminPreference> adminPreferences = DataContext.GetTable<AdminPreference>();
            var adminPreference = new AdminPreference
                                      {
                                          Language = language,
                                          AutoPrint = autoPrint,
                                          AutoSampleId = autoSampleId,
                                          SoundsOn = soundsOn,
                                          SpeedMode = speedMode,
                                          SpeedModeDualScan = speedModeDualScan
                                      };

            adminPreferences.InsertOnSubmit(adminPreference);
            DataContext.SubmitChanges();
        }


        public static void InsertAdminPreference(string logoPath, bool algorithmVersion, string reportAddress1, string reportAddress2,
                                          string mailTo, string defautReport, string reportDirectory)
        {
            string defaultLanguage = ConfigurationManager.AppSettings[Helper.DEFAULT_LANGUAGE];

            Table<AdminPreference> adminPreferences = DataContext.GetTable<AdminPreference>();
            var adminPreference = new AdminPreference
                                      {
                                          LogoFilePath = logoPath,
                                          AlgorithamVersion = algorithmVersion,
                                          AddressOnReportLine1 = reportAddress1,
                                          AddressOnReportLine2 = reportAddress2,
                                          Email = mailTo,
                                          Language = defaultLanguage,
                                          DefaultReport = defautReport,
                                          ReportDirectory = reportDirectory
                                      };

            adminPreferences.InsertOnSubmit(adminPreference);
            DataContext.SubmitChanges();
        }


        public static void UpdateAdminPreference(string language, bool autoPrint, bool autoSampleId, bool soundsOn, bool speedMode, bool speedModeDualScan)
        {
            var adminPreference = DataContext.GetTable<AdminPreference>().FirstOrDefault();

            if (null != adminPreference)
            {
                adminPreference.Language = language;
                adminPreference.AutoPrint = autoPrint;
                adminPreference.AutoSampleId = autoSampleId;
                adminPreference.SoundsOn = soundsOn;
                adminPreference.SpeedMode = speedMode;
                adminPreference.SpeedModeDualScan = speedModeDualScan;
            }

            DataContext.SubmitChanges();
        }


        public static void UpdateAdminPreference(string logoPath, bool algorithmVersion, string reportAddress1, string reportAddress2,
                                          string mailTo, string defaultReport, string reportDirectory)
        {
            var adminPreference = DataContext.GetTable<AdminPreference>().FirstOrDefault();

            if (null != adminPreference)
            {
                adminPreference.LogoFilePath = logoPath;
                adminPreference.AlgorithamVersion = algorithmVersion;
                adminPreference.AddressOnReportLine1 = reportAddress1;
                adminPreference.AddressOnReportLine2 = reportAddress2;
                adminPreference.Email = mailTo;
                adminPreference.DefaultReport = defaultReport;
                adminPreference.ReportDirectory = reportDirectory;
            }

            DataContext.SubmitChanges();
        }


        public static string GetCulture()
        {
            try
            {
                AdminPreference adminPreference = DataContext.GetTable<AdminPreference>().FirstOrDefault();

                if (adminPreference == null || adminPreference.Language.IsNullOrWhiteSpace())
                    return "en-US";
                else
                    return adminPreference.Language;
            }
            catch (SqlCeException se)
            {
                Helper.LogError("GetCulture", string.Empty, se, false);
                return "en-US";
            }
            catch (Exception e)
            {
                Helper.LogError("GetCulture", string.Empty, e, false);
                return "en-US";
            }
        }


        public static void InsertRecordedSampleIdentifiers(Sample currentSample, int materialId)
        {
            // delete any previous recorded sample identifiers for specified material
            var lastRecordedSI = DataContext.RecordedSampleIdentifiers.Where(r => r.MaterialId == materialId);
            DataContext.RecordedSampleIdentifiers.DeleteAllOnSubmit(lastRecordedSI);

            foreach (SampleIdentifier identifier in currentSample.SampleIdentifiers)
            {
                var recordedSampleIdentifier = new RecordedSampleIdentifier
                                                   {
                                                       Id = Guid.NewGuid(),
                                                       MaterialId = materialId,
                                                       Name = identifier.Name,
                                                       Value = identifier.Value
                                                   };
                DataContext.RecordedSampleIdentifiers.InsertOnSubmit(recordedSampleIdentifier);
            }

            DataContext.SubmitChanges();
        }


        public static bool InsertResults(DataService.Result results, int materialId)
        {
            string requestID = string.Empty;
            int i = 0;
            onemoretry:
            try
            {
                bool isSuccess;
                requestID = string.IsNullOrEmpty(results.RequestId) ? string.Empty : results.RequestId;

                using (var transaction = new TransactionScope())
                {
                    var resultHeader = new ResultHeader
                                           {
                                               Id = Guid.NewGuid(),
                                               RequestId = results.RequestId,
                                               TimeStamp = results.TimeStamp,
                                               LocationName = results.LocationName,
                                               CompanyName = results.CompanyName,
                                               UserName = results.Username,
                                               SpectSN = results.SpectrometerSerialNumber,
                                               TestStatusDesc = results.TestStatus,
                                               PresDesc = results.PresentationName,
                                               MaterialDesc = results.MaterialName,
                                               CategoryDesc = results.CategoryName,
                                               SCDesc = results.SubCategoryName,
                                               SampleClass = results.SampleClassName,
                                               AnalysisId = results.AnalysisId,
                                               SampleId = results.SampleId,
                                               LocalTimeStamp = results.LocalTimeStamp
                                           };

                    DataContext.ResultHeaders.InsertOnSubmit(resultHeader);

                    foreach (var detail in results.Details)
                    {
                        var resultDetail = new ResultDetail
                                               {
                                                   Id = Guid.NewGuid(),
                                                   RequestId = results.RequestId,
                                                   ModelGroupName = detail.TraitName,
                                                   QuantFileVersion = detail.QuantFileVersion,
                                                   UpperBRTestValue = detail.UpperBusinessRuleTestValue,
                                                   LowerBRTestValue = detail.LowerBusinessRuleTestValue,
                                                   TestStatusDesc = detail.TestStatus,
                                                   BusRuleText = detail.BusinessRuleText,
                                                   ReqOrder = detail.RequestOrder,
                                                   DisplayText = detail.DisplayText
                                               };

                        DataContext.ResultDetails.InsertOnSubmit(resultDetail);
                    }

                    // delete any previous recorded sample identifiers for specified material
                    var lastRecordedSI = DataContext.RecordedSampleIdentifiers.Where(r => r.MaterialId == materialId);
                    DataContext.RecordedSampleIdentifiers.DeleteAllOnSubmit(lastRecordedSI);

                    foreach (var identifier in results.Identifiers)
                    {
                        var resultIdentifier = new ResultIdentifier
                                                   {
                                                       Id = Guid.NewGuid(),
                                                       RequestId = results.RequestId,
                                                       Description = identifier.Attribute,
                                                       AttribValue = identifier.Value
                                                   };

                        DataContext.ResultIdentifiers.InsertOnSubmit(resultIdentifier);

                        var recordedSampleIdentifier = new RecordedSampleIdentifier
                                                           {
                                                               Id = Guid.NewGuid(),
                                                               MaterialId = materialId,
                                                               Name = identifier.Attribute,
                                                               Value = identifier.Value
                                                           };

                        DataContext.RecordedSampleIdentifiers.InsertOnSubmit(recordedSampleIdentifier);
                    }

                    DataContext.SubmitChanges();
                    transaction.Complete();
                    isSuccess = true;
                }

                return isSuccess;
            }
            catch (SqlCeException se)
            {
                Helper.LogError("InsertResults", requestID, se, false);
                i++;
                if (i == 1)
                    goto onemoretry;
                return false;
            }
            catch (Exception e)
            {
                Helper.LogError("InsertResults", requestID, e, false);
                return false;
            }
        }


        public static ResultHeader GetResultHeader(string requestId)
        {
            return (from s in DataContext.ResultHeaders
                    where s.RequestId == requestId
                    select s).FirstOrDefault();
        }


        public static List<ResultIdentifier> GetResultIdentifiers(string requestId)
        {
            return (from s in DataContext.ResultIdentifiers
                    where s.RequestId == requestId
                    select s).ToList();
        }


        public static List<ResultDetail> GetResultDetails(string requestId)
        {
            return (from s in DataContext.ResultDetails
                    where s.RequestId == requestId
                    orderby s.ReqOrder
                    select s).ToList();
        }

        public static List<ResultDetail> GetAllResultDetails()
        {
            return (from s in DataContext.ResultDetails
                    orderby s.ReqOrder
                    select s).ToList();
        }

        public static void CheckSpeedMode()
        {
            Trace.WriteLine("In CheckSpeedMode()..");
            using (var conn = new SqlCeConnection(Helper.CONNECTION_STRING))
            {
                try
                {
                    conn.Open();
                    var cmdce = new SqlCeCommand(
                        "SELECT count(*) as cnt  FROM Information_SCHEMA.columns WHERE (Table_name = 'AdminPreferences') AND (column_name = 'SpeedMode')",
                        conn);

                    //Trace.WriteLine("In CheckSpeedMode()..Before: cmdce.ExecuteScalar()");
                    int recCnt = int.Parse(cmdce.ExecuteScalar().ToString());
                    //Trace.WriteLine("In CheckSpeedMode()..After:  cmdce.ExecuteScalar()");
                    if (recCnt == 0)
                    {
                        string sql = "ALTER TABLE AdminPreferences ADD SpeedMode bit NOT NULL DEFAULT 0, SpeedModeDualScan bit NOT NULL DEFAULT 0";
                        cmdce.CommandText = sql;
                        cmdce.ExecuteNonQuery();
                    }
                }
                catch (SqlCeException se)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    Helper.LogError("AddSpeedMode", string.Empty, se, true);
                }
                catch (Exception ex)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    Helper.LogError("AddSpeedMode", string.Empty, ex, true);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }


        public static void PurgeResultTables()
        {
            Trace.WriteLine("Enter: PurgeResultTables()");
            using (var conn = new SqlCeConnection(Helper.CONNECTION_STRING))
            {
                try
                {
                    string resultHeaderPurgeLimit = ConfigurationManager.AppSettings["ResultHeaderPurgeLimit"];
                    string resultHeaderReturnLimit = ConfigurationManager.AppSettings["ResultHeaderReturnLimit"];

                    conn.Open();
                    var cmdce = new SqlCeCommand("SELECT COUNT(*) AS RecCnt FROM ResultHeader", conn);
                    int recCnt = int.Parse(cmdce.ExecuteScalar().ToString());
                    if (recCnt >= int.Parse(resultHeaderPurgeLimit))
                    {
                        cmdce.CommandText = "SELECT MIN(CAST(RequestId AS int)) AS MinRequestID FROM (SELECT TOP (" +
                                            resultHeaderReturnLimit +
                                            ") LocalTimeStamp, RequestId FROM ResultHeader ORDER BY LocalTimeStamp DESC) AS derivedtbl_1";

                        int requestID = int.Parse(cmdce.ExecuteScalar().ToString());

                        cmdce.CommandText = "Delete From ResultHeader Where (Cast(RequestID as int) < " + requestID + ")";
                        cmdce.ExecuteNonQuery();

                        cmdce.CommandText = "Delete From ResultDetail Where (Cast(RequestID as int) < " + requestID + ")";
                        cmdce.ExecuteNonQuery();

                        cmdce.CommandText = "Delete From ResultIdentifiers Where (Cast(RequestID as int) < " + requestID + ")";
                        cmdce.ExecuteNonQuery();
                        Trace.WriteLine("Deletes complete.");
                    }
                }
                catch (SqlCeException se)
                {
                    Trace.WriteLine("Exception: PurgeResultTables()");
                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    Helper.LogError("PurgeResultTables", string.Empty, se, true);
                }
                finally
                {
                    Trace.WriteLine("PurgeResultTables() - Finally Block.");
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }


        public static List<ResultHeader> GetAllResultHeader()
        {
            try
            {
                PurgeResultTables();
                return (from s in DataContext.ResultHeaders
                        orderby s.TimeStamp descending
                        select s).Take(250).ToList();
            }
            catch (SqlCeException se)
            {
                Helper.LogError("GetAllResultHeader", string.Empty, se, true);
                return new List<ResultHeader>();
            }
        }

        public static void GetAllGraphHeader(String material)
        {
            try
            {
                PurgeResultTables();
                requestIDs = new List<string>();

                var rHeader = (from s in DataContext.ResultHeaders
                               where s.MaterialDesc == material
                               orderby s.TimeStamp descending
                               select s).Take(250).ToList();
                foreach (var h in rHeader)
                {
                    requestIDs.Add(h.RequestId);
                }
            }
            catch (SqlCeException se)
            {
                Helper.LogError("GetAllResultHeader", string.Empty, se, true);
            }
        }

        public static List<String> GetDistinctMaterials()
        {
            try
            {
                List<ResultHeader> rHeader = new List<ResultHeader>();
                rHeader = GetAllResultHeader();
                materialList = new List<String>();
                requestIDs = new List<String>();

                var materials = (from s in rHeader
                                 select s.MaterialDesc).Distinct();
                foreach (var material in materials)
                {
                    materialList.Add(material);
                    var requests = (from s in rHeader
                                    where s.MaterialDesc == material
                                    select s.RequestId);

                    foreach (var r in requests)
                    {
                        requestIDs.Add(r);
                    }
                }

                return materialList;
            }
            catch (SqlCeException se)
            {
                Helper.LogError("GetAllResultHeader", string.Empty, se, true);
                return new List<String>();
            }
        }

        public static List<String> GetDistinctTraits()
        {
            try
            {
                List<String> traitList = new List<String>();
                var rDetails = DAL.GetAllResultDetails();
                int count = 0;
                foreach (var rID in requestIDs)
                {
                    rDetails = GetResultDetails(rID);
                    var traits = (from s in rDetails
                                  where s.RequestId == requestIDs[count]
                                  select s.ModelGroupName).Distinct();
                    foreach (var t in traits)
                    {
                        traitList.Add(t);
                    }
                }
                return traitList;
            }
            catch (SqlCeException se)
            {
                Helper.LogError("GetDistinctTraits", string.Empty, se, true);
                return new List<String>();
            }
        }

        public static CacheSample GetCachedSample(string sampleType)
        {
            return (from s in DataContext.CacheSamples
                    where s.SampleType == sampleType
                    select s).FirstOrDefault();
        }


        public static List<CacheSampleIdentifier> GetCachedSampleIdentifiers()
        {
            return (from s in DataContext.CacheSampleIdentifiers
                    select s).ToList();
        }


        public static string GetMaterialCode(int materialId)
        {
            return (from t in DataContext.Traits
                    where t.MaterialId == materialId
                    select t.MaterialCode).FirstOrDefault();
        }


        public static List<string> GetUniqueCheckSamples()
        {
            return (from s in DataContext.SampleTypes
                    where s.SampleTypeName.ToLower() == Helper.SAMPLE_CLASS_CHECK
                    select s.SampleId).Distinct().ToList();
        }
    }
}
