using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        // I don't know if this works.
        public static void Invoke(float seconds, Delegate method, params object[] args) {
            Clock clock = new Clock();
            while (clock.ElapsedTime.AsSeconds() < seconds) { }
            method.DynamicInvoke(args);
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

        public override string ToString() {
            return "[DotCreator App] Title(\"" + title + "\") Size(" + size.X + ", " + size.Y + ")";
        }

        public void SetPixel(int pixel, Color color) {
            dots[pixel].FillColor = color;
        }

        public Color GetPixel(int pixel) {
            return dots[pixel].FillColor;
        }

        public App(string _title) {
            title = _title;
            size = new Vector2u(502, 502);
        }

        public void Run() {
            if (prestartevent != null)
                if (prestartevent())
                    return;
            window = new RenderWindow(new VideoMode(size.X, size.Y), title, Styles.Close);
            window.SetFramerateLimit(60);
            window.SetVerticalSyncEnabled(true);

            if (startevent != null)
                if (startevent())
                    return;

            dots = new List<CircleShape>();

            int i = 0;

            for (int y = 0; y < 12; y++) {
                for (int x = 0; x < 12; x++) {
                    dots.Add(new CircleShape(20.0f, 100));
                    dots[i].FillColor = Color.Green;
                    dots[i].Position = new Vector2f(42 * x, 42 * y);
                    i++;
                }
            }

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

            if (poststartevent != null)
                if (poststartevent())
                    return;

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
