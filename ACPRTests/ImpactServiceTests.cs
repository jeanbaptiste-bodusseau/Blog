using ACPR.Domaine;
using ACPR.Helper;
using System.Data;

namespace ACPRTests;

public class ImpactServiceTests
{
    public ImpactServiceTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsImpactService.log");
        JsonHelper.LireJsonRegles(reglesJson);        
    }

    [Fact]
    public void ImpactService_DureePanneNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactPanneService, null }
        };

        Assert.Throws<FormatException>(() => ServiceImpact.VerifierPanne(valeurs));
        
    }

    [Fact]
    public void ImpactService_DureePanneNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactPanneService, "azerty" }
        };

        Assert.Throws<FormatException>(() => ServiceImpact.VerifierPanne(valeurs));
        
    }

    [Fact]
    public void ImpactService_DureePanneDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => ServiceImpact.VerifierPanne(null));
    }

    [Fact]
    public void ImpactService_ActionTemporaireNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactActionTemporaire, null }
        };

        Assert.Throws<NoNullAllowedException>(() => ServiceImpact.VerifierActions(valeurs, out _));
    }

    [Fact]
    public void ImpactService_ActionTemporaireNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactActionTemporaire, "azerty" }
        };

        Assert.Throws<FormatException>(() => ServiceImpact.VerifierActions(valeurs, out _));
    }

    [Fact]
    public void ImpactService_DescriptionActionNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactActionTemporaire, "yes" },
            { KeyWords.ServiceImpactDescriptionAction, null }
        };

        Assert.Throws<NoNullAllowedException>(() => ServiceImpact.VerifierActions(valeurs, out _));
    }

    [Fact]
    public void ImpactService_ActionDictionnaireNull_NoNullALlowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => ServiceImpact.VerifierActions(null, out _));
    }

    [Fact]
    public void ImpactService_DateNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactDateRestauration, null }
        };

        Assert.Throws<NoNullAllowedException>(() => ServiceImpact.VerifierDate(valeurs));
    }

    [Fact]
    public void ImpactService_DateNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactDateRestauration, "qwerty" }
        };

        Assert.Throws<FormatException>(() => ServiceImpact.VerifierDate(valeurs));
    }

    [Fact]
    public void ImpactService_DateDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => ServiceImpact.VerifierDate(null));
    }

    [Fact]
    public void ImpactService_CreationImpactService_ImpactServiceConforme()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ServiceImpactPanneService, "000:55:55" },
            { KeyWords.ServiceImpactActionTemporaire, "yes" },
            { KeyWords.ServiceImpactDescriptionAction, "azerty" },
            { KeyWords.ServiceImpactDateRestauration, "2025-04-28T10:49:00Z" }
        };

        var result = ServiceImpact.Creer(valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.PanneService);
        Assert.Equal("00:55:55",result.PanneService);

        Assert.NotNull(result.ActionTemporairePourRecuperation);
        Assert.True(result.ActionTemporairePourRecuperation);

        Assert.NotNull(result.DescriptionActionTemporaire);
        Assert.Equal("azerty", result.DescriptionActionTemporaire);

        Assert.NotNull(result.RestaurationServiceDate);
        Assert.Equal(DateTime.Parse("2025-04-28T10:49:00Z").ToUniversalTime(), result.RestaurationServiceDate);
    }
}