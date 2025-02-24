using System.Numerics;
using System.Reflection.Metadata;
using Raylib_cs;
using Zurutsushi.Framework.Exceptions;
using Colour = Raylib_cs.Color;

/// <summary>
/// This abstraction layer is basically needed because
/// Raylib bindings doesn't include proper destructors for
/// C#'s garbage collection.
/// So I have to add an abstraction layer over
/// the bindings in order to not "leak memory" because
/// C#'s GC doesn't really seem to give a rat's ass about
/// "external" memory allocations, so-called "umanaged memory".
/// Refer to: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
/// </summary>
namespace Zurutsushi.Framework {
  /// <summary>
  /// Abstraction layer as class for the Raylib's <c>Font</c> struct.
  /// 
  /// Added <c>IDisposable</c> implementation and some helper methods
  /// that call to Raylib functions to align a bit more with OOP.
  /// (Or to not leak memory in a dang managed language)
  /// </summary>
  public class RFont : IDisposable {
    /// <summary>
    /// Raylib's <c>Font</c> struct.
    /// </summary>
    private Font inner_font;

    /// <summary>
    /// The path of the font to load.
    /// </summary>
    private string asset_path;

    /// <summary>
    /// Tracking for the spacing. Not needed to be public specifically.
    /// Used for <c>RFont.textDraw(...)</c>.
    /// </summary>
    private float current_spacing = RFont.DefaultSpacing;

    /// <summary>
    /// Tracking for the font size. Not needed to be public specifically.
    /// Used for <c>RFont.textDraw(...)</c>.
    /// </summary>
    private float current_fontsz = RFont.DefaultFontSize;

    /// <summary>
    /// Tracking for the font colour. Not needed to be public specifically.
    /// Used for <c>RFont.textDraw(...)</c>.
    /// </summary>
    private Colour current_colour = Colour.Black;

    // Helpers

    /// <summary>
    /// The default Raylib's font.
    /// </summary>
    public static readonly Font RaylibDefaultFont = Raylib.GetFontDefault();

    /// <summary>
    /// The default colour used in <c>RFont.textDraw(...)</c>.
    /// </summary>
    public static readonly Colour DefaultColour = Colour.Black;

    /// <summary>
    /// The default font spacing used in <c>RFont.textDraw(...)</c>.
    /// </summary>
    public static readonly float DefaultSpacing = 1f;

    /// <summary>
    /// The default font size used in <c>RFont.textDraw(...)</c>.
    /// </summary>
    public static readonly float DefaultFontSize = 18;


    public int baseSize {
      get { return this.inner_font.BaseSize; }
    }

    public int glyphCount {
      get { return this.inner_font.GlyphCount; }
    }

    public int glyphPadding {
      get { return this.inner_font.GlyphPadding; }
    }

    public Texture2D innerTexture {
      get { return this.inner_font.Texture; }
    }

    public unsafe Rectangle[] rectangles {
      get {
        //return RFont.RecsPtrAsArray(this.inner_font.Recs); 
        return null;
      }
    }

    /// <summary>
    /// Getter for the font's path.
    /// </summary>
    public string fontPath {
      get { return this.asset_path; }
    }

    /// <summary>
    /// Getter/setter for the font spacing used on <c>RFont.textDraw(...)</c>.
    /// </summary>
    public float spacing {
      get { return this.current_spacing; }
      set { this.spacing = value; }
    }

    /// <summary>
    /// Getter/setter for the font size used on <c>RFont.textDraw(...)</c>.
    /// </summary>
    public float fontSz {
      get { return this.current_fontsz; }
      set { this.current_fontsz = value; }
    }

    /// <summary>
    /// Getter/setter for the font colour used on <c>RFont.textDraw(...)</c>.
    /// </summary>
    public Colour colour {
      get { return this.current_colour; }
      set { this.current_colour = value; }
    }

    [Obsolete("This fuction is unsafe and not tested adequately.", true)]
    public unsafe static Rectangle[] RecsPtrAsArray(Rectangle* recptr) {
      Rectangle[] final_arr = [];

      try {
        for(uint i = 0; /* something should be here */; ++i) {
          final_arr.Append(recptr[i]);
        }
      } catch(IndexOutOfRangeException _) {}

      return final_arr;
    }

