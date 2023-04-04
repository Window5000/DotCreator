using SFML.Window;

namespace Dot {
    public class Input {
        public bool GetKey(Keyboard.Key key) {
            return Keyboard.IsKeyPressed(key);
        }
        public bool GetKey(Mouse.Button key) {
            return Mouse.IsButtonPressed(key);
        }
        public bool GetButton(Keyboard.Key key) {
            return Keyboard.IsKeyPressed(key);
        }
        public bool GetButton(Mouse.Button key) {
            return Mouse.IsButtonPressed(key);
        }
    }
}