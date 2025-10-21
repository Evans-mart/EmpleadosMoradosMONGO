// Model/Estado.cs (CORRECCIÓN FINAL)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;

public class Estado
{
    // ⚠️ CRÍTICO: El ID de la colección 'cat_estados' es '_id'
    [BsonId]
    [BsonElement("_id")]
    public string Id_Estado { get; set; }

    [BsonElement("nom_estado")]
    public string Nombre_Estado { get; set; }

    [BsonElement("pais")]
    public string Pais { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("estatus")]
    public string Estatus { get; set; }

    // 💡 AÑADIDO: Propiedad auxiliar para capturar el ID cuando viene en formato 'id_estado'
    // (como en el subdocumento anidado de 'usuarios').
    //[BsonElement("id_estado")]
    //public string Id_Estado_Anidado { get; set; }

    public Estado() { }
}