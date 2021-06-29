namespace InstrumentUI_ATK.DataAccess.Model
{
    public class ScheduleItem
    {
        public string UserName { get; set; }
        public short LocationNumber { get; set; }
        public bool IsActive { get; set; }
        public int MaterialId { get; set; }
        public string Material { get; set; }
        public string UserField1 { get; set; }
        public string UserField2 { get; set; }
        public short ScansPerHour { get; set; }

        public ScheduleItem()
        {
            IsActive = false;
            MaterialId = 0;
            UserField1 = "";
            UserField2 = "";
            ScansPerHour = 0;
        }
    }
}
