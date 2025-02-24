using System.Numerics;
using System.Reflection.Metadata;
using Raylib_cs;
using Zurutsushi.Framework.Exceptions;
using Colour = Raylib_cs.Color;

// This abstraction layer is basically needed because
// Raylib bindings doesn't include proper destructors for
// C#'s garbage collection.
// So I have to add an abstraction layer over
// the bindings in order to not "leak memory" because
// C#'s GC doesn't really seem to give a rat's ass about
// "external" memory allocations, so-called "umanaged memory".
// Refer to: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
namespace Zurutsushi.Framework {
  public class RFont : IDisposable {
    private Font inner_font;
    private string asset_path;
    private float current_spacing = RFont.DefaultSpacing;
    private float current_fontsz = RFont.DefaultFontSize;
    private Colour current_colour = Colour.Black;

    public static readonly Font RaylibDefaultFont = Raylib.GetFontDefault();
    public static readonly Colour DefaultColour = Colour.Black;
    public static readonly float DefaultSpacing = 1f;
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
      get { return RFont.RecsPtrAsArray(this.inner_font.Recs); }
    }

    public string fontPath {
      get { return this.asset_path; }
    }

    public float spacing {
      get { return this.current_spacing; }
      set { this.spacing = value; }
    }

    public float fontSz {
      get { return this.current_fontsz; }
      set { this.current_fontsz = value; }
    }

    public Colour colour {
      get { return this.current_colour; }
      set { this.current_colour = value; }
    }

    public unsafe static Rectangle[] RecsPtrAsArray(Rectangle* recptr) {
      Rectangle[] final_arr = [];

      try {
        for(uint i = 0; /* something should be here */; ++i) {
          final_arr.Append(recptr[i]);
        }
      } catch(IndexOutOfRangeException _) {}

      return final_arr;
    }

    public void Dispose()
    => Raylib.UnloadFont(this.inner_font);
    
    public RFont(string font_path) {
      if(File.Exists(font_path)) {
        this.inner_font = Raylib.LoadFont(font_path);
        this.asset_path = font_path;
      } else {
        throw new AssetNotFoundException("RFont", "font", font_path, "._.");
      }
    }

    public void textDraw(string text, Vector2 position, Colour? colour, float? fontsz, float? spacing) {
      Colour col = colour != null ? (Colour) colour! : new Colour(0, 0, 0, 0xff);
      float fsz = fontsz != null ? (float) fontsz! : this.current_fontsz;
      float spa = spacing != null ? (float) spacing! : this.current_spacing;

      Raylib.DrawTextEx(this.inner_font, text, position, fsz, spa, col);
    }

