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

        private void button1_Click(object sender, EventArgs e)
        {
            string err = "";
            if (txttenkhoa.Text == "")
            {
                err += "\nTên khoa không được để trống";
            }

            if (err.Length == 0)
            {
                Khoa k = new Khoa();
                k.tenKhoa = txttenkhoa.Text;
                if (rdoShow.Checked)
                {
                    k.trangThai = 1;
                }
                else
                {
                    k.trangThai = 0;
                }
                //Thêm và DB
                _ctx.Khoas.InsertOnSubmit(k);
                try
                {
                    //lưu thay đổi trên DB
                    _ctx.SubmitChanges();
                    //Thông báo
                    MessageBox.Show("Thêm mới thành công", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (Exception)
                {
                    MessageBox.Show("Thêm mới thất bại", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                //load
                load_Khoa();
                resetForm();
            }
            else
            {
                MessageBox.Show(err, "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void resetForm()
        {
            txttenkhoa.Text = "";
            rdoShow.Checked = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtmakhoa.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mục muốn sửa", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //lấy bản ghi cũ
                var old_k = (from k in _ctx.Khoas
                               where k.maKhoa == int.Parse(txtmakhoa.Text)
                               select k).SingleOrDefault();
                //update data
                old_k.tenKhoa = txttenkhoa.Text;
                if (rdoShow.Checked)
                {
                    old_k.trangThai = 1;
                }
                else
                {
                    old_k.trangThai = 0;
                }
                //lưu
                _ctx.SubmitChanges();
                load_Khoa();
            }
        }

        private void dgvKhoa_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKhoa.CurrentRow != null)
            {
                var row = dgvKhoa.CurrentRow;
                txtmakhoa.Text = row.Cells[0].Value.ToString();
                txttenkhoa.Text = row.Cells[1].Value.ToString();
                if (row.Cells[2].Value.ToString() == "Hiện")
                {
                    rdoShow.Checked = true;
                }
                else
                {
                    rdoHide.Checked = true;
                }
            }
        }
    }
}
