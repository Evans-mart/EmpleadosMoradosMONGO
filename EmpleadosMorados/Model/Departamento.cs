// Model/Departamento.cs (CORRECCIÓN FINAL Y DEFINITIVA)
using MongoDB.Bson.Serialization.Attributes;
namespace EmpleadosMorados.Model;
public class Departamento
{
    // ⚠️ CRÍTICO: Usamos _id para la colección principal (así lo lee la DAL)
    // El driver es inteligente y a veces permite que esta propiedad capture ambos,
    // pero debemos asegurarnos.
    [BsonId]
    [BsonElement("_id")]
    public string Id_Depto { get; set; }

    // Mapeo del nombre.
    [BsonElement("nombre_depto")]
    public string Nombre_Depto { get; set; }

    // 💡 AÑADIDO: Propiedad auxiliar para capturar el ID cuando viene en el formato 'id_depto' 
    // (como en el subdocumento de tu JSON de ejemplo). 
    // Esto resuelve el conflicto del documento anidado.
    [BsonElement("id_depto")]
    public string Id_Depto_Anidado { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("estatus")]
    public string Estatus { get; set; }
}