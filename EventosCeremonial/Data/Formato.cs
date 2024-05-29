using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EventosCeremonial.Data
{
    public class Formato
    {
        public Formato()
        {
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; }
        public bool Value { get; set; }
    }
}