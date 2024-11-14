using Microsoft.Extensions.DependencyInjection;
using onlineHealthCare.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            @this.AddTransient<IDoctorService, DoctorServics>();
            @this.AddTransient<ILoginService, LoginService>();
            @this.AddTransient<IPatientService, PatientService>();
            return @this;
        }

    }
}
