using BepInEx;
using EFT;
using EFT.Animations.NewRecoil;
using HarmonyLib;
using System;

namespace Hawks.Arges.Juggernaut;
[BepInPlugin("com.hawks.arges.juggernaut.client","Arges Juggernaut Integration","1.0.1")]
[BepInDependency("com.silian.juggernaut.client","1.1.1")]
[BepInDependency("team.aminus.arges.client")]
public sealed class Integration : BaseUnityPlugin
{
    private static Func<Player,int> armorTier;
    private static readonly AccessTools.FieldRef<NewRecoilShotEffect, Player.FirearmController> Controller =
        AccessTools.FieldRefAccess<NewRecoilShotEffect, Player.FirearmController>("_firearmController");

    private void Awake()
    {
        var method = AccessTools.Method("JuggernautClient.JugTiers:Tier", new[]{typeof(Player)});
        armorTier = (Func<Player,int>)Delegate.CreateDelegate(typeof(Func<Player,int>),method);
        var harmony = new Harmony("com.hawks.arges.juggernaut.client");
        BossAmmunition.Install(harmony);
        harmony.Patch(
            AccessTools.DeclaredMethod(typeof(NewRecoilShotEffect),"AddRecoilForce",new[]{typeof(float)}),
            prefix:new HarmonyMethod(typeof(Integration),nameof(ApplyPoweredBonus)));
    }

    private static void ApplyPoweredBonus(NewRecoilShotEffect __instance, ref float __0)
    {
        var controller = Controller(__instance);
        if (controller?.Item?.TemplateId != "686a13500000000000000001") return;
        var player = Comfort.Common.Singleton<GameWorld>.Instance?.MainPlayer;
        if (player && ReferenceEquals(player.HandsController, controller) && armorTier(player) == 1)
            __0 *= 0.6f;
    }
}
