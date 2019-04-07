using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.BO.BOCompteRendu
{
    public class TypeEtablissement
    {
        private int _id;
        private string _nom;


        public int Id { get => _id; set => _id = value; }
        public string Nom { get => _nom; set => _nom = value; }


        public TypeEtablissement()
        {
            this.Id = 0;
            this.Nom = "";
        }

        public TypeEtablissement(int id, string nom)
        {
            this.Id = id;
            this.Nom = nom;
        }

        public override string ToString()
        {
            return this.Nom;
        }
    }

}
