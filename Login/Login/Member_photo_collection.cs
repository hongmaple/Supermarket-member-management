
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
 
//using AForge
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using AForge.Controls;
using System.IO;

namespace Login
{
    public partial class Member_photo_collection : Form
    {
        ////第二步：声明一个委托类型的事件  
        //public event setTextValue setFormTextValue;  
        public delegate void Slave2MainDelegate(string lujin); //定义委托   
        public Member_photo_collection()
        {
            InitializeComponent();
        }
 
        //关闭窗口响应函数
        private void button2_Click(object sender, EventArgs e)
        {

        }
 
        private FilterInfoCollection videoDevices;  //摄像头设备
        private VideoCaptureDevice videoSource;     //视频的来源选择
        private VideoSourcePlayer videoSourcePlayer;    //AForge控制控件
        private VideoFileWriter writer;     //写入到视频
        private bool is_record_video = false;   //是否开始录像
        System.Timers.Timer timer_count;
        int tick_num = 0;
 
        //窗体初始化函数
        private void Form1_Load(object sender, EventArgs e)
        {
            this.label5.Visible = false;

            this.videoSourcePlayer = new AForge.Controls.VideoSourcePlayer();
            this.videoSource = new VideoCaptureDevice();
            this.writer = new VideoFileWriter();

            //设置视频编码格式
            this.comboBox2.Items.Add("Raw");
            this.comboBox2.Items.Add("MPEG2");
            this.comboBox2.Items.Add("FLV1");
            this.comboBox2.Items.Add("H263p");
            this.comboBox2.Items.Add("MSMPEG4v3");
            this.comboBox2.Items.Add("MSMPEG4v2");
            this.comboBox2.Items.Add("WMV2");
            this.comboBox2.Items.Add("WMV1");
            this.comboBox2.Items.Add("MPEG4");
            this.comboBox2.SelectedIndex = 1;
 
            //设置视频来源
            try
            {
                // 枚举所有视频输入设备
                videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
 
                if (videoDevices.Count == 0)
                    throw new ApplicationException();   //没有找到摄像头设备
 
                foreach (FilterInfo device in videoDevices)
                {
                    this.comboBox1.Items.Add(device.Name);
                }
                //this.comboBox_camera.SelectedIndex = 0;   //注释掉，选择摄像头来源的时候才会才会触发显示摄像头信息
            }
            catch (ApplicationException)
            {
                videoDevices = null;
                MessageBox.Show("没有找到摄像头设备", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
 
            //秒表
            this.timer_count = new System.Timers.Timer();   //实例化Timer类，设置间隔时间为10000毫秒；
            this.timer_count.Elapsed += new System.Timers.ElapsedEventHandler(tick_count);   //到达时间的时候执行事件；
            this.timer_count.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；
            this.timer_count.Interval = 1000;
        }
 
        //视频源选择下拉框选择之后的响应函数
        private void comboBox_camera_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selected_index = this.comboBox1.SelectedIndex;
            this.videoSource = new VideoCaptureDevice(videoDevices[selected_index].MonikerString);
            // set NewFrame event handler
            videoSource.NewFrame += new NewFrameEventHandler(show_video);
            videoSource.Start();
            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();
            this.label5.Text = "连接中...";
            this.label5.Visible = true;
            isshowed = true;
        }
 
        bool isshowed = true;
        //新帧的触发函数
        private void show_video(object sender, NewFrameEventArgs eventArgs)
        {
            if (isshowed)
            {
                Member_photo_collection aa = new Member_photo_collection();
                aa.label5.Visible = false;
                isshowed = false;
            }
            Bitmap bitmap = eventArgs.Frame;    //获取到一帧图像
            pictureBox1.Image = Image.FromHbitmap(bitmap.GetHbitmap());        
            if (is_record_video)
            {
                writer.WriteVideoFrame(bitmap);
            }
        }
 
        //拍摄图像按钮响应函数
        private void button1_Click(object sender, EventArgs e)
        {
            //拍照
            if (this.videoSource.IsRunning && this.videoSourcePlayer.IsRunning)
            {
                //if (!Directory.Exists(_capturePath))
                //    Directory.CreateDirectory(_capturePath);
                //if (!Directory.Exists(_videoPath))
                //    Directory.CreateDirectory(_videoPath);
                pictureBox2.Image = this.videoSourcePlayer.GetCurrentVideoFrame();
            }
            else
                MessageBox.Show("摄像头没有运行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
 
        //开始录像按钮响应函数
        private void button_start_Click(object sender, EventArgs e)
        {
            int width = 640;    //录制视频的宽度
            int height = 480;   //录制视频的高度
            int fps = 9;
 
            //创建一个视频文件
            String video_format = this.comboBox2.Text.Trim(); //获取选中的视频编码
            if (this.videoSource.IsRunning && this.videoSourcePlayer.IsRunning)
            {
                if (-1 != video_format.IndexOf("MPEG"))
                {
                    writer.Open("test.avi", width, height, fps, VideoCodec.MPEG4);
                }
                else if (-1 != video_format.IndexOf("WMV"))
                {
                    writer.Open("test.wmv", width, height, fps, VideoCodec.WMV1);
                }
                else
                {
                    writer.Open("test.mkv", width, height, fps, VideoCodec.Default);
                }
            }
            else
                MessageBox.Show("没有视频源输入，无法录制视频。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
 
            timer_count.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            this.label5.Visible = true;
            this.label5.Text = "REC";
            this.is_record_video = true;
        }
 
 
        //停止录制视频响应函数
        private void button_stop_Click(object sender, EventArgs e)
        {
            this.label5.Visible = false;
            this.is_record_video = false;
            this.writer.Close();
            this.timer_count.Enabled = false;
            tick_num = 0;
        }
 
        //暂停按钮响应函数
        private void button3_Click(object sender, EventArgs e)
        {
            if (this.button3.Text.Trim() == "暂停录像")
            {
                this.is_record_video = false;
                this.label5.Visible = false;
                this.button3.Text = "恢复录像";
                timer_count.Enabled = false;    //暂停计时
                return;
            }
            if (this.button3.Text.Trim() == "恢复录像")
            {
                this.is_record_video = true;
                timer_count.Enabled = true;     //恢复计时
                this.label5.Visible = true;
                this.button3.Text = "暂停录像";
            }
        }
 
        //计时器响应函数
        public void tick_count(object source, System.Timers.ElapsedEventArgs e)
        {
            tick_num++;
            int temp = tick_num;
 
            int sec = temp % 60;
 
            int min = temp / 60;
            if (60 == min)
            {
                min = 0;
                min++;
            }
 
            int hour = min / 60;
 
            String tick = hour.ToString() + "：" + min.ToString() + "：" + sec.ToString();
            Member_photo_collection aa = new Member_photo_collection();
            aa.label4.Text= tick;
        }

        private void Member_photo_collection_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (videoSource != null && videoSource.IsRunning)
            {

                videoSource.Stop();

            }
        }
        //// 第一步：声明一个委托。（根据自己的需求）  
        //public delegate void setTextValue(string textValue);  
        public Slave2MainDelegate Slave2MainDele;//定义委托实例   
        private void button6_Click(object sender, EventArgs e)
        {
            //拍照
            if (this.videoSource.IsRunning && this.videoSourcePlayer.IsRunning)
            {
                string fileName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ff") + ".jpeg";
                  string fullPath = Application.StartupPath + "img\\";

               // Directory类是用于文件夹操作,可以带来很多便利.
               // Directory是位于System.IO的,所以为了方便使用,建议先引用System.IO
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

                Bitmap bitmap = this.videoSourcePlayer.GetCurrentVideoFrame();
                bitmap.Save(fullPath+fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitmap.Dispose();
                Slave2MainDele.Invoke(fullPath + fileName); 
                MessageBox.Show("保存成功");
                videoSource.Stop();
                this.Close();
            }
            else
                MessageBox.Show("摄像头没有运行", "错误", MessageBoxButtons.OK, MessageBoxIcon.Information); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            videoSource.Stop();
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

        private void label29_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       System.Drawing.Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        private void 神一样的登录界面_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new System.Drawing.Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        private void 神一样的登录界面_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                System.Drawing.Point mouseSet = Control.MousePosition;
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
