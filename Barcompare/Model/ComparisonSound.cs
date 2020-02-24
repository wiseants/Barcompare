using HVision.Common.Tool;

namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 비교 결과 사운드 구조체.
    /// </summary>
    public struct ComparisonSound
    {
        #region Fields

        public static readonly ComparisonSound Empty = new ComparisonSound();

        private static readonly string WAV_FILE_FOLDER = @"\Asset\Sound\";

        #endregion

        #region Constructors

        /// <summary>
        /// Windows User Account Control 시스템 사운드.
        /// C:\Windows\Media\Windows User Account Control.wav
        /// </summary>
        public static ComparisonSound UserAccount
        {
            get
            {
                return new ComparisonSound(FolderTool.WorkingFolderFullPath + WAV_FILE_FOLDER + @"Beep_07.mp3", 8);
            }
        }

        /// <summary>
        /// Radio Interruption Sound
        /// http://soundbible.com/1063-Radio-Interruption.html
        /// </summary>
        public static ComparisonSound RadioInterruption
        {
            get
            {
                return new ComparisonSound(FolderTool.WorkingFolderFullPath + WAV_FILE_FOLDER + @"Radio Interruption.mp3", 8);
            }
        }

        #endregion

        #region Private constructors

        private ComparisonSound(string fileName, int volume, int durationTime = 0)
        {
            FileName = fileName;
            Volume = volume;
            DurationTime = durationTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 사운드 파일 이름.
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        /// <summary>
        /// 사운드 볼륨.
        /// </summary>
        public int Volume
        {
            get;
            set;
        }

        /// <summary>
        /// 사운드 지속 시간.
        /// </summary>
        public int DurationTime
        {
            get;
            private set;
        }

        #endregion
    }
}
