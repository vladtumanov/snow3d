using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Snow
{
    public partial class Main : Form
    {
        BackgroundWorker bw = new BackgroundWorker();

        public Main()
        {
            InitializeComponent();
            SettingsComponent();
            SettingsBackgroundWorker();
        }

        private void SettingsComponent()
        {
            textBox1.Text = @"C:\LData\untitled.txt";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[0].Cells[0].Value = "2";
            dataGridView1.Rows[0].Cells[1].Value = "2";
            dataGridView1.Rows[0].Cells[2].Value = "0.24";
            dataGridView1.Rows[0].Cells[3].Value = "1.0";
            dataGridView1.Rows[0].Cells[4].Value = "1.0";
            dataGridView1.Rows[0].Cells[5].Value = "42";
            dataGridView1.Rows[0].Cells[7].Value = "0.05";
            dataGridView1.Rows[0].Cells[8].Value = "0";
            dataGridView1.Rows[0].Cells[9].Value = "1 2 3";
            dataGridView1.Rows[0].Cells[10].Value = "0 0 0";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[1].Cells[0].Value = "3";
            dataGridView1.Rows[1].Cells[1].Value = "2";
            dataGridView1.Rows[1].Cells[2].Value = "0.24";
            dataGridView1.Rows[1].Cells[3].Value = "1.0";
            dataGridView1.Rows[1].Cells[4].Value = "1.0";
            dataGridView1.Rows[1].Cells[5].Value = "42";
            dataGridView1.Rows[1].Cells[7].Value = "0.05";
            dataGridView1.Rows[1].Cells[8].Value = "90";
            dataGridView1.Rows[1].Cells[9].Value = "1 2 3";
            dataGridView1.Rows[1].Cells[10].Value = "0 0 0";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[2].Cells[0].Value = "4";
            dataGridView1.Rows[2].Cells[1].Value = "2";
            dataGridView1.Rows[2].Cells[2].Value = "0.24";
            dataGridView1.Rows[2].Cells[3].Value = "1.0";
            dataGridView1.Rows[2].Cells[4].Value = "1.0";
            dataGridView1.Rows[2].Cells[5].Value = "42";
            dataGridView1.Rows[2].Cells[7].Value = "0.05";
            dataGridView1.Rows[2].Cells[8].Value = "180";
            dataGridView1.Rows[2].Cells[9].Value = "1 2 3";
            dataGridView1.Rows[2].Cells[10].Value = "0 0 0";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[3].Cells[0].Value = "5";
            dataGridView1.Rows[3].Cells[1].Value = "2";
            dataGridView1.Rows[3].Cells[2].Value = "0.24";
            dataGridView1.Rows[3].Cells[3].Value = "1.0";
            dataGridView1.Rows[3].Cells[4].Value = "1.0";
            dataGridView1.Rows[3].Cells[5].Value = "42";
            dataGridView1.Rows[3].Cells[7].Value = "0.05";
            dataGridView1.Rows[3].Cells[8].Value = "270";
            dataGridView1.Rows[3].Cells[9].Value = "1 2 3";
            dataGridView1.Rows[3].Cells[10].Value = "0 0 0";

            dataGridView1.Rows.Add();
            dataGridView1.Rows[4].Cells[0].Value = "6";
            dataGridView1.Rows[4].Cells[1].Value = "2";
            dataGridView1.Rows[4].Cells[2].Value = "0.24";
            dataGridView1.Rows[4].Cells[3].Value = "1.0";
            dataGridView1.Rows[4].Cells[4].Value = "1.0";
            dataGridView1.Rows[4].Cells[5].Value = "42";
            dataGridView1.Rows[4].Cells[7].Value = "0.05";
            dataGridView1.Rows[4].Cells[8].Value = "360";
            dataGridView1.Rows[4].Cells[9].Value = "1 2 3";
            dataGridView1.Rows[4].Cells[10].Value = "0 0 0";

            comboBox4.Items.Add("42 Пластины (3 узла)");
            comboBox4.Items.Add("44 Пластины (4 узла)");

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;

            //\u2554╔ 2550═ 2566╦ 2557╗ 2551║ 2560╠  2563╣ 256C╬  255A╚ 255D╝ 256B╫ 255F╟  2562╢ 2500─

            richTextBox1.AppendText(">>> Добро пожаловать!\n" +
                ">>> Примечание: ввод чисел в нужные поля осуществляется через пробел (11 12 13 14)\n" +
                ">>> Примечание: в качестве десятичного разделителя используется точка! (10.01234)\n" +
                ">>> Примечание: радиус купола определяется по формуле R = (x^2 + y^2 + z^2)^1/2\n" +
                ">>> Примечание: расчёт нагрузки выполняется по формуле So = 0.7·Ce·Ct·\u00B5·Sg и приложению Г.13\n>>> ");
            toolStripLabel1.Text = "";
        }

        private void SettingsBackgroundWorker()
        {
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += (s, e) => e.Cancel = Start(bw);
            bw.ProgressChanged += (s, e) => DisplayProgress(e.ProgressPercentage);
            bw.RunWorkerCompleted += (s, e) => DisplayResult(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "Файл процессора|*.txt";
                openFileDialog1.Title = "Выберите файл процессора";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog1.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells["Number"].Value = numericUpDown1.Value;
            dataGridView1.Rows[index].Cells["Variant"].Value = comboBox3.Text.Split(' ')[1];
            dataGridView1.Rows[index].Cells["Sg"].Value = textBox4.Text;
            dataGridView1.Rows[index].Cells["Ce"].Value = textBox5.Text;
            dataGridView1.Rows[index].Cells["Ct"].Value = textBox6.Text;
            if (checkBox1.Checked) dataGridView1.Rows[index].Cells["FType"].Value = comboBox4.Text.Split(' ')[0];
            if (checkBox2.Checked) dataGridView1.Rows[index].Cells["FEJ"].Value = textBox7.Text;
            dataGridView1.Rows[index].Cells["fd"].Value = textBox2.Text;
            dataGridView1.Rows[index].Cells["Gamma"].Value = textBox8.Text;
            dataGridView1.Rows[index].Cells["Axes"].Value = comboBox1.Text.Replace('X', '1').Replace('Y', '2').Replace('Z', '3');
            dataGridView1.Rows[index].Cells["Center"].Value = textBox3.Text;

            numericUpDown1.UpButton();
            richTextBox1.AppendText("Сценарий добавлен\n>>> ");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count != 0)
            {
                string[] a = new string[11];
                int r = dataGridView1.SelectedCells[0].RowIndex;

                for (int i = 0; i < 11; i++)
                    if (dataGridView1[i, r].Value != null) a[i] = dataGridView1[i, r].Value.ToString();
                richTextBox1.AppendText("Сценарий удалён: " + string.Join("; ", a) + "\n>>> ");
                dataGridView1.Rows.RemoveAt(r);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void toolStripButton2_Click(object sender, EventArgs h)
        {
            toolStripButton2.Text = "Подождите...";
            toolStripButton2.Enabled = false;

            if (bw.IsBusy) bw.CancelAsync();
            else
            {
                toolStripProgressBar1.Maximum = dataGridView1.Rows.Count;
                bw.RunWorkerAsync();
            }
        }

        private void DisplayProgress(int percent)
        {
            toolStripLabel1.Text = "Вычисление нагрузки " + dataGridView1[0, percent].Value;
            toolStripProgressBar1.Value = percent + 1;
            toolStripButton2.Enabled = true;
            toolStripButton2.Text = "Остановить";
        }

        private void DisplayResult(RunWorkerCompletedEventArgs e)
        {
            toolStripButton2.Enabled = true;
            toolStripButton2.Text = "Выполнить";
            toolStripLabel1.Text = "";
            toolStripProgressBar1.Value = 0;

            richTextBox1.AppendText(
                e.Cancelled ?
                "Задание отменено! Изменения не сохранены.\n>>> " :
                "Файл обновлён: " + textBox1.Text + "\n>>> Задание выполнено!\n>>> ");
        }

        private bool Start(BackgroundWorker bw)
        {
            Lira lira = new Lira(textBox1.Text);
            int ftype = 0;
            int fej = 0;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (bw.CancellationPending) return true;
                bw.ReportProgress(item.Index);
                lira.Number = Convert.ToInt32(item.Cells["Number"].Value);
                lira.Variant = Convert.ToInt32(item.Cells["Variant"].Value);
                lira.Sg = Convert.ToDouble(item.Cells["Sg"].Value);
                lira.Ce = Convert.ToDouble(item.Cells["Ce"].Value);
                lira.Ct = Convert.ToDouble(item.Cells["Ct"].Value);
                if (item.Cells["FType"].Value != null) ftype = Convert.ToInt32(item.Cells["FType"].Value);
                if (item.Cells["FEJ"].Value != null) fej = Convert.ToInt32(item.Cells["FEJ"].Value.ToString().Split(' ')[0]);
                lira.fd = Convert.ToDouble(item.Cells["fd"].Value);
                lira.Axes = item.Cells["Axes"].Value.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x) - 1).ToArray();
                lira.Center = item.Cells["Center"].Value.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToDouble(x)).ToArray();
                lira.Gamma = Convert.ToDouble(item.Cells["Gamma"].Value) - 90;
                lira.Run(ftype, fej);
            }
            lira.Save(textBox1.Text);
            return false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0: textBox4.Text = "0.08"; break;
                case 1: textBox4.Text = "0.12"; break;
                case 2: textBox4.Text = "0.18"; break;
                case 3: textBox4.Text = "0.24"; break;
                case 4: textBox4.Text = "0.32"; break;
                case 5: textBox4.Text = "0.40"; break;
                case 6: textBox4.Text = "0.48"; break;
                case 7: textBox4.Text = "0.56"; break;
            }
        }
    }
}
