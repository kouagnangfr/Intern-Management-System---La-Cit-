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

    public partial class Stagiaires : UserControl
    {
        
        public static readonly DependencyProperty ListeDesProgrammesProperty =
            DependencyProperty.Register("ListeDesProgrammes", typeof(ObservableCollection<string>), typeof(Stagiaires), new PropertyMetadata(null));

        public ObservableCollection<string> ListeDesProgrammes
        {
            get { return (ObservableCollection<string>)GetValue(ListeDesProgrammesProperty); }
            set { SetValue(ListeDesProgrammesProperty, value); }
        }

        //Constructeur
        public Stagiaires()
        {
            InitializeComponent();
            StagiairesVM viewModel = new StagiairesVM();
            ListeDesProgrammes = new ObservableCollection<string>(viewModel.RecuperListeProgramme());
            DataContext = viewModel;
        }


        //Code pour ajouter un satgiaire
        private void AjouterStagiaire_Click(object sender, RoutedEventArgs e)
        {
            int numEtudiant;
            string nomPrenom, sexe, programme;
            DateTime dateNaissance;

            try
            {
                numEtudiant = int.Parse(NumeroEtudiant.Text);
                nomPrenom = NomPrenomStagiaire.Text;
                dateNaissance = DateNaissance.SelectedDate ?? DateTime.MinValue;
                sexe = Sexe.Text;
                programme = (Programme.Text).Substring(6,12);
                StagiairesVM viewModel = DataContext as StagiairesVM;
                if (numEtudiant >= 1000000 && numEtudiant <= 9999999)
                {
                    if (!string.IsNullOrEmpty(nomPrenom) && dateNaissance != DateTime.MinValue
                        && !string.IsNullOrEmpty(sexe) && !string.IsNullOrEmpty(programme))
                    {
                        int age = CalculerAge(dateNaissance);
                        if (age >= 15)
                        {
                            if (viewModel.StagiaireExisteDeja(numEtudiant))
                            {
                                Message2.Text = "Un etudiant avec ce numero existe deja !!";
                                Message2.Foreground = Brushes.Red;
                            }
                            else
                            {
                                string chaineDeConnexion = "Server=localhost;Database=stagiaires_la_cite;" +
                                        "Uid=root;Pwd=;";
                                MySqlConnection connection = new MySqlConnection(chaineDeConnexion);
                                try
                                {
                                    connection.Open();

                                    //Ajout du programme a la Base de donnees
                                    string requette = "INSERT INTO Stagiaire (Numero_etudiant, NomPrenom_etudiant,	DateNaissance_etudiant,	Sexe_etudiant,	Numero_programme)" +
                                        " VALUES (@numEtudiant, @nomPrenom, @dateNaissance, @sexe, @programme)";
                                    MySqlCommand cmd = new MySqlCommand(requette, connection);
                                    cmd.Parameters.AddWithValue("@numEtudiant", numEtudiant);
                                    cmd.Parameters.AddWithValue("@nomPrenom", nomPrenom);
                                    cmd.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                                    cmd.Parameters.AddWithValue("@sexe", sexe);
                                    cmd.Parameters.AddWithValue("@programme", programme);
                                    cmd.ExecuteNonQuery();


                                    //Affichage du message de reussite
                                    Message2.Text = "Stagiaire ajouter avec success !!";
                                    Message2.Foreground = Brushes.Green;
                                }
                                catch (MySqlException ex)
                                {
                                    Message2.Text = "Erreur lors de la connexion a la BD" + programme + "f";
                                    Message2.Foreground = Brushes.Red;
                                }
                                finally
                                {
                                    connection.Close();
                                }
                            }
                        }
                        else
                        {
                            Message2.Text = "Le candidat doit avoir au moins 15 ans pour s'inscrire !!";
                            Message2.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        Message2.Text = "Veuillez remplir tous les champs!!";
                        Message2.Foreground = Brushes.Red;
                    }

                }
                else
                {
                    Message2.Text = "Le numéro d'étudiant doit avoir 7 chiffres !!";
                    Message2.Foreground = Brushes.Red;
                }
            }
            catch (Exception)
            {
                Message2.Text = "Le numero d'etudiant doit etre une suite de 7 chiffres !!";
                Message2.Foreground = Brushes.Red;
            }
        }


        //Fonction calculant l'age car l'age min est de 15ans
        private int CalculerAge(DateTime dateNaissance)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - dateNaissance.Year;
            if (dateNaissance > now.AddYears(-age))
                age--;
            return age;
        }

        //vider les champs du formulaire
        private void Supprimer_Click2(object sender, RoutedEventArgs e)
        {
            NumeroEtudiant.Text = "";
            NomPrenomStagiaire.Text = "";
            DateNaissance.Text = "";
            Sexe.Text = "";
            Programme.Text = "";
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
