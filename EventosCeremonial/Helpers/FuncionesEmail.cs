using EventosCeremonial.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MimeKit;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Extensions.Configuration;
using System.Text;
using MimeKit.Text;
using Bytescout.BarCode;

namespace EventosCeremonial.Helpers
{
    public class FuncionesEmail
    {

        /// <summary>Crear the cuerpo mensaje confirmacion
        /// Verifica si se trata de un invitado argentino o extranjero
        /// Si es argentino solo manda el flyer y el programa
        /// Si es extranjero manda flyer, programa e invitacion si se trata de un evento presencial
        /// llama a los htmls de los correos
        /// arma el body del emial
        /// adjunta los archivos
        /// Devuelve el correo electronico armado con los adjuntos
        /// </summary>
        /// <param name="oEvento">The evento.</param>
        /// <param name="localhost">Url del sitio</param>
        /// <param name="ubicacion">Lugar en donde se hace el evento</param>
        /// <param name="esExtranjero">if set to <c>true</c> [es extranjero].</param>
        /// <param name="idInvitacion">Id del invitado</param>
        /// <param name="nombreFlyer">El nombre del archivo flyers</param>
        public static MimeMessage CrearCuerpoMensajeConfirmacion(Evento oEvento, string localhost, string ubicacion, bool esExtranjero, int idInvitacion, string nombreFlyer, bool esRecordatorio)
        {
            var root = FuncionesBasicas.getAppSettings();
            var mailMessage = new MimeMessage();

           

            //mailMessage.From.Add(new MailboxAddress(Encoding.UTF8, root.GetSection("SmtpClient")["UserName"], root.GetSection("SmtpClient")["Address"]));

            mailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "Ceremonial y Relaciones Institucionales", root.GetSection("SmtpClient")["Address"]));


            mailMessage.Subject = "Asistencia aceptada al evento: " + oEvento.Nombre;
            var bodyBuilder = new BodyBuilder();
            LoggerManger logger = new LoggerManger();


            if (oEvento.Formato.Contains("Streaming") || oEvento.Formato.Contains("Virtual"))
            {
                if (oEvento.RutaPrograma != null && oEvento.RutaPrograma != "")
                {
                    bodyBuilder.HtmlBody = CorreoElectronico.textoConfirmacionVritualOStreaming(oEvento, esRecordatorio, true);

                }
                else
                {

                    bodyBuilder.HtmlBody = CorreoElectronico.textoConfirmacionVritualOStreaming(oEvento, esRecordatorio, false);

                }
            }
            else
            {
                if (oEvento.RutaPrograma != null && oEvento.RutaPrograma != "")
                {
                    bodyBuilder.HtmlBody = CorreoElectronico.textoConfirmacionEventoPresencial(true, esRecordatorio);

                }
                else
                {
                    bodyBuilder.HtmlBody = CorreoElectronico.textoConfirmacionEventoPresencial(false, esRecordatorio);

                }
                if (esExtranjero == true)
                {
                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + CorreoElectronico.retornarNotaInvitadoExtranjero();
                }
                else
                {
                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + CorreoElectronico.retornarNotaInvitadoArgentino();
                }
            }

