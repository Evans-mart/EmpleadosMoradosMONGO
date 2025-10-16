// Data/CatalogosDataAccess.cs (NUEVA CLASE)
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmpleadosMorados.Data
{
    // Clase para manejar las llamadas a catálogos, delegando a MongoDBDataAccess
    public class CatalogosDataAccess
    {
        private readonly MongoDBDataAccess _mongoData;

        public CatalogosDataAccess()
        {
            _mongoData = new MongoDBDataAccess();
        }

        // Usamos .GetAwaiter().GetResult() para llamar a métodos asíncronos de forma síncrona (NECESARIO PARA LA VISTA)

        public List<KeyValuePair<string, string>> ObtenerDepartamentosActivos()
        {
            return _mongoData.ObtenerDepartamentosActivosAsync().GetAwaiter().GetResult();
        }

        public List<KeyValuePair<string, string>> ObtenerEstadosActivos()
        {
            return _mongoData.ObtenerEstadosActivosAsync().GetAwaiter().GetResult();
        }

        public List<KeyValuePair<string, string>> ObtenerMunicipiosPorEstado(string idEstado)
        {
            return _mongoData.ObtenerMunicipiosPorEstadoAsync(idEstado).GetAwaiter().GetResult();
        }

        public List<KeyValuePair<string, string>> ObtenerPuestosPorDepto(string idDepto)
        {
            return _mongoData.ObtenerPuestosPorDeptoAsync(idDepto).GetAwaiter().GetResult();
        }
    }
}