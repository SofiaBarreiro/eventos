using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosCeremonial.Data
{
    public class SmtpSettings
    {

        public string SenderName { get; set; }
        public string SenderEmail { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Server { get; set; }
        public int Port { get; set; }


    }
}
