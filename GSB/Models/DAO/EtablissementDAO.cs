using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LaboGSB.Models.BO.BOCompteRendu;

namespace LaboGSB.Models.DAO.DAOCompteRendu
{
    class EtablissementDAO : DAO<Etablissement>
    {
        public override void Create(Etablissement etablissement)
        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "INSERT INTO etablissement (nom,adresse,numeroTelephone,mel,idType) VALUES (@nom,@adresse,@numeroTelephone,@mel,@idType); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@nom", etablissement.Nom);
            command.Parameters.AddWithValue("@adresse", etablissement.Adresse);
            command.Parameters.AddWithValue("@numeroTelephone", etablissement.NumeroTelephone);
            command.Parameters.AddWithValue("@mel", etablissement.Mel);
            command.Parameters.AddWithValue("@idType", etablissement.Type.Id);
            // Exécution de la requête
            // command.ExecuteNonQuery();
            // pour récupérer la clé générée

            int newId = Convert.ToInt32(command.ExecuteScalar());
            etablissement.Id = newId;
        }

        public override Etablissement Read(int id)
        {
            Etablissement etablissement = null;
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT * FROM etablissement WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                int idEtablissement = id;
                string nom = dataReader.GetString(1);
                string adresse = dataReader.GetString(2);
                string numeroTelephone = dataReader.GetString(3);
                string mel = dataReader.GetString(4);
                int idType = dataReader.GetInt32(5);
                dataReader.Close();

                TypeEtablissementDAO typeEtabDAO = new TypeEtablissementDAO();
                TypeEtablissement typeEtab = typeEtabDAO.Read(idType);

                etablissement = new Etablissement(idEtablissement, nom, adresse, numeroTelephone, mel, typeEtab);
            }
            dataReader.Close();

            return etablissement;
        }

        public override void Update(Etablissement etablissement)

        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "UPDATE etablissement SET nom = @nom, adresse = @adresse, numeroTelephone = @numeroTelephone, mel = @mel, idType = @idType WHERE id = @id";
            command.Parameters.AddWithValue("@id", etablissement.Id);
            command.Parameters.AddWithValue("@nom", etablissement.Nom);
            command.Parameters.AddWithValue("@adresse", etablissement.Adresse);
            command.Parameters.AddWithValue("@numeroTelephone", etablissement.NumeroTelephone);
            command.Parameters.AddWithValue("@mel", etablissement.Mel);
            command.Parameters.AddWithValue("@idType", etablissement.Type.Id);
            // Exécution de la requête
            command.ExecuteNonQuery();
        }

        public override void Delete(Etablissement etablissement)

        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();

            command.CommandText = "DELETE FROM etablissement WHERE id = @id";
            command.Parameters.AddWithValue("@id", etablissement.Id);
            command.ExecuteNonQuery();

        }

        public List<Etablissement> RetournerTousLesEtablissements()
        {
            List<Etablissement> listeEtablissements = new List<Etablissement>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT id FROM etablissement";
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            foreach (int id in listeId)
            {
                Etablissement etab = this.Read(id);
                listeEtablissements.Add(etab);
}

            return listeEtablissements;
        }

    }
}