using onlineHealthCare.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application.Repositories
{
    public interface IUserService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
    }
}
