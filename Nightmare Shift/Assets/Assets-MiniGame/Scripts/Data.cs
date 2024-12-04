using System;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public const string introSceneName = "IntroScene";
    public const string gameSceneName = "GameScene";
    public const string lossSceneName = "EndScene";
    public const string winSceneName = "WinTextScene";

    public const string miniGameMainObjectTag = "minigame";
    public const string miniGameCameraTag = "minigameCamera";
    public const string pausePanelTag = "pausePanel";
    public const string endPanelTag = "endMinigamePanel";
    public const string lightningEffectTag = "lightning";

    public const float playerHealth = 100f;
    public const float xboxBossHealth = 400f;

    public const float xboxWeaponDmg = 2f;
    public const float basicWeaponDmg = 5f;
    public const float fireballDmg = 10f;
    public const float lightningDmg = 20f;

    public enum AnimationState
    {
        Idle,
        RegularAttack,
        AttackFireball,
        AttackLightning,
        Defending,
        DrinkingPotion,
        GettingHit,
        Dying
    }

    public static Dictionary<AnimationState, string> animationStates => states;

    static Dictionary<AnimationState, string> states = new()
    {
        { AnimationState.Idle, "idle" },
        { AnimationState.RegularAttack, "attack" },
        { AnimationState.AttackFireball, "fire" },
        { AnimationState.AttackLightning, "light" },
        { AnimationState.Defending, "defend" },
        { AnimationState.DrinkingPotion, "life" },
        { AnimationState.GettingHit, "get hit" },
        { AnimationState.Dying, "dead" }
    };

   public static GameObject FindInactiveObjectWithTag(string tag)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None)
            {
                return obj;
            }
        }

        return null;
    }
}
