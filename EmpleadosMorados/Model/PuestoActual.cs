// Model/PuestoActual.cs
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class PuestoActual
{
    [BsonElement("id_puesto")]
    public string Id_Puesto { get; set; }

    [BsonElement("nombre_puesto")]
    public string Nombre_Puesto { get; set; }

    // ¡CRÍTICO: INCORPORACIÓN DEL SUB-DOCUMENTO!
    [BsonElement("departamento")]
    public Departamento Departamento { get; set; }

    public PuestoActual()
    {
        Departamento = new Departamento(); // Inicialización
    }
}