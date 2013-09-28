using System;
using System.Linq;

namespace Medic.ViewModels
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public DateTime Time { get; set; }

        public String PatientName { get; set; }

        public int PatientId { get; set; }

        public int ExaminationId { get; set; }

        public string Hour
        {
            get
            {
                return this.Time.ToString("HH:mm");
            }
        }
    }
}
