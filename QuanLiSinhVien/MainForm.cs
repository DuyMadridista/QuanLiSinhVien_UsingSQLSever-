using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using WindowsFormsApp1;

namespace QuanLiSinhVien
{
    public partial class MainForm : Form
    {
        private static MainForm instance;

        public static MainForm Instance { get => instance; set => instance=value; }

        public MainForm()
        {
            InitializeComponent();
            setCbbClass();
            setCbbSort();
            Instance = this;
        }

        
        
        public void setCbbClass()
        {
            cbLop.Items.Add("All");
            DataTable dt = new DataTable();
            dt= DBHelper.Instance.GetRecordsx("select LopSH from SV");
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            IEnumerable<string> distinctValues = list.Distinct();
            foreach (string s in distinctValues)
            {
                cbLop.Items.Add(s);
            }
            cbLop.SelectedIndex = 0;
            
        }
        private void setCbbSort()
        {
            cbbSort.Items.Add("MSSV");
            cbbSort.Items.Add("Name");
            cbbSort.Items.Add("LopSH");
            cbbSort.Items.Add("DTB");
            cbbSort.Items.Add("NgaySinh");
            cbbSort.Items.Add("Gender");
            cbbSort.Items.Add("Anh");
            cbbSort.Items.Add("HocBa");
            cbbSort.Items.Add("CCCD");
            cbbSort.SelectedIndex = 0;
        }
        private void cbLop_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbLop.SelectedIndex == 0)
            {
                dataGridView1.DataSource= DBHelper.Instance.GetRecordsx("select * from SV");
                //return;
            }
            else
            {
                SqlParameter p = new SqlParameter("@value", cbLop.SelectedItem.ToString());
                string query = "select * from SV where LopSH = @value";
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.Add(p);
                dataGridView1.DataSource = DBHelper.Instance.GetRecordsx(cmd);
                // return;
            }
            SeachText.Text="";
        }
        public void Search_Click(object sender, EventArgs e)
        {
            if (cbLop.SelectedItem == null)
            {
                MessageBox.Show("Vui Lòng Chọn lớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (cbLop.SelectedIndex == 0)
            {
                dataGridView1.DataSource = DBHelper.Instance.GetRecordsx("select * from SV where Name like N'%"+SeachText.Text+"%'");
                return;
            }
            else
            {
                //SqlParameter p = new SqlParameter("@Name", SeachText.Text);
                SqlParameter p2 = new SqlParameter("@value", cbLop.SelectedItem.ToString());
                string query = "select * from SV where Name like N'%"+SeachText.Text+"%' and LopSH = @value";
                SqlCommand cmd = new SqlCommand(query);
                //cmd.Parameters.Add(p);
                cmd.Parameters.Add(p2);
                dataGridView1.DataSource = DBHelper.Instance.GetRecordsx(cmd);
            }
        }



        private void Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count ==1)
            {
                
                    string s = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    SqlParameter p2 = new SqlParameter("@MSSV", s);
                    string query2 = "delete from SV where MSSV = @MSSV";
                    DBHelper.Instance.ExecuteDB(query2, p2);
                Search_Click(sender, e);
            }
            Search_Click(sender, e);
        }
       private void Sort_Click(object sender, EventArgs e)
        {
            if (cbLop.SelectedIndex == 0)
            {
                dataGridView1.DataSource = DBHelper.Instance.GetRecordsx("select * from SV order by "+ cbbSort.SelectedItem.ToString());
                return;
            }
            else
            {

                SqlParameter p2 = new SqlParameter("@Lop", cbLop.SelectedItem.ToString());
                //SqlParameter p = new SqlParameter("@value", cbbSort.SelectedItem.ToString());
                string query = "select * from SV where LopSH = @Lop order by "+ cbbSort.SelectedItem.ToString();
                SqlCommand cmd = new SqlCommand(query);
                //cmd.Parameters.Add(p);
                cmd.Parameters.Add(p2);
                dataGridView1.DataSource = DBHelper.Instance.GetRecordsx(cmd);
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
              DetailsForm f= new DetailsForm();
              f.ShowDialog();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string mssv = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                DetailsForm f = new DetailsForm(mssv);
                f.ShowDialog();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        
    }
}
