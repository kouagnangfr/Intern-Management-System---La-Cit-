using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using StagiaireLaCite.Model;

namespace StagiaireLaCite.VuesModels
{
    class ConsulterVM : Utilitaires.ModelBaseVue
    {
        public ConsulterVM()
        {

        }

        //recuperation de la liste des programmes de la BD pour le combo Box
        public List<string> RecuperListeProgramme()
        {
            List<string> listeDesProgrammes = new List<string>();
            string chaineDeConnexion = "Server=localhost;Database=stagiaires_la_cite;" +
                                       "Uid=root;Pwd=;";
            MySqlConnection connection = new MySqlConnection(chaineDeConnexion);
            try
            {
                connection.Open();
                string query = "SELECT Numero_programme, Nom_programme, Duree_programme  FROM Programme";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string numeroProgramme = reader["Numero_programme"].ToString();
                    string nomProgramme = reader["Nom_programme"].ToString();
                    string dureeProgramme = reader["Duree_programme"].ToString();
                    string programme = "      " + numeroProgramme + "              " + nomProgramme +
                        "              " + dureeProgramme;
                    listeDesProgrammes.Add(programme);
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
            return listeDesProgrammes;
        }
    }



}
