using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using RaidLootAnnotator.ApiClient;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace RaidLootAnnotator.Windows;

public class ConfigWindow : Window, IDisposable
{
    #region Fields

    private readonly Plugin plugin;
    private readonly Configuration Configuration;

    private string name = string.Empty;
    private string groupCode = string.Empty;

    private string groupCreationResponse = string.Empty;
    private bool isCreatingGroup = false;
    private string newGroupCode = string.Empty;

    #endregion

    #region Constructor & Dispose

    // We give this window a constant ID using ###
    // This allows for labels being dynamic, like "{FPS Counter}fps###XYZ counter window",
    // and the window ID will always be "###XYZ counter window" for ImGui
    public ConfigWindow(Plugin plugin) : base(Plugin.PluginName + " Config###ConfigWindow1")
    {
        Flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
                ImGuiWindowFlags.NoScrollWithMouse;

        Size = new Vector2(500, 400);
        SizeCondition = ImGuiCond.Always;

        Configuration = plugin.Configuration;
        this.plugin = plugin;
    }

    public void Dispose() { }

    #endregion

    #region Window Overrides

    public override void PreDraw()
    {
        Flags &= ~ImGuiWindowFlags.NoMove;
    }

    public override void Draw()
    {
        DrawCreateGroupSection();
        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();
        DrawJoinGroupSection();

        // Example for future config options:
        /*
        var configValue = Configuration.SomePropertyToBeSavedAndWithADefault;
        if (ImGui.Checkbox("Random Config Bool", ref configValue))
        {
            Configuration.SomePropertyToBeSavedAndWithADefault = configValue;
            Configuration.Save();
        }

        var movable = Configuration.IsConfigWindowMovable;
        if (ImGui.Checkbox("Movable Config Window", ref movable))
        {
            Configuration.IsConfigWindowMovable = movable;
            Configuration.Save();
        }
        */
    }

    #endregion

    #region UI Sections

    private void DrawCreateGroupSection()
    {
        using var child = ImRaii.Child("createGroup", new Vector2(600, 250), true);
        if (!child.Success) return;

        ImGui.Text("Create a new group");
        ImGui.InputText("Group Name", ref name, 128);
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("Give a nice name!");

        ImGui.BeginDisabled(isCreatingGroup);
        if (ImGui.Button("Create Group"))
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                groupCreationResponse = "Group name cannot be empty.";
                return;
            }

            isCreatingGroup = true;
            groupCreationResponse = "Creating group...";
            newGroupCode = string.Empty;
            _ = CreateGroupAsync();
        }
        ImGui.EndDisabled();

        if (isCreatingGroup)
        {
            ImGui.Spacing();
            ImGui.Text(groupCreationResponse);
            ImGui.InputText("New code:", ref newGroupCode, 36, ImGuiInputTextFlags.ReadOnly | ImGuiInputTextFlags.AutoSelectAll);
        }
    }

    private void DrawJoinGroupSection()
    {
        using var child = ImRaii.Child("joinGroup", new Vector2(600, 250), true);
        if (!child.Success) return;

        ImGui.Text("Join a group");
        ImGui.InputText("Group code: ", ref groupCode, 128);
        if (ImGui.Button("Join Group"))
        {
            //try to create static
        }
    }

    #endregion

    #region Async Logic

    public async Task CreateGroupAsync()
    {
        var logger = new RaidLootAnnotator.Logging.PluginLogLoggerAdapter<RlaApiClient>(Plugin.Log);
        var client = new RlaApiClient(Plugin.httpClient, logger);

        try
        {
            var response = await client.PostStaticAndReadResponseAsync(name);
            if (response != null)
            {
                groupCreationResponse = $"Group created, COPY THE CODE!";
                newGroupCode = response.Guid.ToString();
            }
            else
            {
                groupCreationResponse = "Failed to create group.";
                Plugin.Log.Warning($"API response was null when creating group with name: {name}");
            }
        }
        catch (Exception ex)
        {
            groupCreationResponse = "An error occurred while creating the group.";
            Plugin.Log.Error(ex, $"Exception thrown during group creation for name: {name}");
        }
    }

    #endregion
}
