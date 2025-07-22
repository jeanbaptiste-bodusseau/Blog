using ACPR.Domaine;
using ACPR.Helper;
using System.Data;

namespace ACPRTests;
public class TypeIncidentTests
{
    public TypeIncidentTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsTypeIncident.log");
        JsonHelper.LireJsonRegles(reglesJson);
    }

    [Fact]
    public void TypeIncident_ClassificationNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeClassification, null }
        };

        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierClassification(valeurs));
    }

    [Fact]
    public void TypeIncident_ClassificationNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeClassification, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => TypeIncident.VerifierClassification(valeurs));
    }

    [Fact]
    public void TypeIncident_ClassificationDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierClassification(null));
    }

    [Fact]
    public void TypeIncident_CompromisCyberSecuriteNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeCompromis, null}
        };

        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierCyberSecurity(valeurs, out _));
    }

    [Fact]
    public void TypeIncident_TypeMenacesNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeCompromis, ["azerty"]},
            { KeyWords.IncidentTypeMenace, null}
        };

        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierCyberSecurity(valeurs, out _));
    }

    [Fact]
    public void TypeIncident_TypeMenacesNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeCompromis, ["azerty"]},
            { KeyWords.IncidentTypeMenace, ["qwerty"]}
        };

        Assert.Throws<FormatException>(() => TypeIncident.VerifierCyberSecurity(valeurs, out _));
    }

    [Fact]
    public void TypeIncident_CyberSecuriteDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierCyberSecurity(null, out _));
    }

    [Fact]
    public void TypeIncident_AutreClassificationNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeClassification, ["other"] },
            { KeyWords.IncidentTypeAutreClassification, null}
        };

        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierAutreClassification(valeurs));
    }

    [Fact]
    public void TypeIncident_AutreClassificationDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierAutreClassification(null));
    }

    [Fact]
    public void TypeIncident_AutreMenacesNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeAutreMenace, null}
        };

        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierAutresMenaces(valeurs));
    }

    [Fact]
    public void TypeIncident_AutreMenacesDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => TypeIncident.VerifierAutresMenaces(null));
    }

    [Fact]
    public void TypeIncident_CreationTypeIncident_TypeIncidentConforme()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentTypeClassification, ["cybersecurity-related"] },
            { KeyWords.IncidentTypeCompromis, ["azerty"]},
            { KeyWords.IncidentTypeMenace, ["other_(please_specify)"]},
            { KeyWords.IncidentTypeAutreMenace, ["qwerty"]}
        };

        var result = TypeIncident.Creer(valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.ClassificationIncident);
        Assert.True(result.ClassificationIncident.EstEgal(["cybersecurity-related"]));

        Assert.NotNull(result.IndicateurCompromis);
        Assert.Equal("azerty", result.IndicateurCompromis);

        Assert.NotNull(result.TechniqueMenace);
        Assert.True(result.TechniqueMenace.EstEgal(["other_(please_specify)"]));

        Assert.NotNull(result.AutreTechniqueMenace);
        Assert.Equal("qwerty",result.AutreTechniqueMenace);
    }
}

