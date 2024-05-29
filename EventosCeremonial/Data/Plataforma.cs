using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    [Table("Plataforma")]
    public partial class Plataforma
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; }
        [StringLength(2)]
        public string Activo { get; set; }
    }
}
