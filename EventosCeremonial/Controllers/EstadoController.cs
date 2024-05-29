using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;
using Microsoft.AspNetCore.Authorization;

namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Estado>> oRespuesta = new Respuesta<List<Estado>>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Estados.ToList();
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
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<Estado> oRespuesta = new Respuesta<Estado>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Estados.Find(Id);
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
        [HttpPost]
        public IActionResult Add(Estado model)
        {
            Respuesta<Estado> oRespuesta = new Respuesta<Estado>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Estado oEstado = new Estado();
                    oEstado.Nombre = model.Nombre;
                    oEstado.Activo = model.Activo;
                    db.Estados.Add(oEstado);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpPut]
        public IActionResult Edit(Estado model)
        {
            Respuesta<Estado> oRespuesta = new Respuesta<Estado>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Estado oEstado = db.Estados.Find(model.Id);
                    oEstado.Nombre = model.Nombre;
                    oEstado.Activo  = model.Activo;
                    db.Entry(oEstado);
                    db.Estados.Add(oEstado).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Estado> oRespuesta = new Respuesta<Estado>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Estado oEstado = db.Estados.Find(Id);
                    db.Remove(oEstado);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }
}
