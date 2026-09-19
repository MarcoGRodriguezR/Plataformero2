using EjJorge.Base_DONOTDELETE.Scenes.BoostPads;
using Godot;
using System;

public abstract partial class _BoostPadBase : Node2D
{
	public _BoostPadHelper BoostPad;

	public override void _Ready()
	{
		BoostPad = GetNode<_BoostPadHelper>("BoostPad");
		BoostPad.Hitbox.BodyEntered += Boost;
	}
	protected abstract void Boost(Node2D body);
}
