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
    public class UbicacionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Ubicacion>> oRespuesta = new Respuesta<List<Ubicacion>>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Ubicacions.ToList();
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
            Respuesta<Ubicacion> oRespuesta = new Respuesta<Ubicacion>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Ubicacions.Find(Id);
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
        public IActionResult Add(Ubicacion model)
        {
            Respuesta<Ubicacion> oRespuesta = new Respuesta<Ubicacion>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Ubicacion oUbicacion = new Ubicacion();
                    oUbicacion.Domicilio = model.Domicilio;
                    oUbicacion.Provincia = model.Provincia;
                    oUbicacion.Localidad = model.Localidad;
                    db.Ubicacions.Add(oUbicacion);
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
        public IActionResult Edit(Ubicacion model)
        {
            Respuesta<Ubicacion> oRespuesta = new Respuesta<Ubicacion>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Ubicacion oUbicacion = db.Ubicacions.Find(model.Id);
                    oUbicacion.Domicilio = model.Domicilio;
                    oUbicacion.Provincia = model.Provincia;
                    oUbicacion.Localidad = model.Localidad; db.Entry(oUbicacion);
                    db.Ubicacions.Add(oUbicacion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
            Respuesta<Ubicacion> oRespuesta = new Respuesta<Ubicacion>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Ubicacion oUbicacion = db.Ubicacions.Find(Id);
                    db.Remove(oUbicacion);
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