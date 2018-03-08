using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUp_Scl_Service.Model
{
    public class OperationRecords_
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Mode { get; set; }
        public string Time { get; set; }
        public Guid RecordId { get; set; }
        public string Detail { get; set; }

    }
}
