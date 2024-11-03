using MySql.Data.MySqlClient;
using StagiaireLaCite.VuesModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace StagiaireLaCite.Vues
{
    /// <summary>
    /// Logique d'interaction pour Consulter.xaml
    /// </summary>


    public partial class Consulter : UserControl
    {
        //liste des etudiants rechercher
        public List<Etudiant> ListeDesEtudiants { get; set; }


        //Preparation du chargement de la liste des programmes
        public static readonly DependencyProperty ListeDesProgrammesProperty =
           DependencyProperty.Register("ListeDesProgrammes", typeof(ObservableCollection<string>), typeof(Stagiaires), new PropertyMetadata(null));

        public ObservableCollection<string> ListeDesProgrammes
        {
            get { return (ObservableCollection<string>)GetValue(ListeDesProgrammesProperty); }
            set { SetValue(ListeDesProgrammesProperty, value); }
        }

        //Constructeur
        public Consulter()
        {
            InitializeComponent();
            ConsulterVM viewModel = new ConsulterVM();
            ListeDesProgrammes = new ObservableCollection<string>(viewModel.RecuperListeProgramme());
            DataContext = viewModel;
        }


        //Rechercher un Etudiant par programme, par nom ou par numero
        private void RechercherBtn_Click(object sender, RoutedEventArgs e)
        {
            ListeDesEtudiants = new List<Etudiant>();

            string chaineDeConnexion = "Server=localhost;Database=stagiaires_la_cite;Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(chaineDeConnexion);
            try
            {
                connection.Open();
                string searchTerm = SearchBox.Text.Trim();
                string programme = (Programme.Text).Substring(6, 12); 

                string query = "SELECT s.Numero_etudiant, s.NomPrenom_etudiant, s.DateNaissance_etudiant, s.Sexe_etudiant " +
                               "FROM Stagiaire s " +
                               "INNER JOIN Programme p ON s.Numero_programme = p.Numero_programme " +
                               "WHERE (s.NomPrenom_etudiant LIKE @searchTerm OR s.Numero_etudiant LIKE @numEtudiant) " +
                               "AND p.Numero_programme = @programme";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                cmd.Parameters.AddWithValue("@numEtudiant", $"%{searchTerm}%"); // Recherche de toute partie du numéro

                cmd.Parameters.AddWithValue("@programme", programme);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Etudiant etudiant = new Etudiant
                    {
                        Numéro = Convert.ToInt32(reader["Numero_etudiant"]),
                        Nom_Prenom = reader["NomPrenom_etudiant"].ToString(),
                        Date_Naissance = reader["DateNaissance_etudiant"].ToString().Substring(0, 10),
                        Sexe = reader["Sexe_etudiant"].ToString()
                    };
                    ListeDesEtudiants.Add(etudiant);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            StudentDataGrid.ItemsSource = ListeDesEtudiants;
        }



        //Affichage de la liste des programmes dans le comboBox
        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Accès au ViewModel
            StagiairesVM viewModel = DataContext as StagiairesVM;
            if (viewModel != null)
            {
                var listeProgrammes = viewModel.RecuperListeProgramme();
                if (listeProgrammes != null)
                {
                    ListeDesProgrammes.Clear();
                    foreach (var programme in listeProgrammes)
                    {
                        ListeDesProgrammes.Add(programme); // Ajout des 
                    }
                }
            }
        }
    }
}


//Classe permettant de modeliser l'affichage d'un etudiant
    public class Etudiant
{
    public int Numéro { get; set; }
    public string Nom_Prenom { get; set; }
    public string Date_Naissance { get; set; }
    public string Sexe { get; set; }
}
