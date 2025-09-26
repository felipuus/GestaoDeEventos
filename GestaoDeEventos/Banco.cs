using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.IO;

namespace GestaoDeEventos
{
    public static class IniFile
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);

        public static string Ler(string arquivo, string secao, string chave)
        {
            StringBuilder buffer = new StringBuilder(255);
            GetPrivateProfileString(secao, chave, "", buffer, buffer.Capacity, arquivo);
            return buffer.ToString();
        }
    }

    public static class Banco
    {

        private static string arquivoIni = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public static string StringConexao
        {
            get
            {
                string server = IniFile.Ler(arquivoIni, "Banco", "Server");
                string database = IniFile.Ler(arquivoIni, "Banco", "Database");
                string user = IniFile.Ler(arquivoIni, "Banco", "User");
                string password = IniFile.Ler(arquivoIni, "Banco", "Password");

                return $"Server={server};Database={database};User Id={user};Password={password};";
            }
        }

        public static SqlConnection GetConexao()
        {
            return new SqlConnection(StringConexao);
        }

    }





}