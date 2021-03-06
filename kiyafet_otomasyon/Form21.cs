﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;

namespace kiyafet_otomasyon
{
    public partial class Form21 : Form
    {
        public Form1 frm1;
        public Form21()
        {
            InitializeComponent();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adtr = new SqlDataAdapter("select musteriId,musteriAdi,musteriSoyadi,tcKimlik,cepTel,evTel,adres,silinmeTarihi From silinmismusteribil", frm1.bag);
            string alan = "";
            if (comboBox1.Text == "Müşteri Adı") alan = "musteriAdi";
            //eğer combo1 in metni  Müşteri Adı ise alan değişkine musteriAdi a eşitle yani combo1 de Müşteri Adı seçeneği seçiliyse
            else if (comboBox1.Text == "Müşteri Soyadı") alan = "musteriSoyadi";
            else if (comboBox1.Text == "Tc Kimlik") alan = "tcKimlik";
            else if (comboBox1.Text == "Cep Tel") alan = "cepTel";
            else if (comboBox1.Text == "Ev Tel") alan = "evTel";
            else if (comboBox1.Text == "Adres") alan = "adres";
            else if (comboBox1.Text == "Silinme Tarihi") alan = "silinmeTarihi";

            if (comboBox1.Text == "Tümü")//eğer texbox boş ise
            {
                frm1.bag.Open();
                frm1.tabloMusteri.Clear();
                frm1.kmt.Connection = frm1.bag;
                frm1.kmt.CommandText = "select musteriId,musteriAdi,musteriSoyadi,tcKimlik,cepTel,evTel,adres,silinmeTarihi From silinmismusteribil";//tüm kayıtları seç
                adtr.SelectCommand = frm1.kmt;
                adtr.Fill(frm1.tabloMusteri);
                frm1.bag.Close();
            }
            if (alan != "")
            {
                frm1.bag.Open();
                adtr.SelectCommand.CommandText = "select musteriId,musteriAdi,musteriSoyadi,tcKimlik,cepTel,evTel,adres,silinmeTarihi From silinmismusteribil" + " where(" + alan + " like '%" + textBox1.Text + "%' )";
                // textbox1 e girilen değerin belitilen alan içinde geçenleri seç
                frm1.tabloMusteri.Clear();
                adtr.Fill(frm1.tabloMusteri);//tabloMusteri sanal tablosunu adaptör ile doldur.
                frm1.bag.Close();
            }
        }

        private void Form21_Load(object sender, EventArgs e)
        {
            frm1.silinmismusteriListele();
            try
            {
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.Columns[0].HeaderText = "Müşteri Adı";
                dataGridView1.Columns[1].HeaderText = "Müşteri Soyadı";
                dataGridView1.Columns[2].HeaderText = "Tc Kimlik";
                dataGridView1.Columns[3].HeaderText = "Cep Tel";
                dataGridView1.Columns[4].HeaderText = "Ev Tel";
                dataGridView1.Columns[5].HeaderText = "Adres";
                dataGridView1.Columns[6].HeaderText = "Silinme Tarihi";
            }
            catch
            {
                ;
            }
        }

        private void kapat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            frm1.frm4.textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //form4 deki textbox1 in textine datagridview1 deki seçili satırın 0. hücresindeki değeri yaz.
            frm1.frm4.textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            frm1.frm4.textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            frm1.frm4.textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            frm1.frm4.textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            frm1.frm4.textBox6.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            frm1.frm4.textBox6.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            frm1.frm4.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) //EXEL Aktar
        {
            Microsoft.Office.Interop.Excel.Application uyg = new Microsoft.Office.Interop.Excel.Application();
            uyg.Visible = true;
            Microsoft.Office.Interop.Excel.Workbook kitap = uyg.Workbooks.Add(System.Reflection.Missing.Value);
            Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)kitap.Sheets[1];
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[1, i + 1];//satır sütun şeklindendir.
                myRange.Value2 = dataGridView1.Columns[i].HeaderText;
            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {

                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[j + 2, i + 1];//satır sütun şeklindendir.
                    myRange.Value2 = dataGridView1[i, j].Value;// kolom satır şeklindedir
                }
            }
        }
    }
}
