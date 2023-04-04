using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;
using SFML.System;
using Dot.Event;
using MouseWheelEvent = Dot.Event.MouseWheelEvent;
using MouseMoveEvent = Dot.Event.MouseMoveEvent;

namespace Dot {

    public static class Util {
        public static string getTitle(Window window) {
            return Process.GetCurrentProcess().MainWindowTitle;
        }

        public static void Invoke(float seconds, Delegate method, params object[] args) {
            Thread thread = new Thread(() => {
                Clock clock = new();
                while (clock.ElapsedTime.AsSeconds() < seconds) { }
                method.DynamicInvoke(args);
            });
            thread.Start();
        }

        public static Vector2i GetCoordinate(List<Shape> dots, int i) {
            int x = 0;
            int y = 0;
            if (!(dots[i].Position.X == 0))
                x = (int) (dots[i].Position.X / 42);
            if (!(dots[i].Position.Y == 0))
                y = (int) (dots[i].Position.Y / 42);
            return new Vector2i(x, y);
        }

        public static Vector2i GetCoordinate(CircleShape shape) {
            int x = 0;
            int y = 0;
            if (!(shape.Position.X == 0))
                x = (int) (shape.Position.X / 42);
            if (!(shape.Position.Y == 0))
                y = (int) (shape.Position.Y / 42);
            return new Vector2i(x, y);
        }

        // Returns a shape if true, or null if false.
        public static CircleShape? DotExists(List<CircleShape> old, Vector2i neww) {
            foreach(CircleShape shape in old) {
                if(GetCoordinate(shape) == neww) {
                    //Console.WriteLine(shape.Position.ToString() + ", " + neww.ToString());
                    return shape;
                }
            }
            return null;
        }
    }

    public class App {

        public event PreStartEvent? prestartevent;
        public event StartEvent? startevent;
        public event PostStartEvent? poststartevent;
        public event RunEvent? runevent;
        public event PostRunEvent? postrunevent;
        public event DrawEvent? drawevent;
        public event CloseEvent? closeevent;
        public event MouseDotClickEvent? mousedotclickevent;
        public event MouseWheelEvent? mousewheelevent;
        public event MouseClickEvent? mouseclickevent;
        public event MouseReleaseEvent? mousereleaseevent;
        public event MouseCoordinateClickEvent? mousecoordinateclickevent;
        public event MouseCoordinateReleaseEvent? mousecoordinatereleaseevent;
        public event MouseDotReleaseEvent? mousedotreleaseevent;
        public event MouseDotMoveEvent? mousedotmoveevent;
        public event MouseMoveEvent? mousemoveevent;
        public event MouseCoordinateMoveEvent? mousecoordinatemoveevent;
        public event KeyPressedEvent? keypressedevent;
        public event KeyReleasedEvent? keyreleasedevent;

        public string title;
        public Vector2u size;

        public List<CircleShape> dots = new List<CircleShape>();
        public RenderWindow window;

        public int dotAmountX = 12;
        public int dotAmountY = 12;

        public bool resizeable = false;

        public int dotSize = 40;

        public Vector2i GetCoordinate(int i) {
            int x = 0;
            int y = 0;
            if (!(dots[i].Position.X == 0))
                x = (int) (dots[i].Position.X / 42);
            if (!(dots[i].Position.Y == 0))
                y = (int) (dots[i].Position.Y / 42);
            return new Vector2i(x, y);
        }

        public void ReCalculateDots(Vector2u windowSize, int dotSize) {
            dotAmountX = (int)windowSize.X / (dotSize + 2);
            dotAmountY = (int)windowSize.Y / (dotSize + 2);

            Console.WriteLine("X: " + dotAmountX + " Y: " + dotAmountY);

            List<CircleShape> newdots = new List<CircleShape>();

            int i = 0;

            for (int y = 0; y < dotAmountY; y++) {
                for (int x = 0; x < dotAmountX; x++) {
                    newdots.Add(new CircleShape(dotSize / 2, 100));
                    newdots[i].Position = new Vector2f((dotSize + 2) * x, (dotSize + 2) * y);
                    CircleShape ?olddot = Util.DotExists(dots, new Vector2i(x, y));
                    newdots[i].FillColor = olddot != null ? olddot.FillColor : Color.Green;
                    i++;
                }
            }
            dots.Clear();
            dots = newdots;
        }

        public override string ToString() {
            return "[DotCreator App] Title(\"" + title + "\") Size(" + size.X + ", " + size.Y + ")";
        }

        public void SetPixel(int pixel, Color color) {
            dots[pixel].FillColor = color;
        }

        public Color GetPixel(int pixel) {
            return dots[pixel].FillColor;
        }

        public App(string _title, Vector2u _size, bool _resizeable) {
            title = _title;
            size = _size;
            resizeable = _resizeable;
        }

