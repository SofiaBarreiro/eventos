using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;

namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlataformaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Plataforma>> oRespuesta = new Respuesta<List<Plataforma>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Plataformas.ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error en get plataforma", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<Plataforma> oRespuesta = new Respuesta<Plataforma>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Plataformas.Find(Id);
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error en get plataforma x id", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpPost]
        public IActionResult Add(Plataforma model)
        {
            Respuesta<Plataforma> oRespuesta = new Respuesta<Plataforma>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Plataforma oPlataforma = new Plataforma();
                    oPlataforma.Nombre = model.Nombre;
                    db.Plataformas.Add(oPlataforma);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error en post plataforma", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpPut]
        public IActionResult Edit(Plataforma model)
        {
            Respuesta<Plataforma> oRespuesta = new Respuesta<Plataforma>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Plataforma oPlataforma = db.Plataformas.Find(model.Id);
                    oPlataforma.Nombre = model.Nombre;
                    db.Entry(oPlataforma);
                    db.Plataformas.Add(oPlataforma).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error en put plataforma", ex);

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Plataforma> oRespuesta = new Respuesta<Plataforma>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Plataforma oPlataforma = db.Plataformas.Find(Id);
                    db.Remove(oPlataforma);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error en delete plataforma", ex);

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
    }
}