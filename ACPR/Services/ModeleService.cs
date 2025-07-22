using ACPR.Domaine;
using ACPR.Helper;
using System.Data;
using ACPR.Ressources;
using static ACPR.Domaine.ReglesJson;
using Microsoft.Extensions.Logging;
namespace ACPR.Services;

public class ModeleService(Dictionary<string, IEnumerable<string>> valeursExcel) : IModeleService
{
    public Dictionary<string, IEnumerable<string>> ValeursExcel { get; set; } = valeursExcel;

    internal string TypeRapport = "";
    internal string DeviseRapport = "";
    internal Dictionary<string, Entite> ListeEntites = [];
    internal IEnumerable<Entite> ListeEntiteAffecte = [];
    internal Dictionary<string, Contact?> ListeContacts = [];
    internal Incident? IncidentRapport = null;
    internal IEnumerable<Classification> ListeClassifications = [];
    internal EvaluationImpact? ImpactAssessment = null;
    internal IEnumerable<string>? RapportAutorite;
    internal string? RapportAutreAutorite;
    internal string? DureeActuelOuEstime;

    /// <summary>
    /// Creer un rapport grace au données fournis par le fichier excel
    /// </summary>
    /// <returns>rapport</returns>
    public Rapport CreerRapport()
    {
        CreerTypeRapport();
        CreerDeviseRapport();
        CreerEntites();
        CreerContacts();
        CreerClassifications();
        CreerIncident();
        CreerImpact();
        CompleterIncident();
        CreerRapportAutorite();
        CreerDureeActuelOuEstime();

        return new Rapport(
            TypeRapport,
            DeviseRapport,
            ListeEntites[KeyWords.EntiteSoumettant],
            ListeEntiteAffecte,
            ListeEntites[KeyWords.EntiteParent],
            ListeContacts[KeyWords.ContactPremier],
            ListeContacts[KeyWords.ContactSecond],
            IncidentRapport,
            ImpactAssessment,
            RapportAutorite,
            RapportAutreAutorite,
            DureeActuelOuEstime
        );
    }
    /// <summary>
    /// Récupere le type du rapport
    /// </summary>
    internal void CreerTypeRapport()
    {
        TypeRapport = ValeursExcel[C1_1].FirstOrDefault()!;
        if (string.IsNullOrEmpty(TypeRapport))
            throw new NoNullAllowedException(Resource.ModeleServiceChamp1_1Obligatoire);
        
        LoggerHelper.Instance.Log(Resource.ModeleServiceChamp1_1Reussite, LogLevel.Information);
    }

    /// <summary>
    /// Recupere la devise du rapport
    /// </summary>
    internal void CreerDeviseRapport()
    {
        DeviseRapport = ValeursExcel[C1_15].FirstOrDefault()!;
        if (string.IsNullOrEmpty(DeviseRapport))
            throw new NoNullAllowedException(Resource.ModeleServiceChamp1_15Obligatoire);
        if (!ValidationHelper.ValiderRegle(Regles![C1_15], DeviseRapport))
            throw new FormatException(Resource.ModeleServiceChamp1_15NonConforme);
        LoggerHelper.Instance.Log(Resource.ModeleServiceChamp1_15Reussite, LogLevel.Information);
    }

    /// <summary>
    /// Créer les entités du rapport
    /// </summary>
    internal void CreerEntites()
    {
        var entites = new Dictionary<string, Entite>();
        var entitesAffectes = new List<Entite>();
        var dicoEntite = new Dictionary<string, IEnumerable<string>?>();

        //Données entité Soumettant
        LoggerHelper.Instance.Log(KeyWords.CategorieEntiteSoumettant, LogLevel.Information);
        var typeEntite = KeyWords.EntiteSoumettant;
        dicoEntite.Add(KeyWords.EntiteNom, ValeursExcel[C1_2]);
        dicoEntite.Add(KeyWords.EntiteLei, ValeursExcel[C1_3a]);
        dicoEntite.Add(KeyWords.EntiteCode, ValeursExcel[C1_3b]);
        dicoEntite.Add(KeyWords.EntiteTypeEntiteAffecte, ValeursExcel[C1_4]);
        entites.Add(typeEntite, Entite.Creer(typeEntite, dicoEntite));

        dicoEntite.Remove(KeyWords.EntiteCode);

        //Données entités Affectés
        for (var index = 0; index < ValeursExcel[C1_5].Count(); index++)
        {
            LoggerHelper.Instance.Log(KeyWords.CategorieEntiteAffecte, LogLevel.Information);
            typeEntite = KeyWords.EntiteAffecte;
            dicoEntite[KeyWords.EntiteNom] = [ValeursExcel[C1_5].ToList()[index]];
            dicoEntite[KeyWords.EntiteLei] = [ValeursExcel[C1_6].ToList()[index]];
            entitesAffectes.Add(Entite.Creer(typeEntite, dicoEntite));
        }
        ListeEntiteAffecte = entitesAffectes;

        //Données entité parent
        LoggerHelper.Instance.Log(KeyWords.CategorieEntiteParent, LogLevel.Information);
        typeEntite = KeyWords.EntiteParent;
        dicoEntite[KeyWords.EntiteNom] = ValeursExcel[C1_13];
        dicoEntite[KeyWords.EntiteLei] = ValeursExcel[C1_14];
        entites.Add(typeEntite, Entite.Creer(typeEntite, dicoEntite));

        ListeEntites = entites;
    }

