using GreenMaps.Areas.Identity.Data;
using GreenMaps.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GreenMaps.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PontoColeta> PontoColeta { get; set; }
        public DbSet<TipoLixo> TipoLixo { get; set; }
        public DbSet<TipoPonto> TipoPonto { get; set; }
    }
}
