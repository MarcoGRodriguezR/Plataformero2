using EjJorge.Base_DONOTDELETE;
using Godot;
using System;

public partial class _PlatformBase : Node2D
{
    public Main Main;
    public CollisionShape2D Hitbox;

    public Area2D LeftBlock, LeftPlatform, RightBlock, RightPlatform;

    public AnimatedSprite2D Sprite;

    protected CollisionTypes Left, Right;
    protected enum CollisionTypes
    {
        None, Block, Platform
    }

    public sealed override void _Ready()
    {
        Main = FindParent("Main") as Main;
        if (Main is null) throw new Exception($"Main is null");

        Hitbox = GetNode<CollisionShape2D>("Hitbox");

        Sprite = GetNode<AnimatedSprite2D>("Sprite");

        LeftBlock = GetNode<Area2D>("Left Block");
        LeftBlock.BodyEntered += Left_BodyEntered;

        LeftPlatform = GetNode<Area2D>("Left Platform");
        LeftPlatform.BodyEntered += LeftBlock_BodyEntered;

        RightBlock = GetNode<Area2D>("Right Block");
        RightBlock.BodyEntered += Right_BodyEntered;

        RightPlatform = GetNode<Area2D>("Right Platform");
        RightPlatform.BodyEntered += RightBlock_BodyEntered;

        _ReadyExtras();
    }
    protected virtual void _ReadyExtras() { }

    #region Physic Collisions
    private void Left_BodyEntered(Node2D body)
    {
        if (body is not TileMapLayer layer) return;
        //Debug.Print($"I am {Position}, found block {body.Name} to my left");
        Left = CollisionTypes.Block;
    }

    private void LeftBlock_BodyEntered(Node2D body)
    {
        //if (body is PlatformUp) { Debug.Print($"Yes, found platform, hmm"); }
        //if (body is not TileMapLayer layer) return;
        //Debug.Print($"I am {Position}, found platform {body.Name} to my left, {body.GetType()}");
        Left = CollisionTypes.Platform;
    }
    private void Right_BodyEntered(Node2D body)
    {
        if (body is not TileMapLayer) return;
        //Debug.Print($"I am {Position}, found block {body.Name} to my Right");
        Right = CollisionTypes.Block;
    }

    private void RightBlock_BodyEntered(Node2D body)
    {
        //if (body is PlatformUp) { Debug.Print($"Yes, found platform, hmm"); }
        //if (body is not TileMapLayer layer) return;
        //Debug.Print($"I am {Position}, found platform {body.Name} to my Right, {body.GetType()}");
        Right = CollisionTypes.Platform;
    }
    #endregion

    public void VisualUpdate()
    {
        //LeftBlock.SetCollisionMaskValue(4, false);
        //LeftPlatform.SetCollisionMaskValue(4, false);
        //RightBlock.SetCollisionMaskValue(4, false);
        //RightPlatform.SetCollisionMaskValue(4, false);

        LeftBlock.QueueFree(); LeftPlatform.QueueFree();
        RightBlock.QueueFree(); RightPlatform.QueueFree();

        //Debug.Print($"I am {GlobalPosition}, checking: {Left}, {Right}");

        //if (Left is CollisionTypes.None && Right is CollisionTypes.None) Sprite.Play("default");
        //else 
        string type = "default";
        if (Left is CollisionTypes.None)
        {
            type = Right switch
            {
                CollisionTypes.None => "default",
                CollisionTypes.Block => "Right_Lone",
                CollisionTypes.Platform => "Left_Empty"
            };
        }
        else if (Left is CollisionTypes.Platform)
        {
            type = Right switch
            {
                CollisionTypes.None => "Right_Empty",
                CollisionTypes.Block => "Right",
                CollisionTypes.Platform => "Middle"
            };
        }
        else if (Left is CollisionTypes.Block)
        {
            type = Right switch
            {
                CollisionTypes.None => "Left_Lone",
                CollisionTypes.Block => "default",
                CollisionTypes.Platform => "Left"
            };
        }
        Sprite.Play(type);

    }
}
