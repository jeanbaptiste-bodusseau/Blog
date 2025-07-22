using ACPR.Helper;
using ACPR.Services;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Logging;
using System.Data;

namespace ACPRConsole
{
    public class AcprConsole
    {
        public static void Main(string[] args)
        {
            //Initialisation des fichiers de configuration
            var cheminFichierResultat = string.Empty;
            var cheminFichierSchema = AppContext.BaseDirectory + "schema.json";
            var jsonRegles = AppContext.BaseDirectory + "regles.json";
            try
            {
                //Initialisation des fichiers de lectures/écritures
                cheminFichierResultat = FichierHelper.CheminOuFichier(args[1]);
                if (string.IsNullOrEmpty(cheminFichierResultat))
                    throw new NoNullAllowedException("Veuillez indiquer l'endroit de sortie du fichier json");

                if (Path.GetExtension(cheminFichierResultat) != ".json")
                    throw new FileNotFoundException("Fichier non-conforme. Veuillez vérifier le format : " + cheminFichierResultat);
                LoggerHelper.Instance.InitialiserLog(FichierHelper.VerifierRepertoire(cheminFichierResultat) + "/logs.log");
                FichierHelper.VerifierFichier(args[0], ".xlsx");

                JsonHelper.LireJsonRegles(jsonRegles);
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.QuitterProgramme(ex);
            }

            var cheminFichierExcel = args[0];
            LoggerHelper.Instance.NettoyerLog();

            try
            {
                //Lecture données
                var workbook = ExcelHelper.ChargerFichier(cheminFichierExcel);
                var valeurs = ExcelHelper.LireFichier(workbook);

                //Creation Rapport
                var rapport = new ModeleService(valeurs).CreerRapport();

                //Ecriture résultat
                JsonHelper.SauvegardeJson(cheminFichierResultat!, rapport!, JsonHelper.GetOptions());

                if (JsonHelper.ValidationJson(cheminFichierResultat!, cheminFichierSchema, out var errors))
                {
                    LoggerHelper.Instance.Log("Json is valid\n",LogLevel.Information);
                }
                else
                {
                    LoggerHelper.Instance.Log("Json is invalid\n",LogLevel.Error);
                    JsonHelper.AfficherErreurJson(errors);
                }
            }
            //En cas d'erreur forçant a quitter le programme
            catch (Exception ex)
            {
                if (ex is IOException or UnauthorizedAccessException)
                    LoggerHelper.Instance.Log(
                        ex.Message != "Le fichier renseigné est déja utilisé par un autre processus"
                            ? "Une erreur est survenu lors de l'exécution du programmme. Veuillez réessayer"
                            : ex.Message, LogLevel.Error);
                else
                    LoggerHelper.Instance.QuitterProgramme(ex);
            }
        }
    }
}