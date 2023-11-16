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
    public partial class FrmCliente : Form
    {
        bool esNuevo = false;
        public FrmCliente()
        {
            InitializeComponent();
        }

        private void listar()
        {
            var clientes = ClienteCln.listarPa(txtParametro.Text.Trim());
            dgvListaClientes.DataSource = clientes;
            dgvListaClientes.Columns["id"].Visible = false;
            dgvListaClientes.Columns["estado"].Visible = false;
            dgvListaClientes.Columns["ci"].HeaderText = "Cédula de Identidad";
            dgvListaClientes.Columns["nombres"].HeaderText = "Nombres";
            dgvListaClientes.Columns["primerApellido"].HeaderText = "Primer Apellido";
            dgvListaClientes.Columns["segundoApellido"].HeaderText = "Segundo Apellido";
            dgvListaClientes.Columns["celular"].HeaderText = "Celular";
            dgvListaClientes.Columns["usuarioRegistro"].HeaderText = "Usuario";
            dgvListaClientes.Columns["fechaRegistro"].HeaderText = "Fecha de Registro";
            btnEditar.Enabled = clientes.Count > 0;
            btnEliminar.Enabled = clientes.Count > 0;
            if (clientes.Count > 0) dgvListaClientes.Rows[0].Cells["nombre"].Selected = true;
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            Size = new Size(961, 431);
            listar();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(961, 593);
            txtcedulaIdentidad.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(961, 593);

            int index = dgvListaClientes.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvListaClientes.Rows[index].Cells["id"].Value);
            var cliente = ClienteCln.get(id);
            txtcedulaIdentidad.Text = cliente.cedulaIdentidad;
            txtNombres.Text = cliente.nombres;
            txtprimerApellido.Text = cliente.primerApellido;
            txtsegundoApellido.Text = cliente.segundoApellido;
            txtCelular.Text = cliente.celular;
        }
    }
}
