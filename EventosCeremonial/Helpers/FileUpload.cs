using BlazorInputFile;
using EventosCeremonial.IRepository;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;



namespace EventosCeremonial.Helpers
{
    public class FileUpload : IFileUpload
    {

        private readonly IWebHostEnvironment _oWebHostEnvironment;

        /// <summary>Initializes a new instance of the <see cref="FileUpload" /> class.
        /// Necesaria para poder utilizar la url del sitio</summary>
        /// <param name="oWebHostEnvironment">The o web host environment.</param>
        public FileUpload(IWebHostEnvironment oWebHostEnvironment)
        {
            _oWebHostEnvironment = oWebHostEnvironment;

        }

        /// <summary>Compara con un array de string de tipos de archivos si el archivo indicado está dentro ese array</summary>
        /// <param name="tipos">The tipos.</param>
        /// <param name="path">The path.</param>
        /// <returns>true si es valido, false si es invalido</returns>
        public static bool TipoArchivoValido(string[] tipos, string path)
        {

            foreach (var item in tipos)
            {
                string extension = path.Split(".")[1];
                if (extension == item)
                {
                    return true;
                }
            }
            return false;

        }

        /// <summary>Recorre la carpeta Archivos index y borra los archivos que sobran. Con excepcion de Noborrar</summary>
        public async Task borrarArchivosEnArchivosIndex()
        {
            var root = FuncionesBasicas.getAppSettings();




            string temporales =    root.GetSection("DVArchivos")["EventosCeremonial"] + "/FotosIndex/";
            LoggerManger logger = new LoggerManger();



            System.IO.DirectoryInfo di = new DirectoryInfo(temporales);

            foreach (FileInfo file in di.GetFiles())
                {
                    if (file.Name != "NOBORRAR.pdf")
                    {
                        string fileDelete = System.IO.Path.Combine(temporales, file.Name);


                    if (File.Exists(fileDelete))
                    {
                        System.IO.File.Delete(fileDelete);

                    }

                }

                }

        }

        /// <summary>Borrar los archivos en temporales, se puede especificar en cuales de las carpetas de temporales se puede borrar y cual es el archivo que se quiere eliminar</summary>
        /// <param name="archivo">The archivo.</param>
        /// <param name="tipo">The tipo.</param>
        public async Task borrarArchivosEnTemporales(string archivo, string tipo)
        {
            LoggerManger logger = new LoggerManger();
            var root = FuncionesBasicas.getAppSettings();

            string temporales =    root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Otros/";


            try
            {

                System.IO.DirectoryInfo di = new DirectoryInfo(temporales);

                if (archivo == "")
                {
                   

                    foreach (FileInfo file in di.GetFiles())
                    {

                        if (file.Name != "NOBORRAR.pdf")
                        {
                            string fileDelete = System.IO.Path.Combine(temporales, file.Name);


                            if (File.Exists(fileDelete)) {


                                try
                                {
                                    System.IO.File.Delete(fileDelete);



                                }
                                catch (Exception e)
                                {
                                    logger.LogError("Error en borrarArchivosEnTemporales", e);

                                }

                            }


                        }

                    }



                    string temporalesF =    root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Flyers";


                    di = new DirectoryInfo(temporalesF);


                    foreach (FileInfo file in di.GetFiles())
                    {


                        if (file.Name != "NOBORRAR.pdf")
                        {

                            string fileDelete = System.IO.Path.Combine(temporalesF, file.Name);

                            if (File.Exists(fileDelete))
                            {
                                System.IO.File.Delete(fileDelete);

                            }

                        }

                    }

                    string temporalesP =    root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Portadas";


                    di = new DirectoryInfo(temporalesP);


                    foreach (FileInfo file in di.GetFiles())
                    {


                        if (file.Name != "NOBORRAR.pdf")
                        {

                            string fileDelete = System.IO.Path.Combine(temporalesP, file.Name);

                            if (File.Exists(fileDelete))
                            {
                                System.IO.File.Delete(fileDelete);

                            }

                        }

                    }


                }

                if (tipo == "flyer")
                {

                    if (archivo != null)
                    {

                        string temporalesF =  root.GetSection("DVArchivos")["EventosCeremonial"] + " / Temporales/Flyers/";

                        Console.WriteLine(temporalesF);


                        string fileDelete = System.IO.Path.Combine(temporalesF, archivo);

                        Console.WriteLine(fileDelete);

                        if (File.Exists(fileDelete))
                        {
                            System.IO.File.Delete(fileDelete);


                        }
                    }
                }

                if (tipo == "programa")
                {

                    if (archivo != null)
                    {

                        string temporalesPr =    root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Otros/";


                        string fileDelete = System.IO.Path.Combine(temporalesPr, archivo);
                        if (File.Exists(fileDelete))
                        {
                            System.IO.File.Delete(fileDelete);

                        }
                    }
                }

                if (tipo == "portada")
                {

                    if (archivo != null)
                    {
                        string temporalesP =    root.GetSection("DVArchivos")["EventosCeremonial"] + "/Temporales/Portadas/";

                        string fileDelete = System.IO.Path.Combine(temporalesP, archivo);
                        if (File.Exists(fileDelete))
                        {
                            try
                            {
                                System.IO.File.Delete(fileDelete);

                            }
                            catch (Exception e)
                            {

                                logger.LogError("Error eliminar archivo programa", e);
                            }

                        }
                    }
                }

            }
            catch (Exception e)
            {

                logger.LogError("Error editar programa", e);
            }


        }

