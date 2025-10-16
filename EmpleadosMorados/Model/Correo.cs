using MongoDB.Bson.Serialization.Attributes;

namespace EmpleadosMorados.Model;
public class Correo
{
    [BsonElement("tipo")] // Mapea a 'tipo' en Mongo
    public string Tipo { get; set; }

    [BsonElement("correo")] // Mapea a 'correo' en Mongo
    public string Correo_Electronico { get; set; }
}