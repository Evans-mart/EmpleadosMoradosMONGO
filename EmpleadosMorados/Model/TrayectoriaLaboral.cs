using System;
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class TrayectoriaLaboral
{
    [BsonElement("fecha_alta")]
    public DateTime Fecha_Alta { get; set; }

    [BsonElement("usr_contrato")]
    public string Usr_Contrato { get; set; }
}