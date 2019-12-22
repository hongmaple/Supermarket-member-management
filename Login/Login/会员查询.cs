using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;//把数据导出到文件中的命名空间
using System.Drawing.Printing;//打印

namespace Login
{
    public partial class 会员查询 : Form
    {
        public 会员查询()
        {
            InitializeComponent();
            this.printDocument1.OriginAtMargins = true;//启用页边距
            this.pageSetupDialog1.EnableMetric = true; //以毫米为单位
        }
        DataSet huiyuanchaxun = new DataSet();
        DBTools cha = new DBTools();
        private void button5_Click(object sender, EventArgs e)
        {
            try 
	    {
                    string tj = "";
                    int typeid =int.Parse(comboBox1.SelectedValue.ToString());
                    if (textBox1.Text=="")
                    {
                        if (typeid>0)
                        {
                            tj = "cardType=" + typeid;
                        }
                    }
                    else if (IsNumber(textBox1.Text))
                    {
                        tj = "id="+int.Parse(textBox1.Text);
                    }
                        shaxuan.RowFilter = tj;
                        if (shaxuan.Count==0)//判断是否查到数据
                        {
                            MessageBox.Show("未查到相关信息");
                        }
	                 }
	      catch (Exception)
	      {
		
	      }
        }
        public bool IsNumber(string str_number)//判断价格框是否为数字
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"^[0-9]*$");
            //return System.Text.RegularExpressions.Regex.IsMatch(str_number, @"[1-9]\d*.\d*|0.\d*[1-9]\d*");//只能允许价格框输入小数和整数
        }
        DataView shaxuan = null;
        private void 会员查询_Load(object sender, EventArgs e)
        {
            string sql1 = string.Format(@"SELECT [MemberInfo].[id]
                                          ,[name]
                                          ,CardType.typeName
                                          ,[point]
                                          ,[Balance]
                                          ,[MemberInfo].cardType
                                      FROM [Member].[dbo].[MemberInfo]
                                      inner join dbo.CardType
                                      on [MemberInfo].cardType=CardType.id");
            huiyuanchaxun = cha.QuerByAdapter(sql1,"huiyuanxinxi");
            shaxuan=new DataView(huiyuanchaxun.Tables["huiyuanxinxi"]);
            dataGridView1.DataSource=shaxuan;

            string sql2 = "SELECT [id],[typeName] FROM [Member].[dbo].[CardType]";
            huiyuanchaxun = cha.QuerByAdapter(sql2,"huiyuanjibie");
            DataRow hjfdh = huiyuanchaxun.Tables["huiyuanjibie"].NewRow();
            hjfdh[0] = -1;
            hjfdh[1] = "所有级别";
            huiyuanchaxun.Tables["huiyuanjibie"].Rows.InsertAt(hjfdh, 0);//添加到表的第一行位置InsertAt将新行插入到指定位置
            comboBox1.DisplayMember = "typeName";
            comboBox1.ValueMember = "id";
            comboBox1.DataSource = huiyuanchaxun.Tables["huiyuanjibie"];
        }

        private void button1_Click(object sender, EventArgs e)//退出
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            高级查询 cha = new 高级查询();
            cha.Slave2MainDele += textChange;  //总之就是先把form2里的这个事件注册为form1里的内容
            cha.ShowDialog(this);
            this.gaojichaxun();
        }
        string gaojichaxuntj = "";
        public void textChange(string msg)
        {
            gaojichaxuntj = msg;
        }
        public void gaojichaxun() 
        {
            try
            {
                shaxuan.RowFilter = gaojichaxuntj;
                if (shaxuan.Count == 0)//判断是否查到数据
                {
                    MessageBox.Show("未查到相关信息");
                }
            }
            catch (Exception)
            {

            }
        }
