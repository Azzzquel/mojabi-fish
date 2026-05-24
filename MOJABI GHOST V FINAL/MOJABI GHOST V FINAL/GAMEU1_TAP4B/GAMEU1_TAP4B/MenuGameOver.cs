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
    public partial class MenuGameOver : Form
    {
        WindowsMediaPlayer sonidoBoton = new WindowsMediaPlayer();
        WindowsMediaPlayer musicaAmbiental = new WindowsMediaPlayer();

        public MenuGameOver()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            Bitmap imagenCursor = Properties.Resources.player_1;
            this.Cursor = CrearCursorDesdeImagen(imagenCursor);
            if (musicaAmbiental != null)
            {
                musicaAmbiental.URL = Path.Combine(Application.StartupPath, "Sonidos", "APLAUSOS.mp3");
                musicaAmbiental.settings.setMode("loop", true);
                musicaAmbiental.settings.volume = 30;
                musicaAmbiental.controls.play();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            this.DialogResult = DialogResult.Retry;
            this.Close();
            musicaAmbiental.controls.stop();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            this.DialogResult = DialogResult.Abort;
            this.Close();
            musicaAmbiental.controls.stop();
        }

        private void MenuGameOver_Load(object sender, EventArgs e)
        {

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
