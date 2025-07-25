using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;
using CsvHelper;
using System.Globalization;
using System.IO;
using WpfApp1.Utility;
using System.Windows;

namespace WpfApp1.Services
{
    public class ObligationService:IObligationService
    {
        string dataBaseName = "database.csv";
        public void addObligation (Obligation obligation)
        {
            string filePath = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, dataBaseName);
            var obligations = new List<Obligation>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                obligations = csv.GetRecords<Obligation>().ToList();
                obligations.Add(obligation);

            }
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Obligation>();
                csv.NextRecord();
                csv.WriteRecords(obligations);
            }
        }
        public string canAddObligation(string faxNo, DateTime faxDate, DateTime startDate, DateTime endDate, string obligationText, string attendants,string responsibleUnits, string faxFilePath)
        {
            string prompt = "";
            int parsedFaxNo;
            bool success = int.TryParse(faxNo, out parsedFaxNo);
            if (!success)
            {
                prompt = prompt + "تأكد من أن رقم الفاكس صحيح\n";
            }
            if (faxDate > DateTime.Today.Date)
            {
                prompt = prompt + "تأكد من أن تاريخ الفاكس صحيح\n";
            }

            if (startDate < DateTime.Today.Date)
            {
                prompt = prompt + "لا يمكنك اضافة إلتزام مر تاريخ بدايته\n";
            }
            if (endDate < DateTime.Today.Date || endDate < startDate)
            {
                prompt = prompt + "تأكد من صلاحية تاريخ إنتهاء الإلتزام\n";
            }
            if (obligationText.Trim().Length == 0)
            {
                prompt = prompt + "برجاء إضافة نص الإلتزام\n";
            }
            if (prompt.Length == 0)
            {
                Obligation newObligation       = new Obligation();
                newObligation.faxNo            = parsedFaxNo;
                newObligation.faxDate          = faxDate;
                newObligation.startDate        = startDate;
                newObligation.endDate          = endDate;
                newObligation.obligationText   = obligationText.Trim();
                newObligation.attendants       = attendants.Trim();
                newObligation.responsibleUnits = responsibleUnits.Trim();
                newObligation.faxPath          = faxFilePath.Trim();
                addObligation(newObligation);
            }
            return prompt;
        }

        public string modifyObligation(string faxNo, DateTime faxDate, DateTime startDate, DateTime endDate, string obligationText, string attendants, string responsibleUnits, string faxFilePath,Obligation oldObligation)
        {
            if (faxNo.Trim() == oldObligation.faxNo.ToString() && faxDate == oldObligation.faxDate && startDate == oldObligation.startDate && endDate == oldObligation.endDate && obligationText.Trim() == oldObligation.obligationText && attendants.Trim() == oldObligation.attendants && responsibleUnits.Trim() == oldObligation.responsibleUnits && faxFilePath.Trim() == oldObligation.faxPath)
            {
                return "";
            }
            string filePath = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, dataBaseName);
            var obligations = new List<Obligation>();
            string prompt = canAddObligation(faxNo, faxDate, startDate, endDate, obligationText, attendants, responsibleUnits, faxFilePath);
            if (prompt.Length == 0)
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    obligations = csv.GetRecords<Obligation>().ToList();
                    obligations.RemoveAll(p => p.faxNo == oldObligation.faxNo
                    && p.faxDate          == oldObligation.faxDate
                    && p.startDate        == oldObligation.startDate
                    && p.endDate          == oldObligation.endDate
                    && p.obligationText   == oldObligation.obligationText
                    && p.attendants       == oldObligation.attendants
                    && p.responsibleUnits == oldObligation.responsibleUnits
                    && p.faxPath          == oldObligation.faxPath);
                }
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Obligation>();
                    csv.NextRecord();
                    csv.WriteRecords(obligations);
                }
            }           
            return prompt;
        }
        public string deleteObligation(Obligation obligation)
        {
            try {
                string filePath = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, dataBaseName);
                var obligations = new List<Obligation>();
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    obligations = csv.GetRecords<Obligation>().ToList();
                    obligations.RemoveAll(p => p.faxNo == obligation.faxNo 
                    && p.faxDate == obligation.faxDate 
                    && p.startDate == obligation.startDate 
                    && p.endDate == obligation.endDate 
                    && p.obligationText == obligation.obligationText 
                    && p.attendants == obligation.attendants);

                }
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<Obligation>();
                    csv.NextRecord();
                    csv.WriteRecords(obligations);
                }
                return "";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
        public List<Obligation> fetchObligation(string? faxNo, DateTime? obligationDate, string? obligationContent)
        {
            int parsedFaxNo;
            bool success = int.TryParse(faxNo, out parsedFaxNo);
            string filePath = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, dataBaseName);
            var obligations = new List<Obligation>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                obligations = csv.GetRecords<Obligation>().ToList();

            }
            if (faxNo != null && success == true)
            {
                obligations = obligations.Where(p => p.faxNo == parsedFaxNo).ToList();
            }

            if (obligationDate != null)
            {
                obligations = obligations.Where(p => p.startDate <= obligationDate && p.endDate >= obligationDate).ToList();
            }

            if (obligationContent != null && obligationContent.Trim().Length != 0)
            {
                obligations = obligations.Where(p => p.obligationText.Contains(obligationContent)).ToList();
            }
            obligations = obligations.OrderBy(x=>x.startDate).ToList();
            return obligations;
        }
        public void filterOldObligations()
        {
            string filePath = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, dataBaseName);
            var obligations = new List<Obligation>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                obligations = csv.GetRecords<Obligation>().ToList();

            }
            var validObligations = obligations.Where(p=> p.endDate.Date >= DateTime.Today.Date).ToList();
            using(var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Obligation>();
                csv.NextRecord();
                csv.WriteRecords(validObligations);
            }
        }
        public int findNextObligationsCount()
        {
            string filePath = Path.Combine(AppSettings.getSettings().SettingsStorageFolder, dataBaseName);
            var obligations = new List<Obligation>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                obligations = csv.GetRecords<Obligation>().ToList();

            }
            return obligations.Where(p=>DateTime.Today.AddDays(1).Date == p.startDate).Count();
        }
    }
}
