// Model/Municipio.cs (CORREGIDA)
using EmpleadosMorados.Model;
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class Municipio
{
    // ⚠️ CRÍTICO: Aseguramos mapeo a _id para la colección de catálogo.
    [BsonId]
    [BsonElement("_id")]
    public string Id_Municipio { get; set; }

    [BsonElement("nom_municipio")]
    public string Nom_Municipio { get; set; } // 👈 Corrección: Usamos Nom_Municipio para el catálogo

    [BsonElement("id_estado")] // 👈 ¡Campo Faltante!
    public string Id_Estado { get; set; }

    [BsonElement("estatus")] // 👈 ¡Campo Faltante!
    public string Estatus { get; set; }

    // Campos del Sub-Documento (para el anidamiento dentro de Empleado)
    [BsonIgnoreIfNull] // Este es el objeto anidado (null en el catálogo)
    [BsonElement("nombre_municipio")] // Nombre usado en el anidamiento (documento 'usuarios')
    public string Nombre_Municipio { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("estado")]
    public Estado Estado { get; set; }

    public Municipio()
    {
        Estado = new Estado();
    }
}