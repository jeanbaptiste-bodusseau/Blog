using ACPR.Helper;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json.Serialization;
using static ACPR.Domaine.ReglesJson;

namespace ACPR.Domaine;

public class Incident
{
    [JsonPropertyName("financialEntityCode")]
    public string CodeReferenceEntiteFinanciere { get; private set; } //champ 2.1

    [JsonPropertyName("detectionDateTime")]
    public DateTime? DateDetection { get; private set; } //champ 2.2; Format ISO 8601 (YYYY-MM-DDThh:mm:ssZ)

    [JsonPropertyName("classificationDateTime")]
    public DateTime? DateClassification { get; private set; } //champ 2.3; Format ISO 8601 (YYYY-MM-DDThh:mm:ssZ)

    [JsonPropertyName("incidentDescription")]
    public string Description { get; private set; } //champ 2.4

    [JsonPropertyName("otherInformation")] 
    public string InformationAutres { get; private set; } //champ 2.10

    [JsonPropertyName("classificationTypes")]
    public IEnumerable<Classification> CritereClassifications { get; private set; }

    [JsonPropertyName("isBusinessContinuityActivated")]
    public bool ContinueteAffaires { get; private set; } //champ 2.9

    [JsonPropertyName("incidentOccurrenceDateTime")]
    public DateTime? OccurenceIncident { get; private set; } //champ 3.2; Format ISO 8601 (YYYY-MM-DDThh:mm:ssZ)

    [JsonPropertyName("incidentDuration")]
    public string? DureeIncident { get; private set; } //champ 3.15; Format DDD:HH:MM

    [JsonPropertyName("originatesFromThirdPartyProvider")]
    public string OrigineIncident { get; private set; } //champ 2.8

    [JsonPropertyName("incidentDiscovery")]
    public string DecouverteIncident { get; private set; } //champ 2.7

    [JsonPropertyName("competentAuthorityCode")]
    public string? ReferenceIncidentAutorite { get; private set; }

    [JsonPropertyName("incidentType")]
    public TypeIncident? TypeIncident { get; private set; }

    [JsonPropertyName("rootCauseHLClassification")] //4.1
    public IEnumerable<string>? CauseClassification { get; private set; }

    [JsonPropertyName("rootCausesDetailedClassification")] //4.2
    public IEnumerable<string>? DetailsClassification { get; private set; }

    [JsonPropertyName("rootCausesAdditionalClassification")] //4.3
    public IEnumerable<string>? ClassificationAdditionnel { get; private set; }

    [JsonPropertyName("rootCausesOther")]
    public string? AutresCauses { get; private set; }

    [JsonPropertyName("rootCausesInformation")]
    public string? InformationCauses;

    [JsonPropertyName("rootCauseAddressingDateTime")]
    public DateTime? DateTraitementCause;

    [JsonPropertyName("incidentResolutionSummary")]
    public string? ResumeResolutionIncident;

    [JsonPropertyName("incidentResolutionDateTime")]
    public DateTime? DateResolutionIncident;

    [JsonPropertyName("incidentResolutionVsPlannedImplementation")]
    public string? ResolutionIncidentVsPlanning;

    [JsonPropertyName("assessmentOfRiskToCriticalFunctions")]
    public string? EvaluationRisque;

    [JsonPropertyName("informationRelevantToResolutionAuthorities")]
    public string? InformationAutoriteResolution;

    [JsonPropertyName("financialRecoveriesAmount")]
    public float? MontantRecupere;

    [JsonPropertyName("grossAmountIndirectDirectCosts")]
    public float? MontantBrutCouts;

    [JsonPropertyName("recurringNonMajorIncidentsDescription")]
    public string? IncidentRecurrentDescription;

