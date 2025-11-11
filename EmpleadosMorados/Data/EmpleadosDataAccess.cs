// Data/EmpleadosDataAccess.cs
using EmpleadosMorados.Model;
using NLog;

namespace EmpleadosMorados.Data
{
    // Esta clase solo existirá para orquestar la inserción de Empleado (que ahora es un solo documento)
    public class EmpleadosDataAccess
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MongoDBDataAccess _mongoData; // Usamos el nuevo acceso a datos
        private readonly PersonasDataAccess _personasData; // Para las validaciones

        public EmpleadosDataAccess()
        {
            _mongoData = new MongoDBDataAccess(); // Se encarga de la inserción real
            _personasData = new PersonasDataAccess(); // Se encarga de las validaciones de unicidad
        }

        // El método de alta DEBE ser asíncrono
        public async Task<String> InsertarEmpleadoAsync(Empleado empleado)
        {
            //if (empleado == null) return -1;

            _logger.Info($"Iniciando inserción de Empleado en MongoDB...");

            // Ahora, la inserción es atómica y no necesita lógica compleja de múltiples tablas.
            // La lógica de TRAY_LAB ahora está incrustada en el objeto Empleado (Model/Empleado.cs).

            // Llama al método real de inserción en MongoDBDataAccess
            String idGenerado = await _mongoData.InsertarUsuarioAsync(empleado);

            if (idGenerado.Length> 0)
            {
                _logger.Info($"Empleado insertado exitosamente con ID: {idGenerado}");
            }
            else
            {
                _logger.Error("Fallo al insertar el empleado en MongoDB.");
            }
            Console.WriteLine($"✅ ID generado y listo para insertar: {idGenerado}");
            return idGenerado;
        }

    }
}