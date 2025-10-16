// Data/MongoDBDataAccess.cs (CORREGIDA y AMPLIADA)
using System;
using System.Collections.Generic; // Para List<KeyValuePair>
using System.Threading.Tasks;
using EmpleadosMorados.Model;
using MongoDB.Driver;
using NLog;

namespace EmpleadosMorados.Data
{
    public class MongoDBDataAccess
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MongoDbContext _context;

        public MongoDBDataAccess()
        {
            _context = new MongoDbContext();
        }

        public IMongoCollection<Empleado> Usuarios => _context.Usuarios; // Propiedad de acceso directo para PersonasDataAccess

        // ... [El método InsertarUsuarioAsync y GetNextSequenceIdAsync están correctos, los omito por espacio]

        // Método auxiliar para obtener el ID secuencial (Necesario si usas _id: 1, 2, 3, ...)
        //private async Task<int> GetNextSequenceIdAsync()
        //{
        //    try
        //    {
        //        // 🔹 Usa la colección correcta (Empleados)
        //        var sort = Builders<Empleado>.Sort.Descending(e => e.Id);

        //        var lastEmpleado = await _context.Usuarios
        //            .Find(FilterDefinition<Empleado>.Empty)
        //            .Sort(sort)
        //            .Limit(1)
        //            .FirstOrDefaultAsync();

        //        // 🔹 Si existe, suma 1; si no, empieza en 1
        //        int nextId = lastEmpleado != null ? lastEmpleado.Id + 1 : 1;

        //        Console.WriteLine($"➡️ Próximo ID generado: {nextId}");
        //        return nextId;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"❌ Error en GetNextSequenceIdAsync: {ex.Message}");
        //        _logger.Error(ex, "Error al obtener el siguiente ID secuencial en empleados.");
        //        return -2;
        //    }
        //}

        public async Task<string> InsertarUsuarioAsync(Empleado nuevoEmpleado)
        {
            try
            {
                // 1. Ya no se calcula el ID. Simplemente insertamos el objeto.
                //    MongoDB generará el _id automáticamente en este paso.
                await _context.Usuarios.InsertOneAsync(nuevoEmpleado);

                // 2. Después de la inserción, el driver de C# actualiza automáticamente
                //    el objeto 'nuevoEmpleado' con el ID que se acaba de generar.
                return nuevoEmpleado.Id;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                // Error específico si se viola un índice único (CURP, RFC, etc.)
                _logger.Error(ex, "Error de clave duplicada al insertar usuario.");
                return null; // O un string vacío, para indicar el fallo
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error general al insertar usuario en MongoDB");
                return null; // O un string vacío
            }
        }


        // 🚀 Métodos para Catálogos 🚀

        public async Task<List<KeyValuePair<string, string>>> ObtenerDepartamentosActivosAsync()
        {
            // ⚠️ CORRECCIÓN CLAVE: 1. Leer los documentos completos (sin Project)
            var documentos = await _context.Departamentos
                .Find(d => d.Estatus == "ACTIVO")
                .ToListAsync();

            // 2. Convertir en memoria a KeyValuePair usando LINQ
            var departamentos = documentos.Select(d =>
                new KeyValuePair<string, string>(d.Id_Depto, d.Nombre_Depto))
                .ToList();

            return departamentos;
        }

        public async Task<List<KeyValuePair<string, string>>> ObtenerEstadosActivosAsync()
        {
            // 1. Leer documentos completos
            var documentos = await _context.CatEstados
                .Find(e => e.Estatus == "ACTIVO")
                .ToListAsync();

            // 2. Convertir en memoria
            var estados = documentos.Select(e =>
                new KeyValuePair<string, string>(e.Id_Estado, e.Nombre_Estado))
                .ToList();

            return estados;
        }
        public async Task<List<KeyValuePair<string, string>>> ObtenerMunicipiosPorEstadoAsync(string idEstado)
        {
            // 1. Leer documentos completos
            var documentos = await _context.CatMunicipios
                .Find(m => m.Id_Estado == idEstado && m.Estatus == "ACTIVO")
                .ToListAsync();

            // 2. Convertir en memoria (usando Nom_Municipio)
            var municipios = documentos.Select(m =>
                new KeyValuePair<string, string>(m.Id_Municipio, m.Nom_Municipio))
                .ToList();

            return municipios;
        }

        public async Task<List<KeyValuePair<string, string>>> ObtenerPuestosPorDeptoAsync(string idDepto) // 👈 NUEVO
        {
            // Mapea la colección 'puestos' a KeyValuePair<string, string>
            var puestos = await _context.Puestos
                .Find(p => p.Id_Depto == idDepto && p.Estatus == "ACTIVO")
                .Project(p => new KeyValuePair<string, string>(p.Id_Puesto, p.Nom_Puesto))
                .ToListAsync();

            return puestos;
        }

        // 🚀 Métodos para obtener el Documento COMPLETO por ID 🚀
        // Necesarios para la denormalización en el Controller

        public async Task<Departamento> GetDepartamentoByIdAsync(string idDepto)
        {
            return await _context.Departamentos.Find(d => d.Id_Depto == idDepto).FirstOrDefaultAsync();
        }

        public async Task<Puesto> GetPuestoByIdAsync(string idPuesto)
        {
            return await _context.Puestos.Find(p => p.Id_Puesto == idPuesto).FirstOrDefaultAsync();
        }

        public async Task<Estado> GetEstadoByIdAsync(string idEstado)
        {
            return await _context.CatEstados.Find(e => e.Id_Estado == idEstado).FirstOrDefaultAsync();
        }

        public async Task<Municipio> GetMunicipioByIdAsync(string idMunicipio)
        {
            // Asumo que el modelo Municipio está como colección de catálogo y tiene el campo id_estado
            return await _context.CatMunicipios.Find(m => m.Id_Municipio == idMunicipio).FirstOrDefaultAsync();
        }


    }
}