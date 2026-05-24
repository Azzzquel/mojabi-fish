using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace GAMEU1_TAP4B
{
    public partial class registro : Form
    {
        int idSeleccionado = 0;

        public registro()
        {
            InitializeComponent();
            ActualizarTabla();
        }

        private void ActualizarTabla()
        {
            datagridPerfiles.DataSource = DatosDAL.LeerPerfiles();
        }

        private void btnguardarcambios_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtnombre.Text))
            {
                MessageBox.Show("Escribe un nombre");
                return;
            }

            //en este método, tanto para guardar como para actualizar, se llama al mismo método de la DAL. La lógica dentro de ese método se encargará de decidir si es una inserción o una actualización dependiendo del valor de idSeleccionado.
            int result = DatosDAL.GuardarPerfil(txtnombre.Text, idSeleccionado);

            if (result > 0)
            {
                MessageBox.Show("Datos guardados con éxito");
                txtnombre.Clear();
                idSeleccionado = 0; //importante resetear para el siguiente
                ActualizarTabla();

                //avisar al Menú Principal (Owner) que actualice los ComboBox
                if (this.Owner is MenuPrincipal menu)
                {
                    menu.ActualizarCombos();
                }
            }
        }

        private void datagridPerfiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //Usamos el índice 0 que es donde está el id
                idSeleccionado = Convert.ToInt32(datagridPerfiles.Rows[e.RowIndex].Cells[0].Value);
                //Usamos el índice 1 que es donde está el Nombre
                txtnombre.Text = datagridPerfiles.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un perfil de la tabla para eliminar");
                return;
            }

            var confirmar = MessageBox.Show("¿Seguro que quieres eliminar este perfil?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirmar == DialogResult.Yes)
            {
                DatosDAL.EliminarPerfil(idSeleccionado);
                ActualizarTabla();

                //avisar al Menú Principal (Owner) que actualice los ComboBox
                if (this.Owner is MenuPrincipal menu)
                {
                    menu.ActualizarCombos();
                }
                txtnombre.Clear();
                idSeleccionado = 0;
            }
        }

        

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string perfilaBuscar = txtbuscarperfil.Text.Trim(); //trim quita todos los espacios para evitar errores

            if (string.IsNullOrWhiteSpace(perfilaBuscar))
            {
                MessageBox.Show("Por favor escribe el nombre de un perfil para buscar");
                return;
            }

            try
            {
                var resultados = DatosDAL.Buscar(perfilaBuscar);

                datagridPerfiles.DataSource = resultados;

                if (resultados.Count == 0)
                {
                    MessageBox.Show("No se encontraron perfiles con ese nombre.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message);
            }
        }


    }
}
