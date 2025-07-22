using ACPR.Helper;
using ACPR.Domaine;
using System.Data;

namespace ACPRTests;
public class ClassificationTests
{
    public ClassificationTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsClassification.log");
        JsonHelper.LireJsonRegles(reglesJson);       
    }

    [Fact]
    public void Classification_CritereClassificationNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, null }
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierCritere(valeurs));
    }

    [Fact]
    public void Classification_CritereClassificationNonConforme_FormatException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["azerty", "qwerty"] }
        };

        Assert.Throws<FormatException>(() => Classification.VerifierCritere(valeurs));
    }

    [Fact]
    public void Classification_VerifierCritereDictionnaireNull_NoNullAllowed()
    {
        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierCritere(null));
    }

    [Fact]
    public void Classification_CodePaysNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["geographical_spread"] },
            { KeyWords.ClassificationCodePays, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierCodePays(valeurs));
    }

    [Fact]
    public void Classification_CodePaysNonConforme_FormatException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["geographical_spread"] },
            { KeyWords.ClassificationCodePays, ["AZE"]}
        };

        Assert.Throws<FormatException>(() => Classification.VerifierCodePays(valeurs));
    }

    [Fact]
    public void Classification_CodePaysDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierCodePays(null));
    }

    [Fact]
    public void Classification_TypeImpactEtatsNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["geographical_spread"] },
            { KeyWords.ClassificationCodePays, ["ES"]},
            { KeyWords.ClassificationTypeImpactEtats, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierGeographicalSpread(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_DescriptionImpactEtatsNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["geographical_spread"] },
            { KeyWords.ClassificationCodePays, ["ES"]},
            { KeyWords.ClassificationTypeImpactEtats, ["clients"]},
            { KeyWords.ClassificationDescriptionImpactEtats, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierGeographicalSpread( valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_TypeImpactEtatsNonConforme_FormatException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["geographical_spread"] },
            { KeyWords.ClassificationCodePays, ["ES"]},
            { KeyWords.ClassificationTypeImpactEtats, ["azerty"]},
            { KeyWords.ClassificationDescriptionImpactEtats, ["qwerty"]}
        };

        Assert.Throws<FormatException>(() => Classification.VerifierGeographicalSpread( valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_GeographicalSpreadDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierGeographicalSpread(null, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_TypePerteDonneesNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["data_losses"] },
            { KeyWords.ClassificationTypePerteDonnées, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierDataLoss(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_DescriptionPerteDonneeNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["data_losses"] },
            { KeyWords.ClassificationTypePerteDonnées, ["availability"]},
            { KeyWords.ClassificationDescriptionPerteDonnées, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierDataLoss(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_TypePerteDonneesNonConforme_FormatException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["data_losses"] },
            { KeyWords.ClassificationTypePerteDonnées, ["azerty"]},
            { KeyWords.ClassificationDescriptionPerteDonnées, ["qwerty"]}
        };

        Assert.Throws<FormatException>(() => Classification.VerifierDataLoss(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_DataLossDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierDataLoss(null, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_TypeImpactReputationNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["reputational_impact"] },
            { KeyWords.ClassificationTypeImpactReputation, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierImpactReputation(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_DescriptionImpactReputationNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["reputational_impact"] },
            { KeyWords.ClassificationTypeImpactReputation, ["the_major_ict-related_incident_has_been_reflected_in_the_media"]},
            { KeyWords.ClassificationDescriptionImpactReputation, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierImpactReputation(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_TypeImpactReputationNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["reputational_impact"] },
            { KeyWords.ClassificationTypeImpactReputation, ["azerty"]},
            { KeyWords.ClassificationDescriptionImpactReputation, ["qwerty"]}
        };

        Assert.Throws<FormatException>(() => Classification.VerifierImpactReputation(valeurs, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_ImpactReputationDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierImpactReputation(null, KeyWords.RapportIntermediaire));
    }

    [Fact]
    public void Classification_SeuilMaterialiteNull_NoNullAllowedException()
    {
        
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationSeuilImpactEconomique, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Classification.VerifierSeuilMaterialiteImpact(valeurs));
    }

    [Fact]
    public void Classification_CreationConformeGeographicalSpread_ClassificationValide()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["geographical_spread"] },
            { KeyWords.ClassificationCodePays, ["ES"]},
            { KeyWords.ClassificationTypeImpactEtats, ["clients"]},
            { KeyWords.ClassificationDescriptionImpactEtats, ["qwerty"]},
            { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"]}
        };

        var result = Classification.Creer(KeyWords.RapportFinal, valeurs);

        Assert.NotNull(result);
        Assert.NotNull(result.CritereClassification);
        Assert.Equal("geographical_spread", result.CritereClassification);

        Assert.NotNull(result.CodePays);
        Assert.True(result.CodePays.EstEgal(["ES"]));

        Assert.NotNull(result.TypesImpactEtatsMembres);
        Assert.True(result.TypesImpactEtatsMembres.EstEgal(["clients"]));

        Assert.NotNull(result.DescriptionTypesImpactEtatsMembres);
        Assert.Equal("qwerty",result.DescriptionTypesImpactEtatsMembres);

        Assert.NotNull(result.SeuilMaterialiteImpactEconomique);
        Assert.Equal("seuil",result.SeuilMaterialiteImpactEconomique);
    }

    [Fact]
    public void Classification_CreationConformeDataLoss_ClassificationValide()
    {

        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["data_losses"] },
            { KeyWords.ClassificationTypePerteDonnées, ["authenticity"]},
            { KeyWords.ClassificationDescriptionPerteDonnées, ["azerty"]},
            { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"]}
        };

        var result = Classification.Creer(KeyWords.RapportFinal, valeurs);

        Assert.NotNull(result);
        Assert.NotNull(result.CritereClassification);
        Assert.Equal("data_losses", result.CritereClassification);

        Assert.NotNull(result.SeuilsMaterialitePertesDonnees);
        Assert.True(result.SeuilsMaterialitePertesDonnees.EstEgal(["authenticity"]));

        Assert.NotNull(result.DescriptionPerteDonnees);
        Assert.Equal("azerty",result.DescriptionPerteDonnees);

        Assert.NotNull(result.SeuilMaterialiteImpactEconomique);
        Assert.Equal("seuil",result.SeuilMaterialiteImpactEconomique);
    }

    [Fact]
    public void Classification_CreationConformeReputationImpact_ClassificationValide()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["reputational_impact"] },
            { KeyWords.ClassificationTypeImpactReputation, ["the_major_ict-related_incident_has_been_reflected_in_the_media"]},
            { KeyWords.ClassificationDescriptionImpactReputation, ["azerty"]},
            { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"]}
        };

        var result = Classification.Creer(KeyWords.RapportFinal, valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.CritereClassification);
        Assert.Equal("reputational_impact", result.CritereClassification);

        Assert.NotNull(result.TypeImpactReputation);
        Assert.True(result.TypeImpactReputation.EstEgal(["the_major_ict-related_incident_has_been_reflected_in_the_media"]));

        Assert.NotNull(result.DescriptionImpactReputation);
        Assert.Equal("azerty", result.DescriptionImpactReputation);

        Assert.NotNull(result.SeuilMaterialiteImpactEconomique);
        Assert.Equal("seuil", result.SeuilMaterialiteImpactEconomique);
    }

    [Fact]
    public void Classification_CreationConformeAutreClassification_ClassificationValide()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.ClassificationCritere, ["economic_impact"] }
        };

        var result = Classification.Creer(KeyWords.RapportInitial, valeurs);

        Assert.NotNull(result);
        Assert.NotNull(result.CritereClassification);
        Assert.Equal("economic_impact", result.CritereClassification);
    }

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursGeographicalSpread = new()
    {
        { KeyWords.ClassificationCritere, ["geographical_spread"] },
        { KeyWords.ClassificationCodePays, ["ES"]},
        { KeyWords.ClassificationTypeImpactEtats, ["clients"]},
        { KeyWords.ClassificationDescriptionImpactEtats, ["qwerty"]},
        { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"]}
    };

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursPerteDonnees = new()
    {
        { KeyWords.ClassificationCritere, ["data_losses"] },
        { KeyWords.ClassificationTypePerteDonnées, ["authenticity"] },
        { KeyWords.ClassificationDescriptionPerteDonnées, ["azerty"] },
        { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"] }
    };

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursImpactReputation = new()
    {
        { KeyWords.ClassificationCritere, ["reputational_impact"]},
        { KeyWords.ClassificationTypeImpactReputation, ["the_major_ict-related_incident_has_been_reflected_in_the_media"]},
        { KeyWords.ClassificationDescriptionImpactReputation, ["azerty"]},
        { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"]}
    };

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursClassification = new()
    {
        { KeyWords.ClassificationCritere, ["economic_impact"] },
        { KeyWords.ClassificationSeuilImpactEconomique, ["seuil"]}
    };

    public static readonly IEnumerable<object[]> ListeValeurs =
    [
        [ValeursGeographicalSpread],
        [ValeursPerteDonnees],
        [ValeursImpactReputation],
        [ValeursClassification]
    ];

    [Theory]
    [MemberData(nameof(ListeValeurs))]
    public void Classification_CreationClassification_ClassificationValide(Dictionary<string, IEnumerable<string>?> valeurs)
    {
        var result = Classification.Creer(KeyWords.RapportFinal, valeurs);
        Assert.NotNull(result);
    }
}
