using ClosedXML.Excel;

namespace ACPR.Helper
{
    public static class ExtensionsHelper
    {

        /// <summary>
        /// Formatte un string pour correspondre aux normes des regles (exemple Inter Pole -> inter_pole)
        /// </summary>
        /// <param name="str">string a formatter</param>
        /// <returns>string formatté</returns>
        public static string NettoyerString(this string str) =>
            str.ToLower().Trim().Replace(" ", "_").Replace(",", "").Replace("/", "_");

        /// <summary>
        /// Formatte les champs du excel (exemple 1,3 -> 1.3)
        /// </summary>
        /// <param name="str">string a formatter</param>
        /// <returns>string formatté</returns>
        public static string FormatterChamps(this string str) => str.Trim().Replace(",", ".");

      
        /// <summary>
        /// Transforme une liste de cellule excel en liste de string en la formattant si nécessaire grace a <see cref="NettoyerString(string)"/>
        /// </summary>
        /// <param name="liste">liste de cellule</param>
        /// <param name="format">Si la liste doit etre formatté ou non</param>
        /// <returns>liste de string formatté</returns>
        public static List<string> RecupererValeurListCellule(this List<IXLCell> liste, bool format = true) =>
            [.. liste.Select(cellule => format ? cellule.GetValue<string>().NettoyerString() : cellule.GetValue<string>())];

        /// <summary>
        /// Verifie si 2 liste de string sont egaux
        /// </summary>
        /// <param name="liste">liste1</param>
        /// <param name="liste2">liste2</param>
        /// <returns>si les deux sont egaux</returns>
        public static bool EstEgal(this IEnumerable<string> liste, IEnumerable<string>? liste2)
        {
            if (liste2 == null) 
                return false;
            var enumerable = liste.ToList();
            var enumerable1 = liste2.ToList();
            if ( enumerable!.Count != enumerable1!.Count) 
                return false;
            return !enumerable.Where((t, i) => enumerable.ToList()[i] != enumerable1.ToList()[i]).Any();
        }

        /// <summary>
        /// Verifie si 2 float sont egaux (précision a 0.00000001)
        /// </summary>
        /// <param name="a">float1</param>
        /// <param name="b">float2</param>
        /// <returns>si les 2 sont egaux</returns>
        public static bool EstEgal(this float a, float? b)
        {
            if (b == null) 
                return false;
            return Math.Abs(a - (float)b) < 0.00000001f;
        }
    }
}
