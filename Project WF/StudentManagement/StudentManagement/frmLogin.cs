using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class frmLogin : Form
    {
        public static string tenGV;
        dbStudentManagementDataContext _ctx = new dbStudentManagementDataContext();
        public frmLogin()
        {

            InitializeComponent();
            this.groupBox1.BackColor = Color.Transparent;
            this.groupBox2.BackColor = Color.Transparent;
            this.lblLogin.BackColor = Color.Transparent;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            string password = txtPassword.Text;

            string err = "";

            if (email.Length == 0)
            {
                err += "\n Bạn chưa nhập Email";
            }
            else
            {
                if (!isEmail(email))
                {
                    err += "\n Email sai định dạng";
                }
            }

            if (password.Length == 0)
            {
                err += "\n Bạn chưa nhập Password";
            }
            else if (password.Length < 6)
            {
                err += "\n Password chứa ít nhất 6 ký tự";
            }

            if (err.Length == 0)
            {
                var login_gv = (from gv in _ctx.GiaoViens
                               where gv.trangThai == 1 && gv.email == email && gv.password == password
                               select gv).SingleOrDefault();

                if (login_gv == null)
                {
                    MessageBox.Show("Đăng nhập không thành công!", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Properties.Settings.Default.tenGV = login_gv.tenGV;
                    Properties.Settings.Default.Save();
                    frmHome fHome = new frmHome();
                    fHome.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show(err, "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        //Hàm định dạng email
        public static bool isEmail(string inputEmail)
        {
            inputEmail = inputEmail ?? string.Empty;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
