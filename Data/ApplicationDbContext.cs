using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalPro.Models;

namespace FinalPro.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FinalPro.Models.Club> Club { get; set; } = default!;
        public DbSet<FinalPro.Models.Student> Student { get; set; } = default!;
        public DbSet<FinalPro.Models.Contest> Contest { get; set; } = default!;
    }
}
