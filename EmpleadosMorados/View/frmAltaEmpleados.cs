using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmpleadosMorados.Controller;
using EmpleadosMorados.Model;
using static System.Windows.Forms.DataFormats;

namespace EmpleadosMorados.View
{
    public partial class frmAltaEmpleados : Form
    {
        // Instancia de la clase de negocio para orquestar la inserción
        private readonly EmpleadosController _empleadosNegocio;
        public frmAltaEmpleados()
        {
            InitializeComponent(); // Simulación del método generado por el diseñador
            _empleadosNegocio = new EmpleadosController();
            //InicializaVentanaEmpleadoRegistro();
        }
        //public void InicializaVentanaEmpleadoRegistro()
        //{
        //    PoblaGenero();
        //    PoblaDepartamentos();
        //    PoblaEstados();
        //    // Otros inicializadores de Placeholders si se necesitan
        //}
        private void PoblaGenero()
        {
            // Usamos los valores exactos del DDL: MASCULINO, FEMENINO, OTRO
            Dictionary<string, string> listGenero = new Dictionary<string, string>
            {
                { "MASCULINO", "Masculino" },
                { "FEMENINO", "Femenino" },
                { "OTRO", "Otro" }
            };
            cboSexo.DataSource = new BindingSource(listGenero, null);
            cboSexo.DisplayMember = "Value";
            cboSexo.ValueMember = "Key";
            cboSexo.SelectedIndex = -1;
        }

