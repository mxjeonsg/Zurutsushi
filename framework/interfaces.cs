using Raylib_cs;

namespace Zurutsushi.Framework {
  /// <summary>
  /// This interface works as methods that
  /// classes related to SFXs should always
  /// implement.
  /// 
  /// Mostly planned as an abstraction layer
  /// of Raylib's structures + it's related
  /// functions.
  /// 
  /// The interface inherits from `IDisposable`
  /// for the reason explained in the 'raylib.cs'
  /// file. Refer to the next link in case you don't
  /// want to read the reason from me:
  /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose">Microsoft documentation</see>
  /// </summary>
  public interface ZIAudioElement: IDisposable {
    // Stream control

    /// <summary>
    /// This base method ideally should check if the underlying struct
    /// (<c>RSfx.inner_sound</c> or <c>RMusic.inner_music</c>) is valid through a
    /// Raylib call (<c>Raylib.IsSoundValid(Sound)</c> or <c>Raylib.IsMusicStreamValid(Music)</c>)
    /// before trying to play the sound stream (<c>Raylib.PlayAudioStream(Sound)</c> or <c>Raylib.PlayMusicStream(Music)</c>)
    /// </summary>
    void play();

    /// <summary>
    /// Equally to <c>ZIAudioElement.play()</c>,
    /// checks for validation are ideal before proceeding to stop the stream.
    /// </summary>
    void stop();

    /// <summary>
    /// Equally to the other members of the <c>ZIAudioElement</c> interface,
    /// checks for validation are ideal before proceeding to pause the stream.
    /// </summary>
    void pause();

    /// <summary>
    /// Equally to the other members of the <c>ZIAudioElement</c> interface,
    /// checks for validation are ideal before proceeding to resume the stream.
    /// </summary>
    void resume();

    /// <summary>
    /// Equally to the other members of the <c>ZIAudioElement</c> interface,
    /// checks for validation are ideal before proceeding to seek the stream.
    /// </summary>
    void seek(float position);

    // Tone control

    /// <summary>
    /// This member controls the pitch of the SFX.
    /// </summary>
    /// <param name="new_pitch">The new pitch ammount</param>
    /// <exception cref="AudioStreamNotValidException">The underlying call to Raylib failed to check the integrity of the stream. Maybe a malformation or a bug.</exception>
    /// <exception cref="AudioStreamNotReady">The underlying call to Raylib failed to confirm the stream is playable. Maybe a malformation or a bug.</exception>
    void controlPitch(float new_pitch);

    /// <summary>
    /// This member tries to set the pitch in it's default value.
    /// The default value is up to the inheritee class.
    /// Check <c>RSfx</c> or <c>RMusic</c> classes for the default
    /// values, it'd be a <c>public static readonly float</c>. (Or <c>public static readonly uint</c>?)
    /// </summary>
    void setDefaultPitch();

    /// <summary>
    /// This member sets the panning of the stream by the parametre provided.
    /// </summary>
    /// <param name="new_pan">The new panning interval</param>
    void controlPan(float new_pan);

    /// <summary>
    /// This member tried to set the panning in it's default value.
    /// As in <c>ZIAudioElement.SetDefaultPitch()</c>, the so-called
    /// "default" value is up to the inheritee.
    /// </summary>
    void setDefaultPan();

    /// <summary>
    /// This member sets the volume of the stream by the parametre provided.
    /// It's range mostly is <c>0f</c> to <c>1f</c>.
    /// </summary>
    /// <param name="new_volume">The new volume setup from <c>0f</c> to <c>1f</c>. ("Normalised value"?)</param>
    void controlVolume(float new_volume);

    /// <summary>
    /// This member attempts to set the stream's volume to it's
    /// default value.
    /// 
    /// The default value is up to the inheritee yadayada.
    /// </summary>
    void setDefaultVolume();

    /// <summary>
    /// This member sets the stream's volume to it's maximum value.
    /// </summary>
    void setVolumeMax();

    /// <summary>
    /// This member sets the stream's volume to it's minimum value
    /// without muting. (less or equal than <c>0.01f</c>.)
    /// </summary>
    void setVolumeMin();
  }

  /// <summary>
  /// Scenes have four stages:
  /// <list type="bullet">
  ///   <item>
  ///     <term>init()</term>
  ///     <description>Preload its assets</description>
  ///   </item>
  ///   <item>
  ///     <term>update()</term>
  ///     <description>Generic logic needed for the scene dynamism and event polling.</description>
  ///   </item>
  ///   <item>
  ///     <term>draw()</term>
  ///     <description>Logic for the scene's drawing on the screen.</description>
  ///   </item>
  ///   <item>
  ///     <term>destroy()</term>   
  ///     <description>After the scene is triggered to be changed, unload the assets.</description>
  ///   </item>
  /// </list>
  /// </summary>
  public interface ZISceneCallbacks {
    /// <summary>
    /// This initialises the scene, charges up
    /// all the needed assets, and then gets ready to
    /// start updating.
    /// </summary>
    void init();

    /// <summary>
    /// This is the basic engine of the scenes, processes all the
    /// logic to prepare things to be drawn.
    /// </summary>
    void update();

    /// <summary>
    /// This processes the pertinent calls for drawing the scene to the screen.
    /// </summary>
    void draw();

    /// <summary>
    /// Everything's done, the job's done.
    /// Pack things up, destroy the objects representing
    /// the assets and go.
    /// </summary>
    void destroy();
  }
}