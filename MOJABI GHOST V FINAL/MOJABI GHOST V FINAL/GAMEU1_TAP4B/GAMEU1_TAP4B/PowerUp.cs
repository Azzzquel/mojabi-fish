using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAMEU1_TAP4B
{
    internal class PowerUp
    {
        // ATRIBUTOS
        public int Tipo { get; set; }
        public int Duracion { get; set; }
        public string Sonido { get; set; }

        public PowerUp() { }

        //  PROBABILIDAD DE GENERACIÓN DE POWER-UPS:
        public void GenerarPoderAleatorio(Random rd)
        {
            int probabilidad = rd.Next(1, 101);

            if (probabilidad <= 20) Tipo = 1;
            else if (probabilidad <= 40) Tipo = 2;
            else if (probabilidad <= 60) Tipo = 3;
            else if (probabilidad <= 90) Tipo = 4;
            else Tipo = 5;

            // SE LLENAN LAS PROPIEDADES CORRESPONDIENTES AL TIPO DE POWER-UP GENERADO
            AsignarPropiedades(Tipo);
        }

        // FUNCION PARA ASIGNAR PROPIEDADES SEGÚN EL TIPO DE POWER-UP GENERADO
        public void AsignarPropiedades(int tipoAsignado)
        {
            Tipo = tipoAsignado;
            if (Tipo == 3) Duracion = 5000;
            else Duracion = 5000;

            // Asignar Sonido
            switch (Tipo)
            {
                case 1: Sonido = "VELOCIDAD.mp3"; break;
                case 2: Sonido = "RELENTIZAR.mp3"; break;
                case 3: Sonido = "FANTASMA.mp3"; break;
                case 4: Sonido = "ESCUDO.mp3"; break;
                case 5: Sonido = "VIDA.mp3"; break;
                default: Sonido = ""; break; // El fantasma u otros sin sonido específico
            }
        }
    }

}
