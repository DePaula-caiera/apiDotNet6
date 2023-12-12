using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext: DbContext
{
    public DbSet<Produto>? Produtos { get; set; }
    public DbSet<Categoria> Categorias { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options){}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Produto>()
            .Property(p => p.Descricao).HasMaxLength(500).IsRequired(false);
        builder.Entity<Produto>()
            .Property(p => p.Nome).HasMaxLength(120).IsRequired();    
        builder.Entity<Produto>()
            .Property(p => p.Codigo).HasMaxLength(20).IsRequired();
        builder.Entity<Categoria>()
            .ToTable("Categorias");
    }   
}

