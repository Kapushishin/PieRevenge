public static class SwitchParametres
{
    public static int PeachCounter;
    public static float HealthCounter;

    public static bool CanJump = false;
    public static bool CanDoubleJump = false;
    public static bool CanDash = false;

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
}
