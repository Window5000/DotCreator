using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;

namespace Dot.Event {
    public delegate void RunEvent();
    public delegate void PostRunEvent();
    public delegate void MouseDotClickEvent(int dot, Mouse.Button button);
    public delegate void MouseClickEvent(Vector2f position, Mouse.Button button);
    public delegate void MouseCoordinateClickEvent(Vector2i coordinate, Mouse.Button button);
    public delegate void MouseCoordinateReleaseEvent(Vector2i coordinate, Mouse.Button button);
    public delegate void MouseDotReleaseEvent(int dot, Mouse.Button button);
    public delegate void MouseReleaseEvent(Vector2f position, Mouse.Button button);
    public delegate void MouseDotMoveEvent(int dot);
    public delegate void MouseMoveEvent(Vector2f position);
    public delegate void MouseCoordinateMoveEvent(Vector2i coordinate);
    public delegate void MouseWheelEvent(Mouse.Wheel wheel, float amount, Vector2i position);
    public delegate void CloseEvent();
    public delegate bool PreStartEvent();
    public delegate bool StartEvent();
    public delegate bool PostStartEvent();
    public delegate void DrawEvent();
    public delegate void KeyPressedEvent(KeyEventArgs args);
    public delegate void KeyReleasedEvent(KeyEventArgs args);
}
