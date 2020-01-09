using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Collections;

//+吳佳謙 Ver_1.0.0.0 for FinalProject
namespace WindowsFormsApplication3
{
    public partial class InputMessage : Form
    {
        public Image[] img;//宣告全域陣列
        ImageList imglist = new ImageList();
        ImageList imglist2 = new ImageList();
        FileInfo[] file;
        DirectoryInfo FishInfoOriginal = new DirectoryInfo(@"D:\\原圖\\");
        DirectoryInfo FishInfo = new DirectoryInfo(@"D:\\料理圖\\");
        FileInfo[] FishInfoFile;
        DirectoryInfo FishInfo2 = new DirectoryInfo(@"D:\\testtxt\\");
        FileInfo[] FishInfoFile2;
        
        List<string> strTxt = new List<string>();
        List<string> strSpilt = new List<string>();
        OpenFileDialog dialog = new OpenFileDialog();//宣告開檔指標  
        bool bChangeState = false;//切換自動及手動的旗標

        string[] ss  = new string[999];
        int next = 0;
                
        public InputMessage()
        {
            InitializeComponent();
            button1.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        public void CallCMD()
        {
            CMDModel();                          
        }

        public void CMDModel()
        {
            Process CMD_Process = new Process();//宣告呼叫CMD流程    
               
            Global.Delaytime = 5000;//cmd每個指令間隔時間   
            progressBar1.Value = 0;

            
            pictureBox1.Image = null;//初始化picturebox

            //CMD_Process.StartInfo.FileName = "cmd.exe";
            //CMD_Process.StartInfo.UseShellExecute = false;
            //CMD_Process.StartInfo.RedirectStandardInput = true;
            //CMD_Process.StartInfo.WorkingDirectory = "D:\\YOLO_Base\\darknet-master\\build\\darknet\\x64";
            //CMD_Process.StartInfo.RedirectStandardOutput = true;
            //CMD_Process.StartInfo.CreateNoWindow = false;
            //CMD_Process.Start();
            //Thread.Sleep(Global.Delaytime);
            //CMD_Process.StandardInput.WriteLine("darknet.exe detector test data/fish.data yolov3_fish_train.cfg yolov3_fish_train_last0104.weights -dont_show -ext_output < data/testall/test.txt > results/result.txt");
            ////listBox1.Items.Add("darknet.exe detector test data/fish.data yolov3_fish_train.cfg yolov3_fish_train_last0104.weights -dont_show -ext_output < data/testall/test.txt > results/result.txt");                                                  
            ////listBox1.Items.Add(InputIMG);
            //Thread.Sleep(Global.Delaytime);

            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    string Local = dialog.FileName;
            //    StreamReader ImgPath = new StreamReader(Local, Encoding.Default);
            //    string Imgname = ImgPath.ReadLine();//抓出txt檔案內容
            //    listBox1.Items.Add(Imgname);
            //    pictureBox1.Image = Image.FromFile(Local);
            //    //MessageBox.Show(Local);
            //    listBox1.Items.Add(Local);

            //}
            //else
            //{
            //    MessageBox.Show("找不到魚類照片");
            //}
            //Thread.Sleep(10000);

           


            MessageBox.Show("辨識成功檔案已載入!");

            FishInfoFile = FishInfo.GetFiles("*.jpg");//圖檔名稱           
            //imglist2.ColorDepth = ColorDepth.Depth32Bit;//設定像素
            //imglist2.ImageSize = new Size(256, 256);//設定圖片大小            
            //CMD_Process.StandardInput.WriteLine("exit");
           // CMD_Process.Close();
            for (int i = 0; i < FishInfoFile.Count(); i++) 
            {
                progressBar1.Maximum = FishInfoFile.Count() - 1;
                progressBar1.Value = i;
            //    imglist2.Images.Add(Image.FromFile("D:\\料理圖\\" + FishInfoFile[i]));
            }
            MessageBox.Show("料理圖檔已載入!");

            if (bChangeState)                            
                CompareShow();                            
            else          
                CompareOnce();
                
            
                                                 
        }
        public void CompareOnce()
        {
            //pictureBox1.Image = imglist.Images[next];                
            pictureBox1.Image = Image.FromFile(@"D:\\testresult\\" + file[0].ToString());
            FishInfoFile2 = FishInfo2.GetFiles("*.txt");//txt檔
            listBox1.Items.Clear();
            pictureBox2.Image = null;
            pictureBox3.Image = null;

            string pathimg = Path.GetFileNameWithoutExtension(file[0].ToString());
            for (int k = 0; k < FishInfoFile2.Count(); k++)
            {

                string pathtxt = Path.GetFileNameWithoutExtension(FishInfoFile2[k].ToString());
                if (pathimg == pathtxt)
                {
                    int temp = 0;
                    string tempCompare = "";
                    string str;
                    string Match4 = "";
                    string Match5 = "";
                    string Match4First = "";
                    string Match5First = "";
                    char[] strspecialtext = { '(', ')', '\t', '\r', '\n' };
                    string path = @"D:\\testtxt\\" + FishInfoFile2[k].ToString();
                    StreamReader strTXT = new StreamReader(path);

                    while ((str = strTXT.ReadToEnd()) != null && str != "")
                    {
                        str = str.Replace(" ", "");
                        strTxt = str.Split(strspecialtext).ToList();
                    }
                    Match4First = strTxt[0].Substring(0, strTxt[0].Length - 4);
                    Match5First = strTxt[0].Substring(0, strTxt[0].Length - 5);

                    for (int j = 0; j < strTxt.Count(); j++)
                    {
                        for (int s = 0; s < FishInfoFile.Count(); s++)
                        {
                            string cookingimg = Path.GetFileNameWithoutExtension(FishInfoFile[s].ToString());

                            if (strTxt[j] != "")
                            {
                                Match4 = strTxt[j].Substring(0, strTxt[j].Length - 4);
                                Match5 = strTxt[j].Substring(0, strTxt[j].Length - 5);

                                if ((Match4First.IndexOf(cookingimg) > -1 || Match5First.IndexOf(cookingimg) > -1) && temp == 0)
                                {
                                    temp = 1;
                                    for (int dataflow = 0; dataflow < strTxt.Count(); dataflow++)
                                    {
                                        listBox1.Items.Add(strTxt[dataflow].ToString());
                                    }
                                    tempCompare = cookingimg;
                                    pictureBox2.Image = Image.FromFile(@"D:\\料理圖\\" + FishInfoFile[s].ToString());
                                }

                                if ((Match4.IndexOf(cookingimg) > -1 || Match5.IndexOf(cookingimg) > -1) && temp != 0 && tempCompare != cookingimg)
                                {
                                    pictureBox3.Image = Image.FromFile(@"D:\\料理圖\\" + FishInfoFile[s].ToString());
                                }
                            }
                        }

                    }


                }
            }
        }
        public void CompareShow()//資料流比對
        {
            //pictureBox1.Image = imglist.Images[next];                  
            pictureBox1.Image = Image.FromFile(@"D:\\testresult\\" + file[next].ToString());
            FishInfoFile2 = FishInfo2.GetFiles("*.txt");//txt檔
            listBox1.Items.Clear();
            pictureBox2.Image = null;
            pictureBox3.Image = null;
           

            string pathimg = Path.GetFileNameWithoutExtension(file[next].ToString());
            for (int k = 0; k < FishInfoFile2.Count(); k++)
            {

                string pathtxt = Path.GetFileNameWithoutExtension(FishInfoFile2[k].ToString());
                if (pathimg == pathtxt)
                {
                    int temp = 0;
                    string tempCompare = "";
                    string str;
                    string Match4 = "";
                    string Match5 = "";
                    string Match4First = "";
                    string Match5First = "";
                    char[] strspecialtext = { '(', ')', '\t', '\r', '\n' };
                    string path = @"D:\\testtxt\\" + FishInfoFile2[next].ToString();
                    StreamReader strTXT = new StreamReader(path);

                    while ((str = strTXT.ReadToEnd()) != null && str != "")
                    {
                        str = str.Replace(" ", "");
                        strTxt = str.Split(strspecialtext).ToList();
                    }
                    Match4First = strTxt[0].Substring(0, strTxt[0].Length - 4);
                    Match5First = strTxt[0].Substring(0, strTxt[0].Length - 5);

                    for (int j = 0; j < strTxt.Count(); j++)
                    {
                        for (int s = 0; s < FishInfoFile.Count(); s++)
                        {
                            string cookingimg = Path.GetFileNameWithoutExtension(FishInfoFile[s].ToString());

                            if (strTxt[j] != "")
                            {
                                Match4 = strTxt[j].Substring(0, strTxt[j].Length - 4);
                                Match5 = strTxt[j].Substring(0, strTxt[j].Length - 5);

                                if ((Match4First.IndexOf(cookingimg) > -1 || Match5First.IndexOf(cookingimg) > -1) && temp == 0)
                                {
                                    temp = 1;
                                    for (int dataflow = 0; dataflow < strTxt.Count(); dataflow++)
                                    {
                                        listBox1.Items.Add(strTxt[dataflow].ToString());
                                    }
                                    tempCompare = cookingimg;
                                    pictureBox2.Image = Image.FromFile(@"D:\\料理圖\\" + FishInfoFile[s].ToString());
                                }

                                if ((Match4.IndexOf(cookingimg) > -1 || Match5.IndexOf(cookingimg) > -1) && temp != 0 && tempCompare != cookingimg)
                                {
                                    pictureBox3.Image = Image.FromFile(@"D:\\料理圖\\" + FishInfoFile[s].ToString());
                                }
                            }
                        }

                    }


                }
            }
        }
        public void Next()//下一張
        {          
            next++;
            if (next >= imglist.Images.Count)
                next = 0;
            CompareShow();
                                         
        }
        public void Back()//上一張
        {           
            next--;
            if (next == 0)
                next = 0;
            else if (next < 0)
                next = imglist.Images.Count - 1;

            CompareShow();                      
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CallCMD();//cmd指令函式     
            button1.Enabled = false;
        }
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) 
        { 
            return true; 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //string Web = "https://food.ltn.com.tw/article/1521/2";//魚類網站
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;//憑證，避免被ssl擋掉
            //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            //WebRequest request = WebRequest.Create(Web);
            //WebResponse response = request.GetResponse();
            //StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
            //string Result = reader.ReadToEnd();
            //reader.Close();
            //response.Close();
            //int first = Result.LastIndexOf("<h5>" + Global.Fish_name + "</h5>");
            //Result = Result.Substring(first + 4, 2);
            //MessageBox.Show(Result);
        }
       
      

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click_1(object sender, EventArgs e)
        {  
            Back();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Next();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string Local = "";
            next = 0;
            button1.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            bChangeState = false;
            imglist.Images.Clear();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Local = dialog.FileName;
                StreamReader ImgPath = new StreamReader(Local, Encoding.Default);
                string Imgname = ImgPath.ReadLine();//抓出txt檔案內容
                listBox1.Items.Add(Imgname);
                pictureBox1.Image = Image.FromFile(Local);
                MessageBox.Show(Local);

            }
            else
            {
                MessageBox.Show("找不到魚類照片");
            }

