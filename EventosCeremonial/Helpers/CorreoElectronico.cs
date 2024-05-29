using EventosCeremonial.Data;
using System;
using System.Security.Cryptography;
using EventosCeremonial.Helpers;


namespace EventosCeremonial.Helpers
{
    public class CorreoElectronico
    {



        /// <summary>Crea una Url para el codigo QR, compuesta por el Id del evento, Id del Invitado, GUID del evento y nro de QR de invitado</summary>
        /// <param name="evento">The evento.</param>
        /// <param name="invitado">The invitado.</param>
        /// <param name="localhost">The localhost.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static string crearUrlParaQR(Evento evento, Invitacion invitado, string localhost, int IdPersona)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                var root = FuncionesBasicas.getAppSettings();
                var key = root.GetSection("EncryptString")["Key"];
                string encrypted = EncryptHelper.EncryptString(invitado.Qr.ToString(), key);
                string componente = "/pre/";
                string PrioMas = "p/";

                //string url = localhost + componente + evento.GUID.ToString() + "/" + PrioMas + invitado.Id.ToString() + "/" + IdPersona + "/" + encrypted.ToString();


                string url = localhost + componente + PrioMas + invitado.Id.ToString() + "/" + IdPersona.ToString() + "/" + encrypted.ToString();

                return url;

            }
            catch (Exception ex)
            {
                logger.LogError("Error armar url", ex);

            }
            return "";

        }
        public static string crearUrlAlDisp(Evento evento, string localhost)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                
                string url = localhost  + "/evento/disp/" + evento.Id.ToString();
                return url;

            }
            catch (Exception ex)
            {
                logger.LogError("Error armar url", ex);

            }
            return "";

        }
        /// <summary>
        /// Arma el texto para el correo de confirmación en caso de que el invitado haya confirmado su asistencia al evento. Es solo para virtuales o Streaming. Las cadenas de Streaming y Virtual no son iguales.
        /// </summary>
        /// <param name="oEvento">evento.</param>
        public static string textoConfirmacionVritualOStreaming(Evento oEvento, bool esRecordatorio, bool tienePrograma)
        {

            string codigoHTML = "";
           
            string saludo = "<p style=\"background:white\"><span style=\"font-size:11.0pt; font-family:&quot;Arial&quot;,sans-serif;" +
                      "color:black\">Saludos Cordiales,</ span>" +
              "</p>";

            string pathConfirmacion;

            if (esRecordatorio == false)
            {
                 pathConfirmacion = @"wwwroot/Templates/textoConfirmacionVyS1.txt";

            }
            else {
                 pathConfirmacion = @"wwwroot/Templates/textoConfirmacionVyS1Record.txt";

            }




            string primeraParte = FuncionesEmail.obtenerTextoTemplate(pathConfirmacion);

            if (tienePrograma == true)
            {

                string textoPrograma = "<p style=\"background:white\"><span style=\"font-size:11.0pt;font-family:&quot;Arial&quot;,sans-serif;color:black\">Adjunto encontrar&aacute; el programa de la actividad.</ span></p>";

                primeraParte = primeraParte + textoPrograma;

            }


            primeraParte = primeraParte.Replace("@oEvento.Nombre", oEvento.Nombre);
            primeraParte = primeraParte.Replace("@oEvento.FechaHoraInicio", recuperarFecha(oEvento.FechaHoraInicio));
            primeraParte = primeraParte.Replace("@oEvento.HoraInicio", recuperarHora(oEvento.FechaHoraInicio));
            primeraParte = primeraParte.Replace("@oEvento.URL", oEvento.URL);


            if (oEvento.Formato.Contains("Virtual") && oEvento.IdReunionVirtual != null) {
             

                    string cadenaVirtual = "<p style=\"background:white\"><span style=\"font-size:11.0pt; font-family:&quot;Arial&quot;,sans-serif;" +
                      "color:black\">Id de la reuni&oacute;n: " + oEvento.IdReunionVirtual + "</ span>" +
              "</p>< p style =\"background:white\"><span style=\"font-size:11.0pt; font-family:&quot;Arial&quot;,sans-serif;" +
                         "color:black\">Contraseña de ingreso: " + oEvento.Password + "</ span>" +
                 "</p>";

                    codigoHTML = primeraParte + cadenaVirtual + saludo;
            }
            else{

                codigoHTML = primeraParte  + saludo;

            }
           
        
          
            return codigoHTML;
        }

        /// <summary>Cuando crea un usuario con identity se le envia un correo para qe confirme su identidad</summary>
        /// <param name="link">The link.</param>
        public static string textoConfirmacióCorreoIdentity(string link)
        {
            //textoConfirmacionIdentity
            
            string pathConfirmacion = @"wwwroot/Templates/textoConfirmacionIdentity.txt";

            string codigoHTML = FuncionesEmail.obtenerTextoTemplate(pathConfirmacion);

            codigoHTML = codigoHTML.Replace("@link", link );


            return codigoHTML;
        }

        /// <summary>Texto para avisarle al usuario que esta cambiando su contraseña de identity. El link es a donde debe clickear para cambiarla</summary>
        /// <param name="link">The link.</param>
        /// <param name="queryParam">The query parameter.</param>
        public static string recuperarContraseñaIdentity(string link, string queryParam)
        {
            //textoContraseñaIdentity

            string pathContraseña = @"wwwroot/Templates/textoContraseñaIdentity.txt";


            string codigoHTML = FuncionesEmail.obtenerTextoTemplate(pathContraseña);

            codigoHTML = codigoHTML.Replace("@link", link + queryParam);


            return codigoHTML;
        }


        /// <summary>En caso de que el invitado confirme su asistencia al evento se le envia un mail con los datos. Es solo para eventos presenciales</summary>
        public static string textoConfirmacionEventoPresencial(bool tienePrograma, bool esRecordatorio)
        {
            //textoConfirmacionEventoPresencial.txt
            string pathtextoConfP = "";

            if (tienePrograma == true)
            {
                if (esRecordatorio == false)
                {
                    pathtextoConfP = @"wwwroot/Templates/textoConfirmacionEventoPresencialConProg.txt";
                }
                else {
                    pathtextoConfP = @"wwwroot/Templates/textoConfirmacionEventoPresencialConProgYRecor.txt";
                }
            }
            else {
                if (esRecordatorio == false)
                {
                    pathtextoConfP = @"wwwroot/Templates/textoConfirmacionEventoPresencial.txt";
                }
                else {
                    pathtextoConfP = @"wwwroot/Templates/textoConfirmacionEventoPresencialYRecor.txt";

                }
            }
         

            string codigoHTML = FuncionesEmail.obtenerTextoTemplate(pathtextoConfP);

            return codigoHTML;
        }

        /// <summary>Arma el texto para unainvitacion a un evento presencial</summary>
        public static string retornarHTMLTextoInvitacionPresencial(Evento evento)
        {
            //textoInvitacionPresencial.txt

            string pathTemplate = @"wwwroot/Templates/textoInvitacionPresencial.txt";

            string codigoHTML = FuncionesEmail.obtenerTextoTemplate(pathTemplate);

            codigoHTML = codigoHTML.Replace("@eventoNombre", evento.Nombre);

            return codigoHTML;

        }

        public static string retornarBotonConfirmarEmail(string UrldelPre)
        {

            //"<a title= \"Flyer\" href =\"https://" + UrldelPre + "\">Confirmar inscripción al evento</a>";

            //"<button type =\"button\" background-color:#4CAF50;>Confirmar inscripción al evento</ button >";

            //< a href = "https://google.com" class="button">Go to Google</a>

            //string boton = "<button  style=\"background-color:#4CAF50;\" onclick =\"location.href='" + UrldelPre + "'\" type=\"button\">Confirmar inscripcion</button>";

            string boton = "<center><div style=\"padding:10px;background:#add8e6;max-width:fit-content;\" ><h2><a style=\"background-color:blue;color:white;border:#000 1px solid;text-decoration:none;\" title= \"Flyer\" href =\"" + UrldelPre + "\">Confirmar inscripción al evento</a></h2><div><center>";

            //boton = boton + "<a title=\"Flyer\" href =\"https://" + UrldelPre + "\"><button style=\"background-color:#4CAF50;font-size: 12px;\" type =\"button\">Confirmar inscripción al evento </button></a>";

            return boton;

        }



        /// <summary>Arma el texto para unainvitacion a un evento presencial</summary>
        public static string textoCancelacionEvento(Evento evento)
        {
            //textoInvitacionPresencial.txt

            string pathContraseña = @"wwwroot/Templates/textoCancelacionEvento.txt";

            string codigoHTML = FuncionesEmail.obtenerTextoTemplate(pathContraseña);

            codigoHTML = codigoHTML.Replace("@eventoNombre", evento.Nombre );

            return codigoHTML;

        }

        /// <summary>Arma el texto para unainvitacion a un evento virtual o streaming</summary>
        public static string retornarHTMLEventoVirtualOStreaming()
        {
            //textoInvitacionVirtualStreaming.txt

            string pathVirtual = @"wwwroot/Templates/textoInvitacionVirtualStreaming.txt";

            string codigoHTML = FuncionesEmail.obtenerTextoTemplate(pathVirtual);

            return codigoHTML;

        }
       

        /// <summary>Aclara que si el invitado que ingresa al evento es argentino debe ingresar con DNI</summary>
        public static string retornarNotaInvitadoArgentino()
        {

            return "<p style=\"background:white\"><span style=\"font-size:11.0pt; font-family:&quot;Arial&quot;,sans-serif;" +
                  "color:black\">Para el ingreso deber&aacute; presentar su DNI.</ span>" +
          "</p></ div > ";

        }

        /// <summary>Aclara que si el invitado que ingresa al evento es extranjero debe ingresar con el QR enviado adjunto en el correo</summary>
        public static string retornarNotaInvitadoExtranjero()
        {

            return "<p style=\"background:white\"><span style=\"font-size:11.0pt; font-family:&quot;Arial&quot;,sans-serif;" +
              "color:black\"> Para el ingreso deber&aacute; presentar la invitaci&oacute;n adjunta.</ span>" +
                    "</p></ div > ";

        }

        /// <summary>Arma una cadena de texto con la fecha en formato DD-MM-AAAA</summary>
        /// <param name="fechaYhora">The fecha yhora.</param>
        public static string recuperarFecha(DateTime? fechaYhora)
        {
            if (fechaYhora != null)
            {
                DateTime retorno = (DateTime)fechaYhora;
                return retorno.ToShortDateString();
            }
            else {
                return null;
            
            }
           

        }
       
        /// <summary>Arma una cadena de texto con la hora en formato HH:MM hs</summary>
        /// <param name="fechaYhora">The fecha yhora.</param>
        public static string recuperarHora(DateTime? fechaYhora)
        {
            if (fechaYhora != null)
            {
                DateTime retorno = (DateTime)fechaYhora;
                return retorno.ToString("HH:MM") + " hs";
            }
            else
            {
                return null;
            
            }

        }
    }

    

    
}
