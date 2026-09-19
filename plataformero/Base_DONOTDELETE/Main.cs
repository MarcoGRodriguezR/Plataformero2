using EjJorge.Base_DONOTDELETE.Scenes.CheckPoint;
using EjJorge.Base_DONOTDELETE.Scenes.Platforms;
using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using EjJorge.Base_DONOTDELETE.Scenes.Pillars;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff.Player;

namespace EjJorge.Base_DONOTDELETE;

public partial class Main : Node2D
{
    public List<CheckPoint> CheckPoints = [];
    public List<PlatformUp> PlatformsUps = [];
    public List<PlatformDown> PlatformDowns = [];
    public List<PlatformLeft> PlatformLefts = [];
    public List<PlatformRight> PlatformRights = [];
    public List<_PillarBase> Pillars = [];

    public Player Player;

    public override void _Ready()
    {
        Player = GetNode<Player>("Player");
        Player.Main = this;
    }

    bool firstFrame = false;
    public void CheckCheckPoints() { foreach (var c in CheckPoints) c.UpdateGraphic(); }

    /// <summary>
    /// <para>Used to make the checks for <see cref="PlatformDowns"/> (and the others) only run once per ignore is changed</para>
    /// <para>Use 0 to reset, +1 to go through platform, -1 to not go through</para>
    /// </summary>
    public int PrevIgnore = 0;
    public override void _Process(double delta)
    {
        if (!firstFrame)
        {
            CheckCheckPoints();
            foreach (var p in PlatformsUps) p.VisualUpdate();
            foreach (var p in PlatformDowns) p.VisualUpdate();
            foreach (var p in PlatformLefts) p.VisualUpdate();
            foreach (var p in PlatformRights) p.VisualUpdate();

            foreach (var p in Pillars) p.VisualUpdate();
            firstFrame = true;
        }

        int ignore;
        switch (Player.Gravity)
        {
            case GravDirection.Down:
                ignore = Input.IsActionPressed("ui_down") ? +1 : -1;
                if (ignore != PrevIgnore)
                {
                    PrevIgnore = ignore;
                    foreach (var p in PlatformsUps) p.Hitbox.Disabled = ignore == +1;
                    foreach (var p in PlatformDowns) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformLefts) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformRights) p.Hitbox.Disabled = false;
                }
                break;

            case GravDirection.Up:
                ignore = Input.IsActionPressed("ui_up") ? +1 : -1;
                if (ignore != PrevIgnore)
                {
                    PrevIgnore = ignore;
                    foreach (var p in PlatformsUps) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformDowns) p.Hitbox.Disabled = ignore == +1;
                    foreach (var p in PlatformLefts) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformRights) p.Hitbox.Disabled = false;
                }
                break;

            case GravDirection.Left:
                ignore = Input.IsActionPressed("ui_left") ? +1 : -1;
                if (ignore != PrevIgnore)
                {
                    PrevIgnore = ignore;
                    foreach (var p in PlatformsUps) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformDowns) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformLefts) p.Hitbox.Disabled = ignore == +1;
                    foreach (var p in PlatformRights) p.Hitbox.Disabled = false;
                }
                break;

            case GravDirection.Right:
                ignore = Input.IsActionPressed("ui_right") ? +1 : -1;
                if (ignore != PrevIgnore)
                {
                    PrevIgnore = ignore;
                    foreach (var p in PlatformsUps) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformDowns) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformLefts) p.Hitbox.Disabled = false;
                    foreach (var p in PlatformRights) p.Hitbox.Disabled = ignore == +1;
                }
                break;
        }
    }
}
