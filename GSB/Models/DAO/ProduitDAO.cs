using LaboGSB.Models.BO.BOCompteRendu;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.DAO.DAOCompteRendu
{
    class ProduitDAO : DAO<Produit>
    {
        public override void Create(Produit produit)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "INSERT INTO produit (designation,description,quantite,tarif) VALUES (@designation,@description,@quantite,@tarif); SELECT SCOPE_IDENTITY()";
            commande.Parameters.AddWithValue("@designation", produit.Designation);
            commande.Parameters.AddWithValue("@description", produit.Description);
            commande.Parameters.AddWithValue("@quantite", produit.Quantite);
            commande.Parameters.AddWithValue("@tarif", produit.Tarif);

            int newId = Convert.ToInt32(commande.ExecuteScalar());
            produit.Id = newId;
        }

        public override void Delete(Produit produit)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            int id = produit.Id;
            commande.CommandText = "DELETE FROM produit WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            commande.ExecuteNonQuery();
        }

        public override Produit Read(int id)
        {
            Produit produit = null;
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "SELECT * FROM produit WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            SqlDataReader dataReader = commande.ExecuteReader();
            while (dataReader.Read())
            {
                string designation = dataReader.GetString(1);
                string description = dataReader.GetString(2);
                int quantite = dataReader.GetInt32(3);
                double tarif = dataReader.GetDouble(4);

                produit = new Produit(id, designation, description, quantite, tarif);
            }
            dataReader.Close();

            return produit;
        }

        public override void Update(Produit produit)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "UPDATE produit SET designation = @designation, description = @description, quantite = @quantite, tarif = @tarif WHERE id = @id";
            commande.Parameters.AddWithValue("@id", produit.Id);
            commande.Parameters.AddWithValue("@designation", produit.Designation);
            commande.Parameters.AddWithValue("@description", produit.Description);
            commande.Parameters.AddWithValue("@quantite", produit.Quantite);
            commande.Parameters.AddWithValue("@tarif", produit.Tarif);

            commande.ExecuteNonQuery();
        }

        public List<Produit> RetournerTousLesProduits()
        {
            List<Produit> listeProduits = new List<Produit>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT * FROM produit";
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                int id = dataReader.GetInt32(0);
                string designation = dataReader.GetString(1);
                string description = dataReader.GetString(2);
                int quantite = dataReader.GetInt32(3);
                double tarif = dataReader.GetDouble(4);
                Produit produit = new Produit(id, designation, description, quantite, tarif);
                listeProduits.Add(produit);
            }
            dataReader.Close();

            return listeProduits;
        }
    }
}