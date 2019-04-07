using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LaboGSB.Models.BO.BOCompteRendu;

namespace LaboGSB.Models.DAO.DAOCompteRendu
{
    class TypeEtablissementDAO : DAO<TypeEtablissement>
    {
        public override void Create(TypeEtablissement typeEtablissement)
        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "INSERT INTO typeetablissement (nom) VALUES (@nom); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@nom", typeEtablissement.Nom);
            // Exécution de la requête
            // command.ExecuteNonQuery();
            // pour récupérer la clé générée

            int newId = Convert.ToInt32(command.ExecuteScalar());

            typeEtablissement.Id = newId;
        }

        public override TypeEtablissement Read(int id)
        {
            TypeEtablissement typeEtablissement = null;
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT * FROM typeetablissement WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                int idTypeEtablissement = id;
                string nom = dataReader.GetString(1);
                typeEtablissement = new TypeEtablissement(idTypeEtablissement, nom);
            }
            dataReader.Close();

            return typeEtablissement;
        }

        public override void Update(TypeEtablissement typeEtablissement)

        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "UPDATE typeetablissement SET nom = @nom WHERE id = @id";
            command.Parameters.AddWithValue("@id", typeEtablissement.Id);
            command.Parameters.AddWithValue("@nom", typeEtablissement.Nom);
            // Exécution de la requête
            command.ExecuteNonQuery();
        }

        public override void Delete(TypeEtablissement typeEtablissement)

        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();

            command.CommandText = "DELETE FROM typeetablissement WHERE id = @id";
            command.Parameters.AddWithValue("@id", typeEtablissement.Id);
            command.ExecuteNonQuery();

        }

        public List<TypeEtablissement> RetournerTousLesTypesEtablissement()
        {
            List<TypeEtablissement> listeTypesEtablissements = new List<TypeEtablissement>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT * FROM typeetablissement";
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                int idTypeEtablissement = dataReader.GetInt32(0);
                string nom = dataReader.GetString(1);
                TypeEtablissement typeEtablissement = new TypeEtablissement(idTypeEtablissement, nom);
                listeTypesEtablissements.Add(typeEtablissement);
            }
            dataReader.Close();

            return listeTypesEtablissements;
        }

    }
}