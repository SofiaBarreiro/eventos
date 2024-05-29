using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EventosCeremonial.Helpers
{
    public static class HttpContextExtensions
    {

        //public static async Task InsertarParametrosPaginacionEnRespuesta<T>(this HttpContext context, IQueryable<T> queryable, int CantidadDeRegistrosAMostrar)
        //{

        //    if (context == null)
        //    {

        //        throw new ArgumentNullException(nameof(context));
        //    }

        //    double conteo = await queryable.CountAsync();
        //    double totalPaginas = Math.Ceiling(conteo/ CantidadDeRegistrosAMostrar);
        //    context.Response.Headers.Add("totalpaginas", totalPaginas.ToString());
        
        //}
    }
}
