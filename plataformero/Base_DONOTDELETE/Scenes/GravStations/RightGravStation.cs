using EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;
using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.GravStations;

public partial class RightGravStation : _GravStationBase
{
    protected override Player.GravDirection GetGravity() => Player.GravDirection.Right;
}
