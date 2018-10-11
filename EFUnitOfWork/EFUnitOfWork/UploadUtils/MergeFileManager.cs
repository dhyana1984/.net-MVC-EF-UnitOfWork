using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFUnitOfWork.UploadUtils
{
    public class MergeFileSingleton
    {
        private static object _lock = new object();
        private static volatile MergeFileSingleton instance;
        private List<string> MergeFileList;

        private MergeFileSingleton()
        {
            MergeFileList = new List<string>();
        }

        public static MergeFileSingleton Instance
        {
            get
            {
                if(instance!=null)
                {
                    return instance;
                }
                lock(_lock)
                {
                    if(instance==null)
                    {
                        instance = new MergeFileSingleton();
                    }
                }

                return instance;
            }
        }

        public void AddFile(string BaseFileName)
        {
            MergeFileList.Add(BaseFileName);
        }

        public bool RemoveFile(string BaseFileName)
        {
            return MergeFileList.Remove(BaseFileName);
        }

        public bool InUse(string BaseFileName)
        {
            return MergeFileList.Contains(BaseFileName);
        }

    }
}