// Model/Puesto.cs
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class Puesto
{
    [BsonElement("_id")] // CRÍTICO: El ID de la colección es el campo "_id"
    public string Id_Puesto { get; set; }

    [BsonElement("nom_puesto")]
    public string Nom_Puesto { get; set; }

    [BsonElement("id_depto")]
    public string Id_Depto { get; set; }

    [BsonElement("estatus")]
    public string Estatus { get; set; }
}