using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.GravStations;
public partial class _GravStationHelper : Node2D
{
	public Player.GravDirection GravDirection;
	protected Area2D Hitbox;
	public override void _Ready()
	{
		Hitbox = GetNode<Area2D>("Hitbox");
        Hitbox.BodyEntered += Hitbox_BodyEntered;
	}

    private void Hitbox_BodyEntered(Node2D body)
    {
		if (body is Player player) player.SetGravity(GravDirection);
    }
}
