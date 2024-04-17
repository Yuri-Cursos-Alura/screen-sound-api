using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Banco;
public class ScreenSoundContext: DbContext
{
    public DbSet<Artista> Artistas { get; set; }
    public DbSet<Musica> Musicas { get; set; }
    public DbSet<Genero> Generos { get; set; }

    private string localConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ScreenSoundApi;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public ScreenSoundContext(DbContextOptions<ScreenSoundContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;
        optionsBuilder.UseSqlServer(localConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Inserido por motivos de teste.
        modelBuilder.Entity<Artista>().Navigation(a => a.Musicas).AutoInclude();
        modelBuilder.Entity<Musica>().Navigation(m => m.Artista).AutoInclude();
    }
}
