using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Platforms;

public partial class PlatformRight: _PlatformBase
{
    protected override void _ReadyExtras() => Main.PlatformRights.Add(this);
}
