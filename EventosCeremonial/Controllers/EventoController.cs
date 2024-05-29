using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using EventosCeremonial.Data.Response;
using EventosCeremonial.Data;
using EventosCeremonial.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace EventosCeremonial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {

        /// <summary>Obtiene los elementos de la lista de eventos ordenados por fechas.</summary>
        /// <param name="OrdenarXFechas">No recibe nada, se agrego parametro para que interfiera con otro metodo</param>
        /// <returns>devuelve los primeros tres elementos<br /></returns>
        /// 
        [HttpGet("OrdenarXFechas")]
        public IActionResult OrdenarXFechas()
        {
            Respuesta<List<Evento>> oRespuesta = new Respuesta<List<Evento>>();
            LoggerManger logger = new LoggerManger();
            try
            {

                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Eventos.Count() > 0)
                    {
                        if (db.Eventos.Any(p => p.FechaHoraInicio.HasValue))
                        {
                            var lst = db.Eventos.Where(p => p.FechaHoraInicio.HasValue).OrderBy(p => p.FechaHoraInicio.Value).ToList();

                            DateTime today = DateTime.Now;
                            var lstAux = lst.ToList();

                            foreach (var item in lstAux.ToList())
                            {

                                if (item != null)
                                {

                                    if (item.FechaHoraFin < DateTime.Now)
                                    {
                                        lstAux.Remove(item);
                                    }
                                    else
                                    {
                                        if (item.IdEstado != 1)
                                        {
                                            lstAux.Remove(item);

                                        }
                                    }

                                }
                                else
                                {

                                    lstAux.Remove(item);


                                }


                            }

                            var listaRetorno = new List<Evento>();

                            if (lstAux.ElementAt(0) != null)
                            {

                                listaRetorno.Add(lstAux.ElementAt(0));

                            }

                            if (lstAux.Count() > 1)
                            {

                                listaRetorno.Add(lstAux.ElementAt(1));

                            }
                            if (lstAux.Count() > 2)
                            {

                                listaRetorno.Add(lstAux.ElementAt(2));

                            }


                            oRespuesta.Exito = 1;
                            oRespuesta.Data = listaRetorno;
                        }
                        if (db.Eventos.Any(p => p.FechaHoraInicio.HasValue))
                        {

                            var lst = db.Eventos.Where(p => p.FechaHoraInicio.HasValue).OrderBy(p => p.FechaHoraInicio.Value).ToList();

                            DateTime today = DateTime.Now;
                            var lstAux = lst.ToList();

                            foreach (var item in lstAux.ToList())
                            {

                                if (item != null)
                                {

                                    if (item.FechaHoraFin < DateTime.Now)
                                    {
                                        lstAux.Remove(item);
                                    }
                                    else
                                    {
                                        if (item.IdEstado != 1)
                                        {
                                            lstAux.Remove(item);

                                        }
                                    }

                                }
                                else
                                {

                                    lstAux.Remove(item);


                                }


                            }

                            var listaRetorno = new List<Evento>();

                            if (lstAux.ElementAt(0) != null)
                            {

                                listaRetorno.Add(lstAux.ElementAt(0));

                            }

                            if (lstAux.Count() > 1)
                            {

                                listaRetorno.Add(lstAux.ElementAt(1));

                            }
                            if (lstAux.Count() > 2)
                            {

                                listaRetorno.Add(lstAux.ElementAt(2));

                            }


                            oRespuesta.Exito = 1;
                            oRespuesta.Data = listaRetorno;
                        }
                    }
                    else {


                        oRespuesta.Data = null;
                        oRespuesta.Exito = 0;
                    
                    }
                }
            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
                logger.LogError("Error en OrdenarXFechas", ex);

            }

            return Ok(oRespuesta);
        }
        [HttpGet("EventosFinalizados")]

        public IActionResult TraerEventosPasados(int pagina)
        {
            Respuesta<List<Evento>> oRespuesta = new Respuesta<List<Evento>>();
            LoggerManger logger = new LoggerManger();


            try
            {

                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Eventos.Count() > 0)//si la tabla eventos tiene elementos
                    {
                      
                        //busca los quee stan cerrados
                        var lst = db.Eventos.Where(p => p.IdEstado != 1).OrderByDescending(p => p.FechaHoraInicio.Value).Take(100);


                        oRespuesta.Data = lst.ToList();
                        oRespuesta.Exito = 1;

                    }
                    else
                    {

                        oRespuesta.Exito = 0;
                        oRespuesta.Data = null;

                    }

                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("EventosFinalizados", ex);

            }

            return Ok(oRespuesta);
        }

        /// <summary>Devuelve los ultimos 6 eventos que finalizaron de la lista de eventos</summary>
        /// <returns>
        /// los ultimos 6 eventos cerrados
        /// </returns>
        [HttpGet("EventosAnteriores")]
       
        public  IActionResult TraerEventosAnteriores()
        {
            Respuesta<List<Evento>> oRespuesta = new Respuesta<List<Evento>>();
            LoggerManger logger = new LoggerManger();


            try
            {

                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    if (db.Eventos.Count() > 0)//si la tabla eventos tiene elementos
                    {

                        //busca los quee stan cerrados
                        var lst = db.Eventos.Where(p => p.IdEstado == 2).OrderBy(p => p.FechaHoraInicio.Value).ToList();

                        var listaRetorno = new List<Evento>();
                        //va a agregando eventos si no esta null
                        if (lst.ElementAt(0) != null)
                        {

                            listaRetorno.Add(lst.ElementAt(0));

                        }

                        if (lst.Count() > 1)
                        {

                            listaRetorno.Add(lst.ElementAt(1));

                        }
                        if (lst.Count() > 2)
                        {

                            listaRetorno.Add(lst.ElementAt(2));

                        }
                        if (lst.Count() > 3)
                        {

                            listaRetorno.Add(lst.ElementAt(3));

                        }
                        if (lst.Count() > 4)
                        {

                            listaRetorno.Add(lst.ElementAt(4));

                        }
                        if (lst.Count() > 5)
                        {

                            listaRetorno.Add(lst.ElementAt(5));

                        }


                        oRespuesta.Exito = 1;
                        oRespuesta.Data = lst;
                    }
                    else
                    {

                        oRespuesta.Exito = 0;
                        oRespuesta.Data = null;

                    }
                   
                }
            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("EventosAnteriores", ex);

            }

            return Ok(oRespuesta);
        }

        /// <summary>Trae todos los eventos que se encuentran abiertos ordenados por fecha de inico mas cercana</summary>
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta<List<Evento>> oRespuesta = new Respuesta<List<Evento>>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    var lst = db.Eventos.Where(p => p.IdEstado == 1).OrderBy(p => p.FechaHoraInicio.Value).ToList();

                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en get eventos", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

        /// <summary>trae solos los eventos activos ordenados por fecha y pero no los que ya sucedieron pero no estan cerrados</summary>
        [HttpGet("TraerSoloActivos")]
        //[Authorize]
        public IActionResult TraerSoloActivos()
        {
            Respuesta<List<Evento>> oRespuesta = new Respuesta<List<Evento>>();
            LoggerManger logger = new LoggerManger();
          
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    var lst = db.Eventos.Where(p => p.IdEstado == 1).OrderBy(p => p.FechaHoraInicio.Value).ToList();

                    DateTime today = DateTime.Now;
                    var lstAux = lst.ToList();

                    foreach (var item in lstAux.ToList())
                    {

                        if (item != null)
                        {

                            if (item.FechaHoraFin < DateTime.Now)
                            {
                                lstAux.Remove(item);
                            }
                            

                        }
                        else
                        {
                            lstAux.Remove(item);
                        }


                    }

                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lstAux;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en get eventos", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }


        /// <summary>A través del id de la Persona, busca las invitaciones que tiene y consulta cual es la mas cercana a realizarse 
        /// y si se encuentra activa</summary>
        /// <param name="IdPersona">The identifier persona.</param>
        [HttpGet("TraerEventoInvitado/{IdPersona}")]
        public IActionResult TraerEventoInvitado(int IdPersona)
        {
            Respuesta<int> oRespuesta = new Respuesta<int>();
            LoggerManger logger = new LoggerManger();
            List<Evento> eventoActivosDelUsuario = new List<Evento>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    if (db.Invitacions.Any(p => p.IdPersona == IdPersona))
                    {

                        var lst = db.Invitacions.Where(x => x.IdPersona == IdPersona);


                        foreach (Invitacion item in lst.ToList())
                        {

                            if (item.Id != 0)
                            {
                                var estadoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == item.Id).ToList().ElementAt(0);

                                if (estadoParticipante.EstadodeInscripcion == "Pendiente")
                                {

                                    if (db.Eventos.Any(p => p.IdEstado == 1 && item.IdEvento == p.Id))
                                    {
                                        eventoActivosDelUsuario.Add(db.Eventos.Where(p => p.IdEstado == 1 && item.IdEvento == p.Id).ToList().ElementAt(0));

                                    }


                                }


                            }
                           
                    }
                }
                  
                }
                if (eventoActivosDelUsuario.Count() > 0)
                {
                    eventoActivosDelUsuario.OrderBy(p => p.FechaHoraInicio.Value);
                    var  retorno = eventoActivosDelUsuario.ElementAt(0);

                    oRespuesta.Data = retorno.Id;
                    oRespuesta.Exito = 1;
                }
                else
                {
                    logger.LogError("no encontro invitacion");

                    oRespuesta.Exito = 0;

                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en get eventos", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }



        /// <summary>A través de un id de evento trae el elemento de la lista persona</summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {

                    var lst = db.Eventos.Find(Id);
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
                logger.LogError("Error en get evento", ex);

            }

            return Ok(oRespuesta);
        }


        /// <summary>
        /// Agrega un elemento nuevo a la lista de eventos
        /// </summary>
        /// <param name="model">Objeto evento</param>
        /// <returns>
        /// El elemento agregado
        /// </returns>
        [HttpPost]
        public IActionResult Add(Evento model)
        {

            LoggerManger logger = new LoggerManger();

            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Evento oEvento = new Evento();
                    oEvento.Nombre = model.Nombre;
                    oEvento.Descripcion = model.Descripcion;
                    oEvento.Formato = model.Formato;
                    oEvento.URL = model.URL;
                    oEvento.IdReunionVirtual = model.IdReunionVirtual;
                    oEvento.IdPlataforma = model.IdPlataforma;
                    oEvento.IdUbicacion = model.IdUbicacion;
                    oEvento.Agenda = model.Agenda;
                    oEvento.Password = model.Password;
                    oEvento.RutaPrograma = model.RutaPrograma;
                    oEvento.FechaHoraInicio = model.FechaHoraInicio;
                    oEvento.FechaHoraFin = model.FechaHoraFin;
                    oEvento.TipoEvento = model.TipoEvento;
                    if (model.Cupo > 0)
                    {
                        oEvento.Cupo = model.Cupo;
                    }
                    else
                    {
                        oEvento.Cupo = 0;//SE COMPLETA POR DEFECTO CUPO CERO YA QUE NO ES UN CAMPO OBLIGATORIO EN EL FORM PERO SI PARA LA TABLA
                    }
                    oEvento.Invitacions = model.Invitacions;
                    oEvento.IdEstado = 1;// model.IdEstado;
                    oEvento.GUID = Guid.NewGuid();
                    db.Eventos.Add(oEvento);

                    db.SaveChanges();


                    oRespuesta.Exito = 1;
                    oRespuesta.Data = oEvento;
                    oRespuesta.Mensaje = oEvento.Id.ToString();

                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en post evento", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }


        /// <summary>Busca dentro de la lista de eventos, todos los que tienen fecha posterior a la fecha del dia de la fecha y si están en estado abierto los pasa a cerrados</summary>
        [HttpGet("PasarAFinalizados")]
        public IActionResult PasarAFinalizados()
        {
            LoggerManger logger = new LoggerManger();
            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();
            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    DateTime today = DateTime.Now;
                    if (db.Eventos.Count() > 0) {

                        if (db.Eventos.Any(p => p.IdEstado == 1 && p.FechaHoraFin < DateTime.Now))//BUSCA   LOS EVENTOS ABIEROS CON FECHA MENOR A HOY
                        {//SIEMPRE DEBE FIJARSE QUE HAYA ELEMENTOS DENTRO DE LA TABLA DE LO CONTRARIO SI NO HAY PORQUE LA TABLA ES NUEVO VA A LANZAR UN ERROR 

                            var lst = db.Eventos.Where(p => p.IdEstado == 1 && p.FechaHoraFin < DateTime.Now).ToList();

                            foreach (var item in lst.ToList())
                            {
                                if (item != null)
                                {
                                    item.IdEstado = 2;//SIGNIFCA QUE EL ESTADO DEL EVENTO ES CERRADO
                                }

                                db.Entry(item);
                                db.Eventos.Add(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                db.SaveChanges();
                                oRespuesta.Exito = 1;

                            }

                        }
                        else
                        {
                            
                            oRespuesta.Exito = 0;

                        }
                    }
                    else
                    {
                        oRespuesta.Data = null;
                        oRespuesta.Exito = 0;

                    }


                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en edit PasarAFinalizados", ex);
            }
            return Ok(oRespuesta);

        }


        /// <summary>Busca y edita el evento al que se pasa por parámetro
        /// Si se pasa el evento a cerrado  a un evento Presencial se modifica tambien a los invitados que no asistieron
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        [HttpPut("AgregarNuevoEvento")]
        public IActionResult Edit(Evento model)
        {
            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    Evento oEvento = db.Eventos.Find(model.Id);
                    oEvento.Nombre = model.Nombre;
                    oEvento.Descripcion = model.Descripcion;
                    oEvento.Formato = model.Formato;
                    oEvento.URL = model.URL;
                    oEvento.IdReunionVirtual = model.IdReunionVirtual;
                    oEvento.IdPlataforma = model.IdPlataforma;
                    oEvento.IdUbicacion = model.IdUbicacion;
                    oEvento.Agenda = model.Agenda;
                    oEvento.Password = model.Password;
                    oEvento.RutaPrograma = model.RutaPrograma;
                    //oEvento.FlyerEvento = model.FlyerEvento;
                    oEvento.FechaHoraInicio = model.FechaHoraInicio;
                    oEvento.FechaHoraFin = model.FechaHoraFin;
                    oEvento.TipoEvento = model.TipoEvento;
                    oEvento.Cupo = model.Cupo;
                    oEvento.IdEstado = model.IdEstado;

                    //SI EL EVENTO ES CANCELADO PASA A IDESTADO 3 

                    if (model.IdEstado == 2 && model.Formato.Contains("Presencial"))//VERIFICA QUE ESTE CERRADO EL EVENTO Y QUE SEA PRESENCIAL
                    {
                        //SI SE CERRO EL EVENTO PASA A NO ASISTIO A LOS QUE NO FUERON

                        var lstInvitaciones = model.Invitacions;

                        if (lstInvitaciones.Count() > 0)
                        {
                            foreach (var item in lstInvitaciones)
                            {
                                EventoParticipante oEventoParticipante = db.EventoParticipantes.Where(p => p.IdInvitacion == item.Id).ToList().ElementAt(0);
                                if (oEventoParticipante.EstadodeInscripcion != "Asistió")
                                {
                                    oEventoParticipante.EstadodeInscripcion = "No Asistió";
                                    db.Entry(oEventoParticipante);//BUSCA TODOS LOS INVITADOS DEL EVENTOS Y SI NO FUERON LOS PASA A NO ASISTIO YA QUE EL EVENTO FUE CERRADO
                                    db.EventoParticipantes.Add(oEventoParticipante).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    oEvento.Invitacions = model.Invitacions;
                    db.Entry(oEvento);
                    db.Eventos.Add(oEvento).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    oRespuesta.Exito = 1;

                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error en post AgregarNuevoEvento", ex);
                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }

        /// <summary>Elimina un evento de la Lista Eventos, también borra sus invitados y los elementos de EventosParticipante correpondientes</summary>
        /// <param name="Id">El id del evento a eliminar</param>
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta<Evento> oRespuesta = new Respuesta<Evento>();
            LoggerManger logger = new LoggerManger();

            try
            {
                using (EventosCeremonialContext db = new EventosCeremonialContext())
                {
                    oRespuesta.Exito = 1;
                    Evento oEvento = db.Eventos.Find(Id);
                    if (oEvento != null)
                    {
                        try
                        {
                            if (db.Invitacions.Any(p => p.IdEvento == Id))
                            {

                                var listaInvitados = db.Invitacions.Where(p => p.IdEvento == Id).ToList();

                                foreach (var item in listaInvitados)
                                {
                                    try
                                    {
                                        try
                                        {
                                            if (db.EventoParticipantes.Any(p => p.IdInvitacion == item.Id))
                                            {
                                                var itemEP = db.EventoParticipantes.Where(p => p.IdInvitacion == item.Id).ToList().ElementAt(0);
                                                db.Remove(itemEP);//REMUEVE EL EVENTOPARTICIPANTE
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                            logger.LogError("Error en delete EventoParticipantes", e);
                                            oRespuesta.Exito = 0;

                                        }
                                        db.Remove(item);//REMUEVE EL INVITADO

                                    }
                                    catch (Exception e)
                                    {

                                        logger.LogError("Error en delete listaInvitados", e);
                                        oRespuesta.Exito = 0;

                                    }

                                }

                            }

                            db.Remove(oEvento);

                        }
                        catch (Exception e)

                        {
                            logger.LogError("Error en delete evento", e);
                            oRespuesta.Exito = 0;

                        }
                        try
                        {
                            if (db.Archivos.Any(p => p.Temporales == Id) == true)
                            {
                                var lst = db.Archivos.Where(p => p.Temporales == Id).ToList().ElementAt(0);
                                db.Remove(lst);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.LogError("Error en delete archivos del evento", e);
                            oRespuesta.Exito = 0;
                        }
                        db.SaveChanges();
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
