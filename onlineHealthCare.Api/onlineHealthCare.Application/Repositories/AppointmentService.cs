using Microsoft.EntityFrameworkCore;
using onlineHealthCare.Database;
using onlineHealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application.Repositories
{
   public class AppointmentService:IAppoinmentService
    {
        private readonly onlineHealthCareDbContext _context;
        public AppointmentService(onlineHealthCareDbContext context)
        {
            _context=context;
        }
        //public async Task AddAsync(string info,
        //    DateOnly date,
        //    TimeOnly startTime,
        //    TimeOnly? endTime,
        //    string doctorId,
        //    string patientId)
        //{
        //    var appointment = new Appointment
        //    {
        //        AppointmentDateTime = date,
        //        TimeStart = startTime,
        //        TimeEnd = startTime.Add(TimeSpan.FromHours(1)),
        //        DoctorId = doctorId,
        //        PatientId = patientId
        //    };

        //    await this._context.Appoinments.AddAsync(appointment);
        //    await this._context.SaveChangesAsync();
        //}

        public async Task AddAsync(string info, DateOnly date, TimeOnly startTime, TimeOnly? endTime, string doctorId, string patientId)
        {
            var appointment = new Appointment
            {
                AppointmentDateTime = date,
                TimeStart = startTime,
                TimeEnd = endTime,
                DoctorId = doctorId,
                PatientId = patientId,
                Status="Scheduled"
            };

            await this._context.Appoinments.AddAsync(appointment);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> CheckAvailability(string doctorId, DateOnly date, TimeOnly timeStart)
           => !await this._context
                   .Appoinments
                   .AnyAsync(a => a.DoctorId == doctorId
                        && a.AppointmentDateTime == date
                        && a.TimeStart <= timeStart&&a.TimeEnd<timeStart);

            

    }
}
