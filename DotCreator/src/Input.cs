using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Dot {
    public class Input {
        Window window;

        public Input(Window window) {
            this.window = window;
        }

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