// Decompiled with JetBrains decompiler
// Type: TempAR.Converter
// Assembly: TempAR, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C3F8A34-0260-4D18-8C4B-520ED4C1EC88
// Assembly location: D:\ROMs & ISOs\Vita Schtuff\pointer_searcher\TempAR.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TempAR
{
  internal class Converter
  {
    private const int PSPAR = 1;
    private const int CWCHEAT = 2;
    private const int CWCHEAT_POPS = 3;
    private const int NITEPR = 4;
    private const int R4CCE = 5;

    public static void file_cwcpops_pspar(string infile, string outfile)
    {
      string[] strArray = new Regex("_S ", RegexOptions.IgnoreCase).Split(File.ReadAllText(infile));
      StreamWriter streamWriter = new StreamWriter(outfile);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (!string.IsNullOrEmpty(strArray[index]))
          streamWriter.Write(Converter.cwcpops_pspar("_S " + strArray[index]).TrimEnd() + "\r\n");
      }
      streamWriter.Close();
    }

    public static void file_nitepr_pspar(string indir, string outfile)
    {
      FileInfo[] files = new DirectoryInfo(indir).GetFiles("????-?????.txt");
      StringBuilder stringBuilder = new StringBuilder();
      foreach (FileInfo fileInfo in files)
      {
        using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
        {
          stringBuilder.Append(string.Format("_S {0}\r\n", (object) fileInfo.Name.Substring(0, 10)));
          stringBuilder.Append(Converter.nitepr_pspar(streamReader.ReadToEnd()));
          streamReader.Close();
        }
      }
      StreamWriter streamWriter = new StreamWriter(outfile);
      streamWriter.Write(stringBuilder.ToString());
      streamWriter.Close();
    }

    public static void file_reformat_tempar(string infile, string outfile)
    {
      string input = File.ReadAllText(infile);
      StreamWriter streamWriter = new StreamWriter(outfile, false);
      streamWriter.Write(Converter.reformat_tempar(input));
      streamWriter.Close();
    }

    public static string cwcpops_pspar(string input)
    {
      string str1 = (string) null;
      string[] strArray = Converter.reformat_cwcpops(input).Split('\n');
      string str2 = (string) null;
      uint num1 = 0;
      uint num2 = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (Converter.is_codeline(strArray[index], 1))
        {
          if (num2 == 0U)
          {
            try
            {
              uint num3 = uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(4, 6), NumberStyles.AllowHexSpecifier) + 159383552U;
              uint num4 = uint.Parse(strArray[index].Split(' ')[2].Trim().Substring(2, 8), NumberStyles.AllowHexSpecifier);
              switch (uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(2, 2), NumberStyles.AllowHexSpecifier))
              {
                case 16:
                  str1 += string.Format("_M 0xDA000000 0x{0:X08}\r\n_M 0xD4000000 0x{1:X08}\r\n_M 0xD7000000 0x{0:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) num3, (object) num4);
                  num1 = 0U;
                  break;
                case 17:
                  str1 += string.Format("_M 0xDA000000 0x{0:X08}\r\n_M 0xD4000000 0x{1:X08}\r\n_M 0xD7000000 0x{0:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) num3, (object) (4294967296L - (long) num4));
                  num1 = 0U;
                  break;
                case 32:
                  str1 += string.Format("_M 0xDB000000 0x{0:X08}\r\n_M 0xD4000000 0x{1:X08}\r\n_M 0xD8000000 0x{0:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) num3, (object) num4);
                  num1 = 0U;
                  break;
                case 33:
                  str1 += string.Format("_M 0xDB000000 0x{0:X08}\r\n_M 0xD4000000 0x{1:X08}\r\n_M 0xD8000000 0x{0:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) num3, (object) (4294967296L - (long) num4));
                  num1 = 0U;
                  break;
                case 48:
                  str1 += string.Format("_M 0x2{0:X07} 0x{1:X08}\r\n", (object) num3, (object) num4);
                  break;
                case 80:
                  try
                  {
                    uint num5 = uint.Parse(strArray[index + 1].Split(' ')[1].Trim().Substring(4, 6), NumberStyles.AllowHexSpecifier) + 159383552U;
                    uint num6 = uint.Parse(strArray[index + 1].Split(' ')[2].Trim().Substring(2, 8), NumberStyles.AllowHexSpecifier);
                    int num7 = (int) uint.Parse(strArray[index + 1].Split(' ')[1].Trim().Substring(2, 2), NumberStyles.AllowHexSpecifier);
                    if (num4 == 0U)
                      str1 += string.Format("_M 0xC0000000 0x{0:X08}\r\n_M 0x{1:X01}{2:X07} 0x{3:X08}\r\n_M 0xDC000000 0x{4:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) (uint) ((int) ((num3 & 65280U) >> 8) - 1), (object) (((int) num3 & (int) byte.MaxValue) == 1 ? 2 : 1), (object) num5, (object) num6, (object) (uint) ((int) num3 & (int) byte.MaxValue));
                    else
                      str1 += string.Format("_M 0xD5000000 0x{0:X08}\r\n_M 0xC0000000 0x{1:X08}\r\n_M 0x{2:X02}000000 0x{3:X08}\r\n_M 0xD4000000 0x{4:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) num6, (object) (uint) ((int) ((num3 & 65280U) >> 8) - 1), (object) (((int) num3 & (int) byte.MaxValue) == 1 ? 216 : 218), (object) num5, (object) num4);
                    num1 = 0U;
                    num2 = 2U;
                    break;
                  }
                  catch (Exception ex)
                  {
                    break;
                  }
                case 112:
                case 208:
                  str1 += string.Format("_M 0x9{0:X07} 0x0000{1:X04}\r\n", (object) num3, (object) num4);
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                case 128:
                  str1 += string.Format("_M 0x1{0:X07} 0x{1:X08}\r\n", (object) num3, (object) num4);
                  break;
                case 144:
                case 209:
                  str1 += string.Format("_M 0xA{0:X07} 0x0000{1:X04}\r\n", (object) num3, (object) num4);
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                case 194:
                  try
                  {
                    uint num5 = uint.Parse(strArray[index + 1].Split(' ')[1].Trim().Substring(4, 6), NumberStyles.AllowHexSpecifier) + 159383552U;
                    str1 += string.Format("_M 0xD3000000 0x{0:X08}\r\n_M 0xF{1:X07} 0x{2:X08}\r\n_M 0xD2000000 0x00000000\r\n", (object) num3, (object) num5, (object) num4);
                    num1 = 0U;
                    num2 = 2U;
                    break;
                  }
                  catch (Exception ex)
                  {
                    break;
                  }
                case 210:
                  str1 += string.Format("_M 0x7{0:X07} 0x0000{1:X04}\r\n", (object) num3, (object) num4);
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                case 211:
                  str1 += string.Format("_M 0x8{0:X07} 0x0000{1:X04}\r\n", (object) num3, (object) num4);
                  num1 = 2U;
                  break;
                case 224:
                  str1 = num3 % 2U != 1U ? str1 + string.Format("_M 0x9{0:X07} 0x00FF{1:X02}00\r\n", (object) num3, (object) (uint) ((int) num4 & (int) byte.MaxValue)) : str1 + string.Format("_M 0x9{0:X07} 0xFF0000{1:X02}\r\n", (object) (uint) ((int) num3 - 1), (object) (uint) ((int) num4 & (int) byte.MaxValue));
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                case 225:
                  str1 = num3 % 2U != 1U ? str1 + string.Format("_M 0xA{0:X07} 0x00FF{1:X02}00\r\n", (object) num3, (object) (uint) ((int) num4 & (int) byte.MaxValue)) : str1 + string.Format("_M 0xA{0:X07} 0xFF0000{1:X02}\r\n", (object) (uint) ((int) num3 - 1), (object) (uint) ((int) num4 & (int) byte.MaxValue));
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                case 226:
                  str1 = num3 % 2U != 1U ? str1 + string.Format("_M 0x7{0:X07} 0x00FF{1:X02}00\r\n", (object) num3, (object) (uint) ((int) num4 & (int) byte.MaxValue)) : str1 + string.Format("_M 0x7{0:X07} 0xFF0000{1:X02}\r\n", (object) (uint) ((int) num3 - 1), (object) (uint) ((int) num4 & (int) byte.MaxValue));
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                case 227:
                  str1 = num3 % 2U != 1U ? str1 + string.Format("_M 0x8{0:X07} 0x00FF{1:X02}00\r\n", (object) num3, (object) (uint) ((int) num4 & (int) byte.MaxValue)) : str1 + string.Format("_M 0x8{0:X07} 0xFF0000{1:X02}\r\n", (object) (uint) ((int) num3 - 1), (object) (uint) ((int) num4 & (int) byte.MaxValue));
                  str2 = strArray[index];
                  num1 = 2U;
                  break;
                default:
                  str1 = str1 + "#" + strArray[index] + " [unsupported]\r\n";
                  break;
              }
            }
            catch (Exception ex)
            {
            }
          }
        }
        else
          str1 = str1 + strArray[index] + "\r\n";
        if (num2 > 0U)
          --num2;
        if (num1 > 0U)
        {
          --num1;
          if (num1 == 0U)
          {
            if (strArray.Length > index + 1)
            {
              if (str2 == strArray[index + 1])
              {
                ++index;
                ++num1;
              }
              else
              {
                str1 += "_M 0xD2000000 0x00000000\r\n";
                str2 = (string) null;
              }
            }
            else
            {
              str1 += "_M 0xD2000000 0x00000000\r\n";
              str2 = (string) null;
            }
          }
        }
      }
      return str1;
    }

    public static string nitepr_pspar(string input)
    {
      string str = "";
      string[] strArray = Converter.reformat_nitepr(input).Split('\n');
      uint num1 = uint.MaxValue;
      for (int index = 0; index < strArray.Length; ++index)
      {
        try
        {
          if (Converter.is_codeline(strArray[index], 1))
          {
            uint num2 = uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(3), NumberStyles.AllowHexSpecifier);
            uint num3 = uint.Parse(strArray[index].Split(' ')[2].Trim().Substring(2), NumberStyles.AllowHexSpecifier);
            uint num4 = uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(2, 1), NumberStyles.AllowHexSpecifier);
            if (num1 == uint.MaxValue && num2 < 142606336U)
              num2 += 142606336U;
            if (num4 == 15U & num2 == 268435455U)
            {
              if ((int) num1 != (int) num3)
              {
                if (num1 != uint.MaxValue)
                  str += "_M 0xD2000000 0x00000000\r\n";
                num1 = num3;
                str += string.Format("_M 0x6{0:X07} 0x00000000\r\n_M 0xB{0:X07} 0x00000000\r\n", (object) (uint) ((int) num1 + 142606336));
              }
            }
            else
            {
              switch (strArray[index].Split(' ')[2].Trim().Length)
              {
                case 4:
                  str += string.Format("_M 0x2{0:X07} 0x{1:X08}\r\n", (object) num2, (object) num3);
                  continue;
                case 6:
                  str += string.Format("_M 0x1{0:X07} 0x{1:X08}\r\n", (object) num2, (object) num3);
                  continue;
                default:
                  str += string.Format("_M 0x0{0:X07} 0x{1:X08}\r\n", (object) num2, (object) num3);
                  continue;
              }
            }
          }
          else
          {
            if (num1 != uint.MaxValue)
            {
              str += "_M 0xD2000000 0x00000000\r\n";
              num1 = uint.MaxValue;
            }
            str = str + strArray[index] + "\r\n";
          }
        }
        catch (Exception ex)
        {
          str = str + ";Exception: " + ex.Message + "\r\n";
        }
      }
      if (num1 != uint.MaxValue)
        str += "_M 0xD2000000 0x00000000\r\n";
      return str;
    }

    public static string reformat_cwcpops(string input)
    {
      List<string> pattern = new List<string>();
      List<string> replacement = new List<string>();
      string subject = input;
      pattern.Add("[\r\n]+ *[\r\n]+");
      replacement.Add("\n");
      pattern.Add(" {2,}");
      replacement.Add(" ");
      pattern.Add("^(_L)*\\s*([0-9a-z]{8})\\s*([0-9a-z]{4})");
      replacement.Add("_M 0x$2 0x0000$3");
      pattern.Add("^_L (.*)");
      replacement.Add("#_L $1 [bad egg]");
      pattern.Add("_S\\s*(.{4}).(.{5})");
      replacement.Add("_S $1-$2");
      return Converter.preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }

    public static string reformat_nitepr(string input)
    {
      List<string> pattern = new List<string>();
      List<string> replacement = new List<string>();
      string subject = input;
      pattern.Add("[\r\n]+ *[\r\n]+");
      replacement.Add("\n");
      pattern.Add(" {2,}");
      replacement.Add(" ");
      pattern.Add("^0x([0-9a-z]{8})\\s*0x([0-9a-z]{1,})");
      replacement.Add("_M 0x$1 0x$2");
      pattern.Add("^#!!(.*)");
      replacement.Add("_C2 $1");
      pattern.Add("^#!(.*)");
      replacement.Add("_C1 $1");
      pattern.Add("^#(.*)");
      replacement.Add("_C0 $1");
      pattern.Add("^;(.*)");
      replacement.Add("#$1");
      return Converter.preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }

    public static string reformat_r4cce(string input, bool show_folders_comments)
    {
      List<string> pattern = new List<string>();
      List<string> replacement = new List<string>();
      string subject = input;
      pattern.Add("[\r\n]+");
      replacement.Add("\r\n");
      pattern.Add("^---+\\s*");
      replacement.Add("");
      pattern.Add("^!!!(.*)\\s*^!!(.*)");
      replacement.Add("_S $2\r\n_G $1");
      pattern.Add("^!!!(.*)");
      replacement.Add("_G $1");
      pattern.Add("^!!(.*)");
      replacement.Add("_S $1");
      if (show_folders_comments)
      {
        pattern.Add("^!(.*)");
        replacement.Add("_C1 $1");
      }
      else
      {
        pattern.Add("^!(.*)");
        replacement.Add("# <*$1*>");
      }
      if (show_folders_comments)
      {
        pattern.Add("^:::(.*)");
        replacement.Add("_C2 $1");
      }
      else
      {
        pattern.Add("^:::(.*)");
        replacement.Add("# <*$1*>");
      }
      pattern.Add("^::(.*)");
      replacement.Add("_C0 $1");
      if (show_folders_comments)
      {
        pattern.Add("^:(.*)");
        replacement.Add("_C1 $1");
      }
      else
      {
        pattern.Add("^:(.*)");
        replacement.Add("# <*$1*>");
      }
      pattern.Add("^([0-9a-z]{8}) ([0-9a-z]{8})");
      replacement.Add("_M 0x$1 0x$2");
      pattern.Add("(_C0.*)\\s*((_C2.*\\s*)*)\\s*((_M.*\\s*)*)");
      replacement.Add("$1\r\n$4$2");
      return Converter.preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }

    public static string reformat_tempar(string input)
    {
      List<string> pattern = new List<string>();
      List<string> replacement = new List<string>();
      string subject = input;
      pattern.Add("[\r\n]+");
      replacement.Add("\r\n");
      pattern.Add("^_S(.*)\\s*^_G(.*)");
      replacement.Add("_G$2\r\n_S$1");
      pattern.Add("^_G (.*)");
      replacement.Add("!!!$1");
      pattern.Add("^_S (.*)");
      replacement.Add("!!$1");
      pattern.Add("^_C0 (.*)");
      replacement.Add("::$1");
      pattern.Add("^_M 0x([0-9a-z]{8}) 0x([0-9a-z]{8})");
      replacement.Add("$1 $2");
      return Converter.preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
    }

    public static bool is_codeline(string input, int format)
    {
      switch (format)
      {
        case 1:
          if (input.StartsWith("_M ") && input.Length >= 14)
            return input[13] == ' ';
          return false;
        case 2:
        case 3:
          if (input.StartsWith("_L ") && input.Length >= 14)
            return input[13] == ' ';
          return false;
        case 4:
          if (input.StartsWith("0x") && input.Length >= 14)
            return input[10] == ' ';
          return false;
        case 5:
          if (input.Length == 17)
            return input[9] == ' ';
          return false;
        default:
          return false;
      }
    }

    private static string port_addresses(string subject, int difference)
    {
      MatchCollection matchCollection = new Regex("(_M|_L)*\\s*0x(.)(.{7})\\s*0x(.{8})").Matches(subject);
      string str = subject;
      foreach (Match match in matchCollection)
      {
        switch (int.Parse(match.Groups[2].Value, NumberStyles.AllowHexSpecifier))
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
          case 8:
          case 9:
          case 10:
          case 11:
          case 14:
          case 15:
            str = str.Replace(match.Groups[0].Value, match.Groups[1].Value + " 0x" + match.Groups[2].Value + (int.Parse(match.Groups[3].Value, NumberStyles.AllowHexSpecifier) + difference).ToString("X07") + " 0x" + match.Groups[4].Value);
            continue;
          case 13:
            if (int.Parse(match.Groups[3].Value, NumberStyles.AllowHexSpecifier) != 33554432)
            {
              str = str.Replace(match.Groups[0].Value, match.Groups[1].Value + " 0x" + match.Groups[2].Value + match.Groups[3].Value + " 0x" + (int.Parse(match.Groups[4].Value, NumberStyles.AllowHexSpecifier) + difference).ToString("X08"));
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      return str;
    }

    private static string preg_replace(
      List<string> pattern,
      List<string> replacement,
      string subject,
      RegexOptions options)
    {
      string input = subject;
      for (int index = 0; index < pattern.Count; ++index)
        input = new Regex(pattern[index], options).Replace(input, replacement[index]);
      return input;
    }
  }
}
