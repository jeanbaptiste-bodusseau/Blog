using ACPR.Helper;
using System.Text.Json.Serialization;
using System.Data;
using ACPR.Ressources;
using static ACPR.Domaine.ReglesJson;
using Microsoft.Extensions.Logging;

namespace ACPR.Domaine;

public class Entite
{
    [JsonPropertyName("entityType")]
    public string TypeEntite { get; private set; } //En fonction de ou elle se trouve dans le rapport
    [JsonPropertyName("name")]
    public string Nom { get; private set; } //champ 1.2, 1.5 ou 1.13
    [JsonPropertyName("code")]
    public string? Code { get; private set; } //champ 1.3b si Entite Soumettant le rapport
    [JsonPropertyName("affectedEntityType")]
    public IEnumerable<string>? TypeEntiteAffecte { get; private set; } //champ 1.4
    [JsonPropertyName("LEI")]
    public string? Lei { get; private set; } //champ 1.3b, 1.6 ou 1.14; Format ISO 17442

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="type">Type de l'entité</param>
    /// <param name="nom">Nom de l'entité (field 1.2, 1.5 ou 1.13 en fonction du type de l'entité)</param>
    /// <param name="code">Code EU ID de l'entité soumettant le rapport (null si code LEI renseigné) (field 1.3b)</param>
    /// <param name="lei">Code LEI de l'entité (field 1.3a, 1.6 ou 1.14 en fonction du type de l'entité)</param>
    /// <param name="typeEntiteAffecte">Type de l'entité affecté (field 1.4)</param>
    private Entite(string type, string nom, string? code, string? lei, IEnumerable<string>? typeEntiteAffecte = null)
    {
        TypeEntite = type;
        Nom = nom;
        Code = code;
        Lei = lei;
        TypeEntiteAffecte = typeEntiteAffecte;
        LoggerHelper.Instance.Log(Resource.EntiteReussite, LogLevel.Information);
    }

    /// <summary>
    /// Créer une Entité
    /// </summary>
    /// <param name="typeEntite">Type de l'entité (soumettant, affecte ou parent)</param>
    /// <param name="valeurs">Dictionaire des valeurs de l'entité</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static Entite Creer(string? typeEntite,Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (string.IsNullOrEmpty(typeEntite))
            throw new NoNullAllowedException(Resource.EntiteTypeObligatoire);

        if (!ValidationHelper.ValiderRegle(Regles!["typeEntite"], typeEntite))
            throw new FormatException(Resource.EntiteTypeNonConforme);

        LoggerHelper.Instance.Log(Resource.EntiteTypeReussite, LogLevel.Information);

        var nom = VerifierNom(valeurs);
        var lei = VerifierLei(valeurs, typeEntite, out var code);
        var typeAffecte = VerifierTypeEntiteAffecte(valeurs);

        return new Entite(typeEntite, nom!, code, lei, typeAffecte);
    }

    /// <summary>
    /// Verifie le nom de l'entité
    /// </summary>
    /// <param name="valeurs">Dictioinnaire de valeurs</param>
    /// <returns>Nom de l'entité</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierNom(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EntiteNomObligatoire);

        valeurs.TryGetValue(KeyWords.EntiteNom, out IEnumerable<string>? temp);
        var nom = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(nom))
            throw new NoNullAllowedException(Resource.EntiteNomObligatoire);

        LoggerHelper.Instance.Log(Resource.EntiteNomReussite, LogLevel.Information);

        return nom;
    }

    /// <summary>
    /// Permet de vérifier le code Lei (et EU ID) des entités
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="typeEntite">Type de l'entité</param>
    /// <param name="code">Code EU ID de l'entité (si possible)</param>
    /// <returns>Code Lei de l'entité</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string? VerifierLei(Dictionary<string, IEnumerable<string>?>? valeurs, string typeEntite, out string? code)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EntiteLeiObligatoire);

        valeurs.TryGetValue(KeyWords.EntiteLei, out var value);
        var lei = value?.FirstOrDefault();

        valeurs.TryGetValue(KeyWords.EntiteCode, out value);
        code = value?.FirstOrDefault();

        if (typeEntite == KeyWords.EntiteSoumettant)
        {
            if (string.IsNullOrEmpty(lei) && string.IsNullOrEmpty(code))
                throw new NoNullAllowedException(Resource.EntiteLei_EUIDNull);

        } else if (string.IsNullOrEmpty(lei))
            throw new NoNullAllowedException(Resource.EntiteLeiObligatoire);

        if (!string.IsNullOrEmpty(lei))
        {
            if (!ValidationHelper.ValiderLei(lei))
                throw new FormatException(Resource.EntiteLeiNonConforme);
        }

        LoggerHelper.Instance.Log(Resource.EntiteCodeReussite, LogLevel.Information);

        return lei;
    }

    /// <summary>
    /// Verifier le type des entités affectés
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Liste des types</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static IEnumerable<string> VerifierTypeEntiteAffecte(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.EntiteAffecteTypeObligatoire);

        valeurs.TryGetValue(KeyWords.EntiteTypeEntiteAffecte, out var typeAffecte);

        if (!(typeAffecte?.Any() ?? false)) 
            throw new NoNullAllowedException(Resource.EntiteAffecteTypeObligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C1_4], typeAffecte!)) 
            throw new FormatException(Resource.EntiteAffecteTypeNonConforme);

        LoggerHelper.Instance.Log(Resource.EntiteAffecteTypeReussite, LogLevel.Information);
        return typeAffecte!;
    }
}