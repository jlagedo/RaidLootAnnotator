using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.Sheets;
using static FFXIVClientStructs.FFXIV.Component.GUI.AtkComponentNumericInput.Delegates;

namespace RaidLootAnnotator.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    // Light red color (RGBA: 1.0, 0.6, 0.6, 1.0)
    private readonly uint lightRed = ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.6f, 0.6f, 1.0f));
    private readonly uint lightGreen = ImGui.ColorConvertFloat4ToU32(new Vector4(0.6f, 1.0f, 0.6f, 1.0f));

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin)
        : base("Raid Loot Annotator##raidmain123", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {

        // Do not use .Text() or any other formatted function like TextWrapped(), or SetTooltip().
        // These expect formatting parameter if any part of the text contains a "%", which we can't
        // provide through our bindings, leading to a Crash to Desktop.
        // Replacements can be found in the ImGuiHelpers Class
        //ImGui.TextUnformatted($"The random config bool is {Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

        //if (ImGui.Button("Show Settings"))
        //{
        //    Plugin.ToggleConfigUI();
        //}

        ImGui.Spacing();
        if (ImGui.Button("Reset All Values"))
        {
            plugin.Configuration.LootData.ResetAllValues();
            plugin.Configuration.Save();
        }

        DrawLootTableAcc();

        DrawLootTableGear();

        DrawLootTableUpgrades();

    }

    private void DrawLootInputCell(string label, Func<int> getter, Action<int> setter)
    {
        var value = getter();
        ChangeColorByValue(value);
        if (ImGui.InputInt(label, ref value))
        {
            value = Math.Max(0, value);
            setter(value);
            plugin.Configuration.Save();
        }
    }

    private void DrawLootTableAcc()
    {



        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            // Check if this child is drawing
            if (child.Success)
            {
                ImGui.TextUnformatted("Accessories");

                ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 10)); // Adjust values as needed

                if (ImGui.BeginTable("LootTable", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                {
                    // Set up the headers
                    ImGui.TableSetupColumn("Ears");
                    ImGui.TableSetupColumn("Neck");
                    ImGui.TableSetupColumn("Wrists");
                    ImGui.TableSetupColumn("Ring");

                    ImGui.TableHeadersRow();
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.SetNextItemWidth(-1);

                    DrawLootInputCell("##EarsInput",
                        () => plugin.Configuration.LootData.EarsValue,
                        v => plugin.Configuration.LootData.EarsValue = v);

                    ImGui.TableSetColumnIndex(1);
                    ImGui.SetNextItemWidth(-1);

                    DrawLootInputCell("##NeckInput",
                        () => plugin.Configuration.LootData.NeckValue,
                        v => plugin.Configuration.LootData.NeckValue = v);

                    ImGui.TableSetColumnIndex(2);
                    ImGui.SetNextItemWidth(-1);

                    DrawLootInputCell("##WristsInput",
                        () => plugin.Configuration.LootData.WristsValue,
                        v => plugin.Configuration.LootData.WristsValue = v);

                    ImGui.TableSetColumnIndex(3);
                    ImGui.SetNextItemWidth(-1);

                    DrawLootInputCell("##RingInput",
                        () => plugin.Configuration.LootData.RingValue,
                        v => plugin.Configuration.LootData.RingValue = v);

                    ImGui.EndTable();
                }

                ImGui.PopStyleVar();

                /*

                ImGui.TextUnformatted("O Zellvish anotando as coisas de loot......");
                var goatImage = Plugin.TextureProvider.GetFromFile(GoatImagePath).GetWrapOrDefault();
                if (goatImage != null)
                {
                    //using (ImRaii.PushIndent(0f))
                    //{
                        ImGui.Image(goatImage.ImGuiHandle, new Vector2(300f, 300f));
                    //}
                }
                else
                {
                    ImGui.TextUnformatted("Image not found.");
                }

                ImGuiHelpers.ScaledDummy(10.0f);

                // Example for other services that Dalamud provides.
                // ClientState provides a wrapper filled with information about the local player object and client.

                var localPlayer = Plugin.ClientState.LocalPlayer;
                if (localPlayer == null)
                {
                    ImGui.TextUnformatted("Our local player is currently not loaded.");
                    return;
                }

                if (!localPlayer.ClassJob.IsValid)
                {
                    ImGui.TextUnformatted("Our current job is currently not valid.");
                    return;
                }

                // ExtractText() should be the preferred method to read Lumina SeStrings,
                // as ToString does not provide the actual text values, instead gives an encoded macro string.
                ImGui.TextUnformatted($"Our current job is ({localPlayer.ClassJob.RowId}) \"{localPlayer.ClassJob.Value.Abbreviation.ExtractText()}\"");

                // Example for quarrying Lumina directly, getting the name of our current area.
                var territoryId = Plugin.ClientState.TerritoryType;
                if (Plugin.DataManager.GetExcelSheet<TerritoryType>().TryGetRow(territoryId, out var territoryRow))
                {
                    ImGui.TextUnformatted($"We are currently in ({territoryId}) \"{territoryRow.PlaceName.Value.Name.ExtractText()}\"");
                }
                else
                {
                    ImGui.TextUnformatted("Invalid territory.");
                }

                */
            }
        }
    }

    private void ChangeColorByValue(int earsValue)
    {
        if (earsValue <= 0)
            ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, lightGreen);
        //if (earsValue > 0)
        //    ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, lightRed);
    }

    private void DrawLootTableGear()
    {
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            if (child.Success)
            {
                ImGui.TextUnformatted("Gear");

                ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 10));

                if (ImGui.BeginTable("LootTableGear", 6, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                {
                    ImGui.TableSetupColumn("Weapon");
                    ImGui.TableSetupColumn("Head");
                    ImGui.TableSetupColumn("Body");
                    ImGui.TableSetupColumn("Hands");
                    ImGui.TableSetupColumn("Legs");
                    ImGui.TableSetupColumn("Feet");

                    ImGui.TableHeadersRow();
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##WeaponInput",
                        () => plugin.Configuration.LootData.WeaponValue,
                        v => plugin.Configuration.LootData.WeaponValue = v);

                    ImGui.TableSetColumnIndex(1);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##HeadInput",
                        () => plugin.Configuration.LootData.HeadValue,
                        v => plugin.Configuration.LootData.HeadValue = v);

                    ImGui.TableSetColumnIndex(2);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##BodyInput",
                        () => plugin.Configuration.LootData.BodyValue,
                        v => plugin.Configuration.LootData.BodyValue = v);

                    ImGui.TableSetColumnIndex(3);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##HandsInput",
                        () => plugin.Configuration.LootData.HandsValue,
                        v => plugin.Configuration.LootData.HandsValue = v);

                    ImGui.TableSetColumnIndex(4);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##LegsInput",
                        () => plugin.Configuration.LootData.LegsValue,
                        v => plugin.Configuration.LootData.LegsValue = v);

                    ImGui.TableSetColumnIndex(5);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##FeetInput",
                        () => plugin.Configuration.LootData.FeetValue,
                        v => plugin.Configuration.LootData.FeetValue = v);

                    ImGui.EndTable();
                }

                ImGui.PopStyleVar();
            }
        }
    }

    private void DrawLootTableUpgrades()
    {
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            if (child.Success)
            {
                ImGui.TextUnformatted("Upgrades");
                ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 10));
                if (ImGui.BeginTable("LootTableUpgrades", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                {
                    ImGui.TableSetupColumn("Weapon Token");
                    ImGui.TableSetupColumn("Weapon Upgrade");
                    ImGui.TableSetupColumn("Accessorie Upgrade");
                    ImGui.TableSetupColumn("Gear Upgrade");

                    ImGui.TableHeadersRow();
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##WeaponTokenInput",
                        () => plugin.Configuration.LootData.WeaponTokenValue,
                        v => plugin.Configuration.LootData.WeaponTokenValue = v);

                    ImGui.TableSetColumnIndex(1);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##WeaponUpgradeInput",
                        () => plugin.Configuration.LootData.WeaponUpgradeValue,
                        v => plugin.Configuration.LootData.WeaponUpgradeValue = v);

                    ImGui.TableSetColumnIndex(2);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##AccUpgradeValueInput",
                        () => plugin.Configuration.LootData.AccUpgradeValue,
                        v => plugin.Configuration.LootData.AccUpgradeValue = v);

                    ImGui.TableSetColumnIndex(3);
                    ImGui.SetNextItemWidth(-1);
                    DrawLootInputCell("##GearUpgradeValueInput",
                        () => plugin.Configuration.LootData.GearUpgradeValue,
                        v => plugin.Configuration.LootData.GearUpgradeValue = v);

                    ImGui.EndTable();
                }
                ImGui.PopStyleVar();
            }
        }
    }
}

