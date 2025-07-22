namespace ACPR.Domaine;

public static class KeyWords
{

    #region Rapport
    public const string RapportInitial = "initial_notification";

    public const string RapportIntermediaire = "intermediate_report";

    public const string RapportFinal = "final_report";

    public const string RapportReclassement = "major_incident_reclassified_as_non-major";

    public const string RapportAutorite = "rapportAutorite";

    public const string RapportAutoriteAutre = "rapportAutoriteAutres";

    public const string DureeActuelOuEstime = "actualOrEstimateDowntime";
    #endregion

    #region Entite
    public const string EntiteNom = "entityName";

    public const string EntiteSoumettant = "SUBMITTING_ENTITY";

    public const string EntiteAffecte = "AFFECTED_ENTITY";

    public const string EntiteParent = "ULTIMATE_PARENT_UNDERTAKING_ENTITY";

    public const string EntiteLei = "lei";

    public const string EntiteCode = "code";

    public const string EntiteTypeEntiteAffecte = "affectedEntityType";
    #endregion

    #region Contact
    public const string ContactNom = "contactName";

    public const string ContactPremier = "premier";

    public const string ContactSecond = "second";

    public const string ContactMail = "email";

    public const string ContactTelephone = "phoneNumber";
    #endregion

    #region Incident
    public const string IncidentReference = "ref";

    public const string IncidentDateDetection = "detectionDate";

    public const string IncidentDateClassification = "classificationDate";

    public const string IncidentDescription = "description";

    public const string IncidentAutresInformation = "otherInformations";

    public const string IncidentContinueteAffaires = "continuityBusiness";

    public const string IncidentOccurence = "incidentOccurence";

    public const string IncidentDuree = "incidentDuration";

    public const string IncidentOrigine = "incidentOrigin";

    public const string IncidentReferenceAutorite = "ReferenceAutorite";

    public const string IncidentDecouverte = "incidentDiscovery";

    public const string IncidentCauseClassification = "incidentCauseClassification";

    public const string IncidentDetailsCause = "incidentDetailsClass";

    public const string IncidentClassificationAdditionel = "incidentAdditionel";

    public const string IncidentAutresCauses = "incidentOthers";

    public const string IncidentInformationCauses = "incidentCausesInformation";

    public const string IncidentDateTraitement = "incidentDateTreatment";

    public const string IncidentResumeResolution = "incidentResolutionResume";

    public const string IncidentDateResolution = "incidentResolutionDate";

    public const string IncidentResolutionVsPlanning = "IncidentPlanning";

    public const string IncidentEvaluationRisque = "incidentRisqueEvaluation";

    public const string IncidentInformationAutorite = "incidentAutoriteInformation";

    public const string IncidentMontantRecupere = "incidentRecoveries";

    public const string IncidentMontantBrut = "incidentDirectCost";

    public const string IncidentRecurrentDescription = "incidentDescriptionRecurrent";

    public const string IncidentDateRecurence = "incidentDateRecurrent";
    #endregion

    #region Classification
    public const string ClassificationCritere = "classificationCriteria";

    public const string ClassificationCodePays = "countryCode";

    public const string ClassificationTypeImpactEtats = "impactTypesMembers";

    public const string ClassificationDescriptionImpactEtats = "descriptionImpactStates";

    public const string ClassificationTypePerteDonnées = "dataLossType";

    public const string ClassificationDescriptionPerteDonnées = "descriptionDataLoss";

    public const string ClassificationTypeImpactReputation = "reputationImpactType";

    public const string ClassificationDescriptionImpactReputation = "descriptionReputationImpact";

    public const string ClassificationSeuilImpactEconomique = "seuilImpactEconomique";
    #endregion

    #region IncidentType
    public const string IncidentTypeClassification = "incidentClassification";

    public const string IncidentTypeAutreClassification = "incidentAutreClassification";

    public const string IncidentTypeMenace = "techniqueMenace";

    public const string IncidentTypeAutreMenace = "autreMenace";

    public const string IncidentTypeCompromis = "incidentCompromis";
    #endregion

    #region ImpactAssessment
    public const string ImpactAssessmentClient = "impactClient";

    public const string ImpactAssessmentService = "servicesCritical";

    public const string ImpactAssessmentZone = "affectedArea";

    public const string ImpactAssessmentInfraChoice = "isAffectedInfra";

    public const string ImpactAssessmentInfra = "affectedInfra";

    public const string ImpactAssessmentInteretFinancier = "financialInterest";
    #endregion


    #region ServiceImpact
    public const string ServiceImpactDateRestauration = "dateRestauration";

    public const string ServiceImpactPanneService = "panneService";

    public const string ServiceImpactActionTemporaire = "actionTemporaire";

    public const string ServiceImpactDescriptionAction = "descriptionAction";
    #endregion

    #region AffectedAssets
    public const string AffectedAssetsClientNombre = "affectedClientsNumber";
    public const string AffectedAssetsClientPourcent = "affectedClientsPercent";

    public const string AffectedAssetsHomoNombre = "affectedFinancialNumber";
    public const string AffectedAssetsHomoPourcent = "affectedFinancialPercent";

    public const string AffectedAssetsTransactionsNombre = "affectedTransactionsNumber";
    public const string AffectedAssetsTransactionsPourcent = "affectedTransactionsPercent";

    public const string AffectedAssetsValue = "affectedValue";

    public const string AffectedEstimations = "affectedEstimate";
    #endregion

    #region NumberPercentage
    public const string NPnombre = "nombre";

    public const string NPpourcent = "pourcent";
    #endregion


    public const string Client = "clients_financial_counterparts_and_transactions_affected";

    public const string AutreAutorite = "other_(please_specify)";

    public const string GeographicalSpread = "geographical_spread";

    public const string DataLoss = "data_losses";

    public const string ReputationImpact = "reputational_impact";

    public const string CyberSecurity = "cybersecurity-related";

    public const string OtherIncidentType = "other_(please_specify)";

    public const string OtherThreatTypes = "other_(please_specify)";

    public const string ChampAFormatter = "champsAFormatter";
    #region Categorie
    public const string CategorieLectureExcel = "---- Lecture Excel -----\n";

    public const string CategorieValidationJson = "---- Validation Json -----\n";

    public const string CategorieEntiteSoumettant = "----- Entite Soumettant -----\n";

    public const string CategorieEntiteAffecte = "----- Entite Affecte -----\n";

    public const string CategorieEntiteParent = "----- Entite Parent -----\n";

    public const string CategoriePremierContact = "----- Premier Contact -----\n";

    public const string CategorieSecondContact = "----- Second Contact -----\n";

    public const string CategorieTypesClassification = "----- Types Classification -----\n";

    public const string CategorieTypeIncident = "----- Type Incident -----\n";

    public const string CategorieIncident = "----- Incident -----\n";

    public const string CategorieImpactService = "----- Impact Service -----\n";

    public const string CategorieActifsAffecte = "----- Actifs Affectes -----\n";

    public const string CategorieEvaluationService = "----- Evaluation Service -----\n";
    #endregion

}
