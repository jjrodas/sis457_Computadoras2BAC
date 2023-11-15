using CadComputadoras2BAC;
using ClnComputadoras2BAC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CpComputadoras2BAC
{
    public partial class FrmProducto : Form
    {
        bool esNuevo = false;
        public FrmProducto()
        {
            InitializeComponent();
        }
        private void listar()
        {
            var productos = ProductoCln.listarPa(txtParametro.Text.Trim());
            dgvListaProductos.DataSource = productos;
            dgvListaProductos.Columns["id"].Visible = false;
            dgvListaProductos.Columns["estado"].Visible = false;
            dgvListaProductos.Columns["idCategoria"].Visible = false;
            dgvListaProductos.Columns["codigo"].HeaderText = "Código";
            dgvListaProductos.Columns["descripcion"].HeaderText = "Descripción";
            dgvListaProductos.Columns["marca"].HeaderText = "Marca";
            dgvListaProductos.Columns["precioVenta"].HeaderText = "Precio de Venta";
            dgvListaProductos.Columns["usuarioRegistro"].HeaderText = "Usuario";
            dgvListaProductos.Columns["fechaRegistro"].HeaderText = "Fecha de Registro";
            btnEditar.Enabled = productos.Count > 0;
            btnEliminar.Enabled = productos.Count > 0;
            if (productos.Count > 0) dgvListaProductos.Rows[0].Cells["codigo"].Selected = true;
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void FrmProducto_Load(object sender, EventArgs e)
        {
            Size = new Size(961, 431);
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(961, 593);
            txtCodigo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(961, 593);

            int index = dgvListaProductos.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaProductos.Rows[index].Cells["id"].Value);
            var producto = ProductoCln.get(id);
            txtCodigo.Text = producto.codigo;
            txtDescripcion.Text = producto.descripcion;
            txtMarca.Text = producto.marca;
            nudPrecioVenta.Value = producto.precioVenta;

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvListaProductos.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaProductos.Rows[index].Cells["id"].Value);
            string codigo = dgvListaProductos.Rows[index].Cells["codigo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea dar de baja el producto con el código {codigo}?",
                "::: CompumundoBAC - Mensaje :::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                ProductoCln.eliminar(id, "SIS457");
                listar();
                MessageBox.Show("Producto dado de baja correctamente", "::: CompumundoBAC - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(961, 431);
            limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void limpiar()
        {
            txtCodigo.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtMarca.Text = string.Empty;
            nudPrecioVenta.Value = 0;
        }

        private bool validar()
        {
            bool esValido = true;
            erpCodigo.SetError(txtCodigo, "");
            erpDescripcion.SetError(txtDescripcion, "");
            erpMarca.SetError(txtMarca, "");
            erpPrecioVenta.SetError(nudPrecioVenta, "");
            if (string.IsNullOrEmpty(txtCodigo.Text))
            {
                esValido = false;
                erpCodigo.SetError(txtCodigo, "El campo código es obligatorio.");
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                esValido = false;
                erpDescripcion.SetError(txtDescripcion, "El campo descripción es obligatorio.");
            }
            if (string.IsNullOrEmpty(txtMarca.Text))
            {
                esValido = false;
                erpMarca.SetError(txtMarca, "El campo marca es obligatorio.");
            }
            if (string.IsNullOrEmpty(nudPrecioVenta.Text))
            {
                esValido = false;
                erpPrecioVenta.SetError(nudPrecioVenta, "El campo precio de venta es obligatorio.");
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var producto = new Producto();
                producto.codigo = txtCodigo.Text.Trim();
                producto.descripcion = txtDescripcion.Text.Trim();
                producto.marca = txtMarca.Text.Trim();
                producto.precioVenta = (int)nudPrecioVenta.Value;
                producto.usuarioRegistro = "LabSIS457";

                if (esNuevo)
                {
                    producto.fechaRegistro = DateTime.Now;
                    producto.estado = 1;
                    ProductoCln.insertar(producto);
                }
                else
                {
                    int index = dgvListaProductos.CurrentCell.RowIndex;
                    producto.id = Convert.ToInt32(dgvListaProductos.Rows[index].Cells["id"].Value);
                    ProductoCln.actualizar(producto);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Producto guardado correctamente", "::: CompumundoBAC - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
