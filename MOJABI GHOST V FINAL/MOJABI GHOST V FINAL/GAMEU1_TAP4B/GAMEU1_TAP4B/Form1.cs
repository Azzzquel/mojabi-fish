using System.Drawing.Drawing2D;
using System.IO;
using System.Media;
using System.Runtime.CompilerServices;
using WMPLib;
using static GAMEU1_TAP4B.PowerUp;
using Timer = System.Windows.Forms.Timer;

namespace GAMEU1_TAP4B
{
    public partial class Form1 : Form
    {
        #region VARIABLES Y OBJETOS GLOBALES

        // SONIDOS
        WindowsMediaPlayer sonidoComida = new WindowsMediaPlayer();
        WindowsMediaPlayer sonidoGolpe = new WindowsMediaPlayer();
        WindowsMediaPlayer musicaFondo = new WindowsMediaPlayer();
        WindowsMediaPlayer sonidoPowerUp = new WindowsMediaPlayer();
        WindowsMediaPlayer musicaArma = new WindowsMediaPlayer();

        // VALIDACIONES Y CONTADORES
        bool tutorialActivo = true;
        bool listoP1 = false, listoP2 = false;
        int puntosPez1 = 0, puntosPez2 = 0;
        int vidasPez = 3, vidasPez2 = 3;
        int comida, contadorComida;

        // ARMAS Y COLISIONES
        bool armaActiva = false;
        bool pezTieneArma = false;
        int quienTieneArma = 0; // 0 = nadie, 1 = Pez 1, 2 = Pez 2
        int contadorArma = 0;
        bool invencible = false;

        // CONTROLES DE MOVIMIENTO
        bool arriba, abajo, derecha, izquierda, arriba2, abajo2, derecha2, izquierda2;
        int velPez1 = 10;
        int velPez2 = 10;

        // POWER UPS
        int powerUpActual = 0; // 1:Vel, 2:Rel, 3:Atr, 4:Esc, 5:Vida
        PowerUp clasePowerUp = new PowerUp();
        bool atravesarActivoP1 = false, atravesarActivoP2 = false;
        bool escudoActivoP1 = false, escudoActivoP2 = false;
        bool tieneEscudoP1 = false, tieneEscudoP2 = false;

        // MAPA Y VISUALES
        Random rd = new Random();
        List<Rectangle> paredesActuales = new List<Rectangle>();
        int indiceMapa = 0;
        private float faseMovimiento = 0f;
        private float faseOlas = 0f;

        // BURBUJAS
        List<BurbujaAmbiental> listaBurbujasFondo = new List<BurbujaAmbiental>();
        int cantidadBurbujas = 15; // Ajusta este número si quieres más o menos burbujas

        public class BurbujaAmbiental
        {
            public float X, Y;
            public float Velocidad;
            public float Radio;
            public float DesfaseX; 
            public int Opacidad;

            public BurbujaAmbiental(Random rd, int anchoPantalla, int altoPantalla)
            {
                X = rd.Next(0, anchoPantalla);
                Y = rd.Next(altoPantalla, altoPantalla + 600); // Aparecen desde el fondo de forma escalonada
                Velocidad = (float)(rd.NextDouble() * 1.5 + 0.5); 
                Radio = rd.Next(15, 40); // Burbujas 
                DesfaseX = (float)(rd.NextDouble() * Math.PI * 2);
                Opacidad = rd.Next(80, 160); 
            }
        }

        #endregion

        #region CONSTRUCTOR Y CONFIGURACIÓN INICIAL

