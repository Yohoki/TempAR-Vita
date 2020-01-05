using System.Globalization;
using System.Text.RegularExpressions;

namespace TempAR
{
    internal class PointerSearcherLog
    {
        public uint Address { get; }
        public uint Offset { get; }
        public uint Value { get; }
        public bool Negative { get; }

        public PointerSearcherLog(
          uint address,
          uint offset,
          uint value,
          bool negative,
          uint memory_start)
        {
            Address = address;
            Offset = offset;
            Value = value;
            Negative = negative;
            if (Address >= memory_start)
                return;
            Address += memory_start;
        }

        public PointerSearcherLog(string s, uint memory_start)
        {
            var match = new Regex("地址:\\s*0x(.*);\\s*偏移:\\s*(-?)0x(.*);\\s*数值:\\s*0x(.*);").Match(s);
            Address = uint.Parse(match.Groups[1].Value, NumberStyles.AllowHexSpecifier);
            Negative = match.Groups[2].Value == "-";
            Offset = uint.Parse(match.Groups[3].Value, NumberStyles.AllowHexSpecifier);
            Value = uint.Parse(match.Groups[4].Value, NumberStyles.AllowHexSpecifier);
            if (Address >= memory_start)
                return;
            Address += memory_start;
        }

        public override string ToString()
        {
            return $"地址: 0x{Address:X08}; 偏移: {(Negative ? "-" : "")}0x{Offset:X}; 数值: 0x{Value:X08};";
        }

        public string ToString(uint address_base)
        {
            return $"地址: 0x{(uint)((int)Address - (int)address_base):X08}; 偏移: {(Negative ? "-" : "")}0x{Offset:X}; 数值: 0x{(uint)((int)Value - (int)address_base):X08};";
        }
    }
}