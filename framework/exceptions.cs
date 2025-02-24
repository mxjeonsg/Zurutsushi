using System.Runtime.Serialization;
using Raylib_cs;

namespace Zurutsushi.Framework.Exceptions {
  [Serializable] public class AssetNotFoundException: Exception {
    public AssetNotFoundException() {}

    public AssetNotFoundException(string from_class, string asset_type, string asset_path, string emoticon)
    : base($"{from_class}: The {asset_type} \"{asset_path}\" doesn't seem to exist. {emoticon}")
    {}

    public AssetNotFoundException(string from_class, string asset_type, string asset_path, string emoticon, Exception inner_exc)
    : base($"{from_class}: The {asset_type} \"{asset_path}\" doesn't seem to exist. {emoticon}", inner_exc)
    {}
  }

  [Serializable] public class AudioStreamNotValidException: Exception {
    public AudioStreamNotValidException() {}

    public AudioStreamNotValidException(RSfx sound)
    : base($"Oops... The sFX object from \"{sound.sfxPath}\" seems to not be feeling good... :c")
    {}

    public AudioStreamNotValidException(RSfx sound, Exception inner_exc)
    : base($"Oops... The sFX object from \"{sound.sfxPath}\" seems to not be feeling good... :c", inner_exc)
    {}
  }

  [Serializable] public class MusicStreamNotValidException: Exception {
    public MusicStreamNotValidException() {}

    public MusicStreamNotValidException(RMusic music)
    : base($"Oops... The Music object from \"{music.musicPath}\" seems to not be working... :|")
    {}

    public MusicStreamNotValidException(RMusic music, Exception inner_exc)
    : base($"Oops... The Music object from \"{music.musicPath}\" seems to not be working... :|", inner_exc)
    {}
  }

  [Serializable] public class SfxStreamControlNotValidException: Exception {
  }

  [Serializable] public class AudioStreamNotReady: Exception {
    public AudioStreamNotReady() {}

    public AudioStreamNotReady(RSfx sfx)
    : base($"Sfx stream object from \"{sfx.sfxPath}\" isn't ready for playing or interacting. Maybe the object is malformed.")
    {}

    public AudioStreamNotReady(RSfx sfx, Exception inner_exc)
    : base($"Sfx stream object from \"{sfx.sfxPath}\" isn't ready for playing or interacting. Maybe the object is malformed.", inner_exc)
    {}
  }

  [Serializable] public class MusicStreamNotReady: Exception {
    public MusicStreamNotReady() {}

    public MusicStreamNotReady(RMusic music)
    : base($"Sfx stream object from \"{music.musicPath}\" isn't ready for playing or interacting. Maybe the object is malformed.")
    {}

    public MusicStreamNotReady(RMusic music, Exception inner_exc)
    : base($"Sfx stream object from \"{music.musicPath}\" isn't ready for playing or interacting. Maybe the object is malformed.", inner_exc)
    {}
  }
}