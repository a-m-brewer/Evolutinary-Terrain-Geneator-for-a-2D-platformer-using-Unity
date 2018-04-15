using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerMovementRestrictions {
    public static float cMoveSpeed = 320f;
    public static float cGravity = -960f;
    public static float cJumpSpeed = 440f;
    public static float cMaxFallSpeed = -900f;

    public static float cMaxFallSpeedUnity = cMaxFallSpeed / 32f;

    public static int maxJumpHeight = 2;
    public static int maxJumpHeightValue = 2 * maxJumpHeight;
}
