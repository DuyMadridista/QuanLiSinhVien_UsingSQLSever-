using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace QuanLiSinhVien
{
    public partial class DetailsForm : Form
    {
        string MSSV2;
        
        public DetailsForm()
        {
            InitializeComponent();
            setCbbLop();
            setDataAdd();
        }
        public DetailsForm(string mssv)
        {
            InitializeComponent();
            MSSV2 = mssv;
            setCbbLop();
            setEditData();
            

        }
       private void setCbbLop()
        {
            
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
            
        }
        private void setEditData()
        {
            DataTable dt = DBHelper.Instance.GetRecordsx("select * from  SV where MSSV="+MSSV2);
            DataRow sv = dt.Rows[0];
            tbMasv.Text = sv["MSSV"].ToString();
            tbName.Text = sv["Name"].ToString();
            TbDiem.Text= sv["DTB"].ToString();
            cbLop.SelectedIndex=cbLop.Items.IndexOf(sv["LopSH"].ToString());
            dateTimePicker1.Value=Convert.ToDateTime(sv["NS"].ToString());
            if ((bool)sv["Gender"])
            {
                maleB.Checked = true;
            }
            else
            {
                FemaleB.Checked = true;
            }
            CheckAnh.Checked=(bool)(sv["Anh"]);
            checkHB.Checked=(bool)(sv["HocBa"]);
            CheckCmnd.Checked=(bool)(sv["CCCD"]);
            tbMasv.Enabled=false;
            OK.Click+=OK_ClickEdit;
        }
        private void setDataAdd()
        {

            tbMasv.Enabled = true;
           
            FemaleB.Checked = true;
            OK.Click += OK_ClickAdd;

        }
        public void Insert(string mssv, string name, string lopsh, bool gender, DateTime NS, bool anh, bool hocba, bool CCCD,float DTB, bool command)
        {       
                string query1 = "insert into SV values(@MSSV,@Name,@LopSH,@Gender,@NS,@DTB,@Anh,@HocBa,@CCCD)";
                string query2 = "update SV set Name=@Name,LopSH=@LopSH,Gender=@Gender,NS=@NS,DTB=@DTB,Anh=@Anh,HocBa=@HocBa,CCCD=@CCCD where MSSV=@MSSV";
                SqlParameter p0 = new SqlParameter("@MSSV", mssv);
                SqlParameter p1 = new SqlParameter("@Name", name);
                SqlParameter p2 = new SqlParameter("@LopSH", lopsh);
                SqlParameter p3 = new SqlParameter("@Gender", gender);
                SqlParameter p4 = new SqlParameter("@NS", NS);
                SqlParameter p5 = new SqlParameter("@Anh", anh);
                SqlParameter p6 = new SqlParameter("@HocBa", hocba);
                SqlParameter p7 = new SqlParameter("@CCCD", CCCD);
                SqlParameter p8 = new SqlParameter("@DTB", DTB);
                string query;
                if (command==true) query=query1;
                else query=query2;
                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.Add(p0);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);
                cmd.Parameters.Add(p5);
                cmd.Parameters.Add(p6);
                cmd.Parameters.Add(p7);
                cmd.Parameters.Add(p8);


                DBHelper.Instance.ExecuteDB(cmd);
                MainForm.Instance.setCbbClass();
            
        }
        private void OK_ClickAdd(object sender, EventArgs e)
        {

            bool valid = true;
            bool check = true;
            string mssv = tbMasv.Text;
            string Name = tbName.Text;
            string LopSH = cbLop.Text;
            bool Gender = maleB.Checked ? true : false;
            bool CCCD = CheckCmnd.Checked ? true : false;
            bool Anh = CheckAnh.Checked ? true : false;
            bool Hocba = checkHB.Checked ? true : false;
            float DTB ;
            float.TryParse(TbDiem.Text, out DTB);
            DateTime NS = dateTimePicker1.Value;
            NS=Convert.ToDateTime(NS.ToShortDateString());

            DataTable dt = DBHelper.Instance.GetRecordsx("select MSSV from SV ");
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr[0].ToString());
            }
            foreach (string s in list)
                {
                    if (s==mssv)
                    {
                        MessageBox.Show("MSSV da ton tai");
                        check = false;
                        valid = false;
                        break;
                    }
                }
            if (valid)
                {
                    if (mssv != "" && Name != "" && LopSH != "")
                    {
                        Insert(mssv, Name, LopSH, Gender, NS, Anh, Hocba, CCCD, DTB,true);
                    }
                    else
                    {
                        MessageBox.Show("Ban Chua Dien Du Thong Tin");
                        check = false;
                    }
                }
                if (check)
                {

                    this.Dispose();
                    MainForm.Instance.Search_Click(sender, e);
                }
            
        }
        private void OK_ClickEdit(object sender, EventArgs e)
        {

            bool valid = true;
            bool check = true;
            string mssv = tbMasv.Text;
            string Name = tbName.Text;
            string LopSH = cbLop.Text;
            bool Gender = maleB.Checked ? true : false;
            bool CCCD = CheckCmnd.Checked ? true : false;
            bool Anh = CheckAnh.Checked ? true : false;
            bool Hocba = checkHB.Checked ? true : false;
            float DTB;
            float.TryParse(TbDiem.Text, out DTB);
            DateTime NS = dateTimePicker1.Value;
            NS=Convert.ToDateTime(NS.ToShortDateString());
            Insert(mssv, Name, LopSH, Gender, NS, Anh, Hocba, CCCD, DTB, false);
            this.Dispose();
            MainForm.Instance.Search_Click(sender, e);

        }

        private void OK_Click(object sender, EventArgs e)
        {

        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

      
    }
}
