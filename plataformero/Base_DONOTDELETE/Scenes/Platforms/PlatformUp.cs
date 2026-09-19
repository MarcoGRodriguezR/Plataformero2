using Godot;
using System;

namespace EjJorge.Base_DONOTDELETE.Scenes.Platforms;

public partial class PlatformUp : _PlatformBase
{
    protected override void _ReadyExtras() => Main.PlatformsUps.Add(this);
}
