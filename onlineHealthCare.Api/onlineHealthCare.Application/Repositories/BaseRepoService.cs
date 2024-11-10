using Microsoft.EntityFrameworkCore;
using onlineHealthCare.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineHealthCare.Application.Repositories
{
    public class BaseRepoService
    {
        public IDbContextFactory<onlineHealthCareDbContext> Context { get; set; }

        public BaseRepoService(IDbContextFactory<onlineHealthCareDbContext> context)
        {
            Context = context;
        }
    }
}
