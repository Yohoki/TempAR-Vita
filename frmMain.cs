
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
    private RadioButton rdbR4CCE;
    private RadioButton rdbNitePR;
    private Panel pnlConvertFile;
    private TextBox txtOutputPath;
    private TextBox txtInputPath;
    private Label lblOutputPath;
    private Label lblInputPath;
    private Button btnOutputBrowse;
    private Button btnInputBrowse;
    private StatusStrip frmStatusStrip;
    private ToolStripStatusLabel lblStatus;
    private RadioButton rdbTempAR;
    private RadioButton rdbCWCheatPOPS;
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
        // Radio Buttons for Code type
    private RadioButton rdbPointerSearcherCodeTypeVitaCheat;
    private RadioButton rdbPointerSearcherCodeTypeCWCheat;
    private RadioButton rdbPointerSearcherCodeTypeAR;
    private TextBox txtBaseAddress;
        private TabPage tabVitaCheat;
        private Label lblBaseAddress;

    public frmMain()
    {
      this.InitializeComponent();
    }

    private void rdbConvertText_CheckedChanged(object sender, EventArgs e)
    {
      if (!((RadioButton) sender).Checked)
        return;
      this.ChangeFrameMode(1);
    }

    private void rdbConvertFile_CheckedChanged(object sender, EventArgs e)
    {
      if (!((RadioButton) sender).Checked)
        return;
      this.ChangeFrameMode(2);
    }

    private void rdbCWCheatPOPS_CheckedChanged(object sender, EventArgs e)
    {
      this.btnConvert_Click(sender, e);
    }

    private void rdbNitePR_CheckedChanged(object sender, EventArgs e)
    {
      this.btnConvert_Click(sender, e);
    }

    private void rdbR4CCE_CheckedChanged(object sender, EventArgs e)
    {
      this.btnConvert_Click(sender, e);
    }

    private void txtTextInput_TextChanged(object sender, EventArgs e)
    {
      this.btnConvert_Click(sender, e);
    }

    private void btnConvert_Click(object sender, EventArgs e)
    {
      this.lblStatus.Text = "Working...";
      this.lblStatus.Visible = true;
      this.Refresh();
      if (this.rdbConvertText.Checked)
      {
        if (this.rdbCWCheatPOPS.Checked)
          this.txtTextOutput.Text = Converter.cwcpops_pspar(this.txtTextInput.Text);
        else if (this.rdbNitePR.Checked)
          this.txtTextOutput.Text = Converter.nitepr_pspar(this.txtTextInput.Text);
        else if (this.rdbR4CCE.Checked)
          this.txtTextOutput.Text = Converter.reformat_r4cce(this.txtTextInput.Text, true);
        else if (this.rdbTempAR.Checked)
          this.txtTextOutput.Text = Converter.reformat_tempar(this.txtTextInput.Text);
      }
      else if (this.rdbConvertFile.Checked && ((Control) sender).Name == "btnConvert" && (this.txtInputPath.Text.Length > 0 && this.txtOutputPath.Text.Length > 0))
      {
        if (this.rdbCWCheatPOPS.Checked)
        {
          if (File.Exists(this.txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(this.txtOutputPath.Text)))
            Converter.file_cwcpops_pspar(this.txtInputPath.Text, this.txtOutputPath.Text);
        }
        else if (this.rdbNitePR.Checked)
        {
          if (Directory.Exists(this.txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(this.txtOutputPath.Text)))
            Converter.file_nitepr_pspar(this.txtInputPath.Text, this.txtOutputPath.Text);
        }
        else if (this.rdbTempAR.Checked && File.Exists(this.txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(this.txtOutputPath.Text)))
          Converter.file_reformat_tempar(this.txtInputPath.Text, this.txtOutputPath.Text);
      }
      this.lblStatus.Visible = false;
    }

    private void btnInputBrowse_Click(object sender, EventArgs e)
    {
      if (this.rdbCWCheatPOPS.Checked || this.rdbTempAR.Checked)
      {
        this.txtInputPath.Text = this.OpenFile(this.txtInputPath.Text, "CWCheat Database File (*.db)|*.db", "Open");
      }
      else
      {
        if (!this.rdbNitePR.Checked)
          return;
        this.txtInputPath.Text = this.OpenDirectory(this.txtInputPath.Text, "Select your NitePR code file directory:");
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
        ((TextBoxBase) sender).SelectAll();
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
        this.rdbR4CCE.Enabled = true;
      }
      else
      {
        this.pnlConvertFile.BringToFront();
        this.rdbR4CCE.Enabled = false;
        if (!this.rdbR4CCE.Checked)
          return;
        this.rdbCWCheatPOPS.Checked = true;
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
      openFileDialog.FileName = defaultfile.Length > 0 ? defaultfile : (string) null;
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
      saveFileDialog.FileName = defaultfile.Length > 0 ? defaultfile : (string) null;
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
        int num2 = (int) MessageBox.Show("Unable to parse address, make sure value is a valid hexadecimal number.");
        return;
      }
      uint num3;
      try
      {
        num3 = this.parseNum(this.txtPointerSearcherMaxOffset.Text);
      }
      catch
      {
        int num2 = (int) MessageBox.Show("Unable to parse maximum offset, make sure value is a valid hexadecimal number.");
        return;
      }
      try
      {
        this.memory_start = this.parseNum(this.txtBaseAddress.Text);
      }
      catch
      {
        int num2 = (int) MessageBox.Show("Unable to parse base address, make sure value is a valid hexadecimal number.");
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
        int num2 = (int) MessageBox.Show("Unable to parse maximum offset, make sure value is a valid hexadecimal number.");
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
      PropertyInfo propInfo = typeof (T).GetProperty(fieldName);
      Comparison<T> comparison = (Comparison<T>) ((a, b) =>
      {
        object obj1 = asc ? propInfo.GetValue((object) a, (object[]) null) : propInfo.GetValue((object) b, (object[]) null);
        object obj2 = asc ? propInfo.GetValue((object) b, (object[]) null) : propInfo.GetValue((object) a, (object[]) null);
        if (!(obj1 is IComparable))
          return 0;
        return ((IComparable) obj1).CompareTo(obj2);
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
        num &= (uint) ushort.MaxValue;
      }
      else if (this.rdbPointerSearcherBitType8.Checked)
      {
        bittype = 0;
        num &= (uint) byte.MaxValue;
      }
      //
      // Check which code is being generated
      //
      if (this.rdbPointerSearcherCodeTypeVitaCheat.Checked == true)
      {
        this.txtPointerSearcherCode.Text = this.getVitaCheatPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
      }
      else if (this.rdbPointerSearcherCodeTypeCWCheat.Checked == true)
      {
        this.txtPointerSearcherCode.Text = this.getCWCheatPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
      }
      else if (this.rdbPointerSearcherCodeTypeAR.Checked == true)
      {
        this.txtPointerSearcherCode.Text = this.getARPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
      }
        }

    private void txtPointerSearcherMemDump1_Click(object sender, EventArgs e)
    {
      this.txtPointerSearcherMemDump1.Text = this.OpenFile(this.txtPointerSearcherMemDump1.Text, (string) null, "Open");
    }

    private void txtPointerSearcherMemDump2_Click(object sender, EventArgs e)
    {
      this.txtPointerSearcherMemDump2.Text = this.OpenFile(this.txtPointerSearcherMemDump2.Text, (string) null, "Open");
    }

    private void treePointerSearcherPointers_KeyUp(object sender, KeyEventArgs e)
    {
      switch (e.KeyData)
      {
        case Keys.Space:
          this.treePointerSearcherPointers_DoubleClick((object) null, (EventArgs) null);
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
            if ((int) pointerSearcherLogList[index1].Address == (int) pointers[index2].Address)
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
          if ((int) num == (int) address)
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
      this.txtBaseAddress.Text = string.Format("0x{0:X08}", (object) this.memory_start);
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
        str1 = index % 2 != 0 ? str1 + string.Format(" 0x{0:X01}{1:X07}\n", (object) (pointers[index].Negative ? 3 : 2), (object) pointers[index].Offset) : str1 + string.Format("{0}0x{1:X01}{2:X07}", this.chkPointerSearcherRAWCode.Checked ? (object) "" : (object) "_L ", (object) (pointers[index].Negative ? 3 : 2), (object) pointers[index].Offset);
      if (pointers.Count % 2 == 0)
        str1 += string.Format(" 0x00000000");
      string str2 = string.Format("{0}0x6{1:X07} 0x{2:X08}\n{0}0x000{3:X01}{4:X04} 0x{5:X08}\n", this.chkPointerSearcherRAWCode.Checked ? (object) "" : (object) "_L ", (object) (uint) ((int) pointers[0].Address - (int) this.memory_start), (object) value, (object) bittype, (object) pointers.Count, (object) pointers[pointers.Count - 1].Offset) + str1;
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
      ((Control) sender).Text = ((string[]) e.Data.GetData(DataFormats.FileDrop))[0];
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
            this.rdbTempAR = new System.Windows.Forms.RadioButton();
            this.rdbR4CCE = new System.Windows.Forms.RadioButton();
            this.rdbNitePR = new System.Windows.Forms.RadioButton();
            this.rdbCWCheatPOPS = new System.Windows.Forms.RadioButton();
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
            this.rdbPointerSearcherCodeTypeVitaCheat = new System.Windows.Forms.RadioButton();
            this.rdbPointerSearcherCodeTypeCWCheat = new System.Windows.Forms.RadioButton();
            this.rdbPointerSearcherCodeTypeAR = new System.Windows.Forms.RadioButton();
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
            this.SuspendLayout();
            // 
            // pnlConvertFormat
            // 
            this.pnlConvertFormat.Controls.Add(this.rdbTempAR);
            this.pnlConvertFormat.Controls.Add(this.rdbR4CCE);
            this.pnlConvertFormat.Controls.Add(this.rdbNitePR);
            this.pnlConvertFormat.Controls.Add(this.rdbCWCheatPOPS);
            this.pnlConvertFormat.Location = new System.Drawing.Point(6, 6);
            this.pnlConvertFormat.Name = "pnlConvertFormat";
            this.pnlConvertFormat.Size = new System.Drawing.Size(440, 24);
            this.pnlConvertFormat.TabIndex = 7;
            // 
            // rdbTempAR
            // 
            this.rdbTempAR.AutoSize = true;
            this.rdbTempAR.Location = new System.Drawing.Point(300, 4);
            this.rdbTempAR.Name = "rdbTempAR";
            this.rdbTempAR.Size = new System.Drawing.Size(117, 17);
            this.rdbTempAR.TabIndex = 2;
            this.rdbTempAR.Text = "TempAR to R4CCE";
            this.rdbTempAR.UseVisualStyleBackColor = true;
            this.rdbTempAR.Visible = false;
            this.rdbTempAR.CheckedChanged += new System.EventHandler(this.rdbR4CCE_CheckedChanged);
            // 
            // rdbR4CCE
            // 
            this.rdbR4CCE.AutoSize = true;
            this.rdbR4CCE.Location = new System.Drawing.Point(177, 4);
            this.rdbR4CCE.Name = "rdbR4CCE";
            this.rdbR4CCE.Size = new System.Drawing.Size(117, 17);
            this.rdbR4CCE.TabIndex = 2;
            this.rdbR4CCE.Text = "R4CCE to TempAR";
            this.rdbR4CCE.UseVisualStyleBackColor = true;
            this.rdbR4CCE.CheckedChanged += new System.EventHandler(this.rdbR4CCE_CheckedChanged);
            // 
            // rdbNitePR
            // 
            this.rdbNitePR.AutoSize = true;
            this.rdbNitePR.Location = new System.Drawing.Point(112, 4);
            this.rdbNitePR.Name = "rdbNitePR";
            this.rdbNitePR.Size = new System.Drawing.Size(59, 17);
            this.rdbNitePR.TabIndex = 1;
            this.rdbNitePR.Text = "NitePR";
            this.rdbNitePR.UseVisualStyleBackColor = true;
            this.rdbNitePR.CheckedChanged += new System.EventHandler(this.rdbNitePR_CheckedChanged);
            // 
            // rdbCWCheatPOPS
            // 
            this.rdbCWCheatPOPS.AutoSize = true;
            this.rdbCWCheatPOPS.Checked = true;
            this.rdbCWCheatPOPS.Location = new System.Drawing.Point(3, 4);
            this.rdbCWCheatPOPS.Name = "rdbCWCheatPOPS";
            this.rdbCWCheatPOPS.Size = new System.Drawing.Size(103, 17);
            this.rdbCWCheatPOPS.TabIndex = 0;
            this.rdbCWCheatPOPS.TabStop = true;
            this.rdbCWCheatPOPS.Text = "CWCheat POPS";
            this.rdbCWCheatPOPS.UseVisualStyleBackColor = true;
            this.rdbCWCheatPOPS.CheckedChanged += new System.EventHandler(this.rdbCWCheatPOPS_CheckedChanged);
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
            this.pnlConvertFile.Size = new System.Drawing.Size(751, 347);
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
            this.frmStatusStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.FrmStatusStrip_ItemClicked);
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
            this.pnlPointerSearcherCodeType.Controls.Add(this.rdbPointerSearcherCodeTypeVitaCheat);
            this.pnlPointerSearcherCodeType.Controls.Add(this.rdbPointerSearcherCodeTypeCWCheat);
            this.pnlPointerSearcherCodeType.Controls.Add(this.rdbPointerSearcherCodeTypeAR);
            this.pnlPointerSearcherCodeType.Location = new System.Drawing.Point(9, 380);
            this.pnlPointerSearcherCodeType.Name = "pnlPointerSearcherCodeType";
            this.pnlPointerSearcherCodeType.Size = new System.Drawing.Size(279, 24);
            this.pnlPointerSearcherCodeType.TabIndex = 24;
            // 
            // rdbPointerSearcherCodeTypeVitaCheat
            // 
            this.rdbPointerSearcherCodeTypeVitaCheat.AutoSize = true;
            this.rdbPointerSearcherCodeTypeVitaCheat.Checked = true;
            this.rdbPointerSearcherCodeTypeVitaCheat.Location = new System.Drawing.Point(8, 3);
            this.rdbPointerSearcherCodeTypeVitaCheat.Name = "rdbPointerSearcherCodeTypeVitaCheat";
            this.rdbPointerSearcherCodeTypeVitaCheat.Size = new System.Drawing.Size(71, 17);
            this.rdbPointerSearcherCodeTypeVitaCheat.TabIndex = 1;
            this.rdbPointerSearcherCodeTypeVitaCheat.TabStop = true;
            this.rdbPointerSearcherCodeTypeVitaCheat.Text = "VitaCheat";
            this.rdbPointerSearcherCodeTypeVitaCheat.UseVisualStyleBackColor = true;
            // 
            // rdbPointerSearcherCodeTypeCWCheat
            // 
            this.rdbPointerSearcherCodeTypeCWCheat.AutoSize = true;
            this.rdbPointerSearcherCodeTypeCWCheat.Location = new System.Drawing.Point(104, 3);
            this.rdbPointerSearcherCodeTypeCWCheat.Name = "rdbPointerSearcherCodeTypeCWCheat";
            this.rdbPointerSearcherCodeTypeCWCheat.Size = new System.Drawing.Size(71, 17);
            this.rdbPointerSearcherCodeTypeCWCheat.TabIndex = 0;
            this.rdbPointerSearcherCodeTypeCWCheat.Text = "CWCheat";
            this.rdbPointerSearcherCodeTypeCWCheat.UseVisualStyleBackColor = true;
            // 
            // rdbPointerSearcherCodeTypeAR
            // 
            this.rdbPointerSearcherCodeTypeAR.AutoSize = true;
            this.rdbPointerSearcherCodeTypeAR.Location = new System.Drawing.Point(205, 3);
            this.rdbPointerSearcherCodeTypeAR.Name = "rdbPointerSearcherCodeTypeAR";
            this.rdbPointerSearcherCodeTypeAR.Size = new System.Drawing.Size(40, 17);
            this.rdbPointerSearcherCodeTypeAR.TabIndex = 2;
            this.rdbPointerSearcherCodeTypeAR.Text = "AR";
            this.rdbPointerSearcherCodeTypeAR.UseVisualStyleBackColor = true;
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
            this.Text = "TempAR - Vita Edition  V1.04";
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
            this.ResumeLayout(false);
            this.PerformLayout();

    }

        private void FrmStatusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
