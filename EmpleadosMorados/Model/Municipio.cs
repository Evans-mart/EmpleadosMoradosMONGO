// Model/Municipio.cs (CORREGIDA)
using EmpleadosMorados.Model;
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class Municipio
{
    // ⚠️ CRÍTICO: Aseguramos mapeo a _id para la colección de catálogo.
    // --- Propiedades para LEER el catálogo 'cat_municipios' ---
    [BsonId]
    [BsonElement("_id")]
    [BsonIgnoreIfNull] // Ignora si es nulo (cuando está anidado)
    public string Id_Municipio { get; set; }

    [BsonElement("nom_municipio")]
    [BsonIgnoreIfNull]
    public string Nom_Municipio { get; set; } // Lo leemos del catálogo

    [BsonElement("id_estado")]
    [BsonIgnoreIfNull] // Lo leemos del catálogo, pero lo ignoramos al anidar
    public string Id_Estado { get; set; }

    [BsonElement("estatus")]
    [BsonIgnoreIfNull] // Lo leemos del catálogo, pero lo ignoramos al anidar
    public string Estatus { get; set; }

    //// Campos del Sub-Documento (para el anidamiento dentro de Empleado)
    //[BsonIgnoreIfNull] // Este es el objeto anidado (null en el catálogo)
    //[BsonElement("nombre_municipio")] // Nombre usado en el anidamiento (documento 'usuarios')
    //public string Nombre_Municipio { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("estado")]
    public Estado Estado { get; set; }

    public Municipio()
    {
        Estado = new Estado();
    }
}