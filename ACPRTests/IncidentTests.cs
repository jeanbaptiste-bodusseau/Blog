using System.Data;
using ACPR.Domaine;
using ACPR.Helper;

namespace ACPRTests;
public class IncidentTests
{
    public IncidentTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsIncidents.log");
        JsonHelper.LireJsonRegles(reglesJson);        
    }

    [Fact]
    public void Incident_AutreInformationsNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentAutresInformation, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierAutresInformations(valeurs, KeyWords.RapportReclassement));
    }

    [Fact]
    public void Incident_AutreInformationsDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierAutresInformations(null, KeyWords.RapportReclassement));
    }

    [Fact]
    public void Incident_ReferenceIncidentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentReference, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierCode(valeurs));
    }

    [Fact]
    public void Incident_ReferenceDictionnaireNull_NoNullALlowed()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierCode(null));
    }

    [Fact]
    public void Incident_DateDetectionNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDateDetection, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDateDetection(valeurs));
    }

    [Fact]
    public void Incident_DateDetectionNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDateDetection, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => Incident.VerifierDateDetection(valeurs));
    }

    [Fact]
    public void Incident_DateDetectionDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDateDetection(null));
    }

    [Fact]
    public void Incident_DateClassificationNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDateClassification, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDateClassification(valeurs));
    }

    [Fact]
    public void Incident_DateClassificationNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDateClassification, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => Incident.VerifierDateClassification(valeurs));
    }

    [Fact]
    public void Incident_DateClassificationDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDateClassification(null));
    }

    [Fact]
    public void Incident_DescriptionNull_NoNullAllo()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDescription, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDescription(valeurs));
    }

    [Fact]
    public void Incident_DescriptionDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDescription(null));
    }

    [Fact]
    public void Incident_ContinueteAffairesNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentContinueteAffaires, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierContinuite(valeurs));
    }

    [Fact]
    public void Incident_ContinueteAffairesNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentContinueteAffaires, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => Incident.VerifierContinuite(valeurs));
    }

    [Fact]
    public void Incident_ContinueteDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierContinuite(null));
    }

    [Fact]
    public void Incident_OccurenceIncidentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentOccurence, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierOccurence(valeurs, KeyWords.RapportFinal));
    }

    [Fact]
    public void Incident_OccurenceIncidentNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentOccurence, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => Incident.VerifierOccurence(valeurs, KeyWords.RapportFinal));
    }

    [Fact]
    public void Incident_OccurenceDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierOccurence(null, KeyWords.RapportFinal));
    }

    [Fact]
    public void Incident_DureeIncidentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDuree, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDuree(valeurs, KeyWords.RapportFinal));
    }

    [Fact]
    public void Incident_DureeIncidentNonConforme_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDuree, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => Incident.VerifierDuree(valeurs, KeyWords.RapportFinal));
    }

    [Fact]
    public void Incident_DureeDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDuree(null, KeyWords.RapportFinal));
    }

    [Fact]
    public void Incident_OrigineIncidentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentOrigine, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierOrigine(valeurs));
    }

    [Fact]
    public void Incident_OrigineDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierOrigine(null));
    }

    [Fact]
    public void Incident_DecouverteIncidentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDecouverte, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDecouverte(valeurs));
    }

    [Fact]
    public void Incident_DecouverteIncidentNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentDecouverte, ["azerty"] }
        };

        Assert.Throws<FormatException>(() => Incident.VerifierDecouverte(valeurs));
    }

    [Fact]
    public void Incident_DecouverteDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Incident.VerifierDecouverte(null));
    }

    [Fact]
    public void IncidentReussite()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.IncidentReference, ["123AZE456"] },
            { KeyWords.IncidentDateDetection, ["2001-12-17T09:30:47Z"] },
            { KeyWords.IncidentDateClassification, ["2001-12-17T09:30:47.0Z"] },
            { KeyWords.IncidentDescription, ["azerty"] },
            { KeyWords.IncidentOrigine, ["qwerty"] },
            { KeyWords.IncidentDecouverte, ["it_security"] },
            { KeyWords.IncidentContinueteAffaires, ["yes"] },
            { KeyWords.IncidentAutresInformation, ["bepo"] },
            { KeyWords.IncidentOccurence, ["2001-12-17T09:30:47Z"] },
            { KeyWords.IncidentDuree, ["100:26:35"] }
        };

        var result = Incident.Creer(KeyWords.RapportIntermediaire, valeurs,null);
        Assert.NotNull(result);

        Assert.NotNull(result.CodeReferenceEntiteFinanciere);
        Assert.Equal("123AZE456",result.CodeReferenceEntiteFinanciere);

        Assert.NotNull(result.DateDetection);
        Assert.Equal(DateTime.Parse("2001-12-17T09:30:47Z").ToUniversalTime(), result.DateDetection);

        Assert.NotNull(result.DateClassification);
        Assert.Equal(DateTime.Parse("2001-12-17T09:30:47.0Z").ToUniversalTime(), result.DateClassification);

        Assert.NotNull(result.Description);
        Assert.Equal("azerty",result.Description);

        Assert.NotNull(result.OrigineIncident);
        Assert.Equal("qwerty",result.OrigineIncident);

        Assert.NotNull(result.DecouverteIncident);
        Assert.Equal("it_security",result.DecouverteIncident);

        Assert.True(result.ContinueteAffaires);

        Assert.NotNull(result.InformationAutres);
        Assert.Equal("bepo",result.InformationAutres);

        Assert.NotNull(result.OccurenceIncident);
        Assert.Equal(DateTime.Parse("2001-12-17T09:30:47Z").ToUniversalTime(),result.OccurenceIncident);

        Assert.NotNull(result.DureeIncident);
        Assert.Equal("100:26:35", result.DureeIncident);
    }
}
