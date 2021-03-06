﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LaboGSB.Models.DAO.DAOCompteRendu;
using LaboGSB.Models.DAO.DAOGestionFrais;
using LaboGSB.Models.BO.Personne;
using LaboGSB.Models.BO.BOCompteRendu;
using LaboGSB.Models.BO.BOGestionFrais;
using LaboGSB.Models.DAO;
using System.Data.SqlClient;

namespace GSB.Controllers
{
    public class HomeController : Controller
    {

        // GET: Home
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult RechercheVide()
        {
            return View();
        }

        public string MettreEnMinuscule(string text)
        {
            text = text.ToLower();

            return text;
        }

        public ActionResult Index()                 //page de demarrage qui determine si session ouverte ou non
        {
            ActionResult retour = View();

            if (Session["UserID"] != null)
            {
                retour = View();                      //Session ouverte
            }
            else
            {
                retour = RedirectToAction("Login");   //Session fermée redirection vers page Login
            }
            return retour;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(VisiteurMedical objUser)                      //le formulaire de connexion envoie directement l'objet visiteurMedical
        {
            VisiteurMedicalDAO visiteurMedicalDao = new VisiteurMedicalDAO();
            List<VisiteurMedical> listeVisiteursMedicaux = visiteurMedicalDao.RetournerTousLesVisiteursMedicaux(); // creation de la liste de tout les visiteursmedicaux

            if (ModelState.IsValid)
            {
                var obj = listeVisiteursMedicaux.Where(a => a.Mel.Equals(objUser.Mel) && a.MotDePasse.Equals(objUser.MotDePasse)).FirstOrDefault();
                //requete qui utilise les champs(mel, mdp) entrés par l'utilisateur dans Login
                if (obj != null)
                {
                    Session["UserID"] = obj.Id.ToString();      //recupération de l'id utilisateur
                    Session["UserName"] = obj.Nom.ToString();   //recuperation du nom utilisateur
                    return RedirectToAction("Index");           //on peu accèder a la page index
                }
            }
            return View(objUser);
        }

        public ActionResult AccueilCR(string id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)                  // pour chaque page on verifie qu'une session existe
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                CompteRenduDAO crDao = new CompteRenduDAO();
                EtablissementDAO etabDao = new EtablissementDAO();
                ContactDAO contactDao = new ContactDAO();
                ProduitDAO produitDao = new ProduitDAO();
                List<CompteRendu> listeCompteRendus = crDao.RetournerTousLesCompteRendus();
                List<CompteRendu> rechercheCompteRendu = new List<CompteRendu>();
                List<Etablissement> listeEtablissements = etabDao.RetournerTousLesEtablissements();
                List<Etablissement> rechercheEtablissement = new List<Etablissement>();
                List<Contact> listeContacts = contactDao.RetournerTousLesContacts();
                List<Contact> rechercheContact = new List<Contact>();
                List<Produit> listeProduits = produitDao.RetournerTousLesProduits();
                List<Produit> rechercheProduit = new List<Produit>();


                ViewBag.listeCompteRendus = listeCompteRendus;
                ViewBag.listeEtablissements = listeEtablissements;
                ViewBag.listeContacts = listeContacts;
                ViewBag.listeProduits = listeProduits;
                ViewBag.afficherListeCompteRendus = false;
                ViewBag.afficherListeEtab = false;
                ViewBag.afficherListeContacts = false;
                ViewBag.afficherListeProduits = false;

                if (!String.IsNullOrWhiteSpace(id))
                {
                    if (id == "Listedesetablissements")
                    {
                        ViewBag.afficherListeEtab = true;
                    }
                    else if (id == "Listedescontacts")
                    {
                        ViewBag.afficherListeContacts = true;
                    }
                    else if (id == "Listedesproduits")
                    {
                        ViewBag.afficherListeProduits = true;
                    }
                    else
                    {
                        ViewBag.afficherListeCompteRendus = true;
                    }
                }

                //Fonction de recherche
                if (Request.HttpMethod == "POST")                                               // Evenement clic sur le bouton de recherche         
                {
                    if (Request.Form["rechercheEtab"] != null)                                  // on test si la recherche n'est pas vide 
                    {
                        string recherche = MettreEnMinuscule(Request.Form["rechercheEtab"]);    // on met la recherche en lettres minuscules (pour correspondre avec la BD)
                        ViewBag.afficherListeEtab = true;

                        foreach (Etablissement etab in listeEtablissements)                     // pour chaque etablissement de la liste de d'etablissement 
                        {
                            if (MettreEnMinuscule(etab.Nom).Contains(recherche) || MettreEnMinuscule(etab.Adresse).Contains(recherche)) // on compare la recherche avec la BD
                            {
                                rechercheEtablissement.Add(etab);                               // pour chaque correspondance on ajoute dans la liste de recherche 
                            }
                        }
                        ViewBag.listeEtablissements = rechercheEtablissement;           // on change l'ancienne liste affichée sur la View 
                    }

                    else if (Request.Form["rechercheContact"] != null)
                    {
                        string recherche = MettreEnMinuscule(Request.Form["rechercheContact"]);
                        ViewBag.afficherListeContacts = true;

                        foreach (Contact contact in listeContacts)
                        {
                            if (MettreEnMinuscule(contact.Nom).Contains(recherche) || MettreEnMinuscule(contact.Prenom).Contains(recherche))
                            {
                                rechercheContact.Add(contact);
                            }
                        }
                        ViewBag.listeContacts = rechercheContact;
                    }

                    else if (Request.Form["rechercheProduit"] != null)
                    {
                        string recherche = MettreEnMinuscule(Request.Form["rechercheProduit"]);
                        ViewBag.afficherListeProduits = true;

                        foreach (Produit produit in listeProduits)
                        {
                            if (MettreEnMinuscule(produit.Designation).Contains(recherche) || MettreEnMinuscule(produit.Description).Contains(recherche))
                            {
                                rechercheProduit.Add(produit);
                            }
                        }
                        ViewBag.listeProduits = rechercheProduit;
                    }

                    else if (Request.Form["rechercheCompteRendu"] != null)
                    {
                        string recherche = MettreEnMinuscule(Request.Form["rechercheCompteRendu"]);
                        ViewBag.afficherListeCompteRendus = true;

                        foreach (CompteRendu compteRendu in listeCompteRendus)
                        {
                            if (MettreEnMinuscule(compteRendu.Titre).Contains(recherche) || MettreEnMinuscule(compteRendu.Contenu).Contains(recherche))
                            {
                                rechercheCompteRendu.Add(compteRendu);
                            }
                        }
                        ViewBag.listeCompteRendus = rechercheCompteRendu;
                    }
                }
            }
            return retour;
        }