    ~RFont()
    => this.Dispose();
  }

  public class RSfx : ZIAudioElement {
    private Sound inner_sound;
    private string asset_path;
    private bool is_playing = false;
    private float current_pitch = RSfx.DefaultPitch;
    private float current_volume = RSfx.DefaultVolume;
    private float current_pan = RSfx.DefaultPan;

    public static readonly float MaxVolume = 1f;
    public static readonly float MinVolume = 0.00009f;
    public static readonly float DefaultVolume = 0.5f;
    public static readonly float DefaultPitch = 1f;
    public static readonly float DefaultPan = 0.5f; // Centre

    public bool isPlaying {
      get {
        this.is_playing = Raylib.IsSoundPlaying(this.inner_sound);
        return this.is_playing;
      }
    }

    public string sfxPath {
      get { return this.asset_path; }
    }

    public float pitch {
      get { return this.current_pitch; }
    }

    public float volume {
      get { return this.current_volume; }
    }

    public float pan {
      get { return this.current_pan; }
    }

    public bool valid {
      get { return Raylib.IsSoundValid(this.inner_sound); }
    }

    private void verify_audio_obj() {
      if(!Raylib.IsSoundValid(this.inner_sound))
        throw new AudioStreamNotValidException(this);
    }

    public RSfx(string audio_path) {
      if(File.Exists(audio_path)) {
        this.inner_sound = Raylib.LoadSound(audio_path);
        this.asset_path = audio_path;
      } else {
        throw new AssetNotFoundException("RSfx", "sfx file", audio_path, ":^");
      }
    }

    public RSfx(MemoryStream audio_buffer) {
      Wave wave = Raylib.LoadWaveFromMemory("wav", audio_buffer.ToArray());
      this.inner_sound = Raylib.LoadSoundFromWave(wave);
      Raylib.UnloadWave(wave);

      this.asset_path = "<MemoryStream object>";
    }

    public void Dispose()
    => Raylib.UnloadSound(this.inner_sound);

    public void play() {
      this.verify_audio_obj();

      Raylib.PlaySound(this.inner_sound);
      this.is_playing = true;
    }

    public void stop() {
      this.verify_audio_obj();

      Raylib.StopSound(this.inner_sound);
      this.is_playing = false;
    }

    public void pause() {
      this.verify_audio_obj();
      Raylib.PauseSound(this.inner_sound);
      this.is_playing = false;
    }

    public void resume() {
      this.verify_audio_obj();
      Raylib.ResumeSound(this.inner_sound);
      this.is_playing = false;
    }

    public void seek(float position) {
      this.verify_audio_obj();
      throw new InvalidOperationException("RSfx: SFX objects don't support seeking operations. XD");
    }

    public void controlPitch(float new_pitch) {
      this.verify_audio_obj();

      Raylib.SetSoundPitch(this.inner_sound, new_pitch);
      this.current_pitch = new_pitch;
    }

    public void controlPan(float new_pan) {
      this.verify_audio_obj();

      Raylib.SetSoundPan(this.inner_sound, new_pan);
      this.current_pan = new_pan;
    }

    public void controlVolume(float new_volume) {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, new_volume);
      this.current_volume = new_volume;
    }

    public void setDefaultPitch() {
      this.verify_audio_obj();

      Raylib.SetSoundPitch(this.inner_sound, RSfx.DefaultPitch);
      this.current_pitch = RSfx.DefaultPitch;
    }

    public void setDefaultPan() {
      this.verify_audio_obj();

      Raylib.SetSoundPan(this.inner_sound, RSfx.DefaultPan);
      this.current_pan = RSfx.DefaultPan;
    }

    public void setDefaultVolume() {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, RSfx.DefaultVolume);
      this.current_volume = RSfx.DefaultVolume;
    }

    public void setVolumeMax() {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, RSfx.MaxVolume);
      this.current_volume = RSfx.MaxVolume;
    }

    public void setVolumeMin() {
      this.verify_audio_obj();

      Raylib.SetSoundVolume(this.inner_sound, RSfx.MinVolume);
      this.current_volume = RSfx.MinVolume;
    }

    ~RSfx()
    => this.Dispose();
  }

  public class RMusic : ZIAudioElement {
    private string asset_path;
    private Music inner_music;
    private bool is_playing = false;

    private float current_pan = RSfx.DefaultPan;
    private float current_pitch = RSfx.DefaultPitch;
    private float current_volume = RSfx.DefaultVolume;
    private bool already_started = false;

    public string musicPath {
      get { return this.asset_path; }
    }

    public bool isPlaying {
      get{
        this.is_playing = Raylib.IsMusicStreamPlaying(this.inner_music);

        return this.is_playing;
      }
    }

    public RMusic(string music_path) {
      if(File.Exists(music_path)) {
        this.inner_music = Raylib.LoadMusicStream(music_path);
        this.asset_path = music_path;
      } else {
        throw new AssetNotFoundException("RMusic", "music", music_path, "(l.l)");
      }
    }

    private void verify_music_obj() {
      if(!Raylib.IsMusicValid(this.inner_music))
        throw new MusicStreamNotValidException(this);
    }

    public void Dispose()
    => Raylib.UnloadMusicStream(this.inner_music);

    public void play() {
      this.verify_music_obj();
      

      Raylib.PlayMusicStream(this.inner_music);
      this.is_playing = true;
      this.already_started = true;

      Raylib.UpdateMusicStream(this.inner_music);
    }

    public void stop() {
      this.verify_music_obj();
      


      Raylib.StopMusicStream(this.inner_music);
      this.is_playing = false;
      this.already_started = false;
    }

    public void pause() {
      this.verify_music_obj();
      

      Raylib.PauseMusicStream(this.inner_music);
      this.is_playing = false;
    }

    public void resume() {
      if(!this.already_started) this.play();

      this.verify_music_obj();
      

      Raylib.ResumeMusicStream(this.inner_music);
      this.is_playing = true;
      Raylib.UpdateMusicStream(this.inner_music);

    }

    public void seek(float position) {
      this.verify_music_obj();
      

      Raylib.SeekMusicStream(this.inner_music, position);
    }

    public void controlPitch(float new_pitch){
      this.verify_music_obj();
      

      Raylib.SetMusicPitch(this.inner_music, new_pitch);
      this.current_pitch = new_pitch;
    }

    public void setDefaultPitch() {
      this.verify_music_obj();

      
      Raylib.SetMusicPitch(this.inner_music, RSfx.DefaultPitch);
      this.current_pitch = RSfx.DefaultPitch;
    }

    public void controlPan(float new_pan) {
      this.verify_music_obj();
      

      Raylib.SetMusicPan(this.inner_music, new_pan);
      this.current_pan = new_pan;
    }

    public void setDefaultPan() {
      this.verify_music_obj();
      

      Raylib.SetMusicPan(this.inner_music, RSfx.DefaultPan);
      this.current_pan = RSfx.DefaultPan;
    }

    public void controlVolume(float new_volume) {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, new_volume);
      this.current_volume = new_volume;
    }

    public void setDefaultVolume() {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, RSfx.DefaultVolume);
      this.current_volume = RSfx.DefaultVolume;
    }

    public void setVolumeMax() {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, RSfx.MaxVolume);
      this.current_volume = RSfx.MaxVolume;
    }

    public void setVolumeMin() {
      this.verify_music_obj();
      

      Raylib.SetMusicVolume(this.inner_music, RSfx.MinVolume);
      this.current_volume = RSfx.MinVolume;
    }

    ~RMusic()
    => this.Dispose();
  }

  public class RTexture : IDisposable {
    public void Dispose() {
      throw new NotImplementedException();
    }
  }
}