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
    public partial class Pantalla2 : Form
    {

        WindowsMediaPlayer sonidoBoton = new WindowsMediaPlayer();
        public Pantalla2()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            WindowsMediaPlayer sonidoBoton = new WindowsMediaPlayer();
            string rutaMusica = Path.Combine(Application.StartupPath, "Sonidos", "MUSICA AMBIENTAL.mp3");
            
            Bitmap imagenCursor = Properties.Resources.player_1;
            this.Cursor = CrearCursorDesdeImagen(imagenCursor);
 
        }

        private void Pantalla2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (e.KeyChar == (char)Keys.Escape)
            {
                ReproducirClick();
                Form1 juego = new Form1();
                juego.Show();
                this.Hide();
                juego.FormClosed += (s, args) => Application.Exit();
            }

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
