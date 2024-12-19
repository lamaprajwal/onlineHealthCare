using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application.Repositories
{
    public interface IAppoinmentService
    {
        Task AddAsync(string info,
          DateOnly date,
          TimeOnly startTime,
          TimeOnly? endTime,
          string doctorId,
          string patientId);

        Task<bool> CheckAvailability(string doctorId, DateOnly date, TimeOnly timeStart);

    }
}
