using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Identity.Interfaces
{
    internal interface IAppIdentityDbContextSeed
    {
        public Task SeedAsync();
    }
}
