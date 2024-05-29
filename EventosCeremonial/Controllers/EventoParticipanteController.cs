using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;
using Microsoft.AspNetCore.Components.Authorization;

namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoParticipanteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<EventoParticipante>> oRespuesta = new Respuesta<List<EventoParticipante>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    
                    var lst = db.EventoParticipantes.ToList();

                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error get eventoparticipante", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }


        [ActionName("AsistioInvitadoConDNI")]
        [HttpGet("AsistioInvitadoConDNI/{idPersona}/{IdPuntoAcceso}/{IdEvento}")]
        public IActionResult AsistioInvitadoConDNI(int IdInvitado, int IdPuntoAcceso, int? IdEvento)
        {
            Respuesta<string> oRespuesta = new Respuesta<string>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.Id == IdEvento))
                    {
                        var lst = db.Invitacions.Where(x => x.Id == IdEvento).ToList();

                        var idInvitado = lst.ElementAt(0);

                        if (lst.ElementAt(0) != null)
                        {


                            //string fechaHoy = DateTime.Now.ToShortDateString();

                            try
                            {
                                        if (db.Eventos.Any(x => x.Id == idInvitado.IdEvento))
                                        {
                                            var evento = db.Eventos.Where(x => x.Id == idInvitado.IdEvento).ToList().ElementAt(0);

                                            var fecha = evento.FechaHoraInicio.Value.ToShortDateString();


                                            //if (fecha == fechaHoy) DESCOMENTAR YA QUE NO SE PUEDE CONFIRMAR OTRO DIA
                                            //{
                                                if (db.EventoParticipantes.Any(p => p.IdInvitacion ==idInvitado.Id))
                                                {

                                                    EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitado.Id).ToList().ElementAt(0);
                                                    {
                                                        //if (oEventoParticipante.EstadodeInscripcion == "Aceptado")
                                                        //{
                                                            CambiarEstadoInvitacionAsistio(db, oEventoParticipante, IdPuntoAcceso);


                                                            oRespuesta.Mensaje = "Permitido";

                                                        //}
                                                        //else
                                                        //{
                                                        //    oRespuesta.Mensaje = "No fue aceptado su ingreso al evento";


                                                        //}

                                                    }
                                                }
                                               
                                            //}
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        logger.LogError("Error en AsistioInvitadoConDNI");
                                        oRespuesta.Mensaje = "No se encontro invitaciones para ese usuario";

                                    }
                        }
                        else
                        {
                            logger.LogError("No se encontro el id del usuario");
                            oRespuesta.Mensaje = "No se encontro invitaciones para ese usuario";

                        }

                    }
                    else
                    {

                        logger.LogError("No se encontro DNI");
                        oRespuesta.Mensaje = "No se encontro DNI";

                    }


                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                oRespuesta.Data = null;
            }

            return Ok(oRespuesta);
        }
        private void CambiarEstadoInvitacionAsistio(EventosCeremonialContext db, EventoParticipante oEventoParticipante, int IdPuntoAcceso)
        {
            LoggerManger logger = new LoggerManger();

            try
            {
                oEventoParticipante.EstadodeInscripcion = "Asistió";
                oEventoParticipante.ConfirmaAsistencia = "Sí";

                oEventoParticipante.IdPuntoAcceso = IdPuntoAcceso;
                oEventoParticipante.FechaHoraIngreso = DateTime.Now;

                db.Entry(oEventoParticipante);

                db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                db.SaveChanges();
            }
            catch (Exception e )
            {
                logger.LogError("CambiarEstadoInvitacionAsistio", e);

            }
          

        }
        [ActionName("AsistioInvitadoExtranjero")]
        [HttpGet("AsistioInvitadoExtranjero/{idInvitado}/{IdPuntoAcceso}")]

        public IActionResult AsistioInvitadoExtranjero(int idInvitado, int IdPuntoAcceso)
        {
            Respuesta<string> oRespuesta = new Respuesta<string>();

            LoggerManger logger = new LoggerManger();
            string mensaje = "";

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(x => x.Id == idInvitado))
                    {
                        var lstInvitaciones = db.Invitacions.Where(x => x.Id == idInvitado).ToList();
                        //x.FechaHoraInicio.Value.ToShortDateString() == fechaHoy

                        //string fechaHoy = DateTime.Now.ToShortDateString();

                        try
                        {
                            if (db.Eventos.Any(x => x.Id == lstInvitaciones.ElementAt(0).IdEvento))
                            {

                                var rta = db.Eventos.Where(x => x.Id == lstInvitaciones.ElementAt(0).IdEvento).ToList().ElementAt(0);

                                //var fechaEvento = rta.FechaHoraInicio.Value.ToShortDateString();
                                var fechaEvento = DateTime.Now.ToShortDateString();
                                //HAY QUE BORRAR ESTA LINEA


                                //if (fechaEvento == fechaHoy)
                                //{


                                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == lstInvitaciones.ElementAt(0).Id))
                                    {

                                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == lstInvitaciones.ElementAt(0).Id).ToList().ElementAt(0);
                                        CambiarEstadoInvitacionAsistio(db, oEventoParticipante, IdPuntoAcceso);
                                        if (db.Personas.Any(p => p.Id == lstInvitaciones.ElementAt(0).IdPersona))
                                        {

                                            Persona personaInvitada = db.Personas.Where(p => p.Id == lstInvitaciones.ElementAt(0).IdPersona).ToList().ElementAt(0);

                                            mensaje = "Ingreso aceptado. Para: " + personaInvitada.Nombre + " " + personaInvitada.Apellido;
                                            //oRespuesta.Mensaje = mensaje;
                                            oRespuesta.Data = mensaje;
                                            oRespuesta.Exito = 1;
                                        }
                                        else
                                        {

                                            logger.LogError("Error en IdInvitacion");
                                            oRespuesta.Mensaje = "Error en IdInvitacion";

                                            oRespuesta.Exito = 0;
                                        }

                                    }
                                    else
                                    {

                                        logger.LogError("Error en IdInvitacion");
                                        oRespuesta.Mensaje = "Error en IdInvitacion";

                                        oRespuesta.Exito = 0;

                                    }

                            }
                            else
                            {
                                logger.LogError("error en AsistioInvitadoExtranjero");
                                oRespuesta.Mensaje = "No se encontro invitaciones para ese usuario";
                                oRespuesta.Exito = 0;
                            }


                        }
                        catch (Exception e)
                        {
                            logger.LogError("error en AsistioInvitadoExtranjero");
                            oRespuesta.Mensaje = "No se encontro invitaciones para ese usuario";
                            oRespuesta.Exito = 1;
                            oRespuesta.Data = null;
                        }



                    }
                    else {
                        logger.LogError("error en AsistioInvitadoExtranjero");
                        oRespuesta.Mensaje = "No se encontro invitaciones para ese usuario";
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = null;

                    }
                   
                    
                }



            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                oRespuesta.Data = null;

            }


            return Ok(oRespuesta);
        }
        [HttpGet("{Id}")]
        //BUSCA POR EL ID DE LA INVITACION
        public IActionResult Get(int Id)
        {
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            LoggerManger logger = new LoggerManger();


            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == Id))
                    {
                        var lst = db.EventoParticipantes.Where(p => p.IdInvitacion == Id).ToList();

                        oRespuesta.Exito = 1;
                        oRespuesta.Data = lst.ElementAt(0);
                    }
                    else {
                        oRespuesta.Exito = 0;

                        oRespuesta.Data = null;
                    }

                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }



        //[HttpPost]
        //public IActionResult Add(EventoParticipante model)
        //{
        //    Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();

        //    try
        //    {
        //        using (EventosCeremonialContext db = new EventosCeremonialContext())
        //        {
        //            EventoParticipante oEventoParticipante = new EventoParticipante();
        //            oEventoParticipante.IdPersona = model.IdPersona;
        //            oEventoParticipante.IdInvitacion = model.IdInvitacion;
        //            oEventoParticipante.EstadodeInscripcion = model.EstadodeInscripcion;
        //            db.EventoParticipantes.Add(oEventoParticipante);
        //            db.SaveChanges();
        //            oRespuesta.Exito = 1;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oRespuesta.Mensaje = ex.Message;
        //    }

        //    return Ok(oRespuesta);
        //}

        [HttpGet("PasoAPendiente/{id}")]

        public IActionResult PasoAPendiente(int id)
        {
            LoggerManger logger = new LoggerManger();

            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(x => x.IdInvitacion == id).ToList().ElementAt(0);

                    if (oEventoParticipante.EstadodeInscripcion == "" || oEventoParticipante.EstadodeInscripcion == "Pendiente")
                    {
                        oEventoParticipante.EstadodeInscripcion = "Pendiente";
                        db.Entry(oEventoParticipante);
                        db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                        oRespuesta.Exito = 1;
                        oRespuesta.Data = oEventoParticipante;
                    }
                    else {

                        oRespuesta.Exito = 1;
                        oRespuesta.Mensaje = "El invitado ingresado ya se encuentra inscripto o ";

                    }


                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error en  PasoAPendiente");
                oRespuesta.Exito = 0;

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        
        [HttpGet("PasoAPreinscriptoInvitado/{id}")]
        public IActionResult PasoAPreinscriptoInvitado(int id)
        {
            LoggerManger logger = new LoggerManger();
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(x => x.IdInvitacion == id).ToList().ElementAt(0);

                    if (oEventoParticipante.EstadodeInscripcion == "Preinscripto I." || oEventoParticipante.EstadodeInscripcion == "Aceptado" || oEventoParticipante.EstadodeInscripcion == "No aceptado" || oEventoParticipante.EstadodeInscripcion == "Preinscripto") {

                        oRespuesta.Mensaje = "El invitado ingresado ya se encuentra inscripto en el evento.";
                        oRespuesta.Exito = 0;
                    }
                    else {
                        if (oEventoParticipante.EstadodeInscripcion == "Pendiente")
                        {
                            oRespuesta.Mensaje = "El correo ingresado ya se encuentra en la lista de invitado, debe ingresar a su casilla y confirmar su inscripción.";
                            oRespuesta.Exito = 0;
                        }
                        else {

                            if (oEventoParticipante.EstadodeInscripcion == "")
                            {
                                oEventoParticipante.EstadodeInscripcion = "Preinscripto I.";
                                db.Entry(oEventoParticipante);
                                db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                db.SaveChanges();
                                oRespuesta.Exito = 1;
                            }
                            else {
                                oRespuesta.Mensaje = "No se puedo inscribir a la persona en el evento.";
                                oRespuesta.Exito = 0;

                            }
                          
                        }

                    }

                    
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error en  PasoAPreinscriptoInvitado", ex);
                oRespuesta.Exito = 0;
                //oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

       

        [HttpPut("ConfirmaAsistencia/{idInvitacion}")]
        public IActionResult ConfirmaAsistencia(int idInvitacion)
        {
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion))
                    {
                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion).ToList().ElementAt(0);

                        if (oEventoParticipante.EstadodeInscripcion == "Pendiente")
                        {
                            oEventoParticipante.EstadodeInscripcion = "Preinscripto";
                            db.Entry(oEventoParticipante);
                            db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            oRespuesta.Exito = 1;
                        }
                       
                    }
                    else
                    {
                        logger.LogError("Error en  ConfirmaAsistencia");

                    }

                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error en  ConfirmaAsistencia", ex);

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
        [HttpPut("VolverAPreinscripto/{idInvitacion}")]

        public IActionResult VolverAPreinscripto(int idInvitacion)
        {
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion)) {

                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion).ToList().ElementAt(0);

                        if (oEventoParticipante.EstadodeInscripcion == "No aceptado")
                        {
                            oEventoParticipante.EstadodeInscripcion = "Preinscripto";

                            db.Entry(oEventoParticipante);
                            db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            oRespuesta.Exito = 1;
                        }
                    }
                    

                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("Error en  VolverAPreinscripto", ex);

            }

            return Ok(oRespuesta);
        }

        [HttpPut("AceptarParticipante/{idInvitacion}")]
        public IActionResult AceptarParticipante(int idInvitacion)
        {
            LoggerManger logger = new LoggerManger();

            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion)) {

                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion).ToList().ElementAt(0);

                        if (oEventoParticipante.EstadodeInscripcion == "Preinscripto" || oEventoParticipante.EstadodeInscripcion == "Preinscripto I.")
                        {

                            oEventoParticipante.EstadodeInscripcion = "Aceptado";
                            db.Entry(oEventoParticipante);
                            db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            logger.LogError("Error en  AceptarParticipante");

                        }
                    }
                    

                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en  aceptar pre inscripto", ex);
            }
            return Ok(oRespuesta);
        }

        [HttpPut("RechazarParticipante/{idInvitacion}")]
        public IActionResult RechazarParticipante(int idInvitacion)
        {
            LoggerManger logger = new LoggerManger();

            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion))
                    {
                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion).ToList().ElementAt(0);

                        if (oEventoParticipante.EstadodeInscripcion == "Preinscripto" || oEventoParticipante.EstadodeInscripcion == "Preinscripto I.")
                        {
                            oEventoParticipante.EstadodeInscripcion = "No aceptado";
                            db.Entry(oEventoParticipante);
                            db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            oRespuesta.Exito = 1;
                            oRespuesta.Data = oEventoParticipante;
                        }
                        else
                        {
                            oRespuesta.Exito = 0;
                        }
                    }
                    else {

                        logger.LogError("Error RechazarParticipante");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en  RechazarParticipante", ex);
            }
            return Ok(oRespuesta);
        }

            [HttpPut("AsistioInvitado/{idInvitacion}")]
            public IActionResult AsistioInvitado(int idInvitacion)
            {
                Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
                LoggerManger logger = new LoggerManger();

                try
                {
                    using (EventosCeremonialContext db = new EventosCeremonialContext())
                    {
                        if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion))
                        {

                            EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion).ToList().ElementAt(0);
                            {
                                if (oEventoParticipante.EstadodeInscripcion == "Aceptado")
                                {

                                    oEventoParticipante.EstadodeInscripcion = "Asistió";
                                    oEventoParticipante.ConfirmaAsistencia = "Sí";
                                    oEventoParticipante.IdPuntoAcceso = 111;//HAY QUE CORREGIR
                                    oEventoParticipante.FechaHoraIngreso = DateTime.Now;
                                    db.Entry(oEventoParticipante);
                                    db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    oRespuesta.Mensaje = "No fue aceptado su ingreso al evento";


                                }


                            }
                        }
                        else {

                            logger.LogError("Error AsistioInvitado");

                        }


                    }
                }
                catch (Exception ex)
                {
                    oRespuesta.Mensaje = ex.Message;
                }
                return Ok(oRespuesta);
            }

        [HttpPut("NoAsistioInvitado")]
        public IActionResult NoAsistioInvitado(Invitacion idInvitacion)
        {
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion.Id)) {
                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion.Id).ToList().ElementAt(0);
                        if (oEventoParticipante.EstadodeInscripcion == "Aceptado" || oEventoParticipante.EstadodeInscripcion == "Asistió")
                        {

                            oEventoParticipante.EstadodeInscripcion = "No asistió";
                            oEventoParticipante.ConfirmaAsistencia = "Sí";
                            db.Entry(oEventoParticipante);

                            db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            db.SaveChanges();
                            oRespuesta.Exito = 1;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en NoAsistioInvitado", ex);

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

        [HttpPut("AsistioInvitadoVirtual")]
        public IActionResult AsistioInvitadoVirtual(Invitacion idInvitacion)
        {
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.EventoParticipantes.Any(p => p.IdInvitacion == idInvitacion.Id))
                    {

                        EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == idInvitacion.Id).ToList().ElementAt(0);
                        {
                            if (oEventoParticipante.EstadodeInscripcion == "Aceptado" || oEventoParticipante.EstadodeInscripcion == "No asistió")
                            {

                                oEventoParticipante.EstadodeInscripcion = "Asistió";
                                oEventoParticipante.ConfirmaAsistencia = "Sí";
                                oEventoParticipante.IdPuntoAcceso = 0000;//HAY QUE CORREGIR
                                //oEventoParticipante.FechaHoraIngreso = DateTime.Now;
                                db.Entry(oEventoParticipante);
                                db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                oRespuesta.Mensaje = "No fue aceptado su ingreso al evento";
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("Error AsistioInvitado", ex);

            }
            return Ok(oRespuesta);
        }


        [HttpPut("BajaLogicaDeInvitaciones")]
        public IActionResult BajaLogicaDeInvitaciones(int idEvento)
        {
            Respuesta<EventoParticipante> oRespuesta = new Respuesta<EventoParticipante>();
            List<EventoParticipante> listaParticipantes = new List<EventoParticipante>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Invitacions.Any(p => p.IdEvento == idEvento)) {

                        List<Invitacion> oEventoInvitados = db.Invitacions.Where(p => p.IdEvento == idEvento).ToList();

                        List<EventoParticipante> oEventoParticipante = db.EventoParticipantes.Where(p => oEventoInvitados.Any(p2 => (p.EstadodeInscripcion != "Asistió") && (p2.Id == p.IdInvitacion))).ToList();
                        if (oEventoParticipante.Count() != 0)
                        {
                            foreach (var item in oEventoParticipante)
                            {

                                item.EstadodeInscripcion = "No asistió";
                                item.ConfirmaAsistencia = "Sí";
                                db.Entry(item);
                                db.EventoParticipantes.Add(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                            }

                            db.SaveChanges();
                            oRespuesta.Exito = 1;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en BajaLogicaDeInvitaciones", ex);

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }

    }
}
