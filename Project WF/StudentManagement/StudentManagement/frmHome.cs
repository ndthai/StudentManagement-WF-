using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            frmStudent frmStu = new frmStudent();
            loadMdiChild(frmStu);

        }

        private void hệThốngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {

        }
        private void loadMdiChild(Form frm)
        {
            IsMdiContainer = true;
            frm.MdiParent = this;
            frm.StartPosition = FormStartPosition.Manual;
            frm.Dock = DockStyle.Fill;
            if (ActiveMdiChild != null)
            {
                Form frmCurrent = this.ActiveMdiChild;
                if (frmCurrent.Name != frm.Name)
                {
                    frmCurrent.Close();
                    frm.Show();
                }
            }
            else
            {
                frm.Show();
            }
        }

        private void frmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát?", "Hệ thống", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void sInhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudent frmStu = new frmStudent();
            loadMdiChild(frmStu);
        }

        private void điểmDanhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDiemDanh fDD = new frmDiemDanh();
            loadMdiChild(fDD);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin fLogin = new frmLogin();
            fLogin.Show();
            this.Hide();
        }

        private void frmHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
