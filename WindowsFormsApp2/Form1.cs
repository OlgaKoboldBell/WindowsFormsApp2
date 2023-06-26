using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Drawing.Imaging;

using System.Drawing;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Drawing.Imaging;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private SqlConnection conn = null;
        SqlDataAdapter da = null;
        DataSet set = null;
        SqlDataAdapter da1 = null;
        DataSet set1 = null;
        SqlDataAdapter da2 = null;
        DataSet set2 = null;
        SqlCommandBuilder cmd = null;
        string cs = "";
       
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = @" Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = Online_bookstore; Integrated Security = SSPI;";
            conn.ConnectionString = cs;
            
        }

        //усі книги
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                set = new DataSet();
                string sql = "select * from Books";
                da = new SqlDataAdapter(sql, conn);
                textBox9.Visible = false;
                dataGridView1.DataSource = null;
                cmd = new SqlCommandBuilder(da);
                Debug.WriteLine(cmd.GetInsertCommand().CommandText);
                Debug.WriteLine(cmd.GetUpdateCommand().CommandText);
                Debug.WriteLine(cmd.GetDeleteCommand().CommandText);
                da.Fill(set, "myBooks");
                set.Tables[0].TableName = "Books";
                dataGridView1.DataSource = set.Tables["Books"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //усі автори
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                set = new DataSet();
                string sql = "select * from Author";
                da = new SqlDataAdapter(sql, conn);
                dataGridView1.DataSource = null;
                textBox9.Visible = false;
                cmd = new SqlCommandBuilder(da);
                Debug.WriteLine(cmd.GetInsertCommand().CommandText);
                Debug.WriteLine(cmd.GetUpdateCommand().CommandText);
                Debug.WriteLine(cmd.GetDeleteCommand().CommandText);
                da.Fill(set, "myAuthor");
                set.Tables[0].TableName = "Author";
                dataGridView1.DataSource = set.Tables["Author"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //усі жанри
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                set = new DataSet();
                string sql = "select * from Genre";
                da = new SqlDataAdapter(sql, conn);
                dataGridView1.DataSource = null;
                textBox9.Visible = false;
                cmd = new SqlCommandBuilder(da);
                Debug.WriteLine(cmd.GetInsertCommand().CommandText);
                Debug.WriteLine(cmd.GetUpdateCommand().CommandText);
                Debug.WriteLine(cmd.GetDeleteCommand().CommandText);
                da.Fill(set, "myGenre");
                set.Tables[0].TableName = "Genre";
                dataGridView1.DataSource = set.Tables["Genre"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //Топ 5 новинки
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                textBox9.Visible = false;
                set = new DataSet();
                string sql = "select top(5)[Name] from Books order by [Year] DESC";
                da = new SqlDataAdapter(sql, conn);
                cmd = new SqlCommandBuilder(da);
                da.Fill(set, "myBooks");
                set.Tables[0].TableName = "Books";
                dataGridView2.DataSource = set.Tables["Books"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //Топ 3 автори
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                textBox9.Visible = false;
                set = new DataSet();
                string sql = "select top (1) b.LastName, b.FirstName, a.Count from Author as b, ( select a.Name_Author,Count(a.Name) as Count from Books as a \r\nwhere a.Status=N'продано' GROUP BY a.Name_Author ) as a where b.Id=a.Name_Author order by  a.Count DESC";
                     da = new SqlDataAdapter(sql, conn);
                    cmd = new SqlCommandBuilder(da);
                    da.Fill(set, "myAuthor");
                    set.Tables[0].TableName = "Author";
                    dataGridView2.DataSource = set.Tables["Author"]; 
            }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                }
        }

        //топ 3 продаж
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    textBox9.Visible = false;
                    set = new DataSet();
                    string sql = "select top(3) * from Books where Status=N'продано' order by Name DESC";
                    da = new SqlDataAdapter(sql, conn);
                    cmd = new SqlCommandBuilder(da);
                    da.Fill(set, "myBooks");
                    set.Tables[0].TableName = "Books";
                    dataGridView2.DataSource = set.Tables["Books"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //топ 3 жанрів
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                textBox9.Visible = false;
                set = new DataSet();
                string sql = "select top (1) b.GenreName, a.Count from Genre as b, ( select a.Genre,Count(a.Name) as Count from Books as a where a.Status=N'продано' GROUP BY a.Genre) as a where b.Id=a.Genre order by  a.Count DESC";
                da = new SqlDataAdapter(sql, conn);
                cmd = new SqlCommandBuilder(da);
                da.Fill(set, "myGenre");
                set.Tables[0].TableName = "Genre";
                dataGridView2.DataSource = set.Tables["Genre"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        //Button дізнатися -- ПОШУК ПО НАЗВІ КНИГИ
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                textBox9.Text = "";
                set = new DataSet();
                string sql = "select [Name], [Publishing_House], [Count_Pages], [Genre] from Books where [Name] = N'" + textBox2.Text + "'";
                da = new SqlDataAdapter(sql, conn);
                dataGridView1.DataSource = null;
                cmd = new SqlCommandBuilder(da);
                //da.Fill(set, "myBooks");
                if (da.Fill(set, "myBooks") != 0)
                {
                    set.Tables[0].TableName = "Books";
                    dataGridView1.DataSource = set.Tables["Books"];
                }
                else MessageBox.Show("Книги з такою назвою в таблиці немає");
                textBox9.Visible = true;
                textBox9.Text = ""; 
                WebClient client = new WebClient();

                List<Pic> images = new List<Pic>();

                string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Online_bookstore;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql2 = "SELECT * FROM Pictures";
                    SqlCommand command = new SqlCommand(sql2, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string filename = reader.GetString(1);
                            string title = reader.GetString(2);
                            byte[] data = (byte[])reader.GetValue(3);
                            Pic image = new Pic(id, filename, title, data);
                            images.Add(image);
                        }
                    }
                }
             
                switch (textBox2.Text)
                {
                    case "Аліса в країні див":

                        //if (textBox2.Text == "Аліса в країні див")
                        {
                            //using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/19551/19551.txt"))
                            //{
                            //    using (StreamReader reader = new StreamReader(stream))
                            //    {
                            //        string line = "";
                            //        for (int i = 0; i < 150; i++)
                            //        {
                            //            line = reader.ReadLine();
                            //            textBox9.Text += line;
                            //        }
                            //    }
                            //}

                            MemoryStream ms = new MemoryStream(images[0].Data);
                            pictureBox2.Image = System.Drawing.Bitmap.FromStream(ms);
                        }
                        break;
                    case "Три мушкетери":
                        //if (textBox2.Text == "Три мушкетери")
                        {
                            //using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/56054/56054-0.txt"))
                            //{
                            //    using (StreamReader reader = new StreamReader(stream))
                            //    {
                            //        string line = "";
                            //        for (int i = 0; i < 150; i++)
                            //        {
                            //            line = reader.ReadLine();
                            //            textBox9.Text += line;
                            //        }
                            //    }
                            //}
                            MemoryStream ms = new MemoryStream(images[4].Data);
                            pictureBox2.Image = System.Drawing.Bitmap.FromStream(ms);
                        }
                        break;
                    case "Кобзар":
                        //if (textBox2.Text == "Кобзар")
                        {
                            //using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/68486/68486-0.txt"))
                            //{
                            //    using (StreamReader reader = new StreamReader(stream))
                            //    {
                            //        string line = "";
                            //        for (int i = 0; i < 150; i++)
                            //        {
                            //            line = reader.ReadLine();
                            //            textBox9.Text += line;
                            //        }
                            //    }
                            //}
                            MemoryStream ms = new MemoryStream(images[3].Data);
                            pictureBox2.Image = System.Drawing.Bitmap.FromStream(ms);
                        }
                        break;
                    case "Зов предків":
                        //if (textBox2.Text == "Зов предків")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/1163/1163.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }

                            MemoryStream ms = new MemoryStream(images[2].Data);
                            pictureBox2.Image = System.Drawing.Bitmap.FromStream(ms);
                        }
                        break;
                    case "Дракула":
                        //if (textBox2.Text == "Дракула")
                        {
                            //using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/345/345-0.txt"))
                            //{
                            //    using (StreamReader reader = new StreamReader(stream))
                            //    {
                            //        string line = "";
                            //        for (int i = 0; i < 150; i++)
                            //        {
                            //            line = reader.ReadLine();
                            //            textBox9.Text += line;
                            //        }
                            //    }
                            //}
                            MemoryStream ms = new MemoryStream(images[1].Data);
                            pictureBox2.Image = System.Drawing.Bitmap.FromStream(ms);
                        }
                        break;
                    case "Місячний камінь":
                        //if (textBox2.Text == "Місячний камінь")
                        {
                            //using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/1895/1895-0.txt"))
                            //{
                            //    using (StreamReader reader = new StreamReader(stream))
                            //    {
                            //        string line = "";
                            //        for (int i = 0; i < 150; i++)
                            //        {
                            //            line = reader.ReadLine();
                            //            textBox9.Text += line;
                            //        }
                            //    }
                            //}
                        }
                        break;
                }
                        textBox2.Text = "";
    }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public class Pic
        {
            public Pic(int id, string filename, string title, byte[] data)
            {
                Id = id;
                FileName = filename;
                Title = title;
                Data = data;
            }
            public int Id { get; private set; }
            public string FileName { get; private set; }
            public string Title { get; private set; }
            public byte[] Data { get; private set; }
        }


        //Button дізнатися -- ПОШУК ПО АВТОРУ КНИГИ
        private void button12_Click_1(object sender, EventArgs e)
        {
            try
            {
                textBox9.Visible = false;
                set = new DataSet();
                string sql = "select [Name], [Publishing_House], [Count_Pages], [Genre] from Books where [Name_Author]=(select Id from Author where [LastName]=  N'" + textBox1.Text + "')";
                da = new SqlDataAdapter(sql, conn);
                    dataGridView1.DataSource = null;
                    cmd = new SqlCommandBuilder(da);
                    //da.Fill(set, "myBooks");
                if (da.Fill(set, "myBooks") != 0)
                {
                    set.Tables[0].TableName = "Books";
                    dataGridView1.DataSource = set.Tables["Books"];
                }
                else MessageBox.Show("Книги ВКАЗАНОГО АВТОРА в таблиці немає");
                textBox1.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

private class Image
{
    public Image(int id, string filename, string title, byte[] data)
    {
        Id = id;
        FileName = filename;
        Title = title;
        Data = data;
    }
    public int Id { get; private set; }
    public string FileName { get; private set; }
    public string Title { get; private set; }
    public byte[] Data { get; private set; }
}

//Button дізнатися -- ПОШУК ПО ЖАНРУ КНИГИ
private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                textBox9.Visible = false;
                set = new DataSet();
                string sql = "select [Name], [Publishing_House], [Count_Pages], [Promotion] from Books where Genre=(select Id from Genre where GenreName= N'" + textBox3.Text + "')";
             
                da = new SqlDataAdapter(sql, conn);
                dataGridView1.DataSource = null;
                cmd = new SqlCommandBuilder(da);
                //da.Fill(set, "myBooks");
                if (da.Fill(set, "myBooks") != 0)
                { 
                set.Tables[0].TableName = "Books";
                dataGridView1.DataSource = set.Tables["Books"];
                }
                else MessageBox.Show("Книги ВКАЗАНОГО ЖАНРУ в таблиці немає");
                textBox3.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        //ДОБАВИТИ/РЕДАГУВАТИ КНИГУ
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                da.Update(set, "Books");
                da1.Update(set1, "Genre");
                da2.Update(set2, "Author");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //ОБНОВИТИ ТАБЛИЦЮ КНИГИ
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                textBox9.Visible = false;
                set = new DataSet();
                string sql = "select * from Books";
                da = new SqlDataAdapter(sql, conn);
                dataGridView1.DataSource = null;
                cmd = new SqlCommandBuilder(da);
                Debug.WriteLine(cmd.GetInsertCommand().CommandText);
                Debug.WriteLine(cmd.GetUpdateCommand().CommandText);
                Debug.WriteLine(cmd.GetDeleteCommand().CommandText);
                da.Fill(set, "myBooks");
                set.Tables[0].TableName = "Books";
                dataGridView1.DataSource = set.Tables["Books"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        //ВИДАЛИТИ КНИГУ
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                    //видаляю дані
                    set = new DataSet();
                    string sql = "SELECT * FROM Books)";
                    da = new SqlDataAdapter(sql, conn);
                    dataGridView1.DataSource = null;
                    var cmb = new SqlCommandBuilder(da);
                    da.Fill(set, "myBooks");
                    set.Tables[0].TableName = "Books";
                    dataGridView1.DataSource = set.Tables["Books"];
                    SqlCommand cmd = new SqlCommand($"DELETE FROM Books WHERE [Name] = {textBox4.Text} and Name_Author=(select Id from Author where LastName= N'{textBox5.Text}') ", conn);

                    da.DeleteCommand = cmd;
                    da.Update(set.Tables[0]);
                textBox4.Text = "";
                textBox5.Text = "";
            }
                catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string a, b;
        //кнопка ЗАПАМ'ЯТАТИ
        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "")
            {
                a = textBox6.Text;
                b = textBox7.Text;
                textBox6.Text = "";
                textBox7.Text = "";
            }
            else MessageBox.Show("Ви не ввели логін або пароль!");
        }

       //кнопка УВІЙТИ
        private void button15_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == a && textBox7.Text == b)
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Visible = false;
                dataGridView1.Visible = true;
                dataGridView2.Visible = true;
                try
                {
                    set = new DataSet();
                    string sql = "select * from Books";
                    da = new SqlDataAdapter(sql, conn);
                    dataGridView1.DataSource = null;
                    cmd = new SqlCommandBuilder(da);
                    Debug.WriteLine(cmd.GetInsertCommand().CommandText);
                    Debug.WriteLine(cmd.GetUpdateCommand().CommandText);
                    Debug.WriteLine(cmd.GetDeleteCommand().CommandText);
                    da.Fill(set, "myBooks");
                    set.Tables[0].TableName = "Books";
                    dataGridView1.DataSource = set.Tables["Books"];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                }
            }
            else MessageBox.Show("Ви ввели неправильний логін або пароль!");
  
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            { 
            string selectedState = comboBox1.SelectedItem.ToString();
            textBox9.Visible = true;
            textBox9.Text = "";
            WebClient client = new WebClient();
 
                switch (selectedState)
                {
                    case "Аліса в країні див":

                        //if (textBox2.Text == "Аліса в країні див")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/19551/19551.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }
                        }
                        break;
                    case "Три мушкетери":
                        //if (textBox2.Text == "Три мушкетери")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/56054/56054-0.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }
                        }
                        break;
                    case "Кобзар":
                        //if (textBox2.Text == "Кобзар")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/68486/68486-0.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }
                      
                        }
                        break;
                    case "Зов предків":
                        //if (textBox2.Text == "Зов предків")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/1163/1163.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }
                        }
                        break;
                    case "Дракула":
                        //if (textBox2.Text == "Дракула")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/345/345-0.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }
                        }
                        break;
                    case "Місячний камінь":
                        //if (textBox2.Text == "Місячний камінь")
                        {
                            using (Stream stream = client.OpenRead("https://www.gutenberg.org/files/1895/1895-0.txt"))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    string line = "";
                                    for (int i = 0; i < 150; i++)
                                    {
                                        line = reader.ReadLine();
                                        textBox9.Text += line;
                                    }
                                }
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

      



        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
