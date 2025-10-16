// Model/Estado.cs (CORREGIDA)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;

public class Estado
{
    // ⚠️ CRÍTICO: Aseguramos mapeo a _id para la colección de catálogo.
    [BsonId]
    [BsonElement("_id")]
    public string Id_Estado { get; set; }

    [BsonElement("nom_estado")]
    public string Nombre_Estado { get; set; }// Lo usas como "Value" en el ComboBox

    [BsonElement("pais")]
    public string Pais { get; set; }

    // Campo 'estatus' debe estar mapeado para el filtro, no ignorado.
    [BsonElement("estatus")]
    public string Estatus { get; set; }

    public Estado() { }
}