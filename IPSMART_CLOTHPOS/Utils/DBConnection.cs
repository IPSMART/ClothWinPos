using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Drawing.Printing;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
//using System.Data.SqlClient;

/// <summary>
/// Summary description for DBConnection
/// This class is Use for Database connection 
/// </summary>

namespace IPSMART_CLOTHPOS
{
    public class DBConnection
    {
        public OracleDataAdapter da = new OracleDataAdapter();
        //public DataSet ds = new DataSet();
        //public OracleCommandBuilder cb = new OracleCommandBuilder();
        //public OracleDataReader dr;
        //private static string Dftl_Schema = null;
        //private static string Dftl_Machin = null;
        //private static string AutoLOgMsg = null;
        // Create byte array for additional entropy when using Protect method.
        static byte[] encryptionkey = { 9, 0 };

        //public string Getschema
        //{
        //    get { return Dftl_Schema; }
        //    set { Dftl_Schema = value; }
        //}
        //public int Get_AutoTimeout
        //{
        //    get { return AutoTimeout; }
        //    set { AutoTimeout = value; }
        //}
        public string GetConnectionString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["local"].ToString();

            string[] cc = connectionString.Split(';');
            string user = cc[1].ToString().Substring(8);
            string pass = cc[2].ToString().Substring(9);
            return cc[0].ToString() + ";User ID=" + Decrypt(user) + ";Password=" + Decrypt(pass) + "";
        }

        string CS = "";
        public DataTable SQLquery(string SQL)
        {
            CS = GetConnectionString();
            DataTable dt = new DataTable();
            try
            {
                OracleCommand com = new OracleCommand();
                using (OracleConnection con = new OracleConnection(CS))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    string SQLQuery = SQL;
                    com = new OracleCommand(SQLQuery, con);
                    da.SelectCommand = com;
                    da.Fill(dt);
                    com.Dispose();
                    con.Close();
                    con.Dispose();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                //SaveException(ex, SQL);
                return null;
            }
        }
        public string SQLNonQuery(string SQL)
        {
            try
            {
                OracleCommand com = new OracleCommand();
                string CS = GetConnectionString();
                using (OracleConnection con = new OracleConnection(CS))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    com = new OracleCommand(SQL);
                    com.ExecuteNonQuery();
                    com.Dispose();
                    con.Close();
                    con.Dispose();
                    return "";

                }
            }
            catch (Exception ex)
            {
                //Cn.SaveException(ex, SQL);
                return ex.ToString();
            }
        }
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "775466@@##@@!!jaguar86866454";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string IsDBConnected()
        {
            using (var l_oConnection = new OracleConnection(GetConnectionString()))
            {
                try
                {
                    l_oConnection.Open();
                    return "ok";
                }
                catch (OracleException ex)
                {
                    return "Database connection failed " + "\n\n Connection String:{ " + GetConnectionString() + " }\n\nException:" + ex.Message;
                }
            }
        }
    }
}