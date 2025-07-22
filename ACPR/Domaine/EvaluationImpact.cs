using ACPR.Helper;
using System.Data;
using System.Text.Json.Serialization;
using ACPR.Ressources;
using static ACPR.Domaine.ReglesJson;
using Microsoft.Extensions.Logging;

namespace ACPR.Domaine;

public class EvaluationImpact
{
    [JsonPropertyName("hasImpactOnRelevantClients")]
    public bool? ImpactClientImportant { get; } //champ 3.8

    [JsonPropertyName("serviceImpact")]
    public ServiceImpact? ServiceImpact { get; }

    [JsonPropertyName("criticalServicesAffected")]
    public string? ServiceAffecte {  get; } //champ 3.22

    [JsonPropertyName("affectedAssets")]
    public ActifsAffecte? BienAffecte { get; }

    [JsonPropertyName("affectedFunctionalAreas")]
    public string? ZoneAffecte   { get; } //champ 3.27

    [JsonPropertyName("isAffectedInfrastructureComponents")]
    public string? ComposantInfraSontAffecte { get; } //champ3.28

    [JsonPropertyName("affectedInfrastructureComponents")]
    public string? ComposantInfraAffecte { get; } //champ 3.29

    [JsonPropertyName("isImpactOnFinancialInterest")]
    public string? ImpactInteretFinancier { get; } //champ 3.30

    public EvaluationImpact(bool? impactClient, ServiceImpact? serviceImpact, string? serviceAffecte, ActifsAffecte? bienAffecte, string? zoneAffecte, string? composantInfraSontAffecte, string? composantInfraAffecte, string? impactInteretFinancier)
    {
        ImpactClientImportant = impactClient;
        ServiceImpact = serviceImpact;
        ServiceAffecte = serviceAffecte;
        BienAffecte = bienAffecte;
        ZoneAffecte = zoneAffecte;
        ComposantInfraSontAffecte = composantInfraSontAffecte;
        ComposantInfraAffecte = composantInfraAffecte;
        ImpactInteretFinancier = impactInteretFinancier;
        LoggerHelper.Instance.Log(Resource.EvaluationImpactReussite, LogLevel.Information);
    }

    /// <summary>
    /// Créer une Evaluation d'impact
    /// </summary>
    /// <param name="clientImpact">si il s'agit d'un impact client</param>
    /// <param name="valeurs"></param>
    /// <param name="bienAffecte">ActifsAffecte correpondant</param>
    /// <param name="serviceImpact">ImpactService correspondant</param>
    /// <returns></returns>
    public static EvaluationImpact? Creer(bool clientImpact, Dictionary<string, string?>? valeurs, ActifsAffecte? bienAffecte = null, ServiceImpact? serviceImpact = null)
    {
        //recuperation données
        var impactClient = VerifierImpactClient(valeurs, clientImpact);
        var servicesCritiques = VerifierServicesCritiques(valeurs);
        var zones = VerifierZones(valeurs);
        var infraBool = VerifierInfra(valeurs, out var infra);
        var interet = VerifierInteret(valeurs);

        return new EvaluationImpact(impactClient, serviceImpact, servicesCritiques, bienAffecte, zones, infraBool, infra, interet);
    }

    /// <summary>
    /// Verifier l'impact sur les clients
    /// </summary>
    /// <param name="valeurs">Dictionaire des valeurs</param>
    /// <param name="clientImpact">Si le champ 2.5 indique impact_client</param>
    /// <returns>Si l'incident a impacté des clients</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static bool? VerifierImpactClient(Dictionary<string, string?>? valeurs, bool clientImpact)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_8Obligatoire);

        valeurs.TryGetValue(KeyWords.ImpactAssessmentClient, out var impactClient);

        if (clientImpact && string.IsNullOrEmpty(impactClient))
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_8Obligatoire);

        LoggerHelper.Instance.Log(Resource.EvaluationImpactChamp3_8Reussite, LogLevel.Information);

        return ValidationHelper.VerifierYesNoString(impactClient);
    }

    /// <summary>
    /// Verifie les services critiques
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Services critiques</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierServicesCritiques(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_22Obligatoire);

        valeurs.TryGetValue(KeyWords.ImpactAssessmentService, out var services);

        if (string.IsNullOrEmpty(services))
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_22Obligatoire);

        LoggerHelper.Instance.Log(Resource.EvaluationImpactChamp3_22Reussite, LogLevel.Information);

        return services;
    }

    /// <summary>
    /// Verifie les zones affectés
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Zones affectées</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierZones(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_27Obligatoire);

        valeurs.TryGetValue(KeyWords.ImpactAssessmentZone, out var zones);

        if (string.IsNullOrEmpty(zones))
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_27Obligatoire);

        LoggerHelper.Instance.Log(Resource.EvaluationImpactChamp3_27Reussite, LogLevel.Information);

        return zones;
    }

    /// <summary>
    /// Verifie les valeurs d'infrastructures
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="infra">Infra affecté</param>
    /// <returns>Si infra affecté</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierInfra(Dictionary<string, string?>? valeurs, out string? infra)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_28NonConforme);

        valeurs.TryGetValue(KeyWords.ImpactAssessmentInfraChoice, out var infraBool);
        valeurs.TryGetValue(KeyWords.ImpactAssessmentInfra, out infra);

        if (string.IsNullOrEmpty(infraBool))
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_28Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C3_28], infraBool))
            throw new FormatException(Resource.EvaluationImpactChamp3_28NonConforme);

        LoggerHelper.Instance.Log(Resource.EvaluationImpactChamp3_28Reussite, LogLevel.Information);

        if (infraBool != "yes") return infraBool;

        if (string.IsNullOrEmpty(infra))
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_29Obligatoire);

        LoggerHelper.Instance.Log(Resource.EvaluationImpactChamp3_29Reussite, LogLevel.Information);

        return infraBool;
    }

    /// <summary>
    /// Verifier interet financier
    /// </summary>
    /// <param name="valeurs">Dictionaire valeurs</param>
    /// <returns>Interet Financier</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierInteret(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_30Obligatoire);

        valeurs.TryGetValue(KeyWords.ImpactAssessmentInteretFinancier, out var interet);

        if (string.IsNullOrEmpty(interet))
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_30Obligatoire);

        LoggerHelper.Instance.Log(Resource.EvaluationImpactChamp3_30Reussite, LogLevel.Information);

        return interet;
    }
}
