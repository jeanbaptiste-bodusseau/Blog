using Modeles;

namespace Bot;

public class Essai
{
    public int Victoire;
    public int Joueur;

    public int Essaie(Morpion morpion, (int x, int y) cout)
    {
        morpion.Grille[cout.x][cout.y] = Joueur;
        if (morpion.VerifierFin(Joueur) != null)
        {
            return Victoire + ((bool)morpion.VerifierFin(Joueur)! ? 1 : -1);
        }

        foreach (var result in Robot.CoupsPossible(morpion.Grille).Select(coup => new Essai { Joueur = Joueur * -1 }.Essaie(morpion.DeepCopy(), coup)))
        {
            Victoire += result;
        }
        return Victoire;
    }
}