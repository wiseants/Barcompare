using System.IO;
using System.Reflection;

namespace HVision.Common.Tool
{
    /// <summary>
    /// 폴더와 관련된 내용을 가져오는 도구.
    /// </summary>
    public class FolderTool
    {
        #region Fields

        /// <summary>
        /// 현재 작업 폴더의 전체경로를 가져온다.
        /// </summary>
        public static string WorkingFolderFullPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        #endregion
    }
}
