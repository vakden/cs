using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace OS_1A
{
    public partial class Form1 : Form
    {
        public string fileName;
        public string directory;
        public int depth = 1;
        public bool searchDirectoryes = false;
        public bool searchFiles = false;
        public List<string> MyListFirst = new List<string>();
        public List<string> MyListSecond = new List<string>();
        public Thread first, second;

        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            searchDirectoryes = checkBox1.AutoCheck;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            searchFiles = checkBox2.AutoCheck;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fileName = textBox1.Text;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MyListSecond.Clear();
            MyListFirst.Clear();
            richTextBox2.Clear();
            try
            {
                if (directory == null || fileName == null)
                {
                    richTextBox2.AppendText("Введите данные!\n");
                    return;
                }
                if (!searchFiles && !searchDirectoryes)
                {
                    richTextBox2.AppendText("Вы не указали тип искомых объектов!\n");
                    return;
                }

                find(depth, directory);
            }
            catch (Exception ex)
            {
                first.Abort();
                second.Abort();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            directory = textBox2.Text;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            depth = int.Parse(domainUpDown1.Text);
        }

        private void find(int dep, string dir)
        {
            int d = dep;
            if (dep != 0 && (searchFiles || searchDirectoryes))
            {
                first = new Thread((() =>
                {
                    SecondThread(dir,dep, richTextBox2);
                }));
                second = new Thread((() =>
                {
                    FirstTread(dir, dep, richTextBox2);
                }));
                first.Priority = ThreadPriority.Normal;
                second.Priority = ThreadPriority.Normal;
                first.Start();
                second.Start();
                string[] allfiles = Directory.GetDirectories(dir);
                dep--;
                foreach (string file in allfiles)
                {
                    find(dep,file);
                }

                if (d == 0)
                {
                    if (MyListFirst.Count == 0)
                    {
                        Invoke(new Action(() => richTextBox2.AppendText("Первый поток ничего не нашёл\n")));
                    }


                    if (MyListSecond.Count == 0)
                    {
                        Invoke(new Action(() => richTextBox2.AppendText("Второй поток ничего не нашёл\n")));
                    }
                }
            }
        }

        public void FirstTread(string dir, int dep, RichTextBox box)
        {
            try
            {
                if (searchDirectoryes)
                {
                    string[] alldir = Directory.GetDirectories(dir);
                    foreach (string file in alldir)
                    {
                        string fileCheck = file.Substring(file.LastIndexOf("\\"));
                        if (fileCheck.IndexOf(fileName) >= 0)
                        {
                            MyListFirst.Add(file);
                            Invoke(new Action(() => box.AppendText("(Найдено 1-ым потоком)\t" + file + "\n")));
                        }

                    }
                }

                if (searchFiles)
                {
                    string[] allfiles = Directory.GetFiles(dir);
                    foreach (string file in allfiles)
                    {
                        string fileCheck = file.Substring(file.LastIndexOf("\\"));
                        if (fileCheck.IndexOf(fileName) >= 0)
                        {
                            MyListFirst.Add(file);
                            Invoke(new Action(() => box.AppendText("(Найдено 1-ым потоком)\t" + file + "\n")));
                        }

                    }
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Invoke(new Action(() => richTextBox2.AppendText("Попытка доступа в системные файлы, работа 1-го потока в директории " + dir +" прекращена!\n")));
                if (first.IsAlive && second.IsAlive)
                {
                    first.Abort();
                }
            }

        }
        public void SecondThread(string dir, int dep, RichTextBox box)
        {
            try
            {
                if (searchDirectoryes)
                {
                    string[] alldir = Directory.GetDirectories(dir);
                    foreach (string file in alldir)
                    {
                        string fileCheck = file.Substring(file.LastIndexOf("\\"));
                        if (fileCheck.IndexOf(fileName) >= 0)
                        {
                            MyListSecond.Add(file);
                            Invoke(new Action(() => box.AppendText("(Найдено 2-ым потоком)\t" + file + "\n")));
                        }
                    }
                }

                if (searchFiles)
                {
                    string[] allfiles = Directory.GetFiles(dir);
                    foreach (string file in allfiles)
                    {
                        string fileCheck = file.Substring(file.LastIndexOf("\\"));
                        if (fileCheck.IndexOf(fileName) >= 0)
                        {
                            MyListSecond.Add(file);
                            Invoke(new Action(() => box.AppendText("(Найдено 2-ым потоком)\t" + file + "\n")));
                        }
                    }
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Invoke(new Action(() => richTextBox2.AppendText("Попытка доступа в системные файлы, работа 2-го потока в директории " + dir + " прекращена!\n")));
                if (first.IsAlive && second.IsAlive)
                {
                    second.Abort();
                }
            }

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD = new FolderBrowserDialog();
            FBD.ShowNewFolderButton = false;
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = FBD.SelectedPath;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Для начала обязательно укажите, какие типы объектов искать: " +
                "папки, файлы или любые(выбрав оба варианта). Затем выберите " +
                "глубину поиска(т.е. количество реверсивных повторений). По " +
                "умолчанию этот параметр равен 1. После всего этого выберите " +
                "директорию и укажите имя (частично или целиком) искомого файла/папки.\n\n" +
                "Запустите программу и ждите результатов!)",
                "Help",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }



        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Создатель: Вакулич Денис 2 курс 10 группа.",
                "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);
        }

    }
}
