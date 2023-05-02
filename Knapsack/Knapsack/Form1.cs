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


namespace Knapsack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string line;
        int linecount = 0;
        int i = 0, j = 0;
        int satirSayisi;
        int sutunSayisi;
        int itemSayısı = 0;
        int maxKapasite = 0;
        int[,] dizi;
        int[,] data;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                listBox3.Items.Clear();
                OpenFileDialog file = new OpenFileDialog();
                file.RestoreDirectory = true;
                file.CheckFileExists = false;
                file.Title = "Dosyayı seçiniz...";
                file.ShowDialog();

                string DosyaYolu = file.FileName;
                string DosyaAdi = file.SafeFileName;
                label1.Text = DosyaAdi;
                label2.Text = DosyaYolu;

                FileStream dosya = new FileStream(DosyaYolu, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(dosya);
                string yazi = sw.ReadLine();

                //satır ve sütun sayısı hesaplandı
                string[] veri = yazi.Split(' ');
                string[] satirlar = File.ReadAllLines(DosyaYolu);
                satirSayisi = satirlar.Length;
                sutunSayisi = satirlar[0].Split(' ').Length;

                richTextBox1.Text = "Satır sayısı: " + satirSayisi.ToString();
                richTextBox1.Text += "\nSütun sayısı:" + sutunSayisi;

                dizi = new int[satirSayisi, sutunSayisi];
                data = new int[satirSayisi, sutunSayisi];
                using (StreamReader reader = new StreamReader(DosyaYolu))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] deger = line.Split(' ');
                        if (linecount == 0)
                        {
                            itemSayısı = int.Parse(deger[0]);
                            maxKapasite = int.Parse(deger[1]);
                            dizi[0, 0] = itemSayısı;
                            dizi[0, 1] = maxKapasite;
                            richTextBox1.Text += "\nToplam item sayısı: " + itemSayısı.ToString();
                            richTextBox1.Text += "\nMax çanta ağırlığı:" + maxKapasite.ToString();

                        }
                        else
                        {
                            i++;
                            if (i == itemSayısı + 1)
                            {
                                break;
                            }
                            j = 0;
                            dizi[i, j] = int.Parse(deger[0]);
                            j = 1;
                            dizi[i, j] = int.Parse(deger[1]);
                        }
                        linecount++;
                    }
                }
                dosya.Close();
                linecount = 0;
            }
            catch (Exception)
            {
                MessageBox.Show("BELİRLENEN FORMATTA BİR DOSYA SEÇMENİZ LAZIM!!!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int k = 1; k <= itemSayısı; k++)
                {
                    listBox2.Items.Add(k.ToString() + ".--> Value: " + dizi[k, 0].ToString() + " / " + k.ToString() + ". Weight: " + dizi[k, 1].ToString());
                }
            }
            else
            {
                listBox2.Items.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                string yazi = " ";
                string tyazi = " ";
                richTextBox2.Text = " ";
                int cap = 0;
                int[,] canta = new int[itemSayısı + 1, maxKapasite + 1];
                cap = maxKapasite;
                //Hesaplamanın yapıldığı kod satırı
                for (int item = 1; item <= itemSayısı; item++)
                {
                    for (int capacity = 1; capacity <= maxKapasite; capacity++)
                    {
                        if (item == 0 || capacity == 0)
                        {
                            canta[item, capacity] = 0;
                        }
                        else if (dizi[item, 1] <= capacity)
                        {
                            canta[item, capacity] = Math.Max(
                                dizi[item, 0] + canta[item - 1, capacity - dizi[item, 1]],
                                                  canta[item - 1, capacity]
                                                  );
                        }
                        else
                        {
                            canta[item, capacity] = canta[item - 1, capacity];
                        }
                    }
                }
                //Hangi verilerin kullanıldığını gösteren kod satırları
                listBox3.Items.Add("Kapladığı yer: " + canta[itemSayısı, maxKapasite]);
                int m = itemSayısı;
                int n = maxKapasite;
                listBox3.Items.Add("Kullanılan itemler(0-Kullanılmadı/1-Kullanıldı)");
                while (m > 0 && n > 0)
                {
                    if (canta[m, n] == canta[m - 1, n])
                    {
                        //item excluded
                        listBox3.Items.Add(m + ". İtem --> 0");
                        yazi += " 0";
                    }
                    else
                    {
                        //item included
                        listBox3.Items.Add(m + ". İtem--> 1");
                        yazi += " 1";

                        n = n - dizi[m, 1];
                    }
                    m--;
                }
                for (int u = yazi.Length - 1; u >= 0; u--)
                {
                    tyazi = tyazi + yazi[u].ToString();
                }
                richTextBox2.Text += tyazi;


            }
            catch (Exception ett)
            {
                MessageBox.Show(ett.ToString());
            }
        }
    }
}
