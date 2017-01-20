
//*************************************************************************
//	创建日期:	2017/1/20 星期五 13:40:36
//	文件名称:	StringUtility
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanZiToPinYin
{
    class StringUtility
    {

        /**
      @brief 分割字符串
      @param str 源字串
      @param strKey 分割字符
      @param lst 返回列表
      */
        static public void SplitString(ref string str, string strKey, out List<string> lst)
        {
            lst = new List<string>();
            string[] sl = str.Split(strKey.ToCharArray());
            for (int i = 0; i < sl.Length; ++i)
            {
                lst.Add(sl[i]);
            }
        }

        /// <summary>
        /// 从带有 ../../../的路径字符串 获取提取不带../../的路径串
        /// </summary>
        /// <param name="strPath"></param>
        /// <param name="strOut"></param>
        static public void GetAbsolutePath(ref string strPath, out string strOut)
        {
            string strSrcPath = strPath;
            strSrcPath = strSrcPath.Replace("\\", "/");
            int pos = strSrcPath.IndexOf("/..");
            string strPrefix = "";
            string strSuffix = "";
            while (pos != -1)
            {
                strPrefix = strSrcPath.Substring(0, pos);
                strSuffix = strSrcPath.Substring(pos, strSrcPath.Length - pos);
                int lastpos = strPrefix.LastIndexOf("/");
                if (lastpos <= 0)
                {
                    break;
                }

                //string strPrefixNew = strPrefix.Substring(0, lastpos);
                strPrefix = strPrefix.Substring(0, lastpos);
                strSuffix = strSuffix.Substring(3, strSuffix.Length - 3);
                //strPrefix = strPrefixNew;
                strSrcPath = strPrefix + strSuffix;
                pos = strSrcPath.IndexOf("/..");
            }

            strOut = strSrcPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strPathFileName">文件路径</param>
        /// <param name="strPath">路径名</param>
        /// <param name="strFileName">文件名</param>
        /// <param name="strFileNameNoExt">文件名不带扩展名</param>
        /// <param name="strExt">扩展名</param>
        static public void ParseFileName(ref string strPathFileName, out string strPath, out string strFileName, out string strFileNameNoExt, out string strExt)
        {
            strFileNameNoExt = "";
            strFileName = "";
            strPath = "";
            strExt = "";

            string strTemp = strPathFileName.Replace("\\", "/");
            int pos = strTemp.LastIndexOf(".");
            if (pos > 0)
            {
                strExt = strTemp.Substring(pos + 1, strTemp.Length - pos - 1);
                strTemp = strTemp.Substring(0, pos);

                pos = strTemp.LastIndexOf("/");
                if (pos >= 0)
                {
                    strPath = strTemp.Substring(0, pos);
                    strFileNameNoExt = strTemp.Substring(pos + 1);
                }
                else
                {
                    strFileNameNoExt = strTemp;
                }

                strFileName = strFileNameNoExt + "." + strExt;
            }
            else if (pos == 0) // 这种情况，还要处理下 相对路径的写法 .sss ./slf/sd ./
            {
                if (strTemp.Length > pos + 1)
                {
                    if (strTemp[pos + 1] == '/')
                    {
                        strPath = strPathFileName;
                    }
                    else
                    {
                        strExt = strTemp.Substring(pos + 1, strTemp.Length - pos - 1);
                    }
                }
            }
            else if (pos == -1)
            {
                strPath = strPathFileName;
            }
        }
        /// <summary>
        /// 获取某个符号前的路径
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetPrePathByChar(char c, string oldPath,out string leftPath)
        {
            string strTemp = oldPath.Replace("\\", "/");
            int len = strTemp.LastIndexOf(c);
            if(len < 0)
            {
                len = 0;
            }
            leftPath = strTemp.Substring(len+1);
            return strTemp.Substring(0, len);
        }
      

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static bool[] ToBoolList(string strOperation)
        {
            if (string.IsNullOrEmpty(strOperation))
            {
                return null;
            }

            string[] strItem = strOperation.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            if (strItem.Length <= 0)
            {
                return null;
            }

            bool[] opList = new bool[strItem.Length];
            for (int i = 0; i < strItem.Length; ++i)
            {
                if (strItem[i] == "true")
                {
                    opList[i] = true;
                }
                else
                {
                    opList[i] = false;
                }
            }

            return opList;
        }

        public static float[] ToFloatList(string strOperation)
        {
            if (string.IsNullOrEmpty(strOperation))
            {
                return null;
            }

            string[] strItem = strOperation.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            if (strItem.Length <= 0)
            {
                return null;
            }

            float[] opList = new float[strItem.Length];
            for (int i = 0; i < strItem.Length; ++i)
            {
                opList[i] = float.Parse(strItem[i]);
            }

            return opList;
        }

        public static int[] ToIntList(string strOperation)
        {
            if (string.IsNullOrEmpty(strOperation))
            {
                return null;
            }

            string[] strItem = strOperation.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
            if (strItem.Length <= 0)
            {
                return null;
            }

            int[] opList = new int[strItem.Length];
            for (int i = 0; i < strItem.Length; ++i)
            {
                opList[i] = int.Parse(strItem[i]);
            }

            return opList;
        }
        //-------------------------------------------------------------------------------------------------------
        // tostring
        // ToString 只支持基础类型 数值类型(byte,short,int,float,double)和bool类型 其它类型不保证有效
        public static string ToString<T>(T[] operation)
        {
            StringBuilder strResult = new StringBuilder();
            if (operation == null)
            {
                return strResult.ToString();
            }

            for (int i = 0; i < operation.Length; ++i)
            {
                strResult.Append(operation[i].ToString());
                if (i != operation.Length - 1)
                {
                    strResult.Append(",");
                }
            }

            return strResult.ToString();
        }
        public delegate void CopyFileCallback(string strFileName);
        /**
       @brief 递归创建目录
       @param 
       */
        static public void CreateDir(string strDir)
        {
            string strTemp = strDir.Replace("\\", "/");
            if (!Directory.Exists(strTemp))
            {
                int pos = strDir.LastIndexOf('/');
                if (pos == -1)
                {
                    Directory.CreateDirectory(strTemp);
                }
                else
                {
                    CreateDir(strTemp.Substring(0, pos));
                    Directory.CreateDirectory(strTemp);
                }
            }
        }
        // 拷贝目录下所有文件和文件夹
        static public void CopyDirectory(string strSrcDir, string strDestDir, ref List<string> lstFile, List<string> lstFilter = null, CopyFileCallback callback = null)
        {
            strSrcDir.Replace("\\", "/");
            strDestDir.Replace("\\", "/");
            DirectoryInfo src = new DirectoryInfo(strSrcDir);
            if (!src.Exists)
            {
                return;
            }
            DirectoryInfo dest = new DirectoryInfo(strDestDir);
            if (!dest.Exists)
            {
                CreateDir(strDestDir);
            }

            if (strSrcDir.LastIndexOf('/') != strSrcDir.Length - 1)
            {
                strSrcDir += "/";
            }

            DirectoryInfo dir = src as DirectoryInfo;
            //不是目录 
            if (dir == null) return;
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                {
                    string strDestFileName = strDestDir + "/";
                    strDestFileName += file.Name;

                    string strExt = Path.GetExtension(strDestFileName);
                    if (lstFilter != null && lstFilter.Contains(strExt))
                    {
                        continue;
                    }
                    if(strDestFileName.Length > 248)
                    {
                        Console.WriteLine("============="+strDestFileName);
                        break;
                    }
                    file.CopyTo(strDestFileName, true);
                    if (lstFile != null)
                    {
                        lstFile.Add(file.FullName);
                    }

                    if (callback != null)
                    {
                        callback(file.FullName);
                    }
                }
                else
                {
                    string strFullDirName = files[i].FullName;
                    strFullDirName = strFullDirName.Replace("\\", "/");
                    DirectoryInfo srcDir = new DirectoryInfo(strSrcDir);
                    strSrcDir = srcDir.FullName.Replace("\\", "/");
                    string strDir = strFullDirName.Replace(strSrcDir, "");
                    string strSubDir = strDestDir + "/";
                    strSubDir += strDir;

                    CopyDirectory(strFullDirName, strSubDir, ref lstFile, lstFilter, callback);
                }
            }
        }
    }
}