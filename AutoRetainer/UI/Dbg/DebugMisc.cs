﻿using AutoRetainer.Helpers;
using ECommons.MathHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using PInvoke;
using System.Windows.Forms;

namespace AutoRetainer.UI.Dbg;

internal static unsafe class DebugMisc
{
    internal static void Draw()
    {
        ImGuiEx.Text($"CSFramework.Instance()->WindowInactive: {CSFramework.Instance()->WindowInactive}");
        ImGuiEx.Text($"IsKeyPressed(P.config.TempCollectB): {IsKeyPressed(P.config.TempCollectB)}");
        ImGuiEx.Text($"Bitmask.IsBitSet(User32.GetKeyState((int)P.config.TempCollectB), 15): {Bitmask.IsBitSet(User32.GetKeyState((int)P.config.TempCollectB), 15)}");
        ImGuiEx.Text($"DontReassign: {P.config.DontReassign}, key {P.config.TempCollectB}/{(int)P.config.TempCollectB}");
        foreach(var x in P.config.OfflineData)
        {
            ImGuiEx.Text($"{x.Name}@{x.World}: {(x.Gil + x.RetainerData.Sum(z => z.Gil))}");
        }
        var ocd = Utils.GetCurrentCharacterData();
        if(ocd != null)
        {
            ImGuiEx.Text($"Level array:");
            ImGuiEx.Text(ocd.ClassJobLevelArray.Print());
        }
        
        ImGuiEx.Text($"{Utils.TryGetCurrentRetainer(out var n)}/{n}");
        ImGuiEx.Text($"{ItemLevel.Calculate(out var g, out var p)}/{g}/{p}");
        if (ImGui.Button("Regenerate censor seed"))
        {
            P.config.CensorSeed = Guid.NewGuid().ToString();
        }
        var inv = Utils.GetActiveRetainerInventoryName();
        ImGuiEx.Text($"Utils.GetActiveRetainerInventoryName(): {inv.Name} {inv.EntrustDuplicatesIndex}");
        ImGuiEx.Text($"ConditionWasEnabled={P.ConditionWasEnabled}");
        if (ImGui.CollapsingHeader("Task debug"))
        {
            ImGuiEx.Text($"Busy: {P.TaskManager.IsBusy}, abort in {P.TaskManager.AbortAt - Environment.TickCount64}");
            if (ImGui.Button($"Generate random numbers 1/500"))
            {
                P.TaskManager.Enqueue(() => { var r = new Random().Next(0, 500); InternalLog.Verbose($"Gen 1/500: {r}"); return r == 0; });
            }
            if (ImGui.Button($"Generate random numbers 1/5000"))
            {
                P.TaskManager.Enqueue(() => { var r = new Random().Next(0, 5000); InternalLog.Verbose($"Gen 1/5000: {r}"); return r == 0; });
            }
            if (ImGui.Button($"Generate random numbers 1/100"))
            {
                P.TaskManager.Enqueue(() => { var r = new Random().Next(0, 100); InternalLog.Verbose($"Gen 1/100: {r}"); return r == 0; });
            }
        }
        ImGuiEx.Text($"QSI status: {P.quickSellItems?.openInventoryContextHook?.IsEnabled}");
        ImGuiEx.Text($"QuickSellItems.IsReadyToUse: {QuickSellItems.IsReadyToUse()}");

        foreach (var x in StatisticsUI.CharTotal)
        {
            ImGuiEx.Text($"{x.Key} : {x.Value}");
        }
        foreach (var x in StatisticsUI.RetTotal)
        {
            ImGuiEx.Text($"{x.Key} : {x.Value}");
        }

        ImGui.Separator();
        ImGuiEx.Text($"GC Addon Life: {AutoGCHandin.GetAddonLife()}");
        {
            if (ImGui.Button("Fire") && TryGetAddonByName<AtkUnitBase>("GrandCompanySupplyList", out var addon) && IsAddonReady(addon) && addon->UldManager.NodeList[5]->IsVisible)
            {
                AutoGCHandin.InvokeHandin(addon);
            }
        }

        {
            if (TryGetAddonByName<AtkUnitBase>("GrandCompanySupplyList", out var addon) && IsAddonReady(addon))
            {
                ImGuiEx.Text($"IsSelectedFilterValid: {AutoGCHandin.IsSelectedFilterValid(addon)}");
            }
        }

    }
}
