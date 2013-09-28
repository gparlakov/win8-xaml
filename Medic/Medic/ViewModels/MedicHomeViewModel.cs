using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Medic.Behaviour;

namespace Medic.ViewModels
{
    public class MedicHomeViewModel : Common.BindableBase
    {
        public MedicHomeViewModel()
        {
            //this.SelectedApointment = new Appointment();
        }

        private string patientsSearch;        
        public string PatientsSearch
        {
            get { return patientsSearch; }
            set 
            { 
                patientsSearch = value;            
                this.OnPropertyChanged();
            }
        }

        private ObservableCollection<Appointment> todaysAppointments;
        public IEnumerable<Appointment> TodaysAppointments
        {
            get 
            {
                if (this.todaysAppointments == null)
                {
                    this.TodaysAppointments = Data.Data.GetTodaysAppointments();
                }
                return todaysAppointments; 
            }
            set 
            {
                if (this.todaysAppointments == null)
                {
                    this.todaysAppointments = new ObservableCollection<Appointment>();
                }
                this.todaysAppointments.Clear();
                foreach (var item in value)
                {
                    this.todaysAppointments.Add(item);
                }
            }
        }
        
        

        //private ICommand selectedAppointmentCommand;
        //public ICommand SelectedAppointmentCommand
        //{
        //    get
        //    {
        //        if (this.selectedAppointmentCommand == null)
        //        {
        //            this.selectedAppointmentCommand = new DelegateCommand<Appointment>(this.HandleSelectedAppointment);
        //        }
        //        return this.selectedAppointmentCommand;
        //    }
        //}
        //private void HandleSelectedAppointment(Appointment parameter)
        //{
        //    this.SelectedApointment = parameter;
        //}
    }
}
