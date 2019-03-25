using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManagement
{
    public partial class frmStudent : Form
    {
        dbStudentManagementDataContext dbcon = new dbStudentManagementDataContext();
        private string pathApp = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
        public frmStudent()
        {
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ten = txttimkiem.Text;
            var timkiem = from s in dbcon.SinhViens
                          where s.tenSV.StartsWith(ten)
                          select new
                          {
                              masv = s.maSV,
                              hosv = s.hoSV,
                              tensv = s.tenSV,
                              gioitinh = (bool)s.gioiTinh ? "Nam" : "Nữ",
                              sinhNhat = s.sinhNhat,
                              diaChi = s.diaChi,
                              soCMT = s.soCMT,
                              sDT = s.sDT,
                              email = s.email,
                              malophoc = s.LopHoc.tenLop,
                              trangThai = s.trangThai == 1 ? "Đang học" : s.trangThai == 2 ? "Nghỉ học" : "Đã tốt nghiệp"

                          };
            dgvsinhvien.DataSource = timkiem;

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmStudent_Load(object sender, EventArgs e)
        {
            load_sv();
            load_combo_lop();
        }

        public void load_sv()
        {
            var sv = (from s in dbcon.SinhViens
                      select new
                      {
                          masv = s.maSV,
                          hosv = s.hoSV,
                          tensv = s.tenSV,
                          gioitinh = (bool)s.gioiTinh ? "Nam" : "Nữ",
                          sinhNhat = s.sinhNhat,
                          diaChi = s.diaChi,
                          soCMT = s.soCMT,
                          sDT = s.sDT,
                          email = s.email,
                          malophoc = s.LopHoc.tenLop,
                          anh = s.anh,
                         trangThai = s.trangThai == 1 ? "Đang học" : s.trangThai == 2 ? "Nghỉ học" : "Đã tốt nghiệp"

                     }).ToList();
            dgvsinhvien.DataSource = sv;
        }

        public void load_combo_lop()
        {
            cbolop.DataSource = dbcon.LopHocs;
            cbolop.DisplayMember = "tenLop";
            cbolop.ValueMember = "maLopHoc";
        }

        private void dgvsinhvien_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvsinhvien.CurrentRow != null)
            {
                var row = dgvsinhvien.CurrentRow;
                txtmasv.Text = row.Cells[0].Value.ToString();
                txthosv.Text = row.Cells[1].Value.ToString();
                txttensv.Text = row.Cells[2].Value.ToString();
                if (row.Cells[3].Value.ToString() == "Nam")
                {
                    rdoNam.Checked = true;
                }
                else
                {
                    rdoNu.Checked = true;
                }
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[4].Value.ToString());
                txtdiachi.Text = row.Cells[5].Value.ToString();
                txtcmt.Text = row.Cells[6].Value.ToString();
                txtsdt.Text = row.Cells[7].Value.ToString();
                txtemail.Text = row.Cells[8].Value.ToString();
                cbolop.Text = row.Cells[9].Value.ToString();
                ptbsinhvien.Image = Image.FromFile(pathApp + @"\Image\" + row.Cells[10].Value.ToString());
                if (row.Cells[11].Value.ToString() == "Đang học")
                {
                    rdodanghoc.Checked = true;
                }
                else if (row.Cells[9].Value.ToString() == "Nghỉ học")
                {
                    rdonghihoc.Checked = true;
                }
                else 
                {
                    rdototnghiep.Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var malop = this.cbolop.GetItemText(this.cbolop.SelectedValue);
            int maLop = int.Parse(malop);
            string err = "";
            if (txthosv.Text == "")
            {
                err += "\nHọ sinh viên không được để trống";
            }
            else
            {
                if (!checkName(txthosv.Text))
                {
                    err += "\n Email sai định dạng";
                }
            }
            if (txttensv.Text == "")
            {
                err += "\nTên sinh viên không được để trống";
            }
            else
            {
                if (!checkName(txttensv.Text))
                {
                    err += "\n Email sai định dạng";
                }
            }
            if (txtdiachi.Text == "")
            {
                err += "\nĐịa chỉ không được để trống";
            }
            if (txtcmt.Text == "")
            {
                err += "\nCMT không được để trống";
            }
            else if (txtcmt.Text.Length != 13 || txtcmt.Text.Length != 9)
            {
                err += "\nCMT phải có 9 hoặc 13 kí tự";
            }
            if (txtsdt.Text == "")
            {
                err += "\nSố điện thoại không được để trống";
            }
            else
            {
                if (!checkphone(txtsdt.Text))
                {
                    err += "\n Email sai định dạng";
                }
            }
            if (txtemail.Text == "")
            {
                err += "\nEmail không được để trống";
            }
            else
            {
                if (!isEmail(txtemail.Text))
                {
                    err += "\n Email sai định dạng";
                }
            }

            if (err.Length == 0)
            {
                SinhVien s = new SinhVien();
                s.hoSV = txthosv.Text;
                s.tenSV = txttensv.Text;
                s.sinhNhat = Convert.ToDateTime(dateTimePicker1.Text);
                s.diaChi = txtdiachi.Text;
                s.email = txtemail.Text;
                s.soCMT = txtcmt.Text;
                s.maLopHoc = maLop;
                s.gioiTinh = rdoNam.Checked ? true : false;
                s.trangThai = rdodanghoc.Checked ? 1 : rdonghihoc.Checked ? 0 : 2;
                var img = ptbsinhvien.Image;
                img.Save(pathApp + @"\Image\" + txtanh.Text);
                s.anh = txtanh.Text;

                //Thêm và DB
                dbcon.SinhViens.InsertOnSubmit(s);
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
                load_sv();
                resetForm();
            } else
            {
                MessageBox.Show(err, "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void resetForm()
        {
            txthosv.Text = txttensv.Text = txtdiachi.Text = txtsdt.Text = txtcmt.Text = txtsdt.Text = txtmasv.Text = txtemail.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var malop = this.cbolop.GetItemText(this.cbolop.SelectedValue);
            int maLop = int.Parse(malop);
            
            if (txtmasv.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mục muốn sửa", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //lấy bản ghi cũ
                var old_sv = (from s in dbcon.SinhViens
                              where s.maSV == int.Parse(txtmasv.Text)
                              select s).SingleOrDefault();
                //update data
                old_sv.hoSV = txthosv.Text;
                old_sv.tenSV = txttensv.Text;
                old_sv.sinhNhat = Convert.ToDateTime(dateTimePicker1.Text);
                old_sv.diaChi = txtdiachi.Text;
                old_sv.email = txtemail.Text;
                old_sv.sDT = txtsdt.Text;
                old_sv.soCMT = txtcmt.Text;
                old_sv.maLopHoc = maLop;
                old_sv.gioiTinh = rdoNam.Checked ? true : false;
                old_sv.trangThai = rdodanghoc.Checked ? 1 : rdonghihoc.Checked ? 0 : 2;
                var img = ptbsinhvien.Image;
                img.Save(pathApp + @"\Image\" + txtanh.Text);
                old_sv.anh = txtanh.Text;

                //lưu
                try
                {
                    dbcon.SubmitChanges();
                    MessageBox.Show("Sửa thành công", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Sửa thất bại", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                load_sv();
            }
        }

        OpenFileDialog openDlg = new OpenFileDialog();
        private void button4_Click(object sender, EventArgs e)
        {
            
            openDlg.Title = "Mở tệp";
            openDlg.Filter = "Files *.jpg | *.jpg";
            openDlg.RestoreDirectory = true;

            openDlg.ShowDialog();
            if (openDlg.FileName != "")
            {
                ptbsinhvien.Image = Image.FromFile(openDlg.FileName);
                txtanh.Text = Path.GetFileName(openDlg.FileName);
            }
        }

        public static bool checkName(string inputName)
        {
            inputName = inputName ?? string.Empty;
            string strRegex = "[A-Z a-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚÝàáâãèéêìíòóôõùúýĂăĐđĨĩŨũƠơƯưẠ-ỹ]+$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputName))
                return (true);
            else
                return (false);
        }

        public static bool checkphone(string inputphone)
        {
            inputphone = inputphone ?? string.Empty;
            string strRegex = "^0(1\\d{9}|[2-9]\\d{8})$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputphone))
                return (true);
            else
                return (false);
        }

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
