using ACPR.Domaine;
using ACPR.Helper;
using ACPR.Services;

namespace ACPRTests;
public class ConversionTest
{
    [Fact]
    public void ConversionExcelJsonTest()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsConversion.log");
        JsonHelper.LireJsonRegles(reglesJson);
        const string excel = "file1.xlsx";
        const string resultat = "save.json";
        const string schemaJson = "schema.json";

        Rapport rapport = new ModeleService(ExcelHelper.LireFichier(ExcelHelper.ChargerFichier(excel))).CreerRapport();
        JsonHelper.SauvegardeJson(resultat,rapport, JsonHelper.GetOptions());
        Assert.True(JsonHelper.ValidationJson(resultat, schemaJson, out _));
        
    }
}
