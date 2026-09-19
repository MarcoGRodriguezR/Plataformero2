using EjJorge.Base_DONOTDELETE.Scenes.GravStations;
using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

public abstract partial class _GravStationBase : Node
{
	protected abstract Player.GravDirection GetGravity();
	protected _GravStationHelper Helper;
	public override void _Ready()
	{
		Helper = GetNode<_GravStationHelper>("GravStation");
		Helper.GravDirection = GetGravity();
	}
}
