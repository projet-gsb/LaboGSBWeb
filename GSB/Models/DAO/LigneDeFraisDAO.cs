using LaboGSB.Models.BO.BOGestionFrais;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LaboGSB.Models.DAO.DAOGestionFrais
{
    class LigneDeFraisDAO : DAO<LigneDeFrais>
    {
        public override void Create(LigneDeFrais lignedefrais)
        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "INSERT INTO lignedefrais (date, montant, libelle, validee, refusee, horsForfait, report) VALUES(@date, @montant, @libelle, @validee, @refusee, @horsForfait, @report); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@date", lignedefrais.Date);
            command.Parameters.AddWithValue("@montant", lignedefrais.Montant);
            command.Parameters.AddWithValue("@libelle", lignedefrais.Libelle);
            command.Parameters.AddWithValue("@validee", lignedefrais.Validee);
            command.Parameters.AddWithValue("@refusee", lignedefrais.Refusee);
            command.Parameters.AddWithValue("@horsForfait", lignedefrais.HorsForfait);
            command.Parameters.AddWithValue("@report", lignedefrais.Report);

            // Exécution de la requête
            // command.ExecuteNonQuery();
            // pour récupérer la clé générée
            // Int32 newId = (Int32)command.ExecuteScalar();
            int newId = Convert.ToInt32(command.ExecuteScalar());
            lignedefrais.Id = newId;
        }

        public override void Delete(LigneDeFrais lignedefrais)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            int id = lignedefrais.Id;
            commande.CommandText = "DELETE FROM lignedefrais WHERE id = @id";
            commande.Parameters.AddWithValue("@id", lignedefrais.Id);
            commande.ExecuteNonQuery();

        }

        // TODO : vérifier le GetBytes(boolean) pour le booleen

        public override LigneDeFrais Read(int id)
        {
            LigneDeFrais lignedefrais = null;
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "SELECT * FROM lignedefrais WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            SqlDataReader datareader = commande.ExecuteReader();
            while (datareader.Read())
            {
                DateTime date = datareader.GetDateTime(1);
                double montant = datareader.GetInt32(2);
                string libelle = datareader.GetString(3);
                bool validee = datareader.GetBoolean(4);
                bool refusee = datareader.GetBoolean(5);
                bool horsForfait = datareader.GetBoolean(6);
                bool report = datareader.GetBoolean(7);
                lignedefrais = new LigneDeFrais(id, date, montant, libelle, validee, refusee, horsForfait, report);
            }
            datareader.Close();

            return lignedefrais;
        }

        public override void Update(LigneDeFrais lignedefrais)
        {
            if (lignedefrais.Id == 0) {
                this.Create(lignedefrais);
            }
            else
            {
                SqlCommand commande = Connexion.GetInstance().CreateCommand();
                commande.CommandText = "UPDATE lignedefrais SET date = @date, montant = @montant,libelle = @libelle, validee = @validee, refusee = @refusee, horsForfait = @horsForfait, report = @report WHERE id = @id";
                commande.Parameters.AddWithValue("@id", lignedefrais.Id);
                commande.Parameters.AddWithValue("@date", lignedefrais.Date);
                commande.Parameters.AddWithValue("@montant", lignedefrais.Montant);
                commande.Parameters.AddWithValue("@libelle", lignedefrais.Libelle);
                commande.Parameters.AddWithValue("@validee", lignedefrais.Validee);
                commande.Parameters.AddWithValue("@refusee", lignedefrais.Refusee);
                commande.Parameters.AddWithValue("@horsForfait", lignedefrais.HorsForfait);
                commande.Parameters.AddWithValue("@report", lignedefrais.Report);
                commande.ExecuteNonQuery();
            }
        }

        public List<LigneDeFrais> RetournerToutesLesLigneDeFrais()
        {
            List<LigneDeFrais> listeLigneDeFrais = new List<LigneDeFrais>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT id FROM visiteurmedical";
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            foreach (int id in listeId)
            {
                LigneDeFrais cr = this.Read(id);
                listeLigneDeFrais.Add(cr);
            }

            return listeLigneDeFrais;
        }
    }
}