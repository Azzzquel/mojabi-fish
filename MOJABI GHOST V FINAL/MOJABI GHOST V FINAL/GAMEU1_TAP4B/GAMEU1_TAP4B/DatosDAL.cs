using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAMEU1_TAP4B
{
    internal class DatosDAL
    {
        //Método para insertar o actualizar
        public static int GuardarPerfil(string nombre, int id)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
                {
                    string query;
                    if (id == 0)  
                        query = "INSERT INTO TB_perfiles (Nombre) VALUES (@nombre)";
                    else
                        query = "UPDATE TB_perfiles SET Nombre = @nombre WHERE ID_Usuario = @id";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    if (id != 0) cmd.Parameters.AddWithValue("@id", id);

                    resultado = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // El número de error 2627 o 2601 en SQL Server indica violación de restricción UNIQUE (nombre duplicado)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("El nombre '" + nombre + "' ya está registrado. Intenta con otro.", "Nombre Duplicado");
                }
                else
                {
                    MessageBox.Show("Error de base de datos: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inesperado: " + ex.Message);
            }

            return resultado;
        }

        //Método para leer 
        public static List<Usuarios> LeerPerfiles()
        {
            List<Usuarios> lista = new List<Usuarios>();
            using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
            {
                string query = "SELECT ID_Usuario, Nombre, Fecha_Registro FROM TB_perfiles";
                SqlCommand cmd = new SqlCommand(query, conexion);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Usuarios
                    {
                        id_usuarios = reader.GetInt32(0),
                        nombre = reader.GetString(1),
                        fecha_registro = reader.GetDateTime(2)
                    });
                }
            }
            return lista;
        }

        //Método para eliminar un perfil
        public static int EliminarPerfil(int id)
        {
            int retorna = 0;
            try
            {
                using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
                {
                    string query = "DELETE FROM TB_perfiles WHERE ID_Usuario = @id";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    retorna = cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // El error 547 es el código de SQL para conflicto de Llave Foránea
                if (ex.Number == 547)
                {
                    MessageBox.Show("No se puede eliminar un perfil con un historial de partidas registradas.", "Error al eliminar");
                }
                else
                {
                    MessageBox.Show("Error de base de datos: " + ex.Message);
                }
            }
            return retorna;
        }
        

        //Método para guardar el resultado de una partida
        public static int InsertarPartida(Partida registro)
        {
            int retorna = 0;
            using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
            {
                // 1. Insertamos el registro normal de la partida
                string queryPartida = "INSERT INTO TB_partidas (ID_Ganador, ID_Perdedor) VALUES (@ganador, @perdedor)";
                SqlCommand cmd = new SqlCommand(queryPartida, conexion);
                cmd.Parameters.AddWithValue("@ganador", registro.id_ganador);
                cmd.Parameters.AddWithValue("@perdedor", registro.id_perdedor);
                retorna = cmd.ExecuteNonQuery();

                // 2. Lógica de Rankeds: Actualizar puntos y victorias del GANADOR (+15 puntos)
                string queryGanador = @"
            IF NOT EXISTS (SELECT 1 FROM TB_rankeds WHERE ID_Usuario = @idG)
                INSERT INTO TB_rankeds (ID_Usuario, Puntos_Totales, Victorias, Derrotas) VALUES (@idG, 15, 1, 0)
            ELSE
                UPDATE TB_rankeds SET Puntos_Totales = Puntos_Totales + 15, Victorias = Victorias + 1 WHERE ID_Usuario = @idG";

                SqlCommand cmdG = new SqlCommand(queryGanador, conexion);
                cmdG.Parameters.AddWithValue("@idG", registro.id_ganador);
                cmdG.ExecuteNonQuery();

                // 3. Lógica de Rankeds: Actualizar derrotas del PERDEDOR
                string queryPerdedor = @"
            IF NOT EXISTS (SELECT 1 FROM TB_rankeds WHERE ID_Usuario = @idP)
                INSERT INTO TB_rankeds (ID_Usuario, Puntos_Totales, Victorias, Derrotas) VALUES (@idP, 0, 0, 1)
            ELSE
                UPDATE TB_rankeds SET Derrotas = Derrotas + 1 WHERE ID_Usuario = @idP";

                SqlCommand cmdP = new SqlCommand(queryPerdedor, conexion);
                cmdP.Parameters.AddWithValue("@idP", registro.id_perdedor);
                cmdP.ExecuteNonQuery();
            }
            return retorna;
        }

        //Método para obtener el historial de partidas
        public static DataTable ObtenerHistorialPartidas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
            {
                string query = @"SELECT p.ID_Partida, G.Nombre AS Ganador, L.Nombre AS Perdedor, p.Fecha 
                         FROM TB_partidas p 
                         JOIN TB_perfiles G ON p.ID_Ganador = G.ID_Usuario 
                         JOIN TB_perfiles L ON p.ID_Perdedor = L.ID_Usuario
                         ORDER BY p.ID_Partida DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                da.Fill(dt);
            }
            return dt;
        }

        public static List<Usuarios> Buscar(string usuarioBusqueda)
        {
            List<Usuarios> lista = new List<Usuarios>();

            using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
            {
                string query = @"select * from TB_perfiles where Nombre like @Nombre;";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Nombre", "%" + usuarioBusqueda + "%");

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Usuarios usuario = new Usuarios();
                    usuario.id_usuarios = reader.GetInt32(0);
                    usuario.nombre = reader.GetString(1);
                    usuario.fecha_registro = reader.GetDateTime(2);

                    lista.Add(usuario);
                }
            }
            return lista;
        }

        public static DataTable ObtenerTablaRankeds()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conexion = CadenaDeConeccion.ObtenerConeccion())
            {
                string query = @"
            SELECT 
                P.ID_Partida, 
                U.Nombre AS Jugador, 
                R.Victorias AS Victorias,
                R.Derrotas AS Derrotas, 
                R.Puntos_Totales AS Puntos
            FROM TB_partidas P
            JOIN TB_perfiles U ON P.ID_Ganador = U.ID_Usuario
            JOIN TB_rankeds R ON U.ID_Usuario = R.ID_Usuario
            ORDER BY P.ID_Partida DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                da.Fill(dt);
            }
            return dt;
        }
    }
}
