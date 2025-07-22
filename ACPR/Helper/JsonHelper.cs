using ACPR.Domaine;
using ACPR.Ressources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ACPR.Helper;

public static class JsonHelper
{
    public static JsonSerializerOptions GetOptions() =>
        new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Ca sonne bizarre, mais ca permet d'enregistrer les caracteres speciaux (lettre accentuées, +, etc...) dans le JSON
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };
    

    /// <summary>
    /// Transforme un rapport en fichier json
    /// </summary>
    /// <param name="filename">le fichier json de destination</param>
    /// <param name="rapport">le rapport a transformer</param>
    public static void SauvegardeJson(string filename, Rapport rapport, JsonSerializerOptions options)
    {
        var rapportString = JsonSerializer.Serialize(rapport, options);
        File.WriteAllText(filename, rapportString);
        LoggerHelper.Instance.Log(Resource.JsonHelperSauvegardeReussite,LogLevel.Information);
    }


    /// <summary>
    /// Controle le json avec le schema donné
    /// </summary>
    /// <param name="cheminJson">chemin absolu du json a controler</param>
    /// <param name="cheminSchema">chemin absolu du schema json</param>
    /// <returns>true si json valide, false sinon</returns>
    public static bool ValidationJson(string cheminJson, string cheminSchema, out IList<ValidationError> erreurs)
    {
        LoggerHelper.Instance.Log(KeyWords.CategorieValidationJson, LogLevel.Information);
        JSchema schema = JSchema.Parse(File.ReadAllText(cheminSchema));
        JObject json = JObject.Parse(File.ReadAllText(cheminJson));

        return json.IsValid(schema, out erreurs);
    }

    /// <summary>
    /// Affiche les erreurs en cas de non-valildation du Json
    /// </summary>
    /// <param name="erreurs">liste d'erreurs</param>
    public static void AfficherErreurJson(IList<ValidationError> erreurs)
    {
        foreach (var erreur in erreurs)
        {
            LoggerHelper.Instance.Log(Resource.ErreurLog, LogLevel.Error);
            LoggerHelper.Instance.Log($"{erreur.ErrorType} : {erreur.Message}",LogLevel.Warning);
            LoggerHelper.Instance.Log($"Ligne : {erreur.LineNumber}",LogLevel.Warning);
            LoggerHelper.Instance.Log($"Position : {erreur.LinePosition}",LogLevel.Warning);
            LoggerHelper.Instance.Log($"Schema : {erreur.Schema.ToString().Replace("\n","\n\t")}",LogLevel.Warning);
            foreach (var erreurChildError in erreur.ChildErrors)
            {
                LoggerHelper.Instance.Log($"{erreurChildError.ErrorType} : {erreurChildError.Message}",LogLevel.Warning);
            }

        }
    }


    /// <summary>
    /// Lis le fichier json des valeurs possibles par champs et renvoie un objet dynamique contenant des listes de ces valeurs;
    /// </summary>
    /// <param name="cheminFichier"></param>
    /// <returns></returns>
    public static void LireJsonRegles(string cheminFichier)
    {
        var jsonContent = File.ReadAllText(cheminFichier);

        var regles = JsonConvert.DeserializeObject<ReglesTemporaire>(jsonContent);
        if (regles == null)
            throw new NoNullAllowedException(Resource.JsonHelperLectureJsonReglesErreur);
        ReglesJson.Regles = regles.regles;
        LoggerHelper.Instance.Log(Resource.JsonHelperLectureReglesReussite, LogLevel.Information);
    }
}
