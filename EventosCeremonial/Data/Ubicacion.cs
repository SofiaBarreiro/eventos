using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Ubicacion")]
    public partial class Ubicacion
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Domicilio es obligatorio")]
        [StringLength(150)]
        public string Domicilio { get; set; }
        [Required(ErrorMessage = "El campo Provincia es obligatorio")]
        [StringLength(150)]
        public string Provincia { get; set; }
        [Required(ErrorMessage = "El campo Localidad es obligatorio")]
        [StringLength(150)]
        public string Localidad { get; set; }
    }
}
