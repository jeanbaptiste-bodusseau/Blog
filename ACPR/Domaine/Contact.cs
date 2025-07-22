using ACPR.Helper;
using System.Data;
using System.Text.Json.Serialization;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;

namespace ACPR.Domaine;

public class Contact
{
    [JsonPropertyName("name")]
    public string? Nom { get; private set; } //champ 1.7 ou 1.10
    [JsonPropertyName("email")]
    public string? Mail { get; private set; } //champ 1.8 ou 1.11
    [JsonPropertyName("phone")]
    public string? Tel { get; private set; } //champ 1.9 ou 1.12

    /// <summary>
    /// Constructeur
    /// </summary>
    /// <param name="nom">Nom du contact (field 1.7 si premier, sinon 1.10)</param>
    /// <param name="mail">Email du contact (field 1.8 si premier, sinon 1.11)</param>
    /// <param name="tel">Telephone du contact (field 1.9 si premier, sinon 1.12)</param>
    private Contact(string? nom, string? mail, string? tel)
    {
        Nom = nom;
        Mail = mail;
        Tel = tel;
        LoggerHelper.Instance.Log(Resource.ContactReussite, LogLevel.Information);
    }
    
    /// <summary>
    /// Creer un nouveau Contact
    /// </summary>
    /// <param name="typeContact">1er ou 2nd contact</param>
    /// <param name="valeurs">Dictionaire des valeurs de contact</param>
    /// <returns>Contact ou null</returns>
    public static Contact? Creer(string typeContact, Dictionary<string, string?>? valeurs)
    {
        if (valeurs?.Count == 0)
        {
            switch (typeContact)
            {
                case KeyWords.ContactPremier:
                    throw new NoNullAllowedException(Resource.PremierContactObligatoire);
                case KeyWords.ContactSecond:
                    return null;
            }
        }

        var nom = VerifierNom(valeurs);
        var mail = VerifierEmail(valeurs);
        var tel = VerifierTel(valeurs);

        return new Contact(nom, mail, tel);
    }

    /// <summary>
    /// Permet de vérifier le nom du contact
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Nom du contact</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static string VerifierNom(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ContactNomObligatoire);

        valeurs.TryGetValue(KeyWords.ContactNom, out var nom);

        if (string.IsNullOrEmpty(nom))
            throw new NoNullAllowedException(Resource.ContactNomObligatoire);

        LoggerHelper.Instance.Log(Resource.ContactNomReussite, LogLevel.Information);

        return nom;
    }

    /// <summary>
    /// Permet de vérifier l'email du contact
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Email du contact</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierEmail(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ContactEmailObligatoire);

        valeurs.TryGetValue(KeyWords.ContactMail, out string? email);

        if (string.IsNullOrEmpty(email))
            throw new NoNullAllowedException(Resource.ContactEmailObligatoire);

        if (!ValidationHelper.ValiderEmail(email))
            throw new FormatException(Resource.ContactEmailNonConforme);

        LoggerHelper.Instance.Log(Resource.ContactEmailReussite, LogLevel.Information);

        return email;
    }

    /// <summary>
    /// Permet de vérifier le numéro de téléphone du contact
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Numéro du contact</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static string VerifierTel(Dictionary<string, string?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ContactTelObligatoire);

        valeurs.TryGetValue(KeyWords.ContactTelephone, out var tel);

        if (string.IsNullOrEmpty(tel))
            throw new NoNullAllowedException(Resource.ContactTelObligatoire);

        if (!ValidationHelper.ValiderTelephone(tel!))
            throw new FormatException(Resource.ContactTelNonConforme);

        LoggerHelper.Instance.Log(Resource.ContactTelReussite, LogLevel.Information);

        return tel;
    }
}

