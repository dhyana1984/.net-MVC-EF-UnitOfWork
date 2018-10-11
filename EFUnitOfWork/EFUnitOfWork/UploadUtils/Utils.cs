using EF.Core.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EFUnitOfWork.UploadUtils
{
    public class Utils
    {
        private const string PARTTOKEN = ".part_";
        public Utils()
        {
            FileParts = new List<string>();
        }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 临时文件夹
        /// </summary>
        public List<string> FileParts { get; set; }

        public bool MergeFile(string fileName,out bool result,out string storeFileName)
        {
            result = false;
            storeFileName = string.Empty;

            //根据约定匹配模式来获取文件
            var fileNamePartToken = fileName.IndexOf(PARTTOKEN);
            var baseFileName = fileName.Substring(0, fileNamePartToken);
            var trailingTokens = fileName.Substring(fileNamePartToken + PARTTOKEN.Length);

            var fileIndex = 0;
            var fileCount = 0;
            int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out fileCount);

            //获取文件夹中所有匹配模式文件
            var searchPattern = Path.GetFileName(baseFileName) + PARTTOKEN + "*";
            var filesList = FileHelper.GetFiles(Path.GetDirectoryName(fileName), searchPattern);

            //文件未进行安全校验
            if(filesList.Count()==fileCount)
            {
                var extentionName = FileHelper.GetExtensionName(baseFileName);
                storeFileName = FileHelper.GetFileNameWithoutExtension(baseFileName) + extentionName;

                //使用单例模式确保
                if(!MergeFile)

            }
        }
    }
}