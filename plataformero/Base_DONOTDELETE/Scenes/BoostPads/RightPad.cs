using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.BoostPads;

public partial class RightPad : _BoostPadBase
{
    protected override void Boost(Node2D body)
    {
        if (body is Player player)
        {
            if (player.Gravity is Player.GravDirection.Down or Player.GravDirection.Up) player.ResetInertia();

            float vel = player.Velocity.X >= 450 ? player.Velocity.X + 120 : +450;
            player.Velocity = new(vel, player.Velocity.Y);
        }
    }
}
