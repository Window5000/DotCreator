using Dot;
using SFML.System;
using SFML.Window;
using SFML.Graphics;

App app = new App("test", new Vector2u(520, 520), true);

app.backgroundColor = Color.White;

void test(string msg) {
    Console.WriteLine(msg);
}

bool first = true;
app.runevent += () => {
    if(first) {
        first = false;
        Util.Invoke(2, () => {
            app.title = "IT WOORKS!!";
            app.SetPixel(3, Color.Blue);
            Util.Invoke(1.5f, test, "works");
        });
    }

    if (Input.GetJustKey(Keyboard.Key.Space)) {
        Console.WriteLine("Just Space!");
    }
    if (Input.GetJustButton(Mouse.Button.Left)) {
        Console.WriteLine("Just Click!");
    }
};
app.Run();