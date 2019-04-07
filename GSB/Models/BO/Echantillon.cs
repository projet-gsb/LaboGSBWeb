using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOCompteRendu
{
    public class Echantillon
    {
        private int _idCompteRendu;
        private Produit _produit;
        private int _quantite;

        public int IdCompteRendu { get => _idCompteRendu; set => _idCompteRendu = value; }
        public Produit ProduitEchantillonne { get => _produit; set => _produit = value; }
        public int Quantite { get => _quantite; set => _quantite = value; }

        public Echantillon(int idCompteRendu, Produit produit, int quantite)
        {
            this.IdCompteRendu = idCompteRendu;
            this.ProduitEchantillonne = produit;
            this.Quantite = quantite;
        }

        public override string ToString()
        {
            return this.ProduitEchantillonne.Designation;
        }
    }
}