using Microsoft.EntityFrameworkCore;

namespace RestApiCore.Model
{
    public class RestApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }


        // Connection String (Sample)
        // Connect to Server with Username&Pass:     "Data Source=ServerName;Initial Catalog=Databasename;User ID = UserName; Password = Password"
        // Connect to server without Username&Pass:  "Data Source=.;Initial Catalog=RestApi;Integrated Security=SSPI;"
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=w;Initial Catalog=RestApi;Integrated Security=SSPI;");
        }
    }
}
