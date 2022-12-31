using Dot;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

App app = new App("test");
Clock clock = new Clock();

void test(string msg) {
    Console.WriteLine(msg);
}

bool first = true;
app.runevent += () => {
    if(clock.ElapsedTime.AsSeconds() >= 2 && first) {
        app.title = "IT WOORKS!!";
        app.SetPixel(3, Color.Blue);
        first = false;
        Util.Invoke(1.5f, test, "works");
    }
};
app.Run();