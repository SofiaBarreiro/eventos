using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    public partial class Pai
    {
        public Pai()
        {
            Organismos = new HashSet<Organismo>();
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Cultura es obligatorio")]
        [StringLength(10)]
        public string Cultura { get; set; }

        [InverseProperty(nameof(Organismo.IdPaisNavigation))]
        public virtual ICollection<Organismo> Organismos { get; set; }
    }
}
