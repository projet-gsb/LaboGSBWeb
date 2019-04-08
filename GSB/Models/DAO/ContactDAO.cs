using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using LaboGSB.Models.BO.BOCompteRendu;
using LaboGSB.Models.BO.Personne;


namespace LaboGSB.Models.DAO.DAOCompteRendu
{
    class ContactDAO : DAO<Contact>
    {

        public override void Create(Contact contact)
        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "INSERT INTO personne (nom,prenom,mel,numeroTelephone) VALUES (@nom,@prenom,@mel,@numeroTelephone); SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@nom", contact.Nom);
            command.Parameters.AddWithValue("@prenom", contact.Prenom);
            command.Parameters.AddWithValue("@mel", contact.Mel);
            command.Parameters.AddWithValue("@numeroTelephone", contact.NumeroTelephone);
            // Exécution de la requête
            //command.ExecuteNonQuery();
            // pour récupérer la clé générée
            int newId = Convert.ToInt32(command.ExecuteScalar());
            contact.Id = newId;
            command.CommandText = "INSERT INTO contact (idPersonne,poste,commentaire) VALUES (@idPersonne,@poste,@commentaire)";
            command.Parameters.AddWithValue("@idPersonne", contact.Id);
            command.Parameters.AddWithValue("@poste", contact.Poste);
            //command.Parameters.AddWithValue("@employeur", contact.Employeur);
            command.Parameters.AddWithValue("@commentaire", contact.Commentaire);
            command.ExecuteNonQuery();

        }

        public override Contact Read(int id)
        {
            Contact contact = null;
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT * FROM personne, contact WHERE id = @id AND id = idPersonne";
            command.Parameters.AddWithValue("@id", id);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                int idPersonne = id;
                string nom = dataReader.GetString(1);
                string prenom = dataReader.GetString(2);
                string mel = dataReader.GetString(3);
                string numeroTelephone = dataReader.GetString(4);
                string poste = dataReader.GetString(6);
                string commentaire = dataReader.GetString(7);
                dataReader.Close();

                List<BO.BOCompteRendu.Etablissement> employeur = RetrouverListeEmployeur(id);
                contact = new Contact(poste, commentaire, employeur, idPersonne, nom, prenom, mel, numeroTelephone);
            }

            return contact;
        }

        public override void Update(Contact contact)

        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            // Définition de la requête
            command.CommandText = "UPDATE personne SET nom = @nom, prenom = @prenom, mel = @mel, numeroTelephone = @numeroTelephone WHERE id = @id";
            command.Parameters.AddWithValue("@id", contact.Id);
            command.Parameters.AddWithValue("@nom", contact.Nom);
            command.Parameters.AddWithValue("@prenom", contact.Prenom);
            command.Parameters.AddWithValue("@mel", contact.Mel);
            command.Parameters.AddWithValue("@numeroTelephone", contact.NumeroTelephone);
            // Exécution de la requête
            command.ExecuteNonQuery();

            command.CommandText = "UPDATE contact SET poste = @poste, commentaire = @commentaire WHERE idPersonne = @id";
            command.Parameters.AddWithValue("@poste", contact.Poste);
            //command.Parameters.AddWithValue("@employeur", contact.Employeur);
            command.Parameters.AddWithValue("@commentaire", contact.Commentaire);
            // Exécution de la requête
            command.ExecuteNonQuery();
        }

        public override void Delete(Contact contact)
        {
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            int idContact = contact.Id;

            command.CommandText = "DELETE FROM contact WHERE idPersonne = @id";
            command.Parameters.AddWithValue("@id", idContact);
            command.ExecuteNonQuery();

            command.CommandText = "DELETE FROM personne WHERE id = @id";
            command.ExecuteNonQuery();
        }

        public List<Etablissement> RetrouverListeEmployeur(int idContact)
        {
            List<Etablissement> listeEmployeur = new List<Etablissement>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT idEtablissement FROM travailpour WHERE idContact = @idContact";
            command.Parameters.AddWithValue("@idContact", idContact);
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            foreach (int id in listeId)
            {
                EtablissementDAO etablissementDAO = new EtablissementDAO();
                Etablissement etablissement = etablissementDAO.Read(id);

                listeEmployeur.Add(etablissement);
            }
            return listeEmployeur;
        }

        public List<Contact> RetournerTousLesContacts()
        {
            List<Contact> listeContacts = new List<Contact>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT idPersonne FROM contact";
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            foreach (int id in listeId)
            {
                Contact contact = this.Read(id);
                listeContacts.Add(contact);
            }

            return listeContacts;
        }

    }
}