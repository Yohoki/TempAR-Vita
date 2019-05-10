using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TempAR
{
  internal class PointerSearcher
  {
    private string _memdump_path;
    private uint[] _memdump;
    private uint _memory_start;
    private uint _memory_end;

    public PointerSearcher(string memdump_path, uint memory_start)
    {
      this._memdump_path = memdump_path;
      this._memory_start = memory_start;
    }

    public List<PointerSearcherLog> findPointers(
      uint address,
      uint max_offset)
    {
      if (this._memdump == null)
        this.loadMemoryDump();
      if (this._memdump == null)
        return (List<PointerSearcherLog>) null;
      List<PointerSearcherLog> pointerSearcherLogList = new List<PointerSearcherLog>();
      if (address < this._memory_start)
        address += this._memory_start;
      if (address > this._memory_end)
      {
        int num = (int) MessageBox.Show("Address value is too large, please input a smaller value for the address.");
        return pointerSearcherLogList;
      }
      for (int index = 0; index < this._memdump.Length; ++index)
      {
        if (this._memdump[index] >= this._memory_start && this._memdump[index] <= this._memory_end)
        {
          if (this._memdump[index] <= address && this._memdump[index] >= address - max_offset)
            pointerSearcherLogList.Add(new PointerSearcherLog((uint) (index * 4) + this._memory_start, (uint) (4294967296UL - (ulong) (this._memdump[index] - address)), this._memdump[index], false, this._memory_start));
          else if (this._memdump[index] >= address && this._memdump[index] <= address + max_offset)
            pointerSearcherLogList.Add(new PointerSearcherLog((uint) (index * 4) + this._memory_start, this._memdump[index] - address, this._memdump[index], true, this._memory_start));
        }
      }
      return pointerSearcherLogList;
    }

    public uint getPointerAddress(uint address, uint offset, bool negative)
    {
      if (this._memdump == null)
        this.loadMemoryDump();
      if (this._memdump == null)
        return 0;
      address -= this._memory_start;
      address /= 4U;
            if ((long)address > (long)this._memdump.Length || this._memdump[address] < this._memory_start || this._memdump[address] > this._memory_end)
        return 0;
      if (!negative)
        return this._memdump[address] + offset;
      return this._memdump[address] - offset;
    }

    public uint[] MemoryDump
    {
      get
      {
        return this._memdump;
      }
    }

    private void loadMemoryDump()
    {
      if (!File.Exists(this._memdump_path))
        return;
      byte[] numArray = File.ReadAllBytes(this._memdump_path);
      if (numArray.Length % 4 == 0)
      {
        this._memdump = new uint[numArray.Length / 4];
        Buffer.BlockCopy((Array) numArray, 0, (Array) this._memdump, 0, numArray.Length);
      }
      this._memory_end = this._memory_start + (uint) numArray.Length;
    }
  }
}
