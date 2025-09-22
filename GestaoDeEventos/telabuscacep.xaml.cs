using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;



namespace GestaoDeEventos
{
    /// <summary>
    /// Interação lógica para telabuscacep.xam
    /// </summary>
    public partial class telabuscacep : Window
    {
        public telabuscacep()
        {
            InitializeComponent();
        }

        
        private void inforCep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) // Se apertar Enter
            {
                buscarCep_Click(buscarCep, null); // chama o mesmo método do botão
            }
        }


        // Esse método é chamado toda vez que o texto do TextBox muda
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (txt != null)
            {
                // Exemplo: só mostra no título da janela o texto digitado
                this.Title = txt.Text;
            }
        }

        private async void buscarCep_Click(object sender, RoutedEventArgs e)
        {
            string cep = inforCep.Text.Trim();

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
                        endereco.Content = "CEP não encontrado!";
                    }
                    else
                    {
                        string logradouro = (string)obj["logradouro"];
                        string bairro = (string)obj["bairro"];
                        string localidade = (string)obj["localidade"];
                        string uf = (string)obj["uf"];

                        endereco.Content = $"{logradouro}, {bairro} - {localidade}/{uf}";
                    }
                }
            }
            catch
            {
                endereco.Content = "Erro ao buscar o CEP.";
            }
        }


    }
}
