using DOTNET_RPG.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTNET_RPG.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbcontextOptions) : base(dbcontextOptions)
        {

        }

        public DbSet<Character> Characters { get; set; }
    }
}