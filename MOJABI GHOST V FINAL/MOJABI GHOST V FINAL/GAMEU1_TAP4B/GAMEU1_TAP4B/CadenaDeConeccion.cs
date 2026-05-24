using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GAMEU1_TAP4B
{
    internal class CadenaDeConeccion
    {
        public static SqlConnection ObtenerConeccion()
        {
            string cadena = "Integrated Security=SSPI; Persist Security Info=False; Initial Catalog=DB_mojabi_fish; Data Source=JPGAZAEL\\SQLEXPRESS; TrustServerCertificate=True";
            SqlConnection cone = new SqlConnection(cadena);

            try
            {
                cone.Open();
                //MessageBox.Show("conection exitosa");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

            return cone;


        }
    }
}
