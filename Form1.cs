using System.Data;
using System.Text;
using System.Xml;

namespace MNIST
{

    public partial class Form1 : Form
    {
        private byte[] labels = new byte[60000];
        private int Index = 0;
        private int PicNum = 0;
        private int IndexNum = 0;
        private int Num = 0;
        private byte[,,] pics = new byte[60000, 28, 28];
        private Label[,] cells = new Label[28, 28];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int sz = 31;
            this.Top = 10;

            for (int y = 0; y < 28; y++)
                for (int x = 0; x < 28; x++)
                {
                    Label l = new Label();
                    l.Size = new Size(sz, sz);
                    l.Text = (y * 28 + x).ToString("D2");
                    l.Location = new Point(10 + x * sz, 70 + y * sz);
                    l.BorderStyle = BorderStyle.FixedSingle;
                    l.TextAlign = ContentAlignment.MiddleCenter;
                    cells[x, y] = l;
                    Controls.Add(l);
                }

            toolStripStatusLabel1.Text = "";
        }

        private void labelsFileOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (fs.Length == 10008 || fs.Length == 60008)
                    {
                        for (int idx = 0; idx < 60000; idx++)
                        {
                            labels[idx] = 0;
                        }
                        fs.Position = 8;
                        for (int idx = 0; idx < 60000; idx++)
                        {
                            int ret = fs.ReadByte();
                            if (ret == -1)
                            {
                                break;
                            }
                            labels[idx] = (byte)ret;
                            IndexNum = idx;
                        }
                        Index = 0;
                        UpdateStatus();
                    }
                    else
                    {
                        MessageBox.Show("File Size Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        private void UpdateCells()
        {
            for (int y = 0; y < 28; y++)
            {
                for (int x = 0; x < 28; x++)
                {
                    int level = pics[Index, x, y];
                    if (level > 128)
                        cells[x, y].ForeColor = Color.Black;
                    else
                        cells[x, y].ForeColor = Color.White;
                    cells[x, y].Text = level.ToString("X2");
                    cells[x, y].BackColor = Color.FromArgb(level, level, level);

                }
            }
        }
        private void UpdateStatus()
        {
            Num = Math.Min(IndexNum, PicNum);
            toolStripStatusLabel1.Text = "Index=" + Index + " Num=" + Num + " Label=" + labels[Index];
            textBox1.Text = Index.ToString();
            UpdateCells();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (fs.Length == 7840016 || fs.Length == 47040016)
                    {
                        for (int idx = 0; idx < 60000; idx++)
                            for (int x = 0; x < 28; x++)
                                for (int y = 0; y < 28; y++)
                                    pics[idx, x, y] = 0;



                        fs.Position = 16;
                        bool run = true;
                        for (int idx = 0; idx < 60000; idx++)
                        {
                            if (run == false) break;
                            for (int y = 0; y < 28; y++)
                            {
                                if (run == false) break;
                                for (int x = 0; x < 28; x++)
                                {
                                    int ret = fs.ReadByte();
                                    if (ret == -1)
                                    {
                                        run = false;
                                        break;
                                    }
                                    pics[idx, x, y] = (byte)ret;
                                }
                            }
                            PicNum = idx;
                        }
                        Index = 0;
                        UpdateStatus();
                    }
                    else
                    {
                        MessageBox.Show("File Size Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (Index < Num)
            {
                Index++;
                UpdateStatus();
            }
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (Index > 0)
            {
                --Index;
                UpdateStatus();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( e.KeyChar!=8 &&( e.KeyChar < '0' || '9' < e.KeyChar))
            {
                //押されたキーが 0～9でない場合は、イベントをキャンセルする
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int idx;
            try
            {
                idx = Int32.Parse(textBox1.Text);
                if (idx < 0 || idx > Num) return;
            }
            catch (Exception _)
            {
                return;
            }
            Index = idx;
            UpdateStatus();
        }
    }
}
