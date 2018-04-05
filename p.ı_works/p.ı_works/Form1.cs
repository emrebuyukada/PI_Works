using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace p.ı_works
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        struct tekrarsiz_sarki
        {
            public int sarkisayisi;
            public int clientsayisi;
        }
        SqlConnection con = new SqlConnection("server=.; Initial Catalog=emre;Integrated Security=SSPI");
       static  tekrarsiz_sarki[] yapi = new tekrarsiz_sarki[5000];
        SqlCommand cmd;
    
        public int ClientSay ()
        {
            int i = 0;          
            con.Open();
            cmd = new SqlCommand("select top 1  client_id from p_i   order by client_id desc");
            cmd.Connection = con;
            string maxsayfa = cmd.ExecuteScalar().ToString();
            con.Close();
            i = Convert.ToInt32(maxsayfa);  
            return i;
        }

        public int Client_Dinledigi_Sarki(int son_deger)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = " dd/MM/yyyy";
           
            string tarih = dateTimePicker1.Text;
            tarih = tarih.Trim()+"%";

            // MessageBox.Show(tarih);

            int i = 0;
            string gelen;
            // Veri tabanına kayıtları burdan yapmış bulunduk.
            //initial catalog= (veritabani ismi) işlem yapılacak veritabanı
            bool t = true;           

            con.Open();
            string sorgu; 
            while (t)
            {
                sorgu = "select  COUNT(DISTINCT song_id)  from p_i where client_id=" + son_deger.ToString() + "  and play_ts  LIKE '" + tarih + "';";
                cmd = new SqlCommand(sorgu);
                cmd.Connection = con;
                gelen= cmd.ExecuteScalar().ToString();
               // MessageBox.Show(gelen);
                yapi[son_deger].sarkisayisi = Convert.ToInt32(gelen);
                yapi[son_deger].clientsayisi = son_deger;
                //  MessageBox.Show(gelen);              

               

                if (son_deger==1)
                {
                    son_deger--;
                    yapi[son_deger].sarkisayisi = -1;
                    yapi[son_deger].sarkisayisi = son_deger;
                   
                    //  MessageBox.Show(son_deger.ToString());
                    t = false;
                }
                son_deger--;

            }
            con.Close();

           
            return i;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int clientsayisi = ClientSay();

            Client_Dinledigi_Sarki(clientsayisi);
            tekrarsiz_sarki[] yeni = new tekrarsiz_sarki[clientsayisi];

            int i = 0, j = 0, k = 0,s=0,l=1;

            for(i=0;i <= clientsayisi;i++)
            {
              //  MessageBox.Show(yapi[i].sarkisayisi.ToString() + "-" + yapi[i].clientsayisi.ToString());

                for (j=1+i; j <= clientsayisi; j++)
                {
                    if(yapi[i].sarkisayisi==-1) {}

                        if (yapi[i].sarkisayisi > 0)
                        {
                            if(yapi[i].sarkisayisi==yapi[j].sarkisayisi)
                            {
                            //   MessageBox.Show(yapi[i].sarkisayisi.ToString());
                            l++; k++;
                                yapi[j].sarkisayisi = -1;
                            }
                        }
                }

                if(yapi[i].sarkisayisi>0 && k==0)
                {
                    yeni[s].sarkisayisi = yapi[i].sarkisayisi;
                    yeni[s].clientsayisi = 1;
                    s++;
                }
                if(k>0)
                {
                    yeni[s].sarkisayisi = yapi[i].sarkisayisi;
                    yeni[s].clientsayisi = l;
                    
                    s++;
                }
                k = 0;l = 1;

            }

         //   MessageBox.Show(s.ToString());


            for ( i = 0; i < s; i++)
            {
                dataGridView1.Rows.Add();//datagridviewe yeni satır ekler
                dataGridView1.Rows[i].Cells[0].Value = yeni[i].sarkisayisi;
                dataGridView1.Rows[i].Cells[1].Value = yeni[i].clientsayisi;
            }

            if (s > 0)
                MessageBox.Show("Basariyla eklendi..");
            else
                MessageBox.Show("Kayit bulunamadi farkli tarih seciniz.");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }   
}
