// Controller/EmpleadosController.cs (CORREGIDA)
using NLog;
using EmpleadosMorados.Data;
using EmpleadosMorados.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver; // Necesario para algunas excepciones de Mongo

namespace EmpleadosMorados.Controller
{
    public class EmpleadosController
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger(); // Cambiado a GetCurrentClassLogger para simplicidad
        private readonly EmpleadosDataAccess _empleadosData;
        private readonly PersonasDataAccess _personasData; // Solo para validaciones de unicidad
        private readonly MongoDBDataAccess _mongoData; // Para obtener documentos de catálogo
        private readonly CatalogosDataAccess _catalogosData; // Para las llamadas síncronas de ComboBox

        public EmpleadosController()
        {
            _empleadosData = new EmpleadosDataAccess();
            _personasData = new PersonasDataAccess();
            _mongoData = new MongoDBDataAccess();
            _catalogosData = new CatalogosDataAccess();
        }

        // 🚀 MÉTODO PRINCIPAL DE ALTA (Corregido) 🚀
        public async Task<(int id, string mensaje)> RegistrarNuevoEmpleado(
            Empleado empleado,
            string idMunicipio, // Corregido: La vista DEBE pasar los IDs seleccionados
            string idEstado,    // Corregido: La vista DEBE pasar los IDs seleccionados
            string idPuesto,    // Corregido: La vista DEBE pasar los IDs seleccionados
            string idDepto)     // Corregido: La vista DEBE pasar los IDs seleccionados
        {
            try
            {
                // 1. Validaciones de unicidad (usando los índices de Mongo)
                // Se valida CURP, RFC y Teléfono. El correo también se valida con el índice.

                if (await _personasData.ExisteCurpAsync(empleado.Curp))
                {
                    return (-1, $"El CURP {empleado.Curp} ya está registrado.");
                }
                if (await _personasData.ExisteRFCAsync(empleado.Rfc))
                {
                    return (-1, $"El RFC {empleado.Rfc} ya está registrado.");
                }
                // Nota: La validación de unicidad de teléfono y correos se manejará en la capa DAL/Mongo por el índice.

                // 2. Denormalización: Obtener documentos de catálogo y anidarlos

                // a) Trayectoria Laboral
                empleado.TrayectoriaLaboral.Fecha_Alta = DateTime.Now;
                empleado.TrayectoriaLaboral.Usr_Contrato = "NA-0001";

                // b) Correos: Aseguramos el mapeo del correo principal (FALTA EL CAMPO txtCorreoPrincipal en la vista para ser correcto)
                // Asumo que tu vista se llama 'txtCorreoPrincipal' y 'txtCorreoSecundario'
                // Esta lógica se moverá a la Vista, por ahora lo simplificamos (Ver correcciones en la Vista)

                // c) Puesto Actual (Denormalización)
                Departamento depto = await _mongoData.GetDepartamentoByIdAsync(idDepto);
                Puesto puesto = await _mongoData.GetPuestoByIdAsync(idPuesto);

                if (depto == null || puesto == null)
                {
                    return (-1, "Departamento o Puesto no encontrado en los catálogos.");
                }
                // ⚠️ CORRECCIÓN CLAVE: Asegurar que el objeto Departamento exista
                if (empleado.PuestoActual.Departamento == null)
                {
                    empleado.PuestoActual.Departamento = new Departamento();
                }

                // ⚠️ MODIFICACIÓN DEL BLOQUE: Rellena el objeto PuestoActual ya existente
                empleado.PuestoActual.Id_Puesto = puesto.Id_Puesto;
               // empleado.PuestoActual.Nombre_Puesto = puesto.Nom_Puesto;
                empleado.PuestoActual.Departamento.Id_Depto = depto.Id_Depto;
               // empleado.PuestoActual.Departamento.Nombre_Depto = depto.Nombre_Depto;


                // ----------------------------------------------------------------------


                // d) Domicilio (Denormalización Anidada)
                Municipio municipioCat = await _mongoData.GetMunicipioByIdAsync(idMunicipio);
                Estado estadoCat = await _mongoData.GetEstadoByIdAsync(idEstado);

                if (municipioCat == null || estadoCat == null)
                {
                    return (-1, "Estado o Municipio no encontrado en los catálogos geográficos.");
                }

                Estado estadoAnidado = new Estado
                {
                    Id_Estado = estadoCat.Id_Estado,
                   // Nombre_Estado = estadoCat.Nombre_Estado,
                  //  Pais = estadoCat.Pais
                };

                // 3. Creamos un nuevo objeto Municipio ANIDADO y limpio
                Municipio municipioAnidado = new Municipio
                {
                    Id_Municipio = municipioCat.Id_Municipio,
                    // Usamos la propiedad 'Nombre_Municipio' para el anidado
                    //Nom_Municipio = municipioCat.Nom_Municipio,
                    Estado = estadoAnidado // Anidamos el objeto estado limpio
                };

                // 4. REEMPLAZAMOS el objeto 'Municipio' vacío del empleado por el objeto limpio
                empleado.Domicilio.Municipio = municipioAnidado;

                // 3. Registrar el empleado (Inserción Atómica en Mongo)
                _logger.Info($"Iniciando registro para {empleado.Nombre} en Mongo...");
                String idGenerado = await _empleadosData.InsertarEmpleadoAsync(empleado);

                if (idGenerado.Length > 0)
                {
                    return (0, "Empleado registrado exitosamente.");
                }
                else if (idGenerado.Length == -1)
                {
                    return (-1, "Fallo en el registro: Dato duplicado (CURP, RFC, Teléfono o Correo).");
                }
                else // idGenerado == -2 (Error general en DAL)
                {
                    return (-2, "Error desconocido al intentar insertar el documento en MongoDB.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error inesperado en el Controller al registrar empleado.");
                return (-4, $"Error inesperado: {ex.Message}");
            }
        }

        // Métodos auxiliares síncronos para poblar ComboBoxes
        public List<KeyValuePair<string, string>> ObtenerDepartamentos()
        {
            return _catalogosData.ObtenerDepartamentosActivos();
        }

        public List<KeyValuePair<string, string>> ObtenerEstados()
        {
            return _catalogosData.ObtenerEstadosActivos();
        }

        public List<KeyValuePair<string, string>> ObtenerMunicipiosPorEstado(string idEstado)
        {
            return _catalogosData.ObtenerMunicipiosPorEstado(idEstado);
        }
        public List<KeyValuePair<string, string>> ObtenerPuestosPorDepto(string idDepto)
        {
            return _catalogosData.ObtenerPuestosPorDepto(idDepto);
        }

        // 🚀 NUEVOS MÉTODOS ASÍNCRONOS (Delegación directa a MongoDBDataAccess)
        public async Task<List<KeyValuePair<string, string>>> ObtenerDepartamentosAsync()
        {
            return await _mongoData.ObtenerDepartamentosActivosAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> ObtenerEstadosAsync()
        {
            return await _mongoData.ObtenerEstadosActivosAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> ObtenerMunicipiosPorEstadoAsync(string idEstado)
        {
            return await _mongoData.ObtenerMunicipiosPorEstadoAsync(idEstado);
        }

        public async Task<List<KeyValuePair<string, string>>> ObtenerPuestosPorDeptoAsync(string idDepto)
        {
            return await _mongoData.ObtenerPuestosPorDeptoAsync(idDepto);
        }
    }
}