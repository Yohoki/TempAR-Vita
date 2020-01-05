using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TempAR
{
    internal class PointerSearcher
    {
        private string _memdump_path;
        private uint _memory_start;
        private uint _memory_end;
        public uint[] MemoryDump { get; private set; }

        public PointerSearcher(string memdump_path, uint memory_start)
        {
            _memdump_path = memdump_path;
            _memory_start = memory_start;
        }

        public List<PointerSearcherLog> FindPointers(
          uint address,
          uint max_offset)
        {
            if (MemoryDump == null)
                LoadMemoryDump();
            if (MemoryDump == null)
                return null;
            var pointerSearcherLogList = new List<PointerSearcherLog>();
            if (address < _memory_start)
                address += _memory_start;
            if (address > _memory_end)
            {
                MessageBox.Show("地址值错误，请检查后再次输入。");
                return pointerSearcherLogList;
            }
            for (int index = 0; index < MemoryDump.Length; ++index)
            {
                if (MemoryDump[index] >= _memory_start && MemoryDump[index] <= _memory_end)
                {
                    if (MemoryDump[index] <= address && MemoryDump[index] >= address - max_offset)
                    {
                        pointerSearcherLogList.Add(new PointerSearcherLog((uint)(index * 4) + _memory_start, (uint)(0x100000000UL - (MemoryDump[index] - address)), MemoryDump[index], false, _memory_start));
                    }
                    else if (MemoryDump[index] >= address && MemoryDump[index] <= address + max_offset)
                    {
                        pointerSearcherLogList.Add(new PointerSearcherLog((uint)(index * 4) + _memory_start, MemoryDump[index] - address, MemoryDump[index], true, _memory_start));
                    }
                }
            }
            return pointerSearcherLogList;
        }

        public uint GetPointerAddress(uint address, uint offset, bool negative)
        {
            if (MemoryDump == null) LoadMemoryDump();
            if (MemoryDump == null) return 0;
            address -= _memory_start;
            address /= 4U;
            if (address > MemoryDump.Length || MemoryDump[address] < _memory_start || MemoryDump[address] > _memory_end) return 0;
            if (!negative) return MemoryDump[address] + offset;
            return MemoryDump[address] - offset;
        }

        private void LoadMemoryDump()
        {
            if (!File.Exists(_memdump_path))
                return;
            var numArray = File.ReadAllBytes(_memdump_path);
            if (numArray.Length % 4 == 0)
            {
                MemoryDump = new uint[numArray.Length / 4];
                Buffer.BlockCopy((Array)numArray, 0, (Array)MemoryDump, 0, numArray.Length);
            }
            _memory_end = _memory_start + (uint)numArray.Length;
        }
    }
}