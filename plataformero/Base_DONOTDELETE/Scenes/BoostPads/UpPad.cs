using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;
using System.Diagnostics;

namespace EjJorge.Base_DONOTDELETE.Scenes.BoostPads;

public partial class UpPad : _BoostPadBase
{
    protected override void Boost(Node2D body)
    {
        if (body is Player player)
        {
            if (player.Gravity is Player.GravDirection.Left or Player.GravDirection.Right) player.ResetInertia();

            if (player.Velocity.Y < 0) player.Velocity = new Vector2(player.Velocity.X, player.Velocity.Y - 120);
            else player.Velocity = new Vector2(player.Velocity.X, -450);
        }
    }
}
