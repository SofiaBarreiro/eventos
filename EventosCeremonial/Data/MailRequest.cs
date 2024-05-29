using System;
using System.Collections.Generic;


namespace EventosCeremonial.Data
{
    public class MailRequest
    {

       

            public List<Persona> emailPersonas { get; set; }

            public Evento emailEvento { get; set; }


            public int emailNoEnviados { get; set; } = 0;



    }
}
