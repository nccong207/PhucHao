using CDTDatabase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace POSApp
{
    public partial class Login : Form
    {
        public DataRow drUser;
        
        Database db = Database.NewDataDatabase();

        public Login()
        {
            InitializeComponent();
            int h = Screen.PrimaryScreen.WorkingArea.Height;
            int w = Screen.PrimaryScreen.WorkingArea.Width;
            this.Width = w;
            this.Height = h;
            numberpad1.TextBox = textBox1;
           
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM DMNhanVien WHERE Ma = '{0}' AND pin = '{1}' AND Ca = '{2}'";
            DataTable dt = db.GetDataTable(string.Format(sql, textBox1.Text, textBox2.Text, ca.Text.ToString()));
            if (dt.Rows.Count > 0)
            {
                drUser = dt.Rows[0];
                this.DialogResult = DialogResult.OK;
                this.Close();
            } else
            {
                MessageBox.Show("Thông tin đăng nhập không đúng. Vui lòng kiểm tra lại.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {

            numberpad1.TextBox = textBox2;
            textBox2.Focus();
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox2.Focus();
            }
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text.Length == 4)
            {
                
                button2_Click(this, new EventArgs());
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
            panel2.Width = this.ClientSize.Width;
            panel2.Height = (this.ClientSize.Height * 10 )/ 100;
            panel2.Location = new Point(0,0);
            
            panel3.Location = new Point(0,panel2.Height);
            panel3.Width = (this.ClientSize.Width * 60) / 100;
            panel3.Height = this.ClientSize.Height;
            
            panel1.Width = (this.ClientSize.Width * 50) / 100;
            panel1.Height = this.ClientSize.Height;
            panel1.Location = new Point(panel3.Width, panel2.Height);
            pictureBox1.Location = new Point(((panel3.Width * 10) / 100),(panel3.Height/2) - pictureBox1.Height);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("1");
        }

        private void textBox1_Enter_1(object sender, EventArgs e)
        {
            numberpad1.TextBox = textBox1;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Red;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.Red;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.BackColor = System.Drawing.Color.Yellow;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackColor = System.Drawing.Color.White;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackColor = System.Drawing.Color.White;
        }
    }
}
