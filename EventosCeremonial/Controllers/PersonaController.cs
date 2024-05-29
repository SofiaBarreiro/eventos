using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;
using EventosCeremonial.Helpers;


namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Persona>> oRespuesta = new Respuesta<List<Persona>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Personas.Take(100).ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("error en getPersonas", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    
                    var lst = db.Personas.Find(Id);
                    oRespuesta.Data = lst;
                    if (oRespuesta.Data == null)
                    {
                        oRespuesta.Exito = 0;
                    }
                    else
                    {

                        oRespuesta.Exito = 1;


                    }

                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en getPersonas x id", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [ActionName("BuscarDNI")]
        [HttpGet("BuscarDNI/{DNI}")]
        public IActionResult BuscarDNI(string dni)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Personas.Any(x => x.NumeroDocumento == dni))
                    {

                        var lst = db.Personas.Where(x => x.NumeroDocumento == dni).ToList();

                        oRespuesta.Exito = 1;

                        if (lst.Count() != 0)
                        {
                            oRespuesta.Data = lst.ElementAt(0);

                        }
                    }

                    else
                    {

                        oRespuesta.Data = null;
                    }

                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                oRespuesta.Data = null;
                logger.LogError("error en traer id", ex);

            }

            return Ok(oRespuesta);
        }

        [ActionName("TraerId")]
        [HttpGet("TraerId/{Mail}")]
        public IActionResult Get(string mail)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Personas.Any(x => x.MailContacto == mail)) {

                        var lst = db.Personas.Where(x => x.MailContacto == mail).ToList();

                        oRespuesta.Exito = 1;

                        if (lst.Count() != 0)
                        {
                            oRespuesta.Data = lst.ElementAt(0);

                        }
                    }
                   
                    else {

                        oRespuesta.Data = null;
                    }

                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                oRespuesta.Data = null;
                logger.LogError("error en traer id", ex);

            }

            return Ok(oRespuesta);
        }

        [HttpPost]
        public IActionResult Add(Persona model)
        {
            LoggerManger logger = new LoggerManger();
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            Persona oPersona = new Persona();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (model.Nombre != "") {
                        oPersona.Nombre = FuncionesBasicas.UpperCaseFirstChar(model.Nombre);
                    }
                    if (model.Apellido != "")
                    {
                        oPersona.Apellido = FuncionesBasicas.UpperCaseFirstChar(model.Apellido);
                    }

                    oPersona.MailContacto = model.MailContacto;
                    oPersona.NumeroDocumento = model.NumeroDocumento;
                    oPersona.TipoDocumento = model.TipoDocumento;
                    oPersona.Puesto = model.Puesto;
                    int idOrganismo = db.Organismos.Where(x => x.Id == model.IdOrganismo).ToList().ElementAt(0).Id;

                    if (idOrganismo != 0)
                    {
                        oPersona.IdOrganismo = model.IdOrganismo;

                    }
                    else {

                        oPersona.IdOrganismo = 2;

                    }

                    db.Personas.Add(oPersona);
                    db.SaveChanges();

                    oRespuesta.Exito = 1;
                   
                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en traer post persona", ex);
                oRespuesta.Exito = 0;

            }

            return Ok(oRespuesta);
        }
       
     
        [ActionName("AgregarPersonaInscripta")]
        [HttpPost("AgregarPersonaInscripta")]
        public IActionResult AgregarPersonaInscripta(Persona model)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Persona oPersona = new Persona();



                    if (model.Nombre != "")
                    {
                        oPersona.Nombre = FuncionesBasicas.UpperCaseFirstChar(model.Nombre);
                    }
                    if (model.Apellido != "")
                    {
                        oPersona.Apellido = FuncionesBasicas.UpperCaseFirstChar(model.Apellido);
                    }



                    oPersona.MailContacto = model.MailContacto;
                    oPersona.NumeroDocumento = model.NumeroDocumento;
                    oPersona.TipoDocumento = model.TipoDocumento;
                    oPersona.IdOrganismo = model.IdOrganismo;
                    db.Personas.Add(oPersona);
                    db.SaveChanges();
                    oRespuesta.Mensaje = oPersona.Id.ToString();

                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en traer post persona", ex);
                oRespuesta = null;

            }

            return Ok(oRespuesta);
        }

        [ActionName("AgregarPersonaIndex")]
        [HttpPost("AgregarPersonaIndex")]

        public IActionResult AgregarPersonaIndex(Persona model)
        {
            Persona oRespuesta = new Persona();
            LoggerManger logger = new LoggerManger();



            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Persona oPersona = new Persona();
                    oPersona.Nombre = model.Nombre;
                    oPersona.Apellido = model.Apellido;
                    oPersona.NumeroDocumento = model.NumeroDocumento;
                    oPersona.TipoDocumento = model.TipoDocumento;


                    oPersona.MailContacto = model.MailContacto;

                    var idOgranismo = db.Organismos.Where( x => x.Nombre == "SIN ORGANISMO").ToList().ElementAt(0).Id;

                    if (idOgranismo != null)
                    {

                        oPersona.IdOrganismo = idOgranismo;

                    }
                    else {

                        oPersona.IdOrganismo = 2;

                    }


                    db.Personas.Add(oPersona);

                    db.SaveChanges();


                    oRespuesta = oPersona;
                }

            }
            catch (Exception ex)
            {

                logger.LogError("error en AgregarPersonaIndex", ex);
                //oRespuesta.Mensaje = ex.Message;
                oRespuesta = null;

            }

            return Ok(oRespuesta);
        }

       
        [HttpPut("EditarDNI")]
       public IActionResult EditarDNI(Persona model)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                   
                    Persona oPersona = db.Personas.Find(model.Id);


                    if (oPersona != null)
                    {
                        oPersona.NumeroDocumento = model.NumeroDocumento;
                        oPersona.TipoDocumento = model.TipoDocumento;

                        if (model.MailContacto != oPersona.MailContacto)
                        {

                            oPersona.MailContacto = model.MailContacto;

                        }

                        db.Entry(oPersona);
                        db.Personas.Add(oPersona).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                        oRespuesta.Exito = 1;
                    }
                    else {

                        logger.LogError("error en EditarDNI");

                    }


                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en EditarDNI", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpGet("TraerPersonasInvitadas/{Id}")]
        public IActionResult TraerPersonasInvitadas(int Id)
        {
            Respuesta<List<Invitacion>> oRespuesta = new Respuesta<List<Invitacion>>();
            List<Invitacion> personas = new List<Invitacion>();
            Persona aux = new Persona();

            LoggerManger logger = new LoggerManger();
            try
            {

                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.IdEvento == Id))
                    {
                        var lista = db.Invitacions.Where(x => x.IdEvento == Id).ToList();

                        oRespuesta.Data = lista.ToList();

                        //foreach (var invitado in lista)
                        //{

                        //    var eventoParticipanteItem = db.EventoParticipantes.Where(x => x.IdInvitacion == invitado.Id).ToList().ElementAt(0); ;
                        //    if (eventoParticipanteItem != null)
                        //    {

                        //        if (eventoParticipanteItem.EstadodeInscripcion != "" && eventoParticipanteItem.EstadodeInscripcion != "No aceptado")
                        //        {
                        //            //personas.Add(invitado);

                        //            //var itemPersona = db.Personas.Where(x => x.Id == invitado.IdPersona).ToList();
                        //            //if (itemPersona.MailContacto != null)
                        //            //{
                        //            //    aux = itemPersona;
                        //            //    personas.Add(aux);


                        //            //}
                        //        }


                        //    }

                        //}
                        oRespuesta.Exito = 1;

                    }
                    else
                    {

                        oRespuesta.Exito = 0;
                    }
                }
            }
            catch (Exception ex)
            {

                logger.LogError("ObtenerInvitado", ex);
                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }
       
        [HttpPut("EditarPersonaImport")]
        public IActionResult EditPersona(Persona model)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Persona oPersona = db.Personas.Find(model.Id);
                    if (oPersona != null)
                    {
                        
                        oPersona.TelefonoContacto = model.TelefonoContacto;
                        oPersona.Puesto = model.Puesto;
                        oPersona.IdOrganismo = model.IdOrganismo;
                        //ESTO NO SE PUEDE EDITAR JAMAS CAMBIA


                        if (model.Nombre != "")
                        {
                            oPersona.Nombre = FuncionesBasicas.UpperCaseFirstChar(model.Nombre);
                        }
                        if (model.Apellido != "")
                        {
                            oPersona.Apellido = FuncionesBasicas.UpperCaseFirstChar(model.Apellido);
                        }
                        oPersona.MailContacto = model.MailContacto;
                        oPersona.NumeroDocumento = model.NumeroDocumento;
                        oPersona.TipoDocumento = model.TipoDocumento;

                        db.Entry(oPersona);
                        db.Personas.Add(oPersona).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                        oRespuesta.Exito = 1;
                    }
                    else {


                        logger.LogError("error en EditarPersonaImport");
                        oRespuesta.Mensaje = "oPersona es null";
                    }

                }

            }
            catch (Exception ex)
            {
                logger.LogError("error en EditarPersonaImport", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }


        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Persona> oRespuesta = new Respuesta<Persona>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Persona oPersona = db.Personas.Find(Id);
                    
                    db.Remove(oPersona);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("error en delete persona", ex);

            }

            return Ok(oRespuesta);
        }

       
        [HttpGet("TraerEvento/{Id}")]
        public IActionResult EnviarCorreo(int Id)
        {
            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {


                    var lst = db.Eventos.Find(Id);
                    if (lst == null)
                    {

                        oRespuesta.Exito = 0;
                        logger.LogError("error en TraerEvento");


                    }
                    else
                    {
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = lst;

                    }



                }
            }
            catch (Exception e)
            {
                logger.LogError("error en TraerEvento", e);

            }
            return Ok(oRespuesta);

        }




    }
}
