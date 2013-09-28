using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medic.ViewModels
{
    public class Examination
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime Date { get; set; }

        public string Complaint { get; set; }

        public string Diagnosis { get; set; }

        public string TreatmentResults { get; set; }
    }
}
