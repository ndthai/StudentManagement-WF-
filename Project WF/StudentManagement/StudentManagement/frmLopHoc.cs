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
    public partial class frmLopHoc : Form
    {
        dbStudentManagementDataContext dbcon = new dbStudentManagementDataContext();
        bool edit = false;
        public frmLopHoc()
        {
            InitializeComponent();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void frmLopHoc_Load(object sender, EventArgs e)
        {
            Load_LopHoc();
            load_combo_khoa();
            load_combo_he();
        }

        public void Load_LopHoc()
        {
            var lh = from l in dbcon.LopHocs
                     select new {
                         maLopHoc = l.maLopHoc,
                         tenLop = l.tenLop,
                         maHe = l.HeDaoTao.tenHeDaoTao,
                         maKhoa = l.Khoa.tenKhoa,
                         khaiGiang = l.khaiGiang,
                         totNghiep = l.totNghiep,
                         trangThai = l.trangThai ==1 ?"Đang học":"Đã tốt nghiệp"
                     };
            dataGridView1.DataSource = lh;
        }

        public void Load_SV_LopHoc(int id)
        {
            var sv = from s in dbcon.SinhViens
                     join l in dbcon.LopHocs on s.maLopHoc equals l.maLopHoc
                     where s.maLopHoc == id
                     select new
                     {
                         maSV = s.maSV,
                         tensv = s.hoSV + s.tenSV,
                         gioitinh = (bool) s.gioiTinh == true?"Nam":"Nữ",
                         sinhNhat = s.sinhNhat,
                         diachi = s.diaChi,
                         CMT = s.soCMT,
                         sdt = s.sDT,
                         email = s.email,
                         trangThai = s.trangThai == 1?"Đang học":"Đã tốt nghiệp",
                         lophoc = l.tenLop
                     };
            dataGridView2.DataSource = sv;  
        }

        public void load_combo_he()
        {
            cboHe.DataSource = dbcon.HeDaoTaos;
            cboHe.DisplayMember = "tenHeDaoTao";
            cboHe.ValueMember = "maHe";
        }

        public void load_combo_khoa()
        {
            cboKhoa.DataSource = dbcon.Khoas;
            cboKhoa.DisplayMember = "tenKhoa";
            cboKhoa.ValueMember = "maKhoa";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var makhoa = this.cboKhoa.GetItemText(this.cboKhoa.SelectedValue);
            int maKhoa = int.Parse(makhoa);
            var mahe = this.cboHe.GetItemText(this.cboHe.SelectedValue);
            int maHe = int.Parse(mahe);
            string err = "";
            if (txtTenLop.Text == "")
            {
                err += "\nTên lớp không được để trống";
            }

            if (err.Length == 0)
            {
                LopHoc lh = new LopHoc();
                lh.tenLop = txtTenLop.Text;
                lh.maKhoa = maKhoa;
                lh.maHe = maHe;
                lh.khaiGiang = Convert.ToDateTime(dateTimePicker1.Text);
                lh.totNghiep = Convert.ToDateTime(dateTimePicker2.Text);
                if (rdodanghoc.Checked)
                {
                    lh.trangThai = 1;
                }
                else
                {
                    lh.trangThai = 2;
                }
                //Thêm và DB
                dbcon.LopHocs.InsertOnSubmit(lh);
                try
                {
                    //lưu thay đổi trên DB
                    dbcon.SubmitChanges();
                    //Thông báo
                    MessageBox.Show("Thêm mới thành công", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Thêm mới thất bại", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                //load
                Load_LopHoc();
                resetForm();
            }
        }
        public void resetForm()
        {
            txtTenLop.Text = txtmalop.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                var row = dataGridView1.CurrentRow;
                txtmalop.Text = row.Cells["maLopHoc"].Value.ToString();
                txtTenLop.Text = row.Cells["tenLop"].Value.ToString();
                cboHe.Text = row.Cells["maHe"].Value.ToString();
                cboKhoa.Text = row.Cells["maKhoa"].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells["khaiGiang"].Value.ToString());
                dateTimePicker2.Value = Convert.ToDateTime(row.Cells["totNghiep"].Value.ToString());
                if (row.Cells[6].Value.ToString() == "Đang học")
                {
                    rdodanghoc.Checked = true;
                }
                else
                {
                    rdototnghiep.Checked = true;
                }
                edit = true;
                Load_SV_LopHoc(int.Parse(txtmalop.Text));
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var makhoa = this.cboKhoa.GetItemText(this.cboKhoa.SelectedValue);
            int maKhoa = int.Parse(makhoa);
            var mahe = this.cboHe.GetItemText(this.cboHe.SelectedValue);
            int maHe = int.Parse(mahe);
            if (txtmalop.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mục muốn sửa", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //lấy bản ghi cũ
                var old_lh = (from lh in dbcon.LopHocs
                               where lh.maLopHoc == int.Parse(txtmalop.Text)
                               select lh).SingleOrDefault();
                //update data
                old_lh.tenLop = txtTenLop.Text;
                old_lh.maKhoa = maKhoa;
                old_lh.maHe = maHe;
                old_lh.khaiGiang = Convert.ToDateTime(dateTimePicker1.Text);
                old_lh.totNghiep = Convert.ToDateTime(dateTimePicker2.Text);
                if (rdodanghoc.Checked)
                {
                    old_lh.trangThai = 1;
                }
                else
                {
                    old_lh.trangThai = 2;
                }
                
                //lưu
                dbcon.SubmitChanges();
                Load_LopHoc();
            }
        }

    }
}
