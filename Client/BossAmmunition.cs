using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using UnityEngine;

namespace Hawks.Arges.Juggernaut;

internal static class BossAmmunition
{
    private const string Arges = "686a13500000000000000001";
    private const string Belt = "686a13500000000000000002";

    internal static void Install(Harmony harmony)
    {
        harmony.Patch(AccessTools.DeclaredMethod(typeof(Player.FirearmController), "InitiateShot"),
            prefix: new HarmonyMethod(typeof(BossAmmunition), nameof(ReplenishFeed)));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(BotReload), nameof(BotReload.TryReload)),
            prefix: new HarmonyMethod(typeof(BossAmmunition), nameof(AllowReload)));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(BotReload), nameof(BotReload.Reload)),
            prefix: new HarmonyMethod(typeof(BossAmmunition), nameof(AllowReload)));
        harmony.Patch(AccessTools.DeclaredMethod(typeof(Player), nameof(Player.OnDead)),
            prefix: new HarmonyMethod(typeof(BossAmmunition), nameof(PrepareLoot)));
    }

    private static bool IsBoss(Player player) =>
        player && player.IsAI && player.Profile?.Info?.Settings?.Role == WildSpawnType.bossTest;

    private static bool AllowReload(BotReload __instance) =>
        __instance._weapon?.TemplateId != Arges || !IsBoss(__instance._player);

    private static void ReplenishFeed(Player.FirearmController __instance, Player ____player, Ammo ammo)
    {
        if (__instance.Item?.TemplateId != Arges || !IsBoss(____player) || !____player.HealthController.IsAlive)
            return;
        var magazine = __instance.Item.GetCurrentMagazine();
        if (magazine?.TemplateId != Belt || magazine.Count > magazine.MaxCount / 2)
            return;
        Refill(magazine.Cartridges, ammo);
    }

    private static void Refill(StackSlot rounds, Ammo fired)
    {
        int missing = rounds.MaxCount - rounds.Count;
        if (rounds.Last is Ammo top)
        {
            top.StackObjectsCount += missing;
            return;
        }
        var fresh = fired.Clone(MongoID.Generate().ToString());
        fresh.IsUsed = false;
        fresh.StackObjectsCount = missing;
        rounds.Add(fresh, false);
    }
    private static void PrepareLoot(Player __instance)
    {
        if (!IsBoss(__instance)) return;
        var equipment = __instance.Inventory?.Equipment;
        foreach (var slot in new[] { EquipmentSlot.FirstPrimaryWeapon, EquipmentSlot.SecondPrimaryWeapon })
        {
            var weapon = equipment?.GetSlot(slot)?.ContainedItem as Weapon;
            if (weapon?.TemplateId != Arges) continue;
            var magazine = weapon.GetCurrentMagazine();
            if (magazine?.TemplateId != Belt || magazine.Count < 2) continue;
            int target = Random.Range(1, Mathf.Min(magazine.Count, magazine.MaxCount));
            ReduceTo(magazine.Cartridges, target);
        }
    }

    private static void ReduceTo(StackSlot rounds, int target)
    {
        while (rounds.Count > target)
        {
            var top = rounds.Last;
            int surplus = rounds.Count - target;
            if (top.StackObjectsCount > surplus)
                top.StackObjectsCount -= surplus;
            else if (!rounds.Remove(top, false).Succeeded)
                break;
        }
    }
}