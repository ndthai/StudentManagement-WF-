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
    public partial class frmKhoa : Form
    {
        dbStudentManagementDataContext _ctx = new dbStudentManagementDataContext();
        public frmKhoa()
        {
            InitializeComponent();
            dgvKhoa.AutoGenerateColumns = false;
        }

        //Load thông tin ra bảng
        public void load_Khoa()
        {
            var hdt = from k in _ctx.Khoas
                      select new
                  {
                      maKhoa = k.maKhoa,
                      tenKhoa = k.tenKhoa,
                      trangThai = k.trangThai == 1 ? "Hiện" : "Ẩn"
                  };

            dgvKhoa.DataSource = hdt;
        }

        private void dgvKhoa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmKhoa_Load(object sender, EventArgs e)
        {
            load_Khoa();
        }
    }
}
