using System;
using System.Linq;

namespace Medic.ViewModels
{
    public class AppointmentDetailsViewModel : Common.BindableBase
    {
        private Patient patient;
        public Patient Patient
        {
            get
            {
                return this.patient;
            }
            set
            {
                this.patient = value;
                this.OnPropertyChanged();
            }
        }

        private Appointment selectedAppointment;
        public Appointment SelectedAppointment
        {
            get
            {
                return this.selectedAppointment; 
            }
            set
            {
                this.selectedAppointment = value;
                this.OnPropertyChanged("SelectedApointment");
            }
        }

        public void Init()
        {
            this.Patient = Data.Data.GetPatientById(this.SelectedAppointment.PatientId);            
        }
    }
}
