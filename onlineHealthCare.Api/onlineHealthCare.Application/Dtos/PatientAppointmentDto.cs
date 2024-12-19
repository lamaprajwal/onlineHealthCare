using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application.Dtos
{
    public class PatientAppointmentDto
    {
        
            public int Id { get; set; }

            public string Info { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateOnly Date { get; set; }

            [Required]
            [DataType(DataType.Time)]
            public TimeOnly TimeStart { get; set; }

            
            [DataType(DataType.Time)]
            public TimeOnly TimeEnd { get; set; }

            [Required]
            public string DoctorId { get; set; }

            
        
    }
}
