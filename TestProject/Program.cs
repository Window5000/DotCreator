using Dot;
using SFML.System;

App app = new App("test");
Clock clock = new Clock();
bool first = true;
app.runevent += () => {
    if(clock.ElapsedTime.AsSeconds() >= 1 && first) {
        app.title = "IT WOORKS!!";
        first = false;
    }
};
app.run();