        /// <summary>Uploads the asynchronous new temporary file.</summary>
        /// <param name="file">El archivo.</param>
        /// <param name="nombre">El nombre.</param>
        /// <param name="tipo">El tipo.</param>
        public async Task UploadAsyncNewTempFile(IFileListEntry file, string nombre, string tipo)

        {
            LoggerManger logger = new LoggerManger();
            var root = FuncionesBasicas.getAppSettings();

            try
            {

                string path = System.IO.Path.Combine( root.GetSection("DVArchivos")["EventosCeremonial"], "Temporales/Otros/", nombre);

                if (tipo == "flyer")
                {

                    path = System.IO.Path.Combine((root.GetSection("DVArchivos")["EventosCeremonial"]), "Temporales/Flyers/", nombre);

                }

                if (tipo == "portada")
                {


                    path = System.IO.Path.Combine(root.GetSection("DVArchivos")["EventosCeremonial"], "Temporales/Portadas/", nombre);

                }


                MemoryStream memoryStream = null;
                try
                {
                    if (!File.Exists(path)) {//SI NO EXISTE EL ARCHIVO LO VA A GUARDAR, NO PUEDE HABER DOS ARCHIVOS CON EL MISMO NOMBRE EN LA CARPETA

                        using (memoryStream = new MemoryStream())
                        {

                            await file.Data.CopyToAsync(memoryStream);

                            using (System.IO.FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                            {

                                memoryStream.WriteTo(fileStream);


                            }

                        }

                    }
                    
                }
                finally
                {
                    memoryStream.Dispose();
                }
               



               
            }
            catch (Exception e)
            {
                logger.LogError("Error agregar programa temporal", e);

            }


        }


        /// <summary>Guarda un archivo que se encuentra en Bytes en carpeta temporal.</summary>
        /// <param name="file">The file.</param>
        /// <param name="nombre">The nombre.</param>
        public async Task GuardarArchivoEnTemporal(byte[] file, string nombre)

        {
            LoggerManger logger = new LoggerManger();
            var root = FuncionesBasicas.getAppSettings();

            try
            {

                string path = System.IO.Path.Combine(root.GetSection("DVArchivos")["EventosCeremonial"], "Temporales/Otros/", nombre);

                if (File.Exists(path)) {

                    using (var stream = new MemoryStream())
                    {

                        stream.Position = 0;


                        stream.Write(file, 0, file.Length);

                        var fileBytes = stream.ToArray();



                        using (System.IO.FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {

                            stream.WriteTo(fileStream);


                        }

                    }

                }


            }
            catch (Exception e)
            {
                logger.LogError("Error agregar programa temporal", e);

            }


        }


    }
}
