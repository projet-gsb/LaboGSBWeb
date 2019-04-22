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

            int newId = Convert.ToInt32(commande.ExecuteScalar());
            fichedefrais.Id = newId;

            LigneDeFraisDAO ligneDeFraisDao = new LigneDeFraisDAO();

            foreach (LigneDeFrais lf in fichedefrais.ListeDeLignesDeFrais)
            {
                ligneDeFraisDao.Create(lf);
                commande.CommandText = "INSERT INTO listefrais (idFicheDeFrais,idLigneDeFrais) VALUES (@idFicheDeFrais,@idLigneDeFrais)";
                commande.Parameters.AddWithValue("@idFicheDeFrais", fichedefrais.Id);
                commande.Parameters.AddWithValue("@idLigneDeFrais", lf.Id);
            }
        }

        public override void Delete(FicheDeFrais fichedefrais)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            int id = fichedefrais.Id;
            commande.Parameters.AddWithValue("@id", id);

            commande.CommandText = "DELETE FROM listefrais WHERE idFicheDeFrais = @id";
            commande.ExecuteNonQuery();

            commande.CommandText = "DELETE FROM fichedefrais WHERE id = @id";
            commande.ExecuteNonQuery();

            LigneDeFraisDAO ligneDeFraisDao = new LigneDeFraisDAO();

            foreach (LigneDeFrais lf in fichedefrais.ListeDeLignesDeFrais)
            {
                ligneDeFraisDao.Delete(lf);
            }
        }

        public override FicheDeFrais Read(int id)
        {
            FicheDeFrais fichedefrais = null;
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "SELECT * FROM fichedefrais WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            SqlDataReader datareader = commande.ExecuteReader();
            if (datareader.Read())
            {
                int idVisiteurMedical = datareader.GetInt32(1); ;
                DateTime dateCreation = datareader.GetDateTime(2);
                DateTime dateTraitement = datareader.GetDateTime(3);
                bool miseEnPaiement = datareader.GetBoolean(4);
                datareader.Close();

                List<LigneDeFrais> listeLignesDeFrais = this.RetrouverLignesDeFrais(id);

                fichedefrais = new FicheDeFrais(id, idVisiteurMedical, dateCreation, dateTraitement, miseEnPaiement, listeLignesDeFrais);
            }
            datareader.Close();

            return fichedefrais;

        }

        public override void Update(FicheDeFrais fichedefrais)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "UPDATE fichedefrais  SET idVisiteurMedical = @idVisiteurMedical, dateCreation = @dateCreation, dateTraitement = @dateTraitement, miseEnPaiement = @miseEnPaiement WHERE id = @id";

            commande.Parameters.AddWithValue("@id", fichedefrais.Id);
            commande.Parameters.AddWithValue("@idVisiteurMedical", fichedefrais.IdVisiteurMedical);
            commande.Parameters.AddWithValue("@dateCreation", fichedefrais.DateCreation);
            commande.Parameters.AddWithValue("@dateTraitement", fichedefrais.DateTraitement);
            commande.Parameters.AddWithValue("@miseEnPaiement", fichedefrais.MiseEnPaiement);
            commande.ExecuteNonQuery();

            commande.CommandText = "DELETE FROM listefrais WHERE idFicheDeFrais = @id";
            commande.ExecuteNonQuery();

            LigneDeFraisDAO ligneDeFraisDao = new LigneDeFraisDAO();

            foreach (LigneDeFrais lf in fichedefrais.ListeDeLignesDeFrais)
            {
                ligneDeFraisDao.Update(lf);
                commande.Parameters.Clear();
                commande.CommandText = "INSERT INTO listefrais (idFicheDeFrais,idLigneDeFrais) VALUES (@idFicheDeFrais,@idLigneDeFrais)";
                commande.Parameters.AddWithValue("@idFicheDeFrais", fichedefrais.Id);
                commande.Parameters.AddWithValue("@idLigneDeFrais", lf.Id);
                commande.ExecuteNonQuery();
            }

        }

        public List<LigneDeFrais> RetrouverLignesDeFrais(int idFicheDeFrais)
        {
            List<LigneDeFrais> listeLignesDeFrais = new List<LigneDeFrais>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT idLigneDeFrais FROM listefrais WHERE idFicheDeFrais = @idFicheDeFrais";
            command.Parameters.AddWithValue("@idFicheDeFrais", idFicheDeFrais);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            LigneDeFraisDAO ligneDeFraisDao = new LigneDeFraisDAO();

            foreach (int id in listeId)
            {
                LigneDeFrais lf = ligneDeFraisDao.Read(id);
                listeLignesDeFrais.Add(lf);
            }

            return listeLignesDeFrais;
        }

        public List<FicheDeFrais> RetournerToutesLesFichesDeFrais(int IdVisiteurMedical)
        {
            List<FicheDeFrais> listeFichesDeFrais = new List<FicheDeFrais>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT id FROM fichedefrais WHERE idVisiteurMedical = @idVisiteurMedical";
            command.Parameters.AddWithValue("@idVisiteurMedical", IdVisiteurMedical);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            foreach (int id in listeId)
            {
                FicheDeFrais ff = this.Read(id);
                listeFichesDeFrais.Add(ff);
            }

            return listeFichesDeFrais;
        }

    }
}