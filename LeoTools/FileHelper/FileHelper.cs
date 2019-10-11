using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using NPOI.SS.UserModel;
namespace LeoTools.FileHelper
{
    /// <summary>
    /// 写入文本 下载/删除文件 导入导出
    /// </summary>
    /// <author> Write by 云创天成 QQ:2943075966 </author>
    public class FileHelper
    {
        /// <summary>
        /// 写入到txt
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="content"></param>
        public static void WriteInTxt(string savePath, string content)
        {
            string tempPath = System.IO.Path.GetDirectoryName(savePath);
            System.IO.Directory.CreateDirectory(tempPath);  //创建临时文件目录
            if (!System.IO.File.Exists(savePath))
            {
                FileStream fs1 = new FileStream(savePath, FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(content);//开始写入值
                sw.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                sr.WriteLine(content);//开始写入值
                sr.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 读取txt文本
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FileToString(string filePath)
        {
            string strData = "";
            try
            {
                string line;
                // 创建一个 StreamReader 的实例来读取文件 ,using 语句也能关闭 StreamReader
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath))
                {
                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        strData += line +"\r\n";
                    }
                }
            }
            catch (Exception e)
            {
                // 向用户显示出错消息
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return strData;
        }


        /// <summary>
        /// 递归删除文件夹下所有文件
        /// </summary>
        /// <param name="file"></param>
        public static void DeleteFile(string dirPath)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dirPath);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
                //去除文件的只读属性
                System.IO.File.SetAttributes(dirPath, System.IO.FileAttributes.Normal);
                //判断文件夹是否还存在
                if (Directory.Exists(dirPath))
                {
                    foreach (string f in Directory.GetFileSystemEntries(dirPath))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹 
                            DeleteFile(f);
                        }
                    }
                    //删除空文件夹 
                    Directory.Delete(dirPath);
                }
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Http下载文件
        /// </summary>
        /// <param name="url">下载文件路径</param>
        /// <param name="savePath">保存路径</param>
        /// <returns></returns>
        public static bool HttpDownloadFile(string url, string savePath)
        {
            string tempPath = System.IO.Path.GetDirectoryName(savePath);
            System.IO.Directory.CreateDirectory(tempPath);  //创建临时文件目录
            string tempFile = tempPath + @"\" + System.IO.Path.GetFileName(savePath); //临时文件
            if (System.IO.File.Exists(tempFile))
            {
                //存在则跳出
                return true;
                //System.IO.File.Delete(tempFile);    
            }
            try
            {
                FileStream fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    //stream.Write(bArr, 0, size);
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                //stream.Close();
                fs.Close();
                responseStream.Close();
                System.IO.File.Move(tempFile, savePath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="savepath">导出文件完整路径（包括文件名）</param>
        /// <param name="dt">数据源</param>
        /// <param name="widths">列宽集合</param>
        public static void ExportExcel(string savePath,DataTable dt,string title, List<int> widths=null) {

            IWorkbook book = null;
            book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            ISheet sheet = book.CreateSheet("数据清单");

            var headerIndex = 0;
            if (title.Length > 0) { 
                //第1行添加标题
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Rows[0].ItemArray.Length-1));
                headerIndex = 1;
            }
            //设置行高   Height的单位是1/20个点。例：设置高度为30个点                   
            IRow row = sheet.CreateRow(0);
            row.Height = 30 * 20;


            ICell titleCell = row.CreateCell(0);
            titleCell.SetCellValue(title);
            titleCell.CellStyle.Alignment = HorizontalAlignment.Left;
            titleCell.CellStyle.VerticalAlignment = VerticalAlignment.Center;
            IFont titleFont = book.CreateFont();
            titleFont.FontHeightInPoints = 11;
            titleCell.CellStyle.SetFont(titleFont);

            if (widths != null) { 
            //设置列宽 
                for (int i = 0; i < widths.Count; i++)
                {
                    sheet.SetColumnWidth(i, widths[i] * 256); //列宽单位为 1/256个字符 
                }
            }

            int index = 0;
            // 添加表头  
            row = sheet.CreateRow(headerIndex);
            foreach (DataColumn item in dt.Columns)
            {
                ICell cell = row.CreateCell(index);
                cell.SetCellType(CellType.String);
                cell.SetCellValue(item.Caption);
                index++;
            }
         
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                index = 0;
                row = sheet.CreateRow(i+headerIndex+1);    // 添加数据  从第3行开始
                foreach (DataColumn item in dt.Columns)
                {
                    ICell cell = row.CreateCell(index);                    
                    cell.SetCellType(CellType.String);
                    cell.SetCellValue(dt.Rows[i][item.ColumnName].ToString());
                    index++;
                }
            }

            // 写入 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            book = null;

            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="importPath">导入文件路径</param>
        /// <param name="headerIndex">表头所在行索引 （兼容第一行为大标题的情况）</param>
        public static DataSet ImportExcel(string importPath,int headerIndex = 0) {
            DataSet ds = new DataSet();
            DataTable dt = null;
            FileStream fs = new FileStream(importPath, FileMode.Open, FileAccess.Read);
            IWorkbook book;
            if (Path.GetExtension(importPath) == "xls")
                book  = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
            else
                book = new NPOI.XSSF.UserModel.XSSFWorkbook(fs);
            int sheetCount = book.NumberOfSheets;
            for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
            {
                NPOI.SS.UserModel.ISheet sheet = book.GetSheetAt(sheetIndex);
                if (sheet == null) continue;

                NPOI.SS.UserModel.IRow row = sheet.GetRow(headerIndex); //从第0行开始取 列头
                if (row == null) continue;

                int firstCellNum = row.FirstCellNum;
                int lastCellNum = row.LastCellNum;
                if (firstCellNum == lastCellNum) continue;

                dt = new DataTable(sheet.SheetName);
                for (int i = firstCellNum; i < lastCellNum; i++)
                {
                    dt.Columns.Add(row.GetCell(i).StringCellValue, typeof(string));
                }
                for (int i = headerIndex+1; i <= sheet.LastRowNum; i++) //从第1行开始取数据
                {
                    DataRow newRow = dt.Rows.Add();
                    for (int j = firstCellNum; j < lastCellNum; j++)
                    {
                        var cell = sheet.GetRow(i).GetCell(j);
                        cell.SetCellType(CellType.String);
                        newRow[j] = sheet.GetRow(i).GetCell(j).StringCellValue;
                    }
                }
                ds.Tables.Add(dt);
            }
            return ds;
         }


        /// <summary>
        /// 流转文件 (用于本地存储等地方)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        public void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }
    }
}
