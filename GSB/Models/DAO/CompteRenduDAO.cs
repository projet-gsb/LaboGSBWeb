using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LaboGSB.Models.BO.BOCompteRendu;
using LaboGSB.Models.BO.Personne;

namespace LaboGSB.Models.DAO.DAOCompteRendu
{
    class CompteRenduDAO : DAO<CompteRendu>
    {
        public override void Create(CompteRendu compteRendu)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "INSERT INTO compterendu (idVisiteurMedical,idContact,idEtablissement,titre,contenu,date) VALUES (@idVisiteurMedical,@idContact,@idEtablissement,@titre,@contenu,@date); SELECT SCOPE_IDENTITY()";
            commande.Parameters.AddWithValue("@idVisiteurMedical", compteRendu.VisiteurMedicalConcerne.Id);
            commande.Parameters.AddWithValue("@idContact", compteRendu.ContactConcerne.Id);
            commande.Parameters.AddWithValue("@idEtablissement", compteRendu.EtablissementConcerne.Id);
            commande.Parameters.AddWithValue("@titre", compteRendu.Titre);
            commande.Parameters.AddWithValue("@contenu", compteRendu.Contenu);
            commande.Parameters.AddWithValue("@date", compteRendu.Date);

            int newId = Convert.ToInt32(commande.ExecuteScalar());
            compteRendu.Id = newId;

            EchantillonDAO echantillonDao = new EchantillonDAO();
            foreach (Echantillon echantillon in compteRendu.ListeEchantillon)
            {
                echantillonDao.Create(echantillon);
            }
        }

       

        public override void Delete(CompteRendu compteRendu)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            int id = compteRendu.Id;
            commande.CommandText = "DELETE FROM compterendu WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            commande.ExecuteNonQuery();

        }

    
        public override CompteRendu Read(int id)
        {
            CompteRendu compteRendu = null;
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "SELECT * FROM compterendu WHERE id = @id";
            commande.Parameters.AddWithValue("@id", id);
            SqlDataReader dataReader = commande.ExecuteReader();
            if (dataReader.Read())
            {
                int idVisiteurMedical = dataReader.GetInt32(1); ;
                int idContact = dataReader.GetInt32(2);
                int idEtablissement = dataReader.GetInt32(3);
                string titre = dataReader.GetString(4);
                string contenu = dataReader.GetString(5);
                DateTime date = dataReader.GetDateTime(6);
                dataReader.Close();

                EchantillonDAO echantillonDao = new EchantillonDAO();
                List<Echantillon> listeEchantillon = echantillonDao.RetrouverListeEchantillon(id);

                VisiteurMedicalDAO visiteurMedicalDao = new VisiteurMedicalDAO();
                VisiteurMedical visiteurMedical = visiteurMedicalDao.Read(idVisiteurMedical);

                ContactDAO contactDao = new ContactDAO();
                Contact contact = contactDao.Read(idContact);

                EtablissementDAO etablissementDao = new EtablissementDAO();
                Etablissement etablissement = etablissementDao.Read(idEtablissement);

                compteRendu = new CompteRendu(id, visiteurMedical, contact, etablissement, titre, contenu, date, listeEchantillon);
            }

            return compteRendu;

        }

        public override void Update(CompteRendu compteRendu)
        {
            SqlCommand commande = Connexion.GetInstance().CreateCommand();
            commande.CommandText = "UPDATE compterendu  SET idVisiteurMedical = @idVisiteurMedical, idContact = @idContact, idEtablissement = @idEtablissement, titre = @titre, contenu = @contenu, date = @date WHERE id = @id";
            commande.Parameters.AddWithValue("@id", compteRendu.Id);
            commande.Parameters.AddWithValue("@idVisiteurMedical", compteRendu.VisiteurMedicalConcerne.Id);
            commande.Parameters.AddWithValue("@idContact", compteRendu.ContactConcerne.Id);
            commande.Parameters.AddWithValue("@idEtablissement", compteRendu.EtablissementConcerne.Id);
            commande.Parameters.AddWithValue("@titre", compteRendu.Titre);
            commande.Parameters.AddWithValue("@contenu", compteRendu.Contenu);
            commande.Parameters.AddWithValue("@date", compteRendu.Date);
            commande.ExecuteNonQuery();

            commande.CommandText = "DELETE FROM echantillon WHERE idCompteRendu = @id";
            commande.ExecuteNonQuery();

            EchantillonDAO echantillonDao = new EchantillonDAO();
            foreach (Echantillon echantillon in compteRendu.ListeEchantillon)
            {
                echantillonDao.Create(echantillon);
            }
        }

        public List<CompteRendu> RetournerTousLesCompteRendus()
        {
            List<CompteRendu> listeCompteRendus = new List<CompteRendu>();
            List<int> listeId = new List<int>();
            SqlCommand command = Connexion.GetInstance().CreateCommand();
            command.CommandText = "SELECT id FROM compterendu";
            // Lecture des résultats
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                listeId.Add(dataReader.GetInt32(0));
            }
            dataReader.Close();

            foreach (int id in listeId)
            {
                CompteRendu cr = this.Read(id);
                listeCompteRendus.Add(cr);
            }

            return listeCompteRendus;
        }

    }
}