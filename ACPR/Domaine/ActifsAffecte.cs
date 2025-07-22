using ACPR.Helper;
using System.Data;
using System.Text.Json.Serialization;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;
using static ACPR.Domaine.ReglesJson;
namespace ACPR.Domaine;

public class ActifsAffecte
{
    [JsonPropertyName("affectedClients")]
    public NombrePourcentage? ClientAffectes {  get; internal set; } // champ 3.4 et 3.5
    [JsonPropertyName("affectedFinancialCounterparts")]
    public NombrePourcentage? HomologueFinancier { get; internal set; } // champ 3.6 et 3.7
    [JsonPropertyName("affectedTransactions")]
    public NombrePourcentage? Transactions {  get; internal set; } //champ 3.9 et 3.10
    [JsonPropertyName("valueOfAffectedTransactions")]
    public float? ValeurTransaction { get; internal set; } //champ 3.11
    [JsonPropertyName("numbersActualEstimate")]
    public IEnumerable<string>? EstimationsNombres { get; internal set; } //champ 3.12

    private ActifsAffecte(NombrePourcentage? clients, NombrePourcentage? homologue, NombrePourcentage? transactions, float? valeurTransaction, IEnumerable<string>? estimations)
    {
        ClientAffectes = clients;
        HomologueFinancier = homologue;
        Transactions = transactions;
        ValeurTransaction = valeurTransaction;
        EstimationsNombres = estimations ?? (List<string>) [];
        LoggerHelper.Instance.Log(Resource.ActifsAffecteReussite,LogLevel.Information);
    }
    
