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

        public static void File_cwcpops_pspar(string infile, string outfile)
        {
            var strArray = new Regex("_S ", RegexOptions.IgnoreCase).Split(File.ReadAllText(infile));
            using (var streamWriter = new StreamWriter(outfile))
            {
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (!string.IsNullOrEmpty(strArray[index]))
                        streamWriter.Write(Converter.Cwcpops_pspar("_S " + strArray[index]).TrimEnd() + "\r\n");
                }
                streamWriter.Close();
            }
        }

        public static void File_nitepr_pspar(string indir, string outfile)
        {
            var files = new DirectoryInfo(indir).GetFiles("????-?????.txt");
            var stringBuilder = new StringBuilder();
            foreach (FileInfo fileInfo in files)
            {
                using (StreamReader streamReader = new StreamReader(fileInfo.FullName))
                {
                    stringBuilder.Append(string.Format("_S {0}\r\n", (object)fileInfo.Name.Substring(0, 10)));
                    stringBuilder.Append(Converter.Nitepr_pspar(streamReader.ReadToEnd()));
                    streamReader.Close();
                }
            }
            using (var streamWriter = new StreamWriter(outfile))
            {
                streamWriter.Write(stringBuilder.ToString());
                streamWriter.Close();
            }
        }

        public static void File_reformat_tempar(string infile, string outfile)
        {
            var input = File.ReadAllText(infile);
            using (var streamWriter = new StreamWriter(outfile, false))
            {
                streamWriter.Write(Converter.Reformat_tempar(input));
                streamWriter.Close();
            }
        }

        public static string Cwcpops_pspar(string input)
        {
            var strArray = Converter.Reformat_cwcpops(input).Split('\n');
            string str2 = null;
            uint num1 = 0;
            uint num2 = 0;
            var builder = new StringBuilder();

            for (int index = 0; index < strArray.Length; ++index)
            {
                if (Converter.Is_codeline(strArray[index], 1))
                {
                    if (num2 == 0U)
                    {
                        try
                        {
                            var num3 = uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(4, 6), NumberStyles.AllowHexSpecifier) + 0x9800000U;
                            var num4 = uint.Parse(strArray[index].Split(' ')[2].Trim().Substring(2, 8), NumberStyles.AllowHexSpecifier);
                            switch (uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(2, 2), NumberStyles.AllowHexSpecifier))
                            {
                                case 16:
                                    builder.Append($"_M 0xDA000000 0x{num3:X08}\r\n_M 0xD4000000 0x{num4:X08}\r\n_M 0xD7000000 0x{num3:X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                    num1 = 0U;
                                    break;

                                case 17:
                                    builder.Append($"_M 0xDA000000 0x{num3:X08}\r\n_M 0xD4000000 0x{(0x100000000L - num4):X08}\r\n_M 0xD7000000 0x{num3:X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                    num1 = 0U;
                                    break;

                                case 32:
                                    builder.Append($"_M 0xDB000000 0x{num3:X08}\r\n_M 0xD4000000 0x{num4:X08}\r\n_M 0xD8000000 0x{num3:X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                    num1 = 0U;
                                    break;

                                case 33:
                                    builder.Append($"_M 0xDB000000 0x{num3:X08}\r\n_M 0xD4000000 0x{(0x100000000L - num4):X08}\r\n_M 0xD8000000 0x{num3:X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                    num1 = 0U;
                                    break;

                                case 48:
                                    builder.Append($"_M 0x2{num3:X07} 0x{num4:X08}\r\n");
                                    break;

                                case 80:
                                    try
                                    {
                                        var num5 = uint.Parse(strArray[index + 1].Split(' ')[1].Trim().Substring(4, 6), NumberStyles.AllowHexSpecifier) + 0x9800000U;
                                        var num6 = uint.Parse(strArray[index + 1].Split(' ')[2].Trim().Substring(2, 8), NumberStyles.AllowHexSpecifier);
                                        var num7 = (int)uint.Parse(strArray[index + 1].Split(' ')[1].Trim().Substring(2, 2), NumberStyles.AllowHexSpecifier);
                                        if (num4 == 0U)
                                            builder.Append($"_M 0xC0000000 0x{(uint)((int)((num3 & 65280U) >> 8) - 1):X08}\r\n_M 0x{(((int)num3 & (int)byte.MaxValue) == 1 ? 2 : 1):X01}{num5:X07} 0x{num6:X08}\r\n_M 0xDC000000 0x{(uint)((int)num3 & (int)byte.MaxValue):X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                        else
                                            builder.Append($"_M 0xD5000000 0x{num6:X08}\r\n_M 0xC0000000 0x{(uint)((int)((num3 & 0xFF00U) >> 8) - 1):X08}\r\n_M 0x{(((int)num3 & (int)byte.MaxValue) == 1 ? 216 : 218):X02}000000 0x{num5:X08}\r\n_M 0xD4000000 0x{num4:X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                        num1 = 0U;
                                        num2 = 2U;
                                        break;
                                    }
#pragma warning disable CS0168 // Variable is declared but never used
                                    catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
                                    {
                                        break;
                                    }
                                case 112:
                                case 208:
                                    builder.Append($"_M 0x9{num3:X07} 0x0000{num4:X04}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                case 128:
                                    builder.Append($"_M 0x1{num3:X07} 0x{num4:X08}\r\n");
                                    break;

                                case 144:
                                case 209:
                                    builder.Append($"_M 0xA{num3:X07} 0x0000{num4:X04}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                case 194:
                                    try
                                    {
                                        var num5 = uint.Parse(strArray[index + 1].Split(' ')[1].Trim().Substring(4, 6), NumberStyles.AllowHexSpecifier) + 0x9800000U;
                                        builder.Append($"_M 0xD3000000 0x{num3:X08}\r\n_M 0xF{num5:X07} 0x{num4:X08}\r\n_M 0xD2000000 0x00000000\r\n");
                                        num1 = 0U;
                                        num2 = 2U;
                                        break;
                                    }
#pragma warning disable CS0168 // Variable is declared but never used
                                    catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
                                    {
                                        break;
                                    }
                                case 210:
                                    builder.Append($"_M 0x7{num3:X07} 0x0000{num4:X04}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                case 211:
                                    builder.Append($"_M 0x8{num3:X07} 0x0000{num4:X04}\r\n");
                                    num1 = 2U;
                                    break;

                                case 224:
                                    builder.Append(num3 % 2U != 1U ? $"_M 0x9{num3:X07} 0x00FF{(uint)((int)num4 & (int)byte.MaxValue):X02}00\r\n" : $"_M 0x9{(uint)((int)num3 - 1):X07} 0xFF0000{(uint)((int)num4 & (int)byte.MaxValue):X02}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                case 225:
                                    builder.Append(num3 % 2U != 1U ? $"_M 0xA{num3:X07} 0x00FF{(uint)((int)num4 & (int)byte.MaxValue):X02}00\r\n" : $"_M 0xA{(uint)((int)num3 - 1):X07} 0xFF0000{(uint)((int)num4 & (int)byte.MaxValue):X02}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                case 226:
                                    builder.Append(num3 % 2U != 1U ? $"_M 0x7{num3:X07} 0x00FF{(uint)((int)num4 & (int)byte.MaxValue):X02}00\r\n" : $"_M 0x7{(uint)((int)num3 - 1):X07} 0xFF0000{(uint)((int)num4 & (int)byte.MaxValue):X02}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                case 227:
                                    builder.Append(num3 % 2U != 1U ? $"_M 0x8{num3:X07} 0x00FF{(uint)((int)num4 & (int)byte.MaxValue):X02}00\r\n" : $"_M 0x8{(uint)((int)num3 - 1):X07} 0xFF0000{(uint)((int)num4 & (int)byte.MaxValue):X02}\r\n");
                                    str2 = strArray[index];
                                    num1 = 2U;
                                    break;

                                default:
                                    builder.Append($"#{strArray[index]} [unsupported]\r\n");
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            return string.Empty;
                        }
                    }
                }
                else
                    builder.Append($"{strArray[index]}\r\n");
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
                                builder.Append("_M 0xD2000000 0x00000000\r\n");
                                str2 = null;
                            }
                        }
                        else
                        {
                            builder.Append("_M 0xD2000000 0x00000000\r\n");
                            str2 = null;
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static string Nitepr_pspar(string input)
        {
            var str = string.Empty;
            var strArray = Converter.Reformat_nitepr(input).Split('\n');
            var num1 = uint.MaxValue;
            var builder = new StringBuilder();
            for (int index = 0; index < strArray.Length; ++index)
            {
                try
                {
                    if (Converter.Is_codeline(strArray[index], 1))
                    {
                        var num2 = uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(3), NumberStyles.AllowHexSpecifier);
                        var num3 = uint.Parse(strArray[index].Split(' ')[2].Trim().Substring(2), NumberStyles.AllowHexSpecifier);
                        var num4 = uint.Parse(strArray[index].Split(' ')[1].Trim().Substring(2, 1), NumberStyles.AllowHexSpecifier);
                        if (num1 == uint.MaxValue && num2 < 0x8800000U)
                            num2 += 0x8800000U;
                        if (num4 == 15U & num2 == 0xFFFFFFFU)
                        {
                            if ((int)num1 != (int)num3)
                            {
                                if (num1 != uint.MaxValue) builder.Append("_M 0xD2000000 0x00000000\r\n");
                                num1 = num3;
                                builder.Append($"_M 0x6{(object)(uint)((int)num1 + 142606336):X07} 0x00000000\r\n_M 0xB{(object)(uint)((int)num1 + 142606336):X07} 0x00000000\r\n");
                            }
                        }
                        else
                        {
                            switch (strArray[index].Split(' ')[2].Trim().Length)
                            {
                                case 4:
                                    builder.Append($"_M 0x2{(object)num2:X07} 0x{(object)num3:X08}\r\n");
                                    continue;
                                case 6:
                                    builder.Append($"_M 0x1{(object)num2:X07} 0x{(object)num3:X08}\r\n");
                                    continue;
                                default:
                                    builder.Append($"_M 0x0{(object)num2:X07} 0x{(object)num3:X08}\r\n");
                                    continue;
                            }
                        }
                    }
                    else
                    {
                        if (num1 != uint.MaxValue)
                        {
                            builder.Append("_M 0xD2000000 0x00000000\r\n");
                            num1 = uint.MaxValue;
                        }
                        builder.Append($"{strArray[index]}\r\n");
                    }
                }
                catch (Exception ex)
                {
                    builder.Append($";Exception: {ex.Message}\r\n");
                }
            }

            if (num1 != uint.MaxValue) builder.Append("_M 0xD2000000 0x00000000\r\n");
            return builder.ToString();
        }

        public static string Reformat_cwcpops(string input)
        {
            var pattern = new List<string>();
            var replacement = new List<string>();
            var subject = input;
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
            return Converter.Preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string Reformat_nitepr(string input)
        {
            var pattern = new List<string>();
            var replacement = new List<string>();
            var subject = input;
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
            return Converter.Preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string Reformat_r4cce(string input, bool show_folders_comments)
        {
            var pattern = new List<string>();
            var replacement = new List<string>();
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
            return Converter.Preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static string Reformat_tempar(string input)
        {
            var pattern = new List<string>();
            var replacement = new List<string>();
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
            return Converter.Preg_replace(pattern, replacement, subject, RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }

        public static bool Is_codeline(string input, int format)
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

        private static string Port_addresses(string subject, int difference)
        {
            var matchCollection = new Regex("(_M|_L)*\\s*0x(.)(.{7})\\s*0x(.{8})").Matches(subject);
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

        private static string Preg_replace(
          List<string> pattern,
          List<string> replacement,
          string subject,
          RegexOptions options)
        {
            var input = subject;
            for (int index = 0; index < pattern.Count; ++index)
                input = new Regex(pattern[index], options).Replace(input, replacement[index]);
            return input;
        }
    }
}