            var multipart = new Multipart("mixed");
            try
            {

                MemoryStream ms4 = new MemoryStream();
                LinkedResource flyer = null;
                string localwwwroot = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Otros/" + nombreFlyer;

                if (oEvento.RutaPrograma != null)
                {

                    AdjuntarProgramaEvento(oEvento.RutaPrograma, multipart, ms4);
                }

                //esExtranjero = true; //BORRAR ESTO
                //se arma y se adjunta la invitacion para el evento presencial a un extranjero
                if (oEvento.Formato.Contains("Presencial") && esExtranjero == true)
                {
                    try
                    {
                        var templatePath = "https://" + localhost + "/Invitacion.pdf";
                        var urlConfirmarAsistencia = "https://" + localhost + "/api/EventoParticipante/AsistioInvitadoExtranjero/" + idInvitacion + "/1";//QUITAR EL 1 NO ES UN PUNTO DE ACCESO
                        var reader = new PdfReader(templatePath);
                        iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                        //crea y escribe un PDF
                        MemoryStream outStream2 = new MemoryStream();
                        outStream2 = crearPDFDesdeCero(reader, outStream2, oEvento, urlConfirmarAsistencia, ubicacion);
                        byte[] pdfBytes = outStream2.ToArray();
                        var msEntrada = new MemoryStream();
                        msEntrada.Write(pdfBytes, 0, (int)pdfBytes.Length);
                        AdjuntarEntradaExtranjero(msEntrada, multipart);
                    }
                    catch (Exception e)
                    {
                        logger.LogError("Error en invitacion extranjero", e);
                    }
                }


                //esto adjunta el flyear del eventos si es virtual o streaming agrega el link del evento para acceso al correo
                if (File.Exists(localwwwroot))
                {
                    MemoryStream ms6 = new MemoryStream();
                    flyer = FuncionesEmail.crearFlyerYAdjuntarlo(multipart, localwwwroot, nombreFlyer, ms6);
                    if (oEvento.Formato.Contains("Streaming") || oEvento.Formato.Contains("Virtual"))
                    {
                        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + "<a title= \"Flyer\" href =\"" + oEvento.URL + "\">" +
                  "<img src=\"" + flyer.ContentLink.ToString() + "\" alt =\"Flyer\" style=\"width: 600px;\"> </a>";

                    }
                    else
                    {
                        bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + "<img src=\"" + flyer.ContentLink.ToString() + "\" alt =\"Flyer\" style=\"width: 600 px;\">";
                    }
                }

                //agrega la firma al correo
                string pathFirma = @"wwwroot/Templates/textoFirmaCeremonial.txt";
                string textoFirma = obtenerTextoTemplate(pathFirma);
                bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + textoFirma;
                //el encoding es necesario para que acepte los caracteres especiales y las tildes
                var textPart3 = new TextPart("html", Encoding.ASCII, bodyBuilder.HtmlBody);
                multipart.Add(textPart3);
                mailMessage.Body = multipart;
                return mailMessage;
            }
            catch (Exception e)
            {
                logger.LogError("Error en  adjuntar email", e);

            }
            //ADJUNTAR AL CORREO

