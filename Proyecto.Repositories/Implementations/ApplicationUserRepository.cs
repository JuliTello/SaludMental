using Proyecto.Models;
using Proyecto.Persistence;
using Proyecto.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Repositories.Implementations;

public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository
{
    private readonly ProyectoDbContext _db;
    public ApplicationUserRepository(ProyectoDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<ApplicationUser> ObtenerAsync(string id)
    {
        return await _db.ApplicationUser.FindAsync(id);
    }
}

