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
    public partial class opciones : Form
    {
        WindowsMediaPlayer sonidoBoton = new WindowsMediaPlayer();
        public opciones()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            Bitmap imagenCursor = Properties.Resources.player_1;
            this.Cursor = CrearCursorDesdeImagen(imagenCursor);
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private Cursor CrearCursorDesdeImagen(Bitmap bmp)
        {
            IntPtr ptr = bmp.GetHicon();
            return new Cursor(ptr);
        }
        private void ReproducirClick()
        {
            string rutaClick = Path.Combine(Application.StartupPath, "Sonidos", "BOTONES.mp3");
            if (File.Exists(rutaClick))
            {
                sonidoBoton.URL = rutaClick;
                sonidoBoton.controls.play();
            }
        }
    }
}
