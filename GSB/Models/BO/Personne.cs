using LaboGSB.Models.BO.BOCompteRendu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.Personne
{
    public interface IPersonne
    {

    }

    public abstract class Personne : IPersonne
    {
        private string _nom;
        private int _id;
        private string _prenom;
        private string _mel;
        private string _numeroTelephone;

        public int Id { get => _id; set => _id = value; }
        public string Nom { get => _nom; set => _nom = value; }
        public string Prenom { get => _prenom; set => _prenom = value; }
        public string Mel { get => _mel; set => _mel = value; }
        public string NumeroTelephone { get => _numeroTelephone; set => _numeroTelephone = value; }

        protected Personne()
        {
            this.Id = 0;
            this.Nom = "";
            this.Prenom = "";
            this.Mel = "";
            this.NumeroTelephone = "";
        }

        protected Personne(int id, string nom, string prenom, string mel, string numeroTelephone)
        {
            this.Id = id;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Mel = mel;
            this.NumeroTelephone = numeroTelephone;
        }
        public override string ToString()
        {
            return this.Nom.ToUpper() + " " + this.Prenom;
        }
    }

    public class VisiteurMedical : Personne

    {
        private DateTime _dateEmbauche;
        private string _zoneGeographique;
        private List<Etablissement> _client;
        private string _motDePasse;

        public DateTime DateEmbauche { get => _dateEmbauche; set => _dateEmbauche = value; }
        public string ZoneGeographique { get => _zoneGeographique; set => _zoneGeographique = value; }
        public List<Etablissement> Client { get => _client; set => _client = value; }
        public string MotDePasse { get => _motDePasse; set => _motDePasse = value; }

        public VisiteurMedical() : base()
        {
            this.DateEmbauche = new DateTime();
            this.ZoneGeographique = "";
            this.Client = new List<Etablissement>();
            this.MotDePasse = "";

        }

        public VisiteurMedical(DateTime dateEmbauche, string zoneGeographique, List<Etablissement> client, string motDePasse, int idPersonne,
                                    string nom, string prenom, string mel, string numeroTelephone)
                                        : base(idPersonne, nom, prenom, mel, numeroTelephone)
        {
            this.DateEmbauche = dateEmbauche;
            this.ZoneGeographique = zoneGeographique;
            this.Client = client;
            this.MotDePasse = motDePasse;

        }

        public VisiteurMedical(DateTime dateEmbauche, string zoneGeographique, int idPersonne, string nom, string prenom,
                                        string mel, string numeroTelephone) : base(idPersonne, nom, prenom, mel, numeroTelephone)
        {
            this.DateEmbauche = dateEmbauche;
            this.ZoneGeographique = zoneGeographique;


        }
    }

    public class Contact : Personne
    {
        private string _poste;
        private string _commentaire;
        private List<Etablissement> _employeur;

        public string Poste { get => _poste; set => _poste = value; }
        public string Commentaire { get => _commentaire; set => _commentaire = value; }
        public List<Etablissement> Employeur { get => _employeur; set => _employeur = value; }


        public Contact() : base()
        {
            this.Poste = "";
            this.Commentaire = "";
            this.Employeur = new List<Etablissement>();
        }

        public Contact(string poste, string commentaire, List<Etablissement> employeur, int idPersonne, string nom, string prenom,
                                        string mel, string numeroTelephone)
                                        : base(idPersonne, nom, prenom, mel, numeroTelephone)
        {
            this.Poste = poste;
            this.Commentaire = commentaire;
            this.Employeur = employeur;
        }
    }
}