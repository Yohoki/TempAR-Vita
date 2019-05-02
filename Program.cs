// Decompiled with JetBrains decompiler
// Type: TempAR.Program
// Assembly: TempAR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C3F8A34-0260-4D18-8C4B-520ED4C1EC88
// Assembly location: D:\ROMs & ISOs\Vita Schtuff\pointer_searcher\TempAR.exe

using System;
using System.Windows.Forms;

namespace TempAR
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new frmMain());
    }
  }
}
