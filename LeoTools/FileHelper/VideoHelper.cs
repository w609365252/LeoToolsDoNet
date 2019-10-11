using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace LeoTools.FileHelper
{
    /// <summary>
    /// 视频文件处理辅助类
    /// </summary>
    public static class VideoHelper
    {
        /// <summary>
        /// 剪切视频 
        /// </summary>
        /// <param name="exeDir">视频exe所在的文件夹</param>
        /// <param name="filePath">视频文件完整路径</param>
        /// <param name="savePath">视频保存完整路径\\save.mp4</param>
        ///// <param name="sTime">开始时间</param>
        ///// <param name="eTime">持续时间</param>
        /// <param name="videoLength">截取视频长度</param>
        public static void Cut(string exeDir , string filePath , string savePath,string length) {
            try
            {
                int getLength = 0;
                if (Int32.TryParse(length, out getLength))
                {
                    if (getLength <= 0)
                        getLength = 4;
                }
                else {
                    getLength = 4;
                }
                //如果已存在 则跳出
                if (File.Exists(savePath)) { return; }
                var VideoInfo = GetVideoDuration(exeDir, filePath); //视频时长01:00:25  取中间值
                var VideoDate = DateTime.Parse(VideoInfo.VideoLength);
                var seconds = VideoDate.Hour * 3600 + VideoDate.Minute * 60 + VideoDate.Second; //转秒数
                var beginStartSecond = seconds / 2 - getLength / 2;  //取开始时间秒数

                TimeSpan ts = new TimeSpan(0, 0, beginStartSecond);  //exe 执行误差
                TimeSpan ts2 = new TimeSpan(0, 0, getLength); //持续时间

                var beginDate = ts.Hours.ToString() + ":" + ts.Minutes.ToString() + ":" + (ts.Seconds).ToString();
                var saveDate = ts2.Hours.ToString() + ":" + ts2.Minutes.ToString() + ":" + ts2.Seconds.ToString();
                //旧版 速度很快 但是视频时长比较短时不精确 
                //var cmdStr = " -ss " + DateTime.Parse(beginDate).ToString("HH:mm:ss") + " -t " + DateTime.Parse(saveDate).ToString("HH:mm:ss") + " -accurate_seek -i " + filePath + "  -codec copy -avoid_negative_ts 1 -y " + savePath;   

                //旧版 速度很快 但是视频时长比较短时不精确
                var cmdStr = " -ss " + DateTime.Parse(beginDate).ToString("HH:mm:ss") + " -t " + DateTime.Parse(saveDate).ToString("HH:mm:ss") + " -accurate_seek -i " + filePath + " -strict -2  -qscale 0 -intra -y " + savePath; 
                ProcessExe(exeDir, cmdStr);
        
                var SaveVideoInfo = GetVideoDuration(exeDir, savePath);
                var SaveVideoDate = DateTime.Parse(SaveVideoInfo.VideoLength);
                var SaveSeconds = SaveVideoDate.Second; //转秒数
                if (SaveSeconds > 5) {
                    //第二次剪切时  新建一个文件 防止覆盖时出错
                    var NewFilePath = "";
                    var lastI = savePath.LastIndexOf('.');
                    NewFilePath = savePath.Substring(0, lastI);
                    NewFilePath += "_2.mp4";

                    VideoHelper.ProcessExe(AppDomain.CurrentDomain.BaseDirectory, " -ss 00:00:00 -t 00:00:05 -accurate_seek -i "+savePath+ " -vcodec copy -acodec copy -avoid_negative_ts 1  "+ NewFilePath);
                    File.Delete(savePath);
                }
                // p.StartInfo.Arguments = "-i "+ filePath + " -s 1280x720 " + savePath;  //  视频转分辨率 20M 2分30秒
                //DateTime startTime = DateTime.Parse(sTime);
                //DateTime saveTime = DateTime.Parse(eTime);
                //p.StartInfo.Arguments = "-ss " + startTime.ToString("HH:mm:ss") + " -t " + saveTime.ToString("HH:mm:ss") + " -i " + filePath + " -vcodec copy " + savePath;    //执行参数              
            }
            catch(Exception e){
                var a = e.Message;
            }
        }

        /// <summary>
        /// 修改分辨率
        /// </summary>
        public static void ChangePx(string exeDir, string filePath, string savePath, int width, int height) {
          

            ProcessExe(exeDir, " -i " + filePath + "  -c:v libx264 -s " + width + "x" + height + " " + savePath); // 视频转分辨率 20M 1分40秒
        }

        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 运行ffmpeg.exe文件
        /// </summary>
        /// <param name="exeDir">运行ffmpeg所在文件夹</param>
        /// <param name="command">命令字符串</param>
        public static void ProcessExe(string exeDir , string command) {
            String result;  // temp variable holding a string representation of our video's duration  
            StreamReader errorreader;  // StringWriter to hold output from ffmpeg  
            try
            {

                // -i E:\test\1.mp4  -c:v libx264 -s 1280x720 E:\test\2.mp4
                Process p = new Process();
                p.StartInfo.FileName = exeDir + "ffmpeg.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.Arguments = command;
                p.StartInfo.UseShellExecute = false;  ////不使用系统外壳程序启动进程
                p.StartInfo.CreateNoWindow = true;  //不显示dos程序窗口
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中
                p.ErrorDataReceived += P_ErrorDataReceived;
                p.OutputDataReceived += P_OutputDataReceived;
                p.StartInfo.UseShellExecute = false;
                p.Start();
                errorreader = p.StandardError;
                result = errorreader.ReadToEnd();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
               // p.BeginErrorReadLine();//开始异步读取
                p.WaitForExit();//阻塞等待进程结束
                p.Close();//关闭进程
                p.Dispose();//释放资源
            }
            catch (Exception e) {

            }
        }

        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="exeDir">视频exe所在的文件夹</param>
        /// <param name="sourceFile">视频文件完整路径</param>
        /// <returns></returns>
        public static VideoInfo GetVideoDuration(string exeDir, string sourceFile)
        {
            using (System.Diagnostics.Process ffmpeg = new System.Diagnostics.Process())
            {
                String result;  // temp variable holding a string representation of our video's duration  
                StreamReader errorreader;  // StringWriter to hold output from ffmpeg  
                ffmpeg.StartInfo.UseShellExecute = false;
                ffmpeg.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                ffmpeg.StartInfo.RedirectStandardError = true;
                ffmpeg.StartInfo.CreateNoWindow = true;
                ffmpeg.StartInfo.FileName = exeDir + "ffmpeg.exe";
                ffmpeg.StartInfo.Arguments = " -i " + sourceFile;
                ffmpeg.Start();
                errorreader = ffmpeg.StandardError;
                ffmpeg.WaitForExit();
                result = errorreader.ReadToEnd();
               
                var rResolution = new Regex(@"[0-9]{3,4}x[0-9]{3,4}");
                var v = new VideoInfo() {
                    Msg = result,
                    VideoLength = result.Substring(result.IndexOf("Duration: ") + ("Duration: ").Length, ("00:00:00").Length), //视频时长
                    Resolution = rResolution.Match(result).Value //视频分辨率
                };
                return v;
            }
        }

        public class VideoInfo
        {
            /// <summary>
            /// 视频时长
            /// </summary>
            public string VideoLength { get; set; }

            /// <summary>
            /// 分辨率
            /// </summary>
            public string Resolution { get; set; }


            public string Msg { get; set; }
        }
    }
}