    [JsonPropertyName("recurringIncidentDate")]
    public DateTime? DateIncidentRecurrent;


    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="codeReferenceEntiteFinanciere">Reference de l'incident par l'entity financière (field 2.1)</param>
    /// <param name="dateDetection">Date de detection de l'incident (field 2.2)</param>
    /// <param name="dateClassification">Date de classification de l'incident (field 2.3)</param>
    /// <param name="description">Description de l'incident (field 2.4)</param>
    /// <param name="informationAutres">Autres informations importantes (field 2.10)</param>
    /// <param name="critereClassification">Liste classification (field 2.5 et 2.6)</param>
    /// <param name="continueteAffaires">Continuité des affaires (field 2.9)</param>
    /// <param name="origineIncident">Indique si l'incident vient d'un fournisseur tiers ou une autre entité (field 2.8)</param>
    /// <param name="decouverteIncident">Decouverte de l'incident (field 2.7)</param>
    /// <param name="occurenceIncident">Date de l'occurence de l'incident</param>
    public Incident(string codeReferenceEntiteFinanciere, DateTime? dateDetection, DateTime? dateClassification,
        string description, string informationAutres, IEnumerable<Classification> critereClassification,
        bool continueteAffaires, string origineIncident, DateTime? occurenceIncident, string? dureeIncident,
        string decouverteIncident, string? referenceAutorite, TypeIncident? typeIncident)
    {
        CodeReferenceEntiteFinanciere = codeReferenceEntiteFinanciere;
        DateDetection = dateDetection;
        DateClassification = dateClassification;
        Description = description;
        InformationAutres = informationAutres;
        CritereClassifications = critereClassification;
        ContinueteAffaires = continueteAffaires;
        OccurenceIncident = occurenceIncident;
        DureeIncident = dureeIncident;
        OrigineIncident = origineIncident;
        DecouverteIncident = decouverteIncident;
        ReferenceIncidentAutorite = referenceAutorite;
        TypeIncident = typeIncident;
        LoggerHelper.Instance.Log(Resource.IncidentReussite, LogLevel.Information);
    }

    /// <summary>
    /// Créer un Incident
    /// </summary>
    /// <param name="typeRapport">Type du rapport</param>
    /// <param name="valeurs">Dictionaires des données de l'incident</param>
    /// <param name="listeClassification">Liste des types de classifications</param>
    /// <param name="typeIncident">Oui si le type du rapport est une reclassification d'incident majeur en non-majeur</param>
    /// <returns>L'incident correspondant</returns>
    public static Incident Creer(string typeRapport, Dictionary<string, IEnumerable<string>?>? valeurs,
        IEnumerable<Classification>? listeClassification, TypeIncident? typeIncident = null)
    {
        LoggerHelper.Instance.Log(KeyWords.CategorieIncident, LogLevel.Information);
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_1Obligatoire);
        var code = VerifierCode(valeurs);
        var dateDetection = VerifierDateDetection(valeurs);
        var dateClassification = VerifierDateClassification(valeurs);
        var description = VerifierDescription(valeurs);
        var informationAutres = VerifierAutresInformations(valeurs, typeRapport);
        var continueteAffaires = VerifierContinuite(valeurs);
        var occurence = typeRapport != KeyWords.RapportInitial 
            ? VerifierOccurence(valeurs, typeRapport) 
            : default;

        var duree = typeRapport != KeyWords.RapportInitial 
            ? VerifierDuree(valeurs, typeRapport) 
            : null;

        valeurs.TryGetValue(KeyWords.IncidentReferenceAutorite, out var temp);
        var reference = temp?.FirstOrDefault();

        var origineIncident = VerifierOrigine(valeurs);
        var decouverteIncident = VerifierDecouverte(valeurs);
        var critereClassification = listeClassification!;

