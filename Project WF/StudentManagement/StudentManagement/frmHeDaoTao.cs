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

        //Lấy thông tin ở bảng
        public void detal_hdt()
        {
            if (dgvHeDaoTao.CurrentRow != null)
            {
                var row = dgvHeDaoTao.CurrentRow;
                txtId.Text = row.Cells[0].Value.ToString();
                txtName.Text = row.Cells[1].Value.ToString();
                txtSem.Text = row.Cells[2].Value.ToString();
                if (row.Cells[3].Value.ToString() == "Hiện")
                {
                    rdoShow.Checked = true;
                }
                else
                {
                    rdoHide.Checked = true;
                }
            }
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
        //Thêm
        private void button1_Click(object sender, EventArgs e)
        {
            string err = "";
            if (txtName.Text =="")
            {
                err += "\nTên hệ đào tạo không được để trống";
            }
            if (txtSem.Text == "")
            {
                err += "\nSố học kỳ không được để trống";
            }
            else
            {
                try
                {
                    if (int.Parse(txtSem.Text) <= 0)
                    {
                        err += "\nSố học kỳ phải lớn hơn 0";
                    }
                }
                catch (Exception)
                {
                    err += "\nSố học kỳ phải là số";
                }
            }

            if (err.Length == 0)
            {
                HeDaoTao hdt = new HeDaoTao();
                hdt.tenHeDaoTao = txtName.Text;
                hdt.soHocKy = Convert.ToInt32(txtSem.Text);
                if (rdoShow.Checked)
                {
                    hdt.trangThai = 1;
                }
                else
                {
                    hdt.trangThai = 0;
                }
                //Thêm và DB
                _ctx.HeDaoTaos.InsertOnSubmit(hdt);
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
                load_HDT();
                resetForm();
            }
            else
            {
                MessageBox.Show(err, "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void frmHeDaoTao_Load(object sender, EventArgs e)
        {
            load_HDT();
        }
        //Load thông tin ra bảng
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
        //
        public void resetForm()
        {
            txtId.Text = txtName.Text = txtSem.Text = "";
            rdoShow.Checked = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            resetForm();
        }
        //Xóa
        private void button4_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mục muốn xóa", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Hệ thống", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //Tìm id đối tượng cần xóa
                    var id = int.Parse(dgvHeDaoTao.CurrentRow.Cells[0].Value.ToString());
                    var hdt = _ctx.HeDaoTaos.SingleOrDefault(x => x.maHe == id);
                    //xóa
                    _ctx.HeDaoTaos.DeleteOnSubmit(hdt);
                    //lưu
                    _ctx.SubmitChanges();
                    //load lại

                    load_HDT();
                }
            }
        }
        //hiện thông tin từ bảng ra form
        private void dgvHeDaoTao_SelectionChanged(object sender, EventArgs e)
        {
            detal_hdt();
        }
        //sửa
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn mục muốn sửa", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //lấy bản ghi cũ
                var old_hdt = (from hdt in _ctx.HeDaoTaos
                               where hdt.maHe == int.Parse(txtId.Text)
                               select hdt).SingleOrDefault();
                //update data
                old_hdt.tenHeDaoTao = txtName.Text;
                old_hdt.soHocKy = Convert.ToInt32(txtSem.Text);
                if (rdoShow.Checked)
                {
                    old_hdt.trangThai = 1;
                }
                else
                {
                    old_hdt.trangThai = 0;
                }
                //lưu
                _ctx.SubmitChanges();
                load_HDT();
            }
            
        }
    }
}
