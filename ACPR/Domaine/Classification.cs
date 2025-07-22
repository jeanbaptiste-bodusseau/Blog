using ACPR.Helper;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json.Serialization;
using static ACPR.Domaine.ReglesJson;
namespace ACPR.Domaine;


public class Classification
{
    [JsonPropertyName("classificationCriterion")]
    public string CritereClassification { get; } //champ 2.5

    [JsonPropertyName("countryCodeMaterialityThresholds")]
    public IEnumerable<string>? CodePays { get; } //champ 2.6; Format ISO 3166

    [JsonPropertyName("memberStatesImpactType")]
    public IEnumerable<string>? TypesImpactEtatsMembres { get; } //champ 3.18

    [JsonPropertyName("memberStatesImpactTypeDescription")]
    public string? DescriptionTypesImpactEtatsMembres { get; } //champ 3.19

    [JsonPropertyName("dataLossMaterialityThresholds")]
    public IEnumerable<string>? SeuilsMaterialitePertesDonnees { get; } //champ 3.20

    [JsonPropertyName("dataLossesDescription")]
    public string? DescriptionPerteDonnees { get; } //champ 3.21

    [JsonPropertyName("reputationalImpactType")]
    public IEnumerable<string>? TypeImpactReputation { get; } //champ 3.13

    [JsonPropertyName("reputationalImpactDescription")]
    public string? DescriptionImpactReputation { get; } //champ 3.14

    [JsonPropertyName("economicImpactMaterialityThreshold")]
    public string? SeuilMaterialiteImpactEconomique { get; } //champ 4.12

    private Classification(string critereClassification, string? codePaysSeuilMaterialite = null, IEnumerable<string>? typesImpactEtatsMembres = null, string? descriptionTypesImpactEtatsMembres = null, IEnumerable<string>? seuilsMaterialitePertesDonnees = null, string? descriptionPerteDonnees = null, IEnumerable<string>? typeImpactReputation = null, string? descriptionImpactReputation = null, string? seuilMaterialiteImpactEconomique = null)
    {
        CritereClassification = critereClassification;
        CodePays = codePaysSeuilMaterialite == null ? null : new List<string> { codePaysSeuilMaterialite };
        TypesImpactEtatsMembres = typesImpactEtatsMembres;
        DescriptionTypesImpactEtatsMembres = descriptionTypesImpactEtatsMembres;
        SeuilsMaterialitePertesDonnees = seuilsMaterialitePertesDonnees;
        DescriptionPerteDonnees = descriptionPerteDonnees;
        TypeImpactReputation = typeImpactReputation;
        DescriptionImpactReputation = descriptionImpactReputation;
        SeuilMaterialiteImpactEconomique = seuilMaterialiteImpactEconomique;
        LoggerHelper.Instance.Log("Reussite création Classification", LogLevel.Information);
    }

    /// <summary>
    /// Creer une classification d'incident
    /// </summary>
    /// <param name="valeurs">Dictionaire des valeurs de la classification</param>
    /// <returns>Nouvelle Classification</returns>
    /// <exception cref="Exception"></exception>
    public static Classification Creer(string typeRapport, Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        //recuperation des donnees
        var critere = VerifierCritere(valeurs);
        var seuilImpact = typeRapport == KeyWords.RapportFinal 
            ? VerifierSeuilMaterialiteImpact(valeurs) 
            : null;
        var temp = valeurs?.TryGetValue(KeyWords.ClassificationCodePays, out var value) ?? false ? value?.FirstOrDefault() : null;
        var codePays = string.IsNullOrEmpty(temp) ? null : temp;

        return critere switch
        {
            KeyWords.GeographicalSpread => VerifierGeographicalSpread(valeurs, typeRapport, seuilImpact),
            KeyWords.DataLoss => VerifierDataLoss(valeurs, typeRapport, seuilImpact),
            KeyWords.ReputationImpact => VerifierImpactReputation(valeurs, typeRapport, seuilImpact),
            _ => new Classification(critere, codePays, seuilMaterialiteImpactEconomique: seuilImpact),
        };
    }

    /// <summary>
    /// Permet de vérifier le critère de classification
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Critere de classification</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierCritere(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ClassificationChamp2_5Obligatoire);

