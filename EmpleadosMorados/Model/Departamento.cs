// Model/Departamento.cs (CORRECCIÓN FINAL)
using MongoDB.Bson.Serialization.Attributes;

public class Departamento
{
    // ⚠️ CRÍTICO: Indica que Id_Depto es el ID Primario de la colección de catálogo.
    [BsonId]
    [BsonElement("_id")]
    public string Id_Depto { get; set; } // Esto mapea "_id" de la colección de catálogo.

    // Para la Denormalización (subdocumento 'puesto_actual'): El campo es 'nombre_depto' y NO 'id_depto'.
    [BsonElement("nombre_depto")]
    public string Nombre_Depto { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("estatus")]
    public string Estatus { get; set; }
}