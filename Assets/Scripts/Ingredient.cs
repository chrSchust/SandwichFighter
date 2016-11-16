using System.Collections.Generic;


public class Ingredient

{
    internal static readonly int WHITE_BREAD = 1000;
    internal static readonly int BUTTER = 1001;

    public static Dictionary<int, string> NameList = new Dictionary<int, string> { { WHITE_BREAD, "Weißbrot" }, { BUTTER, "Butter" } };
}