using System.Collections.Generic;
public static class SwitchParametres
{
    public static int PeachCounter;
    public static float HealthCounter = 3;

    public static bool CanJump = false;
    public static bool CanDoubleJump = false;
    public static bool CanDash = false;
    public static bool CanAttack = false;

    public static List<string> objectsNames = new List<string>();
    public static List<float> playerPosition = new List<float>();
    public static Dictionary<string, bool> bgTriggersNames = new Dictionary<string, bool>();
    public static string SceneName;
    public static string BGName;
}
