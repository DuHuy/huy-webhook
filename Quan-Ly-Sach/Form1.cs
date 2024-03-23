using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Quan_Ly_Sach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        XmlDocument doc = new XmlDocument();
        XmlElement root;
        string fileName = @"F:\cSao\Quan-Ly-Sach\Sach.xml";

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThi(dgv);
        }
        public void HienThi(DataGridView dgv)// tham so la mot doi tuong data gridview
        {
            doc.Load(fileName);// load tệp xml
            root = doc.DocumentElement;// xác định node gốc

            XmlNodeList ds = root.SelectNodes("sach");// lay danh sách(ds) các node có tên là sach
            int sd = 0;//lưu chỉ số dòng để hiển thị theo từng dòng trong datagridview
            foreach (XmlNode item in ds)// duyệt từng node trong danh sách vừa có
            {
                dgv.Rows.Add();// tạo 1 dòng trắng trên data gridview
                dgv.Rows[sd].Cells[0].Value = item.SelectSingleNode("@masach").Value;
                // lấy giá trị của thuộc tính masach gán vào cột đầu tiên trên dòng thứ sd
                dgv.Rows[sd].Cells[1].Value = item.SelectSingleNode("tensach").InnerText;
                dgv.Rows[sd].Cells[2].Value = item.SelectSingleNode("soluong").InnerText;
                dgv.Rows[sd].Cells[3].Value = item.SelectSingleNode("dongia").InnerText;
                sd++;// tang số dòng lên để hiển thị node tiếp theo
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            doc.Load(fileName);// load tệp xml
            root = doc.DocumentElement;// xác định node gốc

            //tạo nút sach. Do sach có các phần tử con hoặc thuộc tính nên mình phải dung XmlNode.
            XmlNode sach = doc.CreateElement("sach");

            //tạo nút con của sách là masach

            XmlAttribute masach = doc.CreateAttribute("masach");// tạo 1 attribute nút masach
            masach.Value = txtmaS.Text;//gán giá trị trên ô textbox txtMS cho node mã sách
            sach.Attributes.Append(masach);// gán node masach là node con của node sach

            XmlElement tensach = doc.CreateElement("tensach");// tạo 1 element node ten sach
            tensach.InnerText = txttenS.Text;// gán giá trị trên ô textbox txttenS cho node tensach
            sach.AppendChild(tensach);//gán node ténach là node con của node sach

            XmlElement soluong = doc.CreateElement("soluong");
            soluong.InnerText = txtSL.Text;
            sach.AppendChild(soluong);

            XmlElement dongia = doc.CreateElement("dongia");
            dongia.InnerText = txtDG.Text;
            sach.AppendChild(dongia);

            //sau khi tạo xong node sach, thì thêm sach vào gốc root
            root.AppendChild(sach);
            doc.Save(fileName);//lưu dữ liệu
            HienThi(dgv);// hiển thị lại dữ liệu

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            doc.Load(fileName);// load tệp xml 
            root = doc.DocumentElement;// xác định node gốc
            //láy vị trí cần sửa theo mã sách cũ đưa vào
            XmlNode sachCu = root.SelectSingleNode("sach[@masach ='" + txtmaS.Text + "']");
            if (sachCu != null)
            {
                // taoj 1 nut sachSuaMoi
                XmlNode sachSuaMoi = doc.CreateElement("sach");

                //tạo nút con của sách là masach
                XmlAttribute masach = doc.CreateAttribute("masach");
                masach.InnerText = txtmaS.Text;//gán giá trị cho mã sách
                sachSuaMoi.Attributes.Append(masach);

                XmlElement tensach = doc.CreateElement("tensach");
                tensach.InnerText = txttenS.Text;
                sachSuaMoi.AppendChild(tensach);

                XmlElement soluong = doc.CreateElement("soluong");
                soluong.InnerText = txtSL.Text;
                sachSuaMoi.AppendChild(soluong);

                XmlElement dongia = doc.CreateElement("dongia");
                dongia.InnerText = txtDG.Text;
                sachSuaMoi.AppendChild(dongia);

                //thay thế sách cũ bằng sách mới(sửa )
                root.ReplaceChild(sachSuaMoi, sachCu);
                doc.Save(fileName);//lưu lại
                HienThi(dgv);
            }

        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int t = dgv.CurrentCell.RowIndex;
            txtmaS.Text = dgv.Rows[t].Cells[0].Value.ToString();
            txttenS.Text = dgv.Rows[t].Cells[1].Value.ToString();
            txtSL.Text = dgv.Rows[t].Cells[2].Value.ToString();
            txtDG.Text = dgv.Rows[t].Cells[3].Value.ToString();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            doc.Load(fileName);// load tệp xml
            root = doc.DocumentElement;// xác định node gốc
            XmlNode sachCanXoa = root.SelectSingleNode("sach[@masach ='" + txtmaS.Text + "']");
            if (sachCanXoa != null)
            {
                root.RemoveChild(sachCanXoa);

                doc.Save(fileName);

            }
            dgv.Rows.Clear();
            HienThi(dgv);

        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            dgv.Rows.Clear();
            XmlNode sachCanTim = root.SelectSingleNode("sach[@masach ='" + txtmaS.Text.Trim().ToLower() + "']");
            if (sachCanTim != null)
            {

                // dgv.Rows.Add();//thêm một dòng mới

                //đưa dữ liệu vào dòng vừa tạo
                dgv.Rows[0].Cells[0].Value = sachCanTim.SelectSingleNode("@masach").InnerText;
                dgv.Rows[0].Cells[1].Value = sachCanTim.SelectSingleNode("tensach").InnerText;
                dgv.Rows[0].Cells[2].Value = sachCanTim.SelectSingleNode("soluong").InnerText;
                dgv.Rows[0].Cells[3].Value = sachCanTim.SelectSingleNode("dongia").InnerText;
            }

        }

        
    }
}