        public void Run() {
            if (prestartevent != null)
                if (prestartevent())
                    return;
            window = new RenderWindow(new VideoMode(size.X, size.Y), title, resizeable ? Styles.Default : Styles.Close);
            window.SetFramerateLimit(60);
            window.SetVerticalSyncEnabled(true);

            if (startevent != null)
                if (startevent())
                    return;

            dots = new List<CircleShape>();

            ReCalculateDots(size, dotSize);

            if (closeevent == null)
                window.Closed += (_, __) => window.Close();
            else
                window.Closed += (_, __) => closeevent();

            window.MouseButtonPressed += MouseButtonPressed;
            window.MouseWheelScrolled += MouseScrolled;
            window.MouseButtonReleased += MouseButtonReleased;
            window.MouseMoved += MouseMoved;
            window.KeyPressed += KeyPressed;
            window.KeyReleased += KeyReleased;
            window.Resized += (sender, e) => {
                window.Size = new Vector2u(e.Width, e.Height);
                View view = new View(new FloatRect(0.0f, 0.0f, e.Width, e.Height));
                window.SetView(view);
                ReCalculateDots(new Vector2u((uint)e.Width, (uint)e.Height), dotSize);
            };

            if (poststartevent != null)
                if (poststartevent())
                    return;

            int i = 0;

            while (window.IsOpen) {
                window.DispatchEvents();
                if (runevent != null)
                    runevent();

                if (!(Util.getTitle(window).Equals(title))) {
                    window.SetTitle(title);
                }

                window.Clear();

                i = 0;
                while (i < dots.Count) {
                    window.Draw(dots[i]);
                    i++;
                }

                if (drawevent != null)
                    drawevent();

                window.Display();

                if (postrunevent != null)
                    postrunevent();
            }
        }



        private void MouseButtonPressed(object sender, MouseButtonEventArgs e) {
            if (mousedotclickevent != null || mousecoordinateclickevent != null) {
                for (int i = 0; i < dots.Count; i++) {
                    if (e.X < dots[i].Position.X + 42 && e.Y < dots[i].Position.Y + 42) {
                        if (mousedotclickevent != null)
                            mousedotclickevent(i, e.Button);
                        if (mousecoordinateclickevent != null) {
                            int x = 0;
                            int y = 0;
                            if (!(dots[i].Position.X == 0))
                                x = (int) (dots[i].Position.X / 42);
                            if (!(dots[i].Position.Y == 0))
                                y = (int) (dots[i].Position.Y / 42);
                            mousecoordinateclickevent(new Vector2i(x, y), e.Button);
                        }
                        break;
                    }
                }
            }
            if (mouseclickevent != null)
                mouseclickevent(new Vector2f(e.X, e.Y), e.Button);
        }

        private void MouseButtonReleased(object sender, MouseButtonEventArgs e) {
            if (mousedotreleaseevent != null || mousecoordinatereleaseevent != null) {
                for (int i = 0; i < dots.Count; i++) {
                    if (e.X < dots[i].Position.X + 42 && e.Y < dots[i].Position.Y + 42) {
                        if (mousedotreleaseevent != null)
                            mousedotreleaseevent(i, e.Button);
                        if (mousecoordinatereleaseevent != null) {
                            int x = 0;
                            int y = 0;
                            if (!(dots[i].Position.X == 0))
                                x = (int) (dots[i].Position.X / 42);
                            if (!(dots[i].Position.Y == 0))
                                y = (int) (dots[i].Position.Y / 42);
                            mousecoordinatereleaseevent(new Vector2i(x, y), e.Button);
                        }
                        break;
                    }
                }
            }
            if (mousereleaseevent != null)
                mousereleaseevent(new Vector2f(e.X, e.Y), e.Button);
        }

        private void MouseScrolled(object sender, MouseWheelScrollEventArgs e) {
            if (mousewheelevent == null)
                return;

            mousewheelevent(e.Wheel, e.Delta, new Vector2i(e.X, e.Y));
        }

        private void MouseMoved(object sender, MouseMoveEventArgs e) {
            if (mousedotmoveevent != null || mousecoordinatemoveevent != null) {
                for (int i = 0; i < dots.Count; i++) {
                    if (e.X < dots[i].Position.X + 42 && e.Y < dots[i].Position.Y + 42) {
                        if (mousedotmoveevent != null)
                            mousedotmoveevent(i);
                        if (mousecoordinatemoveevent != null) {
                            int x = 0;
                            int y = 0;
                            if (!(dots[i].Position.X == 0))
                                x = (int) (dots[i].Position.X / 42);
                            if (!(dots[i].Position.Y == 0))
                                y = (int) (dots[i].Position.Y / 42);
                            mousecoordinatemoveevent(new Vector2i(x, y));
                        }
                        break;
                    }
                }
            }
            if (mousemoveevent != null) {
                mousemoveevent(new Vector2f(e.X, e.Y));
            }
        }

        private void KeyPressed(object sender, KeyEventArgs e) {
            if (keypressedevent != null)
                keypressedevent(e);
        }
        private void KeyReleased(object sender, KeyEventArgs e) {
            if (keyreleasedevent != null)
                keyreleasedevent(e);
        }
    }
}
