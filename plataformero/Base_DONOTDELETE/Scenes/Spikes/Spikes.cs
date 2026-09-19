using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Spikes;

public partial class Spikes : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        BodyEntered += Kill;
    }

    private void Kill(Node2D body)
    {
        if (body is Player player) player.Respawn();
    }
}
