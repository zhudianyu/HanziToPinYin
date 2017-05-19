﻿//*************************************************************************
//	创建日期:	2017/1/20 星期五 11:40:20
//	文件名称:	RenameImage
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	把文件夹下的文件的中文名字转换成拼音
//*************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Table;
namespace HanZiToPinYin
{
    class Program
    {
        static string magicString = "_程序用";
        static void CopyFiles(string oldDir, string pinyinDir, string filter)
        {
            string[] oldFileList = Directory.GetFiles(oldDir, filter, SearchOption.TopDirectoryOnly);
            Dictionary<string, string> nameDic = new Dictionary<string, string>();
            foreach (var oldFile in oldFileList)
            {
                string temp = oldFile;
                string strPath, strFileName, strFileNameNoExt, strExt;
                StringUtility.ParseFileName(ref temp, out strPath, out strFileName, out strFileNameNoExt, out strExt);
                if (!Hz2Py.IsContainHz(strFileName))
                {
                    continue;
                }
                string pinyinFileName = SearchHelper.GetAllCharByChineseFont(strFileName);

                if (nameDic.ContainsKey(pinyinFileName))
                {
                    Console.WriteLine("已经相同的拼音名字: " + pinyinFileName + " 文件是: " + nameDic[pinyinFileName] + " 重名文件是:" + oldFile);
                    break;
                }
                else
                {
                    nameDic.Add(pinyinFileName, oldFile);
                }
                string newFilePath = Path.Combine(pinyinDir, pinyinFileName.ToLower());
                try
                {
                    File.Copy(oldFile, newFilePath, true);
                    File.Delete(oldFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" file copy error " + ex.ToString());
                }
            }
        }
        static void RenameFile(string inDir, string filter = "*.png")
        {
            string[] dirArray = Directory.GetDirectories(inDir, "*.*", SearchOption.AllDirectories);

            foreach (var dir in dirArray)
            {

                Console.WriteLine(dir);

                CopyFiles(dir, dir, filter);

            }
            if(dirArray.Length == 0)
            {
                CopyFiles(inDir, inDir, filter);
            }
        }
        static public void CopyFileCallBack(string fileName)
        {
            Console.WriteLine(fileName);
        }
        static string PngExeclName = "PngName.xlsx";
        static void WriteToExcel(List<string> fileList)
        {
            List<string> newList = new List<string>();
            for (int i = 0; i < fileList.Count; i++)
            {
                int col = 2 + i;
                string fullName = fileList[i];
                if (!fullName.EndsWith(".png"))
                {
                    continue;
                }
                newList.Add(fullName);
            }
            FileInfo newFile = new FileInfo(PngExeclName);
            if (newFile.Exists)
            {
               
                 using (ExcelPackage package = new ExcelPackage(newFile))
                 {
                     ExcelWorksheet worksheet = package.Workbook.Worksheets["PngName"];
                     for (int i = 0; i < newList.Count; i++)
                     {
                         int col = 2 + i;
                         string fullName = newList[i];
                         string newName = worksheet.Cells[col, 2].GetValue<string>();
                         newName += ".png";
                         fullName = fullName.Replace("\\", "/");
                         int len = fullName.LastIndexOf('/');
                         string prename = fullName.Substring(0, len + 1);
                         newName = prename + newName;
                         if(!File.Exists(newName))
                         {
                             File.Copy(fullName, newName);
                             File.Delete(fullName);
                         }
                       
                     }
                     
                 }
            }
            else
            {
      
                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("PngName");
                    //Add the headers
                    worksheet.Cells[1, 1].Value = "原始名字";
                    worksheet.Cells[1, 2].Value = "新名字";
                    worksheet.Cells[1, 3].Value = "路径";
              
                    for (int i = 0; i < newList.Count; i++)
                    {
                        int col = 2 + i;
                        string fullName = newList[i];
                        if(!fullName.EndsWith(".png"))
                        {
                            continue;
                        }
                       fullName = fullName.Replace("\\", "/");
                        int len = fullName.LastIndexOf('/');
                        string pngName = fullName.Substring(len+1);
                        worksheet.Cells[col, 1].Value = pngName;
                        worksheet.Cells[col, 3].Value = fullName;
                    }
                    package.Save();
                    //xieru
                }
            }
           
        }
        static void Main(string[] args)
        {
            Console.WriteLine("=============修改文件夹下中文命名的文件为拼音命名==================");
            Console.WriteLine("=============Author:Dianyu.Zhu      QQ:1462415060==================");
            Console.WriteLine(@"=============参数一：文件名          例如：C:\  ===================");
            Console.WriteLine(@"=============参数二：过滤文件后缀名  例如：*.png|*.txt=============");
            Console.WriteLine("============================================================");
          //  Console.WriteLine("==========inDir is " + args[0]);
           // Console.WriteLine("==========filter is " + args[1]);
            string inDir = "";
            string filter = "*.png";
            if (args.Length == 0)
            {
                inDir = Directory.GetCurrentDirectory();
               // inDir = @"F:\华夏第一批音效";
            }
            else
            {
                if (args.Length > 0)
                {
                    inDir = args[0];
                }
                if (args.Length > 1)
                {
                    filter = args[1];
                }
            }
            char[] cArray = Path.GetInvalidPathChars();
            foreach (var c in cArray)
            {
                if (inDir.Contains(c))
                {
                    inDir = inDir.Replace(c.ToString(), "");
                    Console.WriteLine("invalid char is " + c.ToString());
                }
            }
            string leftPath;
            string prePath = StringUtility.GetPrePathByChar('/', inDir, out leftPath);
           // Console.WriteLine("==========inDir is " + inDir);
            leftPath = leftPath + magicString;
            //Console.WriteLine("==========leftPath is " + leftPath);
            //Console.WriteLine("==========new path is " + prePath);

       
            //Console.WriteLine("==========leftPath is " + leftPath);
            //Console.ReadKey();
            try
            {
                string newPath = Path.Combine(prePath, leftPath);
                if (Directory.Exists(newPath))
                {
                    Directory.Delete(newPath, true);
                }
                Directory.CreateDirectory(newPath);
                List<string> fileList = new List<string>();
                StringUtility.CopyDirectory(inDir, newPath, ref fileList, null, CopyFileCallBack);
                Console.WriteLine("============" + "当前路径：" + inDir + " 重命名文件格式：" + filter + "============");
                WriteToExcel(fileList);
               // RenameFile(newPath, filter);
                Console.WriteLine("======操作完毕,点击任意键继续========");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }



        }
    }


}
