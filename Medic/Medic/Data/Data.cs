using Medic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Medic.Data
{
    public static class Data
    { 
        #region Mock data
        private static List<Patient> patients = new List<Patient> 
        {       
            new Patient
            {
                Id = 1,
                FirstName = "Petar",
                LastName = "Petrov",
                Age = 21
            },
            new Patient
            {
                Id = 2,
                FirstName = "Dimo",
                LastName = "Dimtrov",
                Age = 31
            },
            new Patient
            {
                Id = 3,
                FirstName = "Iliqna",
                LastName = "Petrova",
                Age = 21
            },
            new Patient
            {
                Id = 4,
                FirstName = "Kalina",
                LastName = "Georgieva",
                Age = 41
            },
        };

        private static List<Examination> examinations = new List<Examination>() 
        {
            new Examination
            {
                PatientId = 1,
                Date = DateTime.Now.AddHours(4)
            },
            new Examination
            {
                PatientId = 2,
                Date = DateTime.Now.AddHours(3)
            },   
            new Examination
            {
                PatientId = 4,
                Date = DateTime.Now.AddMinutes(20)
            },
            new Examination
            {
                PatientId = 4,
                Date = DateTime.Now.AddHours(3.5)
            },   
            new Examination
            {
                PatientId = 3,
                Date = DateTime.Now.AddDays(1)
            },   
            new Examination
            {
                PatientId = 4,
                Date = DateTime.Now.AddDays(2)
            },   
        };        
        #endregion

        public static IEnumerable<Appointment> GetTodaysAppointments()
        {
            var exams = AllExaminations().Where(e => e.Date.Date == DateTime.Now.Date);
            var appointments = exams.Select(e => new Appointment 
            {
                Time = e.Date,
                PatientName = GetPatientById(e.PatientId).GetFullName,
                PatientId = e.PatientId,
                ExaminationId = e.Id
            }).OrderBy(a => a.Time);

            return appointments; 
        }

        public static IQueryable<Examination> AllExaminations()
        {
            //TODO - get it from file
            return examinations.AsQueryable();
        }

        public static IQueryable<Patient> AllPatients()
        {
            //TODO add logic
            return patients.AsQueryable();
        }

        public static Patient GetPatientById(int id)
        {
            //TODO change id to guid
            return AllPatients().FirstOrDefault(p => p.Id == id);
        }

        public static Examination GetExaminationById(int id)
        {
            //TODO change id to guid
            return AllExaminations().FirstOrDefault(e => e.Id == id);
        }
    }
}
