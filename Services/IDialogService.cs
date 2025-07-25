using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public interface IDialogService
    {
        public string selectPathDialog(); 
        public string selectFaxFileDialog();
    }
}
