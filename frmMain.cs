
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TempAR
{
    public class frmMain : Form
    {
        private PointerSearcher memdump;
        private PointerSearcher memdump2;
        private uint memory_start;
#pragma warning disable CS0649 //I got no fucking idea what this is... but it ain't breaking shit.
        private IContainer components;
#pragma warning restore CS0649 //I got no fucking idea what this is... but it ain't breaking shit.
        private Panel pnlConvertFormat;
        private Panel pnlConvertFile;
        private TextBox txtOutputPath;
        private TextBox txtInputPath;
        private Label lblOutputPath;
        private Label lblInputPath;
        private Button btnOutputBrowse;
        private Button btnInputBrowse;
        private StatusStrip frmStatusStrip;
        private ToolStripStatusLabel lblStatus;
        private TabControl tctrlTabs;
        private TabPage tabConverter;
        private Panel pnlConvertText;
        private TextBox txtTextOutput;
        private TextBox txtTextInput;
        private TabPage tabPointerSearcher;
        private Button btnConvert;
        private RadioButton rdbConvertFile;
        private RadioButton rdbConvertText;
        private Panel pnlConvertType;
        private Label lblPointerSearcherMaxOffset;
        private TextBox txtPointerSearcherMaxOffset;
        private Label lblPointerSearcherAddress1;
        private TextBox txtPointerSearcherAddress1;
        private Button btnPointerSearcherFindPointers;
        private TreeView treePointerSearcherPointers;
        private Button btnPointerSearcherClear;
        private CheckBox chkPointerSearcherIncludeNegatives;
        private TextBox txtPointerSearcherCode;
        private Label lblPointerSearcherMemDump1;
        private TextBox txtPointerSearcherMemDump1;
        private CheckBox chkPointerSearcherRealAddresses;
        private Panel pnlPointerSearcherBitType;
        private RadioButton rdbPointerSearcherBitType32;
        private RadioButton rdbPointerSearcherBitType8;
        private RadioButton rdbPointerSearcherBitType16;
        private Label lblPointerSearcherValue;
        private TextBox txtPointerSearcherValue;
        private CheckBox chkPointerSearcherRAWCode;
        private CheckBox chkPointerSearcherOptimizePointerPaths;
        private Label lblPointerSearcherMemDump2;
        private Label lblPointerSearcherAddress2;
        private TextBox txtPointerSearcherMemDump2;
        private TextBox txtPointerSearcherAddress2;
        private ComboBox comboPointerSearcherMode;
        private Label lblPointerSearcherMode;
        private Panel pnlPointerSearcherCodeType;
        private TextBox txtBaseAddress;
        private TabPage tabVitaCheat;
        private Label lblVitaCheatAddress1;
        private TextBox txtVitaCheatAddress1;
        private Label lblVitaCheatAddress2;
        private TextBox txtVitaCheatAddress2;
        private TextBox txtVitaCheatCode;
        private Label lblVitaCheatValue;
        private TextBox txtVitaCheatValue;
        private Button btnVitaCheatGenerate;
        private Panel pnlVitaCheatBitType;
        private RadioButton rdbVitaCheatBitType8Bit;
        private GroupBox groupVitaCheatAddress2Offset;
        private TextBox txtVitaCheatAddress2Offset5;
        private TextBox txtVitaCheatAddress2Offset4;
        private TextBox txtVitaCheatAddress2Offset3;
        private TextBox txtVitaCheatAddress2Offset2;
        private TextBox txtVitaCheatAddress2Offset1;
        private GroupBox groupVitaCheatAddress1Offset;
        private TextBox txtVitaCheatAddress1Offset5;
        private TextBox txtVitaCheatAddress1Offset4;
        private TextBox txtVitaCheatAddress1Offset3;
        private TextBox txtVitaCheatAddress1Offset2;
        private TextBox txtVitaCheatAddress1Offset1;
        private RadioButton rdbVitaCheatBitType32Bit;
        private RadioButton rdbVitaCheatBitType16Bit;
        private ComboBox comboVitaCheatPointerLevel;
        private Label lblVitaCheatPointerLevel;
        private GroupBox groupVitaCheatCompression;
        private Label lblVitaCheatValueGap;
        private Label lblVitaCheatAddressGap;
        private Label lblVitaCheatCompressions;
        private TextBox txtVitaCheatValueGap;
        private TextBox txtVitaCheatAddressGap;
        private NumericUpDown numericVitaCheatCompressions;
        private Label lblBaseAddress;
        // Code types for converter tab
        private Label lblCnvCodeTypes;
        private ComboBox cbCnvCodeTypes;
        private const string CT_CNV_CWCHEATPOPS = "CWCheat POPS";
        private const string CT_CNV_NITEPR      = "NitePR";
        private const string CT_CNV_R4CCE       = "R4CCE to TempAR";
        private const string CT_CNV_TEMPAR      = "TempAR to R4CCE";
        // Code types for pointer search tab
        private Label lblPntCodeTypes;
        private ComboBox cbPntCodeTypes;
        private const string CT_PNT_VITACHEAT = "VitaCheat";
        private const string CT_PNT_CWCHEAT   = "CWCheat";        
        private const string CT_PNT_AR        = "AR";
        // Code types for VitaCheat Code Maker tab
        private Label lblVitaCheatCodeType;
        private ComboBox comboVitaCheatCodeType;
        private const string VC_GEN_WRITE = "Write ($0...)";
        private const string VC_GEN_PNTR = "Pointer ($3...)";
        private const string VC_GEN_COMP = "Compress ($4...)";
        private const string VC_GEN_MOV = "MOV/Copy ($5...)";
        private const string VC_GEN_PTRCOM = "Pointer+Compress ($7...)";
        private const string VC_GEN_PTRMOV = "Pointer+MOV ($8...)";


        public frmMain()
        {
            this.InitializeComponent();

            this.cbCnvCodeTypes.Items.AddRange(new object[] {
            CT_CNV_CWCHEATPOPS,
            CT_CNV_NITEPR,
            CT_CNV_R4CCE,
            CT_CNV_TEMPAR});
            this.cbCnvCodeTypes.Text = CT_CNV_CWCHEATPOPS;

            this.cbPntCodeTypes.Items.AddRange(new object[] {
            CT_PNT_VITACHEAT,
            CT_PNT_CWCHEAT,
            CT_PNT_AR});
            this.cbPntCodeTypes.Text = CT_PNT_VITACHEAT;

            this.comboVitaCheatCodeType.Items.AddRange(new object[] {
            VC_GEN_WRITE,
            VC_GEN_PNTR,
            VC_GEN_COMP,
            VC_GEN_MOV,
            VC_GEN_PTRCOM,
            VC_GEN_PTRMOV});
            this.comboVitaCheatCodeType.Text = VC_GEN_WRITE;
        }

        private void rdbConvertText_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
            this.ChangeFrameMode(1);
        }

        private void rdbConvertFile_CheckedChanged(object sender, EventArgs e)
        {
            if (!((RadioButton)sender).Checked)
                return;
            this.ChangeFrameMode(2);
        }

        private void txtTextInput_TextChanged(object sender, EventArgs e)
        {
            this.txtTextInput.Text = this.txtTextInput.Text.Replace("\r\n", "\n").Replace("\n", "\r\n");
            this.btnConvert_Click(sender, e);
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            this.lblStatus.Text = "Working...";
            this.lblStatus.Visible = true;
            this.Refresh();
            if (this.rdbConvertText.Checked)
            {
                switch (this.cbCnvCodeTypes.Text)
                {
                    case CT_CNV_CWCHEATPOPS:
                        this.txtTextOutput.Text = Converter.cwcpops_pspar(this.txtTextInput.Text);
                        break;
                    case CT_CNV_NITEPR:
                        this.txtTextOutput.Text = Converter.nitepr_pspar(this.txtTextInput.Text);
                        break;
                    case CT_CNV_R4CCE:
                        this.txtTextOutput.Text = Converter.reformat_r4cce(this.txtTextInput.Text, true);
                        break;
                    case CT_CNV_TEMPAR:
                        this.txtTextOutput.Text = Converter.reformat_tempar(this.txtTextInput.Text);
                        break;

                }
            }
            else if (this.rdbConvertFile.Checked && ((Control)sender).Name == "btnConvert" && (this.txtInputPath.Text.Length > 0 && this.txtOutputPath.Text.Length > 0))
            {
                switch (this.cbCnvCodeTypes.Text)
                {
                    case CT_CNV_CWCHEATPOPS:
                        if (File.Exists(this.txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(this.txtOutputPath.Text)))
                            Converter.file_cwcpops_pspar(this.txtInputPath.Text, this.txtOutputPath.Text);
                        break;
                    case CT_CNV_NITEPR:
                        if (Directory.Exists(this.txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(this.txtOutputPath.Text)))
                            Converter.file_nitepr_pspar(this.txtInputPath.Text, this.txtOutputPath.Text);
                        break;
                    case CT_CNV_R4CCE:
                        MessageBox.Show("File conversion not supported for this code type");
                        break;
                    case CT_CNV_TEMPAR:
                        if (File.Exists(this.txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(this.txtOutputPath.Text)))
                            Converter.file_reformat_tempar(this.txtInputPath.Text, this.txtOutputPath.Text);
                        break;

                }
            }
            this.lblStatus.Visible = false;
        }

        private void btnInputBrowse_Click(object sender, EventArgs e)
        {
            switch (this.cbCnvCodeTypes.Text)
            {
                case CT_CNV_CWCHEATPOPS:
                case CT_CNV_TEMPAR:
                    this.txtInputPath.Text = this.OpenFile(this.txtInputPath.Text, "CWCheat Database File (*.db)|*.db", "Open");
                    break;
                case CT_CNV_NITEPR:
                    this.txtInputPath.Text = this.OpenDirectory(this.txtInputPath.Text, "Select your NitePR code file directory:");
                    break;
                case CT_CNV_R4CCE:
                    MessageBox.Show("File conversion not supported for this code type");
                    break;

            }
        }

        private void btnOutputBrowse_Click(object sender, EventArgs e)
        {
            this.txtOutputPath.Text = this.SaveFile(this.txtOutputPath.Text, "TempAR Database File (*.db)|*.db", "Save");
        }

        private void textFieldSelectAll(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBoxBase)sender).SelectAll();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
                this.OnKeyDown(e);
        }

        private void ChangeFrameMode(int mode)
        {
            if (mode == 1)
            {
                this.pnlConvertText.BringToFront();
            }
            else
            {
                this.pnlConvertFile.BringToFront();
                if (!String.IsNullOrEmpty(this.cbCnvCodeTypes.Text) && this.cbCnvCodeTypes.Text == CT_CNV_R4CCE)
                {
                    this.cbCnvCodeTypes.Text = CT_CNV_CWCHEATPOPS;
                }
            }
        }

        private string OpenDirectory(string defaultdir, string description)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = defaultdir.Length > 0 ? defaultdir : Directory.GetCurrentDirectory();
            folderBrowserDialog.Description = description;
            folderBrowserDialog.ShowNewFolderButton = false;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                return folderBrowserDialog.SelectedPath;
            return defaultdir;
        }

        private string OpenFile(string defaultfile, string filter, string title)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = defaultfile.Length > 0 ? defaultfile : (string)null;
            openFileDialog.InitialDirectory = defaultfile.Length > 0 ? defaultfile : Directory.GetCurrentDirectory();
            openFileDialog.Filter = filter;
            openFileDialog.Title = title;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;
            return defaultfile;
        }

        private string SaveFile(string defaultfile, string filter, string title)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = defaultfile.Length > 0 ? defaultfile : (string)null;
            saveFileDialog.InitialDirectory = defaultfile.Length > 0 ? defaultfile : Directory.GetCurrentDirectory();
            saveFileDialog.Filter = filter;
            saveFileDialog.Title = title;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                return saveFileDialog.FileName;
            return defaultfile;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.comboPointerSearcherMode.SelectedIndex = 0;
            this.comboVitaCheatCodeType.SelectedIndex = 0;
            this.comboVitaCheatPointerLevel.SelectedIndex = 0;
        }
        //
        //
        // Pointer Searcher Tab starts here
        //
        //
        private void btnPointerSearcherFindPointers_Click(object sender, EventArgs e)
        {
            uint num1;
            try
            {
                num1 = this.parseNum(this.txtPointerSearcherAddress1.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse address, make sure value is a valid hexadecimal number.");
                return;
            }
            uint num3;
            try
            {
                num3 = this.parseNum(this.txtPointerSearcherMaxOffset.Text);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse maximum offset, make sure value is a valid hexadecimal number.");
                return;
            }
            try
            {
                this.memory_start = this.parseNum(this.txtBaseAddress.Text);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse base address, make sure value is a valid hexadecimal number.");
                return;
            }
            this.treePointerSearcherPointers.Nodes.Clear();
            this.memdump = new PointerSearcher(this.txtPointerSearcherMemDump1.Text, this.memory_start);
            this.memdump2 = new PointerSearcher(this.txtPointerSearcherMemDump2.Text, this.memory_start);
            this.addPointerTree(this.memdump.findPointers(num1, num3), this.treePointerSearcherPointers.SelectedNode);
        }

        private void treePointerSearcherPointers_DoubleClick(object sender, EventArgs e)
        {
            if (this.treePointerSearcherPointers.SelectedNode == null)
                return;
            uint num1;
            try
            {
                num1 = this.parseNum(this.txtPointerSearcherMaxOffset.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse maximum offset, make sure value is a valid hexadecimal number.");
                return;
            }
            this.treePointerSearcherPointers.SelectedNode.Nodes.Clear();
            this.addPointerTree(this.memdump.findPointers(new PointerSearcherLog(this.treePointerSearcherPointers.SelectedNode.Text, this.memory_start).Address, num1), this.treePointerSearcherPointers.SelectedNode);
        }

        private void btnPointerSearcherClear_Click(object sender, EventArgs e)
        {
            this.treePointerSearcherPointers.Nodes.Clear();
        }

        public void SortList<T>(List<T> dataSource, string fieldName, bool asc)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(fieldName);
            Comparison<T> comparison = (Comparison<T>)((a, b) =>
           {
               object obj1 = asc ? propInfo.GetValue((object)a, (object[])null) : propInfo.GetValue((object)b, (object[])null);
               object obj2 = asc ? propInfo.GetValue((object)b, (object[])null) : propInfo.GetValue((object)a, (object[])null);
               if (!(obj1 is IComparable))
                   return 0;
               return ((IComparable)obj1).CompareTo(obj2);
           });
            dataSource.Sort(comparison);
        }

        private void treePointerSearcherPointers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treePointerSearcherPointers.SelectedNode == null)
                return;
            List<PointerSearcherLog> pointers = new List<PointerSearcherLog>();
            string[] strArray = this.treePointerSearcherPointers.SelectedNode.FullPath.ToString().Split('\\');
            for (int index = 0; index < strArray.Length; ++index)
                pointers.Add(new PointerSearcherLog(strArray[strArray.Length - 1 - index], this.memory_start));
            uint num = 0;
            try
            {
                num = this.parseNum(this.txtPointerSearcherValue.Text);
            }
            catch
            {
            }
            int bittype = 2;
            if (this.rdbPointerSearcherBitType16.Checked)
            {
                bittype = 1;
                num &= (uint)ushort.MaxValue;
            }
            else if (this.rdbPointerSearcherBitType8.Checked)
            {
                bittype = 0;
                num &= (uint)byte.MaxValue;
            }
            //
            // Check which code is being generated
            //
            switch (this.cbPntCodeTypes.Text)
            {
                case CT_PNT_VITACHEAT:
                    this.txtPointerSearcherCode.Text = this.getVitaCheatPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
                    break;
                case CT_PNT_CWCHEAT:
                    this.txtPointerSearcherCode.Text = this.getCWCheatPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
                    break;
                case CT_PNT_AR:
                    this.txtPointerSearcherCode.Text = this.getARPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
                    break;
            }
        }

        private void txtPointerSearcherMemDump1_Click(object sender, EventArgs e)
        {
            this.txtPointerSearcherMemDump1.Text = this.OpenFile(this.txtPointerSearcherMemDump1.Text, (string)null, "Open");
        }

        private void txtPointerSearcherMemDump2_Click(object sender, EventArgs e)
        {
            this.txtPointerSearcherMemDump2.Text = this.OpenFile(this.txtPointerSearcherMemDump2.Text, (string)null, "Open");
        }

        private void treePointerSearcherPointers_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Space:
                    this.treePointerSearcherPointers_DoubleClick((object)null, (EventArgs)null);
                    break;
                case Keys.Delete:
                    if (this.treePointerSearcherPointers.SelectedNode == null)
                        break;
                    this.treePointerSearcherPointers.SelectedNode.Remove();
                    break;
            }
        }

        private void addPointerTree(List<PointerSearcherLog> pointers, TreeNode parent)
        {
            if (pointers == null)
                return;
            this.SortList<PointerSearcherLog>(pointers, "Address", true);
            if (this.chkPointerSearcherOptimizePointerPaths.Checked)
            {
                List<PointerSearcherLog> pointerSearcherLogList = new List<PointerSearcherLog>();
                if (this.treePointerSearcherPointers.Nodes.Count > 0)
                {
                    TreeNodeCollection parentEqualTree = this.getParentEqualTree(this.treePointerSearcherPointers.Nodes, this.treePointerSearcherPointers.SelectedNode == null ? 0 : this.treePointerSearcherPointers.SelectedNode.Level);
                    for (int index = 0; index < parentEqualTree.Count; ++index)
                        pointerSearcherLogList.Add(new PointerSearcherLog(parentEqualTree[index].Text, this.memory_start));
                }
                for (int index1 = 0; index1 < pointerSearcherLogList.Count; ++index1)
                {
                    for (int index2 = 0; index2 < pointers.Count; ++index2)
                    {
                        if ((int)pointerSearcherLogList[index1].Address == (int)pointers[index2].Address)
                            pointers.RemoveAt(index2);
                    }
                }
            }
            for (int index1 = 0; index1 < pointers.Count; ++index1)
            {
                Color color = Color.Black;
                if (this.memdump2 != null)
                {
                    string[] strArray = ((this.treePointerSearcherPointers.SelectedNode == null ? "" : this.treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1].ToString()).Split('\\');
                    uint num = this.parseNum(this.txtPointerSearcherAddress2.Text, NumberStyles.AllowHexSpecifier);
                    if (num < this.memory_start)
                        num += this.memory_start;
                    uint address = 0;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        PointerSearcherLog pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], this.memory_start);
                        if (index2 == 0)
                            address = pointerSearcherLog.Address;
                        address = this.memdump2.getPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                    }
                    if ((int)num == (int)address)
                        color = Color.Green;
                }
                if (!pointers[index1].Negative || this.chkPointerSearcherIncludeNegatives.Checked)
                {
                    TreeNode node = new TreeNode();
                    node.Text = pointers[index1].ToString(this.chkPointerSearcherRealAddresses.Checked ? 0U : this.memory_start);
                    node.ForeColor = color;
                    if (parent == null)
                        this.treePointerSearcherPointers.Nodes.Add(node);
                    else
                        parent.Nodes.Add(node);
                }
            }
        }

        private TreeNodeCollection getParentEqualTree(
          TreeNodeCollection nodes,
          int level)
        {
            TreeView treeView = new TreeView();
            foreach (TreeNode node in nodes)
            {
                if (node.Level <= level)
                {
                    treeView.Nodes.Add(node.Text);
                    foreach (TreeNode treeNode in this.getParentEqualTree(node.Nodes, level))
                        treeView.Nodes.Add(treeNode.Text);
                }
            }
            return treeView.Nodes;
        }

        private uint parseNum(string s)
        {
            return this.parseNum(s, NumberStyles.None);
        }

        private uint parseNum(string s, NumberStyles numstyle)
        {
            if (s.Trim().Length == 0)
                return 0;
            if (s.StartsWith("0x"))
                return uint.Parse(s.Remove(0, 2), NumberStyles.AllowHexSpecifier);
            return uint.Parse(s, numstyle);
        }
        //
        // Default values for "Base Address"
        //
        private void comboPointerSearcherMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBaseAddress.Enabled = false;
            switch (this.comboPointerSearcherMode.SelectedIndex)
            {
                case 0: // Sony Vita
                    this.memory_start = 2164260864U;
                    break;
                case 1: // Sony PSP
                    this.memory_start = 142606336U;
                    break;
                case 2: // Nintendo DS
                    this.memory_start = 33554432U;
                    break;
                case 3: // Other..
                    this.memory_start = 0U;
                    this.txtBaseAddress.Enabled = true;
                    break;
            }
            this.txtBaseAddress.Text = string.Format("0x{0:X08}", (object)this.memory_start);
        }
        //
        // AR Code Generation
        //
        private string getARPointerCode(List<PointerSearcherLog> pointers, int bittype, uint value)
        {
            switch (bittype)
            {
                case 0:
                    bittype = 2;
                    break;
                case 1:
                    bittype = 1;
                    break;
                case 2:
                    bittype = 0;
                    break;
                default:
                    bittype = 0;
                    break;
            }
            string str1 = "";
            for (int index = 0; index < pointers.Count; ++index)
                str1 = !pointers[index].Negative ? str1 + string.Format("{0:X01}{1:X07} {2:X08}\n", (object)(index == pointers.Count - 1 ? bittype : 11), (object)pointers[index].Offset, (object)(uint)(index == pointers.Count - 1 ? (int)value : 0)) : str1 + string.Format("DC000000 {0:X08}\n{1:X01}0000000 {2:X08}\n", (object)(4294967296L - (long)pointers[index].Offset), (object)(index == pointers.Count - 1 ? bittype : 11), (object)(uint)(index == pointers.Count - 1 ? (int)value : 0));
            string str2 = string.Format("6{0:X07} 00000000\nB{0:X07} 00000000\n{1}D2000000 00000000", (object)pointers[0].Address, (object)str1);
            return (this.chkPointerSearcherRAWCode.Checked ? "" : "::Generated Code\n") + str2;
        }
        //
        // VitaCheat Code Generation
        //
        private string getVitaCheatPointerCode(List<PointerSearcherLog> pointers, int bittype, uint value)
        {
            switch (bittype)
            {
                case 0:
                    bittype = 0;
                    break;
                case 1:
                    bittype = 1;
                    break;
                case 2:
                    bittype = 2;
                    break;
                default:
                    bittype = 2;
                    break;
            }
            string str1 = "";
            string str2 = "";
            string str3 = string.Format("$3300 00000000 {0:X08}\n", (object)value);
            for (int index = 1; index < pointers.Count; ++index)
                str1 = !pointers[index].Negative ? str1 + string.Format("$3{0}00 00000000 {1:X08}\n", (object)bittype, (object)pointers[index].Offset) : str1 + string.Format("$3{0}00 00000000 {1:X08}\n", (object)bittype, (object)(4294967296L - (long)pointers[index].Offset));
            if (pointers.Count > 1)
                str1 += string.Format("");
            str2 = !pointers[0].Negative ? str2 + string.Format("$3{0:X01}{1:X02} {2:X08} {3:X08}\n", (object)bittype, (object)pointers.Count, (object)pointers[0].Address, (object)pointers[0].Offset) + str1 : str2 + string.Format("$3{0:X01}{1:X02} {2:X08} {3:X08}\n", (object)bittype, (object)pointers.Count, (object)pointers[0].Address, (object)(4294967296L - (long)pointers[0].Offset)) + str1;
            return (this.chkPointerSearcherRAWCode.Checked ? "" : "_V0 Generated Code\n") + str2 + str3;
        }
        //
        // CWCheat Code Generation
        //
        private string getCWCheatPointerCode(
      List<PointerSearcherLog> pointers,
      int bittype,
      uint value)
        {
            if (bittype != 0 && bittype != 1 && bittype != 2)
                bittype = 2;
            if (pointers[0].Negative)
                bittype += 3;
            string str1 = "";
            for (int index = 0; index < pointers.Count - 1; ++index)
                str1 = index % 2 != 0 ? str1 + string.Format(" 0x{0:X01}{1:X07}\n", (object)(pointers[index].Negative ? 3 : 2), (object)pointers[index].Offset) : str1 + string.Format("{0}0x{1:X01}{2:X07}", this.chkPointerSearcherRAWCode.Checked ? (object)"" : (object)"_L ", (object)(pointers[index].Negative ? 3 : 2), (object)pointers[index].Offset);
            if (pointers.Count % 2 == 0)
                str1 += string.Format(" 0x00000000");
            string str2 = string.Format("{0}0x6{1:X07} 0x{2:X08}\n{0}0x000{3:X01}{4:X04} 0x{5:X08}\n", this.chkPointerSearcherRAWCode.Checked ? (object)"" : (object)"_L ", (object)(uint)((int)pointers[0].Address - (int)this.memory_start), (object)value, (object)bittype, (object)pointers.Count, (object)pointers[pointers.Count - 1].Offset) + str1;
            return (this.chkPointerSearcherRAWCode.Checked ? "" : "_C0 Generated Code\n") + str2;
        }

        private void txtFileDragDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void txtFileDragDrop_DragDrop(object sender, DragEventArgs e)
        {
            ((Control)sender).Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void txtValidateHexString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "[^0-9a-fA-F\x0001\x0003\b\x0016]"))
                return;
            e.Handled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlConvertFormat = new System.Windows.Forms.Panel();
            this.lblCnvCodeTypes = new System.Windows.Forms.Label();
            this.cbCnvCodeTypes = new System.Windows.Forms.ComboBox();
            this.pnlConvertFile = new System.Windows.Forms.Panel();
            this.btnOutputBrowse = new System.Windows.Forms.Button();
            this.btnInputBrowse = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.txtInputPath = new System.Windows.Forms.TextBox();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.lblInputPath = new System.Windows.Forms.Label();
            this.frmStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tctrlTabs = new System.Windows.Forms.TabControl();
            this.tabConverter = new System.Windows.Forms.TabPage();
            this.btnConvert = new System.Windows.Forms.Button();
            this.pnlConvertType = new System.Windows.Forms.Panel();
            this.rdbConvertText = new System.Windows.Forms.RadioButton();
            this.rdbConvertFile = new System.Windows.Forms.RadioButton();
            this.pnlConvertText = new System.Windows.Forms.Panel();
            this.txtTextOutput = new System.Windows.Forms.TextBox();
            this.txtTextInput = new System.Windows.Forms.TextBox();
            this.tabPointerSearcher = new System.Windows.Forms.TabPage();
            this.txtBaseAddress = new System.Windows.Forms.TextBox();
            this.lblBaseAddress = new System.Windows.Forms.Label();
            this.comboPointerSearcherMode = new System.Windows.Forms.ComboBox();
            this.pnlPointerSearcherCodeType = new System.Windows.Forms.Panel();
            this.lblPntCodeTypes = new System.Windows.Forms.Label();
            this.cbPntCodeTypes = new System.Windows.Forms.ComboBox();
            this.pnlPointerSearcherBitType = new System.Windows.Forms.Panel();
            this.rdbPointerSearcherBitType32 = new System.Windows.Forms.RadioButton();
            this.rdbPointerSearcherBitType8 = new System.Windows.Forms.RadioButton();
            this.rdbPointerSearcherBitType16 = new System.Windows.Forms.RadioButton();
            this.chkPointerSearcherRealAddresses = new System.Windows.Forms.CheckBox();
            this.txtPointerSearcherCode = new System.Windows.Forms.TextBox();
            this.chkPointerSearcherOptimizePointerPaths = new System.Windows.Forms.CheckBox();
            this.chkPointerSearcherRAWCode = new System.Windows.Forms.CheckBox();
            this.chkPointerSearcherIncludeNegatives = new System.Windows.Forms.CheckBox();
            this.btnPointerSearcherClear = new System.Windows.Forms.Button();
            this.btnPointerSearcherFindPointers = new System.Windows.Forms.Button();
            this.treePointerSearcherPointers = new System.Windows.Forms.TreeView();
            this.lblPointerSearcherValue = new System.Windows.Forms.Label();
            this.lblPointerSearcherMaxOffset = new System.Windows.Forms.Label();
            this.txtPointerSearcherValue = new System.Windows.Forms.TextBox();
            this.txtPointerSearcherMaxOffset = new System.Windows.Forms.TextBox();
            this.lblPointerSearcherMemDump2 = new System.Windows.Forms.Label();
            this.lblPointerSearcherMemDump1 = new System.Windows.Forms.Label();
            this.lblPointerSearcherMode = new System.Windows.Forms.Label();
            this.lblPointerSearcherAddress2 = new System.Windows.Forms.Label();
            this.lblPointerSearcherAddress1 = new System.Windows.Forms.Label();
            this.txtPointerSearcherMemDump2 = new System.Windows.Forms.TextBox();
            this.txtPointerSearcherMemDump1 = new System.Windows.Forms.TextBox();
            this.txtPointerSearcherAddress2 = new System.Windows.Forms.TextBox();
            this.txtPointerSearcherAddress1 = new System.Windows.Forms.TextBox();
            this.tabVitaCheat = new System.Windows.Forms.TabPage();
            this.groupVitaCheatCompression = new System.Windows.Forms.GroupBox();
            this.lblVitaCheatValueGap = new System.Windows.Forms.Label();
            this.lblVitaCheatAddressGap = new System.Windows.Forms.Label();
            this.lblVitaCheatCompressions = new System.Windows.Forms.Label();
            this.txtVitaCheatValueGap = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddressGap = new System.Windows.Forms.TextBox();
            this.numericVitaCheatCompressions = new System.Windows.Forms.NumericUpDown();
            this.lblVitaCheatPointerLevel = new System.Windows.Forms.Label();
            this.comboVitaCheatPointerLevel = new System.Windows.Forms.ComboBox();
            this.pnlVitaCheatBitType = new System.Windows.Forms.Panel();
            this.rdbVitaCheatBitType32Bit = new System.Windows.Forms.RadioButton();
            this.rdbVitaCheatBitType16Bit = new System.Windows.Forms.RadioButton();
            this.rdbVitaCheatBitType8Bit = new System.Windows.Forms.RadioButton();
            this.groupVitaCheatAddress2Offset = new System.Windows.Forms.GroupBox();
            this.txtVitaCheatAddress2Offset5 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress2Offset4 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress2Offset3 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress2Offset2 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress2Offset1 = new System.Windows.Forms.TextBox();
            this.groupVitaCheatAddress1Offset = new System.Windows.Forms.GroupBox();
            this.txtVitaCheatAddress1Offset5 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress1Offset4 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress1Offset3 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress1Offset2 = new System.Windows.Forms.TextBox();
            this.txtVitaCheatAddress1Offset1 = new System.Windows.Forms.TextBox();
            this.btnVitaCheatGenerate = new System.Windows.Forms.Button();
            this.txtVitaCheatCode = new System.Windows.Forms.TextBox();
            this.lblVitaCheatValue = new System.Windows.Forms.Label();
            this.txtVitaCheatValue = new System.Windows.Forms.TextBox();
            this.lblVitaCheatAddress2 = new System.Windows.Forms.Label();
            this.txtVitaCheatAddress2 = new System.Windows.Forms.TextBox();
            this.lblVitaCheatAddress1 = new System.Windows.Forms.Label();
            this.txtVitaCheatAddress1 = new System.Windows.Forms.TextBox();
            this.lblVitaCheatCodeType = new System.Windows.Forms.Label();
            this.comboVitaCheatCodeType = new System.Windows.Forms.ComboBox();
            this.pnlConvertFormat.SuspendLayout();
            this.pnlConvertFile.SuspendLayout();
            this.frmStatusStrip.SuspendLayout();
            this.tctrlTabs.SuspendLayout();
            this.tabConverter.SuspendLayout();
            this.pnlConvertType.SuspendLayout();
            this.pnlConvertText.SuspendLayout();
            this.tabPointerSearcher.SuspendLayout();
            this.pnlPointerSearcherCodeType.SuspendLayout();
            this.pnlPointerSearcherBitType.SuspendLayout();
            this.tabVitaCheat.SuspendLayout();
            this.groupVitaCheatCompression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericVitaCheatCompressions)).BeginInit();
            this.pnlVitaCheatBitType.SuspendLayout();
            this.groupVitaCheatAddress2Offset.SuspendLayout();
            this.groupVitaCheatAddress1Offset.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConvertFormat
            // 
            this.pnlConvertFormat.Controls.Add(this.lblCnvCodeTypes);
            this.pnlConvertFormat.Controls.Add(this.cbCnvCodeTypes);
            this.pnlConvertFormat.Location = new System.Drawing.Point(6, 6);
            this.pnlConvertFormat.Name = "pnlConvertFormat";
            this.pnlConvertFormat.Size = new System.Drawing.Size(440, 24);
            this.pnlConvertFormat.TabIndex = 7;
            // 
            // lblCnvCodeTypes
            // 
            this.lblCnvCodeTypes.AutoSize = true;
            this.lblCnvCodeTypes.Location = new System.Drawing.Point(8, 3);
            this.lblCnvCodeTypes.Name = "lblCnvCodeTypes";
            this.lblCnvCodeTypes.Size = new System.Drawing.Size(62, 13);
            this.lblCnvCodeTypes.TabIndex = 1;
            this.lblCnvCodeTypes.Text = "Code Type:";
            // 
            // cbCnvCodeTypes
            // 
            this.cbCnvCodeTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCnvCodeTypes.FormattingEnabled = true;
            this.cbCnvCodeTypes.Location = new System.Drawing.Point(77, 0);
            this.cbCnvCodeTypes.Name = "cbCnvCodeTypes";
            this.cbCnvCodeTypes.Size = new System.Drawing.Size(150, 21);
            this.cbCnvCodeTypes.TabIndex = 0;
            this.cbCnvCodeTypes.SelectedIndexChanged += new System.EventHandler(this.cbCodeTypes_SelectedIndexChanged);
            // 
            // pnlConvertFile
            // 
            this.pnlConvertFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConvertFile.Controls.Add(this.btnOutputBrowse);
            this.pnlConvertFile.Controls.Add(this.btnInputBrowse);
            this.pnlConvertFile.Controls.Add(this.txtOutputPath);
            this.pnlConvertFile.Controls.Add(this.txtInputPath);
            this.pnlConvertFile.Controls.Add(this.lblOutputPath);
            this.pnlConvertFile.Controls.Add(this.lblInputPath);
            this.pnlConvertFile.Location = new System.Drawing.Point(5, 40);
            this.pnlConvertFile.Name = "pnlConvertFile";
            this.pnlConvertFile.Size = new System.Drawing.Size(750, 700);
            this.pnlConvertFile.TabIndex = 8;
            // 
            // btnOutputBrowse
            // 
            this.btnOutputBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutputBrowse.Location = new System.Drawing.Point(676, 172);
            this.btnOutputBrowse.Name = "btnOutputBrowse";
            this.btnOutputBrowse.Size = new System.Drawing.Size(72, 23);
            this.btnOutputBrowse.TabIndex = 8;
            this.btnOutputBrowse.Text = "Browse";
            this.btnOutputBrowse.UseVisualStyleBackColor = true;
            this.btnOutputBrowse.Click += new System.EventHandler(this.btnOutputBrowse_Click);
            // 
            // btnInputBrowse
            // 
            this.btnInputBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInputBrowse.Location = new System.Drawing.Point(676, 146);
            this.btnInputBrowse.Name = "btnInputBrowse";
            this.btnInputBrowse.Size = new System.Drawing.Size(72, 23);
            this.btnInputBrowse.TabIndex = 7;
            this.btnInputBrowse.Text = "Browse";
            this.btnInputBrowse.UseVisualStyleBackColor = true;
            this.btnInputBrowse.Click += new System.EventHandler(this.btnInputBrowse_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputPath.Location = new System.Drawing.Point(82, 174);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(588, 20);
            this.txtOutputPath.TabIndex = 3;
            this.txtOutputPath.Click += new System.EventHandler(this.btnOutputBrowse_Click);
            // 
            // txtInputPath
            // 
            this.txtInputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputPath.Location = new System.Drawing.Point(82, 148);
            this.txtInputPath.Name = "txtInputPath";
            this.txtInputPath.ReadOnly = true;
            this.txtInputPath.Size = new System.Drawing.Size(588, 20);
            this.txtInputPath.TabIndex = 2;
            this.txtInputPath.Click += new System.EventHandler(this.btnInputBrowse_Click);
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOutputPath.AutoSize = true;
            this.lblOutputPath.Location = new System.Drawing.Point(9, 177);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(67, 13);
            this.lblOutputPath.TabIndex = 1;
            this.lblOutputPath.Text = "Output Path:";
            // 
            // lblInputPath
            // 
            this.lblInputPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInputPath.AutoSize = true;
            this.lblInputPath.Location = new System.Drawing.Point(9, 151);
            this.lblInputPath.Name = "lblInputPath";
            this.lblInputPath.Size = new System.Drawing.Size(59, 13);
            this.lblInputPath.TabIndex = 0;
            this.lblInputPath.Text = "Input Path:";
            // 
            // frmStatusStrip
            // 
            this.frmStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.frmStatusStrip.Location = new System.Drawing.Point(0, 639);
            this.frmStatusStrip.Name = "frmStatusStrip";
            this.frmStatusStrip.Size = new System.Drawing.Size(769, 22);
            this.frmStatusStrip.SizingGrip = false;
            this.frmStatusStrip.TabIndex = 1;
            this.frmStatusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 17);
            this.lblStatus.Text = "Status";
            this.lblStatus.Visible = false;
            // 
            // tctrlTabs
            // 
            this.tctrlTabs.Controls.Add(this.tabConverter);
            this.tctrlTabs.Controls.Add(this.tabPointerSearcher);
            this.tctrlTabs.Controls.Add(this.tabVitaCheat);
            this.tctrlTabs.Location = new System.Drawing.Point(5, 5);
            this.tctrlTabs.Name = "tctrlTabs";
            this.tctrlTabs.SelectedIndex = 1;
            this.tctrlTabs.Size = new System.Drawing.Size(765, 600);
            this.tctrlTabs.TabIndex = 0;
            // 
            // tabConverter
            // 
            this.tabConverter.Controls.Add(this.btnConvert);
            this.tabConverter.Controls.Add(this.pnlConvertType);
            this.tabConverter.Controls.Add(this.pnlConvertFormat);
            this.tabConverter.Controls.Add(this.pnlConvertText);
            this.tabConverter.Controls.Add(this.pnlConvertFile);
            this.tabConverter.Location = new System.Drawing.Point(4, 22);
            this.tabConverter.Name = "tabConverter";
            this.tabConverter.Padding = new System.Windows.Forms.Padding(3);
            this.tabConverter.Size = new System.Drawing.Size(757, 574);
            this.tabConverter.TabIndex = 0;
            this.tabConverter.Text = "Code Converter";
            this.tabConverter.UseVisualStyleBackColor = true;
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvert.Location = new System.Drawing.Point(666, 4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 4;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // pnlConvertType
            // 
            this.pnlConvertType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConvertType.Controls.Add(this.rdbConvertText);
            this.pnlConvertType.Controls.Add(this.rdbConvertFile);
            this.pnlConvertType.Location = new System.Drawing.Point(473, 6);
            this.pnlConvertType.Name = "pnlConvertType";
            this.pnlConvertType.Size = new System.Drawing.Size(187, 24);
            this.pnlConvertType.TabIndex = 6;
            // 
            // rdbConvertText
            // 
            this.rdbConvertText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbConvertText.AutoSize = true;
            this.rdbConvertText.Checked = true;
            this.rdbConvertText.Location = new System.Drawing.Point(3, 4);
            this.rdbConvertText.Name = "rdbConvertText";
            this.rdbConvertText.Size = new System.Drawing.Size(86, 17);
            this.rdbConvertText.TabIndex = 1;
            this.rdbConvertText.TabStop = true;
            this.rdbConvertText.Text = "Convert Text";
            this.rdbConvertText.UseVisualStyleBackColor = true;
            this.rdbConvertText.CheckedChanged += new System.EventHandler(this.rdbConvertText_CheckedChanged);
            // 
            // rdbConvertFile
            // 
            this.rdbConvertFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbConvertFile.AutoSize = true;
            this.rdbConvertFile.Location = new System.Drawing.Point(95, 4);
            this.rdbConvertFile.Name = "rdbConvertFile";
            this.rdbConvertFile.Size = new System.Drawing.Size(81, 17);
            this.rdbConvertFile.TabIndex = 0;
            this.rdbConvertFile.Text = "Convert File";
            this.rdbConvertFile.UseVisualStyleBackColor = true;
            this.rdbConvertFile.CheckedChanged += new System.EventHandler(this.rdbConvertFile_CheckedChanged);
            // 
            // pnlConvertText
            // 
            this.pnlConvertText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConvertText.Controls.Add(this.txtTextOutput);
            this.pnlConvertText.Controls.Add(this.txtTextInput);
            this.pnlConvertText.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.pnlConvertText.Location = new System.Drawing.Point(3, 40);
            this.pnlConvertText.Name = "pnlConvertText";
            this.pnlConvertText.Size = new System.Drawing.Size(751, 534);
            this.pnlConvertText.TabIndex = 5;
            // 
            // txtTextOutput
            // 
            this.txtTextOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtTextOutput.Location = new System.Drawing.Point(383, 3);
            this.txtTextOutput.Multiline = true;
            this.txtTextOutput.Name = "txtTextOutput";
            this.txtTextOutput.ReadOnly = true;
            this.txtTextOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTextOutput.Size = new System.Drawing.Size(365, 528);
            this.txtTextOutput.TabIndex = 2;
            this.txtTextOutput.WordWrap = false;
            this.txtTextOutput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textFieldSelectAll);
            // 
            // txtTextInput
            // 
            this.txtTextInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtTextInput.Location = new System.Drawing.Point(3, 3);
            this.txtTextInput.Multiline = true;
            this.txtTextInput.Name = "txtTextInput";
            this.txtTextInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTextInput.Size = new System.Drawing.Size(365, 528);
            this.txtTextInput.TabIndex = 1;
            this.txtTextInput.WordWrap = false;
            this.txtTextInput.TextChanged += new System.EventHandler(this.txtTextInput_TextChanged);
            this.txtTextInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textFieldSelectAll);
            // 
            // tabPointerSearcher
            // 
            this.tabPointerSearcher.Controls.Add(this.txtBaseAddress);
            this.tabPointerSearcher.Controls.Add(this.lblBaseAddress);
            this.tabPointerSearcher.Controls.Add(this.comboPointerSearcherMode);
            this.tabPointerSearcher.Controls.Add(this.pnlPointerSearcherCodeType);
            this.tabPointerSearcher.Controls.Add(this.pnlPointerSearcherBitType);
            this.tabPointerSearcher.Controls.Add(this.chkPointerSearcherRealAddresses);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherCode);
            this.tabPointerSearcher.Controls.Add(this.chkPointerSearcherOptimizePointerPaths);
            this.tabPointerSearcher.Controls.Add(this.chkPointerSearcherRAWCode);
            this.tabPointerSearcher.Controls.Add(this.chkPointerSearcherIncludeNegatives);
            this.tabPointerSearcher.Controls.Add(this.btnPointerSearcherClear);
            this.tabPointerSearcher.Controls.Add(this.btnPointerSearcherFindPointers);
            this.tabPointerSearcher.Controls.Add(this.treePointerSearcherPointers);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherValue);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherMaxOffset);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherValue);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherMaxOffset);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherMemDump2);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherMemDump1);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherMode);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherAddress2);
            this.tabPointerSearcher.Controls.Add(this.lblPointerSearcherAddress1);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherMemDump2);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherMemDump1);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherAddress2);
            this.tabPointerSearcher.Controls.Add(this.txtPointerSearcherAddress1);
            this.tabPointerSearcher.Location = new System.Drawing.Point(4, 22);
            this.tabPointerSearcher.Name = "tabPointerSearcher";
            this.tabPointerSearcher.Padding = new System.Windows.Forms.Padding(3);
            this.tabPointerSearcher.Size = new System.Drawing.Size(757, 574);
            this.tabPointerSearcher.TabIndex = 1;
            this.tabPointerSearcher.Text = "Pointer Searcher";
            this.tabPointerSearcher.UseVisualStyleBackColor = true;
            // 
            // txtBaseAddress
            // 
            this.txtBaseAddress.Location = new System.Drawing.Point(97, 141);
            this.txtBaseAddress.MaxLength = 10;
            this.txtBaseAddress.Name = "txtBaseAddress";
            this.txtBaseAddress.Size = new System.Drawing.Size(189, 20);
            this.txtBaseAddress.TabIndex = 11;
            this.txtBaseAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidateHexString_KeyPress);
            // 
            // lblBaseAddress
            // 
            this.lblBaseAddress.AutoSize = true;
            this.lblBaseAddress.Location = new System.Drawing.Point(6, 144);
            this.lblBaseAddress.Name = "lblBaseAddress";
            this.lblBaseAddress.Size = new System.Drawing.Size(75, 13);
            this.lblBaseAddress.TabIndex = 10;
            this.lblBaseAddress.Text = "Base Address:";
            // 
            // comboPointerSearcherMode
            // 
            this.comboPointerSearcherMode.DisplayMember = "1";
            this.comboPointerSearcherMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPointerSearcherMode.FormattingEnabled = true;
            this.comboPointerSearcherMode.Items.AddRange(new object[] {
            "Sony Vita",
            "Sony PSP",
            "Nintendo DS",
            "Other..."});
            this.comboPointerSearcherMode.Location = new System.Drawing.Point(97, 114);
            this.comboPointerSearcherMode.Name = "comboPointerSearcherMode";
            this.comboPointerSearcherMode.Size = new System.Drawing.Size(189, 21);
            this.comboPointerSearcherMode.TabIndex = 9;
            this.comboPointerSearcherMode.SelectedIndexChanged += new System.EventHandler(this.comboPointerSearcherMode_SelectedIndexChanged);
            // 
            // pnlPointerSearcherCodeType
            // 
            this.pnlPointerSearcherCodeType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPointerSearcherCodeType.Controls.Add(this.lblPntCodeTypes);
            this.pnlPointerSearcherCodeType.Controls.Add(this.cbPntCodeTypes);
            this.pnlPointerSearcherCodeType.Location = new System.Drawing.Point(9, 380);
            this.pnlPointerSearcherCodeType.Name = "pnlPointerSearcherCodeType";
            this.pnlPointerSearcherCodeType.Size = new System.Drawing.Size(279, 24);
            this.pnlPointerSearcherCodeType.TabIndex = 12;
            // 
            // lblPntCodeTypes
            // 
            this.lblPntCodeTypes.AutoSize = true;
            this.lblPntCodeTypes.Location = new System.Drawing.Point(17, 6);
            this.lblPntCodeTypes.Name = "lblPntCodeTypes";
            this.lblPntCodeTypes.Size = new System.Drawing.Size(62, 13);
            this.lblPntCodeTypes.TabIndex = 2;
            this.lblPntCodeTypes.Text = "Code Type:";
            // 
            // cbPntCodeTypes
            // 
            this.cbPntCodeTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPntCodeTypes.FormattingEnabled = true;
            this.cbPntCodeTypes.Location = new System.Drawing.Point(88, 3);
            this.cbPntCodeTypes.Name = "cbPntCodeTypes";
            this.cbPntCodeTypes.Size = new System.Drawing.Size(150, 21);
            this.cbPntCodeTypes.TabIndex = 1;
            // 
            // pnlPointerSearcherBitType
            // 
            this.pnlPointerSearcherBitType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPointerSearcherBitType.Controls.Add(this.rdbPointerSearcherBitType32);
            this.pnlPointerSearcherBitType.Controls.Add(this.rdbPointerSearcherBitType8);
            this.pnlPointerSearcherBitType.Controls.Add(this.rdbPointerSearcherBitType16);
            this.pnlPointerSearcherBitType.Location = new System.Drawing.Point(97, 219);
            this.pnlPointerSearcherBitType.Name = "pnlPointerSearcherBitType";
            this.pnlPointerSearcherBitType.Size = new System.Drawing.Size(189, 24);
            this.pnlPointerSearcherBitType.TabIndex = 16;
            // 
            // rdbPointerSearcherBitType32
            // 
            this.rdbPointerSearcherBitType32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbPointerSearcherBitType32.AutoSize = true;
            this.rdbPointerSearcherBitType32.Checked = true;
            this.rdbPointerSearcherBitType32.Location = new System.Drawing.Point(111, 3);
            this.rdbPointerSearcherBitType32.Name = "rdbPointerSearcherBitType32";
            this.rdbPointerSearcherBitType32.Size = new System.Drawing.Size(51, 17);
            this.rdbPointerSearcherBitType32.TabIndex = 0;
            this.rdbPointerSearcherBitType32.TabStop = true;
            this.rdbPointerSearcherBitType32.Text = "32-bit";
            this.rdbPointerSearcherBitType32.UseVisualStyleBackColor = true;
            // 
            // rdbPointerSearcherBitType8
            // 
            this.rdbPointerSearcherBitType8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbPointerSearcherBitType8.AutoSize = true;
            this.rdbPointerSearcherBitType8.Location = new System.Drawing.Point(3, 3);
            this.rdbPointerSearcherBitType8.Name = "rdbPointerSearcherBitType8";
            this.rdbPointerSearcherBitType8.Size = new System.Drawing.Size(45, 17);
            this.rdbPointerSearcherBitType8.TabIndex = 1;
            this.rdbPointerSearcherBitType8.Text = "8-bit";
            this.rdbPointerSearcherBitType8.UseVisualStyleBackColor = true;
            // 
            // rdbPointerSearcherBitType16
            // 
            this.rdbPointerSearcherBitType16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbPointerSearcherBitType16.AutoSize = true;
            this.rdbPointerSearcherBitType16.Location = new System.Drawing.Point(54, 3);
            this.rdbPointerSearcherBitType16.Name = "rdbPointerSearcherBitType16";
            this.rdbPointerSearcherBitType16.Size = new System.Drawing.Size(51, 17);
            this.rdbPointerSearcherBitType16.TabIndex = 2;
            this.rdbPointerSearcherBitType16.Text = "16-bit";
            this.rdbPointerSearcherBitType16.UseVisualStyleBackColor = true;
            // 
            // chkPointerSearcherRealAddresses
            // 
            this.chkPointerSearcherRealAddresses.AutoSize = true;
            this.chkPointerSearcherRealAddresses.Checked = true;
            this.chkPointerSearcherRealAddresses.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPointerSearcherRealAddresses.Location = new System.Drawing.Point(97, 318);
            this.chkPointerSearcherRealAddresses.Name = "chkPointerSearcherRealAddresses";
            this.chkPointerSearcherRealAddresses.Size = new System.Drawing.Size(100, 17);
            this.chkPointerSearcherRealAddresses.TabIndex = 20;
            this.chkPointerSearcherRealAddresses.Text = "Real Addresses";
            this.chkPointerSearcherRealAddresses.UseVisualStyleBackColor = true;
            // 
            // txtPointerSearcherCode
            // 
            this.txtPointerSearcherCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtPointerSearcherCode.Location = new System.Drawing.Point(9, 406);
            this.txtPointerSearcherCode.Multiline = true;
            this.txtPointerSearcherCode.Name = "txtPointerSearcherCode";
            this.txtPointerSearcherCode.ReadOnly = true;
            this.txtPointerSearcherCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPointerSearcherCode.Size = new System.Drawing.Size(279, 162);
            this.txtPointerSearcherCode.TabIndex = 25;
            this.txtPointerSearcherCode.WordWrap = false;
            // 
            // chkPointerSearcherOptimizePointerPaths
            // 
            this.chkPointerSearcherOptimizePointerPaths.AutoSize = true;
            this.chkPointerSearcherOptimizePointerPaths.Checked = true;
            this.chkPointerSearcherOptimizePointerPaths.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPointerSearcherOptimizePointerPaths.Location = new System.Drawing.Point(97, 249);
            this.chkPointerSearcherOptimizePointerPaths.Name = "chkPointerSearcherOptimizePointerPaths";
            this.chkPointerSearcherOptimizePointerPaths.Size = new System.Drawing.Size(191, 17);
            this.chkPointerSearcherOptimizePointerPaths.TabIndex = 17;
            this.chkPointerSearcherOptimizePointerPaths.Text = "Only Display Optimal  Pointer Paths";
            this.chkPointerSearcherOptimizePointerPaths.UseVisualStyleBackColor = true;
            // 
            // chkPointerSearcherRAWCode
            // 
            this.chkPointerSearcherRAWCode.AutoSize = true;
            this.chkPointerSearcherRAWCode.Location = new System.Drawing.Point(97, 272);
            this.chkPointerSearcherRAWCode.Name = "chkPointerSearcherRAWCode";
            this.chkPointerSearcherRAWCode.Size = new System.Drawing.Size(80, 17);
            this.chkPointerSearcherRAWCode.TabIndex = 18;
            this.chkPointerSearcherRAWCode.Text = "RAW Code";
            this.chkPointerSearcherRAWCode.UseVisualStyleBackColor = true;
            // 
            // chkPointerSearcherIncludeNegatives
            // 
            this.chkPointerSearcherIncludeNegatives.AutoSize = true;
            this.chkPointerSearcherIncludeNegatives.Location = new System.Drawing.Point(97, 295);
            this.chkPointerSearcherIncludeNegatives.Name = "chkPointerSearcherIncludeNegatives";
            this.chkPointerSearcherIncludeNegatives.Size = new System.Drawing.Size(112, 17);
            this.chkPointerSearcherIncludeNegatives.TabIndex = 19;
            this.chkPointerSearcherIncludeNegatives.Text = "Include Negatives";
            this.chkPointerSearcherIncludeNegatives.UseVisualStyleBackColor = true;
            // 
            // btnPointerSearcherClear
            // 
            this.btnPointerSearcherClear.Location = new System.Drawing.Point(97, 351);
            this.btnPointerSearcherClear.Name = "btnPointerSearcherClear";
            this.btnPointerSearcherClear.Size = new System.Drawing.Size(62, 23);
            this.btnPointerSearcherClear.TabIndex = 21;
            this.btnPointerSearcherClear.Text = "Clear";
            this.btnPointerSearcherClear.UseVisualStyleBackColor = true;
            this.btnPointerSearcherClear.Click += new System.EventHandler(this.btnPointerSearcherClear_Click);
            // 
            // btnPointerSearcherFindPointers
            // 
            this.btnPointerSearcherFindPointers.Location = new System.Drawing.Point(165, 351);
            this.btnPointerSearcherFindPointers.Name = "btnPointerSearcherFindPointers";
            this.btnPointerSearcherFindPointers.Size = new System.Drawing.Size(100, 23);
            this.btnPointerSearcherFindPointers.TabIndex = 22;
            this.btnPointerSearcherFindPointers.Text = "Find Pointers";
            this.btnPointerSearcherFindPointers.UseVisualStyleBackColor = true;
            this.btnPointerSearcherFindPointers.Click += new System.EventHandler(this.btnPointerSearcherFindPointers_Click);
            // 
            // treePointerSearcherPointers
            // 
            this.treePointerSearcherPointers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treePointerSearcherPointers.Location = new System.Drawing.Point(292, 6);
            this.treePointerSearcherPointers.Name = "treePointerSearcherPointers";
            this.treePointerSearcherPointers.Size = new System.Drawing.Size(462, 562);
            this.treePointerSearcherPointers.TabIndex = 23;
            this.treePointerSearcherPointers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treePointerSearcherPointers_AfterSelect);
            this.treePointerSearcherPointers.DoubleClick += new System.EventHandler(this.treePointerSearcherPointers_DoubleClick);
            this.treePointerSearcherPointers.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treePointerSearcherPointers_KeyUp);
            // 
            // lblPointerSearcherValue
            // 
            this.lblPointerSearcherValue.AutoSize = true;
            this.lblPointerSearcherValue.Location = new System.Drawing.Point(6, 196);
            this.lblPointerSearcherValue.Name = "lblPointerSearcherValue";
            this.lblPointerSearcherValue.Size = new System.Drawing.Size(76, 13);
            this.lblPointerSearcherValue.TabIndex = 14;
            this.lblPointerSearcherValue.Text = "Desired Value:";
            // 
            // lblPointerSearcherMaxOffset
            // 
            this.lblPointerSearcherMaxOffset.AutoSize = true;
            this.lblPointerSearcherMaxOffset.Location = new System.Drawing.Point(6, 170);
            this.lblPointerSearcherMaxOffset.Name = "lblPointerSearcherMaxOffset";
            this.lblPointerSearcherMaxOffset.Size = new System.Drawing.Size(85, 13);
            this.lblPointerSearcherMaxOffset.TabIndex = 12;
            this.lblPointerSearcherMaxOffset.Text = "Maximum Offset:";
            // 
            // txtPointerSearcherValue
            // 
            this.txtPointerSearcherValue.Location = new System.Drawing.Point(97, 193);
            this.txtPointerSearcherValue.MaxLength = 10;
            this.txtPointerSearcherValue.Name = "txtPointerSearcherValue";
            this.txtPointerSearcherValue.Size = new System.Drawing.Size(189, 20);
            this.txtPointerSearcherValue.TabIndex = 15;
            this.txtPointerSearcherValue.Text = "0x00000000";
            this.txtPointerSearcherValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidateHexString_KeyPress);
            // 
            // txtPointerSearcherMaxOffset
            // 
            this.txtPointerSearcherMaxOffset.Location = new System.Drawing.Point(97, 167);
            this.txtPointerSearcherMaxOffset.MaxLength = 10;
            this.txtPointerSearcherMaxOffset.Name = "txtPointerSearcherMaxOffset";
            this.txtPointerSearcherMaxOffset.Size = new System.Drawing.Size(189, 20);
            this.txtPointerSearcherMaxOffset.TabIndex = 13;
            this.txtPointerSearcherMaxOffset.Text = "0x1000";
            this.txtPointerSearcherMaxOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidateHexString_KeyPress);
            // 
            // lblPointerSearcherMemDump2
            // 
            this.lblPointerSearcherMemDump2.AutoSize = true;
            this.lblPointerSearcherMemDump2.Location = new System.Drawing.Point(6, 61);
            this.lblPointerSearcherMemDump2.Name = "lblPointerSearcherMemDump2";
            this.lblPointerSearcherMemDump2.Size = new System.Drawing.Size(78, 13);
            this.lblPointerSearcherMemDump2.TabIndex = 4;
            this.lblPointerSearcherMemDump2.Text = "Memory Dump:";
            // 
            // lblPointerSearcherMemDump1
            // 
            this.lblPointerSearcherMemDump1.AutoSize = true;
            this.lblPointerSearcherMemDump1.Location = new System.Drawing.Point(6, 9);
            this.lblPointerSearcherMemDump1.Name = "lblPointerSearcherMemDump1";
            this.lblPointerSearcherMemDump1.Size = new System.Drawing.Size(78, 13);
            this.lblPointerSearcherMemDump1.TabIndex = 0;
            this.lblPointerSearcherMemDump1.Text = "Memory Dump:";
            // 
            // lblPointerSearcherMode
            // 
            this.lblPointerSearcherMode.AutoSize = true;
            this.lblPointerSearcherMode.Location = new System.Drawing.Point(6, 117);
            this.lblPointerSearcherMode.Name = "lblPointerSearcherMode";
            this.lblPointerSearcherMode.Size = new System.Drawing.Size(37, 13);
            this.lblPointerSearcherMode.TabIndex = 8;
            this.lblPointerSearcherMode.Text = "Mode:";
            // 
            // lblPointerSearcherAddress2
            // 
            this.lblPointerSearcherAddress2.AutoSize = true;
            this.lblPointerSearcherAddress2.Location = new System.Drawing.Point(6, 87);
            this.lblPointerSearcherAddress2.Name = "lblPointerSearcherAddress2";
            this.lblPointerSearcherAddress2.Size = new System.Drawing.Size(48, 13);
            this.lblPointerSearcherAddress2.TabIndex = 6;
            this.lblPointerSearcherAddress2.Text = "Address:";
            // 
            // lblPointerSearcherAddress1
            // 
            this.lblPointerSearcherAddress1.AutoSize = true;
            this.lblPointerSearcherAddress1.Location = new System.Drawing.Point(6, 35);
            this.lblPointerSearcherAddress1.Name = "lblPointerSearcherAddress1";
            this.lblPointerSearcherAddress1.Size = new System.Drawing.Size(48, 13);
            this.lblPointerSearcherAddress1.TabIndex = 2;
            this.lblPointerSearcherAddress1.Text = "Address:";
            // 
            // txtPointerSearcherMemDump2
            // 
            this.txtPointerSearcherMemDump2.AllowDrop = true;
            this.txtPointerSearcherMemDump2.Location = new System.Drawing.Point(97, 58);
            this.txtPointerSearcherMemDump2.Name = "txtPointerSearcherMemDump2";
            this.txtPointerSearcherMemDump2.ReadOnly = true;
            this.txtPointerSearcherMemDump2.Size = new System.Drawing.Size(189, 20);
            this.txtPointerSearcherMemDump2.TabIndex = 5;
            this.txtPointerSearcherMemDump2.Click += new System.EventHandler(this.txtPointerSearcherMemDump2_Click);
            this.txtPointerSearcherMemDump2.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFileDragDrop_DragDrop);
            this.txtPointerSearcherMemDump2.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFileDragDrop_DragEnter);
            // 
            // txtPointerSearcherMemDump1
            // 
            this.txtPointerSearcherMemDump1.AllowDrop = true;
            this.txtPointerSearcherMemDump1.Location = new System.Drawing.Point(97, 6);
            this.txtPointerSearcherMemDump1.Name = "txtPointerSearcherMemDump1";
            this.txtPointerSearcherMemDump1.ReadOnly = true;
            this.txtPointerSearcherMemDump1.Size = new System.Drawing.Size(189, 20);
            this.txtPointerSearcherMemDump1.TabIndex = 1;
            this.txtPointerSearcherMemDump1.Click += new System.EventHandler(this.txtPointerSearcherMemDump1_Click);
            this.txtPointerSearcherMemDump1.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFileDragDrop_DragDrop);
            this.txtPointerSearcherMemDump1.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFileDragDrop_DragEnter);
            // 
            // txtPointerSearcherAddress2
            // 
            this.txtPointerSearcherAddress2.Location = new System.Drawing.Point(97, 84);
            this.txtPointerSearcherAddress2.MaxLength = 10;
            this.txtPointerSearcherAddress2.Name = "txtPointerSearcherAddress2";
            this.txtPointerSearcherAddress2.Size = new System.Drawing.Size(189, 20);
            this.txtPointerSearcherAddress2.TabIndex = 7;
            this.txtPointerSearcherAddress2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidateHexString_KeyPress);
            // 
            // txtPointerSearcherAddress1
            // 
            this.txtPointerSearcherAddress1.Location = new System.Drawing.Point(97, 32);
            this.txtPointerSearcherAddress1.MaxLength = 10;
            this.txtPointerSearcherAddress1.Name = "txtPointerSearcherAddress1";
            this.txtPointerSearcherAddress1.Size = new System.Drawing.Size(189, 20);
            this.txtPointerSearcherAddress1.TabIndex = 3;
            this.txtPointerSearcherAddress1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValidateHexString_KeyPress);
            // 
            // tabVitaCheat
            // 
            this.tabVitaCheat.Controls.Add(this.groupVitaCheatCompression);
            this.tabVitaCheat.Controls.Add(this.lblVitaCheatPointerLevel);
            this.tabVitaCheat.Controls.Add(this.comboVitaCheatPointerLevel);
            this.tabVitaCheat.Controls.Add(this.pnlVitaCheatBitType);
            this.tabVitaCheat.Controls.Add(this.groupVitaCheatAddress2Offset);
            this.tabVitaCheat.Controls.Add(this.groupVitaCheatAddress1Offset);
            this.tabVitaCheat.Controls.Add(this.btnVitaCheatGenerate);
            this.tabVitaCheat.Controls.Add(this.txtVitaCheatCode);
            this.tabVitaCheat.Controls.Add(this.lblVitaCheatValue);
            this.tabVitaCheat.Controls.Add(this.txtVitaCheatValue);
            this.tabVitaCheat.Controls.Add(this.lblVitaCheatAddress2);
            this.tabVitaCheat.Controls.Add(this.txtVitaCheatAddress2);
            this.tabVitaCheat.Controls.Add(this.lblVitaCheatAddress1);
            this.tabVitaCheat.Controls.Add(this.txtVitaCheatAddress1);
            this.tabVitaCheat.Controls.Add(this.lblVitaCheatCodeType);
            this.tabVitaCheat.Controls.Add(this.comboVitaCheatCodeType);
            this.tabVitaCheat.Location = new System.Drawing.Point(4, 22);
            this.tabVitaCheat.Name = "tabVitaCheat";
            this.tabVitaCheat.Padding = new System.Windows.Forms.Padding(3);
            this.tabVitaCheat.Size = new System.Drawing.Size(757, 574);
            this.tabVitaCheat.TabIndex = 2;
            this.tabVitaCheat.Text = "VitaCheat";
            this.tabVitaCheat.UseVisualStyleBackColor = true;
            // 
            // groupVitaCheatCompression
            // 
            this.groupVitaCheatCompression.Controls.Add(this.lblVitaCheatValueGap);
            this.groupVitaCheatCompression.Controls.Add(this.lblVitaCheatAddressGap);
            this.groupVitaCheatCompression.Controls.Add(this.lblVitaCheatCompressions);
            this.groupVitaCheatCompression.Controls.Add(this.txtVitaCheatValueGap);
            this.groupVitaCheatCompression.Controls.Add(this.txtVitaCheatAddressGap);
            this.groupVitaCheatCompression.Controls.Add(this.numericVitaCheatCompressions);
            this.groupVitaCheatCompression.Location = new System.Drawing.Point(459, 15);
            this.groupVitaCheatCompression.Name = "groupVitaCheatCompression";
            this.groupVitaCheatCompression.Size = new System.Drawing.Size(236, 98);
            this.groupVitaCheatCompression.TabIndex = 16;
            this.groupVitaCheatCompression.TabStop = false;
            this.groupVitaCheatCompression.Text = "Compression Options";
            // 
            // lblVitaCheatValueGap
            // 
            this.lblVitaCheatValueGap.AutoSize = true;
            this.lblVitaCheatValueGap.Location = new System.Drawing.Point(6, 72);
            this.lblVitaCheatValueGap.Name = "lblVitaCheatValueGap";
            this.lblVitaCheatValueGap.Size = new System.Drawing.Size(60, 13);
            this.lblVitaCheatValueGap.TabIndex = 5;
            this.lblVitaCheatValueGap.Text = "Value Gap:";
            // 
            // lblVitaCheatAddressGap
            // 
            this.lblVitaCheatAddressGap.AutoSize = true;
            this.lblVitaCheatAddressGap.Location = new System.Drawing.Point(6, 45);
            this.lblVitaCheatAddressGap.Name = "lblVitaCheatAddressGap";
            this.lblVitaCheatAddressGap.Size = new System.Drawing.Size(71, 13);
            this.lblVitaCheatAddressGap.TabIndex = 4;
            this.lblVitaCheatAddressGap.Text = "Address Gap:";
            // 
            // lblVitaCheatCompressions
            // 
            this.lblVitaCheatCompressions.AutoSize = true;
            this.lblVitaCheatCompressions.Location = new System.Drawing.Point(6, 22);
            this.lblVitaCheatCompressions.Name = "lblVitaCheatCompressions";
            this.lblVitaCheatCompressions.Size = new System.Drawing.Size(97, 13);
            this.lblVitaCheatCompressions.TabIndex = 3;
            this.lblVitaCheatCompressions.Text = "# of Compressions:";
            // 
            // txtVitaCheatValueGap
            // 
            this.txtVitaCheatValueGap.Location = new System.Drawing.Point(110, 71);
            this.txtVitaCheatValueGap.Name = "txtVitaCheatValueGap";
            this.txtVitaCheatValueGap.Size = new System.Drawing.Size(120, 20);
            this.txtVitaCheatValueGap.TabIndex = 2;
            this.txtVitaCheatValueGap.Text = "0x00000000";
            // 
            // txtVitaCheatAddressGap
            // 
            this.txtVitaCheatAddressGap.Location = new System.Drawing.Point(110, 45);
            this.txtVitaCheatAddressGap.Name = "txtVitaCheatAddressGap";
            this.txtVitaCheatAddressGap.Size = new System.Drawing.Size(120, 20);
            this.txtVitaCheatAddressGap.TabIndex = 1;
            this.txtVitaCheatAddressGap.Text = "0x00000000";
            // 
            // numericVitaCheatCompressions
            // 
            this.numericVitaCheatCompressions.Location = new System.Drawing.Point(110, 19);
            this.numericVitaCheatCompressions.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericVitaCheatCompressions.Name = "numericVitaCheatCompressions";
            this.numericVitaCheatCompressions.Size = new System.Drawing.Size(120, 20);
            this.numericVitaCheatCompressions.TabIndex = 0;
            // 
            // lblVitaCheatPointerLevel
            // 
            this.lblVitaCheatPointerLevel.AutoSize = true;
            this.lblVitaCheatPointerLevel.Location = new System.Drawing.Point(6, 116);
            this.lblVitaCheatPointerLevel.Name = "lblVitaCheatPointerLevel";
            this.lblVitaCheatPointerLevel.Size = new System.Drawing.Size(69, 13);
            this.lblVitaCheatPointerLevel.TabIndex = 15;
            this.lblVitaCheatPointerLevel.Text = "Pointer Level";
            // 
            // comboVitaCheatPointerLevel
            // 
            this.comboVitaCheatPointerLevel.DisplayMember = "0";
            this.comboVitaCheatPointerLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVitaCheatPointerLevel.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.comboVitaCheatPointerLevel.FormattingEnabled = true;
            this.comboVitaCheatPointerLevel.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.comboVitaCheatPointerLevel.Location = new System.Drawing.Point(90, 113);
            this.comboVitaCheatPointerLevel.MaxDropDownItems = 5;
            this.comboVitaCheatPointerLevel.Name = "comboVitaCheatPointerLevel";
            this.comboVitaCheatPointerLevel.Size = new System.Drawing.Size(121, 21);
            this.comboVitaCheatPointerLevel.TabIndex = 14;
            this.comboVitaCheatPointerLevel.SelectedIndexChanged += new System.EventHandler(this.ComboVitaCheatPointerLevel_SelectedIndexChanged);
            // 
            // pnlVitaCheatBitType
            // 
            this.pnlVitaCheatBitType.Controls.Add(this.rdbVitaCheatBitType32Bit);
            this.pnlVitaCheatBitType.Controls.Add(this.rdbVitaCheatBitType16Bit);
            this.pnlVitaCheatBitType.Controls.Add(this.rdbVitaCheatBitType8Bit);
            this.pnlVitaCheatBitType.Location = new System.Drawing.Point(11, 140);
            this.pnlVitaCheatBitType.Name = "pnlVitaCheatBitType";
            this.pnlVitaCheatBitType.Size = new System.Drawing.Size(200, 20);
            this.pnlVitaCheatBitType.TabIndex = 13;
            // 
            // rdbVitaCheatBitType32Bit
            // 
            this.rdbVitaCheatBitType32Bit.AutoSize = true;
            this.rdbVitaCheatBitType32Bit.Checked = true;
            this.rdbVitaCheatBitType32Bit.Location = new System.Drawing.Point(116, 0);
            this.rdbVitaCheatBitType32Bit.Name = "rdbVitaCheatBitType32Bit";
            this.rdbVitaCheatBitType32Bit.Size = new System.Drawing.Size(52, 17);
            this.rdbVitaCheatBitType32Bit.TabIndex = 2;
            this.rdbVitaCheatBitType32Bit.TabStop = true;
            this.rdbVitaCheatBitType32Bit.Text = "32-Bit";
            this.rdbVitaCheatBitType32Bit.UseVisualStyleBackColor = true;
            // 
            // rdbVitaCheatBitType16Bit
            // 
            this.rdbVitaCheatBitType16Bit.AutoSize = true;
            this.rdbVitaCheatBitType16Bit.Location = new System.Drawing.Point(57, 0);
            this.rdbVitaCheatBitType16Bit.Name = "rdbVitaCheatBitType16Bit";
            this.rdbVitaCheatBitType16Bit.Size = new System.Drawing.Size(52, 17);
            this.rdbVitaCheatBitType16Bit.TabIndex = 1;
            this.rdbVitaCheatBitType16Bit.Text = "16-Bit";
            this.rdbVitaCheatBitType16Bit.UseVisualStyleBackColor = true;
            // 
            // rdbVitaCheatBitType8Bit
            // 
            this.rdbVitaCheatBitType8Bit.AutoSize = true;
            this.rdbVitaCheatBitType8Bit.Location = new System.Drawing.Point(4, 0);
            this.rdbVitaCheatBitType8Bit.Name = "rdbVitaCheatBitType8Bit";
            this.rdbVitaCheatBitType8Bit.Size = new System.Drawing.Size(46, 17);
            this.rdbVitaCheatBitType8Bit.TabIndex = 0;
            this.rdbVitaCheatBitType8Bit.Text = "8-Bit";
            this.rdbVitaCheatBitType8Bit.UseVisualStyleBackColor = true;
            // 
            // groupVitaCheatAddress2Offset
            // 
            this.groupVitaCheatAddress2Offset.Controls.Add(this.txtVitaCheatAddress2Offset5);
            this.groupVitaCheatAddress2Offset.Controls.Add(this.txtVitaCheatAddress2Offset4);
            this.groupVitaCheatAddress2Offset.Controls.Add(this.txtVitaCheatAddress2Offset3);
            this.groupVitaCheatAddress2Offset.Controls.Add(this.txtVitaCheatAddress2Offset2);
            this.groupVitaCheatAddress2Offset.Controls.Add(this.txtVitaCheatAddress2Offset1);
            this.groupVitaCheatAddress2Offset.Location = new System.Drawing.Point(338, 15);
            this.groupVitaCheatAddress2Offset.Name = "groupVitaCheatAddress2Offset";
            this.groupVitaCheatAddress2Offset.Size = new System.Drawing.Size(115, 150);
            this.groupVitaCheatAddress2Offset.TabIndex = 12;
            this.groupVitaCheatAddress2Offset.TabStop = false;
            this.groupVitaCheatAddress2Offset.Text = "Address 2 Offsets";
            // 
            // txtVitaCheatAddress2Offset5
            // 
            this.txtVitaCheatAddress2Offset5.Location = new System.Drawing.Point(7, 123);
            this.txtVitaCheatAddress2Offset5.Name = "txtVitaCheatAddress2Offset5";
            this.txtVitaCheatAddress2Offset5.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress2Offset5.TabIndex = 4;
            this.txtVitaCheatAddress2Offset5.Text = "0x00000000";
            // 
            // txtVitaCheatAddress2Offset4
            // 
            this.txtVitaCheatAddress2Offset4.Location = new System.Drawing.Point(7, 97);
            this.txtVitaCheatAddress2Offset4.Name = "txtVitaCheatAddress2Offset4";
            this.txtVitaCheatAddress2Offset4.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress2Offset4.TabIndex = 3;
            this.txtVitaCheatAddress2Offset4.Text = "0x00000000";
            // 
            // txtVitaCheatAddress2Offset3
            // 
            this.txtVitaCheatAddress2Offset3.Location = new System.Drawing.Point(7, 71);
            this.txtVitaCheatAddress2Offset3.Name = "txtVitaCheatAddress2Offset3";
            this.txtVitaCheatAddress2Offset3.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress2Offset3.TabIndex = 2;
            this.txtVitaCheatAddress2Offset3.Text = "0x00000000";
            // 
            // txtVitaCheatAddress2Offset2
            // 
            this.txtVitaCheatAddress2Offset2.Location = new System.Drawing.Point(7, 45);
            this.txtVitaCheatAddress2Offset2.Name = "txtVitaCheatAddress2Offset2";
            this.txtVitaCheatAddress2Offset2.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress2Offset2.TabIndex = 1;
            this.txtVitaCheatAddress2Offset2.Text = "0x00000000";
            // 
            // txtVitaCheatAddress2Offset1
            // 
            this.txtVitaCheatAddress2Offset1.Location = new System.Drawing.Point(7, 19);
            this.txtVitaCheatAddress2Offset1.Name = "txtVitaCheatAddress2Offset1";
            this.txtVitaCheatAddress2Offset1.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress2Offset1.TabIndex = 0;
            this.txtVitaCheatAddress2Offset1.Text = "0x00000000";
            // 
            // groupVitaCheatAddress1Offset
            // 
            this.groupVitaCheatAddress1Offset.Controls.Add(this.txtVitaCheatAddress1Offset5);
            this.groupVitaCheatAddress1Offset.Controls.Add(this.txtVitaCheatAddress1Offset4);
            this.groupVitaCheatAddress1Offset.Controls.Add(this.txtVitaCheatAddress1Offset3);
            this.groupVitaCheatAddress1Offset.Controls.Add(this.txtVitaCheatAddress1Offset2);
            this.groupVitaCheatAddress1Offset.Controls.Add(this.txtVitaCheatAddress1Offset1);
            this.groupVitaCheatAddress1Offset.Location = new System.Drawing.Point(217, 15);
            this.groupVitaCheatAddress1Offset.Name = "groupVitaCheatAddress1Offset";
            this.groupVitaCheatAddress1Offset.Size = new System.Drawing.Size(115, 150);
            this.groupVitaCheatAddress1Offset.TabIndex = 11;
            this.groupVitaCheatAddress1Offset.TabStop = false;
            this.groupVitaCheatAddress1Offset.Text = "Address 1 Offsets";
            // 
            // txtVitaCheatAddress1Offset5
            // 
            this.txtVitaCheatAddress1Offset5.Location = new System.Drawing.Point(7, 123);
            this.txtVitaCheatAddress1Offset5.Name = "txtVitaCheatAddress1Offset5";
            this.txtVitaCheatAddress1Offset5.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress1Offset5.TabIndex = 4;
            this.txtVitaCheatAddress1Offset5.Text = "0x00000000";
            // 
            // txtVitaCheatAddress1Offset4
            // 
            this.txtVitaCheatAddress1Offset4.Location = new System.Drawing.Point(7, 97);
            this.txtVitaCheatAddress1Offset4.Name = "txtVitaCheatAddress1Offset4";
            this.txtVitaCheatAddress1Offset4.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress1Offset4.TabIndex = 3;
            this.txtVitaCheatAddress1Offset4.Text = "0x00000000";
            // 
            // txtVitaCheatAddress1Offset3
            // 
            this.txtVitaCheatAddress1Offset3.Location = new System.Drawing.Point(7, 71);
            this.txtVitaCheatAddress1Offset3.Name = "txtVitaCheatAddress1Offset3";
            this.txtVitaCheatAddress1Offset3.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress1Offset3.TabIndex = 2;
            this.txtVitaCheatAddress1Offset3.Text = "0x00000000";
            // 
            // txtVitaCheatAddress1Offset2
            // 
            this.txtVitaCheatAddress1Offset2.Location = new System.Drawing.Point(7, 45);
            this.txtVitaCheatAddress1Offset2.Name = "txtVitaCheatAddress1Offset2";
            this.txtVitaCheatAddress1Offset2.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress1Offset2.TabIndex = 1;
            this.txtVitaCheatAddress1Offset2.Text = "0x00000000";
            // 
            // txtVitaCheatAddress1Offset1
            // 
            this.txtVitaCheatAddress1Offset1.Location = new System.Drawing.Point(7, 19);
            this.txtVitaCheatAddress1Offset1.Name = "txtVitaCheatAddress1Offset1";
            this.txtVitaCheatAddress1Offset1.Size = new System.Drawing.Size(100, 20);
            this.txtVitaCheatAddress1Offset1.TabIndex = 0;
            this.txtVitaCheatAddress1Offset1.Text = "0x00000000";
            // 
            // btnVitaCheatGenerate
            // 
            this.btnVitaCheatGenerate.Location = new System.Drawing.Point(213, 377);
            this.btnVitaCheatGenerate.Name = "btnVitaCheatGenerate";
            this.btnVitaCheatGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnVitaCheatGenerate.TabIndex = 10;
            this.btnVitaCheatGenerate.Text = "Generate!";
            this.btnVitaCheatGenerate.UseVisualStyleBackColor = true;
            this.btnVitaCheatGenerate.Click += new System.EventHandler(this.BtnVitaCheatGenerate_Click);
            // 
            // txtVitaCheatCode
            // 
            this.txtVitaCheatCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtVitaCheatCode.Location = new System.Drawing.Point(9, 406);
            this.txtVitaCheatCode.Multiline = true;
            this.txtVitaCheatCode.Name = "txtVitaCheatCode";
            this.txtVitaCheatCode.ReadOnly = true;
            this.txtVitaCheatCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtVitaCheatCode.Size = new System.Drawing.Size(279, 162);
            this.txtVitaCheatCode.TabIndex = 9;
            this.txtVitaCheatCode.WordWrap = false;
            // 
            // lblVitaCheatValue
            // 
            this.lblVitaCheatValue.AutoSize = true;
            this.lblVitaCheatValue.Location = new System.Drawing.Point(6, 89);
            this.lblVitaCheatValue.Name = "lblVitaCheatValue";
            this.lblVitaCheatValue.Size = new System.Drawing.Size(76, 13);
            this.lblVitaCheatValue.TabIndex = 8;
            this.lblVitaCheatValue.Text = "Desired Value:";
            // 
            // txtVitaCheatValue
            // 
            this.txtVitaCheatValue.Location = new System.Drawing.Point(90, 86);
            this.txtVitaCheatValue.Name = "txtVitaCheatValue";
            this.txtVitaCheatValue.Size = new System.Drawing.Size(121, 20);
            this.txtVitaCheatValue.TabIndex = 7;
            this.txtVitaCheatValue.Text = "99";
            // 
            // lblVitaCheatAddress2
            // 
            this.lblVitaCheatAddress2.AutoSize = true;
            this.lblVitaCheatAddress2.Location = new System.Drawing.Point(6, 63);
            this.lblVitaCheatAddress2.Name = "lblVitaCheatAddress2";
            this.lblVitaCheatAddress2.Size = new System.Drawing.Size(60, 13);
            this.lblVitaCheatAddress2.TabIndex = 6;
            this.lblVitaCheatAddress2.Text = "Copy From:";
            // 
            // txtVitaCheatAddress2
            // 
            this.txtVitaCheatAddress2.Location = new System.Drawing.Point(90, 60);
            this.txtVitaCheatAddress2.Name = "txtVitaCheatAddress2";
            this.txtVitaCheatAddress2.Size = new System.Drawing.Size(121, 20);
            this.txtVitaCheatAddress2.TabIndex = 5;
            this.txtVitaCheatAddress2.Text = "0x00000000";
            // 
            // lblVitaCheatAddress1
            // 
            this.lblVitaCheatAddress1.AutoSize = true;
            this.lblVitaCheatAddress1.Location = new System.Drawing.Point(6, 37);
            this.lblVitaCheatAddress1.Name = "lblVitaCheatAddress1";
            this.lblVitaCheatAddress1.Size = new System.Drawing.Size(57, 13);
            this.lblVitaCheatAddress1.TabIndex = 4;
            this.lblVitaCheatAddress1.Text = "Address 1:";
            // 
            // txtVitaCheatAddress1
            // 
            this.txtVitaCheatAddress1.Location = new System.Drawing.Point(90, 34);
            this.txtVitaCheatAddress1.Name = "txtVitaCheatAddress1";
            this.txtVitaCheatAddress1.Size = new System.Drawing.Size(121, 20);
            this.txtVitaCheatAddress1.TabIndex = 3;
            this.txtVitaCheatAddress1.Text = "0x00000000";
            // 
            // lblVitaCheatCodeType
            // 
            this.lblVitaCheatCodeType.AutoSize = true;
            this.lblVitaCheatCodeType.Location = new System.Drawing.Point(6, 10);
            this.lblVitaCheatCodeType.Name = "lblVitaCheatCodeType";
            this.lblVitaCheatCodeType.Size = new System.Drawing.Size(62, 13);
            this.lblVitaCheatCodeType.TabIndex = 2;
            this.lblVitaCheatCodeType.Text = "Code Type:";
            // 
            // comboVitaCheatCodeType
            // 
            this.comboVitaCheatCodeType.DisplayMember = "0";
            this.comboVitaCheatCodeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVitaCheatCodeType.FormattingEnabled = true;
            this.comboVitaCheatCodeType.Location = new System.Drawing.Point(90, 7);
            this.comboVitaCheatCodeType.Name = "comboVitaCheatCodeType";
            this.comboVitaCheatCodeType.Size = new System.Drawing.Size(121, 21);
            this.comboVitaCheatCodeType.TabIndex = 1;
            this.comboVitaCheatCodeType.SelectedIndexChanged += new System.EventHandler(this.ComboVitaCheatCodeType_SelectedIndexChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(769, 661);
            this.Controls.Add(this.tctrlTabs);
            this.Controls.Add(this.frmStatusStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TempAR - Vita Edition  V2.01";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnlConvertFormat.ResumeLayout(false);
            this.pnlConvertFormat.PerformLayout();
            this.pnlConvertFile.ResumeLayout(false);
            this.pnlConvertFile.PerformLayout();
            this.frmStatusStrip.ResumeLayout(false);
            this.frmStatusStrip.PerformLayout();
            this.tctrlTabs.ResumeLayout(false);
            this.tabConverter.ResumeLayout(false);
            this.pnlConvertType.ResumeLayout(false);
            this.pnlConvertType.PerformLayout();
            this.pnlConvertText.ResumeLayout(false);
            this.pnlConvertText.PerformLayout();
            this.tabPointerSearcher.ResumeLayout(false);
            this.tabPointerSearcher.PerformLayout();
            this.pnlPointerSearcherCodeType.ResumeLayout(false);
            this.pnlPointerSearcherCodeType.PerformLayout();
            this.pnlPointerSearcherBitType.ResumeLayout(false);
            this.pnlPointerSearcherBitType.PerformLayout();
            this.tabVitaCheat.ResumeLayout(false);
            this.tabVitaCheat.PerformLayout();
            this.groupVitaCheatCompression.ResumeLayout(false);
            this.groupVitaCheatCompression.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericVitaCheatCompressions)).EndInit();
            this.pnlVitaCheatBitType.ResumeLayout(false);
            this.pnlVitaCheatBitType.PerformLayout();
            this.groupVitaCheatAddress2Offset.ResumeLayout(false);
            this.groupVitaCheatAddress2Offset.PerformLayout();
            this.groupVitaCheatAddress1Offset.ResumeLayout(false);
            this.groupVitaCheatAddress1Offset.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void cbCodeTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox) sender;

            if(!String.IsNullOrEmpty(comboBox.Text))
            {
                this.btnConvert.Enabled = true;
                this.btnConvert_Click(sender, e);
            }
        }

        private void ComboVitaCheatCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtVitaCheatAddress2.Enabled = false;
            this.txtVitaCheatValue.Enabled = true;
            this.comboVitaCheatPointerLevel.Enabled = false;
            this.groupVitaCheatAddress1Offset.Enabled = false;
            this.groupVitaCheatAddress2Offset.Enabled = false;
            this.groupVitaCheatCompression.Enabled = false;
            switch (this.comboVitaCheatCodeType.Text)
            {
                case VC_GEN_WRITE: // Write
                    break;
                case VC_GEN_PNTR: // Pointer
                    this.comboVitaCheatPointerLevel.Enabled = true;
                    this.groupVitaCheatAddress1Offset.Enabled = true;
                    break;
                case VC_GEN_COMP: // Compress
                    this.groupVitaCheatCompression.Enabled = true;
                    break;
                case VC_GEN_MOV: // MOV
                    this.txtVitaCheatAddress2.Enabled = true;
                    this.txtVitaCheatValue.Enabled = false;
                    break;
                case VC_GEN_PTRCOM: // Pointer Compress
                    this.comboVitaCheatPointerLevel.Enabled = true;
                    this.groupVitaCheatAddress1Offset.Enabled = true;
                    this.groupVitaCheatCompression.Enabled = true;
                    break;
                case VC_GEN_PTRMOV: // Pointer MOV
                    this.comboVitaCheatPointerLevel.Enabled = true;
                    this.groupVitaCheatAddress1Offset.Enabled = true;
                    this.groupVitaCheatAddress2Offset.Enabled = true;
                    this.txtVitaCheatValue.Enabled = false;
                    this.txtVitaCheatAddress2.Enabled = true;
                    break;
            }
        }

        private void ComboVitaCheatPointerLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtVitaCheatAddress1Offset2.Enabled = false;
            this.txtVitaCheatAddress2Offset2.Enabled = false;
            this.txtVitaCheatAddress1Offset3.Enabled = false;
            this.txtVitaCheatAddress2Offset3.Enabled = false;
            this.txtVitaCheatAddress1Offset4.Enabled = false;
            this.txtVitaCheatAddress2Offset4.Enabled = false;
            this.txtVitaCheatAddress1Offset5.Enabled = false;
            this.txtVitaCheatAddress2Offset5.Enabled = false;
            if (this.comboVitaCheatPointerLevel.SelectedIndex >= 1)
            {

                this.txtVitaCheatAddress1Offset2.Enabled = true;
                this.txtVitaCheatAddress2Offset2.Enabled = true;
            }
            if (this.comboVitaCheatPointerLevel.SelectedIndex >= 2)
            {

                this.txtVitaCheatAddress1Offset3.Enabled = true;
                this.txtVitaCheatAddress2Offset3.Enabled = true;
            }
            if (this.comboVitaCheatPointerLevel.SelectedIndex >= 3)
            {

                this.txtVitaCheatAddress1Offset4.Enabled = true;
                this.txtVitaCheatAddress2Offset4.Enabled = true;
            }
            if (this.comboVitaCheatPointerLevel.SelectedIndex >= 4)
            {

                this.txtVitaCheatAddress1Offset5.Enabled = true;
                this.txtVitaCheatAddress2Offset5.Enabled = true;
            }

        }
        private void BtnVitaCheatGenerate_Click(object sender, EventArgs e)
        {
            //
            //Check for hex numbers and give error if bad
            //
            uint VCadd1;
            try
            {
                VCadd1 = this.parseNum(this.txtVitaCheatAddress1.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse Address 1, make sure value is a valid hexadecimal number.");
                return;
            }
            uint VCadd2;
            try
            {
                VCadd2 = this.parseNum(this.txtVitaCheatAddress2.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse Address 2, make sure value is a valid hexadecimal number.");
                return;
            }
            uint VCaddgap;
            try
            {
                VCaddgap = this.parseNum(this.txtVitaCheatAddressGap.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse Address Gap, make sure value is a valid hexadecimal number.");
                return;
            }
            uint VCvalgap;
            try
            {
                VCvalgap = this.parseNum(this.txtVitaCheatValueGap.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse Value Gap, make sure value is a valid hexadecimal number.");
                return;
            }
            uint VCcomps;
            try
            {
                VCcomps = this.parseNum(this.numericVitaCheatCompressions.Text, NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                int num2 = (int)MessageBox.Show("Unable to parse Value Gap, make sure value is a valid hexadecimal number.");
                return;
            }
            foreach (Control x in this.groupVitaCheatAddress1Offset.Controls)
            {
                if (x is TextBox)
                {
                    if (((TextBox)x).Enabled == true)
                    {

                        try
                        {
                            uint VCgenptr2 = this.parseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                        }
                        catch
                        {
                            int num2 = (int)MessageBox.Show("Unable to parse Address 1's Offsets, make sure values are valid hexadecimal numbers.");
                            return;
                        }
                    }
                }
            }
            foreach (Control x in this.groupVitaCheatAddress2Offset.Controls)
            {
                if (x is TextBox)
                {
                    if (((TextBox)x).Enabled == true)
                    {

                        try
                        {
                            uint VCgenptr2 = this.parseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                        }
                        catch
                        {
                            int num2 = (int)MessageBox.Show("Unable to parse Address 2's Offsets, make sure values are valid hexadecimal numbers.");
                            return;
                        }
                    }
                }
            }
            string VCstr1 = "_V0 Generated Code\r\n\r\n";
            uint VCAddr1 = this.parseNum(this.txtVitaCheatAddress1.Text, NumberStyles.AllowHexSpecifier);
            uint VCAddr2 = this.parseNum(this.txtVitaCheatAddress2.Text, NumberStyles.AllowHexSpecifier);
            uint VCAddGp = this.parseNum(this.txtVitaCheatAddressGap.Text, NumberStyles.AllowHexSpecifier);
            uint VCValGp = this.parseNum(this.txtVitaCheatValueGap.Text, NumberStyles.AllowHexSpecifier);
            uint VCComps = this.parseNum(this.numericVitaCheatCompressions.Text);
            uint VCValue = this.parseNum(this.txtVitaCheatValue.Text);
            //
            //Get Bit Type from radio buttons
            //
            int bittype = 2;
            if (this.rdbVitaCheatBitType8Bit.Checked)
            {
                bittype = 0;
            }
            else if (this.rdbVitaCheatBitType16Bit.Checked)
            {
                bittype = 1;
            }
            else { bittype = 2; }
            //
            // Generate code
            //
            switch (this.comboVitaCheatCodeType.Text)
            {
                    case VC_GEN_WRITE:
                string VCGenWrite1 = string.Format("$0{0}00 {1:X08} {2:X08}\r\n", bittype, VCAddr1, VCValue);
                this.txtVitaCheatCode.Text = VCstr1 + VCGenWrite1;
                break;
                    case VC_GEN_PNTR:
                string VCGenPtrstr2 = "";
                uint VCGenptroff1 = this.parseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                string VCGenPtr1 = string.Format("$3{0}0{1} {2:X08} {3:X08}\r\n", bittype, comboVitaCheatPointerLevel.Text, VCAddr1, VCGenptroff1);
                string VCGenPtr3 = string.Format("$3300 00000000 {0:X08}", VCValue);
                foreach (Control x in this.groupVitaCheatAddress1Offset.Controls)
                {
                    if (x is TextBox)
                    {
                        if (((TextBox)x).Enabled == true)
                        {
                                uint VCGenptr2 = this.parseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                                if (((TextBox)x).TabIndex != 0)
                                {
                                    VCGenPtrstr2 = string.Format("$3{0}00 00000000 {1:X08}\r\n", bittype, VCGenptr2) + VCGenPtrstr2;
                                }
                        }
                    }
                }
                this.txtVitaCheatCode.Text = VCstr1 + VCGenPtr1 + VCGenPtrstr2 + VCGenPtr3;
                break;
                    case VC_GEN_COMP:
                string VCGenComp1 = string.Format("$4{0}00 {1:X08} {2:X08}\r\n", bittype, VCAddr1, VCValue);
                string VCGenComp2 = string.Format("${0:X04} {1:X08} {2:X08}\r\n", VCComps, VCAddGp, VCValGp);
                this.txtVitaCheatCode.Text = VCstr1 + VCGenComp1 + VCGenComp2;
                break;
                    case VC_GEN_MOV:
                string VCGenMov1 = string.Format("$5{0}00 {1:X08} {2:X08}\r\n", bittype, VCAddr1, VCAddr2);
                this.txtVitaCheatCode.Text = VCstr1 + VCGenMov1;
                break;
                    case VC_GEN_PTRCOM:
                string VCGenPtrComstr2 = "";
                uint VCGenptrcomoff1 = this.parseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                string VCGenPtrCom1 = string.Format("$7{0}0{1} {2:X08} {3:X08}\r\n", bittype, comboVitaCheatPointerLevel.Text, VCAddr1, VCGenptrcomoff1);
                string VCGenPtrCom3 = string.Format("$770{0} 00000000 {1:X08}\r\n", comboVitaCheatPointerLevel.Text, VCValue);
                    string VCGenPtrCom4 = string.Format("${0:X04} 0000{1:X04} 0000{2:X04}", VCComps, VCAddGp, VCValGp);
                foreach (Control x in this.groupVitaCheatAddress1Offset.Controls)
                {
                    if (x is TextBox)
                    {
                        if (((TextBox)x).Enabled == true)
                        {
                            uint VCGenptr2 = this.parseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                            if (((TextBox)x).TabIndex != 0)
                            {
                                VCGenPtrComstr2 = string.Format("$7{0}00 00000000 {1:X08}\r\n", bittype, VCGenptr2) + VCGenPtrComstr2;
                            }
                        }
                    }
                }
                this.txtVitaCheatCode.Text = VCstr1 + VCGenPtrCom1 + VCGenPtrComstr2 + VCGenPtrCom3 + VCGenPtrCom4;
                break;
                case VC_GEN_PTRMOV:
                string VCGenPtrMovstr2 = "";
                uint VCGenptrmovoff1 = this.parseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                string VCGenPtrMov1 = string.Format("$8{0}0{1} {2:X08} {3:X08}\r\n", bittype, comboVitaCheatPointerLevel.Text, VCAddr1, VCGenptrmovoff1);
                string VCGenPtrMov3 = string.Format("$8800 00000000 00000000\r\n");
                foreach (Control x in this.groupVitaCheatAddress1Offset.Controls)
                {
                    if (x is TextBox)
                    {
                        if (((TextBox)x).Enabled == true)
                        {
                            uint VCGenptrmov2 = this.parseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                            if (((TextBox)x).TabIndex != 0)
                            {
                                VCGenPtrMovstr2 = string.Format("$8{0}00 00000000 {1:X08}\r\n", bittype, VCGenptrmov2) + VCGenPtrMovstr2;
                            }
                        }
                    }
                }
                string VCGenPtr2str2 = "";
                uint VCGenptrmov2off1 = this.parseNum(txtVitaCheatAddress2Offset1.Text, NumberStyles.AllowHexSpecifier);
                string VCGenPtrMov21 = string.Format("$8{0}0{1} {2:X08} {3:X08}\r\n", bittype + 4, comboVitaCheatPointerLevel.Text, VCAddr2, VCGenptrmov2off1);
                string VCGenPtrMov23 = string.Format("$8900 00000000 00000000");
                foreach (Control x in this.groupVitaCheatAddress2Offset.Controls)
                {
                    if (x is TextBox)
                    {
                        if (((TextBox)x).Enabled == true)
                        {
                            uint VCGenptrmov22 = this.parseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                            if (((TextBox)x).TabIndex != 0)
                            {
                                VCGenPtr2str2 = string.Format("$3{0}00 00000000 {1:X08}\r\n", bittype + 4, VCGenptrmov22) + VCGenPtr2str2;
                            }
                        }
                    }
                }
                this.txtVitaCheatCode.Text = VCstr1 + VCGenPtrMov1 + VCGenPtrMovstr2 + VCGenPtrMov3 + VCGenPtrMov21 + VCGenPtr2str2 + VCGenPtrMov23;
                break;
            }

        }
    }
}
