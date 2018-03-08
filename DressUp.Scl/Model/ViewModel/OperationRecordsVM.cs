using System;
namespace DressUp.Scl.Model.ViewModel
{
    public class OperationRecordsVM
    {
        public Guid RecordId { get; set; }
        public Guid UserId { get; set; }
        public string RecordingTime { get; set; }
        public string Details { get; set; }
        public string OperationType { get; set; }
    }
}