        return new Incident(code!, dateDetection, dateClassification, description!,
            informationAutres!, critereClassification,
            continueteAffaires, origineIncident!, occurence , duree, decouverteIncident!, string.IsNullOrEmpty(reference) ? null : reference,
            typeIncident);
    }

    /// <summary>
    /// Verifie si d'autres informations sont nécessaires
    /// </summary>
    /// <param name="valeurs">Dictionaire valeurs</param>
    /// <param name="typeRapport">Type du rapport</param>
    /// <returns>Autres informations de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string? VerifierAutresInformations(Dictionary<string, IEnumerable<string>?>? valeurs, string typeRapport)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_10Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentAutresInformation, out var temp);
        var autreInformations = temp?.FirstOrDefault();

        if (typeRapport == KeyWords.RapportReclassement && string.IsNullOrEmpty(autreInformations))
            throw new NoNullAllowedException(Resource.IncidentChamp2_10Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_10Reussite, LogLevel.Information);

        return autreInformations;
    }

    /// <summary>
    /// Verifie la réference de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Réference de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierCode(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_1Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentReference, out var temp);
        var code = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(code))
            throw new NoNullAllowedException(Resource.IncidentChamp2_1Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_1Reussite, LogLevel.Information);
        
        return code;
    }

    /// <summary>
    /// Verifie la date de détection de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Date détection de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static DateTime VerifierDateDetection(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_2Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDateDetection, out var temp);
        var date = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(date))
            throw new NoNullAllowedException(Resource.IncidentChamp2_2Obligatoire);

        if (!ValidationHelper.ValiderDate(date!, out var detection))
            throw new FormatException(Resource.IncidentChamp2_2NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_2Reussite, LogLevel.Information);

        return detection.ToUniversalTime();
    }

    /// <summary>
    /// Verifie la date de classification de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Date de classification de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static DateTime VerifierDateClassification(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_3Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDateClassification, out var temp);
        var date = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(date))
            throw new NoNullAllowedException(Resource.IncidentChamp2_3Obligatoire);

        if (!ValidationHelper.ValiderDate(date!, out var classification))
            throw new FormatException(Resource.IncidentChamp2_2NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_3Reussite, LogLevel.Information);

        return classification.ToUniversalTime();
    }

    /// <summary>
    /// Verifie la description de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Description de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierDescription(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_4Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDescription, out var temp);
        var description = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(description))
            throw new NoNullAllowedException(Resource.IncidentChamp2_4Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_4Reussite, LogLevel.Information);
        return description;
    }

    /// <summary>
    /// Verifier si un plan de Continuité d'affaires a été mis en place
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Si un plan est actif</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static bool VerifierContinuite(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_9Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentContinueteAffaires, out var temp);
        var continuite = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(continuite))
            throw new NoNullAllowedException(Resource.IncidentChamp2_9Obligatoire);

        if (ValidationHelper.VerifierYesNoString(continuite!) == null)
            throw new FormatException(Resource.IncidentChamp2_9NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_9Reussite, LogLevel.Information);

        return ValidationHelper.VerifierYesNoString(continuite) ?? false;
    }

    /// <summary>
    /// Verifie la date de l'occurence de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="typeRapport">Type de rapport</param>
    /// <returns>Date de l'occurence</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static DateTime VerifierOccurence(Dictionary<string, IEnumerable<string>?>? valeurs, string typeRapport)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp3_2Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentOccurence, out var temp);
        var occurence = temp?.FirstOrDefault();

        if (typeRapport != KeyWords.RapportInitial && string.IsNullOrEmpty(occurence))
            throw new NoNullAllowedException(Resource.IncidentChamp3_2Obligatoire);

        if (!ValidationHelper.ValiderDate(occurence, out var dateOccurence))
            throw new FormatException(Resource.IncidentChamp3_2NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp3_2Reussite, LogLevel.Information);

        return dateOccurence.ToUniversalTime();
    }

    /// <summary>
    /// Verifie la durére de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="typeRapport">Type de rapport</param>
    /// <returns>Durée de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierDuree(Dictionary<string, IEnumerable<string>?>? valeurs, string typeRapport)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp3_15Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDuree, out var duree);

        var enumerable = duree?.ToList();
        if (typeRapport != KeyWords.RapportInitial && !(enumerable?.Any() ?? false))
            throw new NoNullAllowedException(Resource.IncidentChamp3_15Obligatoire);

        if (!ValidationHelper.VerifierDureeNull(string.Join(":",enumerable!), out var dureeIncident, out var err))
            throw new FormatException(Resource.IncidentChamp3_15Erreur + err);

        LoggerHelper.Instance.Log(Resource.IncidentChamp3_15Reussite, LogLevel.Information);
        //La duree de l'incident doit etre au format DDD:HH:MM. Donc il faut completer de sorte a avoir 2 chiffres dans chaque section;
        return string.Join(":", dureeIncident!.Select(val => val == 0 ? val.ToString() + "0" : val.ToString()));
    }

    /// <summary>
    /// Verifie l'origine de l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Origine de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierOrigine(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_8Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentOrigine, out var temp);
        var origine = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(origine))
            throw new NoNullAllowedException(Resource.IncidentChamp2_8Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_8Reussite, LogLevel.Information);

        return origine;
    }

    /// <summary>
    /// Verifie qui a découvert l'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Découvreur de l'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierDecouverte(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp2_7Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDecouverte, out var temp);
        var decouverte = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(decouverte))
            throw new NoNullAllowedException(Resource.IncidentChamp2_7Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C2_7], decouverte))
            throw new FormatException(Resource.IncidentChamp2_7NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp2_7Reussite, LogLevel.Information);

        return decouverte;
    }


    #region Page4 excel (en attente valeur)

    /// <summary>
    /// Complete un incident en cas de rapport Final
    /// </summary>
    /// <param name="causeClassification"></param>
    /// <param name="detailsClassification"></param>
    /// <param name="classificationAdditionnel"></param>
    /// <param name="autresCauses"></param>
    /// <param name="informationCauses"></param>
    /// <param name="dateTraitementCause"></param>
    /// <param name="resumeResolutionIncident"></param>
    /// <param name="dateResolutionIncident"></param>
    /// <param name="resolutionIncidentVsPlanning"></param>
    /// <param name="evaluationRisque"></param>
    /// <param name="informationAutoriteResolution"></param>
    /// <param name="montantRecupere"></param>
    /// <param name="montantBrutCouts"></param>
    /// <param name="incidentRecurrentDescription"></param>
    /// <param name="dateIncidentRecurrent"></param>
    private void CompleterIncident(IEnumerable<string>? causeClassification, IEnumerable<string>? detailsClassification, IEnumerable<string>? classificationAdditionnel, string? autresCauses, string? informationCauses, DateTime? dateTraitementCause, string? resumeResolutionIncident, DateTime? dateResolutionIncident, string? resolutionIncidentVsPlanning, string? evaluationRisque, string? informationAutoriteResolution, float? montantRecupere, float? montantBrutCouts, string? incidentRecurrentDescription, DateTime? dateIncidentRecurrent)
    {
        CauseClassification = causeClassification;
        DetailsClassification = detailsClassification;
        ClassificationAdditionnel = classificationAdditionnel;
        AutresCauses = autresCauses;
        InformationCauses = informationCauses;
        DateTraitementCause = dateTraitementCause;
        ResumeResolutionIncident = resumeResolutionIncident;
        DateResolutionIncident = dateResolutionIncident;
        ResolutionIncidentVsPlanning = resolutionIncidentVsPlanning;
        EvaluationRisque = evaluationRisque;
        InformationAutoriteResolution = informationAutoriteResolution;
        MontantRecupere = montantRecupere;
        MontantBrutCouts = montantBrutCouts;
        IncidentRecurrentDescription = incidentRecurrentDescription;
        DateIncidentRecurrent = dateIncidentRecurrent;
        LoggerHelper.Instance.Log(Resource.IncidentCompletionReussite, LogLevel.Information);
    }

    /// <summary>
    /// Complete l'incident en cas de rapport Final
    /// </summary>
    /// <param name="valeurs"></param>
    public void Completer(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        var causeClassification = VerifierCauseClassification(valeurs);
        var details = VerifierDetail(valeurs, out var addition);
        var autres = VerifierAutres(valeurs, Regles![C4_4].Intersect(details!).Any());
        var informations = VerifierInformations(valeurs);
        var dateTraitement = VerifierDateTraitement(valeurs);
        var resume = VerifierResume(valeurs);
        var dateResolution = VerifierDateResolution(valeurs);
        var evaluationRisque = VerifierEvaluationRisque(valeurs, out var informationAutorite);
        var benefices = VerifierMontant(valeurs, out var coutTotal);
        var dateReccurence = VerifierRecurrence(valeurs, out var description);

        valeurs!.TryGetValue(KeyWords.IncidentResolutionVsPlanning, out var temp);
        var planning = temp?.FirstOrDefault();
        CompleterIncident(causeClassification, details, addition, autres, informations, dateTraitement, resume, dateResolution, planning, evaluationRisque, informationAutorite, benefices, coutTotal, description, dateReccurence);
    }

    /// <summary>
    /// Verifie les causes de classifications
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static IEnumerable<string> VerifierCauseClassification(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp4_1Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentCauseClassification, out var causeClassification);

        if (!(causeClassification?.Any() ?? false))
            throw new NoNullAllowedException(Resource.IncidentChamp4_1Obligatoire);
        if (!ValidationHelper.ValiderRegle(Regles![C4_1], causeClassification!))
            throw new FormatException(Resource.IncidentChamp4_1NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_1Reussite, LogLevel.Information);

        return causeClassification;
    }

    /// <summary>
    /// Verifie les details de classifications
    /// </summary>
    /// <param name="valeurs"></param>
    /// <param name="addition"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static IEnumerable<string> VerifierDetail(Dictionary<string, IEnumerable<string>?>? valeurs,
        out IEnumerable<string>? addition)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp4_2Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDetailsCause, out var details);
        valeurs.TryGetValue(KeyWords.IncidentClassificationAdditionel, out addition);

        if (!(details?.Any() ?? false))
            throw new NoNullAllowedException(Resource.IncidentChamp4_2Obligatoire);
        if (!ValidationHelper.ValiderRegle(Regles![C4_2], details))
            throw new FormatException(Resource.IncidentChamp4_2NonConforme);

        if (Regles!["relation4_2_4_3"].Intersect(details).Any())
        {
            if (!(addition?.Any() ?? false))
                throw new NoNullAllowedException(Resource.IncidentChamp4_3Obligatoire);

            if (!ValidationHelper.ValiderRegle(Regles[C4_3], addition))
                throw new FormatException(Resource.IncidentChamp4_3NonConforme);
        }

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_2_4_3Reussite, LogLevel.Information);

        return details;
    }

    /// <summary>
    /// Verifie si d'autres causes sont presentes
    /// </summary>
    /// <param name="valeurs"></param>
    /// <param name="detailsAutre"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string? VerifierAutres(Dictionary<string, IEnumerable<string>?>? valeurs, bool detailsAutre)
    {
        if (valeurs == null)
            return null;

        valeurs.TryGetValue(KeyWords.IncidentAutresCauses, out var temp);
        var autres = temp?.FirstOrDefault();

        if (!detailsAutre) return autres;

        if (string.IsNullOrEmpty(autres))
            throw new NoNullAllowedException(Resource.IncidentChamp4_4Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_4Reussite, LogLevel.Information);

        return autres;

    }

    /// <summary>
    /// Verifie les informations des causes
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierInformations(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp4_5Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentInformationCauses, out var temp);
        var informations = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(informations))
            throw new NoNullAllowedException(Resource.IncidentChamp4_5Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_5Reussite, LogLevel.Information);

        return informations;
    }

    /// <summary>
    /// Verifie la date de traitement
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static DateTime VerifierDateTraitement(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new FormatException(Resource.IncidentChamp4_7Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDateTraitement, out var temp);
        var date = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(date))
            throw new NoNullAllowedException(Resource.IncidentChamp4_7Obligatoire);

        if (!ValidationHelper.ValiderDate(date, out var DateTraitement))
            throw new FormatException(Resource.IncidentChamp4_7NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_7Reussite, LogLevel.Information);

        return DateTraitement.ToUniversalTime();
    }

    /// <summary>
    /// Verifie le resume de la resolution
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierResume(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp4_6Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentResumeResolution, out var temp);
        var resume = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(resume))
            throw new NoNullAllowedException(Resource.IncidentChamp4_6Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_6Reussite, LogLevel.Information);

        return resume;
    }

    /// <summary>
    /// Verifie la date de la resolution
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static DateTime VerifierDateResolution(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new FormatException(Resource.IncidentChamp4_8Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentDateResolution, out var temp);
        var date = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(date))
            throw new NoNullAllowedException(Resource.IncidentChamp4_8Obligatoire);

        if (!ValidationHelper.ValiderDate(date, out var DateResolution))
            throw new FormatException(Resource.IncidentChamp4_8NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_8Reussite, LogLevel.Information);

        return DateResolution.ToUniversalTime();
    }

    /// <summary>
    /// Verifie les risques d'evolutions
    /// </summary>
    /// <param name="valeurs"></param>
    /// <param name="informationAutorite"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string? VerifierEvaluationRisque(Dictionary<string, IEnumerable<string>?>? valeurs,
        out string? informationAutorite)
    {
        if (valeurs == null)
            throw new NoNullAllowedException();

        valeurs.TryGetValue(KeyWords.IncidentEvaluationRisque, out var temp);
        var evaluationRisque = temp?.FirstOrDefault();

        valeurs.TryGetValue(KeyWords.IncidentInformationAutorite, out temp);
        informationAutorite = temp?.FirstOrDefault();

        if (!string.IsNullOrEmpty(evaluationRisque) && string.IsNullOrEmpty(informationAutorite))
            throw new NoNullAllowedException(Resource.IncidentChamp4_11Obligatoire);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_10Reussite, LogLevel.Information);

        return evaluationRisque;
    }

    /// <summary>
    /// Verifie les montants de l'incident
    /// </summary>
    /// <param name="valeurs"></param>
    /// <param name="coutTotal"></param>
    /// <returns></returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static float VerifierMontant(Dictionary<string, IEnumerable<string>?>? valeurs, out float coutTotal)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.IncidentChamp4_13Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentMontantRecupere, out var temp);
        var montantRecuperer = temp?.FirstOrDefault();
        valeurs.TryGetValue(KeyWords.IncidentMontantBrut, out temp);
        var montantBrut = temp?.FirstOrDefault();

        if (!float.TryParse(montantRecuperer, out var benefices))
            throw new FormatException(Resource.IncidentChamp4_14NonConforme);

        if (!float.TryParse(montantBrut, out coutTotal))
            throw new FormatException(Resource.IncidentChamp4_13NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_13_4_14Reussite, LogLevel.Information);

        return benefices;
    }

    /// <summary>
    /// Verifie la recurrence de l'incident
    /// </summary>
    /// <param name="valeurs"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    /// <exception cref="FormatException"></exception>
    internal static DateTime? VerifierRecurrence(Dictionary<string, IEnumerable<string>?>? valeurs,
        out string? description)
    {
        if (valeurs == null)
        {
            description = null;
            return null;
        }

        valeurs.TryGetValue(KeyWords.IncidentRecurrentDescription, out var temp);
        description = temp?.FirstOrDefault();
        valeurs.TryGetValue(KeyWords.IncidentDateRecurence, out temp);
        var date = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(description))
            return null;

        if (!ValidationHelper.ValiderDate(date, out var dateRecurrence))
            throw new FormatException(Resource.IncidentChamp4_16NonConforme);

        LoggerHelper.Instance.Log(Resource.IncidentChamp4_15_4_16Reussite, LogLevel.Information);

        return dateRecurrence.ToUniversalTime();
    }
    #endregion

}
