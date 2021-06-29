using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InstrumentUI_ATK.DataAccess;
using InstrumentUI_ATK.Common;
using DataService = InstrumentUI_ATK.DataService;
using System.IO;
using System.Transactions;

namespace InstrumentUI_ATK.FormControls
{
    public partial class ConsoleStartUp : UserControl, IFormControl
    {
        public event EventHandler OnConsoleStartUpCompleted;

        private bool _isFailed = false; // flag to identify any exception or error
        public List<Trait> Traits { get; set; }
        public SpectrometerType SpectType { get; set; }
        public List<SampleClass> SampleClasses { get; set; }
        public DataService.EnumHelpCode HelpCode { get { return DataService.EnumHelpCode.INSTRUMENT_CONSOLE; } }

        public ConsoleStartUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ConsoleStartUp control load event. Localize all texts on the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleStartUp_Load(object sender, EventArgs e)
        {
            LocalizeResource();
        }

        public void StartUp(int userId)
        {
            bgWorkerStartUp.RunWorkerAsync(userId);
        }

        /// <summary>
        /// Store all required data into the memory from the local db cache.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerStartUp_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SyncConfiguration((int)e.Argument);
                SystemStartUp();

                //pause the screen for 2 seconds
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Helper.LogError("ConsoleStartUp.bgWorkerStartUp_DoWork", string.Empty, ex, false);
            }
        }

        /// <summary>
        /// Raise the OnConsoleStartUpCompleted event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerStartUp_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.OnConsoleStartUpCompleted != null && _isFailed == false)
                this.OnConsoleStartUpCompleted(this, new EventArgs());
        }

        /// <summary>
        /// Updates the user interface with current status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorkerStartUp_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0) // Some error occurred
            {
                _isFailed = true;
                Helper.DisplayError(e.UserState.ToString(), true);
            }
            else
            {
                lblMessage.Text = e.UserState.ToString();
            }
        }

        private void SyncConfiguration(int userId)
        {
            DataService.InstrumentServiceClient serviceClient = null;
            bool isServiceClosed = false;
            bool isCacheUpdated = false;

            try
            {
                DataService.SyncUser syncResult = null;

                serviceClient = Helper.GetServiceInstance();

                // Get all the latest data from the service
                syncResult = serviceClient.GetSyncData(userId);

                serviceClient.Close();
                isServiceClosed = true;

                // check flag, whether Sync is required or the data is already updated
                if (syncResult.FullSync == InstrumentUI_ATK.DataService.EnumBool.YES)
                {
                    //string connectionString = ConfigurationManager.ConnectionStrings["InstrumentUIConnectionString"].ConnectionString;
                    using (InstrumentUIDataContext dataContext = new InstrumentUIDataContext(Helper.CONNECTION_STRING))
                    {
                        using (var transaction = new TransactionScope())
                        {
                            bgWorkerStartUp.ReportProgress(80, ResourceHelper.Sync_Status_2);

                            try
                            {
                                // Clear all tables of the local cache
                                dataContext.ExecuteCommand("DELETE Users");
                                dataContext.ExecuteCommand("DELETE MacroFiles");
                                dataContext.ExecuteCommand("DELETE SampleClasses");
                                dataContext.ExecuteCommand("DELETE SampleIdentifiers");
                                dataContext.ExecuteCommand("DELETE SampleTypes");
                                dataContext.ExecuteCommand("DELETE SpectrometerTypes");
                                dataContext.ExecuteCommand("DELETE Traits");
                                dataContext.ExecuteCommand("DELETE CacheSample");
                                dataContext.ExecuteCommand("DELETE CacheSampleIdentifiers");
                            }
                            catch (Exception)
                            {
                                bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error_10218); // delete old record failed
                                throw;
                            }

                            // Sync Spectrometer Type info
                            SyncSpectrometerType(syncResult, dataContext);
                            try
                            {
                                // Sync User info
                                User newUser = new User();
                                newUser.Id = syncResult.Id;
                                newUser.UserName = Helper.CurrentUser.UserName;
                                newUser.FirstName = syncResult.FirstName;
                                newUser.LastName = syncResult.LastName;
                                newUser.WarmParam = syncResult.WarmParam;
                                newUser.CleanParam = syncResult.CleanParam;
                                newUser.BackgroundTTL = syncResult.BackgroundTTL;
                                newUser.LCode = syncResult.LCode;
                                newUser.LCode = "en";

                                if (syncResult.Company != null)
                                {
                                    newUser.CompanyId = syncResult.Company.CompanyId;
                                    newUser.CompanyName = syncResult.Company.CompanyName;
                                }

                                if (syncResult.Location != null)
                                {
                                    newUser.LocationId = syncResult.Location.LocationId;
                                    newUser.LocationName = syncResult.Location.LocationName;
                                }

                                dataContext.Users.InsertOnSubmit(newUser);
                                dataContext.SubmitChanges();
                            }
                            catch (Exception)
                            {
                                bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error_10212);// User info failed
                                throw;
                            }

                            // Sync Traits
                            SyncTraits(syncResult, dataContext);

                            // Synce Sample Classes info
                            foreach (var sampleClass in syncResult.UserAccessibleSampleClasses)
                            {
                                SampleClass newSampleClass = new SampleClass()
                                {
                                    Id = sampleClass.Id,
                                    Name = sampleClass.SampleClassName
                                };

                                dataContext.SampleClasses.InsertOnSubmit(newSampleClass);
                            }

                            dataContext.SubmitChanges();
                            System.Diagnostics.Trace.WriteLine("Before - dataContext.SubmitChanges()");
                            dataContext.SubmitChanges();
                            System.Diagnostics.Trace.WriteLine("After - dataContext.SubmitChanges()");

                            // Sync Sample Identifiers info
                            SyncSampleIdentifiers(syncResult, dataContext);

                            // Sync Sample Types info
                            foreach (var sampleType in syncResult.UserAccessibleSampleTypes)
                            {
                                SampleType newSampleType = new SampleType()
                                {
                                    Id = sampleType.Id,
                                    SampleId = sampleType.SampleId,
                                    SampleTypeName = sampleType.SampleTypeName,
                                    TraitName = sampleType.TraitName,
                                    LCL = sampleType.LCL,
                                    UCL = sampleType.UCL
                                };

                                dataContext.SampleTypes.InsertOnSubmit(newSampleType);
                            }

                            dataContext.SubmitChanges();

                            transaction.Complete();
                            isCacheUpdated = true;
                        }
                    }

                    // Update the flag in the database to indicate that the application has the latest data
                    if (isCacheUpdated)
                    {
                        serviceClient = Helper.GetServiceInstance();

                        //serviceClient.UpdateSyncStatus(userId);

                        serviceClient.Close();
                        isServiceClosed = true;
                    }
                }
                else
                {
                    //pause the screen for 2 seconds if the data is updated
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception)
            {
                if (isCacheUpdated)
                    bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error);
                else
                    bgWorkerStartUp.ReportProgress(0, ResourceHelper.Sync_Error);

                throw;
            }
            finally
            {
                if (!isServiceClosed)
                    serviceClient.Abort();

                if (Helper.ContextScope != null)
                    Helper.ContextScope.Dispose();
            }
        }

        private void SystemStartUp()
        {
            try
            {
                this.Traits = DAL.GetAllTrait().ToList();
                this.SampleClasses = DAL.GetSampleClasses().ToList();

                SpectrometerType spectrometerType = DAL.GetSpectrometerType();

                this.SpectType = spectrometerType;

                InstrumentUI_ATK.DataService.SpectrometerTypeDetail spectrometerTypeDetail = new InstrumentUI_ATK.DataService.SpectrometerTypeDetail();
                spectrometerTypeDetail.SpectrometerTypeId = spectrometerType.Id;
                spectrometerTypeDetail.SpectrometerTypeName = spectrometerType.Name;
                spectrometerTypeDetail.SerialNumber = spectrometerType.SerialNumber;

                if (spectrometerType.WFFileId.HasValue)
                {
                    spectrometerTypeDetail.WorkflowFile = new InstrumentUI_ATK.DataService.BinaryFile()
                    {
                        Id = spectrometerType.WFFileId.Value,
                        Name = spectrometerType.WFFileName
                    };
                }

                if (spectrometerType.StdXPMFileId.HasValue)
                {
                    spectrometerTypeDetail.BackgroundFile = new InstrumentUI_ATK.DataService.BinaryFile()
                    {
                        Id = spectrometerType.StdXPMFileId.Value,
                        Name = spectrometerType.StdXPMFileName
                    };
                }

                Helper.CurrentUser.UserAccessibleSpectrometerType = spectrometerTypeDetail;

                User user = DAL.GetUser();

                InstrumentUI_ATK.DataService.Company company = new InstrumentUI_ATK.DataService.Company();
                company.CompanyId = (int)user.CompanyId;
                company.CompanyName = user.CompanyName;

                Helper.CurrentUser.Company = company;
                Helper.CurrentUser.BackgroundTTL = (int)user.BackgroundTTL;
                Helper.CurrentUser.CleanParam = (decimal)user.CleanParam;
                Helper.CurrentUser.WarmParam = (decimal)user.WarmParam;
                //Helper.SupportPath = System.Configuration.ConfigurationManager.AppSettings["SupportPath"];
            }
            catch (Exception ex)
            {
                bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error);
                throw new Exception(Helper.ERROR_INVALID_LOCAL_DATA, ex);
            }
        }

        /// <summary>
        /// Updates the traits in the local cache
        /// </summary>
        /// <param name="syncResult"></param>
        /// <param name="dataContext"></param>
        private void SyncTraits(DataService.SyncUser syncResult, InstrumentUIDataContext dataContext)
        {
            try
            {
                // start inserting new data into the local cache
                foreach (var trait in syncResult.UserAccessibleTraits)
                {
                    // insert into trait table
                    Trait newTrait = new Trait();
                    newTrait.Id = trait.Id;
                    newTrait.Name = trait.Name;

                    if (trait.Material != null)
                    {
                        newTrait.MaterialId = trait.Material.MaterialId;
                        newTrait.MaterialName = trait.Material.MaterialName;
                        newTrait.MaterialCode = trait.Material.MaterialCode;
                    }

                    if (trait.Category != null)
                    {
                        newTrait.CategoryId = trait.Category.CategoryId;
                        newTrait.CategoryName = trait.Category.CategoryName;
                    }

                    if (trait.SubCategory != null)
                    {
                        newTrait.SubcategoryId = trait.SubCategory.SubCategoryId;
                        newTrait.SubcategoryName = trait.SubCategory.SubCategoryName;
                    }

                    if (trait.Presentation != null)
                    {
                        newTrait.PresentationId = trait.Presentation.PresentationId;
                        newTrait.PresentationName = trait.Presentation.PresentationName;
                    }

                    newTrait.HomogeneityCheck = trait.HomogeneityCheck == InstrumentUI_ATK.DataService.EnumBool.YES ? true : false;
                    newTrait.MGOrder = trait.DisplayOrder;

                    if (trait.ModelGroupStage != null)
                    {
                        newTrait.ModelGroupStageId = trait.ModelGroupStage.ModelGroupStageId;
                        newTrait.ModelGroupStageName = trait.ModelGroupStage.ModelGroupStageName;
                    }

                    if (trait.XpmFile != null)
                    {
                        newTrait.XPMFileId = trait.XpmFile.Id;
                        newTrait.XPMFileName = trait.XpmFile.Name;
                        SaveFile(trait.XpmFile.Data, trait.XpmFile.Name, Helper.FOLDER_PATH_FILES);

                        // Copy(duplicate) the file to the MACRO_FOLDER_PATH also
                        SaveFile(trait.XpmFile.Data, trait.XpmFile.Name, Helper.MACRO_FOLDER_PATH);
                    }

                    dataContext.Traits.InsertOnSubmit(newTrait);
                    dataContext.SubmitChanges();
                }
            }
            catch (Exception)
            {
                bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error_10215);// trait failed
                throw;
            }
        }

        /// <summary>
        /// Updates the Sample Identifiers in the local cache
        /// </summary>
        /// <param name="syncResult"></param>
        /// <param name="dataContext"></param>
        private void SyncSampleIdentifiers(DataService.SyncUser syncResult, InstrumentUIDataContext dataContext)
        {
            try
            {
                foreach (var sampleIdentifier in syncResult.UserAccessibleSampleIdentifiers)
                {
                    int materialId = sampleIdentifier.MaterialId;

                    // Save all sample identifiers
                    foreach (var attribute in sampleIdentifier.SampleIdentifiers)
                    {
                        SampleIdentifier newSampleIdentifier = new SampleIdentifier();
                        newSampleIdentifier.MaterialId = materialId;
                        newSampleIdentifier.Id = attribute.Id;
                        newSampleIdentifier.Name = attribute.Name;
                        newSampleIdentifier.Description = attribute.Description;
                        newSampleIdentifier.DisplayOrder = attribute.DisplayOrder;
                        newSampleIdentifier.Required = attribute.IsRequired;
                        newSampleIdentifier.Numeric = attribute.IsNumeric;

                        if (attribute.IsDropDown)
                            newSampleIdentifier.MultiValues = attribute.DefaultValue;
                        else
                            newSampleIdentifier.Value = attribute.DefaultValue;

                        newSampleIdentifier.DropDown = attribute.IsDropDown;

                        dataContext.SampleIdentifiers.InsertOnSubmit(newSampleIdentifier);
                    }

                    dataContext.SubmitChanges();
                }
            }
            catch (Exception)
            {
                bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error_10216); // sample identifier failed
                throw;
            }
        }

        /// <summary>
        /// Updates the Spectrometer details in the local cache
        /// </summary>
        /// <param name="syncResult"></param>
        /// <param name="dataContext"></param>
        private void SyncSpectrometerType(DataService.SyncUser syncResult, InstrumentUIDataContext dataContext)
        {
            if (syncResult.UserAccessibleSpectrometerType != null)
            {
                try
                {
                    DataService.SpectrometerTypeDetail spectrometerType = syncResult.UserAccessibleSpectrometerType;

                    SpectrometerType newSpectrometerType = new SpectrometerType();

                    newSpectrometerType.Id = spectrometerType.SpectrometerTypeId;
                    newSpectrometerType.Name = spectrometerType.SpectrometerTypeName;
                    newSpectrometerType.SerialNumber = spectrometerType.SerialNumber;

                    if (spectrometerType.BackgroundFile != null)
                    {
                        newSpectrometerType.StdXPMFileId = spectrometerType.BackgroundFile.Id;
                        newSpectrometerType.StdXPMFileName = spectrometerType.BackgroundFile.Name;
                        SaveFile(spectrometerType.BackgroundFile.Data, spectrometerType.BackgroundFile.Name, Helper.FOLDER_PATH_FILES);

                        // Copy(duplicate) the file to the MACRO_FOLDER_PATH also
                        SaveFile(spectrometerType.BackgroundFile.Data, spectrometerType.BackgroundFile.Name, Helper.MACRO_FOLDER_PATH);
                    }

                    if (spectrometerType.WorkflowFile != null)
                    {
                        newSpectrometerType.WFFileId = spectrometerType.WorkflowFile.Id;
                        newSpectrometerType.WFFileName = spectrometerType.WorkflowFile.Name;
                        SaveFile(spectrometerType.WorkflowFile.Data, spectrometerType.WorkflowFile.Name, Helper.FOLDER_PATH_FILES);
                    }

                    dataContext.SpectrometerTypes.InsertOnSubmit(newSpectrometerType);

                    // Update the micro files
                    foreach (var macroFile in syncResult.UserAccessibleSpectrometerType.Macros)
                    {
                        MacroFile newMacroFile = new MacroFile();
                        newMacroFile.FileId = macroFile.Id;
                        newMacroFile.FileName = macroFile.Name;
                        newMacroFile.SpectTypeId = syncResult.UserAccessibleSpectrometerType.SpectrometerTypeId;

                        SaveFile(macroFile.Data, macroFile.Name, Helper.MACRO_FOLDER_PATH);
                        dataContext.MacroFiles.InsertOnSubmit(newMacroFile);
                    }

                    dataContext.SubmitChanges();
                }
                catch (Exception)
                {
                    bgWorkerStartUp.ReportProgress(0, ResourceHelper.Error_10213); // spectrometer info failed
                    throw;
                }
            }

        }

        /// <summary>
        /// Create/Updates files on the local drives
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="fileName"></param>
        private void SaveFile(byte[] fileData, string fileName, string folderPath)
        {
            if (fileData != null)
            {
                int fileLength = fileData.Length;

                if (fileLength > 0)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    folderPath += fileName;

                    File.WriteAllBytes(folderPath, fileData);
                }
            }
        }

        /// <summary>
        /// This function replace all the display text on the form from the relevent resource file
        /// </summary>
        public void LocalizeResource()
        {
            lblConsoleStartUp.Text = ResourceHelper.Console_Start_Up;
            lblStatus.Text = ResourceHelper.Console_Start_Up_Status;
            lblMessage.Text = ResourceHelper.Sync_Status_1;
        }
    }
}
