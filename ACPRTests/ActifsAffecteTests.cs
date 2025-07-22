using ACPR.Helper;
using ACPR.Domaine;
using System.Data;
using ACPR.Ressources;

namespace ACPRTests;
public class ActifsAffecteTests
{
    public ActifsAffecteTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsActifsAffecte.log");
        JsonHelper.LireJsonRegles(reglesJson);        
    }

    [Fact]
    public void ActifsAffecte_VerifierNombrePourcentageNombreNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, null },
            { KeyWords.AffectedAssetsClientPourcent, ["3.6"]}
        };

        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsClientNombre, KeyWords.AffectedAssetsClientPourcent,Resource.ActifsAffecteChamps3_4_3_5Obligatoire, Resource.ActifsAffecteErreur3_4_3_5, Resource.ActifsAffecteChamps3_4_3_5Reussite));
    }

    [Fact]
    public void ActifsAffecte_VerifierNombrePourcentagePourcentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, ["5"] },
            { KeyWords.AffectedAssetsClientPourcent, null}
        };

        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsClientNombre, KeyWords.AffectedAssetsClientPourcent, Resource.ActifsAffecteChamps3_4_3_5Obligatoire, Resource.ActifsAffecteErreur3_4_3_5, Resource.ActifsAffecteChamps3_4_3_5Reussite));
    }

    [Fact]
    public void ActifsAffecte_VerifierNombrePourcentageMauvaisFormat_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, ["aze"] },
            { KeyWords.AffectedAssetsClientPourcent, ["rty"]}
        };
        Assert.Throws<FormatException>(() => ActifsAffecte.VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsClientNombre, KeyWords.AffectedAssetsClientPourcent, Resource.ActifsAffecteChamps3_4_3_5Obligatoire, Resource.ActifsAffecteErreur3_4_3_5, Resource.ActifsAffecteChamps3_4_3_5Reussite));
    }

    [Fact]
    public void ActifsAffecte_VerifierNombrePourcentageTropGrand_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, ["35"] },
            { KeyWords.AffectedAssetsClientPourcent, ["156"]}
        };
        Assert.Throws<FormatException>(() => ActifsAffecte.VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsClientNombre, KeyWords.AffectedAssetsClientPourcent, Resource.ActifsAffecteChamps3_4_3_5Obligatoire, Resource.ActifsAffecteErreur3_4_3_5, Resource.ActifsAffecteChamps3_4_3_5Reussite));
    }

    [Fact]
    public void ActifsAffecte_VerifierNombrePourcentage_NombrePourcentageConforme()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, ["35"] },
            { KeyWords.AffectedAssetsClientPourcent, ["15.6"] }
        };

        var result = ActifsAffecte.VerifierNombrePourcentage(valeurs, KeyWords.AffectedAssetsClientNombre,
            KeyWords.AffectedAssetsClientPourcent, "", "", "log");

        Assert.NotNull(result);
        Assert.Equal(35, result.Nombre);
        Assert.True(15.6f.EstEgal(result.Pourcentage));
    }

    [Fact]
    public void ActifsAffecte_VerifierNombrePourcentageDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierNombrePourcentage(null, "", "", "", "", ""));
    }

    [Fact]
    public void ActifsAffecte_VerifierTransactionsNombreNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsTransactionsNombre, ["3.2"]}
        };

        Assert.Throws<FormatException>(() => ActifsAffecte.VerifierTransactions(valeurs, out _));
    }

    [Fact]
    public void ActifsAffecte_VerifierTransactionsPourcentNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsTransactionsNombre, ["32"]},
            {KeyWords.AffectedAssetsTransactionsPourcent, null}
        };

        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierTransactions(valeurs, out _));
    }

    [Fact]
    public void ActifsAffecteVerifierTransactionsMauvaisFormat_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsTransactionsNombre, ["32"]},
             {KeyWords.AffectedAssetsTransactionsPourcent, ["126"]},
            { KeyWords.AffectedAssetsValue, ["azerty"]}
            };

        Assert.Throws<FormatException>(() => ActifsAffecte.VerifierTransactions(valeurs, out _));
    }

    [Fact]
    public void ActifsAffecte_VerifierTransactionsValeurNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsTransactionsNombre, ["32"]},
            { KeyWords.AffectedAssetsTransactionsPourcent, ["126"]},
            { KeyWords.AffectedAssetsValue, null}
        };

        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierTransactions(valeurs, out _));
    }

    [Fact]
    public void ActifsAffecte_VerifierTransactionsDictionnaireNull_Null()
    {
        Assert.Null(ActifsAffecte.VerifierTransactions(null, out _));
    }

    [Fact]
    public void ActifsAffecte_VerifierTransactionsConforme_NombrePourcentageConforme()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsTransactionsNombre, ["32"] },
            { KeyWords.AffectedAssetsTransactionsPourcent, ["12.3"] },
            { KeyWords.AffectedAssetsValue, ["24"] }
        };
        var result = ActifsAffecte.VerifierTransactions(valeurs, out var valeur);

        Assert.NotNull(result);

        Assert.Equal(32,result.Nombre);

        Assert.True(12.3f.EstEgal(result.Pourcentage));

        Assert.NotNull(valeur);
        Assert.Equal("24",valeur);
    }

    [Fact]
    public void ActifsAffecte_VerifierValeurTransactionNonConforme_FormatException()
    {
        NombrePourcentage.Creer("5", "2.6", out var transaction);
        const string valeur = "azerty";

        Assert.Throws<FormatException>(() => ActifsAffecte.VerifierValeurTransaction(valeur,transaction));
    }

    [Fact]
    public void ActifsAffecte_VerifierValeurTransactionConforme_Float()
    {
        NombrePourcentage.Creer("5", "2.6", out var transaction);
        const string valeur = "26,3";

        var result = ActifsAffecte.VerifierValeurTransaction(valeur, transaction);
        Assert.NotNull(result);

        Assert.Equal(26.3f,result);
    }

    [Fact]
    public void ActifsAffecte_VerifierEstimationsNull_NoNullAllowed()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedEstimations, null}
        };

        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierEstimations(valeurs));
    }

    [Fact]
    public void ActifsAffecte_VerifierEstimationsNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {

            { KeyWords.AffectedEstimations, ["azerty", "qwerty"]}
        };

        Assert.Throws<FormatException>(() => ActifsAffecte.VerifierEstimations(valeurs));
    }

    [Fact]
    public void ActifsAffecte_VerifierEstimationsDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierEstimations(null));
    }
    [Fact]
    public void ActifsAffecte_VerifierEstimationsConforme_EstimationsConforme()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedEstimations, ["no_impact_on_clients"] }
        };

        var result = ActifsAffecte.VerifierEstimations(valeurs);
        Assert.NotNull(result);
        Assert.Contains("no_impact_on_clients", result);
    }

    [Fact]
    public void ActifsAffecte_VerifierEstimationsDictionnaireNull()
    {
        Assert.Throws<NoNullAllowedException>(() => ActifsAffecte.VerifierEstimations(null));
    }

    [Fact]
    public void ActifsAffecte_ReussiteCreationActifsAffecte_ActifsAffecteNonNull()
    {
        var valeurs = new Dictionary<string, IEnumerable<string>?>
        {
            { KeyWords.AffectedAssetsClientNombre, ["35"] },
            { KeyWords.AffectedAssetsClientPourcent, ["6.3"]},
            { KeyWords.AffectedAssetsHomoNombre, ["35"] },
            { KeyWords.AffectedAssetsHomoPourcent, ["56"]},
            { KeyWords.AffectedAssetsTransactionsNombre, ["32"]},
            { KeyWords.AffectedAssetsTransactionsPourcent, ["3.2"]},
            { KeyWords.AffectedAssetsValue, ["3,2"]},
            { KeyWords.AffectedEstimations, ["actual_figures_for_clients_affected", "estimates_for_clients_affected"]}
        };

        var result = ActifsAffecte.Creer(valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.ClientAffectes);
        Assert.Equal(35, result.ClientAffectes.Nombre );
        Assert.True(6.3f.EstEgal(result.ClientAffectes.Pourcentage));

        Assert.NotNull(result.HomologueFinancier);
        Assert.Equal(35,result.HomologueFinancier.Nombre);
        Assert.True(56f.EstEgal(result.HomologueFinancier.Pourcentage));

        Assert.NotNull(result.Transactions);
        Assert.Equal(32, result.Transactions.Nombre);
        Assert.True(3.2f.EstEgal(result.Transactions.Pourcentage));

        Assert.NotNull(result.ValeurTransaction);
        Assert.True(3.2f.EstEgal(result.ValeurTransaction));

        Assert.NotNull(result.EstimationsNombres);
        Assert.True(result.EstimationsNombres.EstEgal(["actual_figures_for_clients_affected", "estimates_for_clients_affected"]));
    }
}
