// Data/PersonasDataAccess.cs
using System;
using System.Threading.Tasks;
using EmpleadosMorados.Model;
using MongoDB.Driver;
using NLog;

namespace EmpleadosMorados.Data
{
    // Esta clase DEBE ser reescrita para usar MongoDBDataAccess, o eliminada
    // ya que su lógica de inserción y validación fue movida o fusionada.

    // Por ahora, solo mantendremos los métodos de validación, usando Mongo.
    public class PersonasDataAccess
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MongoDBDataAccess _dbAccess; // Usamos el nuevo acceso a datos

        public PersonasDataAccess()
        {
            _dbAccess = new MongoDBDataAccess();
        }

        // 🚀 Validaciones de Unicidad con MongoDB 🚀
        public async Task<bool> ExisteCurpAsync(string curp)
        {
            // Busca si existe un usuario activo con ese CURP
            var filter = Builders<Empleado>.Filter.And(
                Builders<Empleado>.Filter.Eq(e => e.Curp, curp),
                Builders<Empleado>.Filter.Eq(e => e.Estatus, "ACTIVO")
            );

            long count = await _dbAccess.Usuarios.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> ExisteRFCAsync(string rfc)
        {
            // Busca si existe un usuario activo con ese RFC
            var filter = Builders<Empleado>.Filter.And(
                Builders<Empleado>.Filter.Eq(e => e.Rfc, rfc),
                Builders<Empleado>.Filter.Eq(e => e.Estatus, "ACTIVO")
            );

            long count = await _dbAccess.Usuarios.CountDocumentsAsync(filter);
            return count > 0;
        }

        // ⚠️ El método InsertarPersona DEBE ser eliminado de aquí y su lógica movida al controlador/negocio
        // El método InsertarUsuarioAsync ya existe en MongoDBDataAccess.
    }
}