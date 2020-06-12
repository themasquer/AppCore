using System.IO;

namespace AppCore.Business.Concretes.Models.File
{
    public class FileDownload
    {
        public FileStream FileStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
