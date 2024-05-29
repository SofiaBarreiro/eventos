using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EventosCeremonial.Data
{
    

    [Table("Archivo")]
    public partial class Archivo
    {
        
        [Key]
        public int Id { get; set; }

        public byte[] Programa { get; set; }

        public byte[] Flyer { get; set; }

        public byte[] Portada { get; set; }

        public int Temporales { get; set; }

        public string NombrePrograma { get; set; }
        public string NombreFlyer { get; set; }
        public string NombrePortada { get; set; }


        //public int IdEvento { get; set; }

        ////public virtual ICollection<Archivo> Archivos { get; set; }


        //[ForeignKey(nameof(IdEvento))]
        //[InverseProperty(nameof(Evento.Archivos))]

        //public virtual Evento IdEventoNavigation { get; set; }



    }
}
