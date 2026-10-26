using Dominio;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos;

public class AppDbContext : DbContext
{
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Metodo> Metodos { get; set; }
    public DbSet<Atributo> Atributos { get; set; }
    public DbSet<VariableLocal> VariableLocales { get; set; }
    public DbSet<Parametro> Parametros { get; set; }
    public DbSet<TipoDato> TipoDatos { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<Interfaz> Interfaz { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clase>()
            .HasMany(c => c.Atributos)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Clase>()
            .HasMany(c => c.MetodosInvocados)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Clase>()
            .HasOne(c => c.ClaseBase)
            .WithMany()
            .HasForeignKey(c => c.ClaseBaseId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder.Entity<Metodo>()
            .HasOne(m => m.Clase)
            .WithMany(c => c.MetodosInvocados)
            .HasForeignKey("ClaseId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Metodo>()
            .HasMany(m => m.Parametros)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Metodo>()
            .HasMany(m => m.VariablesLocales)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Metodo>()
            .HasMany(m => m.MetodosInvocados)
            .WithMany();

        modelBuilder.Entity<Metodo>()
            .HasOne(m => m.TipoRetorno)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Parametro>()
            .HasOne(p => p.TipoDato)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VariableLocal>()
            .HasOne(v => v.TipoReferencia)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Atributo>()
            .HasOne(a => a.TipoReferencia)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Clase>()
            .HasOne(c => c.Namespace)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Interfaz>()
            .HasMany(i => i.FirmaMetodos)
            .WithMany();

        modelBuilder.Entity<Clase>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Metodo>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<VariableLocal>()
            .HasKey(v => v.Id);

        modelBuilder.Entity<Parametro>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Atributo>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<TipoDato>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<ApiKey>()
            .HasKey(k => k.Id);

        modelBuilder.Entity<Interfaz>()
            .HasKey(i => i.Id);

        modelBuilder.Entity<Namespace>()
            .HasKey(n => n.Id);

        modelBuilder.Entity<TipoDato>().HasData(
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Nombre = "bool" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Nombre = "byte" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Nombre = "sbyte" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000004"), Nombre = "char" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000005"), Nombre = "decimal" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000006"), Nombre = "double" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000007"), Nombre = "float" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000008"), Nombre = "int" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000009"), Nombre = "uint" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000010"), Nombre = "nint" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000011"), Nombre = "nuint" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000012"), Nombre = "long" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000013"), Nombre = "ulong" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000014"), Nombre = "short" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000015"), Nombre = "ushort" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000016"), Nombre = "object" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000017"), Nombre = "string" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000018"), Nombre = "delegate" },
            new TipoDato { Id = Guid.Parse("00000000-0000-0000-0000-000000000019"), Nombre = "dynamic" });

        modelBuilder.Entity<Clase>().HasData(new Clase
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000111"),
            Nombre = "Object",
            EsAbstract = false,
            EsSealed = false,
            ClaseBaseId = (Guid?)null
        });

        modelBuilder.Entity<ApiKey>().HasData(
            new ApiKey { Id = Guid.Parse("9C0FF0B1-4ABD-45C6-8A4A-831748FB7A20") },
            new ApiKey { Id = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-1234567890AB") },
            new ApiKey { Id = Guid.Parse("C0DEFA55-1234-4FED-9CBA-112233445566") },
            new ApiKey { Id = Guid.Parse("F3A8C091-04A9-4D88-9A1C-A0B1C2D3E4F5") },
            new ApiKey { Id = Guid.Parse("00112233-4455-6677-8899-AABBCCDDEEFF") },
            new ApiKey { Id = Guid.Parse("AB12CD34-EF56-7890-1234-56789ABCDEFF") },
            new ApiKey { Id = Guid.Parse("1A2B3C4D-5E6F-7081-92A3-B4C5D6E7F8E9") },
            new ApiKey { Id = Guid.Parse("AABBCCDD-1122-3344-5566-77889900AABB") },
            new ApiKey { Id = Guid.Parse("12345678-90AB-CDEF-1234-567890ABCDEF") },
            new ApiKey { Id = Guid.Parse("102AF4BE-30EA-46EE-ED49-E5D9345E3368") });
    }
}
