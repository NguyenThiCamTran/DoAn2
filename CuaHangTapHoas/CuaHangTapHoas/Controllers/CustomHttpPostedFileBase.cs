using System.IO;
using System.Web;

namespace CuaHangTapHoas.Controllers
{
    internal class CustomHttpPostedFileBase : HttpPostedFileBase
    {
        private MemoryStream stream;
        private string contentType;
        private string fileName;

        public CustomHttpPostedFileBase(MemoryStream stream, string contentType, string fileName)
        {
            this.stream = stream;
            this.contentType = contentType;
            this.fileName = fileName;
        }
    }
}