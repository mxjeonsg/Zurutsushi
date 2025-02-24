using System.Net;
using Raylib_cs;

namespace Zurutsushi.Framework {
  public enum BackgroundType {
    Image, Colour
  }

  public enum InstructionType {
    StartScene,
    ClearBkg, PrintTexture, PrintText, SleepWait,
    EndScene
  }

  public struct Scene {
    public ushort id;
    public string name;
    public BackgroundType bkg;
    public List<InstructionType> choreography;
    public uint duration;
  }

  public class SceneManager {
    private List<Scene> scenes = new List<Scene>();
  }
}