using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Repositories.Interfaces;

public interface IApplicationUserRepository: IRepositoryBase<ApplicationUser>
{
    Task<ApplicationUser> ObtenerAsync(string id);
}
