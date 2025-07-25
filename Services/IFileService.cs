using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public interface IFileService
    {
        public string  createStorageFolder(string path, string alarmClock);
        public void    cutStorageFolder(string sourcePath, string destinationPath);
        public void    deleteStorageFolder(string path);

    }
}
