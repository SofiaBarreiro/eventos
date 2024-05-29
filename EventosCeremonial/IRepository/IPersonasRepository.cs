using EventosCeremonial.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosCeremonial.IRepository
{
    public interface IPersonasRepository
    {
        List<Persona> SaveExcel(string fileName);
    }
}
