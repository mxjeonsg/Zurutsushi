using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using Raylib_cs;
using Zurutsushi.Framework;
using static Raylib_cs.Raylib;
using Colour = Raylib_cs.Color;

namespace Zurutsushi {
  internal class MainGame {
    [MarshalAs(UnmanagedType.LPUTF8Str)] static string uwu = "はははは";
    public static void Main(string[] argv) {
      bool[] key_states = [
        false, false
      ];

      Colour PinkishColour = new Colour(238, 211, 238, 0xff);
      // TODO: Maybe check if I can pull out the resources
      // without doing "../../.." each time to pull from
      // the project root from the executable's path.
      RFont akaya = new RFont("../../../assets/fonts/Akaya_Kanadaka/AkayaKanadaka-Regular.ttf");
      RFont kiwi = new RFont("../../../assets/fonts/Kiwi_Maru/KiwiMaru-Regular.ttf");
      RFont borel = new RFont("../../../assets/fonts/Borel/Borel-Regular.ttf");
      

      InitAudioDevice();
      InitWindow(1200, 720, "ズルツシ");
      SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint);
      SetTargetFPS(60);
      
      // FIXME: for some reason music doesn't work.
      // FIXME: Resolve unicode shi

      while(!WindowShouldClose()) {
        // Track keybinds
        if(IsKeyPressed(KeyboardKey.F11)) {
          key_states[1] = !key_states[1];
          ToggleFullscreen();
        } else if(IsKeyPressed(KeyboardKey.F1)) {
          key_states[0] = !key_states[0];
        }

        ClearBackground(PinkishColour);
        BeginDrawing();

        if(key_states[0]) {
          Console.WriteLine("Stat display enabled");
          string stat_0 = $"Cursor position: (X: {GetMouseX()}, Y: {GetMouseY()})";

          DrawText(stat_0, 1, 1, 15, Colour.Black);
          new RMusic("").play();
        }
        
        akaya.textDraw(MainGame.uwu, new(20, 20), null, null, null);
        kiwi.textDraw("Normal ascii text", new(20, 40), null, null, null);
        borel.textDraw("Normal ascii text", new(20, 60), null, null, null);
        EndDrawing();
      }

      CloseAudioDevice();
      CloseWindow();
    }
  }
}