            DirectoryInfo dre = new DirectoryInfo(@"D:\\原圖\\");
            string localfilename = Path.GetFileName(Local);
            file = dre.GetFiles(localfilename);
            imglist.ColorDepth = ColorDepth.Depth32Bit;//設定像素
            imglist.ImageSize = new Size(256, 256);//設定圖片大小

            for (int i = 0; i < file.Count(); i++) //將testresult的所有圖檔加入imagelist裡 
            {
                progressBar1.Maximum = file.Count() - 1;
                progressBar1.Value = i;
                imglist.Images.Add(Image.FromFile(@"D:\\原圖\\" + file[i]));
            }
            MessageBox.Show("單張圖檔已載入完成，請按執行按鈕繼續!");
            //progressBar1.Value = 0;

                  
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            bChangeState = true;
            next = 0;
            imglist.Images.Clear();
            file = null;
            DirectoryInfo dre = new DirectoryInfo(@"D:\\testresult\\");
            string AllJPG = Path.GetFileName("*.jpg");
            file = dre.GetFiles(AllJPG);
            imglist.ColorDepth = ColorDepth.Depth32Bit;//設定像素
            imglist.ImageSize = new Size(256, 256);//設定圖片大小

            for (int i = 0; i < file.Count(); i++) //將testresult的所有圖檔加入imagelist裡 
            {
                progressBar1.Maximum = file.Count() - 1;
                progressBar1.Value = i;
                imglist.Images.Add(Image.FromFile(@"D:\\testresult\\" + file[i]));
            }
            MessageBox.Show("所有圖檔已載入完成，請按執行按鈕繼續!");
            
        }
    }
    public class Global //宣告全域變數類別
    {
        private static string Fn = "";
        private static int DelayTime = 0;
        public static string Fish
        {
            get
            {
                return Fn;
            }
            set 
            {
                Fn = value;
            }
        }
        public static int Delay
        {
            get
            {
                return DelayTime;
            }
            set
            {
                DelayTime = value;
            }
        }
  
       
        public static string Fish_name; //靜態變數回傳魚名
        public static int Delaytime; //靜態變數回傳Delay時間

    }
    
    
}
