using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KursOS
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
           // Application.EnableVisualStyles(); не понял за что это отвечает..
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }        

        public static int[] ScanBlock(string array)
        {
            int[] blocks = new int[(int)Sizes.page];
            if (array != "non")
            {
                string[] block = array.Split(' ');

                for (int i = 0; i < block.Length; i++)

                {

                    blocks[i] = Convert.ToInt32(block[i]);
                    
                }
                if (block.Length < (int)Sizes.page)
                {
                    for (int i = block.Length; i < blocks.Length; i++)
                    {
                        blocks[i] = (int)Сell.empty;
                    }
                }
            }
            else
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i] = (int)Сell.empty;
                }
            }
            return blocks;

        }

        public static int[] ScanTabl(string array)

        {
            
            string[] tabl = array.Split(' ');

            int[] tabls = new int[tabl.Length];


            for (int i = 0; i < tabl.Length; i++)

            {

                tabls[i] = Convert.ToInt32(tabl[i]);

            }
            return tabls;

        }

        public static bool StringCheckBlock(string block)
        {
            bool check = true;
            if (block != "non")
            {
                string[] blocks = block.Split(' ');
                int[] blocksi = new int[blocks.Length];

                for (int i = 0; i < blocks.Length; i++)

                {

                    blocksi[i] = Convert.ToInt32(blocks[i]);

                }
                int tmp = blocksi[0];
                for (int j = 0; j < blocksi.Length - 1; j++)
                {
                    for (int i = j + 1; i < blocksi.Length; i++)
                    {
                        if (tmp == (int)Сell.empty)
                            break;
                        if (tmp == blocksi[i])
                        {
                            MessageBox.Show("Нет смысла делать страницы одинаковыми");
                            check = false;
                            break;
                        }
                    }
                    if (check == false)
                    {
                        break;
                    }
                    tmp = blocksi[j + 1];
                }


                if (blocksi.Length > (int)Sizes.maxPageNum)
                {
                    MessageBox.Show("Страниц не может быть больше 5");
                    check = false;
                }

                for (int i = 0; i < blocksi.Length; i++)
                {
                    if (blocksi[i] > (int)Sizes.maxNum)
                    {
                        MessageBox.Show("Вводите число от 0 до 16");
                        check = false;
                        break;
                    }
                }
            }
            return check;
        }

        public static bool StringCheckTabls(string tabl)
        {
            bool check = true;
            string[] tablsing = tabl.Split(' ');
            int[] tablsi = new int[tablsing.Length];

            for (int i = 0; i < tablsing.Length; i++)

            {
                tablsi[i] = Convert.ToInt32(tablsing[i]);
            }
            for (int i = 0; i < tablsi.Length; i++)
            {
                if (tablsi[i] > (int)Sizes.maxNum)
                {
                    MessageBox.Show("Вводите число от 0 до 16");
                    check = false;
                    break;
                }
            }
            if (tablsi.Length > (int)Sizes.maxTablNum)
            {
                MessageBox.Show("Вводите не больше 40 страниц");
                check = false;
            }
            return check;
        }

        public static List<int> AlgorithmFIFO(int[] blocks, int[] tabl)
        {
            List<int> numbers = new List<int>();            
            int blank = 0; // если приходит пустая ячейка, то заполняется не сдвигом а подменой. Подсчёт таких ячеек.
            for (int p = 0; p < blocks.Length; p++)
            {
                
                if (blocks[p] == (int)Сell.empty)
                {
                    break;
                }
                else
                {
                    blank++;
                }
            }
            for (int i = 0; i < tabl.Length; i++)
            {
                bool flag = true; // Флаг. true - страница найдена в страничном блоке. false - не найдена.
                for (int j = 0; j < blocks.Length; j++)
                {
                    if (blocks[j] == tabl[i])
                    {
                        numbers.Add((int)Сell.interrupt); // прерывание
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (flag == false)
                {                                      
                    if (blank < blocks.Length && blocks[blank] == (int)Сell.empty)
                    {
                        blocks[blank] = tabl[i];
                        blank++;
                    }
                    else
                    {
                        int k = 0; 
                        while (k < blocks.Length - 1) // сдвиг массива
                        {
                            blocks[k] = blocks[k + 1];
                            k++;
                        }
                        blocks[blocks.Length - 1] = tabl[i]; // вставка элемента в конец
                    }
                }
                for (int k = 0; k < blocks.Length; k++)
                {
                    numbers.Add(blocks[k]);
                }
            }
            return numbers;
        }
    }
}
