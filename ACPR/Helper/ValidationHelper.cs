using System.Text.RegularExpressions;

namespace ACPR.Helper;

public static partial class ValidationHelper
{
    [GeneratedRegex("^[A-Z0-9]{18}[0-9]{2}$")]
    private static partial Regex Lei();
    /// <summary>
    /// Verifie un code LEI
    /// </summary>
    /// <param name="lei">code LEI à verifier</param>
    /// <returns>true si c'est bon, false sinon</returns>
    public static bool ValiderLei(string lei)
    {
        return Lei().IsMatch(lei);
    }
    
    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex Email();

    /// <summary>s
    /// Verifie une adresse email
    /// </summary>
    /// <param name="email">adresse email a verifier</param>
    /// <returns></returns>
    public static bool ValiderEmail(string email)
    {
        return Email().IsMatch(email);
    }

    [GeneratedRegex("\\+[1-9]\\d{1,14}(\\s?\\(\\d+\\))?([-\\s.]?\\d+)*$")]
    private static partial Regex Telephone();
    /// <summary>
    /// Verifie un numero de telephone
    /// </summary>
    /// <param name="tel">numero de telephone a verifier (en string)</param>
    /// <returns>true si c'est bon, false sinon</returns>
    public static bool ValiderTelephone(string tel)
    {
        return Telephone().IsMatch(tel);
    }

    /// <summary>
    /// Verifie et convertie une date un DateTime
    /// </summary>
    /// <param name="date">date en string a verifier</param>
    /// <param name="dateConvertie">retour de la date convert</param>
    /// <returns>true si c'est bon</returns>
    public static bool ValiderDate(string? date, out DateTime dateConvertie)
    {
        return DateTime.TryParse(date!, System.Globalization.CultureInfo.InvariantCulture, out dateConvertie);
    }


    /// <summary>
    /// Valider si un element est present dans la regle correspondante
    /// </summary>
    /// <param name="regle">numéro de la regle</param>
    /// <param name="valeur">valeur a verifier</param>
    /// <returns>true si la regle est respecte</returns>
    public static bool ValiderRegle(IEnumerable<string> regle, string valeur)
    {
        return regle.Contains(valeur);
    }

    /// <summary>
    /// Valider si un IEnumerable d'element est autorisé par la regle correspondante
    /// </summary>
    /// <param name="regle">numéro de la regle</param>
    /// <param name="valeur">IEnumerable des valeurs à controler</param>
    /// <returns>true si la regle est respecte pour toutes les valeurs</returns>
    public static bool ValiderRegle(IEnumerable<string> regle, IEnumerable<string> valeur)
    {
        foreach(var element in valeur)
        {
            if (!regle.Contains(element)) return false;
        }
        return true;
    }

    /// <summary>
    /// renvoie True, False ou null pour les valeurs correpondantes (Yes, No, autres)
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool? VerifierYesNoString(string? str)
    {
        return str switch
        {
            _ when string.IsNullOrEmpty(str) => null,
            var p when p.Equals("yes", StringComparison.InvariantCultureIgnoreCase) => true,
            var p when p.Equals("no", StringComparison.InvariantCultureIgnoreCase) => false,
            _ => null,
        };
    }
    
    /// <summary>
    /// Une durée est un string au format DDD:HH:mm. Cette fonction est faite pour vérifier qu'un string est donc au bon format.
    /// </summary>
    /// <param name="str">Potentielle durée</param>
    /// <param name="format">Liste d'entier contenant les valeurs</param>
    /// <param name="mes">Message d'erreur</param>
    /// <returns></returns>
    public static bool VerifierDureeNull(string? str, out List<int>? format, out string mes)
    {
        format = [];
        mes = "";
        if (string.IsNullOrEmpty(str)) return false;

        var res = str.Split(":");
        //Verifie qu'il y a bien 3 champs
        if (res.Length != 3)
        {
            format = null;
            mes = "pas assez ou trop d'element";
            return false;
        }

        //Verifie que toutes les valeurs soient des entiers
        foreach (var item in res)
        {
            if (int.TryParse(item, out var val))
            {
                format.Add(val);
            }
            else
            {
                mes = "erreur lors du parse";
                format = null;
                return false;
            }
        }

        //Verifie qu'aucune valeur soit en dessous de 0;
        var check = true;
        foreach (var item in format.Where(item => item < 0))
        {
            check = false;
        }
        format = check ? format : null;
        return true;
    }

}

