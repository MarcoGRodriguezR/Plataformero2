using EjJorge.Base_DONOTDELETE.Scenes.Pillars;
using EjJorge.Base_DONOTDELETE.Scenes.Platforms;
using Godot;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EjJorge.Base_DONOTDELETE.Scenes.PlayerStuff;

public partial class Player : CharacterBody2D
{
    public float Speed = 160.0f;
    public float JumpVelocity = -350.0f;
    public bool SlowFall = false;
    public float FallMaxNormal = 500, FallMaxSlow = 150;

    public enum GravDirection
    {
        Down,
        Up,
        Right,
        Left
    }
    [Export]
    public GravDirection Gravity { get; protected set; } = GravDirection.Down;
    public void SetGravity(GravDirection gravity)
    {
        Main.PrevIgnore = 0;
        Gravity = gravity;
    }

    #region Respawn logic
    public Vector2 RespawnSpot;
    public void Respawn()
    {
        //Debug.Print($"Respawned");
        Inertia = 0;
        Position = RespawnSpot;
        RespawnFrame = true;
    }
    /// <summary>
    /// Used to stop all momentum in one frame when <see cref="Respawn"/> is called
    /// </summary>
    protected bool RespawnFrame = false;
    #endregion

    #region Main menu
    public void GoToMainMenu()
    {
        string UID = "uid://bnh1udvsrppga";

        Error error = GetTree().ChangeSceneToFile(UID);

        if (error is not Error.Ok) Debug.Print($"error cambiando a {UID}, {error}");
    }
    #endregion

    #region Ready Stuff
    public Sprite2D Body;
    public CollisionShape2D Hitbox;
    public Area2D LeftHitbox, RightHitbox;
    protected bool OnLeftFloor = false, OnRightFloor = false;

    protected Camera2D Camera;
    protected Button ZoomCamera, UnZoomCamera;
    protected Label PositonLBL;

    protected RayCast2D LeftTop, LeftBottom, RightTop, RightBottom;

    public Main Main;
    public override void _Ready()
    {
        Body = GetNode<Sprite2D>("Body");
        Hitbox = GetNode<CollisionShape2D>("Hitbox");

        LeftHitbox = GetNode<Area2D>("LeftHitbox");

        RightHitbox = GetNode<Area2D>("RightHitbox");
        //RightHitbox.Body

        RespawnSpot = Position;

        Camera = GetNode<Camera2D>("Camera2D");
        PositonLBL = GetNode<Label>("Camera2D/UI/HBoxContainer/Left/Position");
        ZoomCamera = GetNode<Button>("Camera2D/UI/HBoxContainer/Right/ZoomCamera");
        ZoomCamera.Pressed += ZoomingCamera;
        UnZoomCamera = GetNode<Button>("Camera2D/UI/HBoxContainer/Right/UnZoomCamera");
        UnZoomCamera.Pressed += UnZoomingCamera;

        //LeftTop = GetNode<RayCast2D>("LeftTop");
        //LeftBottom = GetNode<RayCast2D>("LeftBottom");
        //RightTop = GetNode<RayCast2D>("RightTop");
        //RightBottom = GetNode<RayCast2D>("RightBottom");
    }

    #region Camera Zoom and UnZoom
    /// <summary>
    /// Used for <see cref="ZoomingCamera"/> and <see cref="UnZoomingCamera"/>
    /// </summary>
    protected const float maxZoom = 3, minZoom = 1, interval = 0.5f;
    private void ZoomingCamera()
    {
        float zoomLevel = Camera.Zoom.X;

        if (zoomLevel < maxZoom) zoomLevel += interval;
        else zoomLevel = maxZoom;

        Camera.Zoom = new(zoomLevel, zoomLevel);
    }
    private void UnZoomingCamera()
    {
        float zoomLevel = Camera.Zoom.X;

        if (zoomLevel > minZoom) zoomLevel -= interval;
        else zoomLevel = minZoom;

        Camera.Zoom = new(zoomLevel, zoomLevel);
    }
    #endregion

    #endregion

    protected double Inertia = 0;
    protected double InertiaMAX = 0.1;
    public void ResetInertia() => Inertia = InertiaMAX;

