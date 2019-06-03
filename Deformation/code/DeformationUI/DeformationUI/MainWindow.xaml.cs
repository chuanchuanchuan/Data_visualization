using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeformationUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Tuple<int, int>> originalLandmarks = new List<Tuple<int, int>>();
        List<Tuple<int, int>> referenceLandmarks = new List<Tuple<int, int>>();
        public MainWindow()
        {
            InitializeComponent();
            //OriginalImg.SourceUpdated += OriginalImg_SourceUpdated;
            //OriginalImg.Source = new BitmapImage(new Uri(@"F:\OneDrive\课件\数据可视化\Hw5\DeformationUI\DeformationUI\bin\Debug\baby.png"));
            //ReferenceImg.Source = new BitmapImage(new Uri(@"F:\OneDrive\课件\数据可视化\Hw5\DeformationUI\DeformationUI\bin\Debug\ape.png"));
            
            OriginalImg.Loaded += OriginalImg_Loaded;
            ReferenceImg.Loaded += ReferenceImg_Loaded;

            OriginalImgBtn.SizeChanged += OriginalImgBtn_SizeChanged;
            ReferenceImgBtn.SizeChanged += ReferenceImgBtn_SizeChanged;



        }


        private void ReferenceImgBtn_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //ReferenceImgBtn.Height = ReferenceImgBtn.ActualWidth / ReferenceImg.Source.Width * ReferenceImg.Source.Height;
            DrawLandmarks(referenceLandmarks, ReferenceImgCanvas, ReferenceImg);

        }

        private void OriginalImgBtn_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawLandmarks(originalLandmarks, OriginalImgCanvas, OriginalImg);
            //OriginalImgBtn.Height = OriginalImgBtn.ActualWidth / OriginalImg.Source.Width * OriginalImg.Source.Height;  
        }



        private void ReferenceImg_Loaded(object sender, RoutedEventArgs e)
        {
            /*ReferenceImgBtn.Width = 100;
            ReferenceImgBtn.Height = 100/ ReferenceImg.Source.Width* ReferenceImg.Source.Height;*/
        }

        private void OriginalImg_Loaded(object sender, RoutedEventArgs e)
        {
            /*OriginalImgBtn.Width = 100;
            OriginalImgBtn.Height = 100 / OriginalImg.Source.Width * OriginalImg.Source.Height;*/
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process p = new Process();
            string m_exename = "python.exe";
            string m_space = " ";
            string m_cmdLine;
            m_cmdLine = "\""+Directory.GetCurrentDirectory() + "\\deformation.py" + "\"" +
                " --landmarks_img "+ LandmarksToString(originalLandmarks) +
                " --landmarks_ref "+ LandmarksToString(referenceLandmarks) +
                " --input_img_dir "+ (OriginalImg.Source as BitmapImage).UriSource.LocalPath +
                " --input_ref_dir " + (ReferenceImg.Source as BitmapImage).UriSource.LocalPath;
            //p.StartInfo.WorkingDirectory =  ;   //可以手动设置启动程序时的当前路径，否则可能因为OpenFileDialog操作而改变
            p.StartInfo.FileName = m_exename;
            p.StartInfo.Arguments = m_cmdLine;
            p.StartInfo.UseShellExecute = false;    //必须为false才能重定向输出
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            //p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.Start();
            using (StreamReader reader = p.StandardOutput)
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            using (StreamReader reader = p.StandardError)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            SetImageSource(TargetImg, Directory.GetCurrentDirectory() + "\\target_img.png");

            


        }

        private void OriginalImgBtn_Click(object sender, RoutedEventArgs e)
        {
            Point clickPoint = Mouse.GetPosition(OriginalImg);
            if (VisualTreeHelper.HitTest(OriginalImg, clickPoint) != null)
            {
                var x = clickPoint.X / OriginalImg.ActualWidth * (OriginalImg.Source as BitmapSource).PixelWidth;
                var y = clickPoint.Y / OriginalImg.ActualHeight * (OriginalImg.Source as BitmapSource).PixelHeight;
                originalLandmarks.Add(new Tuple<int, int>(Convert.ToInt32(x), Convert.ToInt32(y)));
                Console.WriteLine(LandmarksToString(originalLandmarks));

                DrawLandmarks(originalLandmarks, OriginalImgCanvas, OriginalImg);
            }
            
        }

        private void ReferenceImgBtn_Click(object sender, RoutedEventArgs e)
        {
            Point clickPoint = Mouse.GetPosition(ReferenceImg);
            if (VisualTreeHelper.HitTest(ReferenceImg, clickPoint) != null)
            {
                var x = clickPoint.X / ReferenceImg.ActualWidth * (ReferenceImg.Source as BitmapSource).PixelWidth;
                var y = clickPoint.Y / ReferenceImg.ActualHeight * (ReferenceImg.Source as BitmapSource).PixelHeight;
                referenceLandmarks.Add(new Tuple<int, int>(Convert.ToInt32(x), Convert.ToInt32(y)));
                Console.WriteLine(LandmarksToString(referenceLandmarks));

                DrawLandmarks(referenceLandmarks, ReferenceImgCanvas, ReferenceImg);
            }
        }

        private static string LandmarksToString(List<Tuple<int, int>> landmarks)
        {
            string result = "[";
            foreach (var landmark in landmarks)
            {
                var landmarkString = String.Format("[{0},{1}],", landmark.Item2, landmark.Item1);
                result += landmarkString;
            }
            result += "]";
            return result;
        }

        private void UploadOriginalBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {

                OriginalImg.Source = new BitmapImage(new Uri(dialog.FileName));
                originalLandmarks.Clear();
            }
        }

        private void UploadReferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ReferenceImg.Source = new BitmapImage(new Uri(dialog.FileName));
                referenceLandmarks.Clear();
            }
        }

        private void SetImageSource(Image img, string fullPath)
        {
            MemoryStream ms = new MemoryStream();
            BitmapImage bi = new BitmapImage();

            byte[] arrbytFileContent = File.ReadAllBytes(fullPath);
            ms.Write(arrbytFileContent, 0, arrbytFileContent.Length);
            ms.Position = 0;
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            img.Source = bi;
        }

        private void DrawLandmarks(List<Tuple<int, int>> landmarks, Canvas canvas, Image img)
        {
            canvas.Children.Clear();
            foreach(var landmark in landmarks)
            {
                var x = landmark.Item1;
                var y = landmark.Item2;

                canvas.Height = img.ActualHeight;
                canvas.Width = img.ActualWidth;

                var actualY = y * img.ActualHeight / (img.Source as BitmapSource).PixelHeight ;
                var actualX = x * img.ActualWidth / (img.Source as BitmapSource).PixelWidth;

                Ellipse ellipse = new Ellipse { Width = 5, Height = 5 };
                ellipse.StrokeThickness = 2;
                ellipse.Stroke = Brushes.Red;
                Canvas.SetLeft(ellipse, actualX - (5 / 2));
                Canvas.SetTop(ellipse, actualY - (5 / 2));
                canvas.Children.Add(ellipse);
            }
            
        }

        private void UndoOriginalBtn_Click(object sender, RoutedEventArgs e)
        {
            var count = originalLandmarks.Count;
            if (count != 0)
            {
                originalLandmarks.RemoveAt(count - 1);
            }
            DrawLandmarks(originalLandmarks, OriginalImgCanvas, OriginalImg);
        }

        private void UndoReferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            var count = referenceLandmarks.Count;
            if (count != 0)
            {
                referenceLandmarks.RemoveAt(count - 1);
            }
            DrawLandmarks(referenceLandmarks, ReferenceImgCanvas, ReferenceImg);
        }
    }
}
