using Dot.Event;
using SFML.Window;
using System.Collections;

namespace Dot {
    public static class Input {
        private static List<int> justButtons = new List<int>();
        private static List<int> justKeys = new List<int>();

        public static void update() {
            justKeys.Clear();
            justButtons.Clear();
        }

        public static void KeyPressed(object sender, KeyEventArgs e) {
            justKeys.Add((int) e.Code);
        }
        public static void MouseButtonPressed(object sender, MouseButtonEventArgs e) {
            justButtons.Add((int) e.Button);
        }

        public static bool GetKey(Keyboard.Key key) {
            return Keyboard.IsKeyPressed(key);
        }

        public static bool GetButton(Mouse.Button key) {
            return Mouse.IsButtonPressed(key);
        }

        public static bool GetJustKey(Keyboard.Key key) {
            return justKeys.Contains((int) key);
        }

        public static bool GetJustButton(Mouse.Button key) {
            return justButtons.Contains((int) key);
        }
    }
}