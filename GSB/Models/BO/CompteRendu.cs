using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaboGSB.Models.BO.Personne;

namespace LaboGSB.Models.BO.BOCompteRendu
{
    public class CompteRendu
    {
  
        private int _id;
        private VisiteurMedical _visiteurMedicalConcerne;
        private Contact _contactConcerne;
        private Etablissement _etablissementConcerne;
        private string _titre;
        private string _contenu;
        private DateTime _date;
        private List<Echantillon> _listeEchantillon;

        public int Id { get => _id; set => _id = value; }
        public VisiteurMedical VisiteurMedicalConcerne { get => _visiteurMedicalConcerne; set => _visiteurMedicalConcerne = value; }
        public Contact ContactConcerne { get => _contactConcerne; set => _contactConcerne = value; }
        public Etablissement EtablissementConcerne { get => _etablissementConcerne; set => _etablissementConcerne = value; }
        public string Titre { get => _titre; set => _titre = value; }
        public string Contenu { get => _contenu; set => _contenu = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public List<Echantillon> ListeEchantillon { get => _listeEchantillon; set => _listeEchantillon = value; }

        public CompteRendu()
        {
            this.Id = 0;
            this.VisiteurMedicalConcerne = new VisiteurMedical();
            this.ContactConcerne = new Contact();
            this.EtablissementConcerne = new Etablissement();
            this.Titre = "";
            this.Contenu = "";
            this.Date = new DateTime();
            this.ListeEchantillon = new List<Echantillon>();
        }

        public CompteRendu(int id, VisiteurMedical visiteurMedical, Contact contact, Etablissement etablissement, string titre, string contenu, DateTime date, List<Echantillon> listeEchantillon)
        {
            this.Id = id;
            this.VisiteurMedicalConcerne = visiteurMedical;
            this.ContactConcerne = contact;
            this.EtablissementConcerne = etablissement;
            this.Titre = titre;
            this.Contenu = contenu;
            this.Date = date;
            this.ListeEchantillon = listeEchantillon;
        }
    }
}
