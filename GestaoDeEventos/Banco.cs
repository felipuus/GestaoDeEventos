using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace GestaoDeEventos
{


    /// <summary>
    ///  conexão com banco de dados
    ///  
    /// pode ser feito pela autencicação com windows: public static string StringConexao = @"Server=FELIPE\SQLEXPRESS;Database=GestaoDeEventos;Integrated Security=True";
    ///  
    /// </summary>


    public static class Banco


        {
            public static string StringConexao = @"Server=FELIPE\SQLEXPRESS;Database=GestaoDeEventos;User Id=sa;Password=inter#system;";

            public static SqlConnection GetConexao()
            {
                return new SqlConnection(StringConexao);
            }
        }

    



}
