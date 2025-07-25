using Ookii.Dialogs.Wpf;
using System.Windows.Forms;

namespace WpfApp1.Services
{
    public class DialogService : IDialogService
    {
        public string selectPathDialog() {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Select a folder";
            dialog.UseDescriptionForTitle = true; // true = uses Description as title on Vista+

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string selectedPath = dialog.SelectedPath;
                return selectedPath;
            }
            else
            {
                return null;
            }
        }

        public string selectFaxFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                return selectedFilePath;
            }
            else
            {
                return "";
            }
        }

    }
}
