public static class SwitchParametres
{
    public static int PeachCounter;
    public static float HealthCounter;

    public static bool CanJump = false;
    public static bool CanDoubleJump = false;
    public static bool CanDash = false;

    public static bool HaveWater = false;
    public static bool CanGoHome = false;

    public static void SwitchDoubleJump()
    {
        CanDoubleJump = true;
    }

    public static void SwitchJump()
    {
        CanJump = true;
    }

    public static void SwitchDash()
    {
        CanDash = true;
    }

    public static void SwitchHaveWater()
    {
        HaveWater = true;
    }

    public static void SwitchGoHome()
    {
        CanGoHome = true;
    }
}
