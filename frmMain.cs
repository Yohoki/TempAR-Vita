using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TempAR
{
    public partial class FrmMain : Form
    {
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
        private const string VC_GEN_MOV = "MOV ($5...)";
        private const string VC_GEN_COMP = "Compress ($4...)";
        private const string VC_GEN_PNTR = "Pointer Write ($3...)";
        private const string VC_GEN_PTRMOV = "Pointer MOV ($8...)";
        private const string VC_GEN_PTRCOM = "Pointer Compress ($7...)";
        private const string VC_GEN_ARMWRT = "ARM Write ($A...)";
        private const string VC_GEN_BTNPAD = "Button Pad ($C...)";
        private const string VC_GEN_CNDTN = "Condition ($D...)";
        private const string VC_GEN_B2COD = "B200 ($B...)";

        // Segment Options for VitaCheat Code Maker tab
        private const string VC_GEN_B2_NONE = "None";
        private const string VC_GEN_B2_SEG0 = "Seg0";
        private const string VC_GEN_B2_SEG1 = "Seg1";

        public int PointerBlk { get; private set; }
        public int PointerGrn { get; private set; }
        public int PointerBlu { get; private set; }
        public int PointerPur { get; private set; }
        public int PointerRed { get; private set; }
        public int PointerOrn { get; private set; }

        public FrmMain()
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

            //vita cheat code type drop list text
            comboVitaCheatCodeType.Items.AddRange(new object[] {
            VC_GEN_WRITE,
            VC_GEN_MOV,
            VC_GEN_COMP,
            VC_GEN_PNTR,
            VC_GEN_PTRMOV,
            VC_GEN_PTRCOM,
            VC_GEN_ARMWRT,
            VC_GEN_BTNPAD,
            VC_GEN_CNDTN,
            VC_GEN_B2COD,});
            comboVitaCheatCodeType.Text = VC_GEN_WRITE;

            //vita cheat B200 drop list text
            comboVitaCheatB200.Items.AddRange(new object[] {
            VC_GEN_B2_NONE,
            VC_GEN_B2_SEG0,
            VC_GEN_B2_SEG1,});
            comboVitaCheatB200.Text = VC_GEN_B2_NONE;

            //vita cheat button type drop list text and value
            List<VitaCheatData> ControllerType = new List<VitaCheatData>
            {
                new VitaCheatData() { Type = 9, Name = "None" },
                new VitaCheatData() { Type = 0, Name = "Undefined" },
                new VitaCheatData() { Type = 1, Name = "PSVita" },
                new VitaCheatData() { Type = 2, Name = "PSTV" },
                new VitaCheatData() { Type = 4, Name = "DualShock 3" },
                new VitaCheatData() { Type = 8, Name = "DualShock 4" }
            };
            comboVitaCheatButtonType.DataSource = ControllerType;
            comboVitaCheatButtonType.DisplayMember = "Name";
            comboVitaCheatButtonType.ValueMember = "Type";

            //vita cheat button drop list text and value
            List<VitaCheatData> ButtonType1 = new List<VitaCheatData>
            {
                new VitaCheatData() { Type = 0,    Name = "Null" },
                new VitaCheatData() { Type = 1,    Name = "Select" },
                new VitaCheatData() { Type = 8,    Name = "Start" },
                new VitaCheatData() { Type = 10,   Name = "UP" },
                new VitaCheatData() { Type = 20,   Name = "Right" },
                new VitaCheatData() { Type = 40,   Name = "Down" },
                new VitaCheatData() { Type = 80,   Name = "Left" },
                new VitaCheatData() { Type = 100,  Name = "L" },
                new VitaCheatData() { Type = 200,  Name = "R" },
                new VitaCheatData() { Type = 1000, Name = "Triangle" },
                new VitaCheatData() { Type = 2000, Name = "Circle" },
                new VitaCheatData() { Type = 4000, Name = "Cross" },
                new VitaCheatData() { Type = 8000, Name = "Square" }
            };
            comboVitaCheatButton.DataSource = ButtonType1;
            comboVitaCheatButton.DisplayMember = "Name";
            comboVitaCheatButton.ValueMember = "Type";

            //vita cheat 2nd button drop list text and value
            List<VitaCheatData> ButtonType2 = new List<VitaCheatData>
            {
                new VitaCheatData() { Type = 0,    Name = "Null" },
                new VitaCheatData() { Type = 1,    Name = "Select" },
                new VitaCheatData() { Type = 8,    Name = "Start" },
                new VitaCheatData() { Type = 10,   Name = "UP" },
                new VitaCheatData() { Type = 20,   Name = "Right" },
                new VitaCheatData() { Type = 40,   Name = "Down" },
                new VitaCheatData() { Type = 80,   Name = "Left" },
                new VitaCheatData() { Type = 100,  Name = "L" },
                new VitaCheatData() { Type = 200,  Name = "R" },
                new VitaCheatData() { Type = 1000, Name = "Triangle" },
                new VitaCheatData() { Type = 2000, Name = "Circle" },
                new VitaCheatData() { Type = 4000, Name = "Cross" },
                new VitaCheatData() { Type = 8000, Name = "Square" }
            };
            comboVitaCheatButton2.DataSource = ButtonType2;
            comboVitaCheatButton2.DisplayMember = "Name";
            comboVitaCheatButton2.ValueMember = "Type";

            //vita cheat Condition drop list text and value
            List<VitaCheatData> ConditionalType = new List<VitaCheatData>
            {
                new VitaCheatData() { Type = 99, Name = "None" },
                new VitaCheatData() { Type = 0,  Name = "(=) Equal to X 8bit" },
                new VitaCheatData() { Type = 1,  Name = "(=) Equal to X 16bit" },
                new VitaCheatData() { Type = 2,  Name = "(=) Equal to X 32bit" },
                new VitaCheatData() { Type = 3,  Name = "(<>) Unequal to X (8bit)" },
                new VitaCheatData() { Type = 4,  Name = "(<>) Unequal to X (16bit)" },
                new VitaCheatData() { Type = 5,  Name = "(<>) Unequal to X (32bit)" },
                new VitaCheatData() { Type = 6,  Name = "(>) Greater than X (8bit)" },
                new VitaCheatData() { Type = 7,  Name = "(>) Greater than X (16bit)" },
                new VitaCheatData() { Type = 8,  Name = "(>) Greater than X (32bit)" },
                new VitaCheatData() { Type = 9,  Name = "(<) Less than X (8bit)" },
                new VitaCheatData() { Type = 10, Name = "(<) Less than X (16bit)" },
                new VitaCheatData() { Type = 11, Name = "(<) Less than X (32bit)" }
            };
            comboVitaCheatCondition.DataSource = ConditionalType;
            comboVitaCheatCondition.DisplayMember = "Name";
            comboVitaCheatCondition.ValueMember = "Type";

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
            comboVitaCheatB200.SelectedIndex = 0;
            comboVitaCheatPointerLevel.SelectedIndex = 0;
            comboVitaCheatButtonType.SelectedIndex = 0;
            comboVitaCheatButton.SelectedIndex = 0;
            comboVitaCheatButton2.SelectedIndex = 0;
        }

        //
        //
        // Pointer Searcher Tab starts here
        //
        //
        private PointerSearcher memdump;
        private PointerSearcher memdump2;
        private PointerSearcher memdump3;
        private PointerSearcher memdump4;
        private PointerSearcher memdump5;
        private PointerSearcher memdump6;
        private uint memory_start;

        private void BtnPointerSearcherFindPointers_Click(object sender, EventArgs e)
        {
            uint address1 = Utils.ParseNum(txtPointerSearcherAddress1.Text, NumberStyles.AllowHexSpecifier);
            uint maxOffset = Utils.ParseNum(txtPointerSearcherMaxOffset.Text, NumberStyles.AllowHexSpecifier);
            memory_start = Utils.ParseNum(txtBaseAddress.Text, NumberStyles.AllowHexSpecifier);

            memdump  = new PointerSearcher(txtPointerSearcherMemDump1.Text, memory_start);
            memdump2 = new PointerSearcher(txtPointerSearcherMemDump2.Text, memory_start);
            memdump3 = new PointerSearcher(txtPointerSearcherMemDump3.Text, memory_start);
            memdump4 = new PointerSearcher(txtPointerSearcherMemDump4.Text, memory_start);
            memdump5 = new PointerSearcher(txtPointerSearcherMemDump5.Text, memory_start);
            memdump6 = new PointerSearcher(txtPointerSearcherMemDump6.Text, memory_start);

            ResetPointerCounts();

            treePointerSearcherPointers.BeginUpdate();
            treePointerSearcherPointers.Nodes.Clear();
            AddPointerTree(memdump.FindPointers(address1, maxOffset), treePointerSearcherPointers.SelectedNode);
            treePointerSearcherPointers.EndUpdate();
        }

        private void TreePointerSearcherPointers_DoubleClick(object sender, EventArgs e)
        {
            if (treePointerSearcherPointers.SelectedNode == null) return;

            var maxOffset = Utils.ParseNum(txtPointerSearcherMaxOffset.Text, NumberStyles.AllowHexSpecifier);
            treePointerSearcherPointers.SelectedNode.Nodes.Clear();
            AddPointerTree(memdump.FindPointers(new PointerSearcherLog(treePointerSearcherPointers.SelectedNode.Text, memory_start).Address, maxOffset), treePointerSearcherPointers.SelectedNode);
        }

        private void BtnPointerSearcherClear_Click(object sender, EventArgs e)
        {
            treePointerSearcherPointers.BeginUpdate();
            treePointerSearcherPointers.Nodes.Clear();
            treePointerSearcherPointers.EndUpdate();
            ResetPointerCounts();
        }

        private void ResetPointerCounts()
        {
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
                Color color = Color.Black;
                Color rootedColor = Color.Transparent;
                int PointerColor = 0;
                PointerColor = SearchAdditionalDumps(pointers, memdump2, txtPointerSearcherAddress2.Text, index1, PointerColor);
                PointerColor = SearchAdditionalDumps(pointers, memdump3, txtPointerSearcherAddress3.Text, index1, PointerColor);
                PointerColor = SearchAdditionalDumps(pointers, memdump4, txtPointerSearcherAddress4.Text, index1, PointerColor);
                PointerColor = SearchAdditionalDumps(pointers, memdump5, txtPointerSearcherAddress5.Text, index1, PointerColor);
                PointerColor = SearchAdditionalDumps(pointers, memdump6, txtPointerSearcherAddress6.Text, index1, PointerColor);

                switch (PointerColor)
                {
                    case 0:
                        color = Color.Black;
                        PointerBlk += 1;
                        //txtColorBlack.Text = PointerBlk.ToString();
                        break;

                    case 1:
                        color = Color.Green;
                        PointerGrn += 1;
                        //txtColorGreen.Text = PointerGrn.ToString();
                        break;

                    case 2:
                        color = Color.Blue;
                        PointerBlu += 1;
                        //txtColorBlue.Text = PointerBlu.ToString();
                        break;

                    case 3:
                        color = Color.DarkOrchid;
                        PointerPur += 1;
                        //txtColorOrchid.Text = PointerPur.ToString();
                        break;

                    case 4:
                        color = Color.Red;
                        PointerRed += 1;
                        //txtColorRed.Text = PointerRed.ToString();
                        break;

                    case 5:
                        color = Color.Chocolate;
                        PointerOrn += 1;
                        //txtColorOrange.Text = PointerOrn.ToString();
                        break;

                    default:
                        break;
                }

                string seg0Start = txtPointerSearcherSeg0Addr.Text;
                string seg0Range = txtPointerSearcherSeg0Range.Text;
                string seg1Start = txtPointerSearcherSeg1Addr.Text;
                string seg1Range = txtPointerSearcherSeg1Range.Text;
                string vitaCheatStart = txtPointerSearcherVitaCheatSeg1Address.Text;
                string vitaCheatRange = txtPointerSearcherVitaCheatSeg1Range.Text;

                if (Utils.CheckInsideSegments(pointers[index1].Address, seg0Start, seg0Range) ||
                    Utils.CheckInsideSegments(pointers[index1].Address, seg1Start, seg1Range))
                {
                    rootedColor = Color.PowderBlue;
                }

                if (Utils.CheckInsideSegments(pointers[index1].Address, vitaCheatStart, vitaCheatRange))
                {
                    if (chkIgnoreDBFiles.Checked) { continue; }
                    rootedColor = Color.Black;
                    color = Color.White;
                }

                if (!pointers[index1].Negative || chkPointerSearcherIncludeNegatives.Checked)
                {
                    var node = new TreeNode
                    {
                        Text = pointers[index1].ToString(chkPointerSearcherRealAddresses.Checked ? 0U : memory_start),
                        ForeColor = color,
                        BackColor = rootedColor
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

            TotalPointersPerColor();
        }

        private void TotalPointersPerColor()
        {
            txtColorBlack.Text  = PointerBlk.ToString();
            txtColorGreen.Text  = PointerGrn.ToString();
            txtColorBlue.Text   = PointerBlu.ToString();
            txtColorOrchid.Text = PointerPur.ToString();
            txtColorRed.Text    = PointerRed.ToString();
            txtColorOrange.Text = PointerOrn.ToString();
        }

        private int SearchAdditionalDumps(List<PointerSearcherLog> pointers, PointerSearcher dump, string txtAddress, int index1, int PointerColor)
        {
            if (!String.IsNullOrEmpty(txtAddress))
            {
                string[] strArray = ((treePointerSearcherPointers.SelectedNode == null ? "" : treePointerSearcherPointers.SelectedNode.FullPath + "\\") + pointers[index1]).Split('\\');
                uint num = Utils.ParseNum(txtAddress, NumberStyles.AllowHexSpecifier);
                if (num < memory_start) num += memory_start;
                uint address = 0u;
                for (int index2 = 0; index2 < strArray.Length; ++index2)
                {
                    PointerSearcherLog pointerSearcherLog = new PointerSearcherLog(strArray[strArray.Length - 1 - index2], memory_start);
                    if (index2 == 0) address = pointerSearcherLog.Address;
                    address = dump.GetPointerAddress(address, pointerSearcherLog.Offset, pointerSearcherLog.Negative);
                }
                if ((int)num == (int)address) PointerColor += 1;
            }

            return PointerColor;
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
            return (!chkPointerSearcherRAWCode.Checked ? "" : "::Generated Code\n") + str2;

            

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

            uint targetAddress = pointers[0].Address;
            string strB200 = "";
            GenerateB200(pointers, ref targetAddress, ref strB200);

            str2 = !pointers[0].Negative ? $"{str2}$3{bittype:X01}{pointers.Count:X02} {targetAddress:X08} {pointers[0].Offset:X08}\n" + str1 : $"{str2}$3{bittype:X01}{pointers.Count:X02} {targetAddress:X08} {(0x100000000L - pointers[0].Offset):X08}\n{str1}";

            return (!chkPointerSearcherRAWCode.Checked ? $"{strB200}{str2}{str3}" : $"_V0 Generated Code\n{strB200}{str2}{str3}");
        }

        private void GenerateB200(List<PointerSearcherLog> pointers, ref uint targetAddress, ref string strB200)
        {
            if (chkPointerSearcherB200.Checked)
            {
                string seg0Start = txtPointerSearcherSeg0Addr.Text;
                string seg0Range = txtPointerSearcherSeg0Range.Text;
                string seg1Start = txtPointerSearcherSeg1Addr.Text;
                string seg1Range = txtPointerSearcherSeg1Range.Text;

                if (Utils.CheckInsideSegments(pointers[0].Address, seg0Start, seg0Range))
                {
                    strB200 = $"$B200 00000000 00000000\n";
                    targetAddress -= Utils.ParseNum(seg0Start, NumberStyles.AllowHexSpecifier);
                }
                else if (Utils.CheckInsideSegments(pointers[0].Address, seg1Start, seg1Range))
                {
                    strB200 = $"$B200 00000001 00000000\n";
                    targetAddress -= Utils.ParseNum(seg1Start, NumberStyles.AllowHexSpecifier);
                }
            }
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
                str1 = index % 2 != 0 ? str1 + $" 0x{(pointers[index].Negative ? 3 : 2):X01}{pointers[index].Offset:X07}\n" : $"{str1}{(!chkPointerSearcherRAWCode.Checked ? "" : "_L ")}0x{(pointers[index].Negative ? 3 : 2):X01}{pointers[index].Offset:X07}";
            }

            if (pointers.Count % 2 == 0) str1 += string.Format(" 0x00000000");

            var str2 = $"{(!chkPointerSearcherRAWCode.Checked ? "" : "_L ")}0x6{(uint)((int)pointers[0].Address - (int)memory_start):X07} 0x{value:X08}\n{(!chkPointerSearcherRAWCode.Checked ? "" : "_L ")}0x000{bittype:X01}{pointers.Count:X04} 0x{pointers[pointers.Count - 1].Offset:X08}\n{str1}";

            return (!chkPointerSearcherRAWCode.Checked ? $"{str2}" : $"_C0 Generated Code\n{str2}");
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
        private void TxtVCInstructions_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void ComboVitaCheatButtonType_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (comboVitaCheatButtonType.SelectedValue)
            {
                case 9:
                    comboVitaCheatButton.Enabled = false;
                    comboVitaCheatButton2.Enabled = false;
                    break;

                default:
                    comboVitaCheatButton.Enabled = true;
                    comboVitaCheatButton2.Enabled = true;
                    break;
            }
        }

        private void ComboVitaCheatCondition_SelectedValueChanged(object sender, EventArgs e)
        {
            switch (comboVitaCheatCondition.SelectedValue)
            {
                case 99:
                    txtVitaCheatCondAddr.Enabled = false;
                    txtVitaCheatCondValue.Enabled = false;
                    break;

                default:
                    txtVitaCheatCondAddr.Enabled = true;
                    txtVitaCheatCondValue.Enabled = true;
                    break;

            }
        }

        private void ComboVitaCheatCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Git Wiki url
            var strWiki = "https://github.com/r0ah/vitacheat/wiki/";
            txtVitaCheatAddress2.Enabled = false;
            txtVitaCheatValue.Enabled = true;
            comboVitaCheatPointerLevel.Enabled = false;
            groupVitaCheatAddress1Offset.Enabled = false;
            groupVitaCheatAddress2Offset.Enabled = false;
            groupVitaCheatCompression.Enabled = false;
            pnlVitaCheatMain.Enabled = true;
            btnVitaCheatGenerate.Enabled = true;
            string strPage;
            switch (comboVitaCheatCodeType.Text)
            {
                case VC_GEN_WRITE: // Write
                    strPage = "Write";
                    txtVCInstructions.Text = "WRITE\r\nCreates a code that locks the value at an address to the specified number.\r\n\r\nPut the desired address in the 'Address 1' box and put your desired value in the box marked 'Desired Value'\r\n\r\nFor example, to lock your HP at 100, we need to put our HP's address (I'll use 83001337) into 'Address 1' and 100 into 'Desired Value'.\r\n\r\nThis generates the code:\r\n_V0 Infinite HP\r\n$0200 83001337 00000064\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_MOV: // MOV
                    strPage = "MOV";
                    txtVitaCheatAddress2.Enabled = true;
                    txtVitaCheatValue.Enabled = false;
                    txtVCInstructions.Text = "MOV\r\nMOV codes simply copies the value from one address to another.\r\n\r\nPut the address you want changed into 'Address 1' and the address that you want to copy from in 'Copy From'.\r\n\r\nExample:\r\nTo make an 'Always Full HP' code, we can put the address for our current HP (83001337) into 'Address 1'. Then put the address for our Max HP (83001333) into 'Copy From'. The code generator will give the following code:\r\n_V0 Always Full HP\r\n$5200 83001337 83001333\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_COMP: // Compress
                    strPage = "Compression";
                    groupVitaCheatCompression.Enabled = true;
                    numericVitaCheatCompressionLevelOffset.Enabled = false;
                    lblVitaCheatCompressionLevelOffset.Enabled = false;
                    txtVCInstructions.Text = "COMPRESS\r\nCompress is an advanced code that applies the 'Write' code several times in different places in an ordered manner.\r\n\r\nType the first address in the 'Address 1' box as well as the desired value in 'Desired Value'.\r\n\r\nFind out how far away the second address is. You can use a hex Calculator to subtract these two. Place that offset into the box labeled 'Address Gap'\r\n\r\nIf you would like to have the value increased each time the code is applied, use 'Value Gap' to increase it.\r\n\r\nFinally, select or type the number of times you need this code to repeat in the '# of Compressions' box.\r\n\r\nExample:\r\nTo give 99 of each potion type, we will first find the address for the 1st potion (We'll use 83001337) in the game and know how many potions there are. We'll pretend there are normal, greater and high potions, so 3 compressions total. The greater potion is at 83001347 and high is at 83001357. This puts the Value offset at 0x00000010. We want them all to be 99, so the desired value will be 99 and the Value Gap will remain 0. The generated code will then be:\r\n_V0 Infinite Potions\r\n$4200 83001337 00000063\r\n$0003 00000010 00000000\r\n\r\nThis has the same effect as the following code:\r\n_V0 Infinite Potions\r\n$0200 83001337 00000063\r\n$0200 83001347 00000063\r\n$0200 83001357 00000063\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_PNTR: // Pointer Write
                    strPage = "Pointer-Write";
                    comboVitaCheatPointerLevel.Enabled = true;
                    groupVitaCheatAddress1Offset.Enabled = true;
                    txtVCInstructions.Text = "POINTER Write\r\nPointers are advanced codes that write to addresses that move around.\r\n\r\nSometimes developers move blocks of RAM around.To keep track of this movement, a specific address keeps track of that block's starting point. The location of an address within that block is called an Offset and is the distance from the start of the block to the desired location. Often, that location is another pointer, leading to a new movable block. To follow a second, third or more pointers, use the pointer level.\r\n\r\nPut the pointer's Address into the 'Address 1' box.And the value you would like in the 'Desired Value' box.\r\n\r\nSelect how many pointers you need to follow in the 'Pointer Level' box and put each of their offsets into an offset box.The first offset is at the top, and the last offset is at the bottom.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_PTRMOV: // Pointer MOV
                    strPage = "Pointer-MOV";
                    comboVitaCheatPointerLevel.Enabled = true;
                    groupVitaCheatAddress1Offset.Enabled = true;
                    groupVitaCheatAddress2Offset.Enabled = true;
                    txtVitaCheatValue.Enabled = false;
                    txtVitaCheatAddress2.Enabled = true;
                    txtVCInstructions.Text = "POINTER MOV\r\nPointer MOV copies one address to another, but uses pointers as the starting points.\r\n\r\nExamples\r\n\r\nExample of a level 1 pointer MOV:\r\n\r\n_V0 Infinite MP\r\n$8201 818714E8 00000E0C\r\n$8800 00000000 00000000\r\n$8601 815715D9 00000FDC\r\n$8900 00000000 00000000\r\n\r\nCopy (32bit) value from 0x815715D9 + 0xFDC to 0x818714E8 + 0xE0C.\r\n\r\nExample of a level 3 pointer MOV:\r\n\r\n_V0 Infinite MP\r\n$8203 818714E8 00000E0C\r\n$8200 00000000 0000000D\r\n$8200 00000000 0000124D\r\n$8800 00000000 00000000\r\n$8603 815715D9 00000FDC\r\n$8600 00000000 0000000C\r\n$8600 00000000 00001255\r\n$8900 00000000 00000000\r\n\r\nCopy (32bit) value from 0x815715D9 + 0xFDC + 0x0C + 0x1255 to 0x818714E8 + 0xE0C + 0x0D + 0x124D.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_PTRCOM: // Pointer Compress
                    strPage = "Pointer-Compression";
                    comboVitaCheatPointerLevel.Enabled = true;
                    groupVitaCheatAddress1Offset.Enabled = true;
                    groupVitaCheatCompression.Enabled = true;
                    numericVitaCheatCompressionLevelOffset.Enabled = true;
                    lblVitaCheatCompressionLevelOffset.Enabled = true;
                    txtVCInstructions.Text = "POINTER COMPRESS\r\nCreates several Write codes in an ordered manner with a pointer as the starting point.\r\n\r\nMake sure to set which level you want the code to Compres at. Leaving it at '1' will apply the compression at the first offset.\r\nLeaving it at '0' will change the ADDRESS value by the selected offset gap, not it's offset.\r\n\r\nExample:\r\n\r\n_V0 Max Stats\r\n$7203 818714E8 00000FDC\r\n$7200 00000000 0000000C\r\n$7200 00000000 000005DD\r\n$7703 00000000 000003E6\r\n$0002 00000004 00000000\r\n\r\nCompress the third pointer offset which is 0x05DD with the address gap of 0x04. Number of compression is 0x02.\r\n\r\nThe code above is equivalent to:\r\n\r\n$3203 818714E8 00000FDC\r\n$3200 00000000 0000000C\r\n$3200 00000000 000005DD\r\n$3300 00000000 000003E6\r\n\r\n\r\n$3203 818714E8 00000FDC\r\n$3200 00000000 0000000C\r\n$3200 00000000 000005E1\r\n$3300 00000000 000003E6\r\n #INCREMENT ADDRESS GAP:`0x05DD`+`0x04`=`0x05E1`\r\n#VALUE IS STATIC 3E6\r\nC\r\nChange Address with known address gap\r\n\r\n_V0 WH Item Slot Modifier\r\n$7201 862BFEC8 0000001C\r\n$7700 00000000 00000032\r\n$0060 00000004 00000000\r\n\r\nCompress the pointer Address which is 0x862BFEC8 with the address gap of 0x04. Number of compression is 0x60.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_ARMWRT: // ARM Write
                    strPage = "ARM-Write";
                    txtVitaCheatAddress2.Enabled = true;
                    txtVitaCheatValue.Enabled = false;
                    txtVCInstructions.Text = "ARM Write\r\nWrite ARM instructions. ARM Write can also be used to store the original value of what is being overwritten, and restored when the code is deactivated.\r\n\r\nIt's like a switch for ON and OFF for a cheat to restore it to its former value.\r\n\r\nNote: The ARM architecture uses the Little-endian storage format.\r\n\r\nExample:\r\n\r\n_V0 EXTRA MAX\r\n$A100 8114C8A2 0000BF00\r\n\r\nWrite 0x00BF to 0x8114C8A2 (16bit), default is off.\r\n\r\n0x00BF means NOP in ARM (Thumb-2 HEX).\r\n\r\n_V1 Branch Test\r\n$A200 81132EA8 EA01D709\r\n\r\nWrite 0x75F012BE to 0x81132EA8 (32bit), default is on.\r\n\r\n0x09D701EA means b #0x75C24 in ARM (ARM GBD/LLBD).\r\n\r\n_V0 Walk Thru Walls\r\n$A000 810A12A6 00000001\r\n\r\n\r\nWrites 0x01 to address 0x810A12A6, Default is 0x00\r\nWhen the code is turned off, it is restored to the original value of 0x00.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_BTNPAD: // Button Pad
                    strPage = "Button-PAD";
                    pnlVitaCheatMain.Enabled = false;
                    btnVitaCheatGenerate.Enabled = false;
                    txtVCInstructions.Text = "Button PAD\r\nThis code type is useful when you want your code to be activated based on the button input.\r\n\r\nButton Type\r\n\r\n0000 - Undefined\r\n0001 - Vita (Default)\r\n0002 - PSTV\r\n0004 - DualShock 3\r\n0008 - DualShock 4\r\n\r\nButtons\r\n00000001 - psvita-select\r\n00000008 - psvita-start\r\n00000010 - psvita-up\r\n00000020 - psvita-right\r\n00000040 - psvita-down\r\n00000080 - psvita-left\r\n00000100 - psvita-L\r\n00000200 - psvita-R\r\n00001000 - psvita-triangle\r\n00002000 - psvita-circle\r\n00004000 - psvita-cross\r\n00008000 - psvita-square\r\n00000000 - null\r\n\r\nExample:\r\n\r\n_V0 Button PAD\r\n$C201 00000001 00000300\r\n$0200 8xxxxxxx xxxxxxxx\r\n\r\nPressing the psvita-L + psvita-R will execute the following 0x01 related lines of code $0200 8xxxxxxx xxxxxxxx.\r\n\r\nButton Combo\r\npsvita-square + psvita-select:\r\n\r\n_V0 Button PAD\r\n$C201 00000001 00008001\r\n$0200 8xxxxxxx xxxxxxxx\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_CNDTN: // Condition
                    strPage = "Condition";
                    pnlVitaCheatMain.Enabled = false;
                    btnVitaCheatGenerate.Enabled = false;
                    txtVCInstructions.Text = "Condition\r\nThe condition code type checks if the specific requirement is met then execute the code. Read Conditional (computer programming) for more details.\r\n\r\nCurrently only support static address.\r\n\r\nX: Operators\r\n0 - (=) Equal to X 8bit\r\n1 - (=) Equal to X 16bit\r\n2 - (=) Equal to X 32bit\r\n3 - (<>) Unequal to X (8bit)\r\n4 - (<>) Unequal to X (16bit)\r\n5 - (<>) Unequal to X (32bit)\r\n6 - (>) Greater than X (8bit)\r\n7 - (>) Greater than X (16bit)\r\n8 - (>) Greater than X (32bit)\r\n9 - (<) Less than X (8bit)\r\nA - (<) Less than X (16bit)\r\nB - (<) Less than X (32bit)\r\n\r\nExamples\r\n\r\nOperators: Equal to X (=)\r\n\r\n_V0 Condition Operators\r\n$D201 81000000 FFA8FF2D #CODE-TYPE IDENTIFIER\r\n$0200 8xxxxxxx xxxxxxxx #LINE #1\r\n\r\nIf 0x81000000's value is equal to 0xFFA8FF2D (32bit) then execute the following 0x01 related lines of code.\r\n\r\nOperators: Greater than X (>)\r\n\r\n__V0 Condition Operators\r\n$D605 81000000 00000005 #CODE-TYPE IDENTIFIER\r\n$0200 8xxxxxxx xxxxxxxx #LINE #1\r\n$0200 8xxxxxxx xxxxxxxx #LINE #2\r\n$0200 8xxxxxxx xxxxxxxx #LINE #3\r\n$0200 8xxxxxxx xxxxxxxx #LINE #4\r\n$0200 8xxxxxxx xxxxxxxx #LINE #5\r\n\r\nIf 0x81000000's value is greater than 0x00000005 (8bit) then execute the following 0x05 related lines of code.\r\n\r\nOperators: Less than X (<)\r\n\r\n_V0 Condition Operators\r\n$D90A 81000000 00000005 #CODE-TYPE IDENTIFIER\r\n$0200 8xxxxxxx xxxxxxxx #LINE #1\r\n$0200 8xxxxxxx xxxxxxxx #LINE #2\r\n$0200 8xxxxxxx xxxxxxxx #LINE #3\r\n$0200 8xxxxxxx xxxxxxxx #LINE #4\r\n$0200 8xxxxxxx xxxxxxxx #LINE #5\r\n$0200 8xxxxxxx xxxxxxxx #LINE #6\r\n$0200 8xxxxxxx xxxxxxxx #LINE #7\r\n$0200 8xxxxxxx xxxxxxxx #LINE #8\r\n$0200 8xxxxxxx xxxxxxxx #LINE #9\r\n$0200 8xxxxxxx xxxxxxxx #LINE #10\r\nIf 0x81000000's value is less than 0x00000005 (8bit) then execute the following 0x0A related lines of code.\r\n\r\nMore information at: " + strWiki + strPage;
                    break;

                case VC_GEN_B2COD: // B2 Code
                    strPage = "B2-Code";
                    pnlVitaCheatMain.Enabled = false;
                    btnVitaCheatGenerate.Enabled = false;
                    txtVCInstructions.Text = "B2 Code\r\nThis code type basically makes all address into relative. For example, the absolute address 0x816652E0 becomes a relative address 0x000652E0 and the base (segX) 0x816 is automatically obtained by VitaCheat. To view the segX information, browse the memory then press the R-Stick + Up button.\r\n\r\nSuper useful to find pointer addresses when cheat addresses are in the 0x81000000 - 0x83000000 range since 99% of the time it can be used to make pointer addresses without TempAR by just substractin the address of the found cheat and the Seg1 data.\r\n\r\nExample\r\n_V0 inf.HP Talis\r\n$B200 00000001 00000000\r\n$0200 00017B3C 0000270F\r\n\r\nThis was found by substracting the 81317B3C address - 81300000 Seg1 = 17B3C\r\n\r\nAlso this type of code solves the situation where the base of different version occurs.\r\n\r\nNote: The $B2 Code type does not function on Firmware 3.60. You must be using z05 or z06 (preferably) on 3.65 or VitaCheat will crash when used.\r\n\r\nExamples\r\nFor example: the mai version of the PCSH00181 Ys: Memories of Celceta has a different offset from the vitamin version.\r\n\r\nPCSH00181 伊苏树海-1.00-MAI5 by dask\r\n\r\n_V0 Money MAX\r\n$A100 810C6872 0000BF00\r\nSeg0:81000000-811F9188\r\n\r\n# PCSH00181 伊苏树海-1.00-vitamin by dask\r\n\r\nV0 Money MAX\r\n$A100 810C68D2 0000BF00\r\nSeg0:81000060-811F91E8\r\n\r\n\r\nYou can solve this problem with the B format code.\r\n\r\n# PCSH00181 伊苏树海-1.00 by dask\r\n\r\n_V0 Money MAX\r\n$B200 00000000 00000000\r\n$A100 000C6872 0000BF00\r\n\r\nThe above is an example of seg0. The application of seg1 can try Ninja Dragon Sword 2+ on its own. With $B200, it can be used in the European version (PCSB00294) and Hong Kong version (PCSG00157).\r\n\r\nMore information at: " + strWiki + strPage;
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
            CheckValidNumbers();

            //Declared variables for the code types
            string VCstr1 = "_V0 Generated Code\r\n\r\n";
            uint VCAddr1 = Utils.ParseNum(txtVitaCheatAddress1.Text, NumberStyles.AllowHexSpecifier);
            uint VCAddr2 = Utils.ParseNum(txtVitaCheatAddress2.Text, NumberStyles.AllowHexSpecifier);
            uint VCAddGp = Utils.ParseNum(txtVitaCheatAddressGap.Text, NumberStyles.AllowHexSpecifier);
            uint VCValGp = Utils.ParseNum(txtVitaCheatValueGap.Text, NumberStyles.AllowHexSpecifier);
            uint VCComps = Utils.ParseNum(numericVitaCheatCompressions.Text);
            uint VCValue = Utils.ParseNum(txtVitaCheatValue.Text);
            uint VCBtntype = Utils.ParseNum(comboVitaCheatButtonType.SelectedValue.ToString());
            uint VCBtn = Utils.ParseNum(comboVitaCheatButton.SelectedValue.ToString());
            uint VCBtn2 = Utils.ParseNum(comboVitaCheatButton2.SelectedValue.ToString());
            //uint VCRelLin = Utils.ParseNum("1");
            //uint VCSeg = Utils.ParseNum("1");
            //uint VCNull = Utils.ParseNum("0");

            //
            //Get Seg0/Seg1 State And Apply to VCstr1
            //
            switch (comboVitaCheatB200.Text)
            {
                case VC_GEN_B2_SEG0:
                    VCstr1 += "$B200 00000000 00000000\r\n";
                    break;

                case VC_GEN_B2_SEG1:
                    VCstr1 += "$B200 00000001 00000000\r\n";
                    break;

                case VC_GEN_B2_NONE:
                    break;
            }

            //
            // Get Conditional State and apply to VCstr1
            //
            int RelatedLines = Utils.RelatedLines(comboVitaCheatPointerLevel.SelectedIndex, comboVitaCheatCodeType.Text, VCBtntype, 1);
            if (comboVitaCheatCondition.Text != "None")
            {

                uint VCCondAddr    = Utils.ParseNum(txtVitaCheatCondAddr.Text, NumberStyles.AllowHexSpecifier);
                uint VCCondVal     = Utils.ParseNum(txtVitaCheatCondValue.Text);
                uint VCOperators   = Utils.ParseNum(comboVitaCheatCondition.SelectedValue.ToString());
                string VCGenCNDTN1 = $"$D{VCOperators:X01}{RelatedLines:X02} {VCCondAddr:X08} {VCCondVal:X08}\r\n";
                VCstr1 += VCGenCNDTN1;
            }

            //
            //Get Bit Type from radio buttons
            //
            int bittype = rdbVitaCheatBitType8Bit.Checked ? 0 : rdbVitaCheatBitType16Bit.Checked ? 1 : 2;

            //
            //Get Button Pad State And Apply to VCstr1
            //
            RelatedLines = Utils.RelatedLines(comboVitaCheatPointerLevel.SelectedIndex, comboVitaCheatCodeType.Text, VCBtntype, 0);
            switch (VCBtntype)
            {
                case 9:
                    break;

                default:
                    var VCBtnMath = VCBtn + VCBtn2;
                    VCstr1 += $"$C2{RelatedLines:X02} {VCBtntype:X08} {VCBtnMath:X08}\r\n";
                    break;
            }

            //
            // Generate code Types
            //
            switch (comboVitaCheatCodeType.Text)
            {
                case VC_GEN_WRITE:
                    var VCGenWrite1 = $"$0{bittype}00 {VCAddr1:X08} {VCValue:X08}\r\n";
                    txtVitaCheatCode.Text = VCstr1 + VCGenWrite1;
                    break;

                case VC_GEN_MOV:
                    var VCGenMov1 = string.Format("$5{0}00 {1:X08} {2:X08}\r\n", bittype, VCAddr1, VCAddr2);
                    txtVitaCheatCode.Text = VCstr1 + VCGenMov1;
                    break;

                case VC_GEN_COMP:
                    var VCGenComp1 = $"$4{bittype}00 {VCAddr1:X08} {VCValue:X08}\r\n";
                    var VCGenComp2 = $"${VCComps:X04} {VCAddGp:X08} {VCValGp:X08}\r\n";
                    txtVitaCheatCode.Text = VCstr1 + VCGenComp1 + VCGenComp2;
                    break;

                case VC_GEN_PNTR:
                    var VCGenPtrstr2 = "";
                    var VCGenptroff1 = Utils.ParseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtr1 = $"$3{bittype}0{comboVitaCheatPointerLevel.Text} {VCAddr1:X08} {VCGenptroff1:X08}\r\n";
                    var VCGenPtr3 = $"$3300 00000000 {VCValue:X08}";

                    foreach (Control x in groupVitaCheatAddress1Offset.Controls)
                    {
                        if (x is TextBox box && x.Enabled)
                        {
                            var VCGenptr2 = Utils.ParseNum(box.Text, NumberStyles.AllowHexSpecifier);
                            if (box.TabIndex != 0)
                            {
                                VCGenPtrstr2 = $"$3{bittype}00 00000000 {VCGenptr2:X08}\r\n{VCGenPtrstr2}";
                            }
                        }
                    }
                    txtVitaCheatCode.Text = VCstr1 + VCGenPtr1 + VCGenPtrstr2 + VCGenPtr3;
                    break;

                case VC_GEN_PTRMOV:
                    var VCGenPtrMovstr2 = "";
                    var VCGenptrmovoff1 = Utils.ParseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtrMov1 = $"$8{bittype}0{comboVitaCheatPointerLevel.Text} {VCAddr1:X08} {VCGenptrmovoff1:X08}\r\n";
                    var VCGenPtrMov3 = string.Format("$8800 00000000 00000000\r\n");
                    foreach (Control x in groupVitaCheatAddress1Offset.Controls)
                    {
                        if (x is TextBox box && x.Enabled)
                        {
                            var VCGenptrmov2 = Utils.ParseNum(box.Text, NumberStyles.AllowHexSpecifier);
                            if (box.TabIndex != 0)
                            {
                                VCGenPtrMovstr2 = $"$8{bittype}00 00000000 {VCGenptrmov2:X08}\r\n{VCGenPtrMovstr2}";
                            }
                        }
                    }
                    var VCGenPtr2str2 = "";
                    var VCGenptrmov2off1 = Utils.ParseNum(txtVitaCheatAddress2Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtrMov21 = $"$8{bittype + 4}0{comboVitaCheatPointerLevel.Text} {VCAddr2:X08} {VCGenptrmov2off1:X08}\r\n";
                    var VCGenPtrMov23 = string.Format("$8900 00000000 00000000");
                    foreach (Control x in groupVitaCheatAddress2Offset.Controls)
                    {
                        if (x is TextBox box && x.Enabled)
                        {
                            uint VCGenptrmov22 = Utils.ParseNum(box.Text, NumberStyles.AllowHexSpecifier);
                            if (box.TabIndex != 0)
                            {
                                VCGenPtr2str2 = $"$8{bittype + 4}00 00000000 {VCGenptrmov22:X08}\r\n{VCGenPtr2str2}";
                            }
                        }
                    }
                    txtVitaCheatCode.Text = VCstr1 + VCGenPtrMov1 + VCGenPtrMovstr2 + VCGenPtrMov3 + VCGenPtrMov21 + VCGenPtr2str2 + VCGenPtrMov23;
                    break;

                default:
                    break;

                case VC_GEN_PTRCOM:
                    var VCGenPtrComstr2 = "";
                    var VCGenptrcomoff1 = Utils.ParseNum(txtVitaCheatAddress1Offset1.Text, NumberStyles.AllowHexSpecifier);
                    var VCGenPtrCom1 = $"$7{bittype}0{comboVitaCheatPointerLevel.Text} {VCAddr1:X08} {VCGenptrcomoff1:X08}\r\n";
                    var VCGenPtrCom3 = $"$770{numericVitaCheatCompressionLevelOffset.Text} 00000000 {VCValue:X08}\r\n";
                    var VCGenPtrCom4 = $"${VCComps:X04} 0000{VCAddGp:X04} 0000{VCValGp:X04}";
                    foreach (Control x in groupVitaCheatAddress1Offset.Controls)
                    {
                        if (x is TextBox box && x.Enabled)
                        {
                            var VCGenptr2 = Utils.ParseNum(box.Text, NumberStyles.AllowHexSpecifier);
                            if (box.TabIndex != 0)
                            {
                                VCGenPtrComstr2 = $"$7{bittype}00 00000000 {VCGenptr2:X08}\r\n{VCGenPtrComstr2}";
                            }
                        }
                    }
                    txtVitaCheatCode.Text = VCstr1 + VCGenPtrCom1 + VCGenPtrComstr2 + VCGenPtrCom3 + VCGenPtrCom4;
                    break;

                case VC_GEN_ARMWRT:
                    var VCGenARMWRT1 = $"$A{bittype}00 {VCAddr1:X08} {VCAddr2:X08}\r\n";
                    txtVitaCheatCode.Text = VCstr1 + VCGenARMWRT1;
                    break;
            }
        }

        private void CheckValidNumbers()
        {
            //
            //Check for hex numbers and give error if bad
            //
            Utils.ParseNum(txtVitaCheatAddress1.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Address 1, make sure address is a valid hexadecimal number.");
            Utils.ParseNum(txtVitaCheatAddress2.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Address 2 (Copy from), make sure address is a valid hexadecimal number.");
            Utils.ParseNum(txtVitaCheatAddressGap.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Address Gap, make sure value is a valid hexadecimal number.");
            Utils.ParseNum(txtVitaCheatValueGap.Text, NumberStyles.AllowHexSpecifier, "Unable to parse Value Gap, make sure value is a valid hexadecimal number.");
            Utils.ParseNum(numericVitaCheatCompressions.Text, NumberStyles.AllowHexSpecifier, "You shouldn't be seeing this error! My bad, dude. Error: Compressions thingy is fucked.");
            Utils.ParseGroupNums(groupVitaCheatAddress1Offset);
            Utils.ParseGroupNums(groupVitaCheatAddress2Offset);
        }
    }
}