using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Evento")]
    public partial class Evento
    {
        public Evento()
        {
            Invitacions = new HashSet<Invitacion>();
            Patrocinantes = new HashSet<Patrocinante>();
            //Archivos = new HashSet<Archivo>();

        }
        


        [Required(ErrorMessage = "El campo nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres")]
        public string Nombre { get; set; }
        [StringLength(200, ErrorMessage = "La descripción no puede tener más de 200 caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Formato es obligatorio")]
        [StringLength(50)]
        public string Formato { get; set; }
        public int? IdPlataforma { get; set; }
        [StringLength(250, ErrorMessage = "La Url no puede tener más de 250 caracteres")]
        public string URL { get; set; }
        [StringLength(150, ErrorMessage = "El IdReunionVirtual no puede tener más de 150 caracteres")]
        public string IdReunionVirtual { get; set; }
        public int? IdUbicacion { get; set; }
        [StringLength(150, ErrorMessage = "El Password no puede tener más de 150 caracteres")]
        public string Password { get; set; }
        [StringLength(4000, ErrorMessage = "La Agenda no puede tener más de 4000 caracteres")]
        public string Agenda { get; set; }
        [StringLength(250, ErrorMessage = "La Ruta del programa no puede tener más de 250 caracteres")]
        public string RutaPrograma { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaHoraInicio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaHoraFin { get; set; }
        [StringLength(50, ErrorMessage = "El Tipo de evento no puede tener más de 50 caracteres")]
        public string TipoEvento { get; set; }
        [Range(0, 10000, ErrorMessage = "El cupo no puede ser un número negativo")]
        public int? Cupo { get; set; }
        public int IdEstado { get; set; }
        public Guid GUID { get; set; }
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(IdEstado))]
        [InverseProperty(nameof(Estado.Eventos))]
        public virtual Estado IdEstadoNavigation { get; set; }

        [InverseProperty(nameof(Invitacion.IdEventoNavigation))]
        public virtual ICollection<Invitacion> Invitacions { get; set; }

        [InverseProperty(nameof(Patrocinante.IdEventoNavigation))]
        public virtual ICollection<Patrocinante> Patrocinantes { get; set; }


        //[InverseProperty(nameof(Archivo.IdEventoNavigation))]
        //public virtual ICollection<Archivo> Archivos { get; set; }



    }
}
