using MySql.Data.MySqlClient;
using StagiaireLaCite.VuesModels;
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
using System.Windows.Shapes;

namespace StagiaireLaCite.Vues
{
    /// <summary>
    /// Logique d'interaction pour Programmes.xaml
    /// </summary>
    public partial class Programmes : UserControl
    {
        public Programmes()
        {
            InitializeComponent();
            

        }




        
        //Fonction permettant d'ajouter un programme
        private void AjouterProgramme_Click(object sender, RoutedEventArgs e)
        {
            int numProgramme = 0;
            string nomProgramme;
            string dureeProgramme;
            try
            {
                numProgramme = int.Parse(NumeroDuProgramme.Text);
                nomProgramme = NomDuProgramme.Text;
                dureeProgramme = DureeProgramme.Text;
                ProgrammesVM viewModel = DataContext as ProgrammesVM;
                if (int.TryParse(NumeroDuProgramme.Text, out numProgramme))
                {
                    if (numProgramme >= 1000000 && numProgramme <= 9999999)
                    {
                        //Verifions qu'aucun des champs n'est vide
                        if (!string.IsNullOrEmpty(nomProgramme) && dureeProgramme.Length > 3)
                        {
                            // Vérifion que  le numéro de programme existe déjà dans la liste des Prog.
                            if (viewModel.ProgrammeExisteDeja(numProgramme))
                            {
                                Message.Text = "Ce programme existe deja Dans la BD!!";
                                Message.Foreground = Brushes.Red;
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
                                    string requette = "INSERT INTO Programme (Numero_programme, Nom_programme, Duree_programme) VALUES (@NumeroProgramme, @NomProgramme, @DureeProgramme)";
                                    MySqlCommand cmd = new MySqlCommand(requette, connection);
                                    cmd.Parameters.AddWithValue("@NumeroProgramme", numProgramme);
                                    cmd.Parameters.AddWithValue("@NomProgramme", nomProgramme);
                                    cmd.Parameters.AddWithValue("@DureeProgramme", dureeProgramme);
                                    cmd.ExecuteNonQuery();


                                    //Affichage du message de reussite
                                    Message.Text = "Programme ajouté avec succès";
                                    Message.Foreground = Brushes.Green;
                                }
                                catch (MySqlException ex)
                                {
                                    Message.Text = "Erreur lors de la connexion a la BD";
                                    Message.Foreground = Brushes.Red;
                                }
                                finally
                                {
                                    connection.Close();
                                }

                            }
                        }
                        else
                        {
                            Message.Text = "Veuillez remplir tous les champs";
                            Message.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        Message.Text = "Le numéro de programme doit avoir 7 chiffres";
                        Message.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    Message.Text = "Le numéro de programme doit être un nombre valide";
                    Message.Foreground = Brushes.Red;
                }
            }
            catch (Exception ex)
            {

                Message.Text = "Le numéro du programme ne doit pas contenir de lettres !!";
                Message.Foreground = Brushes.Red;
            }
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            Message.Text = "Veuillez remplir tous les champs";
            Message.Foreground = Brushes.Red;
            NumeroDuProgramme.Text = "";
            NomDuProgramme.Text = "";
            DureeProgramme.Text = "";
        }
       


    }
}
