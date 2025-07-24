using System.Drawing;
using static Modeles.Extensions;
namespace Modeles;

public class Morpion
{
    public List<List<int>> Grille;

    public (int x, int y) PosJoueur = (1, 1);

    public Morpion(List<List<int>>? grille = null)
    {
        if (grille != null)
        {
            Grille = grille;
            return;
        }
        Grille = [];
        for (var i = 0; i < 3; i++)
        {
            List<int> ligne = [];
            for (var f = 0; f < 3; f++)
            {
                ligne.Add(0);
            }
            Grille.Add(ligne);
        }
    }

    public Morpion DeepCopy()
    {
        return new() { Grille = Grille.DeepCopy() };
    }

    public void Afficher(bool player = true)
    {
        Console.Clear();

        for (var i = 0; i < Grille.Count; i++)
        {
            for (var j = 0; j < Grille[i].Count; j++)
            {
                var color = Grille[i][j] switch
                {
                    1 => Color.Red,
                    -1 => Color.SkyBlue,
                    2 => Color.Green,
                    _ => Color.White,
                };
                if (player && i == PosJoueur.x && j == PosJoueur.y)
                    color = Color.Green;
                Console.Write(" " + new StringColorise("■", color).Str + " ");
            }

            Console.WriteLine("\n");
        }
    }

    public void PlacerJoueur(int currentPlayer)
    {
        Grille[PosJoueur.x][PosJoueur.y] = currentPlayer;
    }

    public bool? VerifierFin(int currentPlayer)
    {
        for (var i = 0; i < 3; i++)
        {
            if (SumCol(i) == currentPlayer * 3 || SumLigne(i) == currentPlayer * 3 || SumDiagoLeft() == currentPlayer * 3 || SumDiagoRight() == currentPlayer * 3)
                return true;
        }
        if (!Grille.Any(ligne => ligne.Any(cell => cell == 0))) return false;
        return null;
    }

    private int SumLigne(int id)
    {
        return Grille[id].Sum();
    }

    private int SumCol(int id)
    {
        return Grille.Sum(e => e[id]);
    }

    private int SumDiagoLeft()
    {
        var id = 0;
        return Grille.Sum(e => e[id++]);
    }

    private int SumDiagoRight()
    {
        var id = 2;
        return Grille.Sum(e => e[id--]);
    }
}