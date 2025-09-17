using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUpload.Models
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileUri { get; set; }
        public string PhysicalPath { get; set; }
    }
}