        private async Task PoblaDepartamentosAsync()
        {
            try
            {
                // 🚨 Llamar a la nueva versión asíncrona del Controller
                var departamentos = await _empleadosNegocio.ObtenerDepartamentosAsync();
                cboDepto.DataSource = new BindingSource(departamentos, null);
                cboDepto.DisplayMember = "Value";
                cboDepto.ValueMember = "Key";
                cboDepto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar departamentos: " + ex.Message, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task PoblaEstadosAsync()
        {
            try
            {
                // 🚨 Llamar a la nueva versión asíncrona del Controller
                var estados = await _empleadosNegocio.ObtenerEstadosAsync();
                cboEstado.DataSource = new BindingSource(estados, null);
                cboEstado.DisplayMember = "Value";
                cboEstado.ValueMember = "Key";
                cboEstado.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar estados: " + ex.Message, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCreateEmpleado_Click_1(object sender, EventArgs e)
        {
            frmAltaEmpleados formAlta = new frmAltaEmpleados();
            formAlta.Show();
            this.Hide();
        }

        private void btnReadEmpleado_Click_1(object sender, EventArgs e)
        {
            frmListaEmpleado formList = new frmListaEmpleado();
            formList.Show();
            this.Hide();
        }

        private void btnUpdateEmpleado_Click(object sender, EventArgs e)
        {
            frmActualizaEmpleado formActualiza = new frmActualizaEmpleado();
            formActualiza.Show();
            this.Hide();
        }

        private void btnDeleteEmpleado_Click(object sender, EventArgs e)
        {
            frmEliminaEmpleado formElim = new frmEliminaEmpleado();
            formElim.Show();
            this.Hide();
        }

        private void frmAltaEmpleados_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Cuando esta ventana se cierra, cerramos la aplicación entera 
            // para evitar que el menú quede oculto sin forma de salir.
            Application.Exit();

            // O, si tienes una forma de referenciar el formulario anterior, 
            // podrías hacer que se muestre de nuevo. Pero Application.Exit() es más seguro y simple.
        }

        private async void cboEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboEstado.SelectedValue != null && cboEstado.SelectedValue is string idEstado)
            {
                await PoblaMunicipiosAsync(idEstado); // 👈 Usar await
            }
            else
            {
                cboMunicipio.DataSource = null;
            }
        }
        private async Task PoblaMunicipiosAsync(string idEstado)
        {
            try
            {
                // 🚨 Llamar a la nueva versión asíncrona del Controller
                var municipios = await _empleadosNegocio.ObtenerMunicipiosPorEstadoAsync(idEstado);
                cboMunicipio.DataSource = new BindingSource(municipios, null);
                cboMunicipio.DisplayMember = "Value";
                cboMunicipio.ValueMember = "Key";
                cboMunicipio.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar municipios: " + ex.Message, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task PoblaPuestosAsync(string idDepto)
        {
            try
            {
                // 🚨 Llamar a la nueva versión asíncrona del Controller
                var puestos = await _empleadosNegocio.ObtenerPuestosPorDeptoAsync(idDepto);
                cboPuesto.DataSource = new BindingSource(puestos, null);
                cboPuesto.DisplayMember = "Value";
                cboPuesto.ValueMember = "Key";
                cboPuesto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar puestos: " + ex.Message, "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnAgregar_Click(object sender, EventArgs e)
        {
            await GuardarEmpleadoAsync();
        }
        // 🚀 MÉTODO DE INSERCIÓN CORREGIDO PARA PREVENIR FORMATEXCEPTION 🚀
        private async Task GuardarEmpleadoAsync()
        {
            // 1. Validaciones de la Vista
            if (!ValidarDatosFormulario())
            {
                return;
            }

            // Variables para la conversión segura
            int codigoPostal;
            long telefono;

            // ⚠️ 1.1 Conversión Segura de CP
            if (!int.TryParse(txtCP.Text.Trim(), out codigoPostal))
            {
                MessageBox.Show("El Código Postal (CP) debe ser un número entero válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ⚠️ 1.2 Conversión Segura de Teléfono
            if (!long.TryParse(txtTelefono.Text.Trim(), out telefono))
            {
                MessageBox.Show("El Teléfono debe ser un número entero válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Obtener IDs seleccionados
                string idDepto = cboDepto.SelectedValue.ToString();
                string idPuesto = cboPuesto.SelectedValue.ToString();
                string idEstado = cboEstado.SelectedValue.ToString();
                string idMunicipio = cboMunicipio.SelectedValue.ToString();

                // 2. Mapeo de Domicilio
                Domicilio domicilio = new Domicilio
                {
                    Calle = txtCalle.Text.Trim(),
                    NoExterior = txtNoExt.Text.Trim(),
                    NoInterior = txtNoInt.Text.Trim(),
                    CodigoPostal = codigoPostal, // Usamos la variable convertida
                    Colonia = txtColonia.Text.Trim(),
                };

                // 3. Mapeo de Correos
                List<Correo> correos = new List<Correo>();
                correos.Add(new Correo { Tipo = "PRINCIPAL", Correo_Electronico = txtCorreoPrincipal.Text.Trim() });
                if (!string.IsNullOrWhiteSpace(txtCorreoSecundario.Text))
                {
                    correos.Add(new Correo { Tipo = "SECUNDARIO", Correo_Electronico = txtCorreoSecundario.Text.Trim() });
                }


                // 4. Mapeo de Empleado
                Empleado empleado = new Empleado
                {
                    Nombre = txtNombre.Text.Trim(),
                    ApellidoPaterno = txtApPat.Text.Trim(),
                    ApellidoMaterno = string.IsNullOrWhiteSpace(txtApMat.Text) ? null : txtApMat.Text.Trim(),
                    Curp = txtCURP.Text.Trim(),
                    Rfc =string.IsNullOrWhiteSpace(txtRFC.Text) ? null : txtRFC.Text.Trim(),
                    Sexo = cboSexo.SelectedValue?.ToString() ?? "OTRO",
                    Telefono = telefono, // Usamos la variable convertida
                    Estatus = "ACTIVO",
                    Domicilio = domicilio,
                    Correos = correos
                };

                // 5. Llamar al Controlador/Negocio
                var (id, mensaje) = await _empleadosNegocio.RegistrarNuevoEmpleado(
                    empleado,
                    idMunicipio,
                    idEstado,
                    idPuesto,
                    idDepto);

                if (id == 0)
                {
                    MessageBox.Show($"Empleado registrado: {mensaje}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                }
                else
                {
                    string prefijo = id == -1 ? "Error de Duplicidad" : "Fallo en el registro";
                    MessageBox.Show($"{prefijo}: {mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                // Ahora esta captura es para errores inesperados o de conexión
                MessageBox.Show($"Error crítico en la aplicación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private bool ValidarDatosFormulario()
        {
            // --- 1. Validación de ComboBox (Todos son obligatorios) ---

            // Verificamos si la selección de cualquiera de los combos es nula.
            if (cboSexo.SelectedValue == null || cboDepto.SelectedValue == null ||
                cboEstado.SelectedValue == null || cboMunicipio.SelectedValue == null ||
                cboPuesto.SelectedValue == null) 
            {
                MessageBox.Show("Seleccione una opción válida en todos los menús desplegables.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // --- 2. Validación de Campos de Texto Obligatorios ---

            // Lista de campos de texto OBLIGATORIOS (excluyendo RFC, ApMat, NoInt, NoExt)

            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApPat.Text) ||       // Apellido Paterno
                string.IsNullOrWhiteSpace(txtCURP.Text) ||        // CURP (Asumiendo que es obligatorio)
                
                string.IsNullOrWhiteSpace(txtCorreoPrincipal.Text) ||
                string.IsNullOrWhiteSpace(txtCalle.Text) ||
                string.IsNullOrWhiteSpace(txtColonia.Text) ||
                string.IsNullOrWhiteSpace(txtCP.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Asegúrese de haber llenado todos los campos de texto obligatorios (marcados con *).", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Verifica que la longitud del texto sea exactamente 10
            if (txtTelefono.Text.Length != 10)
            {
                MessageBox.Show("El número de teléfono debe contener exactamente 10 dígitos.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verifica que la longitud del texto sea exactamente 5
            if (txtCP.Text.Length != 5)
            {
                MessageBox.Show("El Código Postal debe contener exactamente 5 dígitos.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 1. Verificar si el correo principal contiene el símbolo '@'
            if (!txtCorreoPrincipal.Text.Contains("@"))
            {
                MessageBox.Show("El campo 'Correo Principal' debe contener el símbolo '@'.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 2. Opcional: Verificar el correo secundario si es obligatorio y existe
            if (!string.IsNullOrWhiteSpace(txtCorreoSecundario.Text) && !txtCorreoSecundario.Text.Contains("@"))
            {
                MessageBox.Show("El campo 'Correo Secundario' debe contener el símbolo '@' si se proporciona.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Si todas las validaciones pasan
            return true;
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtApPat.Text = string.Empty;
            txtApMat.Text = string.Empty;
            txtCURP.Text = string.Empty;
            txtRFC.Text = string.Empty;
            cboSexo.SelectedIndex = -1;
            txtCalle.Text = string.Empty;
            txtColonia.Text = string.Empty;
            txtCP.Text = string.Empty;
            cboEstado.SelectedIndex = -1;
            cboMunicipio.SelectedIndex = -1;
            txtCorreoPrincipal.Text = string.Empty;
            txtCorreoSecundario.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            cboDepto.SelectedIndex = -1;
            cboPuesto.SelectedIndex = -1;
            txtNoExt.Text = string.Empty;
            txtNoInt.Text = string.Empty;
            //sexo, estado, municipio, depto puesto
            // ...
            //InicializaVentanaEmpleadoRegistro(); // La forma más simple de resetear combos y reestablecer estados
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private async void cboDepto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDepto.SelectedValue != null && cboDepto.SelectedValue is string idDepto)
            {
                await PoblaPuestosAsync(idDepto); // 👈 Usar await
            }
            else
            {
                cboPuesto.DataSource = null;
            }
        }

        private async void frmAltaEmpleados_Load_1(object sender, EventArgs e)
        {
            PoblaGenero(); // Síncrono, OK
            await PoblaDepartamentosAsync(); // Usar la nueva versión asíncrona
            await PoblaEstadosAsync();
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si la tecla presionada NO es un dígito (número).
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back) // Permite la tecla 'Back' (borrar)
            {
                // Si no es un dígito ni la tecla Back, anula el evento.
                // Esto evita que el carácter se muestre en el TextBox.
                e.Handled = true;
            }
        }

        private void txtCP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permite dígitos (números) y la tecla de retroceso (borrar).
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)Keys.Back)
            {
                // Bloquea cualquier otra tecla presionada.
                e.Handled = true;
            }
        }
    }
}
