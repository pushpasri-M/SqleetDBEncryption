using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EmployeeMangementForm
{
        public partial class Form1 : Form
        {
        public Form1()
        {
             InitializeComponent();
            // Open DB
            DatabaseManager.InitializeDatabase();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hidePannel();
            HomePannel.Visible = true;
            SetActiveButton(button2);
            try
            {
                // Load all employee data into the DataGridView
                dataGridView1.DataSource = DatabaseManager.GetEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }

        }
            private void hidePannel()
            {
                HomePannel.Visible = false;
                AboutPanel.Visible = false;

            }
           
            private void SetActiveButton(System.Windows.Forms.Button activeBtn)
            {
                // Reset colors
                button2.BackColor = Color.Black;
                button3.BackColor = Color.Black;


                // Highlight active
                activeBtn.BackColor = Color.Gray;
            }
            private void ApplyModernStyle(DataGridView dgv)
            {
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            dgv.EnableHeadersVisualStyles = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.RowHeadersVisible = false;
                

            // Row styles
                dgv.RowsDefaultCellStyle.BackColor = Color.White;
                dgv.RowsDefaultCellStyle.ForeColor = Color.Black;
                dgv.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(234, 228, 213);
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 0, 0); // Blue highlight
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;

                // Header styles
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(182, 176, 159); // Grey
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(182, 176, 159); // same as normal
                dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;

                // Apply per-column header style to fix ID column
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.HeaderCell.Style.BackColor = Color.FromArgb(182, 176, 159);
                    col.HeaderCell.Style.ForeColor = Color.White;
                    col.HeaderCell.Style.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }

        private void button2_Click_1(object sender, EventArgs e)
        {
            hidePannel();
            HomePannel.Visible = true;

            SetActiveButton(button2);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            hidePannel();
            AboutPanel.Visible = true;
            AboutPanel.BringToFront();
            dataGridView1.EnableHeadersVisualStyles = false;

            ApplyModernStyle(dataGridView1);


            SetActiveButton(button3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Read values from text boxes
               
                string name = textBox2.Text;
                int salary = int.Parse(textBox3.Text);
                string address = textBox4.Text;
                DatabaseManager.InsertEmployee(name, salary, address);
                MessageBox.Show("Employee saved!");

                dataGridView1.DataSource = DatabaseManager.GetEmployees();
               

                // Optional: Clear text fields after insertion
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AboutPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        
       

        private void button4_Click(object sender, EventArgs e)
        {
             Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
       

    }




}
