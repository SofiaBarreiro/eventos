using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeOpenXml;
using EventosCeremonial.Data;
using EventosCeremonial.Data.Response;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using OfficeOpenXml.Style;
using System.Drawing;
using Microsoft.AspNetCore.Http;
using System.Text;


namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     public  class ExcelController : Controller
    {


        /// <summary>Genera un excel que se descarga automaticamente en el navegador</summary>
        /// <param name="id">El id del evento</param>
        /// <returns>el archivo excel en formato xls</returns>
        [HttpGet("{Id}")]
        [ActionName("GenerarExcel")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<FileContentResult> GenerarExcel(int id)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            byte[] bytes = null;

            LoggerManger logger = new LoggerManger();
            try
            {

                FileInfo fileName = ReturnFile(id);

                if (fileName.Exists)
                {
                    fileName.Delete();

                }
                Respuesta<List<Organismo>> oRespuesta = new Respuesta<List<Organismo>>();
                //agrega los datos de organismos en el excel de el archivo
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    var lst = db.Organismos.ToList();
                    if (lst.Count() > 0)
                    {
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = lst;
                    }
                    else
                    {
                        oRespuesta.Exito = 0;
                    }

                }
                string builder = null;
                if (oRespuesta.Exito == 1)
                {

                    using (HttpClient httpClient = new HttpClient())
                    {
                        using (var package = new ExcelPackage())
                        {
                            //agrega libros en la planilla de excel
                            var sheet = package.Workbook.Worksheets.Add("Libro");
                            var sheet2 = package.Workbook.Worksheets.Add("Organismos");
                            sheet = EscribirPlanillaParticipantes(sheet);
                            sheet2 = EscribirPlantillaOrganismos(sheet2, oRespuesta.Data);

                            bytes = await package.GetAsByteArrayAsync();

                        }
                    }

                }
               
            }
            catch (Exception ex) {
                logger.LogError("Error generar excel ", ex);


            }
           
            return File(bytes, "text/xlsx", "Invitaciones.xlsx");

        }

        /// <summary>Agrega las columnas a la plantilla de invitados</summary>
        /// <param name="sheet">The sheet.</param>
        [ApiExplorerSettings(IgnoreApi = true)]

        private ExcelWorksheet EscribirPlanillaParticipantes(ExcelWorksheet sheet)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                sheet.Cells[1, 1].Value = "Nombre";
                sheet.Cells[1, 2].Value = "Apellido";
                sheet.Cells[1, 3].Value = "eMail";
                sheet.Cells[1, 4].Value = "Telefono";
                sheet.Cells[1, 5].Value = "Puesto";
                sheet.Cells[1, 6].Value = "IdOrganismo";


                using (var range = sheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);

                }
                sheet.Column(6).AutoFit();


            }catch(Exception e)
            {
                logger.LogError("Error generar excel ", e);

            }


            return sheet;
        }

        /// <summary>Agrega las columnas a la planilla de Participantes</summary>
        /// <param name="sheet">The sheet.</param>
        [ApiExplorerSettings(IgnoreApi = true)]

        private ExcelWorksheet EncabezadoListaInvitados(ExcelWorksheet sheet)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                sheet.Cells[1, 1].Value = "Nombre";
                sheet.Cells[1, 2].Value = "Apellido";
                sheet.Cells[1, 3].Value = "eMail";
                sheet.Cells[1, 4].Value = "Telefono";
                sheet.Cells[1, 5].Value = "Puesto";
                sheet.Cells[1, 6].Value = "Organismo";
                sheet.Cells[1, 7].Value = "Asistió";

                using (var range = sheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);

                    range.AutoFilter = true;
                    var colCompany = sheet.AutoFilter.Columns.AddValueFilterColumn(0);

                    // ApplyFilter() will set the rows that don't match the filters to hidden.
                    sheet.AutoFilter.ApplyFilter();
                }

                sheet.Column(7).AutoFit();

            }
            catch (Exception e)
            {
                logger.LogError("Error EncabezadoListaInvitados", e);

            }

            return sheet;
        }

        /// <summary>Escriba la plantilla de organismos</summary>
        /// <param name="sheet2">The sheet2.</param>
        /// <param name="listaOrganismos">The lista organismos.</param>
        [ApiExplorerSettings(IgnoreApi = true)]

        private ExcelWorksheet EscribirPlantillaOrganismos(ExcelWorksheet sheet2, List<Organismo> listaOrganismos)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                sheet2.Cells[1, 1].Value = "Id";
                sheet2.Cells[1, 2].Value = "Nombre";
                var cantidadOrganismos = 0;
                var total = listaOrganismos.Count + 1;

                int col = 0;
                int row = 0;

                for (row = 2; row <= total; row++) //es porque empieza de una fila menos, necesito una fila mas
                {
                    Organismo item = listaOrganismos.ElementAt(cantidadOrganismos);

                    for (col = 1; col <= 2; col++)
                    {
                        sheet2.Column(col).AutoFit();

                        if (col == 1) sheet2.Cells[row, col].Value = item.Id;
                        else if (col == 2) sheet2.Cells[row, col].Value = item.Nombre.ToString();



                    }
                    cantidadOrganismos++;


                }

                for (int i = 1; i < row; i++)
                {

                    using (var range = sheet2.Cells[i, 1, i, 2])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                        range.Style.Font.Color.SetColor(Color.White);
                    }

                }

                using (var range = sheet2.Cells[1, 1, 1, 2])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                    range.Style.Font.Color.SetColor(Color.White);
                }

            }
            catch (Exception e)
            {
                logger.LogError("Error EscribirPlantillaOrganismos", e);

            }
            return sheet2;
        }


        /// <summary>Crea la plantilla de excel de evento finalizado, descarga un excel con los datos</summary>
        /// <param name="id">The identifier.</param>
        [HttpGet("EventoFinalizado/{Id}")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<FileContentResult> GenerarExcelEventoFinalizado(int id)
        {
            LoggerManger logger = new LoggerManger();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            byte[] bytes = null;
            FileInfo fileName = ReturnFile(id);
            if (fileName.Exists)
            {
                fileName.Delete();
            }

            List<Invitacion> listaInvitacion = new List<Invitacion>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Invitacions.Any(x => x.IdEvento == id))
                    {
                        listaInvitacion = db.Invitacions.Where(x => x.IdEvento == id).ToList();

                    }
                    else {
                        logger.LogError("Error invitacion no se encontro");

                    }


                }

                using (HttpClient httpClient = new HttpClient())
                {

                    using (var package = new ExcelPackage())
                    {
                        var sheet = package.Workbook.Worksheets.Add("Libro");
                        sheet = EncabezadoListaInvitados(sheet);


                        var estadoParticipante = "";

                        using (EventosCeremonialContext db = new EventosCeremonialContext())
                        {
                            var row = 2;

                            foreach (var item in listaInvitacion)
                            {
                                if (db.EventoParticipantes.Any(p => p.IdInvitacion == item.Id)) {

                                    EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == item.Id).ToList().ElementAt(0);
                                    estadoParticipante = oEventoParticipante.EstadodeInscripcion;
                                    if (db.Personas.Any(p => p.Id == item.IdPersona))
                                    {

                                        Persona oPersona = db.Personas.Where(p => p.Id == item.IdPersona).ToList().ElementAt(0);

                                        var organismo = db.Organismos.Find(oPersona.IdOrganismo);

                                        var col = 1;

                                        for (col = 1; col <= 7; col++)
                                        {
                                            sheet.Column(col).AutoFit();

                                            if (col == 1) sheet.Cells[row, col].Value = oPersona.Nombre.ToString();
                                            else if (col == 2) sheet.Cells[row, col].Value = oPersona.Apellido.ToString();
                                            else if (col == 3) sheet.Cells[row, col].Value = oPersona.MailContacto.ToString();
                                            else if (col == 4) sheet.Cells[row, col].Value = oPersona.TelefonoContacto;
                                            //else if (col == 5) sheet.Cells[row, col].Value = oPersona.Puesto.ToString();
                                            else if (col == 6) sheet.Cells[row, col].Value = organismo.Nombre;
                                            else if (col == 7)
                                            {

                                                sheet.Cells[row, col].Value = estadoParticipante.ToString();

                                                using (var range = sheet.Cells[row, 1, row, 7])
                                                {
                                                    if (estadoParticipante.ToString() == "Asistió")
                                                    {

                                                        range.Style.Font.Bold = true;
                                                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                        range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                                                        range.Style.Font.Color.SetColor(Color.Black);

                                                    }

                                                }
                                            }

                                        }


                                        row++;
                                    }
                                    else {
                                        logger.LogError("Error Personas no se encontro");

                                    }
                                }
                                else
                                {
                                    logger.LogError("Error EventoParticipantes no se encontro");

                                }


                            }
                        }
                        bytes = await package.GetAsByteArrayAsync();

                    }


                }
            }
            catch (Exception e)
            {
                logger.LogError("Error GenerarExcelEventoFinalizado", e);

            }

            return File(bytes, "text/xlsx", "Participantes"+ id + ".xlsx");

        }

        /// <summary>Si el archivo ya se encuentra denro de la carpeta descargas lo que hace es fijarse si se encuentra y si esta lo borra para que no haya archivos duplicados</summary>
        /// <param name="Id">The identifier.</param>

        [ApiExplorerSettings(IgnoreApi = true)]

        private FileInfo ReturnFile(int Id)
        {
            LoggerManger logger = new LoggerManger();
            var cadena = "";
            string fileName = "Invitaciones#" + Id.ToString() + ".xlsx";

            try
            {
                 cadena = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                Environment.SpecialFolder.UserProfile.ToString();

                string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).ToString();
                }


            }
            catch (Exception ex)
            {
                logger.LogError("Error delete excel ", ex);

            }

            var FilePath = cadena + "\\Downloads\\" + fileName;
            FileInfo fileinfo = new FileInfo(FilePath);

            return fileinfo;

        }


        /// <summary>escribe los organismos en la plantilla de organismos</summary>
        /// <param name="worksheetOrganismos">The worksheet organismos.</param>
        //protected List<Organismo> recorrerLibroOrganismo(ExcelWorksheet worksheetOrganismos)
        //{
        //    LoggerManger logger = new LoggerManger();

        //    List<Organismo> listaOrg = new List<Organismo>();
        //    Organismo oOrganismo = new Organismo();
        //    try
        //    {
        //        int colCount = worksheetOrganismos.Dimension.End.Column;
        //        int rowCount = worksheetOrganismos.Dimension.End.Row;
        //        for (int row = 2; row <= rowCount; row++)
        //        {
        //            Organismo org = new Organismo();

        //            for (int col = 1; col <= colCount; col++)
        //            {
        //                if (col == 1) org.IdPais = retornarDatoInt(worksheetOrganismos.Cells[row, col].Value.ToString());
        //                if (col == 2) org.Nombre = retornarDatoString(worksheetOrganismos.Cells[row, col].Value);
        //                if (col == 3) org.MailContacto = retornarDatoString(worksheetOrganismos.Cells[row, col].Value);
        //                if (col == 4) org.TelefonoContacto = worksheetOrganismos.Cells[row, col].Value.ToString();
        //                if (col == 5) org.TipoOrganismo = retornarDatoString(worksheetOrganismos.Cells[row, col].Value);

        //            }
        //            corregirDatosOrganismo(org);

        //            //org.tipo = "Organismo";
        //            listaOrg.Add(org);

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        logger.LogError("recorrerLibroOrganismo", e);


        //    }

        //    return listaOrg;


        //}

        /// <summary>Abre el archivo excel de invitados y lo lee, devuelve un array de Personas con sus datos</summary>
        /// <param name="myFile">My file.</param>
        [HttpPost("UpLoadExcel")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult Post(IFormFile myFile)
        {

            LoggerManger logger = new LoggerManger();

            Respuesta<List<Persona>> oRespuesta = new Respuesta<List<Persona>>();

            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                myFile.CopyTo(stream);
            }
            try
            {
                List<Persona> listaPar = new List<Persona>();
                using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(1250)))
                {
                    //This allows you to do one Read operation.

                    string line;
                    int cont = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var array = line.Split(";");

                        if (cont > 0)
                        {

                            Persona oPersona = new Persona();

                            oPersona.Nombre = array[0];
                            oPersona.Apellido = array[1];
                            oPersona.MailContacto = array[2];
                            oPersona.TelefonoContacto = array[3];
                            oPersona.Puesto = array[4];
                            int valorIdOrg = retornarDatoInt(array[5]);
                            if (valorIdOrg == 1)
                            {
                                using (EventosCeremonialContext db = new EventosCeremonialContext())
                                {
                                    var idOgranismo = db.Organismos.Where(x => x.Nombre == "SIN ORGANISMO").ToList().ElementAt(0).Id;
                                    if (idOgranismo == 0)
                                    {

                                        oPersona.IdOrganismo = idOgranismo;

                                    }
                                    else
                                    {

                                        oPersona.IdOrganismo = 2;

                                    }
                                }
                                


                            }
                            else
                            {

                                oPersona.IdOrganismo = valorIdOrg;
                            }

                            listaPar.Add(oPersona);

                        }
                        cont++;

                    }
                }


                if (listaPar.Count() == 0)
                {
                    oRespuesta.Exito = 2;

                }
                else
                {
                    oRespuesta.Data = listaPar;
                    oRespuesta.Exito = 1;

                }
            }
            catch (Exception ex)
            {
                logger.LogError("UpLoadExcel", ex);

            }


            return Ok(oRespuesta);
        }

        /// <summary>Recorre la lista de participantes y la escribe en el archivo</summary>
        /// <param name="worksheetParticipantes">The worksheet participantes.</param>
         [ApiExplorerSettings(IgnoreApi = true)]
        public List<Persona> recorrerLibroParticipante(ExcelWorksheet worksheetParticipantes)
        {
            LoggerManger logger = new LoggerManger();

            List<Persona> listaPar = new List<Persona>();
            Persona oPersona = new Persona();
            try
            {
                int colCount = worksheetParticipantes.Dimension.End.Column;
                int rowCount = worksheetParticipantes.Dimension.End.Row;
                for (int row = 2; row <= rowCount; row++)
                {
                    //if (worksheetParticipantes.Cells[row, 1].Value.ToString() != "" && worksheetParticipantes.Cells[row, 2].Value.ToString() != "") {
                    Persona pers = new Persona();

                    for (int col = 1; col <= colCount; col++)
                    {
                        if (col == 1) pers.Nombre = retornarDatoString(worksheetParticipantes.Cells[row, col].Value);
                        else if (col == 2) pers.Apellido = retornarDatoString(worksheetParticipantes.Cells[row, col].Value);
                        else if (col == 3) pers.MailContacto = VerificarExpresionEmail(worksheetParticipantes.Cells[row, col].Value);
                        else if (col == 4)
                        {

                            var dato = retornarDatoString(worksheetParticipantes.Cells[row, col].Value);

                            if (dato != "" && dato != null)
                            {
                                pers.TelefonoContacto = dato;

                            }
                            else
                            {
                                pers.TelefonoContacto = "11111";
                            }

                        }
                        else if (col == 5) pers.Puesto = retornarDatoString(worksheetParticipantes.Cells[row, col].Value);
                        //else if (col == 6) pers.IdOrganismo = retornarDatoInt(worksheetParticipantes.Cells[row, col].Value);
                        else if (col == 6)
                        {
                            int valorIdOrg = retornarDatoInt(worksheetParticipantes.Cells[row, col].Value);
                           


                            if (valorIdOrg == 1)
                            {

                                using (EventosCeremonialContext db = new EventosCeremonialContext())
                                {
                                    var idOgranismo = db.Organismos.Where(x => x.Nombre == "SIN ORGANISMO").ToList().ElementAt(0).Id;
                                    if (idOgranismo == 0)
                                    {

                                        pers.IdOrganismo = idOgranismo;

                                    }
                                    else
                                    {

                                        pers.IdOrganismo = 2;

                                    }
                                }
                            }
                            else
                            {

                                pers.IdOrganismo = valorIdOrg;
                            }


                        }


                    }

                    listaPar.Add(pers);


                }
            }
            catch (Exception ex)
            {
                logger.LogError("recorrerLibroParticipante", ex);

            }

            return listaPar;


        }

        /// <summary>Se fija que la expresión ingresada contenga el formato correcto para un email</summary>
        /// <param name="dato">The dato.</param>
        [ApiExplorerSettings(IgnoreApi = true)]

        public string VerificarExpresionEmail(object dato)
        {
            string email = retornarDatoString(dato);
            LoggerManger logger = new LoggerManger();

            try
            {
                if (email != "")
                {

                    String sFormato;
                    sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                    if (Regex.IsMatch(email, sFormato))
                    {
                        if (Regex.Replace(email, sFormato, String.Empty).Length == 0)
                        {
                            email = dato.ToString();
                        }
                        else
                        {
                            email = "";
                        }
                    }
                    else
                    {
                        email = "";
                    }
                }
            }
            catch(Exception e)
            {
                logger.LogError("VerificarExpresionEmail", e);

            }

            return email;
        }

        /// <summary>Verific que un dato sea string, sino retorna ""</summary>
        /// <param name="celda">The celda.</param>
        [ApiExplorerSettings(IgnoreApi = true)]

        private string retornarDatoString(Object celda)
        {
            string retorno = "";
            var a = celda as string;
            if (a != null)
            {
                retorno = a;
            }
            return retorno;

        }

        /// <summary>Verifica que el dato ingresado sea int, de lo contrario devuelve un cero</summary>
        /// <param name="celda">The celda.</param>
        [ApiExplorerSettings(IgnoreApi = true)]

        private int retornarDatoInt(Object celda)
        {
            int retorno = Convert.ToInt32(celda);
            if (retorno == 0)
            {
                retorno = 1;
            }
            return retorno;

        }

        /// <summary>verifica que el dato ingresado no cntenga datos varios, de lo contrario los rellena con informacion</summary>
        /// <param name="org">The org.</param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public Organismo corregirDatosOrganismo(Organismo org)
        {

            if (!(org.IdPais >= 1))
            {
                org.IdPais = 1;
            }

            if (org.Nombre == "")
            {
                org.Nombre = "SIN NOMBRE";
            }
            if (org.MailContacto == "")
            {
                org.Nombre = "SIN MAIL";
            }
            return org;

        }


    }
}