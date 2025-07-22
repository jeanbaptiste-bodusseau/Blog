using ACPR.Helper;
using System.Data;
using System.Text.Json.Serialization;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;

namespace ACPR.Domaine;

public class ServiceImpact
{
    [JsonPropertyName("serviceRestorationDateTime")]
    public DateTime? RestaurationServiceDate { get; }  //champ 3.3; Format ISO 8601 (YYYY-MM-DDThh:mm:ssZ)

    [JsonPropertyName("serviceDowntime")]
    public string? PanneService { get; } //champ 3.16; Format DDD:HH:MM

    [JsonPropertyName("isTemporaryActionsMeasuresForRecovery")]
    public bool? ActionTemporairePourRecuperation { get; } //champ 3.33

    [JsonPropertyName("descriptionOfTemporaryActionsMeasuresForRecovery")]
    public string? DescriptionActionTemporaire { get; } //champ 3.34


    private ServiceImpact(DateTime? restaurationDate, string? panneService, bool? actionTemporaire, string? descriptionActionTemporaire)
    {
        RestaurationServiceDate = restaurationDate;
        PanneService = panneService;
        ActionTemporairePourRecuperation = actionTemporaire;
        DescriptionActionTemporaire = descriptionActionTemporaire;
        LoggerHelper.Instance.Log(Resource.ImpactServiceReussite, LogLevel.Information);
    }
    
    /// <summary>
    /// Créer un Impact des Services
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    public static ServiceImpact? Creer(Dictionary<string, string?>? valeurs)
    {
        //recuperation données
        var panne = VerifierPanne(valeurs);
        var date = !ValidationHelper.VerifierDureeNull(panne, out _, out _) 
            ? default 
            : VerifierDate(valeurs);
        var actionTemporaire = VerifierActions(valeurs, out var description);

        return new ServiceImpact(date, panne, actionTemporaire, description);
    }

    /// <summary>
    /// Verifie la date
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Date de l'impact</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static DateTime VerifierDate(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EvaluationImpactChamp3_30Obligatoire);

        valeurs.TryGetValue(KeyWords.ServiceImpactDateRestauration, out var date);

        if (string.IsNullOrEmpty(date))
            throw new NoNullAllowedException(Resource.ImpactServiceChamp3_3Obligatoire);

        if (!ValidationHelper.ValiderDate(date, out var dateConvertie))
            throw new FormatException(Resource.ImpactServiceChamp3_3NonConforme);

        LoggerHelper.Instance.Log(Resource.ImpactServiceChamp3_3Reussite, LogLevel.Information);

        return dateConvertie.ToUniversalTime();
    }

    /// <summary>
    /// Verifie la durée de la panne
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Durée de la panne</returns>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierPanne(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ImpactServiceChamp3_16Erreur);

        valeurs.TryGetValue(KeyWords.ServiceImpactPanneService, out var panne);
        
        if (!ValidationHelper.VerifierDureeNull(panne, out var duree, out var err))
            throw new FormatException(Resource.ImpactServiceChamp3_16Erreur + err);

        LoggerHelper.Instance.Log(Resource.ImpactServiceChamp3_16Reussite, LogLevel.Information);

        //Rajoute un 0 pour que chaque section de la durée ai minimum 2 chiffres;
        //La durée doit etre au forma DDD:HH:MM
        return string.Join(":", duree!.Select(val => val == 0 ? val.ToString() + "0" : val.ToString()));
    }

    /// <summary>
    /// Verifie si une action temporaire a été prise
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="description">Description de l'action</param>
    /// <returns>Si une action a été prise</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static bool VerifierActions(Dictionary<string, string?>? valeurs, out string? description)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ImpactServiceChamp3_33Obligatoire);

        valeurs.TryGetValue(KeyWords.ServiceImpactActionTemporaire, out var actionTemp);
        valeurs.TryGetValue(KeyWords.ServiceImpactDescriptionAction, out description);

        if (string.IsNullOrEmpty(actionTemp))
            throw new NoNullAllowedException(Resource.ImpactServiceChamp3_33Obligatoire);

        var action = ValidationHelper.VerifierYesNoString(actionTemp);

        if (action == null) throw new FormatException(Resource.ImpactServiceErreur3_33);
        if ((bool)action! && string.IsNullOrEmpty(description))
            throw new NoNullAllowedException(Resource.ImpactServiceChamp3_34Obligatoire);

        LoggerHelper.Instance.Log(Resource.ImpactServiceChamp3_4Reussite, LogLevel.Information);

        return (bool)action;

    }
}


