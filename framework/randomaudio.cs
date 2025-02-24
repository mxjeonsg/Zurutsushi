using System.Diagnostics.Metrics;
using System.Media;
using Newtonsoft.Json.Serialization;

namespace Zurutsushi.Framework {
  public class RandomDing {
    // private SoundPlayer player;
    private BinaryWriter writer;
    public byte[] data_bytearr = [];
    public MemoryStream memoryStream = new MemoryStream();
    public BinaryWriter binaryWriter;

    public RandomDing(double freq, uint tenthsec) {
      this.binaryWriter = new BinaryWriter(this.memoryStream);

      string header_group = "RIFF";
      uint header_filesz = 0;
      string header_Riff = "WAVE";

      string fmt_chunkid = "fmt ";
      uint fmt_chunksz = 16;
      ushort fmt_formattag = 1;
      ushort fmt_channel = 1;
      uint fmt_sampleps = 14_000;
      ushort fmt_bitsps = 8;
      ushort fmt_blkalign = (ushort)(fmt_channel * (fmt_bitsps / 8));
      uint fmt_avgbytesps = (fmt_sampleps * fmt_blkalign);

      string data_chunkid = "data";
      uint data_chunksz;

      uint numsample = (fmt_sampleps * fmt_channel) * tenthsec;
      this.data_bytearr = new byte[numsample];
      int amplitude = 127, offset = 128;
      double period = (2.0 * Math.PI) * freq;
      double amp;

      for(uint i = 0; i < numsample - 1; i += fmt_channel) {
        amp = amplitude * (double)(numsample - 1) / numsample;

        for(int channel = 0; channel < fmt_channel; channel++) {
          this.data_bytearr[i+channel] = Convert.ToByte(amp * Math.Sin(i * period) + offset);
        }
      }

      data_chunksz = (uint)(this.data_bytearr.Length * (fmt_bitsps / 8));
      header_filesz = 4 + (8 + fmt_chunksz) + (8 + data_chunksz);

      this.binaryWriter.Write(header_group.ToCharArray());
      this.binaryWriter.Write(header_filesz);
      this.binaryWriter.Write(header_Riff.ToCharArray());

      this.binaryWriter.Write(fmt_chunkid.ToCharArray());
      this.binaryWriter.Write(fmt_chunksz);
      this.binaryWriter.Write(fmt_formattag);
      this.binaryWriter.Write(fmt_channel);
      this.binaryWriter.Write(fmt_sampleps);
      this.binaryWriter.Write(fmt_avgbytesps);
      this.binaryWriter.Write(fmt_blkalign);
      this.binaryWriter.Write(fmt_bitsps);

      this.binaryWriter.Write(data_chunkid.ToCharArray());
      this.binaryWriter.Write(data_chunksz);
      foreach(byte datap in this.data_bytearr)
        this.binaryWriter.Write(datap);

      // player = new SoundPlayer(audiostream);
    }

    public void Dispose() {
      if(this.writer != null) this.writer.Close();
      // if(this.player != null) this.player.Dispose();

      this.writer = null;
      // this.player = null;
    }

    // public void Play() {
    //   if(this.player != null) {
    //     this.player.Stream.Seek(0, SeekOrigin.Begin);
    //     this.player.Play();
    //   }
    // }

    public void WriteTo(string path) {
    }
  }

  public class RandomAudio {

  }
}