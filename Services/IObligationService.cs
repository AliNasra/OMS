using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Services
{
    public interface IObligationService
    {
        public void             addObligation(Obligation obligation);
        public void             filterOldObligations();
        public int              findNextObligationsCount();
        public string           modifyObligation(string faxNo, DateTime faxDate, DateTime startDate, DateTime endDate, string obligationText, string attendants, string responsibleUnits, string faxFilePath, Obligation oldObligation);
        public string           deleteObligation(Obligation obligation);
        public string           canAddObligation(string faxNo, DateTime faxDate, DateTime startDate, DateTime endDate, string obligationText, string attendants, string responsibleUnits, string faxFilePath);
        public List<Obligation> fetchObligation(string? faxNo,DateTime? obligationDate, string? obligationContent);
    }
}
