using System.Globalization;
using ACPR.Helper;

namespace ACPRTests;

public class HelperTests
{

    public HelperTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsHelper.log");
        JsonHelper.LireJsonRegles(reglesJson);
    }

    [Fact]
    public void ErreurHelper_VerifierDureeNull_False()
    {
        const string valeur = "";
        Assert.False(ValidationHelper.VerifierDureeNull(valeur, out _, out _));
    }

    [Fact]
    public void ErreurHelper_VerifierDureeNull_TropGrande()
    {
        const string valeur = "456:25:59:65";
        Assert.False(ValidationHelper.VerifierDureeNull(valeur, out _, out var err));
        Assert.Equal("pas assez ou trop d'element", err);
    }

    [Fact]
    public void ErreurHelper_VerifiDureeNull_NonConforme()
    {
        const string valeur = "456:mlk:59";
        Assert.False(ValidationHelper.VerifierDureeNull(valeur, out _, out var err));
        Assert.Equal("erreur lors du parse", err);
    }

    [Fact]
    public void ErreurrHelper_VerifierDureeNull_True()
    {
        const string valeur = "456:25:46";
        Assert.True(ValidationHelper.VerifierDureeNull(valeur, out var res, out _));
        Assert.Equal([456, 25, 46], res);
    }

    [Fact]
    public void ExtensionsHelper_NettoyerString_Reussite()
    {
        const string valeur = "Hello, /there";
        Assert.Equal("hello__there", valeur.NettoyerString());
    }

    [Fact]
    public void ExtensionsHelper_FormatterChamps_Reussite()
    {
        const string valeur = "4,56";
        Assert.Equal("4.56", valeur.FormatterChamps());
    }

    [Fact]
    public void ExtensionsHelper_ListeEstEgal_True()
    {
        var liste1 = new List<string> { "azerty", "qwerty" };
        var liste2 = new List<string> { "azerty", "qwerty" };
        Assert.True(liste1.EstEgal(liste2));
    }

    [Fact]
    public void ExtensionsHelper_ListeEstEgal_False()
    {
        var liste1 = new List<string> { "azerty", "qwerty" };
        var liste2 = new List<string> { "qwerty", "azerty" };
        Assert.False(liste1.EstEgal(liste2));
    }

    [Fact]
    public void ExtensionsHelper_FloatEstEgal_True()
    {
        var a = 0.6f;
        var b = 0.6f;
        Assert.True(a.EstEgal(b));
    }

    [Fact]
    public void ExtensionsHelper_FloatEstEgal_False()
    {
        var a = 0.6f;
        var b = 0.7f;
        Assert.False(a.EstEgal(b));
    }

    [Fact]
    public void FichierHelper_VerifierFichier_FichierNonTrouve()
    {
        const string chemin = "notfound.txt";
        Assert.Throws<FileNotFoundException>(() => FichierHelper.VerifierFichier(chemin, ".txt"));
    }

    [Fact]
    public void FichierHelper_VerifierFichier_ExtensionNonTrouve()
    {
        const string chemin = "found.json";
        Assert.Throws<FileNotFoundException>(() => FichierHelper.VerifierFichier(chemin, ".mp4"));
    }

    [Fact]
    public void FichierHelper_VerifierFichier_FichierPresent()
    {
        const string chemin = "found.json";
        FichierHelper.VerifierFichier(chemin, ".json");
    }
    [Fact]
    public void FichierHelper_VerifierRepertoire_Reussite()
    {
        const string chemin = "found.json";
        Assert.Equal(Path.GetDirectoryName(AppContext.BaseDirectory), FichierHelper.VerifierRepertoire(chemin));
    }

    [Fact]
    public void JsonHelper_ValidationJson_False()
    {
        const string chemin = "found.json";
        const string schema = "schema.json";
        Assert.False(JsonHelper.ValidationJson(chemin,schema, out _));
    }

    [Fact]
    public void ValidationHelper_ValiderLei_False()
    {
        const string lei = "azerty";
        Assert.False(ValidationHelper.ValiderLei(lei));
    }

    [Fact]
    public void ValidationHelper_ValiderLei_True()
    {
        const string lei = "123AZE456RTY789YUI00";
        Assert.True(ValidationHelper.ValiderLei(lei));
    }

    [Fact]
    public void ValidationHelper_ValiderEmail_False()
    {
        const string mail = "johndoe.com";
        Assert.False(ValidationHelper.ValiderEmail(mail));
    }

    [Fact]
    public void ValidationHelper_ValiderEmail_True()
    {
        const string mail = "johndoe@email.com";
        Assert.True(ValidationHelper.ValiderEmail(mail));
    }

    [Fact]
    public void ValidationHelper_ValiderTelephone_False()
    {
        const string tel = "9587643216";
        Assert.False(ValidationHelper.ValiderTelephone(tel));
    }

    [Fact]
    public void ValidationHelper_ValiderTelephon_True()
    {

        const string tel = "+9587643216";
        Assert.True(ValidationHelper.ValiderTelephone(tel));
    }

    [Fact]
    public void ValidationHelper_ValiderDate_False()
    {
        const string date = "aujourd'hui";
        Assert.False(ValidationHelper.ValiderDate(date, out _));
    }

    [Fact]
    public void ValidationHelper_ValiderDate_True()
    {
        var date = DateTime.Now.ToUniversalTime().ToString(CultureInfo.InvariantCulture);
        Assert.True(ValidationHelper.ValiderDate(date, out _));
    }
}