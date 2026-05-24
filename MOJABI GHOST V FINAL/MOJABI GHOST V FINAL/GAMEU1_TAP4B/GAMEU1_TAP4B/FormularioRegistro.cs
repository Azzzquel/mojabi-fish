using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAMEU1_TAP4B
{
    public partial class FormularioRegistro : Form
    {
        int idSeleccionado = 0; // 0 significa "Nuevo Perfil"

        public FormularioRegistro()
        {
            InitializeComponent();
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtnombre.Text))
            {
                MessageBox.Show("Escribe un nombre");
                return;
            }

            // Enviamos el nombre y el ID actual (si es 0, insertará; si tiene valor, actualizará)
            int result = DatosDAL.GuardarPerfil(txtnombre.Text, idSeleccionado);

            if (result > 0)
            {
                MessageBox.Show("¡Datos guardados con éxito!");
                txtnombre.Clear();
                idSeleccionado = 0; // Importante resetear para el siguiente
                //ActualizarTabla(); // Tu método que carga el DataGridView
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
