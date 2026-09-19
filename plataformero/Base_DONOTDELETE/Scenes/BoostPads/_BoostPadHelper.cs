using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.BoostPads;
public partial class _BoostPadHelper : Node2D
{
	public Area2D Hitbox;

	public override void _Ready()
	{
		Hitbox = GetNode<Area2D>("Hitbox");
	}
}
