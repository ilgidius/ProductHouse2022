namespace Server.Common.Classes.Models.Common
{
    public class EventModel
    {
        public long UserId { get; set; }
        public long Id { get; set; }
        public string? AddedTime { get; set; }
        public string? SentTime { get; set; }
        public string? EventType { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}
