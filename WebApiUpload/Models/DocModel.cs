using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUpload.Models
{
    public class DocModel
    {
        public int DocId { get; set; }
        public string TicketReply { get; set; }
        public List<FileModel> Files { get; set; }
    }
}
