using ACPR.Domaine;
using ACPR.Helper;
using System.Data;

namespace ACPRTests;

public class EvaluationImpactTests
{
    public EvaluationImpactTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsEvaluationImpact.log");
        JsonHelper.LireJsonRegles(reglesJson);

    }
    [Fact]
    public void EvaluationImpact_ClientNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentClient, null }
        };

        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierImpactClient(valeurs, true));
    }

    [Fact]
    public void EvaluationImpact_ClientDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierImpactClient(null, true));
    }

    [Fact]
    public void EvaluationImpact_ServicesCritiquesNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentService, null }
        };

        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierServicesCritiques(valeurs));
    }

    [Fact]
    public void EvaluationImpact_ServicesCritiquesDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierServicesCritiques(null));
    }

    [Fact]
    public void EvaluationImpact_ZonesNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentZone, null }
        };

        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierZones(valeurs));
    }

    [Fact]
    public void EvaluationImpact_ZoneDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierZones(null));
    }

    [Fact]
    public void EvaluationImpact_ChoixInfraNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentInfraChoice, null }
        };

        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierInfra(valeurs, out _));
    }

    [Fact]
    public void EvaluationImpact_ChoixInfraNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentInfraChoice, "bepo" }
        };

        Assert.Throws<FormatException>(() => EvaluationImpact.VerifierInfra( valeurs, out _));
    }

    [Fact]
    public void EvaluationImpact_InfraAffecteNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentInfraChoice, "yes" },
            { KeyWords.ImpactAssessmentInfra, null }
        };

        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierInfra(valeurs, out _));
    }

    [Fact]
    public void EvaluationImpact_InfraDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierInfra(null, out _));
    }

    [Fact]
    public void EvaluationImpact_InteretFinancierNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentInteretFinancier, null }
        };

        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierInteret(valeurs));
    }

    [Fact]
    public void EvaluationImpact_InteretDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => EvaluationImpact.VerifierInteret(null));
    }

    [Fact]
    public void EvaluationImpact_CreationEvaluationImpact_EvaluationImpactConforme()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ImpactAssessmentClient, "yes" },
            { KeyWords.ImpactAssessmentService, "azerty" },
            { KeyWords.ImpactAssessmentZone, "qwerty" },
            { KeyWords.ImpactAssessmentInfraChoice, "yes" },
            { KeyWords.ImpactAssessmentInfra, "mlkjhg" },
            { KeyWords.ImpactAssessmentInteretFinancier, "bepo" }
        };

        var result = EvaluationImpact.Creer(true, valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.ImpactClientImportant);
        Assert.True(result.ImpactClientImportant);

        Assert.NotNull(result.ServiceAffecte);
        Assert.Equal("azerty",result.ServiceAffecte);

        Assert.NotNull(result.ZoneAffecte);
        Assert.Equal("qwerty",result.ZoneAffecte);

        Assert.NotNull(result.ComposantInfraSontAffecte);
        Assert.Equal("yes",result.ComposantInfraSontAffecte);

        Assert.NotNull(result.ComposantInfraAffecte);
        Assert.Equal("mlkjhg",result.ComposantInfraAffecte);

        Assert.NotNull(result.ImpactInteretFinancier);
        Assert.Equal("bepo",result.ImpactInteretFinancier);
    }
}