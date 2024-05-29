using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosCeremonial.Data;

namespace EventosCeremonial.IRepository
{
    public interface IFileUpload
    {
        Task UploadAsyncNewTempFile(IFileListEntry file, string nombre, string tipo);
        Task borrarArchivosEnTemporales(string nombreTemporalArchivo, string tipo);
        Task GuardarArchivoEnTemporal(Byte[] file, string nombre);
        Task borrarArchivosEnArchivosIndex();
    }
}
