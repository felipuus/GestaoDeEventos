using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestaoDeEventos
{
    /// <summary>
    /// Lógica interna para TipoDeEvento.xaml
    /// </summary>
    public partial class TipoDeEvento : Window
    {
        public TipoDeEvento()
        {
            InitializeComponent();
            carregarTipoDeEvento();
            sumircbeltparticipantes();
            sumirbtcancelarpart();
            sumirbtexcluirpart();
            desativarbtalterar();
        }

        private void sumircbeltparticipantes()
        {
            cbtipodeevento.Visibility = Visibility.Collapsed;
            lbTipodeEvento.Visibility = Visibility.Collapsed;
        }

        private void aparecercbeltparticipantes()
        {
            cbtipodeevento.Visibility = Visibility.Visible;
            lbTipodeEvento.Visibility = Visibility.Visible;
        }

        private void sumirbtcancelarpart()
        {
            btcancelarpart.Visibility = Visibility.Collapsed;
        }

        private void exibirbtcancelarpart()
        {
            btcancelarpart.Visibility = Visibility.Visible;
        }

        private void sumirbtexcluirpart()
        {
            btexcluirpart.Visibility = Visibility.Collapsed;
        }

        private void exibirbtexcluirpart()
        {
            btexcluirpart.Visibility = Visibility.Visible;
        }

        private void desativarbtalterar()
        {
            btalterarpart.IsEnabled = false;
        }

        private void ativarbtalterar()
        {
            btalterarpart.IsEnabled = true;
        }

        private void desativarbtcriar()
        {
            btcriarpart.IsEnabled = false;
        }

        private void ativarbtcriar()
        {
            btcriarpart.IsEnabled = true;
        }

        private void limparcampospart()
        {
            txtnometipoevento.Clear();
            txtcodigotipoevento.Clear();
            cbtipodeevento.SelectedIndex = -1;

        }


        private void carregarTipoDeEvento()
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Seleciona os nomes dos tipos
                    SqlCommand cmdselctforn = new SqlCommand("select Cod_Tipo, Nome_Tipo from TipoEvento", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmdselctforn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Preenche a ComboBox
                    cbtipodeevento.ItemsSource = dt.DefaultView;
                    cbtipodeevento.DisplayMemberPath = "Nome_Tipo"; // O que aparece na tela

                    cbtipodeevento.SelectedValuePath = "Cod_Tipo"; // O valor selecionado (pode ser o mesmo)

                    



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os Tipos de Evento: " + ex.Message);
            }
        }

        private void cbparticipante_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbtipodeevento.SelectedItem is DataRowView row)
            {
                txtnometipoevento.Text = row["Nome_Tipo"].ToString();

                txtcodigotipoevento.Text = row["Cod_Tipo"].ToString();

            }
        }

        private void btconsultarpart_Click(object sender, RoutedEventArgs e)
        {
            carregarTipoDeEvento();
            aparecercbeltparticipantes();
            exibirbtcancelarpart();
            exibirbtexcluirpart();
            ativarbtalterar();
            limparcampospart();
            desativarbtcriar();



        }

        private void btalterarpart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtnometipoevento.Text))
            {
                MessageBox.Show("Informe o nome do tipo de evento antes de salvar!");
                return;
            }

            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();




                    // Faz o UPDATE
                    string sql = "UPDATE TipoEvento SET Nome_Tipo = @Nome_Tipo  WHERE Cod_Tipo = @Cod_Tipo";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Cod_Tipo", txtcodigotipoevento.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nome_Tipo", txtnometipoevento.Text.Trim());



                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Tipo de evento alterado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum tipo de evento foi alterado.");
                        }

                    }
                }


                limparcampospart();
                sumircbeltparticipantes();
                sumirbtcancelarpart();
                sumirbtexcluirpart();
                desativarbtalterar();
                ativarbtcriar();



            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar participante: " + ex.Message);
            }

        }

        private void btcriarpart_Click(object sender, RoutedEventArgs e)
        {


            try
            {
                if (string.IsNullOrWhiteSpace(txtnometipoevento.Text))
                {
                    MessageBox.Show("Informe o nome do tipo de evento antes de salvar!");
                    return;
                }
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                   
                    string sql = "INSERT INTO TipoEvento (Nome_Tipo) VALUES (@Nome_Tipo)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        
                        cmd.Parameters.AddWithValue("@Nome_Tipo", txtnometipoevento.Text.Trim());



                        cmd.ExecuteNonQuery();
                    }

                    limparcampospart();
                    sumircbeltparticipantes();
                    sumirbtcancelarpart();
                    sumirbtexcluirpart();
                    desativarbtalterar();
                    

                }

                MessageBox.Show("Tipo de evento cadastrado com sucesso!");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar tipo de evento: " + ex.Message);
            }
        }

        private void btcancelarpart_Click(object sender, RoutedEventArgs e)
        {
            limparcampospart();
            sumircbeltparticipantes();
            sumirbtcancelarpart();
            sumirbtexcluirpart();
            desativarbtalterar();
            ativarbtcriar();

        }

        private void btexcluirpart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Faz o DELETE
                    string sql = "DELETE FROM TipoEvento WHERE Cod_Tipo = @Cod_Tipo";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Cod_Tipo", txtcodigotipoevento.Text.Trim());
                        int linhasAfetadas = cmd.ExecuteNonQuery();
                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Tipo de Evento excluído com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum Evento foi excluído.");
                        }
                    }
                }

                limparcampospart();
                sumircbeltparticipantes();
                sumirbtcancelarpart();
                sumirbtexcluirpart();
                desativarbtalterar();
                ativarbtcriar();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir participante: " + ex.Message);
            }
        }
    }
}
