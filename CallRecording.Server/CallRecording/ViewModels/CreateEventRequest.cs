using System.ComponentModel.DataAnnotations;

namespace CallRecording.ViewModels
{
    public class CreateEventRequest
    {
        //public CreateEventRequest() { }
        [Required(ErrorMessage = "Added time is required.")]
        [DataType(DataType.Date)]
        public DateTime AddedTime { get; set; }
        [Required(ErrorMessage = "Sent time is required.")]
        [DataType(DataType.Date)]
        public DateTime SentTime { get; set; }
        [Required(ErrorMessage = "Event type time is required.")]
        [RegularExpression(@"(INIT|RINGING|START|VOICE|STOP)", ErrorMessage = "Event type does not match possible options.")]
        public string EventType { get; set; } = null!;
        [Required(ErrorMessage = "Bussines data is required.")]
        public BusinessData BussinesData { get; set; } = new BusinessData();
    }
    public class BusinessData
    {
        [Required(ErrorMessage = "Key is required.")]
        [StringLength(100, ErrorMessage = "Key length can't be more than 100.")]
        public string Key { get; set; } = null!;
        [Required(ErrorMessage = "Value is required.")]
        [StringLength(200, ErrorMessage = "Value length can't be more than 200.")]
        public string Value { get; set; } = null!;
    }
}