        valeurs.TryGetValue(KeyWords.ClassificationCritere, out var temp);
        var critere = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(critere))
            throw new NoNullAllowedException(Resource.ClassificationChamp2_5Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C2_5], critere))
            throw new FormatException(Resource.ClassificationChamp2_5NonConforme);

        LoggerHelper.Instance.Log(Resource.ClassificationChamp2_5Reussite, LogLevel.Information);
        return critere;
    }

    /// <summary>
    /// Permet de verifier le seuil de l'impact matériel (?)
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Seuil d'impact</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierSeuilMaterialiteImpact(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ClassificationChamp4_12Obligatoire);

        valeurs.TryGetValue(KeyWords.ClassificationSeuilImpactEconomique, out var temp);
        var seuil = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(seuil))
            throw new NoNullAllowedException(Resource.ClassificationChamp4_12Obligatoire);

        LoggerHelper.Instance.Log(Resource.ClassificationChamp4_12Reussite, LogLevel.Information);
        return seuil;
    }

    /// <summary>
    /// Permet de vérifier toutes les valeurs en cas de critère GeographicalSpread
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="typeRapport">Type de rapport</param>
    /// <param name="seuilImpact">Seuil de l'impact matériel</param>
    /// <returns>Classification correspondante</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static Classification VerifierGeographicalSpread(Dictionary<string, IEnumerable<string>?>? valeurs, string typeRapport, string? seuilImpact = null)
    {
        const string critere = KeyWords.GeographicalSpread;
        var codePays = VerifierCodePays(valeurs);

        if (typeRapport == KeyWords.RapportInitial)
            return new Classification(critere, codePays, seuilMaterialiteImpactEconomique: seuilImpact);

        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ClassificationChamp3_18_3_19Obligatoire);

        valeurs.TryGetValue(KeyWords.ClassificationTypeImpactEtats, out var typeImpact);

        valeurs.TryGetValue(KeyWords.ClassificationDescriptionImpactEtats, out var temp);
        var description = temp?.FirstOrDefault();
        if (!(typeImpact?.Any() ?? false) || string.IsNullOrEmpty(description))
            throw new NoNullAllowedException(Resource.ClassificationChamp3_18_3_19Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C3_18], typeImpact!))
            throw new FormatException(Resource.ClassificationChamp3_18NonConforme);

        LoggerHelper.Instance.Log(Resource.ClassificationChamp3_18_3_19Reussite, LogLevel.Information);
        return new Classification(critere,codePays,typeImpact,description, seuilMaterialiteImpactEconomique:seuilImpact);
    }

    /// <summary>
    /// Permet de vérifier les valeurs correspondantes en cas de critère DataLoss
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="typeRapport">Type de rapport</param>
    /// <param name="seuilImpact">Seuil de l'impact matériel</param>
    /// <returns>Classification correspondante</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static Classification VerifierDataLoss(Dictionary<string, IEnumerable<string>?>? valeurs, string typeRapport, string? seuilImpact = null)
    {
        var critere = KeyWords.DataLoss;

        if (typeRapport == KeyWords.RapportInitial) 
            return new Classification(critere, seuilMaterialiteImpactEconomique: seuilImpact);

        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ClassificationChamp3_20_3_21Obligatoire);

        valeurs.TryGetValue(KeyWords.ClassificationTypePerteDonnées, out var perteDonnees);

        valeurs.TryGetValue(KeyWords.ClassificationDescriptionPerteDonnées, out var temp);
        var description = temp?.FirstOrDefault();

        if (!(perteDonnees?.Any() ?? false) || string.IsNullOrEmpty(description))
            throw new NoNullAllowedException(Resource.ClassificationChamp3_20_3_21Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C3_20], perteDonnees!))
            throw new FormatException(Resource.ClassificationChamp3_20NonConforme);

        LoggerHelper.Instance.Log(Resource.ClassificationChamp3_20_3_21Reussite, LogLevel.Information);
        return new Classification(critere,seuilsMaterialitePertesDonnees:perteDonnees!, descriptionPerteDonnees:description, seuilMaterialiteImpactEconomique:seuilImpact);
    }

    /// <summary>
    /// Permet de vérifier les valeurs correspondantes en cas de critère ReputationalImpact
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="typeRapport">Type de rapport</param>
    /// <param name="seuilImpact">Seuil de l'impact matériel</param>
    /// <returns>Classification correspondante</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static Classification VerifierImpactReputation(Dictionary<string, IEnumerable<string>?>? valeurs, string typeRapport, string? seuilImpact = null)
    {
        var critere = KeyWords.ReputationImpact;

        if (typeRapport == KeyWords.RapportInitial)
            return new Classification(critere, seuilMaterialiteImpactEconomique: seuilImpact);

        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ClassificationChamp3_13_3_14Obligatoire);
        
        valeurs.TryGetValue(KeyWords.ClassificationTypeImpactReputation, out var impactReputation);

        valeurs.TryGetValue(KeyWords.ClassificationDescriptionImpactReputation, out var temp);
        var description = temp?.FirstOrDefault();

        if (!(impactReputation?.Any() ?? false) || string.IsNullOrEmpty(description))
            throw new NoNullAllowedException(Resource.ClassificationChamp3_13_3_14Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C3_13], impactReputation!))
            throw new FormatException(Resource.ClassificationChamp3_13NonConforme);

        LoggerHelper.Instance.Log(Resource.ClassificationChamp3_13_3_14Reussite, LogLevel.Information);

        return new Classification(critere, typeImpactReputation:impactReputation!, descriptionImpactReputation:description, seuilMaterialiteImpactEconomique:seuilImpact);
    }

    /// <summary>
    /// Permet de vérifier le codePays
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Code pays</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierCodePays(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ClassificationChamp2_6Obligatoire);

        valeurs.TryGetValue(KeyWords.ClassificationCodePays, out var temp);
        var codePays = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(codePays))
            throw new NoNullAllowedException(Resource.ClassificationChamp2_6Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C2_6], codePays))
            throw new FormatException(Resource.ClassificationChamp2_5NonConforme);

        LoggerHelper.Instance.Log(Resource.ClassificationChamp2_6Reussite, LogLevel.Information);

        return codePays;
    }
}
