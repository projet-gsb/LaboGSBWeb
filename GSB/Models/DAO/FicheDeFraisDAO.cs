using LaboGSB.Models.BO.BOGestionFrais;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.DAO.DAOGestionFrais
{
    class FicheDeFraisDAO : DAO<FicheDeFrais>
    {
        public override void Create(FicheDeFrais fichedefrais)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "INSERT INTO fichedefrais (idVisiteurMedical,dateCreation,dateTraitement,miseEnPaiement) VALUES (@idVisiteurMedical,@dateCreation,@dateTraitement,@miseEnPaiement); SELECT SCOPE_IDENTITY()";
            commande.Parameters.AddWithValue("@idVisiteurMedical", fichedefrais.IdVisiteurMedical);
            commande.Parameters.AddWithValue("@dateCreation", fichedefrais.DateCreation);
            commande.Parameters.AddWithValue("@dateTraitement", fichedefrais.DateTraitement);
            commande.Parameters.AddWithValue("@miseEnPaiement", fichedefrais.MiseEnPaiement);

            int newId = (int)commande.ExecuteScalar();
            fichedefrais.Id = newId;
        }

        public override void Delete(FicheDeFrais fichedefrais)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            int id = fichedefrais.Id;
            commande.CommandText = "DELETE * FROM fichedefrais WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            commande.ExecuteNonQuery();

        }

        public override FicheDeFrais Read(int id)
        {
            FicheDeFrais fichedefrais = null;
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "SELECT * FROM fichedefrais WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            SqlDataReader datareader = commande.ExecuteReader();
            while (datareader.Read())
            {
                int Id = id;
                int idVisiteurMedical = datareader.GetInt32(1); ;
                DateTime dateCreation = datareader.GetDateTime(6);
                DateTime dateTraitement = datareader.GetDateTime(6);
                bool miseEnPaiement = datareader.GetBoolean(1);
                fichedefrais = new FicheDeFrais(id, idVisiteurMedical, dateCreation, dateTraitement, miseEnPaiement);
            }
            datareader.Close();

            return fichedefrais;

        }

        public override void Update(FicheDeFrais fichedefrais)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "UPDATE compterendu  SET idVisiteurMedical = @idVisiteurMedical, dateCreation = @dateCreation, dateTraitement = @dateTraitement, miseEnPaiement = @miseEnPaiement WHERE id = @id";

            commande.Parameters.AddWithValue("@id", fichedefrais.Id);
            commande.Parameters.AddWithValue("@idVisiteurMedical", fichedefrais.IdVisiteurMedical);
            commande.Parameters.AddWithValue("@dateCreation", fichedefrais.DateCreation);
            commande.Parameters.AddWithValue("@dateTraitement", fichedefrais.DateTraitement);
            commande.Parameters.AddWithValue("@miseEnPaiement", fichedefrais.MiseEnPaiement);
            commande.ExecuteNonQuery();
        }
    }
}