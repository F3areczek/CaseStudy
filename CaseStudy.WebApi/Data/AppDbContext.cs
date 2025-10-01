using CaseStudy.WebApi.Data.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CaseStudy.WebApi.Data
{
    /// <summary>
    /// Database context for the application, managing the connection to the database and providing access to the Product entities.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Products in databse
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Databse context constructor with options parameter
        /// </summary>
        /// <param name="options">Options for <see cref="AppDbContext"/> e.g. type of databse</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

    }
}
