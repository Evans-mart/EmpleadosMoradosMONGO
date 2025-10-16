// Model/Departamento.cs (REVISADA)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;

public class Departamento
{
    // ✅ Agregamos [BsonId] para ser explícitos sobre el ID primario
    [BsonId]
    [BsonElement("_id")]
    public string Id_Depto { get; set; }

    [BsonElement("nombre_depto")]
    public string Nombre_Depto { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("estatus")]
    public string Estatus { get; set; }
}