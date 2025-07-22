using System.Data;
using ACPR.Domaine;
using ACPR.Helper;


namespace ACPRTests;

public class EntiteTest
{

    public EntiteTest()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsEntite.log");
        JsonHelper.LireJsonRegles(reglesJson);
    }

    [Fact]
    public void Entite_TypeEntiteNonConforme_FormatException()
    {
        const string typeEntite = "EntitetNonConforme";
        
        Assert.Throws<FormatException>(() => Entite.Creer(typeEntite, []));
    }

    [Fact]
    public void Entite_TypeEntiteNull_NoNullAllowed()
    {
        Assert.Throws<NoNullAllowedException>(() => Entite.Creer(null, null));
    }

    [Fact]
    public void Entite_NomEntiteNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.EntiteNom, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierNom(valeurs));
    }

    [Fact]
    public void Entite_NomEntiteDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierNom(null));
    }

    [Fact]
    public void Entite_LeiEntiteNull_NoNullAllowedException()
    {
        const string typeEntite = KeyWords.EntiteParent;
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.EntiteLei, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierLei(valeurs, typeEntite, out _));
    }

    [Fact]
    public void Entite_CodeEUIDEntiteNull_NoNullAllowedException()
    {
        const string typeEntite = KeyWords.EntiteSoumettant;
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.EntiteLei, null },
            { KeyWords.EntiteCode, null },
        };

        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierLei(valeurs, typeEntite, out _));
    }

    [Fact]
    public void Entite_LeiEntiteNonConforme_FormatException()
    {
        const string typeEntite = KeyWords.EntiteAffecte;
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.EntiteLei, ["123456789"]}
        };

        Assert.Throws<FormatException>(() => Entite.VerifierLei(valeurs, typeEntite, out _));
    }

    [Fact]
    public void Entite_LeiDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierLei(null, KeyWords.EntiteSoumettant, out _));
    }

    [Fact]
    public void Entite_TypeEntiteAffecteNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            {KeyWords.EntiteTypeEntiteAffecte, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierTypeEntiteAffecte(valeurs));
    }

    [Fact]
    public void Entite_TypeEntiteAffecteNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            {KeyWords.EntiteTypeEntiteAffecte, [ "azerty", "qwerty"]}
        };

        Assert.Throws<FormatException>(() => Entite.VerifierTypeEntiteAffecte(valeurs));
    }

    [Fact]
    public void Entite_TypeAffecteDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Entite.VerifierTypeEntiteAffecte(null));
    }

    [Fact]
    public void Entite_CreationEntite_EntiteConforme()
    {
        const string typeEntite = KeyWords.EntiteAffecte;
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.EntiteNom, [ "testNomEntite" ] },
            { KeyWords.EntiteLei, ["123AZE456RTY789YUI00"] },
            { KeyWords.EntiteTypeEntiteAffecte, [ "credit_institution", "central_securities_depository" ] }
        };

        var result = Entite.Creer(typeEntite, valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.TypeEntite);
        Assert.Equal(KeyWords.EntiteAffecte,result.TypeEntite);

        Assert.NotNull(result.Nom);
        Assert.Equal("testNomEntite", result.Nom);

        Assert.NotNull(result.Lei);
        Assert.Equal("123AZE456RTY789YUI00", result.Lei);

        Assert.Null(result.Code);

        Assert.NotNull(result.TypeEntiteAffecte);
        Assert.True(result.TypeEntiteAffecte.EstEgal(["credit_institution", "central_securities_depository"]));
    }

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursEntiteSoumettant = new()
    {
        { KeyWords.EntiteNom, ["EntiteSoumettant"] },
        { KeyWords.EntiteLei, ["123AZE456RTY789YUI00"] },
        { KeyWords.EntiteTypeEntiteAffecte, ["credit_institution", "central_securities_depository"] }
    };

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursEntiteSoumettantLeiNull = new()
    {
        { KeyWords.EntiteNom, ["EntiteSoumettant"] },
        { KeyWords.EntiteCode, ["123AZE456RTY789YUI00"] },
        { KeyWords.EntiteTypeEntiteAffecte, ["credit_institution", "central_securities_depository"] }
    };

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursEntiteAffecte = new()
    {
        { KeyWords.EntiteNom, ["EntiteAffecte"] },
        { KeyWords.EntiteLei, ["123AZE456RTY789YUI00"] },
        { KeyWords.EntiteTypeEntiteAffecte, ["credit_institution", "central_securities_depository"] }
    };

    private static readonly Dictionary<string, IEnumerable<string>?> ValeursEntiteParent = new()
    {
        { KeyWords.EntiteNom, ["EntiteParent"] },
        { KeyWords.EntiteLei, ["123AZE456RTY789YUI00"] },
        { KeyWords.EntiteTypeEntiteAffecte, ["credit_institution", "central_securities_depository"] }
    };

    public static readonly IEnumerable<object[]> ListeValeurs =
    [
        [KeyWords.EntiteSoumettant, ValeursEntiteSoumettant],
        [KeyWords.EntiteSoumettant, ValeursEntiteSoumettantLeiNull],
        [KeyWords.EntiteAffecte, ValeursEntiteAffecte],
        [KeyWords.EntiteParent, ValeursEntiteParent]
    ];

    [Theory]
    [MemberData(nameof(ListeValeurs))]
    public void Entite_CreationEntite_EntiteValide(string typeEntite, Dictionary<string, IEnumerable<string>?> valeurs)
    {
        var result = Entite.Creer(typeEntite, valeurs);
        Assert.NotNull(result);

    }
}