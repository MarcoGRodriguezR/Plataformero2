using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Speed;

public partial class Slow : Area2D
{
	public override void _Ready()
    {
        BodyEntered += Slow_BodyEntered;
    }

    private void Slow_BodyEntered(Node2D body)
    {
        if (body is Player player) player.SlowFall = true;
    }
}
