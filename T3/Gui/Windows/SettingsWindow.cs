﻿using System.Collections.Generic;
using ImGuiNET;
using T3.Gui.Commands;
using T3.Gui.Graph;
using T3.Gui.TypeColors;
using T3.Gui.UiHelpers;

namespace T3.Gui.Windows
{
    public class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            Config.Title = "Settings";
        }

        public static bool UseVSync = true;

        public static bool WindowRegionsVisible;
        public static bool ItemRegionsVisible;

        protected override void DrawContent()
        {
            if (ImGui.TreeNode("User Interface"))
            {
                ImGui.Checkbox("Use arc connections", ref UserSettings.Config.UseArcConnections);
                ImGui.Checkbox("Use Jog Dial Control", ref UserSettings.Config.UseJogDialControl);
                ImGui.DragFloat("Scroll smoothing", ref UserSettings.Config.ScrollSmoothing);
                ImGui.Checkbox("Show Graph thumbnails", ref UserSettings.Config.ShowThumbnails);
                ImGui.Checkbox("Drag snapped nodes", ref UserSettings.Config.SmartGroupDragging);
                ImGui.Separator();
                ImGui.DragFloat("Snap strength", ref UserSettings.Config.SnapStrength);
                ImGui.DragFloat("Click threshold", ref UserSettings.Config.ClickThreshold);

                ImGui.DragFloat("Timeline Raster Density", ref UserSettings.Config.TimeRasterDensity, 0.01f);
                ImGui.Checkbox("Count Bars from Zero", ref UserSettings.Config.CountBarsFromZero);
                
                ImGui.Checkbox("Swap Main & 2nd windows when fullscreen", ref UserSettings.Config.SwapMainAnd2ndWindowsWhenFullscreen);
                ImGui.Checkbox("Save Only Modified Symbols", ref UserSettings.Config.SaveOnlyModified);
                ImGui.Checkbox("Enable Auto Backup", ref UserSettings.Config.AutoSaveAfterSymbolCreation);
                 
                ImGui.TreePop();
            }
            
            if (ImGui.TreeNode("Space Mouse"))
            {
                ImGui.DragFloat("Smoothing", ref UserSettings.Config.SpaceMouseDamping, 0.01f, 0.01f, 1f);
                ImGui.DragFloat("Move Speed", ref UserSettings.Config.SpaceMouseMoveSpeedFactor, 0.01f, 0, 10f);
                ImGui.DragFloat("Rotation Speed", ref UserSettings.Config.SpaceMouseRotationSpeedFactor, 0.01f, 0, 10f);
                ImGui.TreePop();
            }

            
            if (ImGui.TreeNode("Additional settings"))
            {
                //ImGui.Checkbox("Show Timeline", ref UserSettings.Config.ShowTimeline);
                //ImGui.Checkbox("Show Title", ref UserSettings.Config.ShowTitleAndDescription);
                ImGui.DragFloat("Gizmo size", ref UserSettings.Config.GizmoSize);
                ImGui.DragFloat("Tooltip delay", ref UserSettings.Config.TooltipDelay);
                ImGui.Checkbox("Save after symbol creating", ref UserSettings.Config.AutoSaveAfterSymbolCreation);
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Debug Options"))
            {
                ImGui.Checkbox("VSync", ref UseVSync);
                ImGui.Checkbox("Show Window Regions", ref WindowRegionsVisible);
                ImGui.Checkbox("Show Item Regions", ref ItemRegionsVisible);

                if (ImGui.TreeNode("Undo Queue"))
                {
                    ImGui.TextUnformatted("Undo");
                    ImGui.Indent();
                    foreach (var c in UndoRedoStack.UndoStack)
                    {
                        ImGui.Selectable(c.Name);
                    }

                    ImGui.Unindent();
                    ImGui.Spacing();
                    ImGui.TextUnformatted("Redo");
                    ImGui.Indent();
                    foreach (var c in UndoRedoStack.RedoStack)
                    {
                        ImGui.Selectable(c.Name);
                    }

                    ImGui.Unindent();
                    ImGui.TreePop();
                }
                
                if (ImGui.TreeNode("Modified Symbols"))
                {
                    foreach (var symbolUi in UiModel.GetModifiedSymbolUis())
                    {
                        if (symbolUi.HasBeenModified)
                        {
                            ImGui.TextUnformatted(symbolUi.Symbol.Namespace + ". " +  symbolUi.Symbol.Name);
                        }
                    }
                    
                    ImGui.TreePop();
                }                
                
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Look (not saved)"))
            {
                ColorVariations.DrawSettingsUi();

                if (ImGui.TreeNode("T3 Ui Style"))
                {
                    ImGui.DragFloat("Height Connection Zone", ref GraphNode.UsableSlotThickness);
                    ImGui.DragFloat2("Label position", ref GraphNode.LabelPos);
                    ImGui.DragFloat("Slot Gaps", ref GraphNode.SlotGaps, 0.1f, 0, 10f);
                    ImGui.DragFloat("Input Slot Margin Y", ref GraphNode.InputSlotMargin, 0.1f, 0, 10f);
                    ImGui.DragFloat("Input Slot Thickness", ref GraphNode.InputSlotThickness, 0.1f, 0, 10f);
                    ImGui.DragFloat("Output Slot Margin", ref GraphNode.OutputSlotMargin, 0.1f, 0, 10f);

                    ImGui.ColorEdit4("ValueLabelColor", ref T3Style.Colors.ValueLabelColor.Rgba);
                    ImGui.ColorEdit4("ValueLabelColorHover", ref T3Style.Colors.ValueLabelColorHover.Rgba);
                }
                
                if (ImGui.TreeNode("T3 Graph colors"))
                {
                    T3Style.DrawUi();
                    ImGui.TreePop();
                }                
                ImGui.TreePop();
            }
            
            ImGui.Separator();
            T3Metrics.Draw();
        }

        public override List<Window> GetInstances()
        {
            return new List<Window>();
        }
    }
}