        public Form1()
        {
            InitializeComponent();

      
            // MUSICA DE FONDO
            string rutaMusicaFondo = Path.Combine(Application.StartupPath, "Sonidos", "MUSICA AMBIENTAL.mp3");
            if (File.Exists(rutaMusicaFondo))
            {
                musicaFondo.URL = rutaMusicaFondo;
                musicaFondo.settings.setMode("loop", true);
                musicaFondo.settings.volume = 30; 
                musicaFondo.controls.play();
            }

            // CONFIGURACIÓN DEL FORM
            this.FormBorderStyle = FormBorderStyle.FixedSingle; this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            pbPez.SizeMode = PictureBoxSizeMode.Zoom;
            pbPez2.SizeMode = PictureBoxSizeMode.Zoom;
            this.DoubleBuffered = true;

            // TIMER PRINCIPAL DE MOVIMIENTO Y COLISIONES
            timer1.Interval = 30;
            timer1.Start();

            // TIMER MAPAS
            timer2.Interval = 30000; // Cambia el mapa cada 30 segundos
            timer2.Tick += timer2_Tick;
            timer2.Start();

            // TIMER POWER-UPS
            timerPowerUp.Interval = 5000;
            timerPowerUp.Start();

            Bitmap imagenCursor = Properties.Resources.player_1;
            this.Cursor = CrearCursorDesdeImagen(imagenCursor);
            CargarMapa(0);
            this.Paint += new PaintEventHandler(Form1_Paint);
            lBLCOORDS .Visible = false; // Solo para depuración, muestra las coordenadas del mouse

            // INICIALIZAR BURBUJAS
            for (int i = 0; i < cantidadBurbujas; i++)
            {
                listaBurbujasFondo.Add(new BurbujaAmbiental(rd, this.ClientSize.Width, this.ClientSize.Height));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        #endregion

        #region EVENTOS DE TECLADO Y CONTROLES

        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // MOVIMIENTO DE TUTORIAL
            if (tutorialActivo)
            {
                if (!listoP1 && (e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D))
                {
                    listoP1 = true;
                    pbtutopez1.Visible = false;
                }

                if (!listoP2 && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right))
                {
                    listoP2 = true;
                    pbtutopez2.Visible = false;
                }

                if (listoP1 && listoP2)
                {
                    tutorialActivo = false;
                    this.Text = "¡QUE COMIENCE EL JUEGO!";
                }
            }

            // JUGADOR 1
            if (e.KeyCode == Keys.W) arriba = true;
            if (e.KeyCode == Keys.S) abajo = true;
            if (e.KeyCode == Keys.A) { izquierda = true; RedibujarPez1("izq"); }
            if (e.KeyCode == Keys.D) { derecha = true; RedibujarPez1("der"); }

            // JUGADOR 2
            if (e.KeyCode == Keys.Up) arriba2 = true;
            if (e.KeyCode == Keys.Down) abajo2 = true;
            if (e.KeyCode == Keys.Left) { izquierda2 = true; RedibujarPez2("izq"); }
            if (e.KeyCode == Keys.Right) { derecha2 = true; RedibujarPez2("der"); }

            // ESUCUDOS
            // JUGADOR 1
            if (e.KeyCode == Keys.Q && tieneEscudoP1 && !escudoActivoP1)
            {
                tieneEscudoP1 = false;
                pbMostrarEscudoJ1.Visible = false;
                lblpresionaQ.Visible = false;

                ReproducirSonidoEfecto("ESCUDO.mp3");
                escudoActivoP1 = true;
                RedibujarPez1(izquierda ? "izq" : "der");

                await Task.Delay(5000);

                escudoActivoP1 = false;
                RedibujarPez1(izquierda ? "izq" : "der");
            }

            // JUGADOR 2
            if (e.KeyCode == Keys.Enter && tieneEscudoP2 && !escudoActivoP2)
            {
                tieneEscudoP2 = false;
                pbMostrarEsucdoJ2.Visible = false;
                lblpresionaEnter.Visible = false;

                ReproducirSonidoEfecto("ESCUDO.mp3");
                escudoActivoP2 = true;
                RedibujarPez2(izquierda2 ? "izq" : "der");

                await Task.Delay(5000);

                escudoActivoP2 = false;
                RedibujarPez2(izquierda2 ? "izq" : "der");
            }
        }

        // AL SOLTAR LAS TECLAS, SE DETIENE EL MOVIMIENTO
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) arriba = false;
            if (e.KeyCode == Keys.A) izquierda = false;
            if (e.KeyCode == Keys.S) abajo = false;
            if (e.KeyCode == Keys.D) derecha = false;

