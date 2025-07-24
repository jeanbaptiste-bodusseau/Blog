namespace Modeles;
public class Jeu
{
    private static Morpion morpion;

    private static int _currentPlayer = -1;

    public static void Jouer()
    {
        morpion = new Morpion();
        morpion.Afficher();
        while (morpion.VerifierFin(_currentPlayer*-1) == null)
        {
            if (_currentPlayer == 1)
            {
                var bot = new Robot(1,morpion.Grille);
                var coup = bot.CoupAJouer;
                morpion.Grille[coup.x][coup.y] = 1;
                morpion.Afficher();
                _currentPlayer *= -1;
                continue;
            }
            var touche = Console.ReadKey().Key;

            if (VerifierInput(touche))
                DeplacerJoueur(touche);

            morpion.Afficher();

            if (touche != ConsoleKey.Spacebar) continue;

            morpion.PlacerJoueur(_currentPlayer);
            _currentPlayer *= -1;
        }
        morpion.Afficher(false);
        Console.WriteLine(morpion.VerifierFin(_currentPlayer*=-1) switch
        {
            true when _currentPlayer == 1 => "red",
            true when _currentPlayer == -1 => "blue",
            false => "egalite",
            _ => ""
        });
    }

    private static void DeplacerJoueur(ConsoleKey touche)
    {
        switch (touche)
        {
            case ConsoleKey.RightArrow:
                morpion.PosJoueur.y++;
                break;
            case ConsoleKey.LeftArrow:
                morpion.PosJoueur.y--;
                break;
            case ConsoleKey.UpArrow:
                morpion.PosJoueur.x--;
                break;
            case ConsoleKey.DownArrow:
                morpion.PosJoueur.x++;
                break;
            default:
                break;
        }
    }

    private static bool VerifierInput(ConsoleKey touche)
    {
        return touche switch
        {
            ConsoleKey.LeftArrow when morpion.PosJoueur.y == 0 => false,
            ConsoleKey.RightArrow when morpion.PosJoueur.y == 2 => false,
            ConsoleKey.UpArrow when morpion.PosJoueur.x == 0 => false,
            ConsoleKey.DownArrow when morpion.PosJoueur.x == 2 => false,
            _ => true,
        };
    }
    
}