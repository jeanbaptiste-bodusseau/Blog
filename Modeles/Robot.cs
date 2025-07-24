using System.Drawing;
using static Modeles.Extensions;

namespace Modeles;
public class Robot
{
    public List<Morpion> morpions = [];
    public List<(int x, int y)> coups;
    public (int x, int y) CoupAJouer;
    public int Nb;

    public Robot(int joueur, List<List<int>> grille = null)
    {
        coups = CoupsPossible(grille);
        foreach (var coup in coups)
        {
            morpions.Add(new (grille.DeepCopy()));
        }

        CoupAJouer = EssaieCoup(joueur);
    }

    public (int x, int y) EssaieCoup(int joueur)
    {
        var meilleurCoup = coups[0];
        var meilleurScore = int.MinValue;

        foreach (var (i, coup) in coups.Select((c, i) => (i, c)))
        {
            if (VerifierCoupAutreJoueur(-joueur, morpions[i].DeepCopy(), coup) == true)
                return coup;

            var morpionCopy = morpions[i];
            morpionCopy.Grille[coup.x][coup.y] = joueur;

            if (morpionCopy.VerifierFin(joueur) == true)
            {
                return coup;
            }

            var essai = new Essai { Joueur = -joueur };
            var score = essai.Essaie(morpionCopy, coup);

            if (score <= meilleurScore) continue;
            meilleurScore = score;
            meilleurCoup = coup;
        }

        return meilleurCoup;
    }

    public static bool? VerifierCoupAutreJoueur(int joueur,Morpion morpion, (int x, int y) coup)
    {
        morpion.Grille[coup.x][coup.y] = joueur;
        return morpion.VerifierFin(joueur);

    }


    public static List<(int x, int y)> CoupsPossible(List<List<int>> grille)
    {
        List<(int x, int y)> result = [];
        for (var i = 0; i < 3; i++)
        {
            for (var f = 0; f < 3; f++)
            {
                if (grille[i][f] == 0)
                    result.Add((i, f));
            }
        }

        return result;
    }

    public void AfficherGrilles(List<Morpion>? liste = null)
    {
        Console.Clear();
        var result = liste == null ? morpions.SplitList(3) : liste.SplitList(3);
        Console.WriteLine("┌─────────┬─────────┬─────────┐");
        foreach (var grille in result)
        {
            for (var i = 0; i < 3; i++)
            {
                Console.Write("│");
                for (var j = 0; j < grille.Count; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var color = grille[j].Grille[i][k] switch
                        {
                            1 => Color.Red,
                            -1 => Color.SkyBlue,
                            _ => Color.White,
                        };
                        Console.Write(" " + new StringColorise("■", color).Str + " ");
                    }
                    Console.Write("│");
                        
                }
                Console.Write("\n");
            }
            Console.WriteLine("└─────────┴─────────┴─────────┘");
        }
    }
}
