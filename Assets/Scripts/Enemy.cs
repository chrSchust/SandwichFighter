using System.Collections.Generic;

public class Enemy
{
    public const int NORMAL = 2000;
    public const int VEGAN = 2001;
    public const int FAT = 2003;


    public static Dictionary<int, int> HealthList = new Dictionary<int, int> { { NORMAL, 50 }, { VEGAN, 30 }, { FAT, 70 } };

    public static Dictionary<int, string> NameList = new Dictionary<int, string> { { NORMAL, "Normal" }, { VEGAN, "Vegan" }, { FAT, "Fett" } };

    public static Dictionary<int, float> SpeedList = new Dictionary<int, float> { { NORMAL, 2 }, { VEGAN, 3 }, { FAT, 1 } };

    public static Dictionary<int, int> WaypointList = new Dictionary<int, int> { { NORMAL, 2 }, { VEGAN, 3 }, { FAT, 1 } };

}