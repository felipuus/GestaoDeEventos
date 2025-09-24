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
    /// Lógica interna para TelaEventos.xaml
    /// </summary>
    public partial class TelaEventos : Window
    {
        public TelaEventos()
        {
            InitializeComponent();
            CarregarFornecedores();
            CarregarParticipantes();
            CarregarTipoEvento();
            desativarbtalterar();
            sumirbtcancelar();
            this.Deactivated += TelaEventos_Deactivated;
            this.LocationChanged += TelaEventos_LocationChanged;
            this.PreviewMouseDown += TelaEventos_PreviewMouseDown;
            sumirbtexcluir();
        }

        private bool podeAbrirEventos = false;


        private void txtnomedoevento_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (podeAbrirEventos) 
            {
                popupEventos.IsOpen = true;
            }
            else
            {
                popupEventos.IsOpen = false;
            }
        }



        private void TelaEventos_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Fecha o popup de fornecedores se estiver aberto e o clique for fora dele
            if (popupFornecedores.IsOpen && !popupFornecedores.IsMouseOver)
            {
                popupFornecedores.IsOpen = false;
            }

            // Fecha o popup de participantes se estiver aberto e o clique for fora dele
            if (popupParticipantes.IsOpen && !popupParticipantes.IsMouseOver)
            {
                popupParticipantes.IsOpen = false;
            }

            // Fecha o popup de eventos se estiver aberto e o clique for fora dele
            if (popupEventos.IsOpen && !popupEventos.IsMouseOver)
            {
                popupEventos.IsOpen = false;
            }

        }




        private void TelaEventos_Deactivated(object sender, EventArgs e)
        {
            FecharPopups();
        }

        private void TelaEventos_LocationChanged(object sender, EventArgs e)
        {
            FecharPopups();
        }

        private void FecharPopups()
        {
            popupFornecedores.IsOpen = false;
            popupParticipantes.IsOpen = false;
            popupEventos.IsOpen = false;
        }

        private void apagartudo()
        {
            // limpar tudo

            txtnomedoevento.Text = "";
            TxtEnderecoEvent1.Text = "";
            TxtEnderecoEventNumero.Text = "";
            TxtInforCepEventos.Text = "";
            lotacaomaximatleven.Text = "";
            orcamentotleventos.Text = "0";
            txtobs.Text = "";
            valortotal.Text = "";
            valortotalpartic.Text = "";
            txtFornecedores.Text = "Selecione fornecedores...";
            txtParticipantes.Text = "Selecione participantes...";
            txtcodigo.Text = "";

            // Limpar ComboBox
            cbtipoeventotleventos.SelectedIndex = -1;

            // Limpar DatePicker
            dpdatadoevent.SelectedDate = null;
            dpdatadoevent_ate.SelectedDate = null;

            // Limpar ListBoxes selecionados
            lbFornecedores.UnselectAll();
            lbParticipantes.UnselectAll();
        }

        private void desativarbtcriar()
        {
            btcriareventos.IsEnabled = false;
        }

        private void sumirbtexcluir() 
        {
            btexcluir.Visibility = Visibility.Collapsed;
        }

        private void exibirbtexcluir()
        {
            btexcluir.Visibility = Visibility.Visible;
        }

        private void ativarbtcriar()
        {
            btcriareventos.IsEnabled = true;
        }

        private void desativarbtalterar()
        {
            btalterareventos.IsEnabled =false;
        }

        private void ativarbtalterar() 
        {
            btalterareventos.IsEnabled = true;
        }

        private void sumirbtcancelar()
        {

            btcancelar.Visibility = Visibility.Collapsed;

        }

        private void exibirbtcancelar()
        {

            btcancelar.Visibility = Visibility.Visible;

        }

       


        private void txtnomedoevento_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            popupEventos.IsOpen = false;
        }

        private void txtFornecedores_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            popupFornecedores.IsOpen = true;
        }
        // Permite digitar apenas números
        private void orcamentotleventos_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"\d"); // só números
        }

        // Formata como preço sem R$
        private void orcamentotleventos_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) return;

            // Remove o evento para evitar loop
            txt.TextChanged -= orcamentotleventos_TextChanged;

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
            txt.TextChanged += orcamentotleventos_TextChanged;
        }
        
        // text lotaçao maxima permitir apenas numero 
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"\d");
        }

        private void lotacaomaximatleven_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = e.DataObject.GetData(DataFormats.Text) as string;
                if (!Regex.IsMatch(text, @"^\d+$")) // só números
                {
                    e.CancelCommand(); // cancela a colagem
                }
            }
            else
            {
                e.CancelCommand();
            }
        }



        private void BtnFecharPopup_Click(object sender, RoutedEventArgs e)
        {
            popupFornecedores.IsOpen = false;

            // Pega os fornecedores selecionados
            var selecionados = lbFornecedores.SelectedItems
                .Cast<DataRowView>()
                .Select(r => new
                {
                    Nome = r["Nome"].ToString(),
                    Preco = Convert.ToDecimal(r["Preco"])
                })
                .ToList();

            // Mostra o resumo na TextBox
            txtFornecedores.Text = string.Join(", ", selecionados.Select(f => $"{f.Nome} - {f.Preco:N2}"));

            // Atualiza o total em outra TextBox (exemplo: txtTotal)
            decimal total = selecionados.Sum(f => f.Preco);
            valortotal.Text = total.ToString("N2");
        }


        // Abre o popup ao clicar na TextBox
        private void txtParticipantes_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            popupParticipantes.IsOpen = true;
        }

        // Fecha o popup ao clicar OK e atualiza o texto da TextBox
        private void BtnFecharPopupParticipantes_Click(object sender, RoutedEventArgs e)
        {
            popupParticipantes.IsOpen = false;

            // Pega os participantes selecionados
            var selecionados = lbParticipantes.SelectedItems
                .Cast<DataRowView>()
                .Select(r => r["Nome"].ToString())
                .ToList();



            // Mostra o resumo na TextBox
            txtParticipantes.Text = string.Join(", ", selecionados);
            // Exibe o total de participantes
            valortotalpartic.Text = selecionados.Count.ToString();

        }

        private void CarregarFornecedores()
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Seleciona os nomes dos fornecedores
                    SqlCommand cmdselctforn = new SqlCommand("SELECT Nome,CPF_CNPJ, PRECO FROM Fornecedores", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmdselctforn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Preenche a ComboBox
                    lbFornecedores.ItemsSource = dt.DefaultView;
                    lbFornecedores.SelectedValuePath = "CPF_CNPJ"; // O valor selecionado (pode ser o mesmo)
                    lbFornecedores.SelectedValuePath = "PRECO"; // O valor selecionado (pode ser o mesmo)


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar fornecedores: " + ex.Message);
            }
        }

        private void CarregarParticipantes()
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    // Seleciona os nomes dos participantes
                    SqlCommand cmdselctforn = new SqlCommand("SELECT Nome,CPF_CNPJ FROM Participantes", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmdselctforn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Preenche a ComboBox
                    lbParticipantes.ItemsSource = dt.DefaultView;
                    
                    lbParticipantes.SelectedValuePath = "CPF_CNPJ"; // O valor selecionado (pode ser o mesmo)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar Participantes: " + ex.Message);
            }
        }

        private void CarregarTipoEvento()
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
                    cbtipoeventotleventos.ItemsSource = dt.DefaultView;
                    cbtipoeventotleventos.DisplayMemberPath = "Nome_Tipo"; // O que aparece na tela
                    cbtipoeventotleventos.SelectedValuePath = "Cod_Tipo"; // O valor selecionado (pode ser o mesmo)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os Tipos de Evento: " + ex.Message);
            }
        }

        private void TxtInforCepEventos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Se apertar Enter
            {
                BtBuscarCepTlEv_Click(BtBuscarCepTlEv, null); // chama o mesmo método do botão
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                // Exemplo: só mostra no título da janela o texto digitado
                this.Title = txt.Text;
            }
        }

        private async void BtBuscarCepTlEv_Click(object sender, RoutedEventArgs e)
        {
            string cep = TxtInforCepEventos.Text.Trim();
            if (string.IsNullOrWhiteSpace(cep))
            {
                MessageBox.Show("Digite um CEP válido.");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"https://viacep.com.br/ws/{cep}/json/";
                    string response = await client.GetStringAsync(url);
                    JObject obj = JObject.Parse(response);

                    if (obj["erro"] != null)
                    {
                        TxtEnderecoEvent1.Text = "CEP não encontrado!";
                    }
                    else
                    {
                        string logradouro = (string)obj["logradouro"];
                        string bairro = (string)obj["bairro"];
                        string localidade = (string)obj["localidade"];
                        string uf = (string)obj["uf"];
                        TxtEnderecoEvent1.Text = $"{logradouro}, {bairro} - {localidade}/{uf}";
                    }
                }
            }
            catch
            {
                TxtEnderecoEvent1.Text = "Erro ao buscar o CEP.";
            }
        }


        private void btcriareventos_Click(object sender, RoutedEventArgs e)
        {

            DateTime novoInicio = dpdatadoevent.SelectedDate.Value;
            DateTime novoFim = dpdatadoevent_ate.SelectedDate.Value;

            if (decimal.TryParse(valortotal.Text, out decimal totalvlcr) &&
            decimal.TryParse(orcamentotleventos.Text, out decimal orcamentovlcr))
            {
                if (totalvlcr > orcamentovlcr)
                {
                    MessageBox.Show("O valor total dos fornecedores ultrapassa o orçamento máximo permitido!");
                    return; 
                }
            }

            // Validação de lotação antes de abrir a conexão
            if (int.TryParse(valortotalpartic.Text, out int totalParticipantesvld) &&
                int.TryParse(lotacaomaximatleven.Text, out int lotacaoMaximavld))
            {
                if (totalParticipantesvld > lotacaoMaximavld)
                {
                    MessageBox.Show("O total de participantes selecionados ultrapassa a lotação máxima do evento!");
                    return; // sai do método, não cria o evento
                }
            }


            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    // Inicia transação
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        string sqleventos = @"
                    INSERT INTO Eventos
                    (Nome_Evento, Cod_Tipo, Local, Numero_Local, CEP_Local,
                     Lotacao_Maxima, Orcamento_Maximo, Total_Participantes,
                     Valor_Total, Observacao, Data_Evento, Data_Fim_Evento)
                    VALUES
                    (@Nome_Evento, @Cod_Tipo, @Local, @Numero_Local, @CEP_Local,
                     @Lotacao_Maxima, @Orcamento_Maximo, @Total_Participantes,
                     @Valor_Total, @Observacao, @Data_Evento, @Data_Fim_Evento);
                    SELECT SCOPE_IDENTITY();";

                        int novoIdEvento;

                        // ==== INSERT EVENTO ====
                        using (SqlCommand cmd = new SqlCommand(sqleventos, con, transaction))
                        {
                            // Nome do evento
                            cmd.Parameters.AddWithValue("@Nome_Evento", txtnomedoevento.Text);

                            // Tipo de evento
                            if (cbtipoeventotleventos.SelectedValue == null)
                            {
                                MessageBox.Show("Selecione um tipo de evento.");
                                return;
                            }
                            cmd.Parameters.AddWithValue("@Cod_Tipo", cbtipoeventotleventos.SelectedValue);

                            // Local
                            cmd.Parameters.AddWithValue("@Local", TxtEnderecoEvent1.Text);
                            cmd.Parameters.AddWithValue("@Numero_Local", TxtEnderecoEventNumero.Text);
                            cmd.Parameters.AddWithValue("@CEP_Local", TxtInforCepEventos.Text);

                            // Lotação
                            if (!int.TryParse(lotacaomaximatleven.Text, out int lotacao))
                                lotacao = 0;
                            cmd.Parameters.AddWithValue("@Lotacao_Maxima", lotacao);

                            // Orçamento
                            if (!decimal.TryParse(orcamentotleventos.Text, out decimal orcamento))
                                orcamento = 0;
                            cmd.Parameters.AddWithValue("@Orcamento_Maximo", orcamento);

                            // Total participantes
                            if (!int.TryParse(valortotalpartic.Text, out int totalParticipantes))
                                totalParticipantes = 0;
                            cmd.Parameters.AddWithValue("@Total_Participantes", totalParticipantes);

                            // Valor total fornecedores
                            if (!decimal.TryParse(valortotal.Text, out decimal valorTotal))
                                valorTotal = 0;
                            cmd.Parameters.AddWithValue("@Valor_Total", valorTotal);

                            // Data do Evento

                            if (dpdatadoevent.SelectedDate.HasValue && dpdatadoevent_ate.SelectedDate.HasValue)
                            {
                                DateTime dataInicio = dpdatadoevent.SelectedDate.Value;
                                DateTime dataFim = dpdatadoevent_ate.SelectedDate.Value;

                                if (dataFim < dataInicio)
                                {
                                    MessageBox.Show("A data de término não pode ser menor que a data de início do evento.");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Selecione as duas datas (início e fim) do evento.");
                                return;
                            }

                            DateTime dataEvento;
                            if (dpdatadoevent.SelectedDate.HasValue)
                            {
                                dataEvento = dpdatadoevent.SelectedDate.Value;
                            }
                            else
                            {
                                MessageBox.Show("Selecione uma data válida para o evento.");
                                return;
                            }
                            cmd.Parameters.AddWithValue("@Data_Evento", dataEvento);

                            // Data Fim do Evento
                            DateTime dataEventoFim;
                            if (dpdatadoevent_ate.SelectedDate.HasValue)
                            {
                                dataEventoFim = dpdatadoevent_ate.SelectedDate.Value;
                            }
                            else
                            {
                                MessageBox.Show("Selecione uma data válida para o fim do evento.");
                                return;
                            }
                            cmd.Parameters.AddWithValue("@Data_Fim_Evento", dataEventoFim);

                            // Observação
                            cmd.Parameters.AddWithValue("@Observacao", txtobs.Text);

                            // Executa insert de Evento e pega o ID
                            novoIdEvento = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (var item in lbParticipantes.SelectedItems.Cast<DataRowView>())
                        {
                            string codParticipante = item["CPF_CNPJ"].ToString();

                            // Checa se o participante já está em outro evento nesse período
                            string sqlCheckConflito = @"
        SELECT COUNT(*) 
        FROM parteventos p
        INNER JOIN Eventos e ON p.cod_event = e.Cod_Evento
        WHERE p.cod_part = @cod_part
          AND e.Data_Evento <= @novoFim
          AND e.Data_Fim_Evento >= @novoInicio";

                            using (SqlCommand cmdCheck = new SqlCommand(sqlCheckConflito, con, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@cod_part", codParticipante);
                                cmdCheck.Parameters.AddWithValue("@novoInicio", novoInicio);
                                cmdCheck.Parameters.AddWithValue("@novoFim", novoFim);

                                int conflito = (int)cmdCheck.ExecuteScalar();
                                if (conflito > 0)
                                {
                                    MessageBox.Show($"O participante {item["Nome"]} já está cadastrado em outro evento nesse período!");
                                    transaction.Rollback(); // cancela tudo
                                    return; // sai do método
                                }
                            }
                        }


                        // ==== PARTICIPANTES ====
                        string sqlParticipantes = @"
                    INSERT INTO parteventos (cod_event, cod_part, DataCadastro, Data_Evento, Data_Fim_Evento) 
                    VALUES (@cod_event, @cod_partev, @DataCadastro, @Data_Evento, @Data_Fim_Evento)";

                        foreach (var item in lbParticipantes.SelectedItems.Cast<DataRowView>())
                        {
                            using (SqlCommand cmdPart = new SqlCommand(sqlParticipantes, con, transaction))
                            {
                                cmdPart.Parameters.AddWithValue("@cod_event", novoIdEvento);
                                cmdPart.Parameters.AddWithValue("@cod_partev", item["CPF_CNPJ"]);

                                if (dpdatadoevent.SelectedDate.HasValue)
                                    cmdPart.Parameters.AddWithValue("@Data_Evento", dpdatadoevent.SelectedDate.Value);
                                else
                                    cmdPart.Parameters.AddWithValue("@Data_Evento", DBNull.Value);

                                if (dpdatadoevent_ate.SelectedDate.HasValue)
                                    cmdPart.Parameters.AddWithValue("@Data_Fim_Evento", dpdatadoevent_ate.SelectedDate.Value);
                                else
                                    cmdPart.Parameters.AddWithValue("@Data_Fim_Evento", DBNull.Value);

                                cmdPart.Parameters.AddWithValue("@DataCadastro", DateTime.Now);

                                cmdPart.ExecuteNonQuery();
                            }
                        }

                        // ==== FORNECEDORES ====
                        string sqlforne = @"
                    INSERT INTO foreventos (cod_event, cod_forn, Preco_forn, DataCadastro) 
                    VALUES (@cod_event, @cod_forn, @Preco_forn, @DataCadastro)";

                        foreach (var item in lbFornecedores.SelectedItems.Cast<DataRowView>())
                        {
                            using (SqlCommand cmdFor = new SqlCommand(sqlforne, con, transaction))
                            {
                                cmdFor.Parameters.AddWithValue("@cod_event", novoIdEvento);
                                cmdFor.Parameters.AddWithValue("@cod_forn", item["cpf_cnpj"]);

                                if (!decimal.TryParse(item["Preco"].ToString(), out decimal preco))
                                    preco = 0;
                                cmdFor.Parameters.AddWithValue("@Preco_forn", preco);

                                cmdFor.Parameters.AddWithValue("@DataCadastro", DateTime.Now);

                                cmdFor.ExecuteNonQuery();
                            }
                        }

                        // ==== FINALIZA ====
                        transaction.Commit();
                        MessageBox.Show("Evento cadastrado com sucesso!");
                        apagartudo();
                    }
                    catch (Exception ex)
                    {
                        // Se der erro, desfaz tudo
                        transaction.Rollback();
                        MessageBox.Show("Erro ao criar evento: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message);
            }
        }


        private void btconsultareventos_Click(object sender, RoutedEventArgs e)
        {
            apagartudo();
           
            exibirbtcancelar();
            desativarbtcriar();



            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    string sql = "SELECT Cod_Evento, Nome_Evento FROM Eventos ORDER BY Cod_Evento";
                    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    lbEventos.ItemsSource = dt.DefaultView;
                    lbEventos.DisplayMemberPath = "Nome_Evento"; // Mostra o nome
                    lbEventos.SelectedValuePath = "Cod_Evento";   // Valor que vamos usar

                    popupEventos.IsOpen = true;
                    podeAbrirEventos = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar eventos: " + ex.Message);
            }

            

        }

        


        private void BtnFecharPopupEventos_Click(object sender, RoutedEventArgs e)
        {

            ativarbtalterar();
            exibirbtexcluir();

            popupEventos.IsOpen = false;

            int codEvento = Convert.ToInt32(lbEventos.SelectedValue);

            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();
                    string sql = "SELECT * FROM Eventos WHERE Cod_Evento = @Cod_Evento";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Cod_Evento", codEvento);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtnomedoevento.Text = reader["Nome_Evento"].ToString();
                                txtcodigo.Text = reader["Cod_Evento"].ToString();
                                TxtEnderecoEvent1.Text = reader["Local"].ToString();
                                TxtEnderecoEventNumero.Text = reader["Numero_Local"].ToString();
                                TxtInforCepEventos.Text = reader["CEP_Local"].ToString();
                                lotacaomaximatleven.Text = reader["Lotacao_Maxima"].ToString();
                                orcamentotleventos.Text = reader["Orcamento_Maximo"].ToString();
                                valortotalpartic.Text = reader["Total_Participantes"].ToString();
                                cbtipoeventotleventos.SelectedValue = reader["Cod_Tipo"];
                                valortotal.Text = reader["Valor_Total"].ToString();
                                txtobs.Text = reader["Observacao"].ToString();
                                dpdatadoevent.SelectedDate = Convert.ToDateTime(reader["Data_Evento"]);
                                dpdatadoevent_ate.SelectedDate = Convert.ToDateTime(reader["Data_Fim_Evento"]);

                                // Carrega fornecedores vinculados ao evento
                                CarregarFornecedoresDoEvento(codEvento);

                                CarregarParticipantesDoEvento(codEvento);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar evento: " + ex.Message);
            }




        }

        private void CarregarFornecedoresDoEvento(int codEvento)
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    //  Carrega todos os fornecedores
                    string sqlTodos = @"SELECT Nome, CPF_CNPJ, Preco FROM Fornecedores";
                    SqlDataAdapter daTodos = new SqlDataAdapter(sqlTodos, con);
                    DataTable dtTodos = new DataTable();
                    daTodos.Fill(dtTodos);

                    lbFornecedores.ItemsSource = dtTodos.DefaultView;
                    lbFornecedores.SelectedValuePath = "CPF_CNPJ";

                    //  Busca os fornecedores vinculados ao evento
                    string sqlSelecionados = @"SELECT cod_forn, Preco_forn FROM foreventos WHERE cod_event = @cod_event";
                    SqlCommand cmdSel = new SqlCommand(sqlSelecionados, con);
                    cmdSel.Parameters.AddWithValue("@cod_event", codEvento);
                    SqlDataAdapter daSel = new SqlDataAdapter(cmdSel);
                    DataTable dtSel = new DataTable();
                    daSel.Fill(dtSel);

                    //  Limpa seleção antes de marcar
                    lbFornecedores.UnselectAll();

                    // Marcar apenas os fornecedores do evento
                    foreach (DataRow row in dtSel.Rows)
                    {
                        string codForn = row["cod_forn"].ToString();
                        decimal preco = Convert.ToDecimal(row["Preco_forn"]);
                        foreach (DataRowView item in lbFornecedores.Items)
                        {
                            if (item["CPF_CNPJ"].ToString() == codForn)
                            {
                                lbFornecedores.SelectedItems.Add(item);
                                // Atualiza o preço do item na ListBox
                                item["Preco"] = preco;
                                break;
                            }
                        }
                    }

                    // Atualiza TextBox resumo e total
                    var selecionados = lbFornecedores.SelectedItems
                        .Cast<DataRowView>()
                        .Select(r => new
                        {
                            Nome = r["Nome"].ToString(),
                            Preco = Convert.ToDecimal(r["Preco"])
                        })
                        .ToList();

                    txtFornecedores.Text = string.Join(", ", selecionados.Select(f => $"{f.Nome} - {f.Preco:N2}"));
                    valortotal.Text = selecionados.Sum(f => f.Preco).ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar fornecedores: " + ex.Message);
            }
        }

        private void CarregarParticipantesDoEvento(int codEvento)
        {
            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    // Buscar os participantes vinculados ao evento
                    string sqlSelecionados = @"SELECT cod_part FROM parteventos WHERE cod_event = @cod_event";
                    SqlCommand cmdSel = new SqlCommand(sqlSelecionados, con);
                    cmdSel.Parameters.AddWithValue("@cod_event", codEvento);

                    SqlDataAdapter daSel = new SqlDataAdapter(cmdSel);
                    DataTable dtSel = new DataTable();
                    daSel.Fill(dtSel);

                    // Limpa seleção antes de marcar
                    lbParticipantes.UnselectAll();

                    // Marcar apenas os que pertencem ao evento
                    foreach (DataRow row in dtSel.Rows)
                    {
                        string codPart = row["cod_part"].ToString();
                        foreach (DataRowView item in lbParticipantes.Items)
                        {
                            if (item["CPF_CNPJ"].ToString() == codPart)
                            {
                                lbParticipantes.SelectedItems.Add(item);
                                break;
                            }
                        }
                    }

                    // Atualizar TextBox resumo
                    var selecionados = lbParticipantes.SelectedItems
                        .Cast<DataRowView>()
                        .Select(r => r["Nome"].ToString())
                        .ToList();

                    txtParticipantes.Text = string.Join(", ", selecionados);
                    valortotalpartic.Text = selecionados.Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar participantes: " + ex.Message);
            }
        }


        private void btalterareventos_Click(object sender, RoutedEventArgs e)
        {
            if (lbEventos.SelectedItem == null)
            {
                MessageBox.Show("Selecione um evento para alterar.");
                return;
            }

            // Validação de lotação antes de abrir a conexão
            if (int.TryParse(valortotalpartic.Text, out int totalParticipantesvld) &&
                int.TryParse(lotacaomaximatleven.Text, out int lotacaoMaximavld))
            {
                if (totalParticipantesvld > lotacaoMaximavld)
                {
                    MessageBox.Show("O total de participantes selecionados ultrapassa a lotação máxima do evento!");
                    return; // sai do método, não cria o evento
                }
            }

            int codEvento = Convert.ToInt32(lbEventos.SelectedValue);

            if (decimal.TryParse(valortotal.Text, out decimal totalvl) &&
            decimal.TryParse(orcamentotleventos.Text, out decimal orcamentovl))
            {
                if (totalvl > orcamentovl)
                {
                    MessageBox.Show("O valor total dos fornecedores ultrapassa o orçamento máximo permitido!");
                    return; 
                }
            }


            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    // Atualiza os dados do evento
                    string sqlUpdateEvento = @"
                UPDATE Eventos
                SET Nome_Evento = @Nome_Evento,
                    Cod_Tipo = @Cod_Tipo,
                    Local = @Local,
                    Numero_Local = @Numero_Local,
                    CEP_Local = @CEP_Local,
                    Lotacao_Maxima = @Lotacao_Maxima,
                    Orcamento_Maximo = @Orcamento_Maximo,
                    Total_Participantes = @Total_Participantes,
                    Valor_Total = @Valor_Total,
                    Observacao = @Observacao,
                    Data_Evento = @Data_Evento,
                    Data_Fim_Evento = @Data_Fim_Evento
                WHERE Cod_Evento = @Cod_Evento";

                    using (SqlCommand cmd = new SqlCommand(sqlUpdateEvento, con))
                    {
                        cmd.Parameters.AddWithValue("@Cod_Evento", codEvento);
                        cmd.Parameters.AddWithValue("@Nome_Evento", txtnomedoevento.Text);
                        cmd.Parameters.AddWithValue("@Cod_Tipo", cbtipoeventotleventos.SelectedValue ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Local", TxtEnderecoEvent1.Text);
                        cmd.Parameters.AddWithValue("@Numero_Local", TxtEnderecoEventNumero.Text);
                        cmd.Parameters.AddWithValue("@CEP_Local", TxtInforCepEventos.Text);

                        if (!int.TryParse(lotacaomaximatleven.Text, out int lotacao))
                            lotacao = 0;
                        cmd.Parameters.AddWithValue("@Lotacao_Maxima", lotacao);

                        if (!decimal.TryParse(orcamentotleventos.Text, out decimal orcamento))
                            orcamento = 0;
                        cmd.Parameters.AddWithValue("@Orcamento_Maximo", orcamento);

                        if (!int.TryParse(valortotalpartic.Text, out int totalParticipantes))
                            totalParticipantes = 0;
                        cmd.Parameters.AddWithValue("@Total_Participantes", totalParticipantes);

                        if (!decimal.TryParse(valortotal.Text, out decimal valorTotal))
                            valorTotal = 0;
                        cmd.Parameters.AddWithValue("@Valor_Total", valorTotal);

                        cmd.Parameters.AddWithValue("@Observacao", txtobs.Text);

                        if (dpdatadoevent.SelectedDate.HasValue && dpdatadoevent_ate.SelectedDate.HasValue)
                        {
                            DateTime dataInicio = dpdatadoevent.SelectedDate.Value;
                            DateTime dataFim = dpdatadoevent_ate.SelectedDate.Value;

                            if (dataFim < dataInicio)
                            {
                                MessageBox.Show("A data de término não pode ser menor que a data de início do evento.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Selecione as duas datas (início e fim) do evento.");
                            return;
                        }


                        // Ajuste para Data_Evento nula
                        object dataEventoParam = dpdatadoevent.SelectedDate.HasValue
                            ? (object)dpdatadoevent.SelectedDate.Value
                            : DBNull.Value;
                        cmd.Parameters.AddWithValue("@Data_Evento", dataEventoParam);

                        // Ajuste para Data_Fim_Evento nula
                        object dataFimEventoParam = dpdatadoevent_ate.SelectedDate.HasValue
                            ? (object)dpdatadoevent_ate.SelectedDate.Value
                            : DBNull.Value;
                        cmd.Parameters.AddWithValue("@Data_Fim_Evento", dataFimEventoParam);

                        cmd.ExecuteNonQuery();
                    }

                    // Atualiza os participantes: remove antigos e insere os novos
                    string sqlDeleteParticipantes = "DELETE FROM parteventos WHERE cod_event = @cod_event";
                    using (SqlCommand cmdDel = new SqlCommand(sqlDeleteParticipantes, con))
                    {
                        cmdDel.Parameters.AddWithValue("@cod_event", codEvento);
                        cmdDel.ExecuteNonQuery();
                    }

                    string sqlInsertParticipantes = @"
                INSERT INTO parteventos (cod_event, cod_part, DataCadastro, Data_Evento,Data_Fim_Evento)
                VALUES (@cod_event, @cod_partev, @DataCadastro, @Data_Evento,@Data_Fim_Evento)";


                    DateTime novoInicio = dpdatadoevent.SelectedDate.Value;
                    DateTime novoFim = dpdatadoevent_ate.SelectedDate.Value;

                    foreach (var item in lbParticipantes.SelectedItems.Cast<DataRowView>())
                    {
                        string codParticipante = item["CPF_CNPJ"].ToString();

                        string sqlCheckConflito = @"
        SELECT COUNT(*)
        FROM parteventos p
        INNER JOIN Eventos e ON p.cod_event = e.Cod_Evento
        WHERE p.cod_part = @cod_part
          AND e.Cod_Evento <> @cod_event -- ignora o evento atual
          AND e.Data_Evento <= @novoFim
          AND e.Data_Fim_Evento >= @novoInicio";

                        using (SqlCommand cmdCheck = new SqlCommand(sqlCheckConflito, con))
                        {
                            cmdCheck.Parameters.AddWithValue("@cod_part", codParticipante);
                            cmdCheck.Parameters.AddWithValue("@cod_event", codEvento);
                            cmdCheck.Parameters.AddWithValue("@novoInicio", novoInicio);
                            cmdCheck.Parameters.AddWithValue("@novoFim", novoFim);

                            int conflito = (int)cmdCheck.ExecuteScalar();
                            if (conflito > 0)
                            {
                                MessageBox.Show($"O participante {item["Nome"]} já está em outro evento no mesmo período!");
                                return; // sai sem salvar nada
                            }
                        }
                    }


                    foreach (var item in lbParticipantes.SelectedItems.Cast<DataRowView>())
                    {
                        using (SqlCommand cmdPart = new SqlCommand(sqlInsertParticipantes, con))
                        {
                            cmdPart.Parameters.AddWithValue("@cod_event", codEvento);
                            cmdPart.Parameters.AddWithValue("@cod_partev", item["CPF_CNPJ"]);

                            object dataEventoPart = dpdatadoevent.SelectedDate.HasValue
                                ? (object)dpdatadoevent.SelectedDate.Value
                                : DBNull.Value;
                            object dataFimEventoPart = dpdatadoevent_ate.SelectedDate.HasValue
                                ? (object)dpdatadoevent_ate.SelectedDate.Value
                                : DBNull.Value;
                            cmdPart.Parameters.AddWithValue("@Data_Fim_Evento", dataFimEventoPart);
                            cmdPart.Parameters.AddWithValue("@Data_Evento", dataEventoPart);
                            cmdPart.Parameters.AddWithValue("@DataCadastro", DateTime.Now);

                            cmdPart.ExecuteNonQuery();
                        }
                    }

                    // Atualiza fornecedores: remove antigos e insere os novos
                    string sqlDeleteFornecedores = "DELETE FROM foreventos WHERE cod_event = @cod_event";
                    using (SqlCommand cmdDelForn = new SqlCommand(sqlDeleteFornecedores, con))
                    {
                        cmdDelForn.Parameters.AddWithValue("@cod_event", codEvento);
                        cmdDelForn.ExecuteNonQuery();
                    }

                    string sqlInsertFornecedores = @"
                INSERT INTO foreventos (cod_event, cod_forn, Preco_forn, DataCadastro)
                VALUES (@cod_event, @cod_forn, @Preco_forn, @DataCadastro)";

                    foreach (var item in lbFornecedores.SelectedItems.Cast<DataRowView>())
                    {
                        using (SqlCommand cmdForn = new SqlCommand(sqlInsertFornecedores, con))
                        {
                            cmdForn.Parameters.AddWithValue("@cod_event", codEvento);
                            cmdForn.Parameters.AddWithValue("@cod_forn", item["cpf_cnpj"]);

                            if (!decimal.TryParse(item["Preco"].ToString(), out decimal preco))
                                preco = 0;
                            cmdForn.Parameters.AddWithValue("@Preco_forn", preco);

                            cmdForn.Parameters.AddWithValue("@DataCadastro", DateTime.Now);
                            cmdForn.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Evento alterado com sucesso!");
                    desativarbtalterar();
                    apagartudo();
                    desativarbtalterar();
                    sumirbtcancelar();
                    ativarbtcriar();
                    sumirbtexcluir();
                    podeAbrirEventos = false;
                    popupEventos.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar evento: " + ex.Message);
            }
        }

        private void btcancelar_Click(object sender, RoutedEventArgs e)
        {
            apagartudo();
            desativarbtalterar();
            sumirbtcancelar();
            ativarbtcriar();
            sumirbtexcluir();
            podeAbrirEventos = false;
            popupEventos.IsOpen = false; 

        }

        private void excluir_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtcodigo.Text))
            {
                MessageBox.Show("Selecione um evento para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtcodigo.Text, out int codEvento))
            {
                MessageBox.Show("Código do evento inválido.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var confirm = MessageBox.Show(
                "Tem certeza que deseja excluir este evento?\nTodos os dados vinculados também serão removidos.",
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                using (SqlConnection con = Banco.GetConexao())
                {
                    con.Open();

                    // Inicia a transação
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            // 1 - excluir participantes vinculados
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM parteventos WHERE cod_event = @cod_event", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@cod_event", codEvento);
                                cmd.ExecuteNonQuery();
                            }

                            // 2 - excluir fornecedores vinculados
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM foreventos WHERE cod_event = @cod_event", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@cod_event", codEvento);
                                cmd.ExecuteNonQuery();
                            }

                            // 3 - excluir o evento
                            using (SqlCommand cmd = new SqlCommand("DELETE FROM eventos WHERE cod_evento = @cod_event", con, trans))
                            {
                                cmd.Parameters.AddWithValue("@cod_event", codEvento);
                                int rows = cmd.ExecuteNonQuery();

                                if (rows == 0)
                                {
                                    throw new Exception("Evento não encontrado para exclusão.");
                                }
                            }

                            // Confirma a transação
                            trans.Commit();
                            MessageBox.Show("Evento excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                            //
                            apagartudo();
                            desativarbtalterar();
                            sumirbtcancelar();
                            ativarbtcriar();
                            sumirbtexcluir();
                            podeAbrirEventos = false;
                            popupEventos.IsOpen = false;
                        }
                        catch (Exception ex)
                        {
                            // Se algo falhar, desfaz tudo
                            trans.Rollback();
                            MessageBox.Show("Erro ao excluir evento: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    }




}