    /// <summary>
    /// Créer les Actifs Affectes d'une Evaluation d'Impact
    /// </summary>
    /// <param name="valeurs"></param>
    /// <returns></returns>
    public static ActifsAffecte? Creer(Dictionary<string, IEnumerable<string>?> valeurs)
    {
        //recuperationDonnées
        var client = VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsClientNombre, KeyWords.AffectedAssetsClientPourcent, Resource.ActifsAffecteChamps3_4_3_5Obligatoire, Resource.ActifsAffecteErreur3_4_3_5, Resource.ActifsAffecteChamps3_4_3_5Reussite);
        var homologue = VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsHomoNombre, KeyWords.AffectedAssetsHomoPourcent, Resource.ActifsAffecteChamp3_6_3_7Obligatoire, Resource.ActifsAffecteErreur3_6_3_7, Resource.ActifsAffecteChamp3_6_3_7Reussite);
        var transactions = VerifierTransactions(valeurs, out var valTrans);
        var valeurTransaction = VerifierValeurTransaction(valTrans, transactions);
        var estimations = VerifierEstimations(valeurs);
        
        return new ActifsAffecte(client, homologue, transactions, valeurTransaction, estimations);
    }


    /// <summary>
    /// Permet de créer les relations nombre-pourcentages des client et homologues financiers
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="cleNb">Cle de dictionnaire pour le nombre</param>
    /// <param name="clePc">Cle de dictionnaire pour le pourcentage</param>
    /// <param name="errNull">Erreur a sortir si valeur null</param>
    /// <param name="errFmt">Erreur a sortir si valeur non conforme</param>
    /// <param name="log">Message en cas de réussite</param>
    /// <returns>NombrePourcentage correspondant</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static NombrePourcentage? VerifierNombrePourcentage(Dictionary<string, IEnumerable<string>?>? valeurs, string cleNb, string clePc, string errNull, string errFmt, string log)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(errNull);

        valeurs.TryGetValue(cleNb, out var temp);
        var nb = temp?.FirstOrDefault();

        valeurs.TryGetValue(clePc, out temp);
        var pc = temp?.FirstOrDefault();

        if (string.IsNullOrEmpty(nb) || string.IsNullOrEmpty(pc))
            throw new NoNullAllowedException(errNull);

        if (!NombrePourcentage.Creer(nb, pc, out var result))
            throw new FormatException(errFmt);

        LoggerHelper.Instance.Log(log, LogLevel.Information);
        return result;
    }

    /// <summary>
    /// Permet de creer une relation NombrePourcentage lorsque les valeurs ont déjà été extraite
    /// </summary>
    /// <param name="nb">Nombre</param>
    /// <param name="pc">Pourcentage</param>
    /// <param name="errNull">Erreur si valeur null</param>
    /// <param name="errFmt">Erreur si valeur non conforme</param>
    /// <param name="log">Message en cas de réussite</param>
    /// <returns>NombrePourcentage correspondant</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static NombrePourcentage? VerifierNombrePourcentage(string nb, string pc, string errNull, string errFmt, string log)
    {
        if (string.IsNullOrEmpty(nb) || string.IsNullOrEmpty(pc))
            throw new NoNullAllowedException(errNull);

        if (!NombrePourcentage.Creer(nb, pc, out var result))
            throw new FormatException(errFmt);

        LoggerHelper.Instance.Log(log, LogLevel.Information);
        return result;
    }

    /// <summary>
    /// Permet de verifier les valeurs lié aux Transactions
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <param name="transactionValeur">Valeur transaction</param>
    /// <returns>NombrePourcentage correspondant à la transaction</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static NombrePourcentage? VerifierTransactions(Dictionary<string, IEnumerable<string>?>? valeurs, out string? transactionValeur)
    {
        if (valeurs == null)
        {
            transactionValeur = null;
            return null;
        }

        valeurs.TryGetValue(KeyWords.AffectedAssetsTransactionsNombre, out var temp);
        var transactionNombre = temp?.FirstOrDefault();

        valeurs.TryGetValue(KeyWords.AffectedAssetsTransactionsPourcent, out temp);
        var transactionPourcent = temp?.FirstOrDefault();

        valeurs.TryGetValue(KeyWords.AffectedAssetsValue, out temp);
        transactionValeur = temp?.FirstOrDefault();

        //Les valeurs peuvent etre null, si le nombre de transactions affecté est inférieur ou égal à 0
        if (string.IsNullOrEmpty(transactionNombre)) return null;

        if (!int.TryParse(transactionNombre, out var transactionNombreResultat))
            throw new FormatException(Resource.ActifsAffecteParse3_9);

        if (transactionNombreResultat <= 0) return null;

        if (string.IsNullOrEmpty(transactionValeur) || string.IsNullOrEmpty(transactionPourcent))
            throw new NoNullAllowedException(Resource.ActifsAffecteChamps3_10_3_11Obligatoire);

        return VerifierNombrePourcentage(transactionNombre, transactionPourcent, Resource.ActifsAffecteChamps3_10_3_11Obligatoire, Resource.ActifsAffecteErreur3_9_3_10, Resource.ActifsAffecteChamps3_9_3_10_3_11Reussite);

    }

    /// <summary>
    /// Verifie la valeur de transaction et la convertie en float
    /// </summary>
    /// <param name="valTrans">Valeur Transaction</param>
    /// <param name="trans">NombrePourcentage de la transaction</param>
    /// <returns>Valeur transaction</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    internal static float? VerifierValeurTransaction(string? valTrans, NombrePourcentage? trans)
    {
        //Si le nombre de transactions est 0 ou null, la valeur de la transaction est donc null également
        if (trans is not { Nombre: > 0 }) return null;
        
        if (!float.TryParse(valTrans, out var valeurTrans))
            throw new FormatException(Resource.ActifsAffecteParse3_11);

        LoggerHelper.Instance.Log(Resource.ActifsAffecteChamp3_11Reussite, LogLevel.Information);

        return valeurTrans;
    }

    /// <summary>
    /// Permet de vérifier les estimations
    /// </summary>
    /// <param name="valeurs">Dictionaire de valeurs</param>
    /// <returns>Liste estimations</returns>
    /// <exception cref="NoNullAllowedException"></exception>
    /// <exception cref="FormatException"></exception>
    internal static IEnumerable<string> VerifierEstimations(Dictionary<string, IEnumerable<string>?>? valeurs)
    {
        if (valeurs == null)
            throw new NoNullAllowedException(Resource.ActifsAffecteChamp3_12Obligatoire);

        valeurs.TryGetValue(KeyWords.AffectedEstimations, out IEnumerable<string>? estimations);

        if (!(estimations?.Any() ?? false))
            throw new NoNullAllowedException(Resource.ActifsAffecteChamp3_12Obligatoire);

        if (!ValidationHelper.ValiderRegle(Regles![C3_12], estimations!))
            throw new FormatException(Resource.ActifsAffecteChamp3_12NonConforme);

        LoggerHelper.Instance.Log(Resource.ActifsAffecteChamp3_12Reussite, LogLevel.Information);
        return estimations!;
    }
}
