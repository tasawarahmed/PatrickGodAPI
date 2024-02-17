
namespace PatrickGodAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //public DbSet<Character> Characters { get; set; }
        //If you get warning in DataContext constructor you can use is as
        public DbSet<Character> Characters => Set<Character>();
    }
}
