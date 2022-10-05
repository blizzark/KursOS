using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace KursOS
{

    enum Сell
    {
        empty = -1,
        interrupt = -2
    }

    enum Sizes
    {
        page = 5,
        maxPageNum = 5,
        maxNum = 16,
        maxTablNum = 40
        
    }

    public partial class Form1 : Form
    {
        List<int> numbers = new List<int>(); // лист для заолнения таблицы
        int prer = 0; // количество прерываний
        bool first = true; // первый ли проход
        public Form1()
        {
            InitializeComponent();
            var myToolTip = new ToolTip(); // подсказки
            myToolTip.SetToolTip(textBox1, "Введите до 5 чисел через пробел (от 0 до 16).\nЭту строку можно оставить пустой ");
            myToolTip.SetToolTip(textBox2, "Введите до 40 чисел через пробел (от 0 до 16).");
            myToolTip.SetToolTip(button2, "Очистить ввод и таблицу");
        }

        private void button1_Click(object sender, EventArgs e) // кнопка Старт
        {
            if (!first) // очистка таблицы после предыдущего раза
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                label4.Text = "";
                numbers.Clear();
            }
            first = false;
            bool check = true; // проверка на валидность
            // блок проверки ввода
            { 
                try
                {
                    string block = Convert.ToString(textBox1.Text);
                    string tabl = Convert.ToString(textBox2.Text);

                    if (textBox2.Text == "")
                    {
                        check = false;
                    }
                    if (textBox1.Text == "")
                    {
                        block = "non";
                    }
                    check = Program.StringCheckBlock(block);
                    if (check)
                        check = Program.StringCheckTabls(tabl);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Вводите числа!");
                    check = false;
                }
            }

            if (check == true)
            {
                string block = Convert.ToString(textBox1.Text);
                if (textBox1.Text == "")
                {
                    block = "non";
                }
                // работа алгоритма
                numbers = Program.AlgorithmFIFO(Program.ScanBlock(Convert.ToString(block)), Program.ScanTabl(Convert.ToString(textBox2.Text.ToString())));
                int[] tabls = Program.ScanTabl(textBox2.Text.ToString());



                //подготовка (создание колонок и строк
                {

                    for (int i = 0; i < tabls.Length; i++) // вывод колонок
                    {
                        dataGridView1.Columns.Add(i.ToString(), Convert.ToString(tabls[i]));
                    }

                    for (int i = 0; i < tabls.Length; i++)  // задаю ширину
                        dataGridView1.Columns[i].Width = 25; // установка ширины ячейки
                    for (int i = 1; i < 6; i++) // начальная строка
                    {
                        dataGridView1.Rows.Add(" "); // заполнение 1 столбца
                    }
                }

                //заполнение таблицы
                {
                    int k = 0; // счётчик прохода по numbers
                    prer = 0;
                    for (int i = 0; i < tabls.Length; i++)
                    {
                        for (int j = 0; j < (int)Sizes.page; j++)
                        {
                            if (numbers[k] == (int)Сell.interrupt)
                            {
                                for (int i2 = 0; i2 < dataGridView1.Rows.Count; i2++)
                                {
                                    dataGridView1[i, i2].Style.BackColor = Color.LightSeaGreen;
                                    prer++;
                                }
                                k++;
                            }
                            if (numbers[k] == (int)Сell.empty)
                            {
                                dataGridView1.Rows[j].Cells[i].Value = " ";
                                k++;
                            }
                            else
                            {
                                dataGridView1.Rows[j].Cells[i].Value = numbers[k];
                                k++;
                            }
                        }
                    }
                    prer /= 5;
                    prer = tabls.Length - prer;
                    label4.Text = prer.ToString(); // вывод прерываний
                }
            }

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewColumn dgvc in dataGridView1.Columns) 
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable; // запрет на сортировку по столбцам
            }
        }

        private void разработчикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данную программу разработал студент\nСПбГТИ(ТУ) факультета ИТиУ 475группы:\nОвчинников Роман Сергеевич ", "Информация о разработчике");
        }

        private void button2_Click(object sender, EventArgs e) //Кнопка Сброс
        {
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            label4.Text = "";
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            numbers.Clear();
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileDialog1.FileName;
                    // читаем файл в строку
                    string block = File.ReadLines(filename).First();
                    string tabl = File.ReadLines(filename).Skip(1).First(); ;
                    textBox1.Text = block;
                    textBox2.Text = tabl;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка файла!");

            }
        }

        private void исходныеДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveFileDialog1.FileName;
                    // сохраняем текст в файл
                   
                  

                    File.WriteAllText(filename, textBox1.Text + '\n' + textBox2.Text);

                    MessageBox.Show("Файл сохранен");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения файла!");

            }
        }

        private void конечныйРезультатToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.DefaultExt = "txt";
                saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {

                    string filename = saveFileDialog1.FileName;
                    // сохраняем текст в файл
                    string block = Convert.ToString(textBox1.Text);

                    string tabl = Convert.ToString(textBox2.Text);
                    StreamWriter f = new StreamWriter(filename);
                    f.WriteLine("Исходные данные: " + block);
                    f.WriteLine("Подгружаемые стриницы: " + tabl);
                    f.WriteLine("Результат работы алгоритма FIFO");
                    f.WriteLine("Количество прерываний: " + prer);

                    //сохраняем таблицу в файл
                    for (int i = 0; i < numbers.Count - (int)Sizes.page;)
                    {
                        for (int j = 0; j < (int)Sizes.page; j++)
                        {
                            if (numbers[i] == (int)Сell.interrupt)
                            {
                                f.Write("Нет прерывания ");
                                f.Write(numbers[++i] + " ");
                                i++;
                            }
                            else if (numbers[i] == (int)Сell.empty)
                            {
                                f.Write("  ");
                                i++;
                            }
                            else
                            {
                                f.Write(numbers[i++] + " ");                             
                            }
                            
                        }
                        f.WriteLine("");
                    }
                    f.Close();
                    MessageBox.Show("Файл сохранен");
                }
               
               
            }
            
            catch (Exception)
            {
                MessageBox.Show("Ошибка сохранения файла!");
            }

            
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, @"HelpFIFO.chm", HelpNavigator.TableOfContents);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