    /// <summary>
    /// Destructor. Releases the memory allocated by the call to <c>Raylib.LoadFont(string)</c>.
    /// 
    /// Needed for the C#'s <c>using(RFont _ = new RFont(string)) {...}</c>
    /// usage.
    /// </summary>
    public void Dispose()
    => Raylib.UnloadFont(this.inner_font);
    

    /// <summary>
    /// Constructor. Calls `Raylib.LoadFont(string)` to load the font into after
    /// checking if the referred <c>font_path</c> exists in the first place.
    /// </summary>
    /// <param name="font_path">The font's path relative from the executable's path.</param>
    /// <exception cref="AssetNotFoundException">The file couldn't be found. Maybe check the path?</exception>
    public RFont(string font_path) {
      if(File.Exists(font_path)) {
        this.inner_font = Raylib.LoadFont(font_path);
        this.asset_path = font_path;
      } else {
        throw new AssetNotFoundException("RFont", "font", font_path, "._.");
      }
    }

    /// <summary>
    /// This calls to <c>Raylib.DrawTextEx(...)</c> to draw the provided string in screen.
    /// </summary>
    /// <param name="text">The text to draw</param>
    /// <param name="position">Where to draw</param>
    /// <param name="colour">The colour to use, or <c>null</c> to use the default/last one.</param>
    /// <param name="fontsz">The font size to use, or <c>null</c> to use the default/last one.</param>
    /// <param name="spacing">The font spacing to use, or <c>null</c> to use the default/last one.</param>
    public void textDraw(string text, Vector2 position, Colour? colour, float? fontsz, float? spacing) {
      Colour col = colour != null ? (Colour) colour! : new Colour(0, 0, 0, 0xff);
      float fsz = fontsz != null ? (float) fontsz! : this.current_fontsz;
      float spa = spacing != null ? (float) spacing! : this.current_spacing;

      Raylib.DrawTextEx(this.inner_font, text, position, fsz, spa, col);
    }

    /// <summary>
    /// Destructor. This gets called by the GC.
    /// </summary>
    ~RFont()
    => this.Dispose();
  }

  /// <summary>
  /// The abstraction layer over the Raylib's <c>Sound</c> struct.
  /// 
  /// It implements the helper functions from the <c>ZIAudioElement</c>
  /// interface, plus the <c>IDisposable</c> whatever ones. lmao
  /// </summary>
  public class RSfx : ZIAudioElement {
    /// <summary>
    /// The og <c>Sound</c> struct.
    /// </summary>
    private Sound inner_sound;

    /// <summary>
    /// The path of the SFX.
    /// </summary>
    private string asset_path;

    /// <summary>
    /// Playing state tracking.
    /// </summary>
    private bool is_playing = false;

    /// <summary>
    /// Pitch tracking. Not needed to be public.
    /// </summary>
    private float current_pitch = RSfx.DefaultPitch;

    /// <summary>
    /// Volume tracking. Not needed to be public.
    /// </summary>
    private float current_volume = RSfx.DefaultVolume;

    /// <summary>
    /// Panning tracking. Not needed to be public.
    /// </summary>
    private float current_pan = RSfx.DefaultPan;

    // Helpers

    /// <summary>
    /// The maximum volume interval.
    /// </summary>
    public static readonly float MaxVolume = 1f;

    /// <summary>
    /// The minimum volume interval before going mute.
    /// </summary>
    public static readonly float MinVolume = 0.00009f;

    /// <summary>
    /// The default volume interval.
    /// </summary>
    public static readonly float DefaultVolume = 0.5f;

    /// <summary>
    /// The default pitch interval.
    /// </summary>
    public static readonly float DefaultPitch = 1f;

    /// <summary>
    /// The default pan interval. Centre.
    /// </summary>
    public static readonly float DefaultPan = 0.5f;

    /// <summary>
    /// Getter for polling Raylib if the sfx stream is being played.
    /// </summary>
    /// TODO: Maybe a setter for pausing? Meeeh. (24.2.5)
    public bool isPlaying {
      get {
        this.is_playing = Raylib.IsSoundPlaying(this.inner_sound);
        return this.is_playing;
      }
    }

    /// <summary>
    /// Getter of the sfx's path.
    /// </summary>
    public string sfxPath {
      get { return this.asset_path; }
    }

