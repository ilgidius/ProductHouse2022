namespace CallRecording
{
    public class MainData
    {
        public DateTime AddedTime { get; set; }
        public DateTime SentTime { get; set; }
        public string EventType { get; set; }
        public List<BusinessData> BussinesData { get; set; } = new List<BusinessData>();
    }
    public class BusinessData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
