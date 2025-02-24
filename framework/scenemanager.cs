namespace Zurutsushi.Framework {
  /// <summary>
  ///  This enumeration enlists all the possible Scenes
  ///  in the game.
  /// </summary>
  public enum Scenes {
    None, MainMenu, TheEnd, Credits, Debug
  }

  /// <summary>
  /// This class serves as representant of a
  /// possible Scene, implements `ZISceneCallbacks`
  /// functions so the scene excecution flow stays
  /// the same across all Scenes.
  /// Too OOP-ish if you ask me, but blame
  /// the game not the players.
  /// 
  /// Shouldn't've said it when I'm actually
  /// making a game, but it is what it is and
  /// it isn't what it isn't.
  /// </summary>
  public class SceneDescriptor : ZISceneCallbacks {
    bool shouldExit = false;
    Scenes nextScene = Scenes.None;

    public void destroy() {
      throw new NotImplementedException();
    }

    public void draw() {
      throw new NotImplementedException();
    }

    public void init() {
      throw new NotImplementedException();
    }

    public void update() {
      throw new NotImplementedException();
    }
  }
}