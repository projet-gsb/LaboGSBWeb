using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOCompteRendu
{
    public class Etablissement
    {
        private int _id;
        private string _nom;
        private string _adresse;
        private string _numeroTelephone;
        private string _mel;
        private TypeEtablissement _type;

        public int Id { get => _id; set => _id = value; }
        public string Nom { get => _nom; set => _nom = value; }
        public string Adresse { get => _adresse; set => _adresse = value; }
        public string NumeroTelephone { get => _numeroTelephone; set => _numeroTelephone = value; }
        public string Mel { get => _mel; set => _mel = value; }
        public TypeEtablissement Type { get => _type; set => _type = value; }

        public Etablissement()
        {
            this.Id = 0;
            this.Nom = "";
            this.Adresse = "";
            this.NumeroTelephone = "";
            this.Mel = "";
            this.Type = new TypeEtablissement();
        }

        public Etablissement(int id, string nom, string adresse, string numeroTelephone, string mel, TypeEtablissement type)
        {
            this.Id = id;
            this.Nom = nom;
            this.Adresse = adresse;
            this.NumeroTelephone = numeroTelephone;
            this.Mel = mel;
            this.Type = type;
        }

        public override string ToString()
        {
            return this.Nom;
        }
    }

}
