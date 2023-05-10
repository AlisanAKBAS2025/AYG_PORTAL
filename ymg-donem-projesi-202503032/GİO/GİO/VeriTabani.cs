using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace GİO

{
    class VeriTabani
    {
        VeriTabani()
        {

        }

        static SqlConnection con;
        //static SqlCommand cmd;
        static SqlDataAdapter da;
        static DataSet ds;

        public static string SqlCon = "DESKTOP-3BUBNE9\\SQLEXPRESS; Initial Catalog = GİO; Integrated Security = TRUE";

        public static bool BaglantiDurum()
        {
            using (con = new SqlConnection(SqlCon))
            {
                try
                {
                    con.Open();
                    return true;

                }
                catch (SqlException exp)
                {
                    MessageBox.Show(exp.Message);
                    return false;
                }
            }
        }



        public static DataGridView GridDoldur(DataGridView datagridim, string sqlSeecetSorgu)
        {
            con = new SqlConnection(SqlCon);
            da = new SqlDataAdapter(sqlSeecetSorgu, con);
            ds = new DataSet();

            con.Open();

            da.Fill(ds, sqlSeecetSorgu);
            datagridim.DataSource = ds.Tables[sqlSeecetSorgu];

            con.Close();
            return datagridim;
        }

        public static string MD5Sifrele(string metin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] dizi = Encoding.UTF8.GetBytes(metin);

            StringBuilder sb = new StringBuilder();

            dizi = md5.ComputeHash(dizi);

            foreach (byte item in dizi)

                sb.Append(item.ToString("x2").ToLower());



            return sb.ToString();
        }




    }
}
