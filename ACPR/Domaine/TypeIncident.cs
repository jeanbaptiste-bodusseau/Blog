using ACPR.Helper;
using System.Data;
using System.Text.Json.Serialization;
using ACPR.Ressources;
using static ACPR.Domaine.ReglesJson;
using Microsoft.Extensions.Logging;

namespace ACPR.Domaine;

public class TypeIncident
{
    [JsonPropertyName("incidentClassification")]
    public IEnumerable<string> ClassificationIncident { get; set; } //champ 3.23

    [JsonPropertyName("otherIncidentClassification")]
    public string? AutreClassificationIncident { get; set; } //champ 3.24

    [JsonPropertyName("threatTechniques")]
    public IEnumerable<string>? TechniqueMenace {  get; set; } //champ 3.25
    [JsonPropertyName("otherThreatTechniques")]
    public string? AutreTechniqueMenace { get; set; } //champ 3.26

    [JsonPropertyName("indicatorsOfCompromise")]
    public string? IndicateurCompromis {  get; set; } //champ 3.35

    private TypeIncident(IEnumerable<string> classificationIncident, string? autreClassificationIncident, IEnumerable<string>? techniqueMenace, string? autreTechniqueMenace, string? indicateurCompromis)
    {
        ClassificationIncident = classificationIncident;
        AutreClassificationIncident = autreClassificationIncident;
        TechniqueMenace = techniqueMenace;
        AutreTechniqueMenace = autreTechniqueMenace;
        IndicateurCompromis = indicateurCompromis;
        LoggerHelper.Instance.Log(Resource.TypeIncidentReussite, LogLevel.Information);
    }
    
    /// <summary>
    /// Creer un Type d'Incident
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    public static TypeIncident? Creer(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        string? compromis = null;
        //recuperation données
        var classification = VerifierClassification(valeurs);
        var autreClassification = classification?.Contains(KeyWords.OtherIncidentType) ?? false
            ? VerifierAutreClassification(valeurs)
            : null;
        var menaces = classification?.Contains(KeyWords.CyberSecurity) ?? false 
            ?  VerifierCyberSecurity(valeurs, out compromis) 
            : null;
        var autreMenace = menaces?.Contains(KeyWords.OtherThreatTypes) ?? false 
            ? VerifierAutresMenaces(valeurs) 
            : null;

        return new TypeIncident(classification!, autreClassification, menaces, autreMenace, compromis);
    }

    /// <summary>
    /// Verifier les classifications
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Liste des classification du type d'incident</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static IEnumerable<string> VerifierClassification(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_23Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentTypeClassification, out var classification);

        if (!(classification?.Any() ?? false))
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_23Obligatoire);
        

        if (!ValidationHelper.ValiderRegle(Regles![C3_23], classification!))
            throw new FormatException(Resource.TypeIncidentChamp3_23NonConforme);

        LoggerHelper.Instance.Log(Resource.TypeIncidentChamp3_23Reussite, LogLevel.Information);

        return classification!;
    }

    /// <summary>
    /// Verifie les valeurs lié en cas d'incident lié a la cybersecurité
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="compromis">Compromis</param>
    /// <returns>Listes des menaces</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static IEnumerable<string> VerifierCyberSecurity(Dictionary<string, IEnumerable<string>?>? valeurs, out string? compromis)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_25_3_35Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentTypeCompromis, out var temp);
        compromis = temp?.FirstOrDefault();

        valeurs.TryGetValue(KeyWords.IncidentTypeMenace, out var menaces);

        if (string.IsNullOrEmpty(compromis) || !(menaces?.Any() ?? false))
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_25_3_35Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C3_25], menaces!))
            throw new FormatException(Resource.TypeIncidentChamp3_25NonConforme);

        LoggerHelper.Instance.Log(Resource.TypeIncidentChamp3_25_3_35Reussite, LogLevel.Information);

        return menaces!;
    }

    /// <summary>
    /// Verifie si d'autres menaces sont précisé
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Autre menaces enregistré</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierAutresMenaces(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_26Obligatoire);

        valeurs.TryGetValue(KeyWords.IncidentTypeAutreMenace, out var temp);
        var autreMenace = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(autreMenace))
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_26Obligatoire);

        LoggerHelper.Instance.Log(Resource.TypeIncidentChamp3_26Reussite, LogLevel.Information);

        return autreMenace;
    }

    /// <summary>
    /// Verifie si d'autres classifications sont précisé
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Autre Classification</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierAutreClassification(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        var autreClassification = valeurs?.ContainsKey(KeyWords.IncidentTypeAutreClassification) ?? false
            ? valeurs[KeyWords.IncidentTypeAutreClassification]?.FirstOrDefault()
            : null;

        if (string.IsNullOrEmpty(autreClassification))
            throw new NoNullAllowedException(Resource.TypeIncidentChamp3_24Obligatoire);

        LoggerHelper.Instance.Log(Resource.TypeIncidentChamp3_24Reussite, LogLevel.Information);

        return autreClassification;
    }
}
