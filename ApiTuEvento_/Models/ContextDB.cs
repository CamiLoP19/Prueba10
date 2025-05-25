using Microsoft.EntityFrameworkCore;

namespace ApiTuEvento_.Models
{
    public class ContextDB : DbContext
    {
        public ContextDB(DbContextOptions<ContextDB> options) : base(options) 
        { }


        public DbSet<ApiTuEvento_.Models.Persona> personas { get; set; } = default!;
        public DbSet<ApiTuEvento_.Models.Evento> eventos { get; set; } = default!;
        public DbSet<ApiTuEvento_.Models.Carrito> carritos { get; set; } = default!;
        public DbSet<ApiTuEvento_.Models.CategoriaEvento> categoriaEventos { get; set; } = default!;
        public DbSet<ApiTuEvento_.Models.Boleto> boletos { get; set; } = default!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación Boleto - Evento 
            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.evento)
                .WithMany(e => e.Boletos)
                .HasForeignKey(b => b.EventoId)
                .OnDelete(DeleteBehavior.Cascade); // Opcional, para eliminar boletos si evento se elimina

            // Relación Boleto - Persona
            modelBuilder.Entity<Boleto>()
                .HasOne(b => b.persona)
                .WithMany(p => p.boletos)
                .HasForeignKey(b => b.PersonaId)
                .OnDelete(DeleteBehavior.SetNull); // Si quieres que al borrar persona se mantengan boletos

            // Relación Evento - CategoriaEvento
            modelBuilder.Entity<Evento>()
                .HasOne(e => e.CategoriaEvento)
                .WithMany(c => c.Eventos)
                .HasForeignKey(e => e.CategoriaEventoId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Boleto>()
            .Property(b => b.TipoBoleto)
            .HasConversion<string>();

        }

    }
}
