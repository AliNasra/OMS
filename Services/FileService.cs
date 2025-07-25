using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp1.Services
{
    public class FileService: IFileService
    {
        public string createStorageFolder(string path,string alarmClock)
        {
            string newWorkingFolderPath = Path.Combine(path, "OMSFiles");
            Directory.CreateDirectory(newWorkingFolderPath);

            string csvPath = Path.Combine(newWorkingFolderPath, "data.csv");
            string txtPath = Path.Combine(newWorkingFolderPath, "log.txt");

            using (StreamWriter writer = new StreamWriter(csvPath))
            {
                writer.WriteLine("faxNo,faxDate,startDate,endDate,obligationText,attendants,responsibleUnits");
 
            }

            using (StreamWriter writer = new StreamWriter(txtPath))
            {
                writer.WriteLine("Alarm Hour : " + alarmClock);
            }
            return newWorkingFolderPath;
        }
        public void cutStorageFolder(string sourcePath,string destinationPath)
        {
            if (Directory.Exists(sourcePath))
            {
                // Check if the destination folder already exists
                if (!Directory.Exists(destinationPath))
                {
                    // Move the folder and its contents
                    Directory.Move(sourcePath, destinationPath);
                }
            }
        }
        public void deleteStorageFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive: true);
            }
        }
    }
}
