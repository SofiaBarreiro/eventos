using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;
using EventosCeremonial.Helpers;
using MimeKit;
namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitacionController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Invitacion>> oRespuesta = new Respuesta<List<Invitacion>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var lst = db.Invitacions.ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {

                logger.LogError("get invitados", ex);
                oRespuesta.Mensaje = ex.Message;

            }

            return Ok(oRespuesta);
        }

        [HttpGet("ObtenerInvitado/{Id}")]
        public IActionResult ObtenerInvitado(int Id)
        {
            Respuesta<List<Invitacion>> oRespuesta = new Respuesta<List<Invitacion>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.Id == Id))
                    {
                        var lista = db.Invitacions.Where(x => x.Id == Id).ToList();

                        oRespuesta.Data = lista;
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


        [HttpGet("ObtenerInvitadoXEvento/{IdEvento}/{IdPersona}")]
        public IActionResult ObtenerInvitadoXEvento(int IdEvento, int IdPersona)
        {
            Respuesta<List<Invitacion>> oRespuesta = new Respuesta<List<Invitacion>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.IdPersona == IdPersona))
                    {
                        var lista = db.Invitacions.Where(x => x.IdPersona == IdPersona).ToList();
                        var persona = lista.Where(x => x.IdEvento == IdEvento).ToList();
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = persona;

                    }
                    else
                    {
                        oRespuesta.Exito = 0;

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("ObtenerInvitadoXEvento", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }



        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<List<Invitacion>> oRespuesta = new Respuesta<List<Invitacion>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.IdEvento == Id))
                    {

                        var lista = db.Invitacions.Where(x => x.IdEvento == Id).ToList();
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = lista;

                    }
                    else
                    {

                        oRespuesta.Exito = 0;

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("get id invitado", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }



        [HttpPost("EnviarCorreosAPendientes")]
        public IActionResult Enviar(MailRequest lista)
        {
            List<Persona> listaCorreos = lista.emailPersonas;
            Evento evento = lista.emailEvento;
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    int contador = 0;

                    if (db.Archivos.Any(x => x.Temporales == evento.Id))
                    {
                        var archivos = db.Archivos.Where(x => x.Temporales == evento.Id).ToList().ElementAt(0);

                        var nombreFlyer = archivos.NombreFlyer;

                        if (archivos != null)
                        {

                            foreach (var item in listaCorreos)
                            {
                                if (db.Invitacions.Any(x => (x.IdEvento == evento.Id && x.IdPersona == item.Id)) == true)
                                {
                                    var invitado = db.Invitacions.Where(x => (x.IdEvento == evento.Id && x.IdPersona == item.Id)).ToList();
                                    contador++;
                                    string localhost = this.HttpContext.Request.Host.ToString();
                                    string url = CorreoElectronico.crearUrlParaQR(evento, invitado.ElementAt(0), localhost, item.Id);
                                    if (url == "")
                                    {

                                        throw new Exception();

                                    }
                                    Ubicacion oUbicacion = db.Ubicacions.Find(evento.IdUbicacion);
                                    string domicilio = FuncionesBasicas.ExcepcionUbicacion(oUbicacion);
                                    try
                                    {
                                        var nombre = invitado.ElementAt(0).Id.ToString() + item.MailContacto;

                                        var mailMessage = FuncionesEmail.CrearCuerpoMensaje(nombre, evento, url, localhost, domicilio, nombreFlyer);
                                        mailMessage.To.Add(new MailboxAddress(item.Nombre, item.MailContacto));

                                        FuncionesEmail.Conectarse(mailMessage);
                                    }
                                    catch (Exception e)
                                    {
                                        logger.LogError("Error en  adjuntar email", e);
                                    }
                                }
                                else
                                {
                                    logger.LogError("no se encontro invitado" + item.Id);


                                }

                            }
                        }
                    }
                    else
                    {

                        logger.LogError("no se encontro dato en archivos" + evento.Id);


                    }

                }

            }
            catch (Exception e)
            {

                logger.LogError("error enviar correos a pendientes", e);
                return NotFound();
            }

            return Ok(lista);

        }

        [HttpPost("EnviarCorreosConfirmadosRecordatorio")]
        public IActionResult EnviarCorreosConfirmadosRecordatorio(MailRequest lista)
        {
            List<Persona> listaCorreos = lista.emailPersonas;
            Evento evento = lista.emailEvento;
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Archivos.Any(x => x.Temporales == evento.Id))
                    {

                        var archivos = db.Archivos.Where(x => x.Temporales == evento.Id).ToList().ElementAt(0);

                        if (archivos != null)
                        {
                            var nombreFlyer = archivos.NombreFlyer;

                            if (nombreFlyer != null)
                            {

                                int contador = 0;
                                foreach (var item in listaCorreos)
                                {
                                    if (db.Invitacions.Any(x => (x.IdEvento == evento.Id && x.IdPersona == item.Id)))
                                    {
                                        var invitado = db.Invitacions.Where(x => (x.IdEvento == evento.Id && x.IdPersona == item.Id)).ToList();
                                        contador++;

                                        string localhost = this.HttpContext.Request.Host.ToString();

                                        Ubicacion oUbicacion = new Ubicacion();

                                        oUbicacion = db.Ubicacions.Find(evento.IdUbicacion);


                                        string domicilio = FuncionesBasicas.ExcepcionUbicacion(oUbicacion);
                                        bool esExtranjero = false;
                                        try
                                        {
                                            if (item.TipoDocumento == "PASAPORTE" || item.NumeroDocumento == "")
                                            {
                                                     esExtranjero = true;//CORREGIR
                                            }


                                            var mailMessage = FuncionesEmail.CrearCuerpoMensajeConfirmacion(evento, localhost, domicilio, esExtranjero, invitado.ElementAt(0).Id, nombreFlyer, true);

                                            mailMessage.To.Add(new MailboxAddress(item.Nombre, item.MailContacto));

                                            FuncionesEmail.Conectarse(mailMessage);
                                            try
                                            {

                                                EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == invitado.ElementAt(0).Id).ToList().ElementAt(0);
                                                if (oEventoParticipante.EstadodeInscripcion == "Preinscripto" || oEventoParticipante.EstadodeInscripcion == "Preinscripto I.")
                                                {
                                                    oEventoParticipante.EstadodeInscripcion = "Aceptado";
                                                    db.Entry(oEventoParticipante);
                                                    db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                                    db.SaveChanges();
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                                logger.LogError("error en  EstadodeInscripcion Aceptado", ex);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            logger.LogError("Error en  adjuntar email", e);
                                        }


                                    }

                                }
                            }
                            else
                            {

                                logger.LogError("error enviar correos a confirmados, no se encontró archivos");

                            }


                        }
                    }
                    else
                    {
                        logger.LogError("error enviar correos a confirmados, no se encontró archivos");
                    }

                }

            }
            catch (Exception e)
            {
                logger.LogError("error enviar correos a confirmados", e);

                return NotFound();
            }

            return Ok(lista);

        }

        [HttpPost("EnviarCorreosConfirmados")]
        public IActionResult EnviarConfirmacion(MailRequest lista)
        {
            List<Persona> listaCorreos = lista.emailPersonas;
            Evento evento = lista.emailEvento;
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Archivos.Any(x => x.Temporales == evento.Id))
                    {

                        var archivos = db.Archivos.Where(x => x.Temporales == evento.Id).ToList().ElementAt(0);

                        if (archivos != null)
                        {
                            var nombreFlyer = archivos.NombreFlyer;

                            if (nombreFlyer != null)
                            {

                                int contador = 0;
                                foreach (var item in listaCorreos)
                                {
                                    if (db.Invitacions.Any(x => (x.IdEvento == evento.Id && x.IdPersona == item.Id)))
                                    {
                                        var invitado = db.Invitacions.Where(x => (x.IdEvento == evento.Id && x.IdPersona == item.Id)).ToList();
                                        contador++;

                                        string localhost = this.HttpContext.Request.Host.ToString();

                                        Ubicacion oUbicacion = new Ubicacion();

                                        oUbicacion = db.Ubicacions.Find(evento.IdUbicacion);


                                        string domicilio = FuncionesBasicas.ExcepcionUbicacion(oUbicacion);
                                        bool esExtranjero = false;
                                        try
                                        {
                                            if (item.TipoDocumento == "PASAPORTE" || item.NumeroDocumento == "")
                                            {

                                                //var listaUser = db.AspNetUserss;
                                                //var persona = db.AspNetUserss.Where(x => x.NormalizedEmail == item.MailContacto).ToList().ElementAt(0);
                                                esExtranjero = true;//CORREGIR

                                            }


                                            var mailMessage = FuncionesEmail.CrearCuerpoMensajeConfirmacion(evento, localhost, domicilio, esExtranjero, invitado.ElementAt(0).Id, nombreFlyer, false);

                                            mailMessage.To.Add(new MailboxAddress(item.Nombre, item.MailContacto));

                                            FuncionesEmail.Conectarse(mailMessage);
                                            try
                                            {

                                                EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == invitado.ElementAt(0).Id).ToList().ElementAt(0);
                                                if (oEventoParticipante.EstadodeInscripcion == "Preinscripto" || oEventoParticipante.EstadodeInscripcion == "Preinscripto I.")
                                                {
                                                    oEventoParticipante.EstadodeInscripcion = "Aceptado";
                                                    db.Entry(oEventoParticipante);
                                                    db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                                    db.SaveChanges();
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                                logger.LogError("error en  EstadodeInscripcion Aceptado", ex);
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            logger.LogError("Error en  adjuntar email", e);
                                        }


                                    }

                                }
                            }
                            else
                            {

                                logger.LogError("error enviar correos a confirmados, no se encontró archivos");

                            }


                        }
                    }
                    else
                    {
                        logger.LogError("error enviar correos a confirmados, no se encontró archivos");
                    }

                }

            }
            catch (Exception e)
            {
                logger.LogError("error enviar correos a confirmados", e);

                return NotFound();
            }

            return Ok(lista);

        }


        [HttpPost("EnviarCorreosCancelado")]
        public IActionResult EnviarCorreosCancelado(MailRequest lista)
        {
            List<Persona> listaCorreos = lista.emailPersonas;
            Evento evento = lista.emailEvento;
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    foreach (var item in listaCorreos)
                    {

                        try
                        {
                            var mailMessage = FuncionesEmail.CrearCuerpoMensajeCancelado(evento);
                            mailMessage.To.Add(new MailboxAddress(item.Nombre, item.MailContacto));
                            FuncionesEmail.Conectarse(mailMessage);
                        }
                        catch (Exception e)
                        {
                            logger.LogError("Error en  EnviarCorreosCancelado", e);
                        }
                    }

                }

            }
            catch (Exception e)
            {
                logger.LogError("error enviar correos a cancelados", e);

                return NotFound();
            }

            return Ok(lista);

        }



        [HttpPost]
        public IActionResult Add(Invitacion model)
        {
            //Invitacion oRespuesta = new Invitacion();
            LoggerManger logger = new LoggerManger();
            Respuesta<Invitacion> oRespuesta = new Respuesta<Invitacion>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.IdEvento == model.IdEvento && x.IdPersona == model.IdPersona))
                    {
                        logger.LogError("error post invitado ya esta invitado");
                        oRespuesta.Mensaje = "error post invitado ya esta invitado";
                        oRespuesta.Exito = 0;

                    }
                    else
                    {
                        Invitacion oInvitacion = new Invitacion();
                        oInvitacion.IdPersona = model.IdPersona;
                        oInvitacion.IdOrganismo = model.IdOrganismo;
                        oInvitacion.IdEvento = model.IdEvento;
                        oInvitacion.TipoInvitado = model.TipoInvitado;
                        oInvitacion.Qr = Guid.NewGuid();
                        db.Invitacions.Add(oInvitacion);
                        db.SaveChanges();
                        EventoParticipante oEventoParticipante = CrearEventoParticipante((int)oInvitacion.IdPersona, oInvitacion.Id);
                        db.EventoParticipantes.Add(oEventoParticipante);
                        db.SaveChanges();

                        oRespuesta.Exito = 1;
                        oRespuesta.Mensaje = oInvitacion.Id.ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError("error post invitado", ex);
                oRespuesta.Exito = 0;
            }

            return Ok(oRespuesta);
        }

        private EventoParticipante CrearEventoParticipante(int idParticipante, int IdInvitacion)
        {
            EventoParticipante oEventoParticipante = new EventoParticipante();
            LoggerManger logger = new LoggerManger();

            try
            {

                oEventoParticipante.IdPersona = (int)idParticipante;
                oEventoParticipante.IdInvitacion = IdInvitacion;
                oEventoParticipante.EstadodeInscripcion = "";
                oEventoParticipante.FechaHoraIngreso = new DateTime(1900, 01, 01);

            }
            catch (Exception ex)
            {
                logger.LogError("error CrearEventoParticipante", ex);
            }

            return oEventoParticipante;

        }
        [HttpPut]
        public IActionResult Edit(Invitacion model)
        {
            Respuesta<Invitacion> oRespuesta = new Respuesta<Invitacion>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.Id == model.Id))
                    {

                        Invitacion oInvitacion = db.Invitacions.Find(model.Id);
                        oInvitacion.IdPersona = model.IdPersona;
                        oInvitacion.IdOrganismo = model.IdOrganismo;
                        oInvitacion.IdEvento = model.IdEvento;
                        oInvitacion.Qr = Guid.NewGuid();
                        db.Entry(oInvitacion);
                        db.Invitacions.Add(oInvitacion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                        oRespuesta.Exito = 1;
                    }
                    else
                    {
                        oRespuesta.Exito = 0;

                        logger.LogError("error edit invitacion");

                    }


                }

            }
            catch (Exception ex)
            {
                logger.LogError("error edit invitacion", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Invitacion> oRespuesta = new Respuesta<Invitacion>();
            LoggerManger logger = new LoggerManger();

            using (EventosCeremonialContext db = new EventosCeremonialContext())
            {

                try
                {

                    if (db.Invitacions.Any(x => x.Id == Id))
                    {

                        Invitacion oInvitacion = db.Invitacions.Find(Id);
                        db.Remove(oInvitacion);
                        db.SaveChanges();
                        oRespuesta.Exito = 1;

                    }
                    else
                    {

                        oRespuesta.Exito = 0;

                        logger.LogError("error delete invitacion");
                    }

                }
                catch (Exception ex)
                {

                    logger.LogError("error delete invitacion", ex);
                    oRespuesta.Mensaje = ex.Message;
                }
            }
            return Ok(oRespuesta);
        }
        [HttpGet("/BorrarTodas")]

        public IActionResult Delete()
        {
            Respuesta<Invitacion> oRespuesta = new Respuesta<Invitacion>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    var ListaParaBorrar = db.Invitacions.ToList();
                    foreach (var item in ListaParaBorrar)
                    {
                        db.Invitacions.Remove(item);
                    }
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }

            }
            catch (Exception ex)
            {
                logger.LogError("error BorrarTodas", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

    }
}