        public ActionResult AccueilFrais(string id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)                  // pour chaque page on verifie qu'une session existe
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                FicheDeFraisDAO ffDao = new FicheDeFraisDAO();
                int idVisiteurMedical = Convert.ToInt32(Session["UserID"]);
                List<FicheDeFrais> listeFichesDeFrais = ffDao.RetournerToutesLesFichesDeFrais(idVisiteurMedical);
                //List<FicheDeFrais> rechercheFichesDeFrais = new List<FicheDeFrais>();

                ViewBag.listeFichesDeFrais = listeFichesDeFrais;


                ////Fonction de recherche
                //if (Request.HttpMethod == "POST" && Request.Form["rechercher"] != null)
                //{
                //    string recherche = MettreEnMinuscule(Request.Form["rechercher"]);

                //    foreach (FicheDeFrais ff in listeFichesDeFrais)
                //    {
                //        if (MettreEnMinuscule(ff.????).Contains(recherche) || MettreEnMinuscule(ff.????).Contains(recherche))
                //        {
                //            rechercheFichesDeFrais.Add(ff);
                //        }
                //    }
                //    ViewBag.listeFichesDeFrais = rechercheFichesDeFrais;
                //}
            }
            return retour;
        }


        public ActionResult FormCompteRendu(String id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                CompteRenduDAO crDao = new CompteRenduDAO();
                VisiteurMedicalDAO visiteurMedicalDao = new VisiteurMedicalDAO();
                ContactDAO contactDao = new ContactDAO();
                EtablissementDAO etabDao = new EtablissementDAO();
                ProduitDAO produitDao = new ProduitDAO();

                CompteRendu cr = new CompteRendu();
                ViewBag.cr = cr;

                List<VisiteurMedical> listeVisiteursMedicaux = visiteurMedicalDao.RetournerTousLesVisiteursMedicaux();
                ViewBag.listeVisiteursMedicaux = listeVisiteursMedicaux;

                List<Contact> listeContacts = contactDao.RetournerTousLesContacts();
                ViewBag.listeContacts = listeContacts;

                List<Etablissement> listeEtablissements = etabDao.RetournerTousLesEtablissements();
                ViewBag.listeEtablissements = listeEtablissements;

                List<Produit> listeProduits = produitDao.RetournerTousLesProduits();
                ViewBag.listeProduits = listeProduits;

                if (Request.HttpMethod == "POST")
                {
                    int idCompteRendu = Int32.Parse(Request.Form["idCompteRendu"]);
                    VisiteurMedical visiteurMedical = listeVisiteursMedicaux.Find(vm => vm.Id == Int32.Parse(Request.Form["visiteurMedical"]));
                    Contact contact = listeContacts.Find(cont => cont.Id == Int32.Parse(Request.Form["contact"]));
                    Etablissement etablissement = listeEtablissements.Find(etab => etab.Id == Int32.Parse(Request.Form["etablissement"]));
                    string titre = Request.Form["titre"];
                    string contenu = Request.Form["contenu"];
                    DateTime date = Convert.ToDateTime(Request.Form["date"]);
                    List<Echantillon> listeEchantillon = new List<Echantillon>();


                    cr = new CompteRendu(idCompteRendu, visiteurMedical, contact, etablissement, titre, contenu, date, listeEchantillon);

                    if (idCompteRendu == 0)
                    {
                        crDao.Create(cr);
                    }
                    else
                    {
                        crDao.Update(cr);
                    }
                    ViewBag.cr = cr;

                    retour = View("FicheCompteRendu");

                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        if (Int32.TryParse(id, out int idCompteRendu))
                        {
                            if (etabDao.Read(idCompteRendu) != null)
                            {
                                cr = crDao.Read(idCompteRendu);
                                if ((Request.HttpMethod == "GET") && (Request.Params["action"] != null))
                                {
                                    if (Request.Params["action"] == "del")
                                    {
                                        crDao.Delete(cr);
                                        retour = RedirectToAction("FicheCompteRendu");
                                    }
                                }
                                else
                                {
                                    ViewBag.cr = cr;
                                }
                            }
                        }
                    }
                }
            }
            return retour;
        }

        public ActionResult FicheCompteRendu(String id)
        {
            ActionResult retour;

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                CompteRenduDAO crDao = new CompteRenduDAO();
                retour = RedirectToAction("AccueilCR", new { id = "Listedescompterendus" });

                if (!String.IsNullOrWhiteSpace(id))
                {
                    if (Int32.TryParse(id, out int idCompteRendu))
                    {
                        if (crDao.Read(idCompteRendu) != null)
                        {
                            CompteRendu cr = crDao.Read(idCompteRendu);
                            ViewBag.cr = cr;
                            retour = View();
                        }
                    }
                }
            }
            return retour;
        }

        public ActionResult FormEtablissement(string id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                EtablissementDAO etabDao = new EtablissementDAO();
                TypeEtablissementDAO typesEtabDao = new TypeEtablissementDAO();
                Etablissement etab = new Etablissement();
                ViewBag.etablissement = etab;
                List<TypeEtablissement> listeTypesEtablissement = typesEtabDao.RetournerTousLesTypesEtablissement();
                ViewBag.listeTypesEtablissement = listeTypesEtablissement;

                if (Request.HttpMethod == "POST")
                {
                    int idEtab = Int32.Parse(Request.Form["idEtab"]);
                    string nom = Request.Form["nom"];
                    string adresse = Request.Form["adresse"];
                    string mel = Request.Form["adresseMel"];
                    string numeroTelephone = Request.Form["numeroTelephone"];
                    int idType = Int32.Parse(Request.Form["idType"]);
                    TypeEtablissement typeEtab = listeTypesEtablissement.Find(tp => tp.Id == idType);


                    etab = new Etablissement(idEtab, nom, adresse, numeroTelephone, mel, typeEtab);

                    if (idEtab == 0)
                    {
                        etabDao.Create(etab);
                    }
                    else
                    {
                        etabDao.Update(etab);
                    }
                    ViewBag.etablissement = etab;

                    retour = View("FicheEtablissement");

                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        if (Int32.TryParse(id, out int idEtab))
                        {
                            if (etabDao.Read(idEtab) != null)
                            {
                                etab = etabDao.Read(idEtab);
                                if ((Request.HttpMethod == "GET") && (Request.Params["action"] != null))
                                {
                                    if (Request.Params["action"] == "del")
                                    {
                                        etabDao.Delete(etab);
                                        retour = RedirectToAction("FicheEtablissement");
                                    }
                                }
                                else
                                {
                                    ViewBag.etablissement = etab;
                                }
                            }
                        }
                    }
                }
            }
            return retour;
        }

        public ActionResult FicheEtablissement(String id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                EtablissementDAO etabDao = new EtablissementDAO();
                retour = RedirectToAction("AccueilCR", new { id = "Listedesetablissements" });

                if (!String.IsNullOrWhiteSpace(id))
                {
                    if (Int32.TryParse(id, out int idEtab))
                    {
                        if (etabDao.Read(idEtab) != null)
                        {
                            Etablissement etab = etabDao.Read(idEtab);
                            ViewBag.etablissement = etab;
                            retour = View();
                        }
                    }
                }
            }
            return retour;
        }

        public ActionResult FormContact(string id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                ContactDAO contactDao = new ContactDAO();
                Contact contact = new Contact();
                ViewBag.contact = contact;

                if (Request.HttpMethod == "POST")
                {
                    int idContact = Int32.Parse(Request.Form["idContact"]);
                    string nom = Request.Form["nom"];
                    string prenom = Request.Form["prenom"];
                    string mel = Request.Form["adresseMel"];
                    string numeroTelephone = Request.Form["numeroTelephone"];
                    string poste = Request.Form["poste"];
                    string commentaire = Request.Form["commentaire"];
                    List<Etablissement> listeEmployeur = new List<Etablissement>();
                    contact = new Contact(poste, commentaire, listeEmployeur, idContact, nom, prenom, mel, numeroTelephone);

                    if (idContact == 0)
                    {
                        contactDao.Create(contact);
                    }
                    else
                    {
                        contactDao.Update(contact);
                    }
                    ViewBag.contact = contact;

                    retour = View("FicheContact");

                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        if (Int32.TryParse(id, out int idContact))
                        {
                            if (contactDao.Read(idContact) != null)
                            {
                                contact = contactDao.Read(idContact);
                                if ((Request.HttpMethod == "GET") && (Request.Params["action"] != null))
                                {
                                    if (Request.Params["action"] == "del")
                                    {
                                        contactDao.Delete(contact);
                                        retour = RedirectToAction("FicheEtablissement");
                                    }
                                }
                                else
                                {
                                    ViewBag.contact = contact;
                                }
                            }
                        }
                    }
                }
            }
            return retour;
        }

        public ActionResult FicheContact(String id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                ContactDAO contactDao = new ContactDAO();
                retour = RedirectToAction("AccueilCR", new { id = "Listedescontacts" });

                if (!String.IsNullOrWhiteSpace(id))
                {
                    if (Int32.TryParse(id, out int idContact))
                    {
                        if (contactDao.Read(idContact) != null)
                        {
                            Contact contact = contactDao.Read(idContact);
                            ViewBag.contact = contact;
                            retour = View();
                        }
                    }
                }
            }
            return retour;
        }


        public ActionResult FormProduit(String id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ProduitDAO produitDao = new ProduitDAO();
                Produit produit = new Produit();
                ViewBag.produit = produit;

                if (Request.HttpMethod == "POST")
                {
                    int idProduit = Int32.Parse(Request.Form["idProduit"]);
                    string designation = Request.Form["designation"];
                    string description = Request.Form["description"];
                    int quantite = Int32.Parse(Request.Form["quantite"]);
                    double tarif = double.Parse(Request.Form["tarif"]);

                    produit = new Produit(idProduit, designation, description, quantite, tarif);

                    if (idProduit == 0)
                    {
                        produitDao.Create(produit);
                    }
                    else
                    {
                        produitDao.Update(produit);
                    }
                    ViewBag.produit = produit;

                    retour = View("FicheProduit");

                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(id))
                    {
                        if (Int32.TryParse(id, out int idProduit))
                        {
                            if (produitDao.Read(idProduit) != null)
                            {
                                produit = produitDao.Read(idProduit);
                                if ((Request.HttpMethod == "GET") && (Request.Params["action"] != null))
                                {
                                    if (Request.Params["action"] == "del")
                                    {
                                        produitDao.Delete(produit);
                                        retour = RedirectToAction("FicheProduit");
                                    }
                                }
                                else
                                {
                                    ViewBag.produit = produit;
                                }
                            }
                        }
                    }
                }
            }
            return retour;
        }


        public ActionResult FicheProduit(String id)
        {
            ActionResult retour = View();

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                ProduitDAO produitDao = new ProduitDAO();
                retour = RedirectToAction("AccueilCR", new { id = "Listedesproduits" });

                if (!String.IsNullOrWhiteSpace(id))
                {
                    if (Int32.TryParse(id, out int idProduit))
                    {
                        if (produitDao.Read(idProduit) != null)
                        {
                            Produit produit = produitDao.Read(idProduit);
                            ViewBag.produit = produit;
                            retour = View();
                        }
                    }
                }
            }
            return retour;
        }

        public ActionResult FicheFrais(String id)
        {
            ActionResult retour = RedirectToAction("AccueilFrais");

            if (Session["UserID"] == null)
            {
                retour = RedirectToAction("Login");
            }
            else
            {
                FicheDeFraisDAO ficheDeFraisDao = new FicheDeFraisDAO();
                retour = RedirectToAction("AccueilFrais");

                if (!String.IsNullOrWhiteSpace(id) && (Int32.TryParse(id, out int idFicheDeFrais)))
                {
                    if (ficheDeFraisDao.Read(idFicheDeFrais) != null)
                    {
                        FicheDeFrais ficheDeFrais = ficheDeFraisDao.Read(idFicheDeFrais);

                        if ((Request.HttpMethod == "GET") && (Request.Params["action"] != null))
                        {
                            if (Request.Params["action"] == "del")
                            {
                                ficheDeFraisDao.Delete(ficheDeFrais);
                            }
                        }
                        else
                        {
                            ViewBag.ficheDeFrais = ficheDeFrais;
                            retour = View();
                        }
                    }
                }
                else
                {

                    FicheDeFrais ficheDeFrais = new FicheDeFrais();
                    ficheDeFrais.IdVisiteurMedical = Convert.ToInt32(Session["UserID"]);
                    ViewBag.ficheDeFrais = ficheDeFrais;
                    ficheDeFraisDao.Create(ficheDeFrais);
                    retour = View();
                }
            }
            return retour;
        }

        public ActionResult FormLigneFrais(String id)
        {
            ActionResult retour = RedirectToAction("AccueilFrais");

            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                FicheDeFraisDAO ficheDeFraisDao = new FicheDeFraisDAO();
                LigneDeFraisDAO ligneDeFraisDao = new LigneDeFraisDAO();

                if (!String.IsNullOrWhiteSpace(id))
                {
                    if (Int32.TryParse(id, out int idFicheDeFrais))
                    {
                        if (ficheDeFraisDao.Read(idFicheDeFrais) != null)
                        {
                            FicheDeFrais ficheDeFrais = ficheDeFraisDao.Read(idFicheDeFrais);
                            ViewBag.ficheDeFrais = ficheDeFrais;

                            if ((Request.HttpMethod == "GET") && (Request.Params["action"] != null))
                            {
                                if (Request.Params["ligne"] != null)
                                {
                                    if (Request.Params["action"] == "del")
                                    {
                                        int idLf = Convert.ToInt32(Request.Params["ligne"]);
                                        LigneDeFrais lf = ligneDeFraisDao.Read(idLf);
                                        ligneDeFraisDao.Delete(lf);
                                        retour = RedirectToAction("FicheFrais", new { id = ficheDeFrais.Id });
                                    }
                                    else if (Request.Params["action"] == "modif")
                                    {
                                        int idLf = Convert.ToInt32(Request.Params["ligne"]);
                                        LigneDeFrais lf = ligneDeFraisDao.Read(idLf);
                                        ViewBag.ligneDeFrais = lf;
                                        retour = View();
                                    }
                                    else
                                    {
                                        retour = RedirectToAction("AccueilFrais");
                                    }
                                }
                                else
                                {
                                    if (Request.Params["action"] == "ajout")
                                    {
                                        ViewBag.ligneDeFrais = new LigneDeFrais();
                                        retour = View();
                                    }
                                    else
                                    {
                                        retour = RedirectToAction("AccueilFrais");
                                    }

                                }
                            }
                        }
                    }
                }
                else if (Request.HttpMethod == "POST")
                {
                    if (Request.Form["formulaireLigneDeFrais"] == "Ajouter")
                    {
                        int idLigneFrais = Int32.Parse(Request.Form["idLigneFrais"]);
                        int idFicheFrais = Int32.Parse(Request.Form["idFicheFrais"]);
                        String libelle = Request.Form["libelle"];
                        Double montant = double.Parse(Request.Form["montant"]);
                        bool horsforfait = (Request.Form["horsforfait"] == "1");
                        bool report = (Request.Form["report"] == "1");

                        LigneDeFrais lf = new LigneDeFrais(idLigneFrais, DateTime.Now, montant, libelle, false, false, horsforfait, report);

                        FicheDeFrais ficheDeFrais = ficheDeFraisDao.Read(idFicheFrais);

                        if ((ficheDeFrais.ListeDeLignesDeFrais.Find(lf2 => lf2.Id == idLigneFrais)) != null)
                        {
                            LigneDeFrais ligneDeFrais = (ficheDeFrais.ListeDeLignesDeFrais.Find(lf2 => lf2.Id == idLigneFrais));
                            ligneDeFrais.Id = lf.Id;
                            ligneDeFrais.Date = lf.Date;
                            ligneDeFrais.Montant = lf.Montant;
                            ligneDeFrais.Libelle = lf.Libelle;
                            ligneDeFrais.Validee = lf.Validee;
                            ligneDeFrais.Refusee = lf.Refusee;
                            ligneDeFrais.HorsForfait = lf.HorsForfait;
                            ligneDeFrais.Report = lf.Report;
                        }
                        else
                        {
                            ficheDeFrais.ListeDeLignesDeFrais.Add(lf);
                        }

                        ficheDeFraisDao.Update(ficheDeFrais);
                        ViewBag.ficheDeFrais = ficheDeFrais;
                        retour = RedirectToAction("FicheFrais", new { id = ficheDeFrais.Id });
                    }

                }
            }
            return retour;
        }
    }
}