using ACPR.Helper;
using System.Data;
using ACPR.Domaine;

namespace ACPRTests;
public class ContactTests
{
    public ContactTests()
    {
        const string reglesJson = "regles.json";
        LoggerHelper.Instance.InitialiserLog("logs/logsContacts.log");
        JsonHelper.LireJsonRegles(reglesJson);
    }

    [Fact]
    public void Contact_SecondContactNull_Null()
    {
        const string typeContact = KeyWords.ContactSecond;

        Assert.Null(Contact.Creer(typeContact, []));
    }

    [Fact]
    public void Contact_PremierContactNull_NoNullAllowedException()
    {
        const string typeContact = KeyWords.ContactPremier;

        Assert.Throws<NoNullAllowedException>(() => Contact.Creer(typeContact, []));
    }

    [Fact]
    public void Contact_NomContactNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ContactNom, null },
        };

        Assert.Throws<NoNullAllowedException>(() => Contact.VerifierNom(valeurs));
    }

    [Fact]
    public void Contact_NomDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Contact.VerifierNom(null));
    }

    [Fact]
    public void Contact_MailContactNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ContactMail, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Contact.VerifierEmail(valeurs));
    }

    [Fact]
    public void Contact_MailContactNonConforme_FormatException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ContactMail, "azerty169"}
        };

        Assert.Throws<FormatException>(() => Contact.VerifierEmail(valeurs));
    }

    [Fact]
    public void Contact_MailContactDictionnaireNull_NoNullAllowed()
    {
        Assert.Throws<NoNullAllowedException>(() => Contact.VerifierEmail(null));
    }

    [Fact]
    public void Contact_TelContactNull_NoNullAllowedException()
    {
        var valeurs = new Dictionary<string, string?>
        {
            {KeyWords.ContactTelephone, null}
        };

        Assert.Throws<NoNullAllowedException>(() => Contact.VerifierTel(valeurs));
    }

    [Fact]
    public void Contact_TelContactNonConforme_NoNullAllowedException()
    {
       var valeurs = new Dictionary<string, string?>
        {
            {KeyWords.ContactTelephone, "azerty"}
        };

        Assert.Throws<FormatException>(() => Contact.VerifierTel(valeurs));
    }

    [Fact]
    public void Contact_TelContactDictionnaireNull_NoNullAllowedException()
    {
        Assert.Throws<NoNullAllowedException>(() => Contact.VerifierTel(null));
    }

    [Fact]
    public void Contact_CreationContactReussite_ContactConforme()
    {
        const string typeContact = KeyWords.ContactPremier;
        var valeurs = new Dictionary<string, string?>
        {
            { KeyWords.ContactNom, "john" },
            { KeyWords.ContactMail, "john@doe.com"},
            {KeyWords.ContactTelephone, "+33123456789"}
        };

        var result = Contact.Creer(typeContact, valeurs);
        Assert.NotNull(result);

        Assert.NotNull(result.Nom);
        Assert.Equal("john",result.Nom);

        Assert.NotNull(result.Mail);
        Assert.Equal("john@doe.com",result.Mail);

        Assert.NotNull(result.Tel);
        Assert.Equal("+33123456789", result.Tel);
    }
}
