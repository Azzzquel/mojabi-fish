using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;
using static GAMEU1_TAP4B.Usuarios;

namespace GAMEU1_TAP4B
{
    public partial class MenuPrincipal : Form
    {
        // Reproductores
        WindowsMediaPlayer musicaAmbiental = new WindowsMediaPlayer();
        WindowsMediaPlayer sonidoBoton = new WindowsMediaPlayer();

        public MenuPrincipal()
        {
            InitializeComponent();
            CargarComboBoxes();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            string rutaMusica = Path.Combine(Application.StartupPath, "Sonidos", "MUSICA MENUS.mp3");
            if (File.Exists(rutaMusica))
            {
                musicaAmbiental.URL = rutaMusica;
                musicaAmbiental.settings.setMode("loop", true);
                musicaAmbiental.settings.volume = 80;
                musicaAmbiental.controls.play();
            }

            Bitmap imagenCursor = Properties.Resources.player_1;
            this.Cursor = CrearCursorDesdeImagen(imagenCursor);
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            // Validamos que hayan seleccionado a alguien
            if (cmbjugador1.SelectedValue == null || cmbjugador2.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona dos perfiles");
                return;
            }

            // Validamos que no sea la misma persona
            if ((int)cmbjugador1.SelectedValue == (int)cmbjugador2.SelectedValue)
            {
                MessageBox.Show("Selecciona perfiles diferentes para cada jugador");
                return;
            }

            // Guardamos los datos en nuestra clase estática "SesionJuego"
            SesionDeJuego.IdJugador1 = (int)cmbjugador1.SelectedValue;
            SesionDeJuego.IdJugador2 = (int)cmbjugador2.SelectedValue;
            SesionDeJuego.NombreJ1 = cmbjugador1.Text;
            SesionDeJuego.NombreJ2 = cmbjugador2.Text;
            ReproducirClick();
            Pantalla1 juego = new Pantalla1();
            juego.Show();
            this.Hide();
            juego.FormClosed += (s, args) => Application.Exit();
            musicaAmbiental.controls.stop();
        }

        private void btnOpciones_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            opciones opciones = new opciones();
            opciones.Show();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            this.DialogResult = DialogResult.Abort;
            this.Close();
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

        private Cursor CrearCursorDesdeImagen(Bitmap bmp)
        {
            IntPtr ptr = bmp.GetHicon();
            return new Cursor(ptr);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            CadenaDeConeccion.ObtenerConeccion();
        }

        private void btnregistrarse_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            string nombreJugador = Microsoft.VisualBasic.Interaction.InputBox("Ingresa tu nombre de usuario:", "Registro de un nuevo Perfil");

            if (!string.IsNullOrWhiteSpace(nombreJugador))
            {
                int res = DatosDAL.GuardarPerfil(nombreJugador, 0);
                if (res > 0)
                {
                    MessageBox.Show("Perfil creado con éxito");
                    ActualizarCombos();

                }
            }
        }

        private void btnOTCIONES_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            //crear un nuevo formulario para mostrar el historial de partidas en un DataGridView  y cargar los datos desde la base de datos usando el método ObtenerHistorialPartidas() de DatosDAL
            Form frmHistorial = new Form { Text = "Historial de Partidas", Width = 600, StartPosition = FormStartPosition.CenterScreen };
            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill, DataSource = DatosDAL.ObtenerHistorialPartidas(), ReadOnly = true };
            frmHistorial.Controls.Add(dgv);
            frmHistorial.ShowDialog();

        }

        private void btnperfiles_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            registro nuevoFormulario = new registro();
            nuevoFormulario.Owner = this;
            nuevoFormulario.Show();
        }

        //Método para llenar los ComboBox
        private void CargarComboBoxes()
        {
            try
            {
                //Obtenemos los perfiles de la base de datos
                var perfilesJ1 = DatosDAL.LeerPerfiles();
                var perfilesJ2 = DatosDAL.LeerPerfiles(); // Copia para el segundo combo

                //configurar ComboBox Jugador 1
                cmbjugador1.DataSource = perfilesJ1;
                cmbjugador1.DisplayMember = "nombre";      // Lo que se ve
                cmbjugador1.ValueMember = "id_usuarios";   // El ID real

                //configurar ComboBox Jugador 2
                cmbjugador2.DataSource = perfilesJ2;
                cmbjugador2.DisplayMember = "nombre";
                cmbjugador2.ValueMember = "id_usuarios";
            }
            catch (Exception ex)
            {
                //si no hay internet o falla la base de datos
                MessageBox.Show("Error al cargar perfiles: " + ex.Message);
            }
        }

        //este método vive en MenuPrincipal.cs
        public void ActualizarCombos()
        {
            var perfiles = DatosDAL.LeerPerfiles();

            cmbjugador1.DataSource = null;
            cmbjugador1.DataSource = new List<Usuarios>(perfiles);
            cmbjugador1.DisplayMember = "nombre";
            cmbjugador1.ValueMember = "id_usuarios";

            cmbjugador2.DataSource = null;
            cmbjugador2.DataSource = new List<Usuarios>(perfiles);
            cmbjugador2.DisplayMember = "nombre";
            cmbjugador2.ValueMember = "id_usuarios";
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnrankeds_Click(object sender, EventArgs e)
        {
            ReproducirClick();
            FormRankeds nuevoFormulario = new FormRankeds();
            nuevoFormulario.Owner = this;
            nuevoFormulario.Show();
        }
    }
}
