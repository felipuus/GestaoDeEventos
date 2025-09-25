using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica interna para Participantes.xaml
    /// </summary>
    public partial class Participantes : Window
    {
        public Participantes()
        {
            InitializeComponent();
            carregarparticipantes();
            sumircbeltparticipantes();
            sumirbtcancelarpart();
            sumirbtexcluirpart();
            desativarbtalterar();


        }


        private void desativarcpfcnpj()
        {
            txtcnpjoucpf.IsEnabled = false;
        }

        private void ativarcpfcnpj()
        {
            txtcnpjoucpf.IsEnabled = true;
        }


        private void sumircbeltparticipantes()
        {
            cbparticipante.Visibility = Visibility.Collapsed;
            lbParticipantes.Visibility = Visibility.Collapsed;
        }

        private void aparecercbeltparticipantes()
        {
            cbparticipante.Visibility = Visibility.Visible;
            lbParticipantes.Visibility = Visibility.Visible;
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

        private void limparcampospart()
        {
            txtcnpjoucpf.Clear();
            txtnomepart.Clear();
            cbparticipante.SelectedIndex = -1;

        }

        private void txtcnpjoucpf_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d$");
        }

        // bloqueia colagem (Ctrl+V) se tiver caracteres não numéricos
        private void txtcnpjoucpf_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = e.DataObject.GetData(DataFormats.Text) as string;
                if (!Regex.IsMatch(text, @"^\d+$"))
                {
                    e.CancelCommand(); // cancela a colagem
                }
            }
            else
            {
                e.CancelCommand();
            }
        }


        private void carregarparticipantes ()
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Seleciona os nomes dos tipos
                    SqlCommand cmdselctforn = new SqlCommand("select CPF_CNPJ, Nome from Participantes", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmdselctforn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Preenche a ComboBox
                    cbparticipante.ItemsSource = dt.DefaultView;
                    cbparticipante.DisplayMemberPath = "Nome"; // O que aparece na tela

                    cbparticipante.SelectedValuePath = "CPF_CNPJ"; // O valor selecionado (pode ser o mesmo)

                    // Preenche



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os Tipos de Evento: " + ex.Message);
            }
        }

        private void cbparticipante_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbparticipante.SelectedItem is DataRowView row)
            {
                txtcnpjoucpf.Text = row["CPF_CNPJ"].ToString();

                txtnomepart.Text = row["NOME"].ToString();

            }
        }

        private void btconsultarpart_Click(object sender, RoutedEventArgs e)
        {
            aparecercbeltparticipantes();
            desativarcpfcnpj();
            exibirbtcancelarpart();
            exibirbtexcluirpart();
            ativarbtalterar();
            carregarparticipantes();




        }

        private void btalterarpart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();




                    // Faz o UPDATE
                    string sql = "UPDATE Participantes SET Nome = @Nome  WHERE CPF_CNPJ = @CPF_CNPJ";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nome", txtnomepart.Text.Trim());

                        

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Participante alterado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum participante foi alterado.");
                        }

                    }
                }

                
                limparcampospart();
                sumircbeltparticipantes();
                sumirbtcancelarpart();
                sumirbtexcluirpart();
                desativarbtalterar();
                ativarcpfcnpj();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar participante: " + ex.Message);
            }
        }

        private void btcancelarpart_Click(object sender, RoutedEventArgs e)
        {
            limparcampospart();
            sumircbeltparticipantes();
            sumirbtcancelarpart();
            sumirbtexcluirpart();
            desativarbtalterar();
            ativarcpfcnpj();
        }

        private void btcriarpart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    // Verifica se já existe fornecedor com esse CPF/CNPJ
                    string checkSql = "SELECT COUNT(*) FROM Participantes WHERE CPF_CNPJ = @CPF_CNPJ";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Já existe um Participante com esse CPF/CNPJ cadastrado.");
                            return;
                        }
                    }

                    // Se não existir, faz o insert
                    string sql = "INSERT INTO Participantes (CPF_CNPJ, Nome) VALUES (@CPF_CNPJ, @Nome)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nome", txtnomepart.Text.Trim());



                        cmd.ExecuteNonQuery();
                    }
                    
                    limparcampospart();
                    sumircbeltparticipantes();
                    sumirbtcancelarpart();
                    sumirbtexcluirpart();
                    desativarbtalterar();
                    ativarcpfcnpj();

                }

                MessageBox.Show("Participante cadastrado com sucesso!");
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar Participante: " + ex.Message);
            }
        }

        private void btexcluirpart_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Faz o DELETE
                    string sql = "DELETE FROM Participantes WHERE CPF_CNPJ = @CPF_CNPJ";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());
                        int linhasAfetadas = cmd.ExecuteNonQuery();
                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Participante excluído com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum participante foi excluído.");
                        }
                    }
                }

                limparcampospart();
                sumircbeltparticipantes();
                sumirbtcancelarpart();
                sumirbtexcluirpart();
                desativarbtalterar();
                ativarcpfcnpj();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir participante: " + ex.Message);
            }
        }
        
    }
}