    /// <summary>
    /// Créer les contacts du rapport
    /// </summary>
    internal void CreerContacts()
    {
        var contacts = new Dictionary<string, Contact?>();
        var dicoContact = new Dictionary<string, string?>();

        //Données 1er Contact
        LoggerHelper.Instance.Log(KeyWords.CategoriePremierContact, LogLevel.Information);
        var typeContact = KeyWords.ContactPremier;
        dicoContact.Add(KeyWords.ContactNom, ValeursExcel[C1_7].FirstOrDefault()!);
        dicoContact.Add(KeyWords.ContactMail, ValeursExcel[C1_8].FirstOrDefault()!);
        dicoContact.Add(KeyWords.ContactTelephone, ValeursExcel[C1_9].FirstOrDefault()!);
        contacts.Add(typeContact, Contact.Creer(typeContact, dicoContact));

        dicoContact.Clear();
        //Données 2nd Contact
        LoggerHelper.Instance.Log(KeyWords.CategorieSecondContact, LogLevel.Information);
        typeContact = KeyWords.ContactSecond;
        dicoContact.Add(KeyWords.ContactNom, ValeursExcel[C1_10].FirstOrDefault()!);
        dicoContact.Add(KeyWords.ContactMail, ValeursExcel[C1_11].FirstOrDefault()!);
        dicoContact.Add(KeyWords.ContactTelephone, ValeursExcel[C1_12].FirstOrDefault()!);
        contacts.Add(typeContact, Contact.Creer(typeContact, dicoContact));
        
        ListeContacts = contacts;
    }

    /// <summary>
    /// Créer les classifications de l'incident du rapport
    /// </summary>
    internal void CreerClassifications()
    {
        var classifications = new List<Classification>();
        var dicoClassification = new Dictionary<string, IEnumerable<string>?>();
        LoggerHelper.Instance.Log(KeyWords.CategorieTypesClassification, LogLevel.Information);

        for (var index = 0; index < ValeursExcel[C2_5].ToList().Count; index++)
        {
            dicoClassification.Clear();
            dicoClassification.Add(KeyWords.ClassificationCritere, [ValeursExcel[C2_5].ToList()[index]]);
            dicoClassification.Add(KeyWords.ClassificationCodePays, [ValeursExcel[C2_6].ToList()[index]]);
            if (TypeRapport == KeyWords.RapportInitial)
            {
                classifications.Add(Classification.Creer(TypeRapport, dicoClassification));
                continue;
            }
            dicoClassification.Add(KeyWords.ClassificationTypeImpactEtats, ValeursExcel[C3_18]);
            dicoClassification.Add(KeyWords.ClassificationDescriptionImpactEtats, ValeursExcel[C3_19]);
            dicoClassification.Add(KeyWords.ClassificationTypePerteDonnées, ValeursExcel[C3_20]);
            dicoClassification.Add(KeyWords.ClassificationDescriptionPerteDonnées, ValeursExcel[C3_21]);
            dicoClassification.Add(KeyWords.ClassificationTypeImpactReputation, ValeursExcel[C3_13]);
            dicoClassification.Add(KeyWords.ClassificationDescriptionImpactReputation, ValeursExcel[C3_14]);
            if (TypeRapport == KeyWords.RapportFinal)
            {
                dicoClassification.Add(KeyWords.ClassificationSeuilImpactEconomique, ValeursExcel[C4_12]);
            }
            classifications.Add(Classification.Creer(TypeRapport, dicoClassification));
        }
        ListeClassifications = classifications;
    }