    /// <summary>
    /// Getter of the pitch.
    /// </summary>
    public float pitch {
      get { return this.current_pitch; }
    }

    /// <summary>
    /// Getter of the volume.
    /// </summary>
    public float volume {
      get { return this.current_volume; }
    }

    /// <summary>
    /// Getter of the panning.
    /// </summary>
    public float pan {
      get { return this.current_pan; }
    }

    /// <summary>
    /// Getter that checks if the sfx's stream is valid to raylib.
    /// </summary>
    public bool valid {
      get { return Raylib.IsSoundValid(this.inner_sound); }
    }

    /// <summary>
    /// This member checks if the audio stream is still valid
    /// to Raylib.
    /// </summary>
    /// <exception cref="AudioStreamNotValidException">Raylib dissapproved the stream.</exception>
    private void verify_audio_obj() {
      if(!Raylib.IsSoundValid(this.inner_sound))
        throw new AudioStreamNotValidException(this);
    }

    /// <summary>
    /// Constructor for the Sfx.
    /// </summary>
    /// <param name="audio_path">The path to the sfx.</param>
    /// <exception cref="AssetNotFoundException">Are you sure you wrote the sfx's path well?</exception>
    public RSfx(string audio_path) {
      if(File.Exists(audio_path)) {
        this.inner_sound = Raylib.LoadSound(audio_path);
        this.asset_path = audio_path;
      } else {
        throw new AssetNotFoundException("RSfx", "sfx file", audio_path, ":^");
      }
    }

    /// <summary>
    /// This is experimental. This constructor makes a <c>Sound</c> out of
    /// a memory stream.
    /// </summary>
    /// <param name="audio_buffer">The memory stream buffer.</param>
    public RSfx(MemoryStream audio_buffer) {
      Wave wave = Raylib.LoadWaveFromMemory("wav", audio_buffer.ToArray());
      this.inner_sound = Raylib.LoadSoundFromWave(wave);
      Raylib.UnloadWave(wave);

      this.asset_path = "<MemoryStream object>";
    }

    /// <summary>
    /// C#'s <c>using(RSfx _ = new RSfx(?)) {...}</c> whatever.
    /// </summary>
    public void Dispose()
    => Raylib.UnloadSound(this.inner_sound);

    /// <summary>
    /// This starts the streaming from the sfx file and
    /// updating it's status pulling samples off if needed.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void play() {
      this.verify_audio_obj();

      Raylib.PlaySound(this.inner_sound);
      this.is_playing = true;
    }

    /// <summary>
    /// This stops the streaming from the sfx file
    /// and seeking to the start of the file.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void stop() {
      this.verify_audio_obj();