            return mailMessage;

        }


        /// <param name="nombreFlyer">El nombre del archivo flyers</param>
        public static MimeMessage CrearCuerpoMensajeCancelado(Evento oEvento)
        {

            var root = FuncionesBasicas.getAppSettings();

            var mailMessage = new MimeMessage();

            //mailMessage.From.Add(new MailboxAddress(Encoding.UTF8, root.GetSection("SmtpClient")["UserName"], root.GetSection("SmtpClient")["Address"]));


            mailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "Ceremonial y Relaciones Institucionales", root.GetSection("SmtpClient")["Address"]));

            mailMessage.Subject = "Cancelación de Evento: " + oEvento.Nombre;
            var bodyBuilder = new BodyBuilder();
            LoggerManger logger = new LoggerManger();


            bodyBuilder.HtmlBody = CorreoElectronico.textoCancelacionEvento(oEvento);

            var multipart = new Multipart("mixed");

            string pathFirma = @"wwwroot/Templates/textoFirmaCeremonial.txt";

            string textoFirma = obtenerTextoTemplate(pathFirma);

            bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + textoFirma;


            var textPart3 = new TextPart("html", Encoding.ASCII, bodyBuilder.HtmlBody);

            multipart.Add(textPart3);

            mailMessage.Body = multipart;

            return mailMessage;

        }
        /// <summary>Leer el texto de un template de correo y devuelve el multipar armado para enviar</summary>
        /// <param name="root">Root donde esta el template</param>
        public static string obtenerTextoTemplate(string root)
        {


            FileStream fs = new FileStream(root, FileMode.Open, FileAccess.Read);

            string line;
            int contadorLineas = 0;
            string texto = "";


            using (StreamReader ms7 = new StreamReader(fs))
            {

                while ((line = ms7.ReadLine()) != null)
                {

                    texto = line;
                    contadorLineas++;

                }

            }
            fs.Close();

            return texto;

        }

        /// <summary>Crea el mensaje con la invitacion al 
        /// evento ya sea presencial o virtual/streaming. 
        /// Arma los textos y adjunta los flyers(archivos)
        /// Trae las variables y usuarios de conexion del appConfig</summary>
        /// <param name="oEvento">evento.</param>
        /// <param name="url">URL del evento donde tiene que confirmar el usuario</param>
        /// <param name="localhost">Url del sitio en prod</param>
        /// <param name="domicilio">Donde se realiza el evento</param>
        /// <param name="nombreFlyer">Nombre del archivo flyer</param>
        public static MimeMessage CrearCuerpoMensaje(string correoE, Evento oEvento, string url, string localhost, string domicilio, string nombreFlyer)
        {
            var mailMessage = new MimeMessage();
            var root = FuncionesBasicas.getAppSettings();
            //mailMessage.From.Add(new MailboxAddress(root.GetSection("SmtpClient")["UserName"], root.GetSection("SmtpClient")["Address"]));
            mailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "Ceremonial y Relaciones Institucionales", root.GetSection("SmtpClient")["Address"]));

            string UrldelPre = url;

            mailMessage.Subject = "Invitación al evento: " + oEvento.Nombre;
            var bodyBuilder = new BodyBuilder();
            LoggerManger logger = new LoggerManger();
            bodyBuilder = FuncionesEmail.elegirTextoStreamingOPresencialInvitacion(oEvento, bodyBuilder);
            var multipart = new Multipart("mixed");

            bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + CorreoElectronico.retornarBotonConfirmarEmail(UrldelPre);

            //var ruta = @"wwwroot/Templates/" + "qrnuevo.png";


            //try
            //{

            //    string pathqr = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + "qrnuevo.png";
            //    if (File.Exists(pathqr))
            //    {
            //        File.Delete(pathqr);

            //    }

            //    using (QRCode barcode = new QRCode())

            //    {
            //        barcode.RegistrationName = "demo";
            //        barcode.RegistrationKey = "demo";

            //        // Set value            
            //        barcode.Value = UrldelPre;

            //        // Uncomment if you don't need margins
            //        //barcode.Margins = new Margins(0, 0, 0, 0);
            //        //barcode.DrawQuietZones = false;


            //        // Save barcode image to file

            //        //barcode.SaveImage(ruta);

            //        byte[] imagenqr = barcode.GetImageBytes();


            //        MemoryStream m = null;
            //        using (m = new MemoryStream(imagenqr))
            //        {
            //            using (System.IO.FileStream fileStream = new FileStream(pathqr, FileMode.Create, FileAccess.Write))
            //            {

            //                m.WriteTo(fileStream);
            //            }
            //        }
            //    }

            //}
            //catch (Exception e)
            //{

            //    logger.LogError("Error en  crear qr", e);


            //}


            try
            {

                LinkedResource flyer = null;

                //string localwwwroot = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombreFlyer;

                //nombreFlyer = "qrnuevo.png";

                string localwwwroot = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombreFlyer;


                //string localwwwroot = ruta;


                string InputText = CorreoElectronico.crearUrlAlDisp(oEvento, localhost);

                if (File.Exists(localwwwroot))
                {
                    MemoryStream ms6 = new MemoryStream();

                    flyer = FuncionesEmail.crearFlyerYAdjuntarlo(multipart, localwwwroot, nombreFlyer, ms6);

                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + "<a title= \"Flyer\" href =\"https://" + UrldelPre + "\">" +
                    "<img src=\"" + flyer.ContentLink.ToString() + "\" alt =\"Flyer\" style=\"width: 600px;\"> </a>";
                }

                //LinkedResource qr = null;

                ////string localwwwroot = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombreFlyer;

                //string nombreQr = "qrnuevo.png";

                //string localwwwroot2 = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombreQr;


                ////string localwwwroot = ruta;

                //if (File.Exists(localwwwroot))
                //{
                //    MemoryStream msqr = new MemoryStream();

                //    qr = FuncionesEmail.crearFlyerYAdjuntarlo(multipart, localwwwroot2, nombreQr, msqr);

                //    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + "<a title= \"Flyer\" href =\"https://" + UrldelPre + "\">" +
                //    "<img src=\"" + qr.ContentLink.ToString() + "\" alt =\"Flyer\" style=\"width: 600px;\"> </a>";
                //}




                //LinkedResource qrCode = null;

                //string localwwwroot = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombreFlyer;
                //string InputText = CorreoElectronico.crearUrlAlDisp(oEvento, localhost);

                //if (File.Exists(localwwwroot))
                //{
                //    MemoryStream ms6 = new MemoryStream();

                //    flyer = FuncionesEmail.crearFlyerYAdjuntarlo(multipart, localwwwroot, nombreFlyer, ms6);

                //    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + "<a title= \"Flyer\" href =\"https://" + UrldelPre + "\">" +
                //    "<img src=\"" + flyer.ContentLink.ToString() + "\" alt =\"Flyer\" style=\"width: 600px;\"> </a>";

                //}

                string pathFirma = @"wwwroot/Templates/textoFirmaCeremonial.txt";

                string textoFirma = obtenerTextoTemplate(pathFirma);

                bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + textoFirma;


                multipart.Add(bodyBuilder.ToMessageBody());

                mailMessage.Body = multipart;

                return mailMessage;

            }
            catch (Exception e)
            {
                logger.LogError("Error en  adjuntar email", e);

            }
            //ADJUNTAR AL CORREO

            return mailMessage;

        }
        public static MimeMessage CrearCuerpoMensajeInstitucion(string correoE, Evento oEvento, string url, string localhost, string domicilio, string nombreFlyer)
        {
            var mailMessage = new MimeMessage();
            var root = FuncionesBasicas.getAppSettings();
            mailMessage.From.Add(new MailboxAddress(Encoding.UTF8, "Ceremonial y Relaciones Institucionales", root.GetSection("SmtpClient")["Address"]));

            string UrldelPre = url;

            mailMessage.Subject = "Invitación al evento: " + oEvento.Nombre;
            var bodyBuilder = new BodyBuilder();
            LoggerManger logger = new LoggerManger();
            bodyBuilder = FuncionesEmail.elegirTextoStreamingOPresencialInvitacion(oEvento, bodyBuilder);
            var multipart = new Multipart("mixed");

            bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + CorreoElectronico.retornarBotonConfirmarEmail(UrldelPre);

            try
            {

                LinkedResource flyer = null;

                string localwwwroot = root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombreFlyer;

                string InputText = CorreoElectronico.crearUrlAlDisp(oEvento, localhost);

                if (File.Exists(localwwwroot))
                {
                    MemoryStream ms6 = new MemoryStream();

                    flyer = FuncionesEmail.crearFlyerYAdjuntarlo(multipart, localwwwroot, nombreFlyer, ms6);

                    bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + "<a title= \"Flyer\" href =\"" + UrldelPre + "\">" +
                    "<img src=\"" + flyer.ContentLink.ToString() + "\" alt =\"Flyer\" style=\"width: 600px;\"> </a>";
                }


                string pathFirma = @"wwwroot/Templates/textoFirmaCeremonial.txt";

                string textoFirma = obtenerTextoTemplate(pathFirma);

                bodyBuilder.HtmlBody = bodyBuilder.HtmlBody + textoFirma;


                multipart.Add(bodyBuilder.ToMessageBody());

                mailMessage.Body = multipart;

                return mailMessage;

            }
            catch (Exception e)
            {
                logger.LogError("Error en  adjuntar email a institucion", e);

            }
            //ADJUNTAR AL CORREO

            return mailMessage;

        }

        private static BodyBuilder elegirTextoStreamingOPresencialInvitacion(Evento oEvento, BodyBuilder bodyBuilder)
        {
            if (oEvento.Formato.Contains("Streaming") || oEvento.Formato.Contains("Virtual"))
            {
                bodyBuilder.HtmlBody = CorreoElectronico.retornarHTMLEventoVirtualOStreaming();
            }
            else
            {
                bodyBuilder.HtmlBody = CorreoElectronico.retornarHTMLTextoInvitacionPresencial(oEvento);

            }

            return bodyBuilder;

        }

        /// <summary>pasa la imagen a Bytes, y la almacena en un memoryStream</summary>
        /// <param name="multipart">The multipart.</param>
        /// <param name="localwwwroot">The localwwwroot.</param>
        /// <param name="nombreFlyer">The nombre flyer.</param>
        /// <param name="ms6">The MS6.</param>
        private static LinkedResource crearFlyerYAdjuntarlo(Multipart multipart, string localwwwroot, string nombreFlyer, MemoryStream ms6)
        {
            byte[] bytesFlyer = guardarImagenFlyer(localwwwroot);

            LinkedResource retorno;

            //using (ms6 = new MemoryStream())
            //{
            ms6.Write(bytesFlyer, 0, (int)bytesFlyer.Length);
            retorno = retornarImagenFlyer(ms6, nombreFlyer, multipart);

            //}
            return retorno;


        }

        /// <summary>Toma la entrada para el extranjero y la almacena en un mimepart</summary>
        /// <param name="ms4">The MS4.</param>
        /// <param name="multipart">The multipart.</param>
        private static void AdjuntarEntradaExtranjero(MemoryStream ms4, Multipart multipart)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                var attachmentPartPdf = new MimePart(MediaTypeNames.Application.Pdf)
                {
                    Content = new MimeContent(ms4),
                    ContentId = "Invitacion.pdf",
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "Invitacion.pdf"
                };

                multipart.Add(attachmentPartPdf);


            }
            catch (Exception e)
            {
                logger.LogError("error en AdjuntarEntradaExtranjero", e);

            }



        }

        /// <summary>Toma el archivo almacenado en MS y lo guarda en un MimePart para adjuntarlo al correo</summary>
        /// <param name="nombre">The nombre.</param>
        /// <param name="multipart">The multipart.</param>
        /// <param name="ms4">The MS4.</param>
        private static void AdjuntarProgramaEvento(string nombre, Multipart multipart, MemoryStream ms4)
        {
            LoggerManger logger = new LoggerManger();

            byte[] bytes3 = guardarPDF(nombre);
            //MemoryStream ms4 = null;
            MimePart attachmentPartPdf = null;
            try
            {
                ms4.Write(bytes3, 0, (int)bytes3.Length);

                attachmentPartPdf = new MimePart(MediaTypeNames.Application.Pdf)
                {
                    Content = new MimeContent(ms4),
                    ContentId = "ProgramaEvento.pdf",
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "ProgramaEvento.pdf"
                };

            }
            catch (Exception ex)
            {

                logger.LogError("Error en AdjuntarEntradaExtranjero ", ex);

            }
            finally
            {

                //ms4.Dispose();


            }

            multipart.Add(attachmentPartPdf);

        }


        /// <summary>
        /// toma el MS y lo almacena en un MimePart pasa el archivo Flyer a un LinkResource clase que permite adjuntar en el Html un codigo Cid que muestra la imagen dentro del correo electronico
        /// </summary>
        /// <param name="ms3">The MS3.</param>
        /// <param name="nombre">The nombre.</param>
        /// <param name="multipart">The multipart.</param>
        private static LinkedResource retornarImagenFlyer(Stream ms3, string nombre, Multipart multipart)
        {
            LoggerManger logger = new LoggerManger();

            var root = FuncionesBasicas.getAppSettings();

            var attachmentPartImg = new MimePart(MediaTypeNames.Image.Jpeg)
            {
                Content = new MimeContent(ms3),
                ContentId = nombre,
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = nombre
            };

            ms3.Position = 0;

            LinkedResource linkedResource = new LinkedResource(root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers/" + nombre);

            //LinkedResource linkedResource = new LinkedResource("https://eventosdesa.trabajo.gob.ar/templates/" + nombre);


            linkedResource.ContentId = nombre;
            linkedResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            linkedResource.ContentType.Name = nombre;
            linkedResource.ContentLink = new Uri("cid:" + nombre);

            multipart.Add(attachmentPartImg);

            return linkedResource;

        }

        /// <summary>Toma la imagen almacenada en dentro de las carpetas Temporales y lo transforma en un array de bytes necesarios para pasarlo a MemoryStream</summary>
        /// <param name="PicPath">The pic path.</param>
        /// <returns>un array de bytes que es la imagen</returns>
        private static byte[] guardarImagenFlyer(string PicPath)
        {
            LoggerManger logger = new LoggerManger();

            FileStream fs = new FileStream(PicPath, FileMode.Open, FileAccess.Read);

            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];

            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

            //Close a file stream
            fs.Close();

            return imgByteArr;

        }

        /// <summary>Pasa el PDF del programa a Bytes para agregarlo a un stream</summary>
        /// <param name="PicPath">The pic path.</param>
        private static byte[] guardarPDF(string PicPath)
        {
            LoggerManger logger = new LoggerManger();
            FileStream fs = null;
            byte[] pdfByteArray = null;

            var root = FuncionesBasicas.getAppSettings();

            try
            {
                fs = new FileStream(root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Otros/" + PicPath, FileMode.Open, FileAccess.Read);

                //Initialize a byte array with size of stream
                pdfByteArray = new byte[fs.Length];

                //Read data from the file stream and put into the byte array
                fs.Read(pdfByteArray, 0, Convert.ToInt32(fs.Length));

                //Close a file stream
                fs.Close();


            }
            catch (Exception ex)
            {

                logger.LogError("Error en guardarPDF ", ex);

            }
            finally
            {

                fs.Close();

            }


            return pdfByteArray;

        }

        /// <summary>Images to byte.</summary>
        /// <param name="img">The img.</param>
        private static byte[] ImageToByte(System.Drawing.Image img)
        {
            MemoryStream stream = null;
            using (stream = new MemoryStream())
            {

                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);


            }
            return stream.ToArray();
        }

        /// <summary>
        /// Arma el PDF de la Invitacion para usurios extanjeros, Agrega un 
        /// QR y una tabla con texto. En caso de comilacion en VS pede lanzar error 
        /// tres veces hasta finalmente crearse y enviarse. en produccion no tiene inconvenientes.
        /// </summary>
        /// <param name="reader">Clase que instancia un lecto de PDF en Itext</param>
        /// <param name="outStream">Stream.</param>
        /// <param name="oEvento">evento.</param>
        /// <param name="url">Link del codigoQr</param>
        /// <param name="domicilio">Ubicacion del evento</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static MemoryStream crearPDFDesdeCero(PdfReader reader, MemoryStream outStream, Evento oEvento, string url, string domicilio)
        {

            LoggerManger logger = new LoggerManger();
            MemoryStream outStream2 = new MemoryStream();


            try
            {

                var stamper = new PdfStamper(reader, outStream2);
                if (outStream2.Length > 0)
                {
                    iTextSharp.text.Font bold = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                    iTextSharp.text.Font italic = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);

                    PdfContentByte overContentByte = stamper.GetOverContent(1);
                    PdfPTable encabezado = new PdfPTable(1);
                    encabezado.DefaultCell.BackgroundColor = BaseColor.BLACK;

                    PdfPCell celp2 = new PdfPCell(new Phrase(new Chunk(oEvento.Descripcion, italic)));
                    celp2.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell celp1 = new PdfPCell(new Phrase(new Chunk(oEvento.Nombre.ToUpper(), bold)));
                    celp1.HorizontalAlignment = Element.ALIGN_CENTER;


                    encabezado.AddCell(celp1);
                    encabezado.AddCell(celp2);
                    encabezado.DefaultCell.Padding = 10;

                    encabezado.DefaultCell.Border = 0;


                    //document.Add(encabezado);
                    reader.SelectPages("1-2");
                    var pageSize = reader.GetPageSize(1);

                    //gettins the page size in order to substract from the iTextSharp coordinates

                    // PdfContentByte from stamper to add content to the pages over the original content
                    PdfContentByte pbover = stamper.GetOverContent(1);

                    //add content to the page using ColumnText
                    PdfPTable table = new PdfPTable(1);
                    table.TotalWidth = 500;

                    iTextSharp.text.Font italicBlack = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);


                    table.DefaultCell.Padding = 10;
                    table.DefaultCell.UseAscender = true;
                    table.DefaultCell.UseDescender = true;
                    table.DefaultCell.Border = 0;

                    BarcodeQRCode qrcode = new BarcodeQRCode(
                url, 5, 5, null
                   );

                    iTextSharp.text.Image img = qrcode.GetImage();

                    for (int aw = 0; aw < 4; aw++)
                    {
                        if (aw < 3)
                        {
                            Phrase phrasetabla = new Phrase();

                            if (aw == 0)
                            {
                                //Font font = new Font();
                                //font.Size = 25;

                                PdfPCell cell1 = new PdfPCell();
                                phrasetabla.Add(new Chunk("Estimado / a :", italicBlack));
                                //cell1.HorizontalAlignment = Element.ALIGN_CENTER;

                                cell1.AddElement(phrasetabla);

                            }



                            var texto = "A través del presente adjunto le enviamos un código Qr que debe presentar en la entrada para su ingreso."
                                + "Recuerde que debe presentarse el día " + oEvento.FechaHoraInicio.Value.ToShortDateString() + " a las " + oEvento.FechaHoraInicio.Value.ToShortTimeString() + " hs. en " +
                               domicilio + ".";


                            if (aw == 1) phrasetabla.Add(new Chunk(texto, italicBlack));


                            var texto2 = "Muchas gracias.";
                            if (aw == 2) phrasetabla.Add(new Chunk(texto2, italicBlack));

                            table.AddCell(phrasetabla);


                        }
                        else
                        {


                            if (aw == 3)
                            {
                                PdfPCell cell1 = new PdfPCell();

                                cell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                                Paragraph p = new Paragraph();
                                img.ScalePercent(300);

                                p.Add(new Chunk(img, 0, 0, true));
                                p.Alignment = 1;// ES AL MEDIO
                                cell1.Border = 0;

                                cell1.AddElement(p);

                                table.AddCell(cell1);
                            }
                        }

                    }
                    //encabezado.WriteSelectedRows(0, -1, 110, 70, overContentByte);

                    table.WriteSelectedRows(0, -1, 50, 700, overContentByte);


                    stamper.Close();
                }
            }
            catch (Exception ex)
            {
            }


            finally
            {
                reader.Close();

            }
            return outStream2;

        }

        /// <summary>Utiliar Mailkit para enviar un correo, recibe el html del correo con los adjuntos. Trae los 
        /// 
        /// s del appConfig para la conexion</summary>
        /// <param name="mailMessage">The mail message.</param>
        public static async Task Conectarse(MimeMessage mailMessage)
        {
            LoggerManger logger = new LoggerManger();

            var root = FuncionesBasicas.getAppSettings();

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;//variables necesarias que los correos lleguen correctamente a los servicios de google hotmail y outlook
                    var dato = root.GetSection("SmtpClient")["Port"];
                    smtpClient.Connect(root.GetSection("SmtpClient")["Host"], int.Parse(dato));//necesita tranformar datos string del appConfig en numeros
                    smtpClient.Authenticate(root.GetSection("SmtpClient")["UserName"], root.GetSection("SmtpClient")["Password"]);

                    smtpClient.Send(mailMessage);

                    smtpClient.Disconnect(true);
                }
                catch (Exception e)
                {

                    logger.LogError("Error en smtpClient ", e);
                }
            }

        }



    }
}
