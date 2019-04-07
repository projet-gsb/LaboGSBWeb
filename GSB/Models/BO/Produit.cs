using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOCompteRendu
{
    public class Produit
    {
        private int _id;
        private string _designation;
        private string _description;
        private int _quantite;
        private double _tarif;

        public int Id { get => _id; set => _id = value; }
        public string Designation { get => _designation; protected set => _designation = value; }
        public string Description { get => _description; protected set => _description = value; }
        public int Quantite { get => _quantite; protected set => _quantite = value; }
        public double Tarif { get => _tarif; protected set => _tarif = value; }

        public Produit()
        {
            this.Id = 0;
            this.Designation = "";
            this.Description = "";
            this.Quantite = 0;
            this.Tarif = 0;
        }

        public Produit(int id, string designation, string description, int quantite, double tarif)
        {
            this.Id = id;
            this.Designation = designation;
            this.Description = description;
            this.Quantite = quantite;
            this.Tarif = tarif;
        }

        public override string ToString()
        {
            return this.Designation;
        }
    }
}