/// <summary>
        /// 导出Excel文件
        /// </summary>
        /// /// <param name="dataSet"></param>
        /// <param name="dataTable">数据集</param>
        /// <param name="isShowExcle">导出后是否打开文件</param>
        /// <returns></returns>
        //public static bool DataTableToExcel(string filePath, System.Data.DataTable dataTable, bool isShowExcle)
        //{
        //    //System.Data.DataTable dataTable = dataSet.Tables[0];
        //    int rowNumber = dataTable.Rows.Count;
        //    int columnNumber = dataTable.Columns.Count;
        //    int colIndex = 0;
 
        //    if (rowNumber == 0)
        //    {
        //        return false;
        //    }
 
        //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbook workbook = excel.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
        //    excel.Visible = isShowExcle;
        //    Microsoft.Office.Interop.Excel.Range range;
 
 
        //    foreach (DataColumn col in dataTable.Columns)
        //    {
        //        colIndex++;
        //        excel.Cells[1, colIndex] = col.ColumnName;
        //    }
 
        //    object[,] objData = new object[rowNumber, columnNumber];
 
        //    for (int r = 0; r < rowNumber; r++)
        //    {
        //        for (int c = 0; c < columnNumber; c++)
        //        {
        //            objData[r, c] =dataTable.Rows[r][c];
        //        }
        //    }
 
        //    range = worksheet.get_Range(excel.Cells[2, 1], excel.Cells[rowNumber + 1, columnNumber]);
 
        //    range.Value2 = objData;
 
        //    range.NumberFormatLocal = "@";
 
        //    worksheet.SaveAs(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
 
        //   //excel.Quit();
        //    return true;
        //}//导出文件方法一

        private void button2_Click(object sender, EventArgs e)//导出文件方法二
        {
            string fileName = "";
    string saveFileName = "";
    SaveFileDialog saveDialog = new SaveFileDialog();
    saveDialog.DefaultExt = "xlsx";
    saveDialog.Filter = "Excel文件|*.xlsx";
    saveDialog.FileName = fileName;
    saveDialog.ShowDialog();
    saveFileName = saveDialog.FileName;
    if (saveFileName.IndexOf(":") < 0) return; //被点了取消
    Microsoft.Office.Interop.Excel.Application xlApp = 
                        new Microsoft.Office.Interop.Excel.Application();
    if (xlApp == null)
    {
        MessageBox.Show("无法创建Excel对象，您的电脑可能未安装Excel");
        return;
    }
    Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    Microsoft.Office.Interop.Excel.Workbook workbook = 
                workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
    Microsoft.Office.Interop.Excel.Worksheet worksheet = 
                (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 
    //写入标题             
    for (int i = 0; i < dataGridView1.ColumnCount; i++)
    { worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText; }
    //写入数值
    for (int r = 0; r < dataGridView1.Rows.Count; r++)
    {
        for (int i = 0; i < dataGridView1.ColumnCount; i++)
        {
            worksheet.Cells[r + 2, i + 1] = dataGridView1.Rows[r].Cells[i].Value;
        }
        System.Windows.Forms.Application.DoEvents();
    }
    worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
    MessageBox.Show(fileName + "资料保存成功", "提示", MessageBoxButtons.OK);
    if (saveFileName != "")
    {
        try
        {
            workbook.Saved = true;
            workbook.SaveCopyAs(saveFileName);  //fileSaved = true;                 
        }
        catch (Exception ex)
        {//fileSaved = false;                      
            MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
        }
    }
    xlApp.Quit();
    GC.Collect();//强行销毁           

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //打印内容 为 自定义文本内容 
            Font font = new Font("宋体", 12);
            Brush bru = Brushes.Blue;
            for (int i = 1; i <= 5; i++)
            {
                e.Graphics.DrawString("Hello world ", font, bru, i * 20, i * 20);
            }
        }

        //打印设置
        private void btnSetPrint_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog1.ShowDialog();
        }

        //打印预览
        private void btnPrePrint_Click(object sender, EventArgs e)
        {
            this.printPreviewDialog1.ShowDialog();
        }
 
    //打印
    private void button3_Click(object sender, EventArgs e)
        {
            if (this.printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        this.button5_Click(sender, e);
    }

    private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
    {
        this.button5_Click(sender, e);
    }

    private void label29_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void label29_MouseEnter(object sender, EventArgs e)
    {
        label7.Visible = true;
        label7.Text = "关闭";
        label29.ForeColor = Color.Blue;
    }

    private void label29_MouseLeave(object sender, EventArgs e)
    {
            label7.Visible = false;
            label29.ForeColor = Color.White;
        
    }
    Point mouseOff;//鼠标移动的坐标
    bool leftFalg;//标记为是否为左键选中
    private void 神一样的登录界面_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            mouseOff = new Point(-e.X, -e.Y);//得到变量的值
            leftFalg = true;//点击左键，按下鼠标时标记为true
        }
    }

    private void 神一样的登录界面_MouseMove(object sender, MouseEventArgs e)
    {
        if (leftFalg)
        {
            Point mouseSet = Control.MousePosition;
            mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
            Location = mouseSet;
        }
    }

    private void 神一样的登录界面_MouseUp(object sender, MouseEventArgs e)
    {
        if (leftFalg)
        {

            leftFalg = false;//释放鼠标后标记为false

        }
    }
    }
}
