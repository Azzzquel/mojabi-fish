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
    public partial class FormRankeds : Form
    {
        public FormRankeds()
        {
            InitializeComponent();
            CargarDatosRanked();
        }

        private void CargarDatosRanked()
        {
            try
            {
                datagridrankeds.DataSource = DatosDAL.ObtenerTablaRankeds();

                datagridrankeds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el ranking: " + ex.Message);
            }
        }
    }
}
