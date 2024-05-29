using EventosCeremonial.Data;
using EventosCeremonial.Data.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivosController : ControllerBase
    {

        /// <summary>Obtiene los elementos de la lista de Archivos</summary>
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<int> oRespuesta = new Respuesta<int>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Archivos.Count();

                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

        /// <summary>Obtiene el elemento indicado a través del Id. El campo temporal es el Id del elemento, es decir el Id del evento</summary>
        /// <param name="Id">The identifier.</param>
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<Archivo> oRespuesta = new Respuesta<Archivo>();
            LoggerManger logger = new LoggerManger();

            using (EventosCeremonialContext db = new EventosCeremonialContext())
            {

                try
                {

                    if (db.Archivos.Any(p => p.Temporales == Id) == true)
                    {

                        var lst = db.Archivos.Where(p => p.Temporales == Id).ToList().ElementAt(0);

                        oRespuesta.Exito = 1;

                        oRespuesta.Data = lst;
                    }
                    else {


                        oRespuesta.Data = null;


                    }

                }
                catch (Exception ex)
                {
                    oRespuesta.Mensaje = ex.Message;
                    oRespuesta.Data = null;
                    logger.LogError("Error en get archivo", ex);

                }

            }
            //return Ok(oRespuesta);

            return Ok(oRespuesta);
        }

        /// <summary>Edita un elemento de la tabla Archivos a través de su campos Temporales, que es el Id del evento. El campo Temporales es obligatorio.</summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.Exception">no se encontró en base de datos el archico</exception>
        [HttpPut]
        public IActionResult Edit(Archivo model)
        {
            Respuesta<Archivo> oRespuesta = new Respuesta<Archivo>();

            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Archivos.Any(p => p.Temporales == model.Temporales) == true)
                    {

                        Archivo oArchivo = db.Archivos.Where(p => p.Temporales == model.Temporales).ToList().ElementAt(0);


                        if (model.Flyer != null)
                        {
                            oArchivo.Flyer = model.Flyer;
                            oArchivo.NombreFlyer = model.NombreFlyer;

                        }

                        if (model.Programa != null)
                        {
                            oArchivo.Programa = model.Programa;
                            oArchivo.NombrePrograma = model.NombrePrograma;

                        }
                        if (model.Portada != null)
                        {
                            oArchivo.Portada = model.Portada;
                            oArchivo.NombrePortada = model.NombrePortada;

                        }

                        db.Entry(oArchivo);

                        db.Archivos.Add(oArchivo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        db.SaveChanges();
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = oArchivo;
                    }
                    else {
                        logger.LogError("no se encontró en base de datos el archivo");

                        throw new Exception("no se encontró en base de datos el archivo");
                    }
                   
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en put archivos", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }


        /// <summary>Agrega un nuevo elemento a la tabla Archivos, el campo Temporales es obligatorio, es el Id del evento</summary>
        /// <param name="model">The model.</param>
        [HttpPost]
        public IActionResult Add(Archivo model)
        {


            Respuesta<Archivo> oRespuesta = new Respuesta<Archivo>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Archivo oArchivo = new Archivo();

                    if (model.Flyer != null)
                    {
                        oArchivo.NombreFlyer = model.NombreFlyer;

                        oArchivo.Flyer = model.Flyer;

                    }

                    if (model.Programa != null)
                    {
                        oArchivo.NombrePrograma = model.NombrePrograma;

                        oArchivo.Programa = model.Programa;

                    }

                    if (model.Portada != null)
                    {
                        oArchivo.NombrePortada = model.NombrePortada;

                        oArchivo.Portada = model.Portada;

                    }


                    oArchivo.Temporales = model.Temporales;


                    db.Archivos.Add(oArchivo);

                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = oArchivo;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en post archivos", ex);
                oRespuesta.Mensaje = ex.Message;
                oRespuesta.Data = null;

            }

            return Ok(oRespuesta);
        }


        /// <summary>Si se elimina el evento también se tienen que eliminar los archivos en Base de datos</summary>
        /// <param name="Id">The identifier.</param>
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Eventos.Any(x => x.Id == Id))
                    {
                        Evento oEvento = db.Eventos.Find(Id);


                        db.Remove(oEvento);
                        db.SaveChanges();
                        oRespuesta.Exito = 1;

                    }
                    else {
                        oRespuesta.Data = null;

                        oRespuesta.Exito = 0;

                        logger.LogError("Error en delete evento");

                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error en delete evento", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }


}
