using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class Obligation : IComparable<Obligation>
    {
        public int          faxNo            { get; set; }
        public DateTime     faxDate          { get; set; }

        public DateTime     startDate        { get; set; }
        public DateTime     endDate          { get; set; }
        public string       obligationText   { get; set; }
        public string       attendants       { get; set; }
        public string       responsibleUnits { get; set; }
        public string       faxPath          { get; set; }

        public int CompareTo (Obligation other)
        {
            if (other == null) return 1;
            return this.startDate.CompareTo(other.startDate); 
        }
    }
}
