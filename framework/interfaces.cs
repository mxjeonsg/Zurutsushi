using Raylib_cs;

namespace Zurutsushi.Framework {
  public interface ZIAudioElement: IDisposable {
    // Stream control
    void play();
    void stop();
    void pause();
    void resume();
    void seek(float position);

    // Tone control
    void controlPitch(float new_pitch);
    void setDefaultPitch();

    void controlPan(float new_pan);
    void setDefaultPan();

    void controlVolume(float new_volume);
    void setDefaultVolume();
    void setVolumeMax();
    void setVolumeMin();
  }

  public interface ZISceneCallbacks {
    void init();
    void update();
    void draw();
    void destroy();
  }
}