using UnityEngine;

public static class GameVariables
{
    public static bool spriteInteractionLocked = false;

    public static void ResetVariables()
    {
        spriteInteractionLocked = false;
    }
}
