// Model/Estado.cs (CORREGIDA)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;

public class Estado
{
    // ⚠️ CRÍTICO: Mapeo del ID de la colección 'cat_estados'
    [BsonId] // Indica que este campo es el _id primario de la colección
    [BsonElement("_id")]
    public string Id_Estado { get; set; } // Mapea el ID de catálogo ("01", "02", etc.)

    // ⚠️ CRÍTICO: El nombre en la colección de catálogo es 'nom_estado'
    [BsonElement("nom_estado")]
    public string Nombre_Estado { get; set; } // Lo usas como "Value" en el ComboBox

    [BsonElement("pais")]
    public string Pais { get; set; }

    // Campo 'estatus' debe estar mapeado para el filtro, no ignorado.
    [BsonElement("estatus")]
    public string Estatus { get; set; }

    public Estado() { }
}