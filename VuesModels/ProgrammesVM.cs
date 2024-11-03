using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using StagiaireLaCite.Model;
using StagiaireLaCite.Vues;

namespace StagiaireLaCite.VuesModels
{
    class ProgrammesVM : Utilitaires.ModelBaseVue
    {
        private ObservableCollection<Programme> _listeProgrammes;

        public ObservableCollection<Programme> ListeProgrammes
        {
            get { return _listeProgrammes; }
            set
            {
                _listeProgrammes = value;
                OnPropertyChanged(nameof(ListeProgrammes));
            }
        }

        //  initialisation de la liste 
        public ProgrammesVM()
        {
            ListeProgrammes = new ObservableCollection<Programme>();
        }

        // Méthode pour ajouter un nouveau programme à la liste
        public void AjouterProgramme(int numero, string nom, string duree)
        {
            Programme nouveauProgramme = new Programme(numero, nom, duree);
            ListeProgrammes.Add(nouveauProgramme);
            OnPropertyChanged(nameof(ListeProgrammes));
        }

        //Verifier si un programme existe deja dans la Base de donnees
        public bool ProgrammeExisteDeja(int numeroProgramme)
        {
            string chaineDeConnexion = "Server=localhost;Database=stagiaires_la_cite;" +
                                       "Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(chaineDeConnexion);
            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Programme WHERE Numero_programme = @NumeroProgramme";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@NumeroProgramme", numeroProgramme);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (MySqlException ex)
            {

                Console.WriteLine("Erreur lors de la vérification de l'existence du programme dans la base de données : ");
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

    }

}

public class Programme
{
    public int NumeroProgramme { get; set; }
    public string NomProgramme { get; set; }
    public string DureeProgramme { get; set; }

    public Programme(int numeroProgramme, string nomProgramme, string dureeProgramme)
    {
        NumeroProgramme = numeroProgramme;
        NomProgramme = nomProgramme;
        DureeProgramme = dureeProgramme;
    }
}

