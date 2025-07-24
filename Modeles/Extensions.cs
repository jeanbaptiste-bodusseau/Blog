using Pastel;
using System.Drawing;

namespace Modeles;

public static class Extensions
{
    public class StringColorise(string str, Color? color = null)
    {
        public int Length { get; set; } = str.Length;
        public string Str { get; set; } = color == null ? str : str.Pastel((Color)color);
        public string BaseStr { get; set; } = str;

        public Color? Couleur = color;
    }

    public static List<List<T>> SplitList<T>(this List<T> liste, int index)
    {
        var list = new List<List<T>>();
        for (int i = 0; i < liste.Count; i += index)
        {
            list.Add(liste.GetRange(i, Math.Min(index,liste.Count-i)));
        }

        return list;
    }

    public static List<List<T>> DeepCopy<T>(this List<List<T>> liste)
    {
        return liste.
            Select(row => row.ToList())
            .ToList();
    }
}