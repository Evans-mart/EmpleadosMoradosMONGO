// Model/PuestoActual.cs (Revisión de Mapeo)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class PuestoActual
{
    [BsonElement("id_puesto")]
    public string Id_Puesto { get; set; }

    [BsonElement("nombre_puesto")]
    public string Nombre_Puesto { get; set; }

    // El error indica que el subdocumento DEPARTAMENTO falló. 
    // Mantenemos la estructura de anidamiento correcta.
    [BsonElement("departamento")]
    public Departamento Departamento { get; set; }

    public PuestoActual()
    {
        // 🚨 CRÍTICO: El constructor debe inicializar el sub-documento.
        Departamento = new Departamento();
    }
}