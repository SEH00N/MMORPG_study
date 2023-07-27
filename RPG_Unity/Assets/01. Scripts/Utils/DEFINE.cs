using System.Reflection;
using UnityEngine;

public class DEFINE
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster
    }

    public enum State
    {
        Die,
        Moving,
        Idle,
        Skill
    }

    public enum Layer
    {
        Monster = 1 << 6,
        Ground = 1 << 7,
        Block = 1 << 8,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game
    }

    public enum Sound
    {
        BGM,
        Effect,
        Length
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click
    }

    public enum CameraMode
    {
        QuaterView,
    }
}
