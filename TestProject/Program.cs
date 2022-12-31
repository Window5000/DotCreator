using Dot;
using SFML.System;
using SFML.Graphics;
using Dot.Event;

App app = new App("test");
Clock clock = new Clock();
bool first = true;
app.runevent += () => {
    if(clock.ElapsedTime.AsSeconds() >= 2 && first) {
        app.title = "IT WOORKS!!";
        app.SetPixel(3, Color.Blue);
        first = false;
    }
};
app.Run();