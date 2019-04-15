using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LaboGSB.Models.BO.BOCompteRendu;

namespace LaboGSB.Models.DAO.DAOCompteRendu
{
    class EchantillonDAO : DAO<Echantillon>
    {
        public override void Create(Echantillon echantillon)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "INSERT INTO echantillon (idCompteRendu,idProduit,quantite) VALUES (@idCompteRendu,@idProduit,@quantite); SELECT SCOPE_IDENTITY()";
            commande.Parameters.AddWithValue("@idCompteRendu", echantillon.IdCompteRendu);
            commande.Parameters.AddWithValue("@idProduit", echantillon.ProduitEchantillonne.Id);
            commande.Parameters.AddWithValue("@quantite", echantillon.Quantite);

        }

        public override void Delete(Echantillon echantillon)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "DELETE FROM produit WHERE id = @idCompteRendu AND idProduit = @idProduit";
            commande.Parameters.AddWithValue("@idCompteRendu", echantillon.IdCompteRendu);
            commande.Parameters.AddWithValue("@idProduit", echantillon.ProduitEchantillonne.Id);
            commande.ExecuteNonQuery();
        }

        public override Echantillon Read(int id)
        {
            return null;
        }

        public Echantillon Read(int idCompteRendu, int idProduit)
        {
            Echantillon echantillon = null;
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "SELECT quantite FROM produit WHERE idCompteRendu = @idCompteRendu AND idProduit = @idProduit";
            commande.Parameters.AddWithValue("@idCompteRendu", idCompteRendu);
            commande.Parameters.AddWithValue("@idProduit", echantillon.ProduitEchantillonne.Id);
            SqlDataReader datareader = commande.ExecuteReader();
            if (datareader.Read())
            {
                int quantite = datareader.GetInt32(0);
                datareader.Close();

                ProduitDAO produitDao = new ProduitDAO();
                Produit produit = produitDao.Read(idProduit);

                echantillon = new Echantillon(idCompteRendu, produit, quantite);
            }

            return echantillon;
        }

        public override void Update(Echantillon echantillon)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "UPDATE echantillon  SET quantite = @quantite WHERE idCompteRendu = @idCompteRendu AND idProduit = @idProduit";
            commande.Parameters.AddWithValue("@idCompteRendu", echantillon.IdCompteRendu);
            commande.Parameters.AddWithValue("@idProduit", echantillon.ProduitEchantillonne.Id);
            commande.Parameters.AddWithValue("@quantite", echantillon.Quantite);

            commande.ExecuteNonQuery();
        }

        public List<Echantillon> RetrouverListeEchantillon(int idCompteRendu)
        {
            List<Echantillon> listeEchantillon = new List<Echantillon>();
            List<int[]> listeId = new List<int[]>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT * FROM echantillon WHERE idCompteRendu = @idCompteRendu";
            command.Parameters.AddWithValue("@idCompteRendu", idCompteRendu);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                int[] ids = new int[2];
                ids[0] = dataReader.GetInt32(1);
                ids[1] = dataReader.GetInt32(2);
                listeId.Add(ids);
            }
            dataReader.Close();

            foreach (int[] ids in listeId)
        {
                ProduitDAO produitDao = new ProduitDAO();
                Produit produit = produitDao.Read(ids[0]);

                Echantillon echantillon = new Echantillon(idCompteRendu, produit, ids[1]);
                listeEchantillon.Add(echantillon);
            }

            return listeEchantillon;
        }
    }
}