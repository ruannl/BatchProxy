using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace BatchProxy.Controllers
{
    public class ServiceController : Controller
    {
        public JsonResult GetDirectoryInfo(string path)
        {
            if (!Directory.Exists(path))
                return Json(null, JsonRequestBehavior.AllowGet);

            var di = new DirectoryInfo(path);
            var directories = di.GetDirectories()
                                .Where(x => (x.Attributes & FileAttributes.Directory) != 0)
                                .Select(x => x.Name)
                                .ToList();

            var filesInFolder = Directory.GetFiles(path).ToList();
            var files = new List<string>();
            foreach (var file in filesInFolder)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    files.Add(fileInfo.Name);
                }
                catch (FileNotFoundException e)
                {
                    continue;
                }
            }

            return Json(new
            {
                directories,
                files
            }, JsonRequestBehavior.AllowGet);
        }
        
    }
}