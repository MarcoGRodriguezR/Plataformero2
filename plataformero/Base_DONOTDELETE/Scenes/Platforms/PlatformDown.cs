using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Platforms;

public partial class PlatformDown : _PlatformBase
{
    protected override void _ReadyExtras() => Main.PlatformDowns.Add(this);
}
