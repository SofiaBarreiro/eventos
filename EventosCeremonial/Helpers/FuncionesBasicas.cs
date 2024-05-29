using EventosCeremonial.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;


namespace EventosCeremonial.Helpers
{
    public class FuncionesBasicas
    {
        /// <summary>Obtiene los campos seteado en el appConfig.json</summary>
        public static IConfigurationRoot getAppSettings()
        {


            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            return builder.Build();

        }
        /// <summary>Uppers the case first character.</summary>
        /// <param name="s">The s.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static string UpperCaseFirstChar(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            s = s.ToLower();
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>transforma un string en un entero. Si es cero lo pasa a 1 para que no quede vacio. Se usa para las Ids de las listas ya que no existe el elemento con ID 0</summary>
        /// <param name="celda">valor de la celda de una tabla</param>
        public static int retornarDatoInt(Object celda)
        {
            int dato;
            if (celda != "")
            {

                int retorno = Convert.ToInt32(celda);
                if (retorno == 0)
                {
                    dato = 1;
                }
                else
                {

                    dato = retorno;
                }

            }
            else
            {
                dato = 1;



            }

            return dato;
        }

        /// <summary>devuelva la cantidad de veces que tiene que ir creciendo el porcentaje con cada iteracion</summary>
        /// <param name="count">la cantidad de veces que se tiene que dividir</param>
        /// <param name="porcentajeTotal">The porcentaje total.</param>
        public static float calcularPorcentajeXLista(float count, float porcentajeTotal)
        {

            return porcentajeTotal / count;

        }

        /// <summary>suma dos números flotantes y devuelve el porcentaje con un numero entero</summary>
        /// <param name="porcentajeInicial">The porcentaje inicial.</param>
        /// <param name="porcentajeXPersona">The porcentaje x persona.</param>
        /// <returns>
        /// el porcentaje en numeros enteros
        /// </returns>
        public static double acumularPorcentaje(double porcentajeInicial, float porcentajeXPersona)
        {


            porcentajeInicial = porcentajeInicial + porcentajeXPersona;

            return Math.Truncate(porcentajeInicial);
        }

        /// <summary>
        ///   <para>
        /// si la ubicacion del eventos fue eliminado o no se encuentra arma un string  on la palabra LUGAR A DERTEMINAR</para>
        /// </summary>
        /// <param name="oUbicacion">ubicacion.</param>
        public static string ExcepcionUbicacion(Ubicacion oUbicacion)
        {

            string domicilio = "LUGAR A DETERMINAR";

            if (oUbicacion != null)
            {

                domicilio = oUbicacion.Domicilio;


            }
            return domicilio;
        }

        /// <summary>verifica si una cadena de texto supera la cantidad de caracteres permitidos o no, pero antes de eso verifica qe no este vacia o con null</summary>
        /// <param name="cadena">The cadena.</param>
        /// <param name="longitud">The longitud.</param>
        public static bool corregirString(string cadena, int longitud)
        {

            if (cadena != "" && cadena != null)
            {
                cadena = cadena.ToString();
                if (cadena.Count() > longitud)
                {

                    return false;
                }
                else
                {

                    return true;

                }
            }
            return false;

        }

        /// <summary>Descompone una cadena que viene a través de un response y devuelve el Id que necesito para consultar otra tabla</summary>
        /// <param name="array">The array.</param>
        public static string RetornarIdDeResponse(string array)
        {

            string retorno = null;

            var array1 = array.Split("\"");
            var array2 = array1[5].Split("\"\"");

            bool encontro = Regex.IsMatch(array2[0], @"\d");
            if (encontro == true)
            {

                retorno = array2[0];
            }
            return retorno;
        }

        /// <summary>Arma un objeto de clase Invitado Solo guarda personas no ingresa Organismos</summary>
        /// <param name="participante">Objeto Persona</param>
        /// <param name="Id">ID del evento a que esta invitado</param>
        /// <returns>
        ///  Invitado
        /// </returns>
        public static Invitacion armarInvitadoPersona(Persona participante, int Id)
        {
            Invitacion oInvitacion = new Invitacion();
            oInvitacion.IdPersona = participante.Id;
            oInvitacion.IdEvento = Id;
            oInvitacion.TipoInvitado = "Persona";
            oInvitacion.IdOrganismo = participante.IdOrganismo;
            return oInvitacion;

        }

