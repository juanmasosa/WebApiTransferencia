using Microsoft.EntityFrameworkCore;
using WebApiTransferencia.Model;

namespace WebApiTransferencia.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
        }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
    }
}