      Raylib.StopSound(this.inner_sound);
      this.is_playing = false;
    }

    /// <summary>
    /// This puts to pause the sfx file and polling
    /// data off if needed.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void pause() {
      this.verify_audio_obj();
      Raylib.PauseSound(this.inner_sound);
      this.is_playing = false;
    }

    /// <summary>
    /// This continues to play the stream where it was
    /// paused.
    /// 
    /// When called, checks if a call to <c>RSfx.play()</c> was made
    /// before, performing the call if no call was made, otherwise
    /// just continuing like normal.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void resume() {
      this.verify_audio_obj();
      Raylib.ResumeSound(this.inner_sound);
      this.is_playing = false;
    }

    /// <summary>
    /// This seeks the stream cursor to a given position.
    /// 
    /// This call isn't compatible with RSfx, just with RMusic.
    /// So the call throws here.
    /// </summary>
    /// <param name="position">Position where to seek.</param>
    /// <exception cref="InvalidOperationException">The operation isn't allowed by RSfx.</exception>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void seek(float position) {
      this.verify_audio_obj();
      throw new InvalidOperationException("RSfx: SFX objects don't support seeking operations. XD");
    }

    /// <summary>
    /// This sets the stream's pitch to a certain parametre.
    /// </summary>
    /// <param name="new_pitch">Pitch to be set to.</param>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void controlPitch(float new_pitch) {
      this.verify_audio_obj();

      Raylib.SetSoundPitch(this.inner_sound, new_pitch);
      this.current_pitch = new_pitch;
    }

    /// <summary>
    /// This sets the stream's panning to a certain parametre.
    /// </summary>
    /// <param name="new_pan">Panning to be set to. (Range from 0f to 1f)</param>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void controlPan(float new_pan) {
      this.verify_audio_obj();

      Raylib.SetSoundPan(this.inner_sound, new_pan);
      this.current_pan = new_pan;
    }

    /// <summary>
    /// This sets the stream's volume to a certain level.
    /// </summary>
    /// <param name="new_volume">Volume level to be set.</param>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void controlVolume(float new_volume) {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, new_volume);
      this.current_volume = new_volume;
    }

    /// <summary>
    /// This attempts to set the pitch to it's default value.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void setDefaultPitch() {
      this.verify_audio_obj();

      Raylib.SetSoundPitch(this.inner_sound, RSfx.DefaultPitch);
      this.current_pitch = RSfx.DefaultPitch;
    }

    /// <summary>
    /// This attempts to set the panning to it's default value. (Centrered)
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void setDefaultPan() {
      this.verify_audio_obj();

      Raylib.SetSoundPan(this.inner_sound, RSfx.DefaultPan);
      this.current_pan = RSfx.DefaultPan;
    }

    /// <summary>
    /// This attempts to set the volume to it's default value. (0.50f)?
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void setDefaultVolume() {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, RSfx.DefaultVolume);
      this.current_volume = RSfx.DefaultVolume;
    }

    /// <summary>
    /// This just cranks up the volume of the stream.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void setVolumeMax() {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, RSfx.MaxVolume);
      this.current_volume = RSfx.MaxVolume;
    }

    /// <summary>
    /// This sets the volume down to it's minimum without going mute.
    /// </summary>
    [Obsolete("Sound-related calls don't work. Something's off with the audio backend. Should investigate further.", false)]
    public void setVolumeMin() {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, RSfx.MinVolume);
      this.current_volume = RSfx.MinVolume;
    }

    /// <summary>
    /// This just releases the asset and the memory.
    /// It's called by C#'s garbage collector blablablá.
    /// </summary>
    ~RSfx()
    => this.Dispose();
  }

  /// <summary>
  /// Abstraction layer for the Raylib's <c>Music</c> struct.
  /// Implements the helper functions from the <c>ZIAudioElement</c> interface
  /// and the typical <c>IDisposable</c> slop you'd be already used to read
  /// here.
  /// </summary>
  public class RMusic : ZIAudioElement {
    /// <summary>
    /// The path to the music track.
    /// </summary>
    private string asset_path;

    /// <summary>
    /// The og Raylib's <c>Music</c> struct.
    /// </summary>
    private Music inner_music;

    /// <summary>
    /// Tracking if the stream is playing.
    /// </summary>
    private bool is_playing = false;

    /// <summary>
    /// Panning tracking.
    /// </summary>
    private float current_pan = RSfx.DefaultPan;

    /// <summary>
    /// Pitch tracking.
    /// </summary>
    private float current_pitch = RSfx.DefaultPitch;

    /// <summary>
    /// Volume tracking.
    /// </summary>
    private float current_volume = RSfx.DefaultVolume;

    /// <summary>
    /// Playing tracking.
    /// </summary>
    private bool already_started = false;

    /// <summary>
    /// Getter of the track's path.
    /// </summary>
    public string musicPath {
      get { return this.asset_path; }
    }

    /// <summary>
    /// Getter to track if the track is being playrf.
    /// </summary>
    public bool isPlaying {
      get{
        this.is_playing = Raylib.IsMusicStreamPlaying(this.inner_music);

        return this.is_playing;
      }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="music_path">The path to the track relative to the executable's path.</param>
    /// <exception cref="AssetNotFoundException">You didn't mess up the track's path did you?</exception>
    public RMusic(string music_path) {
      if(File.Exists(music_path)) {
        this.inner_music = Raylib.LoadMusicStream(music_path);
        this.asset_path = music_path;
      } else {
        throw new AssetNotFoundException("RMusic", "music", music_path, "(l.l)");
      }
    }

    /// <summary>
    /// Verify if the music struct is still okay to Raylib's eyes.
    /// </summary>
    /// <exception cref="MusicStreamNotValidException">Nop, isn't okay to Raylib's eyes.</exception>
    private void verify_music_obj() {
      if(!Raylib.IsMusicValid(this.inner_music))
        throw new MusicStreamNotValidException(this);
    }

    /// <summary>
    /// Destroyer for the <c>using(RMusic _ = new RMusic(string)) {...}</c> thing.
    /// </summary>
    public void Dispose()
    => Raylib.UnloadMusicStream(this.inner_music);

    /// <summary>
    /// Puts the track to play.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void play() {
      this.verify_music_obj();
      

      Raylib.PlayMusicStream(this.inner_music);
      this.is_playing = true;
      this.already_started = true;

      Raylib.UpdateMusicStream(this.inner_music);
    }

    /// <summary>
    /// Stops the track of playing.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void stop() {
      this.verify_music_obj();
      


      Raylib.StopMusicStream(this.inner_music);
      this.is_playing = false;
      this.already_started = false;
    }

    /// <summary>
    /// Pauses the track's playing.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void pause() {
      this.verify_music_obj();
      

      Raylib.PauseMusicStream(this.inner_music);
      this.is_playing = false;
    }

    /// <summary>
    /// Resulmes the track's playing.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void resume() {
      if(!this.already_started) this.play();

      this.verify_music_obj();
      

      Raylib.ResumeMusicStream(this.inner_music);
      this.is_playing = true;
      Raylib.UpdateMusicStream(this.inner_music);

    }

    /// <summary>
    /// Seeks the track's position to a certain parametre.
    /// </summary>
    /// <param name="position">Where the cursor lands on?</param>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void seek(float position) {
      this.verify_music_obj();
      

      Raylib.SeekMusicStream(this.inner_music, position);
    }

    /// <summary>
    /// This sets the track's pitch to a certain parametre.
    /// </summary>
    /// <param name="new_pitch">What'll be the next pitch?</param>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void controlPitch(float new_pitch){
      this.verify_music_obj();
      

      Raylib.SetMusicPitch(this.inner_music, new_pitch);
      this.current_pitch = new_pitch;
    }

    /// <summary>
    /// Attempts to set the pitch back to it's default setting.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void setDefaultPitch() {
      this.verify_music_obj();

      
      Raylib.SetMusicPitch(this.inner_music, RSfx.DefaultPitch);
      this.current_pitch = RSfx.DefaultPitch;
    }

    /// <summary>
    /// Sets the panning to a certain parametre.
    /// </summary>
    /// <param name="new_pan">New panning setting</param>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void controlPan(float new_pan) {
      this.verify_music_obj();
      

      Raylib.SetMusicPan(this.inner_music, new_pan);
      this.current_pan = new_pan;
    }

    /// <summary>
    /// Tries to set the panning back to it's default setting.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void setDefaultPan() {
      this.verify_music_obj();
      

      Raylib.SetMusicPan(this.inner_music, RSfx.DefaultPan);
      this.current_pan = RSfx.DefaultPan;
    }

    /// <summary>
    /// This sets the track's volume to a certain parametre.
    /// </summary>
    /// <param name="new_volume">New volume level</param>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void controlVolume(float new_volume) {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, new_volume);
      this.current_volume = new_volume;
    }

    /// <summary>
    /// This attempts to set the track's volume back to it's default setting.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void setDefaultVolume() {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, RSfx.DefaultVolume);
      this.current_volume = RSfx.DefaultVolume;
    }

    /// <summary>
    /// This cranks up the track's volume.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void setVolumeMax() {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, RSfx.MaxVolume);
      this.current_volume = RSfx.MaxVolume;
    }

    /// <summary>
    /// This downs the track's volume enough to be minimum but not mute.
    /// </summary>
    [Obsolete("Sound-related calls shouldn't be done. Something's off with sound in Raylib. Should investigate further.", false)]
    public void setVolumeMin() {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, RSfx.MinVolume);
      this.current_volume = RSfx.MinVolume;
    }

    /// <summary>
    /// The destructor. I've commented too much destructors enough
    /// and I'm sleepy. This get's called by the GC. That's it.
    /// </summary>
    ~RMusic()
    => this.Dispose();
  }

  /// <summary>
  /// Abstraction layer of the Raylib's <c>Texture2D</c> struct.
  /// </summary>
  /// TODO: complete the implementation. (24.2.25)
  public class RTexture : IDisposable {
    public void Dispose() {
      throw new NotImplementedException();
    }
  }
}