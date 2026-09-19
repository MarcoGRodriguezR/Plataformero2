using EjJorge.Base_DONOTDELETE;
using Godot;
using System;
using System.Diagnostics;

namespace EjJorge.Base_DONOTDELETE.Scenes.Pillars;

public partial class _PillarBase : StaticBody2D
{
    public Main Main;
    public Area2D Top, Bottom;
    protected bool DevTest = false;
    protected AnimatedSprite2D Sprite;

    protected TouchTypes TopTouch, BottomTouch;
    protected enum TouchTypes
    {
        None, Block, Pillar
    }

    public sealed override void _Ready()
    {
        Main = FindParent("Main") as Main;

        Main.Pillars.Add(this);

        Top = GetNode<Area2D>("Top");
        Top.BodyEntered += Top_BodyEntered;

        Bottom = GetNode<Area2D>("Bottom");
        Bottom.BodyEntered += Bottom_BodyEntered;

        Sprite = GetNode<AnimatedSprite2D>("Sprite");
    }

    private void Bottom_BodyEntered(Node2D body)
    {
        if (DevTest) Debug.Print($"I am ({Position.X / 16}, {Position.Y / 16}), I have touched below");

        if (body is TileMapLayer layer)
        {
            if (DevTest) Debug.Print($"Found block {layer.Name}");
            BottomTouch = TouchTypes.Block;
        }
        else if (body is _PillarBase pillar)
        {
            if (DevTest) Debug.Print($"Found pillar {pillar.Name}");
            BottomTouch = TouchTypes.Pillar;
        }
        else if (DevTest) Debug.Print($"Found neither, thus {body.GetType()} {body.Name}");
    }

    private void Top_BodyEntered(Node2D body)
    {
        if (DevTest) Debug.Print($"I am ({Position.X / 16}, {Position.Y / 16}), I have touched above");

        if (body is TileMapLayer layer)
        {
            if (DevTest) Debug.Print($"Found block {layer.Name}");
            TopTouch = TouchTypes.Block;
        }
        else if (body is _PillarBase pillar)
        {
            if (DevTest) Debug.Print($"Found pillar {pillar.Name}");
            TopTouch = TouchTypes.Pillar;
        }
        else if (DevTest) Debug.Print($"Found neither, thus {body.GetType()} {body.Name}");
    }

    public void VisualUpdate()
    {
        Top.QueueFree();
        Bottom.QueueFree();

        if (DevTest) Debug.Print($"I am ({Position.X / 16}, {Position.Y / 16}), I have {TopTouch} above and {BottomTouch} below");

        string type = "default";
        if (TopTouch is TouchTypes.None)
        {
            type = BottomTouch switch
            {
                TouchTypes.None => "Floater",
                TouchTypes.Pillar => "Top",
                TouchTypes.Block => "default"
            };
        }
        else if (TopTouch is TouchTypes.Block)
        {
            type = BottomTouch switch
            {
                TouchTypes.None => "Void",
                TouchTypes.Pillar => "Middle",
                TouchTypes.Block => "Bottom",
            };
        }
        else if (TopTouch is TouchTypes.Pillar)
        {
            type = BottomTouch switch
            {
                TouchTypes.None => "Void",
                TouchTypes.Pillar => "Middle",
                TouchTypes.Block => "Bottom",
            };
        }

        Sprite.Play(type);
    }
}
