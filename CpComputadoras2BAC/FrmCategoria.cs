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
    public partial class FrmCategoria : Form
    {
        bool esNuevo = false;
        public FrmCategoria()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var categorias = CategoriaCln.listarPa(txtParametro.Text.Trim());
            dgvListaCategorias.DataSource = categorias;
            dgvListaCategorias.Columns["id"].Visible = false;
            dgvListaCategorias.Columns["estado"].Visible = false;
            dgvListaCategorias.Columns["nombre"].HeaderText = "Nombre";
            dgvListaCategorias.Columns["descripcion"].HeaderText = "Descripción";
            dgvListaCategorias.Columns["usuarioRegistro"].HeaderText = "Usuario";
            dgvListaCategorias.Columns["fechaRegistro"].HeaderText = "Fecha de Registro";
            btnEditar.Enabled = categorias.Count > 0;
            btnEliminar.Enabled = categorias.Count > 0;
            if (categorias.Count > 0) dgvListaCategorias.Rows[0].Cells["nombre"].Selected = true;
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void FrmCategoria_Load(object sender, EventArgs e)
        {
            Size = new Size(961, 431);
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(961, 593);
            txtNombre.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(961, 593);

            int index = dgvListaCategorias.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaCategorias.Rows[index].Cells["id"].Value);
            var categoria = CategoriaCln.get(id);
            txtNombre.Text = categoria.nombre;
            txtDescripcion.Text = categoria.descripcion;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvListaCategorias.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaCategorias.Rows[index].Cells["id"].Value);
            string nombre = dgvListaCategorias.Rows[index].Cells["nombre"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea dar de baja la categoría {nombre}?",
                "::: CompumundoBAC - Mensaje :::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                CategoriaCln.eliminar(id, "SIS457");
                listar();
                MessageBox.Show("Categoría dada de baja correctamente", "::: CompumundoBAC - Mensaje :::",
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
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
        }

        private bool validar()
        {
            bool esValido = true;
            erpNombre.SetError(txtNombre, "");
            erpDescripcion.SetError(txtDescripcion, "");
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                esValido = false;
                erpNombre.SetError(txtNombre, "El campo nombre es obligatorio.");
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text))
            {
                esValido = false;
                erpDescripcion.SetError(txtDescripcion, "El campo descripción es obligatorio.");
            }
            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var categoria = new Categoria();
                categoria.nombre = txtNombre.Text.Trim();
                categoria.descripcion = txtDescripcion.Text.Trim();
                categoria.usuarioRegistro = "LabSIS457";

                if (esNuevo)
                {
                    categoria.fechaRegistro = DateTime.Now;
                    categoria.estado = 1;
                    CategoriaCln.insertar(categoria);
                }
                else
                {
                    int index = dgvListaCategorias.CurrentCell.RowIndex;
                    categoria.id = Convert.ToInt32(dgvListaCategorias.Rows[index].Cells["id"].Value);
                    CategoriaCln.actualizar(categoria);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Categoría guardada correctamente", "::: Compumundo - Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
