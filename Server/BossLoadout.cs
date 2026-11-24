using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace Hawks.Arges.Juggernaut;
[Injectable(TypePriority = OnLoadOrder.PostLoad + 2000)]
public sealed class BossLoadout(BotTable bots) : IOnLoad
{
    public Task OnLoadAsync(CancellationToken token)
    {
        if (!bots.Types.TryGetValue("bosstest",out var boss) || boss?.FirstNames?.Contains("Juggernaut") != true)
            return Task.CompletedTask;
        const string gun = "686a13500000000000000001", feed = "686a13500000000000000002";
        var inventory = boss.BotInventory;
        inventory.Equipment[EquipmentSlots.FirstPrimaryWeapon] = new() { [gun] = 100 };
        inventory.Mods[gun] = new() { ["mod_magazine"] = [feed] };
        const string ammo = "5e023e88277cce2b522ff2b1"; // 7.62x51 Ultra Nosler, 15 penetration in SPT 4.1.6.
        inventory.Ammo["Caliber762x51"] = new() { [ammo] = 100 };
        return Task.CompletedTask;
    }
}
