using ACPR.Ressources;

namespace ACPR.Helper;
public static class FichierHelper
{
    /// <summary>
    /// Verifie si le fichier existe et il est au bon format
    /// </summary>
    /// <param name="cheminFichier">fichier a verifier</param>
    /// <param name="extension">Extension a verifier</param>
    /// <exception cref="FileNotFoundException"></exception>
    public static void VerifierFichier(string cheminFichier, string extension)
    {
        if (!File.Exists(cheminFichier)) 
            throw new FileNotFoundException(Resource.FichierHelperNonTrouve);

        if (Path.GetExtension(cheminFichier) != extension)
            throw new FileNotFoundException(Resource.FichierHelperFormatFichierNonConforme + extension);
    }

    /// <summary>
    /// Permet de savoir si l'emplacement passé est un dossier ou un fichier
    /// </summary>
    /// <param name="cheminFichier">emplacement à vérifier</param>
    /// <returns>chemin complet</returns>
    public static string CheminOuFichier(string cheminFichier)
    {
        
        if (File.Exists(cheminFichier))
            return Path.GetFullPath(cheminFichier);

        return Directory.Exists(cheminFichier) ? Path.Combine(Path.GetFullPath(cheminFichier), "save.json") : cheminFichier;
    }

    /// <summary>
    /// Verifie si le repertoire est correct
    /// </summary>
    /// <param name="cheminFichier"></param>
    /// <returns></returns>
    public static string? VerifierRepertoire(string cheminFichier)
    {
        return Path.GetDirectoryName(Path.GetFullPath(cheminFichier));
    }
}
