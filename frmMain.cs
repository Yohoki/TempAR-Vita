using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TempAR
{
    public partial class frmMain : Form
    {
        private PointerSearcher memdump;
        private PointerSearcher memdump2;
        private PointerSearcher memdump3;
        private PointerSearcher memdump4;
        private PointerSearcher memdump5;
        private PointerSearcher memdump6;
        private uint memory_start;

        // Code types for converter tab
        private const string CT_CNV_CWCHEATPOPS = "CWCheat POPS";

        private const string CT_CNV_NITEPR = "NitePR";
        private const string CT_CNV_R4CCE = "R4CCE to CWCheat";
        private const string CT_CNV_TEMPAR = "CWCheat to R4CCE";

        // Code types for pointer search tab
        private const string CT_PNT_VITACHEAT = "VitaCheat";

        private const string CT_PNT_CWCHEAT = "CWCheat";
        private const string CT_PNT_AR = "AR";

        // Code types for VitaCheat Code Maker tab
        private const string VC_GEN_WRITE = "Write ($0...)";

        private const string VC_GEN_PNTR = "Pointer ($3...)";
        private const string VC_GEN_COMP = "Compress ($4...)";
        private const string VC_GEN_MOV = "MOV/Copy ($5...)";
        private const string VC_GEN_PTRCOM = "Pointer+Compress ($7...)";
        private const string VC_GEN_PTRMOV = "Pointer+MOV ($8...)";

        public int PointerBlk { get; private set; }
        public int PointerGrn { get; private set; }
        public int PointerBlu { get; private set; }
        public int PointerPur { get; private set; }
        public int PointerRed { get; private set; }
        public int PointerOrn { get; private set; }

        public frmMain()
        {
            InitializeComponent();

            cbCnvCodeTypes.Items.AddRange(new object[] {
            CT_CNV_CWCHEATPOPS,
            CT_CNV_NITEPR,
            CT_CNV_R4CCE,
            CT_CNV_TEMPAR});
            cbCnvCodeTypes.Text = CT_CNV_CWCHEATPOPS;

            cbPntCodeTypes.Items.AddRange(new object[] {
            CT_PNT_VITACHEAT,
            CT_PNT_CWCHEAT,
            CT_PNT_AR});
            cbPntCodeTypes.Text = CT_PNT_VITACHEAT;

            comboVitaCheatCodeType.Items.AddRange(new object[] {
            VC_GEN_WRITE,
            VC_GEN_PNTR,
            VC_GEN_COMP,
            VC_GEN_MOV,
            VC_GEN_PTRCOM,
            VC_GEN_PTRMOV});
            comboVitaCheatCodeType.Text = VC_GEN_WRITE;
        }

        /// <summary>
        /// Converstion mode Radio Button
        /// 转换器-转换模式单选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdbConvert_CheckedChanged(object sender, EventArgs e)
        {
            ChangeFrameMode(rdbConvertText.Checked);
        }

        /// <summary>
        /// Convert linebreaks uniformly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtTextInput_TextChanged(object sender, EventArgs e)
        {
            txtTextInput.Text = txtTextInput.Text.Replace("\r\n", "\n").Replace("\n", "\r\n");
            BtnConvert_Click(sender, e);
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Working...";
            lblStatus.Visible = true;
            Refresh();
            if (rdbConvertText.Checked)
            {
                switch (cbCnvCodeTypes.Text)
                {
                    case CT_CNV_CWCHEATPOPS:
                        txtTextOutput.Text = Converter.Cwcpops_pspar(txtTextInput.Text);
                        break;

                    case CT_CNV_NITEPR:
                        txtTextOutput.Text = Converter.Nitepr_pspar(txtTextInput.Text);
                        break;

                    case CT_CNV_R4CCE:
                        txtTextOutput.Text = Converter.Reformat_r4cce(txtTextInput.Text, true);
                        break;

                    case CT_CNV_TEMPAR:
                        txtTextOutput.Text = Converter.Reformat_tempar(txtTextInput.Text);
                        break;

                    default:
                        break;
                }
            }
            else if (txtInputPath.Text.Length > 0 && txtOutputPath.Text.Length > 0)
            {
                switch (cbCnvCodeTypes.Text)
                {
                    case CT_CNV_CWCHEATPOPS:
                        if (File.Exists(txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(txtOutputPath.Text)))
                            Converter.File_cwcpops_pspar(txtInputPath.Text, txtOutputPath.Text);
                        break;

                    case CT_CNV_NITEPR:
                        if (Directory.Exists(txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(txtOutputPath.Text)))
                            Converter.File_nitepr_pspar(txtInputPath.Text, txtOutputPath.Text);
                        break;

                    case CT_CNV_R4CCE:
                        MessageBox.Show("File conversion for this code type is not supported at this time!");
                        break;

                    case CT_CNV_TEMPAR:
                        if (File.Exists(txtInputPath.Text) && Directory.Exists(Path.GetDirectoryName(txtOutputPath.Text)))
                            Converter.File_reformat_tempar(txtInputPath.Text, txtOutputPath.Text);
                        break;

                    default:
                        break;
                }
            }
            lblStatus.Visible = false;
        }

        private void BtnInputBrowse_Click(object sender, EventArgs e)
        {
            switch (cbCnvCodeTypes.SelectedIndex)
            {
                case 0:
                case 1:
                    txtInputPath.Text = Utils.OpenFile(txtInputPath.Text, "CWCheat Database File (*.db)|*.db", "Open");
                    break;

                case 2:
                    txtInputPath.Text = Utils.OpenDirectory(txtInputPath.Text, "Select your NitePR code file directory:");
                    break;

                case 3:
                    MessageBox.Show("File conversion for this code type is not supported at this time!");
                    break;

                default:
                    break;
            }
        }

        private void BtnOutputBrowse_Click(object sender, EventArgs e)
        {
            txtOutputPath.Text = Utils.SaveFile(txtOutputPath.Text, "CWCheat Database File (*.db)|*.db", "Save");
        }

        /// <summary>
        /// Select all with Ctrl+S
        /// 实现ctrl+a全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextFieldSelectAll(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                ((TextBoxBase)sender).SelectAll();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else
                OnKeyDown(e);
        }

        /// <summary>
        /// Converter Code type mode switch
        /// 代码转换器- 文本文件模式切换
        /// </summary>
        /// <param name="mode"></param>
        private void ChangeFrameMode(bool mode)
        {
            if (mode)
            {
                pnlConvertText.BringToFront();
            }
            else
            {
                pnlConvertFile.BringToFront();
                if (!String.IsNullOrEmpty(cbCnvCodeTypes.Text) && cbCnvCodeTypes.Text == CT_CNV_R4CCE)
                {
                    cbCnvCodeTypes.Text = CT_CNV_CWCHEATPOPS;
                }
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            comboPointerSearcherMode.SelectedIndex = 0;
            comboVitaCheatCodeType.SelectedIndex = 0;
            comboVitaCheatPointerLevel.SelectedIndex = 0;
        }

        //
        //
        // Pointer Searcher Tab starts here
        //
        //
        private void BtnPointerSearcherFindPointers_Click(object sender, EventArgs e)
        {
            var num1 = Utils.ParseNum(txtPointerSearcherAddress1.Text, NumberStyles.AllowHexSpecifier);
            var num3 = Utils.ParseNum(txtPointerSearcherMaxOffset.Text);
            memory_start = Utils.ParseNum(txtBaseAddress.Text);
            PointerBlk = 0;
            PointerGrn = 0;
            PointerBlu = 0;
            PointerPur = 0;
            PointerRed = 0;
            PointerOrn = 0;
            treePointerSearcherPointers.Nodes.Clear();
            memdump = new PointerSearcher(txtPointerSearcherMemDump1.Text, memory_start);
            memdump2 = new PointerSearcher(txtPointerSearcherMemDump2.Text, memory_start);
            memdump3 = new PointerSearcher(txtPointerSearcherMemDump3.Text, memory_start);
            memdump4 = new PointerSearcher(txtPointerSearcherMemDump4.Text, memory_start);
            memdump5 = new PointerSearcher(txtPointerSearcherMemDump5.Text, memory_start);
            memdump6 = new PointerSearcher(txtPointerSearcherMemDump6.Text, memory_start);
            AddPointerTree(memdump.FindPointers(num1, num3), treePointerSearcherPointers.SelectedNode);
        }

        private void TreePointerSearcherPointers_DoubleClick(object sender, EventArgs e)
        {
            if (treePointerSearcherPointers.SelectedNode == null) return;

            var num1 = Utils.ParseNum(txtPointerSearcherMaxOffset.Text, NumberStyles.AllowHexSpecifier);
            treePointerSearcherPointers.SelectedNode.Nodes.Clear();
            AddPointerTree(memdump.FindPointers(new PointerSearcherLog(treePointerSearcherPointers.SelectedNode.Text, memory_start).Address, num1), treePointerSearcherPointers.SelectedNode);
        }

        private void BtnPointerSearcherClear_Click(object sender, EventArgs e)
        {
            treePointerSearcherPointers.Nodes.Clear();
            txtColorBlack.Text = "0";
            txtColorGreen.Text = "0";
            txtColorBlue.Text = "0";
            txtColorOrchid.Text = "0";
            txtColorRed.Text = "0";
            txtColorOrange.Text = "0";
            PointerBlk = 0;
            PointerGrn = 0;
            PointerBlu = 0;
            PointerPur = 0;
            PointerRed = 0;
            PointerOrn = 0;
        }

        private void TreePointerSearcherPointers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treePointerSearcherPointers.SelectedNode == null) return;
            var pointers = new List<PointerSearcherLog>();
            var strArray = treePointerSearcherPointers.SelectedNode.FullPath.ToString().Split('\\');
            for (int index = 0; index < strArray.Length; ++index)
            {
                pointers.Add(new PointerSearcherLog(strArray[strArray.Length - 1 - index], memory_start));
            }

            var num = Utils.ParseNum(txtPointerSearcherValue.Text);
            var bittype = 2;
            if (rdbPointerSearcherBitType16.Checked)
            {
                bittype = 1;
                num &= (uint)ushort.MaxValue;
            }
            else if (rdbPointerSearcherBitType8.Checked)
            {
                bittype = 0;
                num &= (uint)byte.MaxValue;
            }
            //
            // Check which code is being generated
            //
            switch (cbPntCodeTypes.Text)
            {
                case CT_PNT_VITACHEAT:
                    txtPointerSearcherCode.Text = GetVitaCheatPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
                    break;

                case CT_PNT_CWCHEAT:
                    txtPointerSearcherCode.Text = GetCWCheatPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
                    break;

                case CT_PNT_AR:
                    txtPointerSearcherCode.Text = GetARPointerCode(pointers, bittype, num).Replace("\n", "\r\n");
                    break;

                default:
                    break;
            }
        }

        private void TxtPointerSearcherMemDump_Click(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = Utils.OpenFile(((TextBox)sender).Text, null, "Open File...");
        }

        private void TreePointerSearcherPointers_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Space:
                    TreePointerSearcherPointers_DoubleClick(null, null);
                    break;

                case Keys.Delete:
                    if (treePointerSearcherPointers.SelectedNode == null) break;
                    treePointerSearcherPointers.SelectedNode.Remove();
                    break;

                default:
                    break;
            }
        }

        private void AddPointerTree(List<PointerSearcherLog> pointers, TreeNode parent)
        {
            if (pointers == null)
                return;
            Utils.SortList<PointerSearcherLog>(pointers, "Address", true);
            if (chkPointerSearcherOptimizePointerPaths.Checked)
            {
                var pointerSearcherLogList = new List<PointerSearcherLog>();
                if (treePointerSearcherPointers.Nodes.Count > 0)
                {
                    var parentEqualTree = GetParentEqualTree(treePointerSearcherPointers.Nodes, treePointerSearcherPointers.SelectedNode == null ? 0 : treePointerSearcherPointers.SelectedNode.Level);
                    for (int index = 0; index < parentEqualTree.Count; ++index)
                    {
                        pointerSearcherLogList.Add(new PointerSearcherLog(parentEqualTree[index].Text, memory_start));
                    }
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
                var color = Color.Black;
                var PointerColor = 0;

                if (memdump2 != null)
                {
                    var strArray = ((treePointerSearcherPointers.SelectedNode == null ? "" : treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1]).Split('\\');
                    var num = Utils.ParseNum(txtPointerSearcherAddress2.Text, NumberStyles.AllowHexSpecifier);
                    if (num < memory_start) num += memory_start;
                    var address = 0u;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        var pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], memory_start);
                        if (index2 == 0) address = pointerSearcherLog.Address;
                        address = memdump2.GetPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                    }
                    if ((int)num == (int)address) PointerColor += 1;
                }

                if (memdump2 != null && memdump3 != null)
                {
                    var strArray = ((treePointerSearcherPointers.SelectedNode == null ? "" : treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1]).Split('\\');
                    var num = Utils.ParseNum(txtPointerSearcherAddress3.Text, NumberStyles.AllowHexSpecifier);
                    if (num < memory_start) num += memory_start;
                    var address = 0u;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        var pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], memory_start);
                        if (index2 == 0) address = pointerSearcherLog.Address;
                        address = memdump3.GetPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                    }
                    if ((int)num == (int)address) PointerColor += 1;
                }

                if (memdump2 != null && memdump3 != null && memdump4 != null)
                {
                    var strArray = ((treePointerSearcherPointers.SelectedNode == null ? "" : treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1].ToString()).Split('\\');
                    var num = Utils.ParseNum(txtPointerSearcherAddress4.Text, NumberStyles.AllowHexSpecifier);
                    if (num < memory_start) num += memory_start;
                    var address = 0u;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        var pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], memory_start);
                        if (index2 == 0) address = pointerSearcherLog.Address;
                        address = memdump4.GetPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                    }
                    if ((int)num == (int)address) PointerColor += 1;
                }

                if (memdump2 != null && memdump3 != null && memdump4 != null && memdump5 != null)
                {
                    var strArray = ((treePointerSearcherPointers.SelectedNode == null ? "" : treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1]).Split('\\');
                    var num = Utils.ParseNum(txtPointerSearcherAddress5.Text, NumberStyles.AllowHexSpecifier);
                    if (num < memory_start) num += memory_start;
                    var address = 0u;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        var pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], memory_start);
                        if (index2 == 0) address = pointerSearcherLog.Address;
                        address = memdump5.GetPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                    }
                    if ((int)num == (int)address) PointerColor += 1;
                }

                if (memdump2 != null && memdump3 != null && memdump4 != null && memdump5 != null && memdump6 != null)
                {
                    var strArray = ((treePointerSearcherPointers.SelectedNode == null ? "" : treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1]).Split('\\');
                    var num = Utils.ParseNum(txtPointerSearcherAddress6.Text, NumberStyles.AllowHexSpecifier);
                    if (num < memory_start) num += memory_start;
                    var address = 0u;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        var pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], memory_start);
                        if (index2 == 0) address = pointerSearcherLog.Address;
                        address = memdump6.GetPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                    }
                    if ((int)num == (int)address) PointerColor += 1;
                }

                switch (PointerColor)
                {
                    case 0:
                        color = Color.Black;
                        PointerBlk += 1;
                        txtColorBlack.Text = PointerBlk.ToString();
                        break;

                    case 1:
                        color = Color.Green;
                        PointerGrn += 1;
                        txtColorGreen.Text = PointerGrn.ToString();
                        break;

                    case 2:
                        color = Color.Blue;
                        PointerBlu += 1;
                        txtColorBlue.Text = PointerBlu.ToString();
                        break;

                    case 3:
                        color = Color.Orchid;
                        PointerPur += 1;
                        txtColorOrchid.Text = PointerPur.ToString();
                        break;

                    case 4:
                        color = Color.Red;
                        PointerRed += 1;
                        txtColorRed.Text = PointerRed.ToString();
                        break;

                    case 5:
                        color = Color.Orange;
                        PointerOrn += 1;
                        txtColorOrange.Text = PointerOrn.ToString();
                        break;

                    default:
                        break;
                }

                if (!pointers[index1].Negative || chkPointerSearcherIncludeNegatives.Checked)
                {
                    var node = new TreeNode
                    {
                        Text = pointers[index1].ToString(chkPointerSearcherRealAddresses.Checked ? 0U : memory_start),
                        ForeColor = color
                    };
                    if (parent == null)
                    {
                        treePointerSearcherPointers.Nodes.Add(node);
                    }
                    else
                    {
                        parent.Nodes.Add(node);
                    }
                }
            }
        }

        private TreeNodeCollection GetParentEqualTree(TreeNodeCollection nodes, int level)
        {
            using (var treeView = new TreeView())
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Level <= level)
                    {
                        treeView.Nodes.Add(node.Text);
                        foreach (TreeNode treeNode in GetParentEqualTree(node.Nodes, level))
                            treeView.Nodes.Add(treeNode.Text);
                    }
                }
                return treeView.Nodes;
            }
        }

        //
        // Default values for "Base Address"
        //
        private void ComboPointerSearcherMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBaseAddress.Enabled = false;
            switch (comboPointerSearcherMode.SelectedIndex)
            {
                case 0: // Sony Vita
                    memory_start = 0x81000000U;
                    break;

                case 1: // Sony PSP
                    memory_start = 0x8800000U;
                    break;

                case 2: // Nintendo DS
                    memory_start = 0x2000000U;
                    break;

                case 3: // Other..
                    memory_start = 0x0U;
                    txtBaseAddress.Enabled = true;
                    break;

                default:
                    break;
            }
            txtBaseAddress.Text = string.Format("0x{0:X08}", (object)memory_start);
        }

        //
        // AR Code Generation
        //
        private string GetARPointerCode(List<PointerSearcherLog> pointers, int bittype, uint value)
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
            return (chkPointerSearcherRAWCode.Checked ? "" : "::Generated Code\n") + str2;
        }

        //
        // VitaCheat Code Generation
        //
        private string GetVitaCheatPointerCode(List<PointerSearcherLog> pointers, int bittype, uint value)
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
            var str1 = "";
            var str2 = "";
            var str3 = $"$3300 00000000 {value:X08}\n";
            for (int index = 1; index < pointers.Count; ++index)
            {
                str1 = !pointers[index].Negative ? $"{str1}$3{bittype}00 00000000 {pointers[index].Offset:X08}\n" : $"{str1}$3{bittype}00 00000000 {(0x100000000L - pointers[index].Offset):X08}\n";
            }

            if (pointers.Count > 1) str1 += string.Format("");

            str2 = !pointers[0].Negative ? $"{str2}$3{bittype:X01}{pointers.Count:X02} {pointers[0].Address:X08} {pointers[0].Offset:X08}\n" + str1 : $"{str2}$3{bittype:X01}{pointers.Count:X02} {pointers[0].Address:X08} {(0x100000000L - pointers[0].Offset):X08}\n{str1}";

            return (chkPointerSearcherRAWCode.Checked ? "" : $"_V0 Generated Code\n{str2}{str3}");
        }

        //
        // CWCheat Code Generation
        //
        private string GetCWCheatPointerCode(List<PointerSearcherLog> pointers, int bittype, uint value)
        {
            if (bittype != 0 && bittype != 1 && bittype != 2) bittype = 2;
            if (pointers[0].Negative) bittype += 3;
            var str1 = "";
            for (int index = 0; index < pointers.Count - 1; ++index)
            {
                str1 = index % 2 != 0 ? str1 + $" 0x{(pointers[index].Negative ? 3 : 2):X01}{pointers[index].Offset:X07}\n" : $"{str1}{(chkPointerSearcherRAWCode.Checked ? "" : "_L ")}0x{(pointers[index].Negative ? 3 : 2):X01}{pointers[index].Offset:X07}";
            }

            if (pointers.Count % 2 == 0) str1 += string.Format(" 0x00000000");

            var str2 = $"{(chkPointerSearcherRAWCode.Checked ? "" : "_L ")}0x6{(uint)((int)pointers[0].Address - (int)memory_start):X07} 0x{value:X08}\n{(chkPointerSearcherRAWCode.Checked ? "" : "_L ")}0x000{bittype:X01}{pointers.Count:X04} 0x{pointers[pointers.Count - 1].Offset:X08}\n{str1}";

            return (chkPointerSearcherRAWCode.Checked ? "" : $"_C0 Generated Code\n{str2}");
        }

        private void TxtFileDragDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void TxtFileDragDrop_DragDrop(object sender, DragEventArgs e)
        {
            ((Control)sender).Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
        }

        private void TxtValidateHexString_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Regex.IsMatch(e.KeyChar.ToString(), "[^0-9a-fA-F\x0001\x0003\b\x0016]")) return;
            e.Handled = true;
        }

        private void CbCodeTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;

            if (!string.IsNullOrEmpty(comboBox.Text))
            {
                btnConvert.Enabled = true;
                BtnConvert_Click(sender, e);
            }
        }

        private void TxtPointerSearcherMemDump_TextChanged(object sender, EventArgs e)
        {
            txtPointerSearcherMemDump2.Enabled = txtPointerSearcherAddress2.Enabled = txtPointerSearcherMemDump1.Text.Length > 0;
            txtPointerSearcherMemDump3.Enabled = txtPointerSearcherAddress3.Enabled = txtPointerSearcherMemDump2.Text.Length > 0;
            txtPointerSearcherMemDump4.Enabled = txtPointerSearcherAddress4.Enabled = txtPointerSearcherMemDump3.Text.Length > 0;
            txtPointerSearcherMemDump5.Enabled = txtPointerSearcherAddress5.Enabled = txtPointerSearcherMemDump4.Text.Length > 0;
            txtPointerSearcherMemDump6.Enabled = txtPointerSearcherAddress6.Enabled = txtPointerSearcherMemDump5.Text.Length > 0;
        }
        private void txtVCInstructions_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void ComboVitaCheatCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var strWiki = "https://github.com/r0ah/vitacheat/wiki/";
            var strPage = "";
            txtVitaCheatAddress2.Enabled = false;
            txtVitaCheatValue.Enabled = true;
            comboVitaCheatPointerLevel.Enabled = false;
            groupVitaCheatAddress1Offset.Enabled = false;
            groupVitaCheatAddress2Offset.Enabled = false;
            groupVitaCheatCompression.Enabled = false;
            switch (comboVitaCheatCodeType.Text)
            {
                case VC_GEN_WRITE: // Write
                    strPage = "Write";
                    txtVCInstructions.Text = "WRITE\r\nCreates a code that locks the value at an address to the specified number.\r\n\r\nPut the desired address in the 'Address 1' box and put your desired value in the box marked 'Desired Value'\r\n\r\nFor example, to lock your HP at 100, we need to put our HP's address (I'll use 83001337) into 'Address 1' and 100 into 'Desired Value'.\r\n\r\nThis generates the code:\r\n_V0 Infinite HP\r\n$0200 83001337 00000064\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_PNTR: // Pointer
                    strPage = "Pointer-Write";
                    comboVitaCheatPointerLevel.Enabled = true;
                    groupVitaCheatAddress1Offset.Enabled = true;
                    txtVCInstructions.Text = "POINTER\r\nPointers are advanced codes that write to addresses that move around.\r\n\r\nSometimes developers move blocks of RAM around.To keep track of this movement, a specific address keeps track of that block's starting point. The location of an address within that block is called an Offset and is the distance from the start of the block to the desired location. Often, that location is another pointer, leading to a new movable block. To follow a second, third or more pointers, use the pointer level.\r\n\r\nPut the pointer's Address into the 'Address 1' box.And the value you would like in the 'Desired Value' box.\r\n\r\nSelect how many pointers you need to follow in the 'Pointer Level' box and put each of their offsets into an offset box.The first offset is at the top, and the last offset is at the bottom.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_COMP: // Compress
                    strPage = "Compression";
                    groupVitaCheatCompression.Enabled = true;
                    numericVitaCheatCompressionLevelOffset.Enabled = false;
                    lblVitaCheatCompressionLevelOffset.Enabled = false;
                    txtVCInstructions.Text = "COMPRESS\r\nCompress is an advanced code that applies the 'Write' code several times in different places in an ordered manner.\r\n\r\nType the first address in the 'Address 1' box as well as the desired value in 'Desired Value'.\r\n\r\nFind out how far away the second address is. You can use a hex Calculator to subtract these two. Place that offset into the box labeled 'Address Gap'\r\n\r\nIf you would like to have the value increased each time the code is applied, use 'Value Gap' to increase it.\r\n\r\nFinally, select or type the number of times you need this code to repeat in the '# of Compressions' box.\r\n\r\nExample:\r\nTo give 99 of each potion type, we will first find the address for the 1st potion (We'll use 83001337) in the game and know how many potions there are. We'll pretend there are normal, greater and high potions, so 3 compressions total. The greater potion is at 83001347 and high is at 83001357. This puts the Value offset at 0x00000010. We want them all to be 99, so the desired value will be 99 and the Value Gap will remain 0. The generated code will then be:\r\n_V0 Infinite Potions\r\n$4200 83001337 00000063\r\n$0003 00000010 00000000\r\n\r\nThis has the same effect as the following code:\r\n_V0 Infinite Potions\r\n$0200 83001337 00000063\r\n$0200 83001347 00000063\r\n$0200 83001357 00000063\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_MOV: // MOV
                    strPage = "MOV";
                    txtVitaCheatAddress2.Enabled = true;
                    txtVitaCheatValue.Enabled = false;
                    txtVCInstructions.Text = "MOV/COPY\r\nMOV/Copy codes simply copies the value from one address to another.\r\n\r\nPut the address you want changed into 'Address 1' and the address that you want to copy from in 'Copy From'.\r\n\r\nExample:\r\nTo make an 'Always Full HP' code, we can put the address for our current HP (83001337) into 'Address 1'. Then put the address for our Max HP (83001333) into 'Copy From'. The code generator will give the following code:\r\n_V0 Always Full HP\r\n$5200 83001337 83001333\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_PTRCOM: // Pointer Compress
                    strPage = "Pointer-Compression";
                    comboVitaCheatPointerLevel.Enabled = true;
                    groupVitaCheatAddress1Offset.Enabled = true;
                    groupVitaCheatCompression.Enabled = true;
                    numericVitaCheatCompressionLevelOffset.Enabled = true;
                    lblVitaCheatCompressionLevelOffset.Enabled = true;
                    txtVCInstructions.Text = "POINTER + COMPRESS\r\nCreates several Write codes in an ordered manner with a pointer as the starting point.\r\n\r\nMake sure to set which level you want the code to Compres at. Leaving at '1' will apply the compression at the first offset.\r\n\r\nExample:\r\n\r\n$7203 81000000 00000010\r\n$7200 00000000 00000200\r\n$7200 00000000 00003000\r\n$7702 00000000 00000063  - Compression level Changed to 2 with $7702\r\n$0003 00000100 00000000\r\n\r\nProduces the following code:\r\n\r\n$3203 81000000 00000010\r\n$3200 00000000 00000200  Compression Applied Here\r\n$3200 00000000 00003000\r\n$3303 00000000 00000063\r\n\r\n\r\n$3203 81000000 00000010\r\n$3200 00000000 00000300  Compression Applied Here\r\n$3200 00000000 00003000\r\n$3303 00000000 00000063\r\n\r\n\r\n$3203 81000000 00000010\r\n$3200 00000000 00000400  Compression Applied Here\r\n$3200 00000000 00003000\r\n$3303 00000000 00000063\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_PTRMOV: // Pointer MOV
                    strPage = "Pointer-MOV";
                    comboVitaCheatPointerLevel.Enabled = true;
                    groupVitaCheatAddress1Offset.Enabled = true;
                    groupVitaCheatAddress2Offset.Enabled = true;
                    txtVitaCheatValue.Enabled = false;
                    txtVitaCheatAddress2.Enabled = true;
                    txtVCInstructions.Text = "POINTER + MOV\r\nPointer MOV copies one address to another, but uses pionters as the starting points.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;
            }
        }

        private void ComboVitaCheatPointerLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVitaCheatAddress1Offset2.Enabled = false;
            txtVitaCheatAddress2Offset2.Enabled = false;
            txtVitaCheatAddress1Offset3.Enabled = false;
            txtVitaCheatAddress2Offset3.Enabled = false;
            txtVitaCheatAddress1Offset4.Enabled = false;
            txtVitaCheatAddress2Offset4.Enabled = false;
            txtVitaCheatAddress1Offset5.Enabled = false;
            txtVitaCheatAddress2Offset5.Enabled = false;
            if (comboVitaCheatPointerLevel.SelectedIndex >= 1)
            {
                txtVitaCheatAddress1Offset2.Enabled = true;
                txtVitaCheatAddress2Offset2.Enabled = true;
            }
            if (comboVitaCheatPointerLevel.SelectedIndex >= 2)
            {
                txtVitaCheatAddress1Offset3.Enabled = true;
                txtVitaCheatAddress2Offset3.Enabled = true;
            }
            if (comboVitaCheatPointerLevel.SelectedIndex >= 3)
            {
                txtVitaCheatAddress1Offset4.Enabled = true;
                txtVitaCheatAddress2Offset4.Enabled = true;
            }
            if (comboVitaCheatPointerLevel.SelectedIndex >= 4)
            {
                txtVitaCheatAddress1Offset5.Enabled = true;
                txtVitaCheatAddress2Offset5.Enabled = true;
            }
        }

        private void BtnVitaCheatGenerate_Click(object sender, EventArgs e)
        {
            //
            //Check for hex numbers and give error if bad
            //
            var VCadd1 = Utils.ParseNum(txtVitaCheatAddress1.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Address 1, make sure address is a valid hexadecimal number.");
            var VCadd2 = Utils.ParseNum(txtVitaCheatAddress2.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Address 2 (Copy from), make sure address is a valid hexadecimal number.");
            var VCaddgap = Utils.ParseNum(txtVitaCheatAddressGap.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Address Gap, make sure value is a valid hexadecimal number.");
            var VCvalgap = Utils.ParseNum(txtVitaCheatValueGap.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Value Gap, make sure value is a valid hexadecimal number.");
            var VCcomps = Utils.ParseNum(numericVitaCheatCompressions.Text, NumberStyles.AllowHexSpecifier, "You shouldn't be seeing this error! My bad, dude. Error: Compressions thingy is fucked.");

            foreach (Control x in groupVitaCheatAddress1Offset.Controls)
            {
                if (x is TextBox)
                {
                    if (x.Enabled)
                    {
                        var VCgenptr2 = Utils.ParseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier, "Wrong Format!");
                    }
                }
            }
            foreach (Control x in groupVitaCheatAddress2Offset.Controls)
            {
                if (x is TextBox)
                {
                    if (x.Enabled)
                    {
                        var VCgenptr2 = Utils.ParseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier, "Wrong Format!");
                    }
                }
            }
            var VCstr1 = "_V0 Generated Code\r\n\r\n";
            var VCAddr1 = Utils.ParseNum(txtVitaCheatAddress1.Text, NumberStyles.AllowHexSpecifier);
            var VCAddr2 = Utils.ParseNum(txtVitaCheatAddress2.Text, NumberStyles.AllowHexSpecifier);
            var VCAddGp = Utils.ParseNum(txtVitaCheatAddressGap.Text, NumberStyles.AllowHexSpecifier);
            var VCValGp = Utils.ParseNum(txtVitaCheatValueGap.Text, NumberStyles.AllowHexSpecifier);
            var VCComps = Utils.ParseNum(numericVitaCheatCompressions.Text);
            var VCValue = Utils.ParseNum(txtVitaCheatValue.Text);
            //
            //Get Bit Type from radio buttons
            //
            var bittype = 2;
            bittype = rdbVitaCheatBitType8Bit.Checked ? 0 : rdbVitaCheatBitType16Bit.Checked ? 1 : 2;

            //
            // Generate code
            //
            switch (comboVitaCheatCodeType.Text)
            {
                case VC_GEN_WRITE:
                    var VCGenWrite1 = $"$0{bittype}00 {VCAddr1:X08} {VCValue:X08}\r\n";
                    txtVitaCheatCode.Text = VCstr1 + VCGenWrite1;
                    break;

                case VC_GEN_PNTR:
                    var VCGenPtrstr2 = "";
                    var VCGenptroff1 = Utils.ParseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtr1 = $"$3{bittype}0{comboVitaCheatPointerLevel.Text} {VCAddr1:X08} {VCGenptroff1:X08}\r\n";
                    var VCGenPtr3 = $"$3300 00000000 {VCValue:X08}";

                    foreach (Control x in groupVitaCheatAddress1Offset.Controls)
                    {
                        if (x is TextBox)
                        {
                            if (x.Enabled)
                            {
                                var VCGenptr2 = Utils.ParseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                                if (((TextBox)x).TabIndex != 0)
                                {
                                    VCGenPtrstr2 = $"$3{bittype}00 00000000 {VCGenptr2:X08}\r\n{VCGenPtrstr2}";
                                }
                            }
                        }
                    }
                    txtVitaCheatCode.Text = VCstr1 + VCGenPtr1 + VCGenPtrstr2 + VCGenPtr3;
                    break;

                case VC_GEN_COMP:
                    var VCGenComp1 = $"$4{bittype}00 {VCAddr1:X08} {VCValue:X08}\r\n";
                    var VCGenComp2 = $"${VCComps:X04} {VCAddGp:X08} {VCValGp:X08}\r\n";
                    txtVitaCheatCode.Text = VCstr1 + VCGenComp1 + VCGenComp2;
                    break;

                case VC_GEN_MOV:
                    var VCGenMov1 = string.Format("$5{0}00 {1:X08} {2:X08}\r\n", bittype, VCAddr1, VCAddr2);
                    txtVitaCheatCode.Text = VCstr1 + VCGenMov1;
                    break;

                case VC_GEN_PTRCOM:
                    var VCGenPtrComstr2 = "";
                    var VCGenptrcomoff1 = Utils.ParseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtrCom1 = $"$7{bittype}0{comboVitaCheatPointerLevel.Text} {VCAddr1:X08} {VCGenptrcomoff1:X08}\r\n";
                    var VCGenPtrCom3 = $"$770{numericVitaCheatCompressionLevelOffset.Text} 00000000 {VCValue:X08}\r\n";
                    var VCGenPtrCom4 = $"${VCComps:X04} 0000{VCAddGp:X04} 0000{VCValGp:X04}";
                    foreach (Control x in groupVitaCheatAddress1Offset.Controls)
                    {
                        if (x is TextBox)
                        {
                            if (x.Enabled)
                            {
                                var VCGenptr2 = Utils.ParseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                                if (((TextBox)x).TabIndex != 0)
                                {
                                    VCGenPtrComstr2 = $"$7{bittype}00 00000000 {VCGenptr2:X08}\r\n{VCGenPtrComstr2}";
                                }
                            }
                        }
                    }
                    txtVitaCheatCode.Text = VCstr1 + VCGenPtrCom1 + VCGenPtrComstr2 + VCGenPtrCom3 + VCGenPtrCom4;
                    break;

                case VC_GEN_PTRMOV:
                    var VCGenPtrMovstr2 = "";
                    var VCGenptrmovoff1 = Utils.ParseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtrMov1 = $"$8{bittype}0{comboVitaCheatPointerLevel.Text} {VCAddr1:X08} {VCGenptrmovoff1:X08}\r\n";
                    var VCGenPtrMov3 = string.Format("$8800 00000000 00000000\r\n");
                    foreach (Control x in groupVitaCheatAddress1Offset.Controls)
                    {
                        if (x is TextBox)
                        {
                            if (x.Enabled)
                            {
                                var VCGenptrmov2 = Utils.ParseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                                if (((TextBox)x).TabIndex != 0)
                                {
                                    VCGenPtrMovstr2 = $"$8{bittype}00 00000000 {VCGenptrmov2:X08}\r\n{VCGenPtrMovstr2}";
                                }
                            }
                        }
                    }
                    var VCGenPtr2str2 = "";
                    var VCGenptrmov2off1 = Utils.ParseNum(txtVitaCheatAddress2Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtrMov21 = $"$8{bittype + 4}0{comboVitaCheatPointerLevel.Text} {VCAddr2:X08} {VCGenptrmov2off1:X08}\r\n";
                    var VCGenPtrMov23 = string.Format("$8900 00000000 00000000");
                    foreach (Control x in groupVitaCheatAddress2Offset.Controls)
                    {
                        if (x is TextBox)
                        {
                            if (x.Enabled)
                            {
                                uint VCGenptrmov22 = Utils.ParseNum(((TextBox)x).Text, NumberStyles.AllowHexSpecifier);
                                if (((TextBox)x).TabIndex != 0)
                                {
                                    VCGenPtr2str2 = $"$8{bittype + 4}00 00000000 {VCGenptrmov22:X08}\r\n{VCGenPtr2str2}";
                                }
                            }
                        }
                    }
                    txtVitaCheatCode.Text = VCstr1 + VCGenPtrMov1 + VCGenPtrMovstr2 + VCGenPtrMov3 + VCGenPtrMov21 + VCGenPtr2str2 + VCGenPtrMov23;
                    break;

                default:
                    break;
            }
        }
    }
}