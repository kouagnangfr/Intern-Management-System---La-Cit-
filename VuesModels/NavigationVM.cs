using StagiaireLaCite.Utilitaires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StagiaireLaCite.VuesModels
{

    //Gestion de la navigation entre les differentes fenetres
    class NavigationVM : ModelBaseVue
    {
        private object vueCourante;
        public object VueCourante
        {
            get { return vueCourante; }
            set { vueCourante = value; OnPropertyChanged(); }
        }

        public ICommand CommandeAccueil { get; set; }
        public ICommand CommandeConsulter { get; set; }
        public ICommand CommandeProgrammes { get; set; }
        public ICommand CommandeParametres { get; set; }
        public ICommand CommandeStagiaires { get; set; }


        private void Accueil(object obj) => VueCourante = new AccueilVM();
        private void Consulter(object obj) => VueCourante = new ConsulterVM();
        private void Parametres(object obj) => VueCourante = new ParametresVM();
        private void Programmes(object obj) => VueCourante = new ProgrammesVM();
        private void Stagiaires(object obj) => VueCourante = new StagiairesVM();


        public NavigationVM()
        {
            CommandeAccueil = new CommandeDeRelais(Accueil);
            CommandeConsulter = new CommandeDeRelais(Consulter);
            CommandeParametres = new CommandeDeRelais(Parametres);
            CommandeProgrammes = new CommandeDeRelais(Programmes);
            CommandeStagiaires = new CommandeDeRelais(Stagiaires);

            // Page de depart
            VueCourante = new AccueilVM();
        }


    }
    
}
