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
    public partial class frmHeDaoTao : Form
    {
        dbStudentManagementDataContext _ctx = new dbStudentManagementDataContext();
        public frmHeDaoTao()
        {
            InitializeComponent();
            dgvHeDaoTao.AutoGenerateColumns = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void frmHeDaoTao_Load(object sender, EventArgs e)
        {
            load_HDT();
        }
        public void load_HDT()
        {
            var hdt = from h in _ctx.HeDaoTaos select new
            {
                maHe = h.maHe,
                tenHeDaoTao = h.tenHeDaoTao,
                soHocKy = h.soHocKy,
                trangThai = h.trangThai == 1?"Hiện":"Ẩn"
            };

            dgvHeDaoTao.DataSource = hdt;
        }
    }
}
