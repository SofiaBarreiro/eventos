using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Organismo")]
    public partial class Organismo
    {
        public Organismo()
        {
            Personas = new HashSet<Persona>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; }
        public int IdPais { get; set; }
        [Required(ErrorMessage = "El campo Correo electrónico es obligatorio")]
        [StringLength(150)]
        public string MailContacto { get; set; }
        [StringLength(50)]
        public string TelefonoContacto { get; set; }
        [StringLength(150)]
        public string TipoOrganismo { get; set; }

        [ForeignKey(nameof(IdPais))]
        [InverseProperty(nameof(Pai.Organismos))]
        public virtual Pai IdPaisNavigation { get; set; }
        [InverseProperty(nameof(Persona.IdOrganismoNavigation))]
        public virtual ICollection<Persona> Personas { get; set; }

        //public string tipo { get; set; }

    }
}
