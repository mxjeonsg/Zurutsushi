using System.Runtime.CompilerServices;
using System.Text;

namespace Zurutsushi.Framework {
  public unsafe class ZString : IDisposable {
    private byte[] chars;
    private uint utf8size;

    public ZString(string str) {
      this.chars = Encoding.UTF8.GetBytes(str);
      this.utf8size = (uint)(str.Length * 4) + 1;
    }

    public ZString(char[] carr) {}

    public ZString(byte[] barr) {}

    public unsafe ZString(sbyte* sbptr) {}

    public void Dispose() {
      throw new NotImplementedException();
    }

    public string asUtf8Str() {
      return "";
    }
  }
}