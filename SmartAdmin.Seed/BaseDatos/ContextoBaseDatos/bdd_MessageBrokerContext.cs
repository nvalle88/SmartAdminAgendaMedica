using Microsoft.EntityFrameworkCore;
using SmartAdmin.Seed.BaseDatos.ModelosBaseDatos.bdd_MessageBroker;

namespace SmartAdmin.Seed.BaseDatos.ContextoBaseDatos
{
    public partial class bdd_MessageBrokerContext : DbContext
    {
        public virtual DbSet<EncuestaSobre> EncuestaSobre { get; set; }
        public virtual DbSet<ErrorSobre> ErrorSobre { get; set; }
        public virtual DbSet<HistorialEstadoSobre> HistorialEstadoSobre { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<ReconocimientoSobre> ReconocimientoSobre { get; set; }
        public virtual DbSet<Sobre> Sobre { get; set; }

        public bdd_MessageBrokerContext(DbContextOptions<bdd_MessageBrokerContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncuestaSobre>(entity =>
            {
                entity.HasIndex(e => e.IdSobre);

                entity.Property(e => e.CalificacionEncuesta)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.IdSobreNavigation)
                    .WithMany(p => p.EncuestaSobre)
                    .HasForeignKey(d => d.IdSobre)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ErrorSobre>(entity =>
            {
                entity.Property(e => e.ColaPublicacion)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.MensajeError)
                    .IsRequired()
                    .HasMaxLength(1024);
            });

            modelBuilder.Entity<HistorialEstadoSobre>(entity =>
            {
                entity.HasIndex(e => e.IdSobre);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.IdSobreNavigation)
                    .WithMany(p => p.HistorialEstadoSobre)
                    .HasForeignKey(d => d.IdSobre)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Propiedades).HasColumnType("xml");
            });

            modelBuilder.Entity<ReconocimientoSobre>(entity =>
            {
                entity.Property(e => e.FechaEnvioLiquidacion)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.FechaEnvioRepositorio)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.RespuestaLiquidacion).IsUnicode(false);

                entity.HasOne(d => d.IdSobreNavigation)
                    .WithMany(p => p.ReconocimientoSobre)
                    .HasForeignKey(d => d.IdSobre)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Sobre>(entity =>
            {
                entity.HasIndex(e => e.IdSobreArmonix)
                    .HasName("<IdSobreIndex, sysname,>");

                entity.HasIndex(e => e.NumeroSobre)
                    .HasName("<NameofMissingIndex, sysname,>");

                entity.HasIndex(e => new { e.NumeroSobre, e.NumeroTicketZendesk, e.Estado, e.FechaActualizacion, e.FechaCreacion })
                    .HasName("<Name of Missing Index, sysname,>");

                entity.Property(e => e.CategoriaReembolso)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.ComentarioCliente).HasMaxLength(256);

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DiagnosticoCliente)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.DiagnosticoFinalLiquidacion)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Estado).HasMaxLength(128);

                entity.Property(e => e.GastoOncologico).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.ListaFichero).IsRequired();

                entity.Property(e => e.MedicinaContinua).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.NumeroSobre).HasMaxLength(30);

                entity.Property(e => e.Observacion)
                    .IsRequired()
                    .HasMaxLength(800);

                entity.Property(e => e.RespuestaLiquidacionAutomatica).IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UsuarioCreacion)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
