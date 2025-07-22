using System.Text.Json.Serialization;

namespace ACPR.Domaine;

public class NombrePourcentage
{
    [JsonPropertyName("number")]
    public int Nombre { get; }

    [JsonPropertyName("percentage")]
    public float Pourcentage { get; }

    private NombrePourcentage(int number, float pourcentage)
    {
        Nombre = number;
        Pourcentage = pourcentage;
    }
    
    /// <summary>
    /// Créer un objet contenant un nombre et son pourcentage correspondant
    /// </summary>
    /// <param name="num">nombre</param>
    /// <param name="per">pourcentage</param>
    /// <param name="res">objet en retour</param>
    /// <returns>si la conversion s'est bien passé</returns>
    public static bool Creer(string num, string per, out NombrePourcentage? res)
    {

        //verification null
        if (string.IsNullOrEmpty(num) || string.IsNullOrEmpty(per))
        {
            res = null;
            return false;
        }

        //parse en int/float
        if (!int.TryParse(num.Replace(".", ","), out var nombre) || !float.TryParse(per.Replace(".", ","), out var pourcent))
        {
            res = null;
            return false;
        }

        //verification valeurs
        if (pourcent is < 0 or > 100)
        {
            res = null;
            return false;
        }
        res = new NombrePourcentage(nombre, pourcent);
        return true;
    }
}
