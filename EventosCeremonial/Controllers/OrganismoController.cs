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
   

    public class OrganismoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Organismo>> oRespuesta = new Respuesta<List<Organismo>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Organismos.ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("error en get organismos", ex);

            }

            return Ok(oRespuesta);
        }
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<Organismo> oRespuesta = new Respuesta<Organismo>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Organismos.Find(Id);
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("error en get organismos  x id", ex);

            }

            return Ok(oRespuesta);
        }
        [HttpPost]
        
        public IActionResult Add(Organismo model)
        {
            Respuesta<Organismo> oRespuesta = new Respuesta<Organismo>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Organismo oOrganismo = new Organismo();
                    oOrganismo.Nombre = model.Nombre;
                    oOrganismo.IdPais = model.IdPais;
                    oOrganismo.MailContacto = model.MailContacto;
                    oOrganismo.TelefonoContacto = model.TelefonoContacto;
                    oOrganismo.TipoOrganismo = model.TipoOrganismo;
                    db.Organismos.Add(oOrganismo);


                    db.SaveChanges();
                    oRespuesta.Exito = 1;

                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en post organismos", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpPut]
        public IActionResult Edit(Organismo model)
        {
            Respuesta<Organismo> oRespuesta = new Respuesta<Organismo>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Organismo oOrganismo = db.Organismos.Find(model.Id);
                    oOrganismo.Nombre = model.Nombre;
                    oOrganismo.IdPais = model.IdPais;
                    oOrganismo.MailContacto = model.MailContacto;
                    oOrganismo.TelefonoContacto = model.TelefonoContacto;
                    oOrganismo.TipoOrganismo = model.TipoOrganismo;
                    db.Entry(oOrganismo);
                    db.Organismos.Add(oOrganismo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en put organismos", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Organismo> oRespuesta = new Respuesta<Organismo>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Organismo oOrganismo = db.Organismos.Find(Id);
                    db.Remove(oOrganismo);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en delete organismos", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }
}
