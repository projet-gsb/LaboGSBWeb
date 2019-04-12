using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOGestionFrais
{
    public class FicheDeFrais
    {
        private int _id;
        private int _idVisiteurMedical;
        private DateTime _dateCreation;
        private DateTime _dateTraitement;
        private bool _miseEnPaiement;

        public int Id { get => _id; set => _id = value; }
        public int IdVisiteurMedical { get => _idVisiteurMedical; set => _idVisiteurMedical = value; }
        public DateTime DateCreation { get => _dateCreation; protected set => _dateCreation = value; }
        public DateTime DateTraitement { get => _dateTraitement; protected set => _dateTraitement = value; }
        public bool MiseEnPaiement { get => _miseEnPaiement; protected set => _miseEnPaiement = value; }

        public FicheDeFrais(int id, int idvisiteurmedical, DateTime datecreation, DateTime datetraitement, bool miseenpaiement)
        {
            this.Id = id;
            this.IdVisiteurMedical = idvisiteurmedical;
            this.DateCreation = datecreation;
            this.DateTraitement = datetraitement;
            this.MiseEnPaiement = miseenpaiement;
        }
    }
}