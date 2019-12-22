using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using AForge
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using AForge.Controls;

namespace Login
{
    public partial class ImgCaiji : Form
    {
        //视频输入设备（摄像头）的集合

        public FilterInfoCollection Cameras = null;

        //本程序使用的那个摄像头

        public VideoCaptureDevice Cam = null;

        ///<summary>

        ///窗体的构造函数：

        /// Load事件用于在加载窗体时获取摄像头设备

        /// FormClosed事件用于在直接关闭窗体时关闭摄像头，释放资源

        ///</summary>


        public ImgCaiji()
        {
            InitializeComponent();
            this.Load += ImgCaiji_Load;

            this.FormClosed +=  ImgCaiji_FormClosed;
        }
          ///<summary>

        ///窗体构造函数的Load事件处理程序

        ///用于获取摄像头设备

        ///</summary>

        ///<paramname="sender"></param>

        ///<paramname="e"></param>

        private void ImgCaiji_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            
            try {

                //1、获取并枚举所有摄像头设备

                Cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                //2、判断设备个数，选择某一设备

                if (Cameras.Count >0) {

                    button1.Enabled = true;

                    Cam = new VideoCaptureDevice(Cameras[0].MonikerString);

                    Cam.NewFrame +=Cam_NewFrame;

                } else {

                    MessageBox.Show("未找到视频输入设备！");

                }

            } catch (Exception ex) {

                MessageBox.Show(ex.ToString());

            }
        }
           ///<summary>

        ///摄像头设备Cam的NewFrame事件处理程序

        ///用于实时显示捕获的视频流

        ///</summary>

        ///<paramname="sender"></param>

        ///<paramname="eventArgs"></param>

        private void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs) {

            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }
        bool isshowed = true;
        //新帧的触发函数
        private void show_video(object sender, NewFrameEventArgs eventArgs)
        {
            if (isshowed)
            {
                Member_photo_collection aa = new Member_photo_collection();
                isshowed = false;
            }
            Bitmap bitmap = eventArgs.Frame;    //获取到一帧图像
            pictureBox1.Image = Image.FromHbitmap(bitmap.GetHbitmap());
            //if (is_record_video)
            //{
            //    writer.WriteVideoFrame(bitmap);
            //}
        }
          ///<summary>

        ///点击按钮的事件处理程序

        ///用于控制摄像头的开启、关闭

        ///</summary>

        ///<paramname="sender"></param>

        ///<paramname="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "开启摄像头")
            {

                button1.Text = "关闭摄像头";
                Cam.Start();

            } else {

                button1.Text = "开启摄像头";

                Cam.Stop();

            }
        }
        ///<summary>

        ///在关闭窗体的事件处理程序中，释放摄像头

        ///</summary>

        ///<paramname="sender"></param>

        ///<paramname="e"></param>

        private void ImgCaiji_FormClosed(object sender, FormClosedEventArgs e)
        {
               if (Cam !=null &&Cam.IsRunning) {

                Cam.Stop();

            }

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}