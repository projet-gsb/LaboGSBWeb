using LaboGSB.Models.BO.Personne;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOGestionFrais
{
    public class FicheDeFrais
    {
        private int _id;
        //private int _idVisiteurMedical;  
        private VisiteurMedical _visiteurMedicalConcerne;
        private DateTime _dateCreation;
        private DateTime _dateTraitement;
        private bool _miseEnPaiement;

        public int Id { get => _id; set => _id = value; }
        //public int IdVisiteurMedical { get => _idVisiteurMedical; set => _idVisiteurMedical = value; }
        public VisiteurMedical VisiteurMedicalConcerne { get => _visiteurMedicalConcerne; set => _visiteurMedicalConcerne = value; }
        public DateTime DateCreation { get => _dateCreation; protected set => _dateCreation = value; }
        public DateTime DateTraitement { get => _dateTraitement; protected set => _dateTraitement = value; }
        public bool MiseEnPaiement { get => _miseEnPaiement; protected set => _miseEnPaiement = value; }

        public FicheDeFrais()
        {
            this.Id = 0;
            //this.IdVisiteurMedical = 0;           //à modifier par ID du visiteur connecté
            this.VisiteurMedicalConcerne = new VisiteurMedical();
            this.DateCreation = new DateTime();
            this.DateTraitement = new DateTime();
            this.MiseEnPaiement = false;
        }
                       
        public FicheDeFrais(int id, VisiteurMedical visiteurMedical, DateTime datecreation, DateTime datetraitement, bool miseenpaiement)
        {
            this.Id = id;
            //this.IdVisiteurMedical = idvisiteurmedical;
            this.VisiteurMedicalConcerne = new VisiteurMedical();
            this.DateCreation = datecreation;
            this.DateTraitement = datetraitement;
            this.MiseEnPaiement = miseenpaiement;
        }
    }
}