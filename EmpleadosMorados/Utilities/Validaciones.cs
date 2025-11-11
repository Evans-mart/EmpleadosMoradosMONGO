using System;
using System.Linq; // Necesario para .All()
using System.Text.RegularExpressions;

namespace EmpleadosMorados.Utilities
{
    public static class Validaciones
    {
        /// <summary>
        /// Valida si un correo electrónico es válido.
        /// </summary>
        public static bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return false;
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(correo, patron);
        }

        /// <summary>
        /// Valida si una CURP es válida (Formato de 18 caracteres).
        /// </summary>
        public static bool EsCURPValido(string curp)
        {
            if (string.IsNullOrWhiteSpace(curp)) return false;
            // 2. Tu patrón de CURP era bueno, pero le faltaban 2 caracteres al final.
            // Este patrón valida las 18 posiciones.
            string patron = @"^[A-Z]{4}\d{6}[HM][A-Z]{5}[A-Z0-9]{2}$";
            return Regex.IsMatch(curp.ToUpper(), patron); // Usamos ToUpper()
        }

        /// <summary>
        /// Valida si un RFC es válido.
        /// Ejemplos válidos: ABCD990101XYZ, ROGA8505051A2
        /// Ejemplos no válidos: abcd990101XYZ (minúsculas no permitidas), ABC990101XYZ (faltan caracteres en la parte inicial), ABCD990101XYZ1 (más caracteres de los permitidos)
        /// </summary>
        /// <param name="rfc">RFC a validar</param>
        /// <returns>Retorna verdadero si el RFC es válido, falso en caso contrario</returns>
        public static bool EsRFCValido(string rfc)
        {
            string patron = @"^[A-Z]{4}\d{6}[A-Z0-9]{3}$";
            return Regex.IsMatch(rfc, patron);
        }


        // --- 3. MÉTODOS NUEVOS QUE NECESITAS ---

        /// <summary>
        /// Valida si un número de teléfono es válido (exactamente 10 dígitos numéricos).
        /// </summary>
        public static bool EsTelefonoValido(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            // Revisa dos cosas: que tenga 10 caracteres Y que todos sean números.
            return telefono.Length == 10 && telefono.All(char.IsDigit);
        }

        /// <summary>
        /// Valida si un Código Postal es válido (exactamente 5 dígitos numéricos).
        /// </summary>
        public static bool EsCPValido(string cp)
        {
            if (string.IsNullOrWhiteSpace(cp))
                return false;

            // Revisa dos cosas: que tenga 5 caracteres Y que todos sean números.
            return cp.Length == 5 && cp.All(char.IsDigit);
        }

        /// <summary>
        /// Valida si un texto contiene únicamente números (útil para KeyPress).
        /// </summary>
        public static bool EsSoloNumeros(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return true; // Un campo vacío no es "inválido"
            return texto.All(char.IsDigit);
        }

        /// <summary>
        /// Valida si un nombre es válido (Debe empezar con mayúscula y no contener números).
        /// Ejemplos válidos: Juan, Maria Fernanda, Luis Alberto
        /// Ejemplos no válidos: juan (debe iniciar con mayúscula), JUAN (todo en mayúsculas no permitido), Juan3 (contiene números)
        /// </summary>
        /// <param name="nombre">Nombre a validar</param>
        /// <returns>Retorna verdadero si el nombre es válido, falso en caso contrario</returns>
        public static bool EsNombreValido(string nombre)
        {
            string patron = @"^[A-Z][a-z]+(\s[A-Z][a-z]+)*$";
            return Regex.IsMatch(nombre, patron);
        }

        /// <summary>
        /// Valida si un apellido es válido (Debe empezar con mayúscula y no contener números).
        /// Ejemplos válidos: Gomez, De La Cruz, Fernandez Lopez
        /// Ejemplos no válidos: gomez (debe iniciar con mayúscula), GOMEZ (todo en mayúsculas no permitido), Gomez123 (contiene números)
        /// </summary>
        /// <param name="apellido">Apellido a validar</param>
        /// <returns>Retorna verdadero si el apellido es válido, falso en caso contrario</returns>
        public static bool EsApellidoValido(string apellido)
        {
            string patron = @"^[A-Z][a-z]+(\s[A-Z][a-z]+)*$";
            return Regex.IsMatch(apellido, patron);
        }

        /// <summary>
        /// Valida si una dirección es válida (Debe empezar con mayúscula y no contener números).
        /// Ejemplos válidos: Avenida Reforma, Calle Juarez
        /// Ejemplos no válidos: calle 5 (no empieza con mayúscula), Calle-5 (contiene guiones no permitidos), Calle123 (contiene números)
        /// </summary>
        /// <param name="direccion">Dirección a validar</param>
        /// <returns>Retorna verdadero si la dirección es válida, falso en caso contrario</returns>
        public static bool EsDireccionValida(string direccion)
        {
            string patron = @"^[A-Z][a-z]+(\s[A-Z][a-z]+)*$";
            return Regex.IsMatch(direccion, patron);
        }

    }
}