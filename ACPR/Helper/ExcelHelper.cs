using ACPR.Domaine;
using ACPR.Ressources;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using static ACPR.Domaine.ReglesJson;
namespace ACPR.Helper;

public static class ExcelHelper
{
    public static Dictionary<string, IEnumerable<string>> Valeurs { get; private set; } = [];

    /// <summary>
    /// Récupere les informations du fichier excel
    /// </summary>
    /// <param name="cheminFichier">chemin absolu du fichier excel a lire</param>
    /// <returns>Rapport</returns>
    /// <exception cref="Exception">en cas d'erreur de lecture</exception>
    public static XLWorkbook ChargerFichier(string cheminFichier)
    {
        //ouverture excel
        try
        {
            var contenuExcel = new XLWorkbook(cheminFichier);
            LoggerHelper.Instance.Log(KeyWords.CategorieLectureExcel, LogLevel.Information);
            LoggerHelper.Instance.Log(Resource.ExcelHelperOuvertureReussite, LogLevel.Information);
            return contenuExcel;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.GetType());
            switch (ex)
            {
                case FileNotFoundException _:
                    throw new FileNotFoundException(Resource.ExcelHelperFichierNonTrouve + cheminFichier);
                case IOException _:
                    throw new IOException(Resource.ExcelHelperFichierDejaUtilise);
                default:
                    Console.WriteLine(ex.GetType());
                    throw new Exception(Resource.ExcelHelperErreurInatendu + ex.Message);
            }
        }
    }

    /// <summary>
    /// Lis le contenu du fichier excel
    /// </summary>
    /// <param name="contenuExcel">fichier excel</param>
    /// <returns>Rapport</returns>
    public static Dictionary<string, IEnumerable<string>> LireFichier(XLWorkbook contenuExcel)
    {
        Valeurs.Clear();
        //page 1
        var page1 = contenuExcel.Worksheet(2);
        Valeurs.Add(page1.Column(1).Cell(2).GetValue<string>().FormatterChamps(), [page1.Column(1).Cell(4).GetValue<string>().NettoyerString()]);
        LoggerHelper.Instance.Log(Resource.ExcelHelperExtractionReussite + page1.Name, LogLevel.Information);

        //page 2
        var page2 = contenuExcel.Worksheet(3);
        RecupererValeursFeuille(page2);
        LoggerHelper.Instance.Log(Resource.ExcelHelperExtractionReussite + page2.Name, LogLevel.Information);

        //page 3
        var page3 = contenuExcel.Worksheet(4);
        RecupererValeursFeuille(page3);
        LoggerHelper.Instance.Log(Resource.ExcelHelperExtractionReussite + page3.Name, LogLevel.Information);

        var page4 = contenuExcel.Worksheet(5);
        RecupererValeursFeuille(page4);
        LoggerHelper.Instance.Log(Resource.ExcelHelperExtractionReussite + page4.Name, LogLevel.Information);

        return Valeurs;
    }

    /// <summary>
    /// Recupere toutes les Valeurs d'une feuille excel
    /// </summary>
    /// <param name="page">feuille excel</param>
    public static void RecupererValeursFeuille(IXLWorksheet page)
    {
        var passeIndex = 2;
        var valeur = new List<IXLCell>();
        Valeurs.Add(page.Column(1).Cell(2).GetValue<string>().FormatterChamps(), [page.Column(1).Cell(4).GetValue<string>()]);
        foreach (var colonne in page.ColumnsUsed().Skip(1))
        {
            //Pour la premier case (colonne A), en A2 se trouve le nom du rapport. Ce qui n'est pas interessant, donc uniquement pour celle colonne je rajoute 1 
            // Afin d'acceder directement aux données voulu.
            if (!string.IsNullOrEmpty(colonne.Cell(1).GetValue<string>())) 
                passeIndex++;

            //Je skip les 2 premiere cellule par colonne. La 1ere est une cellule vide, la 2eme est le nom de la valeur. Pas utile puisque qu'on prefere la clé
            var cle = colonne.Cell(2).GetValue<string>().FormatterChamps();
            if (cle == C2_6)
                valeur = [.. colonne.Cells(4,20)]; //L'intervalle 4 a 20 est arbitraire. Je ne peux pas recuperer toutes les cellules utilisé sans recuperer des valeurs inutiles
            else 
                valeur = [.. colonne.CellsUsed().Skip(passeIndex)];

            Valeurs.Add(cle, valeur.RecupererValeurListCellule(Regles![KeyWords.ChampAFormatter].Contains(cle)));

            if (!string.IsNullOrEmpty(colonne.Cell(1).GetValue<string>())) 
                passeIndex--;
        }
    }
}