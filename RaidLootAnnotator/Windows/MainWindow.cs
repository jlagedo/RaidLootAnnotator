using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.Sheets;

namespace RaidLootAnnotator.Windows;

public class MainWindow : Window, IDisposable
{
    private string GoatImagePath;
    private Plugin Plugin;

    // We give this window a hidden ID using ##
    // So that the user will see "My Amazing Window" as window title,
    // but for ImGui the ID is "My Amazing Window##With a hidden ID"
    public MainWindow(Plugin plugin, string goatImagePath)
        : base("Raid Loot Annotator##raidmain123", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        GoatImagePath = goatImagePath;
        Plugin = plugin;
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


        DrawLootTableAcc();

        DrawLootTableGear();

        DrawLootTableUpgrades();

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
                    var earsValue = Plugin.Configuration.LootData.EarsValue;
                    if (ImGui.InputInt("##EarsInput", ref earsValue))
                    {
                        Plugin.Configuration.LootData.EarsValue = earsValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(1);
                    ImGui.SetNextItemWidth(-1);
                    var neckValue = Plugin.Configuration.LootData.NeckValue;
                    if (ImGui.InputInt("##NeckInput", ref neckValue))
                    {
                        Plugin.Configuration.LootData.NeckValue = neckValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(2);
                    ImGui.SetNextItemWidth(-1);
                    var wristsValue = Plugin.Configuration.LootData.WristsValue;
                    if (ImGui.InputInt("##WristsInput", ref wristsValue))
                    {
                        Plugin.Configuration.LootData.WristsValue = wristsValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(3);
                    ImGui.SetNextItemWidth(-1);
                    var ringValue = Plugin.Configuration.LootData.RingValue;
                    if (ImGui.InputInt("##RingInput", ref ringValue))
                    {
                        Plugin.Configuration.LootData.RingValue = ringValue;
                        Plugin.Configuration.Save();
                    }

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

    private void DrawLootTableGear()
    {

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            // Check if this child is drawing
            if (child.Success)
            {
                ImGui.TextUnformatted("Gear");

                ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 10)); // Adjust values as needed

                if (ImGui.BeginTable("LootTableGear", 6, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                {
                    // Set up the headers
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
                    var weaponValue = Plugin.Configuration.LootData.WeaponValue;
                    if (ImGui.InputInt("##WeaponInput", ref weaponValue))
                    {
                        Plugin.Configuration.LootData.WeaponValue = weaponValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(1);
                    ImGui.SetNextItemWidth(-1);
                    var headValue = Plugin.Configuration.LootData.HeadValue;
                    if (ImGui.InputInt("##HeadInput", ref headValue))
                    {
                        Plugin.Configuration.LootData.HeadValue = headValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(2);
                    ImGui.SetNextItemWidth(-1);
                    var bodyValue = Plugin.Configuration.LootData.BodyValue;
                    if (ImGui.InputInt("##BodyInput", ref bodyValue))
                    {
                        Plugin.Configuration.LootData.BodyValue = bodyValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(3);
                    ImGui.SetNextItemWidth(-1);
                    var handsValue = Plugin.Configuration.LootData.HandsValue;
                    if (ImGui.InputInt("##HandsInput", ref handsValue))
                    {
                        Plugin.Configuration.LootData.HandsValue = handsValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(4);
                    ImGui.SetNextItemWidth(-1);
                    var legsValue = Plugin.Configuration.LootData.LegsValue;
                    if (ImGui.InputInt("##LegsInput", ref legsValue))
                    {
                        Plugin.Configuration.LootData.LegsValue = legsValue;
                        Plugin.Configuration.Save();
                    }


                    ImGui.TableSetColumnIndex(5);
                    ImGui.SetNextItemWidth(-1);
                    var feetValue = Plugin.Configuration.LootData.FeetValue;
                    if (ImGui.InputInt("##FeetInput", ref feetValue))
                    {
                        Plugin.Configuration.LootData.FeetValue = feetValue;
                        Plugin.Configuration.Save();
                    }
                    ImGui.EndTable();
                }

                ImGui.PopStyleVar();

            }
        }
    }

    private void DrawLootTableUpgrades()
    {

        // Normally a BeginChild() would have to be followed by an unconditional EndChild(),
        // ImRaii takes care of this after the scope ends.
        // This works for all ImGui functions that require specific handling, examples are BeginTable() or Indent().
        using (var child = ImRaii.Child("SomeChildWithAScrollbar", Vector2.Zero, true))
        {
            // Check if this child is drawing
            if (child.Success)
            {
                ImGui.TextUnformatted("Upgrades");

                ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(10, 10)); // Adjust values as needed

                if (ImGui.BeginTable("LootTableUpgrades", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                {
                    // Set up the headers
                    ImGui.TableSetupColumn("Weapon Token");
                    ImGui.TableSetupColumn("Weapon Upgrade");
                    ImGui.TableSetupColumn("Accessorie Upgrade");
                    ImGui.TableSetupColumn("Gear Upgrade");

                    ImGui.TableHeadersRow();
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    ImGui.SetNextItemWidth(-1);
                    var weaponTokenValue = Plugin.Configuration.LootData.WeaponTokenValue;
                    if (ImGui.InputInt("##WeaponTokenInput", ref weaponTokenValue))
                    {
                        Plugin.Configuration.LootData.WeaponTokenValue = weaponTokenValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(1);
                    ImGui.SetNextItemWidth(-1);
                    var weaponUpgradeValue = Plugin.Configuration.LootData.WeaponUpgradeValue;
                    if (ImGui.InputInt("##WeaponUpgradeInput", ref weaponUpgradeValue))
                    {
                        Plugin.Configuration.LootData.WeaponUpgradeValue = weaponUpgradeValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(2);
                    ImGui.SetNextItemWidth(-1);
                    var accUpgradeValue = Plugin.Configuration.LootData.AccUpgradeValue;
                    if (ImGui.InputInt("##AccUpgradeValueInput", ref accUpgradeValue))
                    {
                        Plugin.Configuration.LootData.AccUpgradeValue = accUpgradeValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.TableSetColumnIndex(3);
                    ImGui.SetNextItemWidth(-1);
                    var gearUpgradeValue = Plugin.Configuration.LootData.GearUpgradeValue;
                    if (ImGui.InputInt("##GearUpgradeValueInput", ref gearUpgradeValue))
                    {
                        Plugin.Configuration.LootData.GearUpgradeValue = gearUpgradeValue;
                        Plugin.Configuration.Save();
                    }

                    ImGui.EndTable();
                }

                ImGui.PopStyleVar();

            }
        }
    }
}

