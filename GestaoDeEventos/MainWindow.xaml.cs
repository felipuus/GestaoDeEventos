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

        private void bttelaconsultarcep_Click(object sender, RoutedEventArgs e)
        {
            telabuscacep novaJanela = new telabuscacep();

            novaJanela.Show();


            

        }

        private void bttelaeventos_Click(object sender, RoutedEventArgs e)
        {
            TelaEventos abrirtelaeventos = new TelaEventos();

            abrirtelaeventos.Show();
        }
    }
}