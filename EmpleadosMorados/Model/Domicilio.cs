// Model/Domicilio.cs (CORREGIDA)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class Domicilio
{
    [BsonElement("calle")]
    public string Calle { get; set; }
    [BsonElement("no_ext")]
    public string NoExterior { get; set; }
    [BsonElement("no_int")]
    public string NoInterior { get; set; }
    [BsonElement("cp")]
    public string CodigoPostal { get; set; } // Lo cambiamos a int para que mapee el tipo BSON
    [BsonElement("colonia")]
    public string Colonia { get; set; }

    // ¡CRÍTICO: INCORPORACIÓN DEL SUB-DOCUMENTO!
    [BsonElement("municipio")]
    public Municipio Municipio { get; set; }

    // Constructor por defecto
    public Domicilio()
    {
        NoInterior = "N/A";
        Municipio = new Municipio();
    }
}