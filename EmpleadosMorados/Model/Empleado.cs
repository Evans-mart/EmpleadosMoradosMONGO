using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic; // Necesario para List<Correo>

namespace EmpleadosMorados.Model;
public class Empleado
{
    // ⚠️ CRÍTICO: ID de MongoDB. Es entero en tus datos, así que forzamos el mapeo.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] // CRÍTICO: Mapea el ObjectId de Mongo a un string en C#
    public string Id { get; set; }

    // DATOS PLANOS (Vienen de tu antigua clase Persona)
    [BsonElement("nombre")]
    public string Nombre { get; set; }
    [BsonElement("apellido_pat")]
    public string ApellidoPaterno { get; set; }
    [BsonElement("apellido_mat")]
    public string ApellidoMaterno { get; set; }
    [BsonElement("curp")]
    public string Curp { get; set; }
    [BsonElement("rfc")]
    public string Rfc { get; set; }
    [BsonElement("telefono")]
    public long Telefono { get; set; } // Lo ponemos como long para que mapee el número de Mongo
    [BsonElement("sexo")]
    public string Sexo { get; set; }
    [BsonElement("estatus")]
    public string Estatus { get; set; }

    // PROPIEDADES LABORALES
    // NOTA: Los campos Puesto, Sueldo, etc. que tenías en Empleado
    // se han FUSIONADO en el objeto PuestoActual, que es una mejor práctica en Mongo.
    // Solo dejamos los campos que no están incrustados:
    public int IdPersona { get; set; } // Propiedad que ya no se necesita en Mongo
    public DateTime FechaIngreso { get; set; } // Ya está en TrayectoriaLaboral

    // 🚀 INCORPORACIONES DE SUB-DOCUMENTOS (EMBEDDING) 🚀

    [BsonElement("trayectoria_laboral")]
    public TrayectoriaLaboral TrayectoriaLaboral { get; set; }

    [BsonElement("puesto_actual")]
    public PuestoActual PuestoActual { get; set; }

    [BsonElement("correos")]
    public List<Correo> Correos { get; set; } = new List<Correo>();

    [BsonElement("domicilio")]
    public Domicilio Domicilio { get; set; }

    // Constructor por defecto
    public Empleado()
    {
        TrayectoriaLaboral = new TrayectoriaLaboral();
        PuestoActual = new PuestoActual();
        Domicilio = new Domicilio();
    }
    // ... (El resto de constructores debe ser refactorizado)
}