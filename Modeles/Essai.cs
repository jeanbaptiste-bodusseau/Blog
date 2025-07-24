namespace Modeles;

public class Essai
{
    public int Joueur;

    public int Essaie(Morpion morpion, (int x, int y) cout)
    {
        var copie = morpion.DeepCopy();
        copie.Grille[cout.x][cout.y] = Joueur;

        var fin = copie.VerifierFin(Joueur*-1);
        if (fin != null)
        {
            return (bool)fin ? 1 : 0; 
        }

        int total = 0;
        foreach (var coup in Robot.CoupsPossible(copie.Grille))
        {
            var essai = new Essai { Joueur = -Joueur };
            total += essai.Essaie(copie.DeepCopy(), coup);
        }

        return total;
    }
}