            if (e.KeyCode == Keys.Up) arriba2 = false;
            if (e.KeyCode == Keys.Left) izquierda2 = false;
            if (e.KeyCode == Keys.Down) abajo2 = false;
            if (e.KeyCode == Keys.Right) derecha2 = false;
        }

        #endregion

        #region MOVIMIENTO Y LÍMITES

        //METODO PARA MOVER LOS PECES
        private void MoverPez()
        {
            if (abajo)
            {
                Rectangle futuro = new Rectangle(pbPez.Left, pbPez.Top + velPez1, pbPez.Width, pbPez.Height);
                if (futuro.Top < this.ClientSize.Height - pbPez.Height && PuedeMoverse(futuro, atravesarActivoP1))
                    pbPez.Top += velPez1;
            }
            if (arriba)
            {
                Rectangle futuro = new Rectangle(pbPez.Left, pbPez.Top - velPez1, pbPez.Width, pbPez.Height);
                // CAMBIO: Ahora el límite superior es 100 en lugar de 0
                if (futuro.Top > 100 && PuedeMoverse(futuro, atravesarActivoP1))
                    pbPez.Top -= velPez1;
            }
            if (derecha)
            {
                Rectangle futuro = new Rectangle(pbPez.Left + velPez1, pbPez.Top, pbPez.Width, pbPez.Height);
                if (futuro.Left < this.ClientSize.Width - pbPez.Width && PuedeMoverse(futuro, atravesarActivoP1))
                    pbPez.Left += velPez1;
            }
            if (izquierda)
            {
                Rectangle futuro = new Rectangle(pbPez.Left - velPez1, pbPez.Top, pbPez.Width, pbPez.Height);
                if (futuro.Left > 0 && PuedeMoverse(futuro, atravesarActivoP1))
                    pbPez.Left -= velPez1;
            }
        }

        private void MoverPez2()
        {
            if (abajo2)
            {
                Rectangle futuro = new Rectangle(pbPez2.Left, pbPez2.Top + velPez2, pbPez2.Width, pbPez2.Height);
                if (futuro.Top < this.ClientSize.Height - pbPez2.Height && PuedeMoverse(futuro, atravesarActivoP2))
                    pbPez2.Top += velPez2;
            }
            if (arriba2)
            {
                Rectangle futuro = new Rectangle(pbPez2.Left, pbPez2.Top - velPez2, pbPez2.Width, pbPez2.Height);
   
                if (futuro.Top > 100 && PuedeMoverse(futuro, atravesarActivoP2))
                    pbPez2.Top -= velPez2;
            }
            if (derecha2)
            {
                Rectangle futuro = new Rectangle(pbPez2.Left + velPez2, pbPez2.Top, pbPez2.Width, pbPez2.Height);
                if (futuro.Left < this.ClientSize.Width - pbPez2.Width && PuedeMoverse(futuro, atravesarActivoP2))
                    pbPez2.Left += velPez2;
            }
            if (izquierda2)
            {
                Rectangle futuro = new Rectangle(pbPez2.Left - velPez2, pbPez2.Top, pbPez2.Width, pbPez2.Height);
                if (futuro.Left > 0 && PuedeMoverse(futuro, atravesarActivoP2))
                    pbPez2.Left -= velPez2;
            }
        }
        // MÉTODO PARA VERIFICAR SI EL PEZ PUEDE MOVERSE A LA POSICIÓN FUTURA SIN CHOCAR CON PAREDES
        private bool PuedeMoverse(Rectangle futuroBounds, bool tienePoderAtravesar)
        {
            if (tienePoderAtravesar) return true;

            futuroBounds.Inflate(-1, -1);
            foreach (Rectangle pared in paredesActuales)
            {
                if (futuroBounds.IntersectsWith(pared)) return false;
            }
            return true;
        }

        // MÉTODO PARA REPOSICIONAR AL PEZ EN UNA ZONA SEGURA DESPUÉS DE SER GOLPEADO
        private void ReposicionarPez(PictureBox pez, string zona)
        {
            int margen = 25; 
            int alturaSuelo = this.ClientSize.Height - pez.Height - margen;

            if (zona == "izquierda-arriba") 
            {
                pez.Location = new Point(margen, alturaSuelo);
            }
            else 
            {
                pez.Location = new Point(this.ClientSize.Width - pez.Width - margen, alturaSuelo);
            }
        }
        // MÉTODO PARA LIBERAR AL PEZ SI QUEDA ATRAPADO EN UNA PARED DESPUÉS DE QUE TERMINE EL PODER DE ATRAVESAR
        private void LiberarPezSiEstaAtrapado(PictureBox pez, int posicionSeguraX, int posicionSeguraY)
        {
            foreach (Rectangle pared in paredesActuales)
            {
                if (pez.Bounds.IntersectsWith(pared))
                {
                    pez.Location = new Point(posicionSeguraX, posicionSeguraY);
                    break;
                }
            }
        }

        #endregion

        #region COLISIONES PRINCIPALES (COMIDA, ARMAS, GOLPES)

        private async void ChecarColisionesComida()
        {
            // LOGICA DE COMIDA
            if (!armaActiva && !pezTieneArma)
            {
                bool comidaTomada = false;
                if (pbPez.Bounds.IntersectsWith(pbComida.Bounds)) { puntosPez1++; comidaTomada = true; }
                else if (pbPez2.Bounds.IntersectsWith(pbComida.Bounds)) { puntosPez2++; comidaTomada = true; }

                if (comidaTomada)
                {
                    sonidoComida.URL = Path.Combine(Application.StartupPath, "Sonidos", "BOTONES.mp3");
                    sonidoComida.controls.play();
                    pbComida.Location = new Point(-100, -100);
                    ActualizarMarcador();

                    if (puntosPez1 >= 6 || puntosPez2 >= 6)
                    {
                        pbComida.Visible = false;
                        SpawnerArma();
                    }
                    else { SpawnerComida(); }
                }
                else 
                {
                    contadorComida++;
                    if (contadorComida >= 133) 
                    {
                        SpawnerComida();
                    }
                }
            }

            // LOGICA DEL ARMA
            if (armaActiva && !pezTieneArma)
            {
                contadorArma++;
                if (pbPez.Bounds.IntersectsWith(pbArma.Bounds) && puntosPez1 >= 6) { quienTieneArma = 1; RecogerArma(); }
                else if (pbPez2.Bounds.IntersectsWith(pbArma.Bounds) && puntosPez2 >= 6) { quienTieneArma = 2; RecogerArma(); }

                if (contadorArma >= 166) ReiniciarComidas();
            }

            // LOGICA DE ATAQUE
            if (pezTieneArma && !invencible)
            {
                contadorArma++;
                if (contadorArma >= 300) 
                {
                    musicaArma.controls.stop(); 
                    ReiniciarComidas();
                    this.Text = "¡TIEMPO AGOTADO! El arma expiró.";
                    return;
                }

                if (quienTieneArma == 1 && pbPez.Bounds.IntersectsWith(pbPez2.Bounds))
                {
                    if (escudoActivoP2) { this.Text = "¡BLOQUEADO! El escudo del Pez 2 te salvó."; }
                    else { vidasPez2--; ReposicionarPez(pbPez2, "derecha-abajo"); ProcesarGolpe(); }
                }
                else if (quienTieneArma == 2 && pbPez2.Bounds.IntersectsWith(pbPez.Bounds))
                {
                    if (escudoActivoP1) { this.Text = "¡BLOQUEADO! El escudo del Pez 1 te salvó."; }
                    else { vidasPez--; ReposicionarPez(pbPez, "izquierda-arriba"); ProcesarGolpe(); }
                }
            }

            // LOGICA DE POWER-UPS
            PictureBox[] listaPBs = { pbSuperVelocidad, pbRelentizar, pbAtravesarParedes, pbEscudo, pbVidaExtra };
            foreach (var pb in listaPBs)
            {
                if (pb.Visible && (pbPez.Bounds.IntersectsWith(pb.Bounds) || pbPez2.Bounds.IntersectsWith(pb.Bounds)))
                {
                    timerPowerUp.Stop();
                    int jugador = pbPez.Bounds.IntersectsWith(pb.Bounds) ? 1 : 2;
                    AplicarEfectoPowerUp(powerUpActual, jugador); // SE LLAMA A LA CLASE EXTERNA PARA APLICAR EL EFECTO SEGÚN EL TIPO DE POWER-UP
                    pb.Visible = false;
                    pb.Location = new Point(-100, -100);
                }
            }
        }

        private async void ProcesarGolpe()
        {
            // METODO PARA PROCESAR EL GOLPE CUANDO UN PEZ ES ATACADO CON EL ARMA
            musicaArma.controls.stop();

            sonidoGolpe.URL = Path.Combine(Application.StartupPath, "Sonidos", "GOLPE.mp3");
            sonidoGolpe.controls.play();

            pezTieneArma = false;
            await ActivarInvencibilidad();
            ReiniciarComidas();
        }

        private async Task ActivarInvencibilidad()
        {
            invencible = true;
            for (int i = 0; i < 5; i++)
            {
                pbPez.Visible = false;
                pbPez2.Visible = false;
                await Task.Delay(150);
                pbPez.Visible = true;
                pbPez2.Visible = true;
                await Task.Delay(150);
            }
            invencible = false;
        }

        #endregion

        #region SISTEMA DE POWER-UPS

        private void SpawnerPowerUps()
        {
            // TAMAÑO DE LOS POWER-UPS: 30x30, SE BUSCA UN PUNTO SEGURO PARA COLOCARLOS
            Point posicion = ObtenerPuntoSeguro(30, 30);

            // SE LLAMA AL MÉTODO DE LA CLASE EXTERNA PARA GENERAR UN POWER-UP ALEATORIO Y ASIGNARLE SUS PROPIEDADES
            clasePowerUp.GenerarPoderAleatorio(rd);

            // SE CONFIGURA EL PICTUREBOX CORRESPONDIENTE SEGÚN EL TIPO DE POWER-UP QUE ASIGNÓ LA CLASE EXTERNA
            if (clasePowerUp.Tipo == 1) ConfigurarPowerUp(pbSuperVelocidad, 1, posicion);
            else if (clasePowerUp.Tipo == 2) ConfigurarPowerUp(pbRelentizar, 2, posicion);
            else if (clasePowerUp.Tipo == 3) ConfigurarPowerUp(pbAtravesarParedes, 3, posicion);
            else if (clasePowerUp.Tipo == 4) ConfigurarPowerUp(pbEscudo, 4, posicion);
            else ConfigurarPowerUp(pbVidaExtra, 5, posicion);
        }

        private Point ObtenerPuntoSeguro(int ancho, int alto)
        {
            // AREA MAX Y MIN PARA GENERAR LOS POWER-UPS
            int maxX = this.ClientSize.Width - ancho - 20;
            int maxY = this.ClientSize.Height - alto - 20;

            // VARIABLES PARA ALMACENAR LAS COORDENADAS GENERADAS Y VERIFICAR SI SON VÁLIDAS
            int x = 0, y = 0;
            bool puntoValido = false;
            int intentos = 0;

            // BUCLE PARA GENERAR COORDENADAS HASTA ENCONTRAR PUNTO SEGURO O AGOTAR INTENTOS
            while (!puntoValido && intentos < 50)
            {
                x = rd.Next(20, maxX);
                y = rd.Next(110, maxY);
                Rectangle rectPrueba = new Rectangle(x, y, ancho, alto);

                // SE VERIFICA QUE EL RECTÁNGULO DEL POWER-UP NO INTERSECTE CON NINGUNA PARED EXISTENTE
                puntoValido = !paredesActuales.Any(p => p.IntersectsWith(rectPrueba));
                intentos++;
            }
            return new Point(x, y);
        }

        private void ConfigurarPowerUp(PictureBox pb, int tipo, Point pos)
        {
            // SE ASIGNA LA IMAGEN CORRESPONDIENTE SEGÚN EL TIPO DE POWER-UP
            pb.Location = pos;
            pb.Visible = true;
            powerUpActual = tipo;
        }

        private async void AplicarEfectoPowerUp(int tipo, int jugador)
        {
            // SE LLAMA AL MÉTODO DE LA CLASE EXTERNA PARA ASIGNAR LAS PROPIEDADES DEL POWER-UP ACTUAL SEGÚN SU TIPO
            clasePowerUp.AsignarPropiedades(tipo);

            // SONIDO
            if (!string.IsNullOrEmpty(clasePowerUp.Sonido))
            {
                string ruta = Path.Combine(Application.StartupPath, "Sonidos", clasePowerUp.Sonido);
                if (File.Exists(ruta))
                {
                    sonidoPowerUp.URL = ruta;
                    sonidoPowerUp.controls.play();
                }
            }

            // LOGICA DE LOS POWERUPS
            switch (tipo)
            {
                case 1: // SUPER VELOCIDAD
                    if (jugador == 1) velPez1 = 20; else velPez2 = 20;
                    await Task.Delay(clasePowerUp.Duracion);
                    velPez1 = velPez2 = 10;
                    break;

                case 2: // RELENTIZAR
                    if (jugador == 1) velPez2 = 5; else velPez1 = 5;
                    await Task.Delay(clasePowerUp.Duracion);
                    velPez1 = velPez2 = 10;
                    break;

                case 3: // ATRAVESAR PAREDES
                    if (jugador == 1) atravesarActivoP1 = true; else atravesarActivoP2 = true;
                    await Task.Delay(clasePowerUp.Duracion);

                    // VALIDAR QUE EL PEZ NO QUEDE ATRAVESADO EN UNA PARED AL TERMINAR EL PODER
                    if (jugador == 1)
                    {
                        while (!PuedeMoverse(pbPez.Bounds, false)) { await Task.Delay(100); }
                        atravesarActivoP1 = false;
                    }
                    else
                    {
                        while (!PuedeMoverse(pbPez2.Bounds, false)) { await Task.Delay(100); }
                        atravesarActivoP2 = false;
                    }
                    break;

                case 4: // ESCUDO
                    if (jugador == 1)
                    {
                        tieneEscudoP1 = true;
                        pbMostrarEscudoJ1.Visible = true;
                        lblpresionaQ.Visible = true;
                    }
                    else
                    {
                        tieneEscudoP2 = true;
                        pbMostrarEsucdoJ2.Visible = true;
                        lblpresionaEnter.Visible = true;
                    }
                    break;

                case 5: // VIDA EXTRA
                    if (jugador == 1 && vidasPez < 3) { vidasPez++; ActualizarVidasVisuales(1, vidasPez); }
                    else if (jugador == 2 && vidasPez2 < 3) { vidasPez2++; ActualizarVidasVisuales(2, vidasPez2); }
                    ActualizarMarcador();
                    break;
            }

            // TIEMPO DE REAPARICIÓN DE POWER-UPS
            await Task.Delay(1000);
            timerPowerUp.Start();
        }

        #endregion

        #region SPAWNERS Y REINICIOS

        private void SpawnerComida()
        {
            if (!armaActiva && !pezTieneArma)
            {
                // SE LLAMA AL MÉTODO PARA OBTENER UN PUNTO SEGURO Y COLOCAR LA COMIDA ALLÍ
                pbComida.Location = ObtenerPuntoSeguro(pbComida.Width, pbComida.Height);
                pbComida.Visible = true;
                contadorComida = 0;
            }
        }

        private void SpawnerArma()
        {
            // SE LLAMA AL MÉTODO PARA OBTENER UN PUNTO SEGURO Y COLOCAR EL ARMA ALLÍ
            pbArma.Location = ObtenerPuntoSeguro(pbArma.Width, pbArma.Height);
            pbArma.Visible = true;
            armaActiva = true;
            contadorArma = 0;
        }

        private void RecogerArma()
        {
            // SE ASIGNA EL ARMA AL PEZ CORRESPONDIENTE Y SE ACTUALIZAN LOS VISUALES Y SONIDOS
            pezTieneArma = true;
            armaActiva = false;
            pbArma.Visible = false;
            pbArma.Location = new Point(-100, -100);
            contadorArma = 0;

            RedibujarPez1(izquierda ? "izq" : "der");
            RedibujarPez2(izquierda2 ? "izq" : "der");

            this.Text = "¡EL PEZ " + quienTieneArma + " HA RECOGIDO EL ARMA!";

            string rutaArma = Path.Combine(Application.StartupPath, "Sonidos", "MUSICA 1.5 (CUANDO TENGAN EL ARMA).mp3");

            if (File.Exists(rutaArma))
            {
                musicaFondo.controls.pause();
                musicaArma.URL = rutaArma;
                musicaArma.controls.play();
            }
        }

        private void ReiniciarComidas() //heyheyhey borrame eso, william
        {
            // VALIDACIÓN PRIMERO: SI ALGÚN PEZ SE QUEDÓ SIN VIDAS, SE TERMINA EL JUEGO Y SE MUESTRA LA PANTALLA DE GAME OVER CORRESPONDIENTE
            if (vidasPez <= 0 || vidasPez2 <= 0)
            {
                timer1.Stop();
                timer2.Stop();
                timerPowerUp.Stop();
                musicaFondo.controls.stop();
                musicaArma.controls.stop();

                //guardar resultado de la partida
                try
                {
                    Partida resultadoPartida = new Partida();

                    if (vidasPez2 <= 0) //gano el Pez 1
                    {
                        resultadoPartida.id_ganador = SesionDeJuego.IdJugador1;
                        resultadoPartida.id_perdedor = SesionDeJuego.IdJugador2;
                        MessageBox.Show($"Gano {SesionDeJuego.NombreJ1}");
                    }
                    else if (vidasPez <= 0) //gano el Pez 2
                    {
                        resultadoPartida.id_ganador = SesionDeJuego.IdJugador2;
                        resultadoPartida.id_perdedor = SesionDeJuego.IdJugador1;
                        MessageBox.Show($"Gano {SesionDeJuego.NombreJ2}");
                    }

                    // Llamamos a DatosDAL para guardar en la tabla TB_partidas
                    DatosDAL.InsertarPartida(resultadoPartida);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se pudo guardar el historial: " + ex.Message);
                }

                Form pantallaGameOver = null;
                if (vidasPez2 <= 0)
                {
                    pantallaGameOver = new MenuGameOver();
                }
                else if (vidasPez <= 0)
                {
                    pantallaGameOver = new GameOver2();
                }

                //  SE MUESTRA LA PANTALLA DE GAME OVER CORRESPONDIENTE Y SE DA LA OPCIÓN DE REINICIAR O SALIR
                if (pantallaGameOver != null)
                {
                    DialogResult resultado = pantallaGameOver.ShowDialog();

                    if (resultado == DialogResult.Retry)
                    {
                        Application.Restart();
                    }
                    else
                    {
                        Application.Exit(); 
                    }
                }

                return;
            }

            // SI NADIE PERDIÓ TODAVÍA, SE REINICIAN LAS COMIDAS Y EL ARMA PARA LA SIGUIENTE RONDA
            puntosPez1 = 0;
            puntosPez2 = 0;
            ActualizarMarcador();

            pbArma.Visible = false;
            pbArma.Location = new Point(-100, -100);
            armaActiva = false;
            pezTieneArma = false;
            quienTieneArma = 0;
            contadorArma = 0;

            velPez1 = 10;
            velPez2 = 10;
            escudoActivoP1 = false;
            escudoActivoP2 = false;

            pbComida.Visible = true;
            contadorComida = 0;
            SpawnerComida();

            musicaArma.controls.stop();
            musicaFondo.controls.play();

            RedibujarPez1("der");
            RedibujarPez2("izq");

            this.Text = "¡Nueva ronda! Recolecten 6 comidas.";
        }

        #endregion

        #region ACTUALIZACIÓN DE UI Y SPRITES

        private void ActualizarMarcador()
        {
            // ACTUALIZA EL TÍTULO DEL FORM CON LOS PUNTOS Y VIDAS DE CADA PEZ
            this.Text = $"Pez 1: {puntosPez1} (Vidas: {vidasPez}) | Pez 2: {puntosPez2} (Vidas: {vidasPez2})";
            lblPuntosP1.Text = "Comida P1: " + puntosPez1 + "/6";
            lblPuntosP2.Text = "Comida P2: " + puntosPez2 + "/6";

            // VIDAS
            pbVida3pez1.Visible = (vidasPez >= 3);
            pbVida2pez1.Visible = (vidasPez >= 2);
            pbVida1pez1.Visible = (vidasPez >= 1);

            pbVida1pez2.Visible = (vidasPez2 >= 3);
            pbVida2pez2.Visible = (vidasPez2 >= 2);
            pbVida3pez2.Visible = (vidasPez2 >= 1);
        }

        private void ActualizarVidasVisuales(int jugador, int cantidad)
        {
            // ACTUALIZA LOS PICTUREBOX DE VIDAS SEGÚN EL JUGADOR Y LA CANTIDAD DE VIDAS QUE LE QUEDAN
            if (jugador == 1)
            {
                if (cantidad >= 1) pbVida1pez1.Visible = true;
                if (cantidad >= 2) pbVida2pez1.Visible = true;
                if (cantidad >= 3) pbVida3pez1.Visible = true;
            }
            else
            {
                if (cantidad >= 1) pbVida1pez2.Visible = true;
                if (cantidad >= 2) pbVida2pez2.Visible = true;
                if (cantidad >= 3) pbVida3pez2.Visible = true;
            }
        }

        // SE REDBIBUJA EL PEZ DE ACUERDO A LA DIRECCIÓN Y LOS ESTADOS ACTUALES (ARMA, ESCUDO, VELOCIDAD, ATRAVESAR)
        private void RedibujarPez1(string direccion)
        {
            bool esIzquierda = (direccion == "izq");

            if (pezTieneArma && quienTieneArma == 1) pbPez.Image = esIzquierda ? Properties.Resources.pez_1_arma_volteado : Properties.Resources.pez_1_arma;
            else if (escudoActivoP1) pbPez.Image = esIzquierda ? Properties.Resources.pez_1_escudo_invertido : Properties.Resources.pez_1_escudo;
            else if (velPez1 > 10) pbPez.Image = esIzquierda ? Properties.Resources.pez_1_velocidad_invertido : Properties.Resources.pez_1_velocidad;
            else if (atravesarActivoP1) pbPez.Image = esIzquierda ? Properties.Resources.player_1_fabtasma_invertido : Properties.Resources.player_1_fantasma;
            else pbPez.Image = esIzquierda ? Properties.Resources.player_1_volteado : Properties.Resources.player_1;
        }

        private void RedibujarPez2(string direccion)
        {
            bool esIzquierda = (direccion == "izq");

            if (pezTieneArma && quienTieneArma == 2) pbPez2.Image = esIzquierda ? Properties.Resources.pez_2_arma_volteado : Properties.Resources.pez_2_arma;
            else if (escudoActivoP2) pbPez2.Image = esIzquierda ? Properties.Resources.pez_2_burbuja_invertida : Properties.Resources.pez_2_burbuja;
            else if (velPez2 > 10) pbPez2.Image = esIzquierda ? Properties.Resources.pez_2_velocidad_invertido : Properties.Resources.pez_2_velocidad;
            else if (atravesarActivoP2) pbPez2.Image = esIzquierda ? Properties.Resources.pez_2_fantasma_invertido : Properties.Resources.pez_2_fantasma;
            else pbPez2.Image = esIzquierda ? Properties.Resources.player_2_volteado : Properties.Resources.player_2;
        }

        #endregion

        #region GRÁFICOS, MAPAS Y TIMERS SECUNDARIOS

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics lienzo = e.Graphics;
            lienzo.SmoothingMode = SmoothingMode.AntiAlias;

            using SolidBrush brocha = new SolidBrush(Color.DarkSlateGray);
            using SolidBrush arenaClara = new SolidBrush(Color.FromArgb(255, 235, 150));
            using SolidBrush arenaOscura = new SolidBrush(Color.FromArgb(240, 190, 40));
            using SolidBrush colorRoca = new SolidBrush(Color.FromArgb(100, 110, 140));
            using SolidBrush colorAlga = new SolidBrush(Color.FromArgb(0, 180, 80));
            using SolidBrush colorCoral = new SolidBrush(Color.FromArgb(180, 100, 255));
            using SolidBrush colorEstrella = new SolidBrush(Color.FromArgb(255, 255, 200));
            using SolidBrush colorBarcoHundido = new SolidBrush(Color.FromArgb(120, 20, 40, 80));

            BarcoHundido(lienzo, colorBarcoHundido);
            ArenaFondo(lienzo, arenaClara);
            ArenaFrente(lienzo, arenaOscura);
            PiedraIzquierda(lienzo, colorRoca);
            PiedraDerecha(lienzo, colorRoca);
            Alga(lienzo, colorAlga, 400, 720, 0.8f, 0f);
            Alga(lienzo, colorAlga, 600, 740, 1.0f, 1.5f);
            Alga(lienzo, colorAlga, 880, 730, 0.7f, 3.0f);
            Coral(lienzo, colorCoral, 100, 680);
            EstrellaDeMar(lienzo, colorEstrella, 1000, 660);

            //BURBUJAS DE FONDO

            foreach (var b in listaBurbujasFondo)
            {
                //PINCELES Y PLUMAS PARA LAS BURBUJAS: RELLENO INTERIOR, CONTORNO Y BRILLO
                using (SolidBrush brochaRelleno = new SolidBrush(Color.FromArgb(b.Opacidad / 3, Color.LightCyan)))
                using (Pen plumaBorde = new Pen(Color.FromArgb(b.Opacidad, Color.White), 2))
                using (SolidBrush brochaBrillo = new SolidBrush(Color.FromArgb(b.Opacidad, Color.White)))
                {
                    // RELLENO
                    lienzo.FillEllipse(brochaRelleno, b.X, b.Y, b.Radio * 2, b.Radio * 2);
                    // CONTORNO
                    lienzo.DrawEllipse(plumaBorde, b.X, b.Y, b.Radio * 2, b.Radio * 2);
                    // BRILL0
                    lienzo.FillEllipse(brochaBrillo, b.X + (b.Radio * 1.4f), b.Y + (b.Radio * 0.4f), b.Radio * 0.4f, b.Radio * 0.4f);
                }
            }

            foreach (Rectangle pared in paredesActuales)
            {
                lienzo.FillRectangle(brocha, pared);
            }

    
        }

        // MÉTODOS DEL DIBUJO DEL FONDO
        private void Alga(Graphics lienzo, SolidBrush brush, int x, int y, float escala, float desfase)
        {
            float oscilacion = MathF.Sin(faseMovimiento + desfase) * 15 * escala;
            Point[] puntossalgas = {
                new Point(x, y), new Point(x - (int)(20 * escala), y - (int)(50 * escala)),
                new Point(x + (int)(10 * escala + oscilacion), y - (int)(100 * escala)),
                new Point(x - (int)(10 * escala - oscilacion), y - (int)(150 * escala)),
                new Point(x + (int)(25 * escala), y - (int)(80 * escala)), new Point(x + (int)(10 * escala), y)
            };
            lienzo.FillClosedCurve(brush, puntossalgas);
        }

        private void ArenaFondo(Graphics lienzo, SolidBrush brush)
        {
            int osc = (int)(MathF.Sin(faseOlas) * 5);
            Point[] puntosarenafondo = {
                new Point(0, 580 + osc), new Point(300, 550 - osc), new Point(600, 570 + osc),
                new Point(900, 540 - osc), new Point(1280, 560), new Point(1280, 720), new Point(0, 720)
            };
            lienzo.FillClosedCurve(brush, puntosarenafondo);
        }

        private void ArenaFrente(Graphics lienzo, SolidBrush brush)
        {
            Point[] puntosarenafrente = {
                new Point(0, 650), new Point(400, 630), new Point(800, 660),
                new Point(1280, 620), new Point(1280, 720), new Point(0, 720)
            };
            lienzo.FillClosedCurve(brush, puntosarenafrente);
        }

        private void PiedraIzquierda(Graphics lienzo, SolidBrush brush)
        {
            Point[] puntosrocaizquierda = {
                new Point(-50, 450), new Point(50, 480), new Point(120, 550),
                new Point(150, 680), new Point(140, 750), new Point(-50, 750)
            };
            lienzo.FillClosedCurve(brush, puntosrocaizquierda);
        }

        private void BarcoHundido(Graphics lienzo, SolidBrush brush)
        {
            Point[] puntosCasco = {
                new Point(350, 430), new Point(650, 445), new Point(1050, 450),
                new Point(1300, 420), new Point(1150, 680), new Point(500, 680), new Point(350, 480),
            };
            lienzo.FillClosedCurve(brush, puntosCasco);

            Point[] mastilGigante = { new Point(820, 450), new Point(850, 50), new Point(880, 451) };
            lienzo.FillPolygon(brush, mastilGigante);

            Point[] mastilSegundo = { new Point(1000, 451), new Point(1100, 180), new Point(1030, 451) };
            lienzo.FillPolygon(brush, mastilSegundo);

            Point[] viga1 = { new Point(780, 200), new Point(920, 220), new Point(920, 235), new Point(780, 215) };
            lienzo.FillPolygon(brush, viga1);

            Point[] viga2 = { new Point(1010, 310), new Point(1150, 340), new Point(1150, 350), new Point(1010, 320) };
            lienzo.FillPolygon(brush, viga2);
        }

        private void PiedraDerecha(Graphics lienzo, SolidBrush brush)
        {
            Point[] puntosrocaderecha = {
                new Point(1280, 400), new Point(1100, 430), new Point(950, 550),
                new Point(930, 720), new Point(1280, 720)
            };
            lienzo.FillClosedCurve(brush, puntosrocaderecha);
        }

        private void Coral(Graphics lienzo, SolidBrush brush, int x, int y) => lienzo.FillEllipse(brush, x, y, 60, 40);

        private void EstrellaDeMar(Graphics lienzo, SolidBrush brush, int x, int y)
        {
            Point[] puntosestrella = {
                new Point(x, y), new Point(x+20, y+10), new Point(x+40, y),
                new Point(x+30, y+30), new Point(x+50, y+50), new Point(x+25, y+40),
                new Point(x, y+50), new Point(x+10, y+25)
            };
            lienzo.FillPolygon(brush, puntosestrella);
        }

        private void CargarMapa(int indice)
        {
            paredesActuales.Clear();

            if (indice == 0)
            {
                // MAPA 1
                paredesActuales.Add(new Rectangle(500, 360, 280, 40));
                paredesActuales.Add(new Rectangle(620, 240, 40, 280));

                paredesActuales.Add(new Rectangle(150, 200, 150, 40));
                paredesActuales.Add(new Rectangle(150, 200, 40, 150));
                paredesActuales.Add(new Rectangle(980, 200, 150, 40));
                paredesActuales.Add(new Rectangle(1090, 200, 40, 150));

                paredesActuales.Add(new Rectangle(150, 580, 150, 40));
                paredesActuales.Add(new Rectangle(150, 470, 40, 150));
                paredesActuales.Add(new Rectangle(980, 580, 150, 40));
                paredesActuales.Add(new Rectangle(1090, 470, 40, 150));
            }
            else if (indice == 1)
            {
                // MAPA 2
                paredesActuales.Add(new Rectangle(150, 220, 400, 40));
                paredesActuales.Add(new Rectangle(730, 220, 400, 40));

                paredesActuales.Add(new Rectangle(150, 400, 400, 40));
                paredesActuales.Add(new Rectangle(730, 400, 400, 40));

                paredesActuales.Add(new Rectangle(150, 580, 400, 40));
                paredesActuales.Add(new Rectangle(730, 580, 400, 40));
            }
            else if (indice == 2)
            {
                // MAPA 3
                paredesActuales.Add(new Rectangle(300, 200, 40, 150));
                paredesActuales.Add(new Rectangle(300, 470, 40, 150));

                paredesActuales.Add(new Rectangle(940, 200, 40, 150));
                paredesActuales.Add(new Rectangle(940, 470, 40, 150));

                paredesActuales.Add(new Rectangle(620, 250, 40, 220));
            }
            else if (indice == 3)
            {
                // MAPA 4
                paredesActuales.Add(new Rectangle(200, 200, 200, 150));
                paredesActuales.Add(new Rectangle(880, 200, 200, 150));
                paredesActuales.Add(new Rectangle(540, 470, 200, 150));
            }
            else if (indice == 4)
            {
                // MAPA 5
                paredesActuales.Add(new Rectangle(620, 200, 40, 100));
                paredesActuales.Add(new Rectangle(400, 300, 480, 40));

                paredesActuales.Add(new Rectangle(620, 520, 40, 100));
                paredesActuales.Add(new Rectangle(400, 480, 480, 40));

                paredesActuales.Add(new Rectangle(150, 350, 40, 120));
                paredesActuales.Add(new Rectangle(1090, 350, 40, 120));
            }
            else if (indice == 5)
            {
                // MAPA 6
                paredesActuales.Add(new Rectangle(0, 380, 450, 40));
                paredesActuales.Add(new Rectangle(830, 380, 450, 40));

                paredesActuales.Add(new Rectangle(620, 200, 40, 140)); 
                paredesActuales.Add(new Rectangle(620, 480, 40, 140)); 
            }

            this.Invalidate();

            // SI EL PEZ QUEDA ATRAPADO EN UNA PARED POR EL CAMBIO DE MAPA, SE LE REPOSICIONA EN UN LUGAR SEGURO
            LiberarPezSiEstaAtrapado(pbPez, 20, 115);
            LiberarPezSiEstaAtrapado(pbPez2, 1190, 640); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoverPez();
            MoverPez2();

            if (!tutorialActivo)
            {
                ChecarColisionesComida();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            indiceMapa++;
            if (indiceMapa > 5)
            {
                indiceMapa = 0;
            }
            CargarMapa(indiceMapa);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            faseOlas += 0.05f;
            faseMovimiento += 0.07f;
            if (faseOlas > MathF.PI * 2) faseOlas -= MathF.PI * 2;
            if (faseMovimiento > MathF.PI * 2) faseMovimiento -= MathF.PI * 2;

            // BURBUJAS
            foreach (var burbuja in listaBurbujasFondo)
            {
                burbuja.Y -= burbuja.Velocidad; // MOVIMIENTO HACIA ARRIBA SEGÚN SU VELOCIDAD                     
                burbuja.X += (float)Math.Sin((burbuja.Y / 60f) + burbuja.DesfaseX) * 0.8f;
                if (burbuja.Y + (burbuja.Radio * 2) < 0)
                {
                    burbuja.Y = this.ClientSize.Height + rd.Next(10, 100);
                    burbuja.X = rd.Next(0, this.ClientSize.Width);
                }
            }

            this.Invalidate();
        }

        private void timerPowerUp_Tick(object sender, EventArgs e)
        {
            if (!pbSuperVelocidad.Visible && !pbRelentizar.Visible &&
                !pbAtravesarParedes.Visible && !pbEscudo.Visible && !pbVidaExtra.Visible)
            {
                SpawnerPowerUps();
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // MUESTRA LAS COORDENADAS DEL MOUSE EN UN LABEL PARA AYUDAR EN LA UBICACIÓN DE LOS POWER-UPS DURANTE EL DESARROLLO
            lBLCOORDS.Text = "X: " + e.X + " Y: " + e.Y;
        }

        // METODO PARA REPRODUCIR EL SONIDO CORRESPONDIENTE CUANDO SE RECOGE UN POWER-UP
        private void ReproducirSonidoEfecto(string archivo)
        {
            string ruta = Path.Combine(Application.StartupPath, "Sonidos", archivo);
            if (File.Exists(ruta))
            {
                sonidoPowerUp.URL = ruta;
                sonidoPowerUp.controls.play();
            }
        }
        private Cursor CrearCursorDesdeImagen(Bitmap bmp)
        {
            IntPtr ptr = bmp.GetHicon();
            return new Cursor(ptr);
        }

        #endregion
    }
}
