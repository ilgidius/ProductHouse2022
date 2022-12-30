using CallRecording.DAL.Models;
using CallRecording.DAL.Repository;
using CallRecording.ViewModels;

namespace CallRecording.Mapping
{
    public class ToEvent
    {
        public static Event CreateEventRequestToUser(CreateEventRequest createEventRequest, long userId)
        {
            return new Event
            {
                UserId = userId,
                AddedTime = createEventRequest.AddedTime.ToString(),
                SentTime = createEventRequest.SentTime.ToString(),
                EventType = createEventRequest.EventType,
                Key = createEventRequest.BussinesData.Key,
                Value = createEventRequest.BussinesData.Value
            };
        }
    }
}
