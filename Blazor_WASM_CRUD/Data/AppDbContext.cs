using Blazor_WASM_CRUD.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_WASM_CRUD.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
     : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
    }
}
