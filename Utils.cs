using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace TempAR
{
    public static class Utils
    {
        /// <summary>
        /// Convert DEC to HEX
        /// 十六进制地址转10进制数值
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static uint ParseNum(string s)
        {
            return ParseNum(s, NumberStyles.None);
        }

        /// <summary>
        /// Convert DEC to HEX
        /// 十六进制地址转10进制数值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="numstyle"></param>
        /// <returns></returns>
        public static uint ParseNum(string s, NumberStyles numstyle, string title = "Wrong Format!")
        {
            try
            {
                if (s.Trim().Length == 0)
                    return 0;
                if (s.StartsWith("0x"))
                    return uint.Parse(s.Remove(0, 2), NumberStyles.AllowHexSpecifier);
                return uint.Parse(s, numstyle);
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to parse, please make sure the value is a valid hexadecimal number.", title);
                return 0;
            }
        }

        /// <summary>
        /// Open Directory
        /// 打开目录
        /// </summary>
        /// <param name="defaultdir"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static string OpenDirectory(string defaultdir, string description)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = defaultdir.Length > 0 ? defaultdir : Directory.GetCurrentDirectory(),
                Description = description,
                ShowNewFolderButton = false
            })
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    return folderBrowserDialog.SelectedPath;
                return defaultdir;
            }
        }

        /// <summary>
        /// Open a File
        /// 打开文件
        /// </summary>
        /// <param name="defaultfile"></param>
        /// <param name="filter"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string OpenFile(string defaultfile, string filter = null, string title = "Open file")
        {
            using (var openFileDialog = new OpenFileDialog
            {
                FileName = defaultfile.Length > 0 ? defaultfile : null,
                InitialDirectory = defaultfile.Length > 0 ? defaultfile : Directory.GetCurrentDirectory(),
                Filter = filter,
                Title = title
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    return openFileDialog.FileName;
                return defaultfile;
            }
        }

        /// <summary>
        /// Save File
        /// 保存文件
        /// </summary>
        /// <param name="defaultfile"></param>
        /// <param name="filter"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string SaveFile(string defaultfile, string filter = null, string title = "Save File")
        {
            using (var saveFileDialog = new SaveFileDialog
            {
                FileName = defaultfile.Length > 0 ? defaultfile : null,
                InitialDirectory = defaultfile.Length > 0 ? defaultfile : Directory.GetCurrentDirectory(),
                Filter = filter,
                Title = title
            })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    return saveFileDialog.FileName;
                return defaultfile;
            }
        }

        /// <summary>
        /// Data Sorting
        /// 数据排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="fieldName"></param>
        /// <param name="asc"></param>
        public static void SortList<T>(List<T> dataSource, string fieldName, bool asc)
        {
            var propInfo = typeof(T).GetProperty(fieldName);
            int comparison(T a, T b)
            {
                var obj1 = asc ? propInfo.GetValue(a, null) : propInfo.GetValue(b, null);
                var obj2 = asc ? propInfo.GetValue(b, null) : propInfo.GetValue(a, null);
                if (!(obj1 is IComparable)) return 0;
                return ((IComparable)obj1).CompareTo(obj2);
            }
            dataSource.Sort(comparison);
        }

        ///
        /// Count lines for Conditionals and ButtonPad
        ///
        public static int RelatedLines(int PtrLvl, string CodeType, uint ButtonType, int ConCheck)
        {
            var Lines = 0;
            if (ConCheck == 1)
            {
                if (ButtonType == 9) { ButtonType = 1; } // If ButtonPad is set to 'None', still count lines as if it wasn't
                else { Lines = 1; }
            }
            switch (ButtonType)
            {
                case 9:
                    return 0;

                default:
                    switch (CodeType)
                    {
                        case "Compress ($4...)":
                            Lines = Lines + 2;
                            return Lines;
                        case "Pointer Write ($3...)":
                            Lines = Lines + (PtrLvl + 1) + 1;
                            return Lines;
                        case "Pointer MOV ($8...)":
                            Lines = Lines + 2 * (PtrLvl + 1) + 2;
                            return Lines;
                        case "Pointer Compress ($7...)":
                            Lines = Lines + (PtrLvl + 1) + 2; ;
                            return Lines;
                        default:
                            return Lines + 1;
                    }
            }
        }
    }
}