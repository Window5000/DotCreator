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

public static class Util {
    public static string getTitle(Window window) {
        Console.WriteLine(Process.GetCurrentProcess().MainWindowTitle);
        return Process.GetCurrentProcess().MainWindowTitle;
    }
}

namespace Dot {

    public class App {

        public event PreStartEvent prestartevent;
        public event StartEvent startevent;
        public event PostStartEvent poststartevent;
        public event RunEvent runevent;
        public event PostRunEvent postrunevent;
        public event DrawEvent drawevent;
        public event CloseEvent closeevent;
        public event MouseDotClickEvent mousedotclickevent;
        public event MouseWheelEvent mousewheelevent;
        public event MouseClickEvent mouseclickevent;
        public event MouseDotMoveEvent mousedotmoveevent;
        public event MouseMoveEvent mousemoveevent;

        public string title;

        public List<CircleShape> dots = new List<CircleShape>();
        public RenderWindow window;

        public App(string _title) {
            title = _title;
        }

        public void run() {
            if (prestartevent != null)
                if (prestartevent())
                    return;
            window = new RenderWindow(new VideoMode(502, 502), title, Styles.Close);
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
            if (mousedotclickevent != null) {
                for (int i = 0; i < dots.Count; i++) {
                    if (e.X < dots[i].Position.X + 42 && e.Y < dots[i].Position.Y + 42) {
                        mousedotclickevent(i, e.Button);
                        break;
                    }
                }
            }
            if (mouseclickevent != null)
                mouseclickevent(new Vector2f(e.X, e.Y), e.Button);
        }

        private void MouseScrolled(object sender, MouseWheelScrollEventArgs e) {
            if (mousewheelevent == null)
                return;

            mousewheelevent(e.Wheel, e.Delta, new Vector2i(e.X, e.Y));
        }

        private void MouseMoved(object sender, MouseMoveEventArgs e) {
            if (mousedotmoveevent != null) {
                for (int i = 0; i < dots.Count; i++) {
                    if (e.X < dots[i].Position.X + 42 && e.Y < dots[i].Position.Y + 42) {
                        mousedotmoveevent(i);
                        Console.WriteLine(dots[i].ToString());
                        break;
                    }
                }
            }
            if (mousemoveevent != null) {

            }
        }
    }
}
