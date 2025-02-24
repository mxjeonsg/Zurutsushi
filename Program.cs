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
      RFont akaya = new RFont("../../../assets/fonts/Akaya_Kanadaka/AkayaKanadaka-Regular.ttf");
      RFont kiwi = new RFont("../../../assets/fonts/Kiwi_Maru/KiwiMaru-Regular.ttf");
      RFont borel = new RFont("../../../assets/fonts/Borel/Borel-Regular.ttf");
      

      InitAudioDevice();
      InitWindow(1200, 720, "ズルツシ");
      SetConfigFlags(ConfigFlags.Msaa4xHint | ConfigFlags.VSyncHint);
      SetTargetFPS(60);
      
      // TODO: for some reason music doesn't work.
      //RMusic temp_music = new RMusic("../../../assets/mainmenu_1.wav");
      //RMusic temp_music2 = new RMusic("../../../assets/i_need_you.mp3");
      //Music iny = LoadMusicStream("../../../assets/i_need_you.mp3");
      //temp_music2.setVolumeMax();
      //temp_music2.play();

      while(!WindowShouldClose()) {
        // Track keybinds
        if(IsKeyPressed(KeyboardKey.F11)) {
          key_states[1] = !key_states[1];
          ToggleFullscreen();
        } else if(IsKeyPressed(KeyboardKey.F1)) {
          key_states[0] = !key_states[0];
        } else if(IsKeyPressed(KeyboardKey.F3)) {
          //if(temp_music2.isPlaying) {
          //  temp_music2.pause();
          //} else {
          //  temp_music2.resume();
          //}

          //if(IsMusicStreamPlaying(iny)) {
          //  PauseMusicStream(iny);
          //} else {
          //  ResumeMusicStream(iny);
          //}
          //UpdateMusicStream(iny);
        }

        ClearBackground(PinkishColour);
        BeginDrawing();
        // DrawText("Sexito", 20, 20, 20, Colour.Black);

        if(key_states[0]) {
          Console.WriteLine("Stat display enabled");
          string stat_0 = $"Cursor position: (X: {GetMouseX()}, Y: {GetMouseY()})";

          DrawText(stat_0, 1, 1, 15, Colour.Black);
        }

        // DrawText("ははははは", 20, 20, 20, Colour.Black);
        
        akaya.textDraw(MainGame.uwu, new(20, 20), null, null, null);
        kiwi.textDraw("Normal ascii text", new(20, 40), null, null, null);
        borel.textDraw("Normal ascii text", new(20, 60), null, null, null);
        EndDrawing();
      }

      //UnloadMusicStream(iny);
      CloseAudioDevice();
      CloseWindow();
    }
  }
}