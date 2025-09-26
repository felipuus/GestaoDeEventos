using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GestaoDeEventos
{
    /// <summary>
    /// Lógica interna para Fornecedores.xaml
    /// </summary>
    public partial class Fornecedores : Window
    {
        public Fornecedores()
        {
            InitializeComponent();
            sumirbtcancelarforn();
            sumirbtexcluirforn();
            sumirbtcancelarforn();
            
            sumircbeltfornecedor();
            desativarbtalterar();
            
        }

        private void sumirbtcriarforn()
        {
            btcriarforn.IsEnabled = false;
        }

        private void exibirbtcriarforn()
        {
            btcriarforn.IsEnabled = true;
        }

        private void desativarbtalterar()
        {
            btalterarforn.IsEnabled = false;
        }

        private void ativarbtalterar()
        {
            btalterarforn.IsEnabled = true;
        }

        private void desativarcpfcnpj()
        {
            txtcnpjoucpf.IsEnabled = false;
        }

        private void ativarcpfcnpj()
        {
            txtcnpjoucpf.IsEnabled = true;
        }


        private void sumircbeltfornecedor()
        {
            cbfornecedor.Visibility = Visibility.Collapsed;
            lbfornecedor.Visibility = Visibility.Collapsed;
        }
        
        private void exibirltfornecedores()
        {
            lbfornecedor.Visibility = Visibility.Visible;
            cbfornecedor.Visibility = Visibility.Visible;
        }

        private void sumirbtcancelarforn()
        {
            btcancelarforn.Visibility = Visibility.Collapsed;
        }

        private void exibirbtcancelarforn()
        {
            btcancelarforn.Visibility = Visibility.Visible;
        }

        private void sumirbtexcluirforn()
        {
            btexcluirforn.Visibility = Visibility.Collapsed;
        }

        private void exibirbtexcluirforn()
        {
            btexcluirforn.Visibility = Visibility.Visible;
        }

        private void limpartudo()
        {
            txtnomeforn.Clear();
            txtcnpjoucpf.Clear();
            precoforn.Clear();
            precoforn.Clear();
            cbfornecedor.SelectedIndex = -1;
        }

        // só aceita dígitos ao digitar
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

        private void percoforn_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Remove o evento para evitar loop
            txt.TextChanged -= percoforn_TextChanged;

            // Remove tudo que não é número
            string onlyNumbers = Regex.Replace(txt.Text, @"[^\d]", "");

            if (string.IsNullOrEmpty(onlyNumbers))
                onlyNumbers = "0";

            // Converte para decimal considerando centavos
            decimal value = decimal.Parse(onlyNumbers) / 100;

            // Formata apenas com separador de milhares e vírgula, sem R$
            txt.Text = value.ToString("N2", CultureInfo.GetCultureInfo("pt-BR"));

            // Mantém o cursor no final
            txt.CaretIndex = txt.Text.Length;

            // Reativa o evento
            txt.TextChanged += percoforn_TextChanged;
        }

        

        private void btconsultarforn_Click(object sender, RoutedEventArgs e)
        {
            CarregarFornecedores();
            exibirltfornecedores();
            ativarbtalterar();
            exibirbtcancelarforn();
            sumirbtcriarforn();
            limpartudo();
            desativarcpfcnpj();
            exibirbtexcluirforn();






        }

        private void CarregarFornecedores()
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Seleciona os nomes dos tipos
                    SqlCommand cmdselctforn = new SqlCommand("select CPF_CNPJ, Nome, Preco from Fornecedores", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmdselctforn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Preenche a ComboBox
                    cbfornecedor.ItemsSource = dt.DefaultView;
                    cbfornecedor.DisplayMemberPath = "Nome"; // O que aparece na tela
                    
                    cbfornecedor.SelectedValuePath = "CPF_CNPJ"; // O valor selecionado (pode ser o mesmo)

                    // Preenche



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os Tipos de Evento: " + ex.Message);
            }
        }

        private void cbfornecedor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbfornecedor.SelectedItem is DataRowView row)
            {
                txtcnpjoucpf.Text = row["CPF_CNPJ"].ToString();
                precoforn.Text = row["PRECO"].ToString();
                txtnomeforn.Text = row["NOME"].ToString();
                
            }
        }

        private void btcancelarforn_Click(object sender, RoutedEventArgs e)
        {
            sumircbeltfornecedor();
            limpartudo();
            sumirbtcancelarforn();
            exibirbtcriarforn();
            desativarbtalterar();
            ativarcpfcnpj();
            sumirbtexcluirforn();

        }

        private void btcriarforn_Click(object sender, RoutedEventArgs e)
        {
            // Validação básica dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtcnpjoucpf.Text))
            {
                MessageBox.Show("Informe o CPF/CNPJ do Fornecedor.");
                txtcnpjoucpf.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtnomeforn.Text))
            {
                MessageBox.Show("Informe o nome do Fornecedor.");
                txtnomeforn.Focus();
                return;
            }


            string cpfCnpj = txtcnpjoucpf.Text.Trim();
           
            // Valida se o CPF/CNPJ é numérico
            if (!cpfCnpj.All(char.IsDigit))
            {
                MessageBox.Show("O campo CPF/CNPJ deve conter apenas números.");
                return;
            }



            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    // Verifica se já existe fornecedor com esse CPF/CNPJ
                    string checkSql = "SELECT COUNT(*) FROM Fornecedores WHERE CPF_CNPJ = @CPF_CNPJ";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());

                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Já existe um fornecedor com esse CPF/CNPJ cadastrado.");
                            return;
                        }
                    }

                    // Se não existir, faz o insert
                    string sql = "INSERT INTO Fornecedores (CPF_CNPJ, Nome, Preco) VALUES (@CPF_CNPJ, @Nome, @Preco)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nome", txtnomeforn.Text.Trim());

                        if (decimal.TryParse(precoforn.Text, out decimal preco))
                        {
                            cmd.Parameters.AddWithValue("@Preco", preco);
                        }
                        else
                        {
                            MessageBox.Show("Digite um valor válido para o preço.");
                            return;
                        }

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Fornecedor cadastrado com sucesso!");
                limpartudo();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar fornecedor: " + ex.Message);
            }
        }

        



        private void btalterarforn_Click(object sender, RoutedEventArgs e)
        {
            // Validação básica dos campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtcnpjoucpf.Text))
            {
                MessageBox.Show("Informe o CPF/CNPJ do Fornecedor.");
                txtcnpjoucpf.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtnomeforn.Text))
            {
                MessageBox.Show("Informe o nome do Fornecedor.");
                txtnomeforn.Focus();
                return;
            }

            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    


                    // Faz o UPDATE
                    string sql = "UPDATE Fornecedores SET Nome = @Nome, Preco = @Preco WHERE CPF_CNPJ = @CPF_CNPJ";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nome", txtnomeforn.Text.Trim());

                        if (decimal.TryParse(precoforn.Text, out decimal preco))
                        {
                            cmd.Parameters.AddWithValue("@Preco", preco);
                        }
                        else
                        {
                            MessageBox.Show("Digite um valor válido para o preço.");
                            return;
                        }

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Fornecedor alterado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum fornecedor foi alterado.");
                        }
                    }
                }

                // Atualiza a lista e limpa os campos
                CarregarFornecedores();
                limpartudo();
                ativarcpfcnpj();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar fornecedor: " + ex.Message);
            }
        }

        private void btexcluirforn_Click(object sender, RoutedEventArgs e)
        {
            sumirbtexcluirforn();
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Faz o DELETE
                    string sql = "DELETE FROM Fornecedores WHERE CPF_CNPJ = @CPF_CNPJ";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@CPF_CNPJ", txtcnpjoucpf.Text.Trim());
                        int linhasAfetadas = cmd.ExecuteNonQuery();
                        if (linhasAfetadas > 0)
                        {
                            MessageBox.Show("Fornecedor excluído com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhum fornecedor foi excluído.");
                        }
                    }
                }
                // Atualiza a lista e limpa os campos
                CarregarFornecedores();
                limpartudo();
                ativarcpfcnpj();
                exibirbtcriarforn();
                sumirbtexcluirforn();
                sumirbtcancelarforn();
                desativarbtalterar();
                sumircbeltfornecedor();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir fornecedor: " + ex.Message);
            }

        }
    }

}
