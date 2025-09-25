using System.Text;
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
using System.Data.SqlClient;
using System.Data;



namespace GestaoDeEventos
{

    



    public partial class MainWindow : Window
    {

      
       






        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void bttelaeventos_Click(object sender, RoutedEventArgs e)
        {
            TelaEventos abrirtelaeventos = new TelaEventos();

            abrirtelaeventos.Show();
        }

        private void bttelafornecedores_Click(object sender, RoutedEventArgs e)
        {
            Fornecedores abrirtelafornecedores = new Fornecedores();
            abrirtelafornecedores.Show();
        }

        private void bttelaparticipante_Click(object sender, RoutedEventArgs e)
        {
            Participantes abrirtelaparticipantes = new Participantes();
            abrirtelaparticipantes.Show();

        }

        private void bttelatipodeevento_Click(object sender, RoutedEventArgs e)
        {
            TipoDeEvento abrirtelatipodeevento = new TipoDeEvento();
            abrirtelatipodeevento.Show();
        }
    }
}