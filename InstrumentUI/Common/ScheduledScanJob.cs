using System;
using System.Diagnostics;
using InstrumentUI_ATK.DataAccess.Model;
using Quartz;

namespace InstrumentUI_ATK.Common
{
    public class ScheduledScanJob : IJob
    {
        /// <summary>
        /// Adds the Scan Job to the ScanQueue
        /// </summary>
        /// <param name="context">Job Execution Context</param>
        public void Execute(IJobExecutionContext context)
        {
            if (context.MergedJobDataMap.ContainsKey("schedule"))
            {
                var scanJob = (ScheduleItem)context.MergedJobDataMap["schedule"];
                Helper.AddScanJobToScanQueue(scanJob);
            }
        }
    }
}
