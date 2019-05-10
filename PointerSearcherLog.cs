using System.Globalization;
using System.Text.RegularExpressions;

namespace TempAR
{
  internal class PointerSearcherLog
  {
    private uint _address;
    private uint _offset;
    private uint _value;
    private bool _negative;

    public PointerSearcherLog(
      uint address,
      uint offset,
      uint value,
      bool negative,
      uint memory_start)
    {
      this._address = address;
      this._offset = offset;
      this._value = value;
      this._negative = negative;
      if (this._address >= memory_start)
        return;
      this._address += memory_start;
    }

    public PointerSearcherLog(string s, uint memory_start)
    {
      Match match = new Regex("Address:\\s*0x(.*);\\s*Offset:\\s*(-?)0x(.*);\\s*Value:\\s*0x(.*);").Match(s);
      this._address = uint.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier);
      this._negative = match.Groups[2].Value == "-";
      this._offset = uint.Parse(match.Groups[3].Value, NumberStyles.AllowHexSpecifier);
      this._value = uint.Parse(match.Groups[4].Value, NumberStyles.AllowHexSpecifier);
      if (this._address >= memory_start)
        return;
      this._address += memory_start;
    }

    public override string ToString()
    {
      return string.Format("Address: 0x{0:X08}; Offset: {1}0x{2:X}; Value: 0x{3:X08};", (object) this._address, this._negative ? (object) "-" : (object) "", (object) this._offset, (object) this._value);
    }

    public string ToString(uint address_base)
    {
      return string.Format("Address: 0x{0:X08}; Offset: {1}0x{2:X}; Value: 0x{3:X08};", (object) (uint) ((int) this._address - (int) address_base), this._negative ? (object) "-" : (object) "", (object) this._offset, (object) (uint) ((int) this._value - (int) address_base));
    }

    public uint Address
    {
      get
      {
        return this._address;
      }
    }

    public uint Offset
    {
      get
      {
        return this._offset;
      }
    }

    public uint Value
    {
      get
      {
        return this._value;
      }
    }

    public bool Negative
    {
      get
      {
        return this._negative;
      }
    }
  }
}
