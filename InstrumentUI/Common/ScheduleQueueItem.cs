using System;
using InstrumentUI_ATK.Common;

namespace InstrumentUI_ATK.DataAccess.Model
{
    public class ScheduleQueueItem
    {
        public int Id { get; set; }
        public bool IsRunning { get; set; }
        public DateTime ScheduledTime { get; set; }
        public short Location { get; set; }
        public string Material { get { return Sample.MaterialName; } }
        public Sample Sample { get; set; }
    }
}