    protected double CoyoteTimer = 0, CoyoteTimerMAX = 0.1;
    public override void _PhysicsProcess(double delta)
    {
        PositonLBL.Text = $"Position: ({(int)(GlobalPosition.X / 16)}, {(int)(GlobalPosition.Y / 16)}) \n" +
            $"Velocity: ({(int)(Velocity.X / 16)}, {(int)(Velocity.Y / 16)});" +
            $"Coyote: {CoyoteTimer}";

        #region Respawn FailSafe
        if (RespawnFrame)
        {
            RespawnFrame = false;
            Velocity = new(0, 0);
            MoveAndSlide();
            return;
        }
        #endregion

        Vector2 velocity = Velocity;

        if (Input.IsActionJustPressed("Restart")) Respawn();
        if (Input.IsActionJustPressed("ui_cancel")) GoToMainMenu();

        float maxFall = SlowFall ? FallMaxSlow : FallMaxNormal;
        float grav = GetGravity().Y;
        float jumpVel = JumpVelocity;

        if (SlowFall) grav /= 3;

        if (Gravity is GravDirection.Up or GravDirection.Left)
        {
            maxFall *= -1;
            grav *= -1;
            jumpVel *= -1;
        }

        #region Gravity and Coyote Timer
        bool isOnFloor = false;
        switch (Gravity)
        {
            case GravDirection.Down: isOnFloor = IsOnFloor(); break;
            case GravDirection.Up: isOnFloor = IsOnCeiling(); break;
            case GravDirection.Left: foreach (Node2D body in LeftHitbox.GetOverlappingBodies()) if (body is TileMapLayer or PlatformLeft or _PillarBase) isOnFloor = true; break;
            case GravDirection.Right: foreach (Node2D body in RightHitbox.GetOverlappingBodies()) if (body is TileMapLayer or PlatformRight or _PillarBase) isOnFloor = true; break;
        }

        if (!isOnFloor)
        {
            bool tooFast;

            if (Gravity is GravDirection.Down or GravDirection.Up)
            {
                velocity.Y += grav * (float)delta;
                tooFast = Gravity switch
                {
                    GravDirection.Down => velocity.Y > maxFall,
                    GravDirection.Up => velocity.Y < maxFall,
                };
                if (tooFast) velocity.Y = maxFall;
            }
            else
            {
                velocity.X += grav * (float)delta;
                tooFast = Gravity switch
                {
                    GravDirection.Left => velocity.X < maxFall,
                    GravDirection.Right => velocity.X > maxFall,
                };
                if (tooFast) velocity.X = maxFall;
            }
            
            if (CoyoteTimer > 0) CoyoteTimer -= delta;
        }
        else CoyoteTimer = CoyoteTimerMAX;
        #endregion

        #region Jumping
        string input = Gravity switch
        {
            GravDirection.Down => "ui_up",
            GravDirection.Up => "ui_down",
            GravDirection.Left => "ui_right",
            GravDirection.Right => "ui_left"
        };
        if (Input.IsActionPressed(input) && CoyoteTimer > 0)
        {
            if (Gravity is GravDirection.Down or GravDirection.Up) velocity.Y = jumpVel;
            else velocity.X = jumpVel;

            CoyoteTimer = 0;
        }
        #endregion

        #region Movement
        if (Inertia > 0) Inertia -= delta;
        if (Inertia <= 0)
        {
            if (Gravity is GravDirection.Down or GravDirection.Up)
            {
                float horDirection = Input.GetAxis("ui_left", "ui_right");
                horDirection *= Speed;

                velocity.X = Mathf.MoveToward(
                velocity.X,
                horDirection,
                Speed / 6
                );
            }
            else
            {
                float verDirection = Input.GetAxis("ui_up", "ui_down");
                verDirection *= Speed;

                velocity.Y = Mathf.MoveToward(
                velocity.Y,
                verDirection,
                Speed / 6
                );
            }
        }
        #endregion

        #region Flip Sprite
        if (Gravity is GravDirection.Down or GravDirection.Up)
        {
            Body.FlipH = velocity.X switch
            {
                > 0 => false,
                < 0 => true,
                _ => Body.FlipH,
            };

            Body.FlipV = Gravity is GravDirection.Up;

            Body.RotationDegrees = 0;
            Hitbox.RotationDegrees = 0;
        }
        else
        {
            Body.FlipH = velocity.Y switch
            {
                > 0 => false,
                < 0 => true,
                _ => Body.FlipH,
            };

            Body.FlipV = Gravity is GravDirection.Right;

            Body.RotationDegrees = 90;
            Hitbox.RotationDegrees = 90;
        }
        #endregion

        Velocity = velocity;
        MoveAndSlide();
    }
}
