using Modeles;
using System.Drawing;
using static Modeles.Extensions;

namespace Bot;
public class Robot
{
    public List<Morpion> morpions = [];
    public List<(int x, int y)> coups;
    public (int x, int y) CoupAJouer;

    public Robot(int joueur, List<List<int>> grille = null)
    {
        coups = CoupsPossible(grille);
        foreach (var coup in coups)
        {
            morpions.Add(new (grille.DeepCopy()));
        }

        CoupAJouer = EssaieCoup();
    }

    public (int x, int y) EssaieCoup()
    {
        var result = coups[0];
        var index = -1;
        var score = 0;
        foreach (var coup in coups)
        {
            index++;
            var indice = new Essai { Joueur = 1 }.Essaie(morpions[index],coup);
            if (indice <= score) continue;
            score = indice;
            result = coup;
        }

        AfficherGrilles();
        return result;
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
                for (var j = 0; j < 3; j++)
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
