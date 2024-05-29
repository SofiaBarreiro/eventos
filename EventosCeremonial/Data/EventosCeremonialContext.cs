using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace EventosCeremonial.Data
{
    public partial class EventosCeremonialContext : DbContext
    {

        private readonly IConfiguration _configuration;

        public EventosCeremonialContext()
        {

        }
        public EventosCeremonialContext(DbContextOptions<EventosCeremonialContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Evento> Eventos { get; set; }
        public virtual DbSet<EventoParticipante> EventoParticipantes { get; set; }
        public virtual DbSet<Invitacion> Invitacions { get; set; }
        public virtual DbSet<Organismo> Organismos { get; set; }
        public virtual DbSet<Pai> Pais { get; set; }
        public virtual DbSet<Patrocinante> Patrocinantes { get; set; }
        public virtual DbSet<Persona> Personas { get; set; }
        public virtual DbSet<Plataforma> Plataformas { get; set; }
        public virtual DbSet<Ubicacion> Ubicacions { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUserss { get; set; }
        public virtual DbSet<Archivo> Archivos { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.



            //}//VERSION ANTERIOR
            //    optionsBuilder.UseSqlServer("Server=S1-DIXX-SQL10;Database=EventosCeremonial;Trusted_Connection=False;User ID=appEventosCeremonial;Password=7ChWBlkw7*uV*");

            //string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //    .SetBasePath(projectPath)
            //    .AddJsonFile("appsettings.json")
            //    .Build();
            LoggerManger logger = new LoggerManger();

            try
            {

                IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
              .AddJsonFile("appsettings.json")
              .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
             

            }
            catch (Exception ex) {

                logger.LogError("error en db context", ex);



            }


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            LoggerManger logger = new LoggerManger();



            modelBuilder.Entity<AspNetUsers>( entity =>
            {


                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.NormalizedEmail).IsUnicode(false);

                entity.Property(e => e.NormalizedUserName).IsUnicode(false);

                entity.Property(e => e.UserName).IsUnicode(false);


            });


            modelBuilder.Entity<Archivo>(entity =>
            {
                entity.Property(e => e.Portada);

                entity.Property(e => e.Flyer);

                entity.Property(e => e.Programa);

                entity.Property(e => e.Temporales);

                entity.Property(e => e.NombreFlyer).IsUnicode(false);

                entity.Property(e => e.NombrePortada).IsUnicode(false);

                entity.Property(e => e.NombrePrograma).IsUnicode(false);

                //entity.Property(e => e.Temporales).IsUnicode(false);
                //entity.HasOne(d => d.IdEventoNavigation)
                //     .WithMany(p => p.Archivos)
                //     .HasForeignKey(d => d.IdEvento)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("FK_Archivo_Evento");

            });

           


            modelBuilder.Entity<Estado>(entity =>
            {
                entity.Property(e => e.Activo).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.Property(e => e.Agenda).IsUnicode(false);

                entity.Property(e => e.IdReunionVirtual).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.RutaPrograma).IsUnicode(false);

                entity.Property(e => e.TipoEvento).IsUnicode(false);

                entity.Property(e => e.URL).IsUnicode(false);

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Eventos)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evento_Estado");
            });

            modelBuilder.Entity<EventoParticipante>(entity =>
            {
                entity.Property(e => e.ConfirmaAsistencia).IsUnicode(false);

                entity.Property(e => e.EstadodeInscripcion).IsUnicode(false);

                entity.HasOne(d => d.IdInvitacionNavigation)
                    .WithMany(p => p.EventoParticipantes)
                    .HasForeignKey(d => d.IdInvitacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventoParticipante_Invitacion");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.EventoParticipantes)
                    .HasForeignKey(d => d.IdPersona)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EventoParticipante_Persona");
            });

            modelBuilder.Entity<Invitacion>(entity =>
            {
                entity.Property(e => e.TipoInvitado).IsUnicode(false);

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.Invitacions)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invitacion_Evento");
            });

           

            modelBuilder.Entity<Organismo>(entity =>
            {
                entity.Property(e => e.MailContacto).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.TelefonoContacto).IsUnicode(false);

                entity.Property(e => e.TipoOrganismo).IsUnicode(false);

                entity.HasOne(d => d.IdPaisNavigation)
                    .WithMany(p => p.Organismos)
                    .HasForeignKey(d => d.IdPais)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Organismo_Pais");
            });

            modelBuilder.Entity<Pai>(entity =>
            {
                entity.Property(e => e.Cultura).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<Patrocinante>(entity =>
            {
                entity.Property(e => e.AreaInterna).IsUnicode(false);

                entity.HasOne(d => d.IdEventoNavigation)
                    .WithMany(p => p.Patrocinantes)
                    .HasForeignKey(d => d.IdEvento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Patrocinante_Evento");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.Property(e => e.Apellido).IsUnicode(false);

                entity.Property(e => e.MailContacto).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);

                entity.Property(e => e.NumeroDocumento).IsUnicode(false);

                entity.Property(e => e.Puesto).IsUnicode(false);

                entity.Property(e => e.TelefonoContacto).IsUnicode(false);

                entity.Property(e => e.TipoDocumento).IsUnicode(false);

                entity.HasOne(d => d.IdOrganismoNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.IdOrganismo)
                    .HasConstraintName("FK_Persona_Organismo");
            });

            modelBuilder.Entity<Plataforma>(entity =>
            {
                entity.Property(e => e.Activo).IsUnicode(false);

                entity.Property(e => e.Nombre).IsUnicode(false);
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.Property(e => e.Domicilio).IsUnicode(false);

                entity.Property(e => e.Localidad).IsUnicode(false);

                entity.Property(e => e.Provincia).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
