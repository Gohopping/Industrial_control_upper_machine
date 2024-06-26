using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;//头文件

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            serialPort1.Encoding = Encoding.GetEncoding("GB2312");
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void port_DataReceived(object sender,SerialDataReceivedEventArgs e) //接收函数
        {
            try
            {
                string recive_data;
                recive_data = serialPort1.ReadExisting();
                textBox1.AppendText(recive_data);
                textBox1.AppendText("\r\n");
            }
            catch{ }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //扫描串口
        private void button1_Click(object sender, EventArgs e)
        {
            SearchAnAddSerialToComboBox(serialPort1, comboBox1);
        }

        private void SearchAnAddSerialToComboBox(SerialPort Myprot, ComboBox MyBox)//搜索串口函数
        {   //将可用的串口号添加到ComboBox
            string[] NmberOfport = new string[20];//最多容纳20个，太多会卡，影响效率
            string MidString1;//中间数组，用于缓存
            MyBox.Items.Clear();//清空combobox的内容
            for (int i = 1; i < 20; i++)
            {
                try //核心是靠try和catch完成遍历
                {
                    MidString1 = "COM" + i.ToString(); //将串口名字赋值给MidString1
                    Myprot.PortName = MidString1;      //将MidString1赋给Myprot.PortName
                    Myprot.Open();                     //如果失败后面代码不执行了
                    NmberOfport[i - 1] = MidString1;   //依次将MidString1的字符赋给NmberOfport
                    MyBox.Items.Add(MidString1);       //打开成功添加到下列列表
                    Myprot.Close();                    //关闭
                    MyBox.Text = NmberOfport[i - 1];   //显示最后扫描成功的串口

                }
                catch { };
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //打开串口
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "打开串口")//为0时，表示关闭，此时可以进行打开操作 
            {
                try
                {
                    serialPort1.PortName = comboBox1.Text;//获取端口
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);//设置波特率 
                    serialPort1.DataBits = Convert.ToInt32(comboBox3.Text);//设置数据位
                    serialPort1.StopBits = (StopBits)Convert.ToInt32(comboBox4.Text);//设置停止位
                    serialPort1.Open();//打开串口
                    button2.Text = "关闭串口";
                }
                catch
                {
                    MessageBox.Show("串口打开错误");
                }
            }
            else //为1时，表示开启，此时可以进行关闭操作
            {
                try 
                {
                    serialPort1.Close();//关闭串口
                    button2.Text = "打开串口";//置位为0，表示状态为关闭

                } 
                catch { }
            }
        }
        //清空
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //接收数据
        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    if (radioSendHex.Checked) //十六进制发送数据
                    {
                        Byte[] strbyte = HexStringToBytes(textBox2.Text);
                        serialPort1.Write(strbyte, 0, strbyte.Length);
                    }
                    else //acsll发送数据
                    {
                        serialPort1.Write(textBox2.Text);
                    }
                    textBox1.AppendText("[" + DateTime.Now + "]发送->:" + textBox2.Text + "\r\n");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("错误：" + ex.Message);
                }
            }
            else 
            {
                MessageBox.Show("请打开串口！");
            }
        }
        //接收HEX格式数据
        private byte[] HexStringToBytes(string hs) //十六进制字符
        {
            string a = hs.Replace(" ", "");
            int bytelength = 0;
            if (a.Length % 2 == 0)
            {
                bytelength = a.Length / 2;
            }
            else 
            {
                bytelength = a.Length / 2 + 1;
            }
            byte[] b = new byte[bytelength];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < bytelength; i++)
            {
                if (i == bytelength - 1)
                {
                    b[i] = Convert.ToByte(a.Substring(i * 2), 16);
                }
                else 
                {
                    b[i] = Convert.ToByte(a.Substring(i * 2, 2), 16);
                }
            }
            //按照指定编码将字节数组变位字符串
            return b;
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1hex_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2ascii_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