    /// <summary>
    /// Créer l'incident du rapport
    /// </summary>
    internal void CreerIncident()
    {
        var dicoIncident = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentReference, ValeursExcel[C2_1] },
            { KeyWords.IncidentDateDetection, ValeursExcel[C2_2]},
            { KeyWords.IncidentDateClassification, ValeursExcel[C2_3]},
            { KeyWords.IncidentDescription, ValeursExcel[C2_4]},
            { KeyWords.IncidentDecouverte, ValeursExcel[C2_7]},
            { KeyWords.IncidentOrigine, ValeursExcel[C2_8]},
            { KeyWords.IncidentContinueteAffaires, ValeursExcel[C2_9]},
            { KeyWords.IncidentAutresInformation, ValeursExcel[C2_10] }
        };
        if (TypeRapport != KeyWords.RapportInitial)
        {
            dicoIncident.Add(KeyWords.IncidentReferenceAutorite, ValeursExcel[C3_1]);
            dicoIncident.Add(KeyWords.IncidentOccurence, ValeursExcel[C3_2]);
            dicoIncident.Add(KeyWords.IncidentDuree, ValeursExcel[C3_15]);
        }
        ;
        if (TypeRapport != KeyWords.RapportInitial)
        {
            var dicoIncidentType = new Dictionary<string, IEnumerable<string>?>
            {
                { KeyWords.IncidentTypeClassification, ValeursExcel[C3_23] },
                { KeyWords.IncidentTypeAutreClassification, ValeursExcel[C3_24] },
                { KeyWords.IncidentTypeMenace, ValeursExcel[C3_25] },
                { KeyWords.IncidentTypeAutreMenace, ValeursExcel[C3_26] },
                { KeyWords.IncidentTypeCompromis, ValeursExcel[C3_35] } 
            };
            LoggerHelper.Instance.Log(KeyWords.CategorieTypeIncident, LogLevel.Information);
            IncidentRapport = Incident.Creer(TypeRapport, dicoIncident, ListeClassifications, TypeIncident.Creer(dicoIncidentType));
        }
        else
        {
            IncidentRapport = Incident.Creer(TypeRapport, dicoIncident, ListeClassifications);
        }
    }

    /// <summary>
    /// Créer l'éevaluation d'impact
    /// </summary>
    internal void CreerImpact()
    {
        if (TypeRapport == KeyWords.RapportInitial)
        {
            ImpactAssessment = null;
            return;
        }
        var dicoServiceImpact = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactDateRestauration, ValeursExcel[C3_3].FirstOrDefault()! },
            { KeyWords.ServiceImpactPanneService, string.Join(":", ValeursExcel[C3_16]) },
            { KeyWords.ServiceImpactActionTemporaire, ValeursExcel[C3_33].FirstOrDefault()! },
            { KeyWords.ServiceImpactDescriptionAction, ValeursExcel[C3_34].FirstOrDefault()! }
        };
        LoggerHelper.Instance.Log(KeyWords.CategorieImpactService, LogLevel.Information);
        var impactService = ServiceImpact.Creer(dicoServiceImpact);

        var dicoActifsAffecte = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, ValeursExcel[C3_4] },
            { KeyWords.AffectedAssetsClientPourcent, ValeursExcel[C3_5] },
            { KeyWords.AffectedAssetsHomoNombre, ValeursExcel[C3_6] },
            { KeyWords.AffectedAssetsHomoPourcent, ValeursExcel[C3_7] },
            { KeyWords.AffectedAssetsTransactionsNombre, ValeursExcel[C3_9] },
            { KeyWords.AffectedAssetsTransactionsPourcent, ValeursExcel[C3_10] },
            { KeyWords.AffectedAssetsValue, ValeursExcel[C3_11] },
            { KeyWords.AffectedEstimations, ValeursExcel[C3_12] } 
        };
        LoggerHelper.Instance.Log(KeyWords.CategorieActifsAffecte, LogLevel.Information);
        var actifsAffecte = ActifsAffecte.Creer(dicoActifsAffecte);

        var dicoImpact = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentClient, ValeursExcel[C3_8].FirstOrDefault()! },
            { KeyWords.ImpactAssessmentService, ValeursExcel[C3_22].FirstOrDefault()! },
            { KeyWords.ImpactAssessmentZone, ValeursExcel[C3_27].FirstOrDefault()! },
            { KeyWords.ImpactAssessmentInfraChoice, ValeursExcel[C3_28].FirstOrDefault()! },
            { KeyWords.ImpactAssessmentInfra, ValeursExcel[C3_29].FirstOrDefault()! },
            { KeyWords.ImpactAssessmentInteretFinancier, ValeursExcel[C3_30].FirstOrDefault()! } 
        };
        LoggerHelper.Instance.Log(KeyWords.CategorieEvaluationService, LogLevel.Information);
        ImpactAssessment = EvaluationImpact.Creer(ValeursExcel[C2_5].Contains(KeyWords.Client), dicoImpact, actifsAffecte, impactService);
    }

    /// <summary>
    /// Créer les valeurs rapportAutorite et rapportAutreAutorite
    /// </summary>
    /// <exception cref="NoNullAllowedException"></exception>
    internal void CreerRapportAutorite()
    {
        if (TypeRapport == KeyWords.RapportInitial) return;

        var rapportAutorite = ValeursExcel[C3_31];
        var rapportAutresAutorite = ValeursExcel[C3_32];

        if (!rapportAutorite?.Any() ?? true)
        {
            throw new NoNullAllowedException(Resource.RapportAutoriteObligatoire + "null");
        }
        if (!ValidationHelper.ValiderRegle(Regles![C3_31], rapportAutorite!))
        {
            throw new NoNullAllowedException(Resource.RapportAutoriteObligatoire + "nonconforme");
        }

        if (rapportAutorite!.Contains(KeyWords.AutreAutorite) && string.IsNullOrEmpty(rapportAutresAutorite.FirstOrDefault()!))
        {
            throw new NoNullAllowedException(Resource.RapportAutreAutorite);
        }

        LoggerHelper.Instance.Log(Resource.ModeleServiceChamps3_31_3_32Reussite, LogLevel.Information);

        RapportAutorite = rapportAutorite;
        RapportAutreAutorite = rapportAutresAutorite?.FirstOrDefault();
    }

    /// <summary>
    /// Créer la valeurs= DureeActuelOuEstime
    /// </summary>
    /// <exception cref="NoNullAllowedException"></exception>
    internal void CreerDureeActuelOuEstime()
    {
        if (TypeRapport == KeyWords.RapportInitial) return;

        var duree = ValeursExcel[C3_17].FirstOrDefault();
        if (!string.IsNullOrEmpty(IncidentRapport?.DureeIncident) || !string.IsNullOrEmpty(ImpactAssessment?.ServiceImpact?.PanneService))
        {
            if (string.IsNullOrEmpty(duree))
            {
                throw new NoNullAllowedException(Resource.IncidentChamp3_17Obligatoire);
            }
        }

        LoggerHelper.Instance.Log(Resource.ModeleServiceChamp3_17Reussite, LogLevel.Information);
        DureeActuelOuEstime = duree;
    }

    /// <summary>
    /// Complete un incident en cas de rapport Final
    /// </summary>
    internal void CompleterIncident()
    {
        if (TypeRapport != KeyWords.RapportFinal) return;
        var dicoIncident = new Dictionary<string, IEnumerable<string>>
        {
            { KeyWords.IncidentCauseClassification, ValeursExcel[C4_1] },
            { KeyWords.IncidentDetailsCause, ValeursExcel[C4_2] },
            { KeyWords.IncidentClassificationAdditionel, ValeursExcel[C4_3] },
            { KeyWords.IncidentAutresCauses, ValeursExcel[C4_4] },
            { KeyWords.IncidentInformationCauses, ValeursExcel[C4_5] },
            { KeyWords.IncidentDateTraitement, ValeursExcel[C4_7] },
            { KeyWords.IncidentResumeResolution, ValeursExcel[C4_6] },
            { KeyWords.IncidentDateResolution, ValeursExcel[C4_8] },
            { KeyWords.IncidentResolutionVsPlanning, ValeursExcel[C4_9] },
            { KeyWords.IncidentEvaluationRisque, ValeursExcel[C4_10] },
            { KeyWords.IncidentInformationAutorite, ValeursExcel[C4_11] },
            { KeyWords.IncidentMontantRecupere, ValeursExcel[C4_14] },
            { KeyWords.IncidentMontantBrut, ValeursExcel[C4_13] },
            { KeyWords.IncidentRecurrentDescription, ValeursExcel[C4_15] },
            { KeyWords.IncidentDateRecurence, ValeursExcel[C4_16] } 
        };
        IncidentRapport!.Completer(dicoIncident);
    }
}
