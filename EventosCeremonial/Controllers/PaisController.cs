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
    public class PaisController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Pai>> oRespuesta = new Respuesta<List<Pai>>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Pais.ToList();
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
            Respuesta<Pai> oRespuesta = new Respuesta<Pai>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Pais.Find(Id);
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
        public IActionResult Add(Pai model)
        {
            Respuesta<Pai> oRespuesta = new Respuesta<Pai>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Pai oPais = new Pai();
                    oPais.Nombre = model.Nombre;
                    oPais.Cultura = model.Cultura;
                    db.Pais.Add(oPais);
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
        public IActionResult Edit(Pai model)
        {
            Respuesta<Pai> oRespuesta = new Respuesta<Pai>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Pai oPais = db.Pais.Find(model.Id);
                    oPais.Nombre = model.Nombre;
                    oPais.Cultura = model.Cultura;
                    db.Entry(oPais);
                    db.Pais.Add(oPais).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
            Respuesta<Pai> oRespuesta = new Respuesta<Pai>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Pai oPais = db.Pais.Find(Id);
                    db.Remove(oPais);
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
