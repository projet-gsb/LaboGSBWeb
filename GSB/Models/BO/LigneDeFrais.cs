using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOGestionFrais
{
    public class LigneDeFrais
    {
        //Vieille méthode
        /*public int Id { get; }
        public DateTime Date { get; protected set; }
        public double Montant { get; protected set; }
        public string Libelle { get; protected set; }
        public bool Validee { get; protected set; }
        public bool Refusee { get; protected set; }
        public bool HorsForfait { get; protected set; }
        public bool Report { get; protected set; }
        */

        private int _id;
        private DateTime _date;
        private double _montant;
        private string _libelle;
        private bool _validee;
        private bool _refusee;
        private bool _horsForfait;
        private bool _report;

        public int Id { get => _id; set => _id = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public double Montant { get => _montant; set => _montant = value; }
        public string Libelle { get => _libelle; set => _libelle = value; }
        public bool Validee { get => _validee; set => _validee = value; }
        public bool Refusee { get => _refusee; set => _refusee = value; }
        public bool HorsForfait { get => _horsForfait; set => _horsForfait = value; }
        public bool Report { get => _report; set => _report = value; }

        public LigneDeFrais()
        {
            this.Id = 0;
            this.Date = DateTime.Now;
            this.Montant = 0;
            this.Libelle = "";
            this.Validee = false;
            this.Refusee = false;
            this.HorsForfait = false;
            this.Report = false;
        }


        public LigneDeFrais(int id, DateTime date, double montant, string libelle, bool validee, bool refusee, bool horsForfait, bool report)
        {
            this.Id = id;
            this.Date = date;
            this.Montant = montant;
            this.Libelle = libelle;
            this.Validee = validee;
            this.Refusee = refusee;
            this.HorsForfait = horsForfait;
            this.Report = report;
        }


        public override string ToString()
        {
            return this.Id + " | " + this.Date + " | " + this.Montant + " | " + this.Libelle + " | " + this.Validee + " | " + this.Refusee + " | " + this.HorsForfait + " | " + this.Report;
        }

    }
}
