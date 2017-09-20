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
        
        private static void GetJobInfo(string source, string destination)
        {
            if (!Directory.Exists(source)) return;

            var sourceRootFolder = source.Substring(source.LastIndexOf('\\'), source.Length - source.LastIndexOf('\\'));
            destination = destination + sourceRootFolder;

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            var sourceFileSystemEntries = Directory.GetFileSystemEntries(source, "*", SearchOption.AllDirectories)
                                                   .Select(path => path.Replace(source, ""))
                                                   .ToArray();

            var destinationFileSystemEntries = Directory.GetFileSystemEntries(destination, "*", SearchOption.AllDirectories)
                                                        .Select(path => path.Replace(destination, ""))
                                                        .ToArray();

            var newDestinationFiles = sourceFileSystemEntries.Union(destinationFileSystemEntries)
                                                             .Except(sourceFileSystemEntries.Intersect(destinationFileSystemEntries));

            var existingDestinationFiles = sourceFileSystemEntries.Intersect(destinationFileSystemEntries);

            var itemsToCopy = (from item in existingDestinationFiles
                               let sourceFileName = string.Format("{0}{1}", source, item)
                               let destinationFileName = string.Format("{0}{1}", destination, item)
                               let sourceFileInfo = new FileInfo(sourceFileName)
                               let destinationFileInfo = new FileInfo(destinationFileName)
                               where !Equals(sourceFileInfo.Length, destinationFileInfo.Length) || 
                                     !Equals(sourceFileInfo.LastWriteTime, destinationFileInfo.LastWriteTime)
                               select sourceFileInfo.FullName).ToList();

            Console.WriteLine("items to add [{0}]", newDestinationFiles.Count());
            Console.WriteLine("items to copy [{0}]", itemsToCopy.Count());

            foreach (var item in itemsToCopy)
            {
                Console.WriteLine(item);
            }
        }
        
    }
}
