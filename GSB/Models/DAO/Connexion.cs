using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace LaboGSB.Models.DAO
{
   
    class Connexion
    {
        private static SqlConnection LaConnexion { get; set; } = null;
        public static SqlConnection GetInstance()
        {
            // Préparation de la connexion à la base de données
            if (LaConnexion == null)
            {   //ligne à decommenter en fonction de chaque poste
                string connectionString = "Server=localhost\\SQLEXPRESS;Database=gsb-gestion;User Id=Admin;Password= mdp; ";       

                LaConnexion = new SqlConnection(connectionString);
                try
                {
                    // Connexion à la base de données
                    LaConnexion.Open();
                    Console.WriteLine("connecté");
                }
                catch (Exception ex)
                { Console.WriteLine("PROBLEME " + ex.Message); }
            }
            return LaConnexion;
        }
        private Connexion() { }

    }
}