        /// <summary>Setea las variables mínimas para armar un Objeto de clase Persona. Se usa para usuarios nuevos de Portal que no fueron invitados a ningun evento</summary>
        /// <param name="item">Dos objetos de la clase Persona</param>
        /// <param name="personaAux">Un objeto igual al ingresado pero con menos campos seteados</param>
        public static Persona retonarPersonaAux(Persona item, Persona personaAux)
        {
            personaAux.Nombre = item.Nombre;
            personaAux.Apellido = item.Apellido;
            personaAux.TelefonoContacto = item.TelefonoContacto;

            return personaAux;

        }

        /// <summary>Reloads the page.</summary>
        /// <param name="uriHelper">The URI helper.</param>
        public static void reloadPage(NavigationManager uriHelper)
        {
            var timer = new Timer(new TimerCallback(_ =>
            {
                uriHelper.NavigateTo(uriHelper.Uri, forceLoad: true);
            }), null, 5000, 5000);
        }

        /// <summary>Verifica si hay preinscriptos.</summary>
        /// <param name="encontroPreinscripto">if set to <c>true</c> [encontro preinscripto].</param>
        public static bool hayPreinscriptos(bool encontroPreinscripto)
        {
            bool retorno = true;

            if (encontroPreinscripto == false)
            {
                retorno = false;
            }
            return retorno;
        }
        
        /// <summary>Hays the confirmados.</summary>
        /// <param name="encontroConfirmados">if set to <c>true</c> [encontro confirmados].</param>
        public static bool hayConfirmados(bool encontroConfirmados)
        {
            bool retorno = true;

            if (encontroConfirmados == false)
            {
                retorno = false;
            }
            return retorno;
        }


        /// <summary>Determines whether [is valid mail] [the specified emailaddress].</summary>
        /// <param name="emailaddress">The emailaddress.</param>
        /// <returns>
        ///   <c>true</c> if [is valid mail] [the specified emailaddress]; otherwise, <c>false</c>.</returns>
        public static bool IsValidMail(string emailaddress)
        {

            string email = emailaddress;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;


        }
        public static string[] recuperarDatosAgenda(string datos)
        {
            string[] cadena = null;
            if (datos != null)
            {
                cadena = datos.Split("\n");
            }
            return cadena;

        }
        public static string RecorrerFormatos(string formato)
        {
            string cadena = "";
            if (formato != null)
            {
                cadena = formato.Replace(";", ", ");
            }
            return cadena + ".";

        }
        public static bool HaveANumber(string texto)
        {
            if (texto.Any(c => char.IsDigit(c))){
                return true;
            }
            else {
                return false;
            }
        }
        public static bool InOnlyNumber(string texto) {

            var regex = new Regex("[0-9]");

            if (regex.IsMatch(texto))
            {
                return true;
            }
            return false;
        }
        
        /// <summary>Arma the object persona con los campos string que se trajeron de la tabla. Si esta vacio o null completa los campos de forma necesaria para que no haya errores</summary>
        /// <param name="array">The array.</param>
        public static Persona armarObjectPersona(string[] array)
        {

            Persona oPersona2 = new Persona();

            oPersona2.TelefonoContacto = array[3];
            oPersona2.Puesto = array[4];
            LoggerManger logger = new LoggerManger();

            if (array[0] == "")
            {

                oPersona2.Nombre = "ERROR"; //si estan incompletos esos campos se va a mostrar en la tabla de erores y no se va a guardar

            }
            else
            {

                oPersona2.Nombre = array[0];

            }
            if (array[1] == "")
            {

                oPersona2.Apellido = "ERROR";

            }
            else
            {

                oPersona2.Apellido = array[1];

            }
            if (array[2] == "")
            {

                oPersona2.MailContacto = "-";

            }
            else
            {
                if (IsValidMail(array[2]))
                {

                    oPersona2.MailContacto = array[2];
                }
                else
                {

                    oPersona2.MailContacto = "-";

                }



            }
            try {
                int valorIdOrg = FuncionesBasicas.retornarDatoInt(array[5]);

                //DEBO CORREGIR PARA QUE NO SEA UNO
                if (valorIdOrg == 1)
                {
                    oPersona2.IdOrganismo = 2;//DENTRO DE DESA Y QA EL UNO NO EXISTE ES 31 
                }
                else
                {
                    oPersona2.IdOrganismo = valorIdOrg;
                }
            }catch(Exception e) {

                logger.LogError("error en id organismo", e);


            }
            

            return oPersona2;
        }

    }
}
