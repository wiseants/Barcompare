using System.Windows.Media;

namespace HVision.Barcompare.Model
{
    /// <summary>
    /// 바코드 비교 결과 구조체.
    /// </summary>
    public struct ComparisonResult
    {
        #region Fields

        public static readonly string WAV_FILE_FOLDER = @"Asset/Wav";

        #endregion

        #region Consturtors

        /// <summary>
        /// 미결과.
        /// </summary>
        public static ComparisonResult None
        {
            get
            {
                return new ComparisonResult(
                    "NONE", 
                    Brushes.Black, 
                    "입력중이거나 결과가 없습니다.",
                    false,
                    ComparisonSound.Empty);
            }
        }

        /// <summary>
        /// 정상 결과.
        /// </summary>
        public static ComparisonResult OK
        {
            get
            {
                return new ComparisonResult(
                    "OK", 
                    Brushes.Green, 
                    "입력된 바코드와 모델 이름이 동일합니다.",
                    false, 
                    ComparisonSound.UserAccount);
            }
        }

        /// <summary>
        /// 불량 결과.
        /// </summary>
        public static ComparisonResult NG
        {
            get
            {
                return new ComparisonResult(
                    "NG", 
                    Brushes.Red, 
                    "입력된 바코드와 모델 이름이 동일하지 않습니다.",
                    true,
                    ComparisonSound.RadioInterruption);
            }
        }

        #endregion

        #region Private constructors

        private ComparisonResult(
            string key, 
            Brush color, 
            string description,
            bool isLoopCount,
            ComparisonSound sound)
        {
            Key = key;
            Brush = color;
            Description = description;
            IsLoopSound = isLoopCount;
            Sound = sound;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// 출력 문자열.
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        /// <summary>
        /// 문자열 색.
        /// </summary>
        public Brush Brush
        {
            get;
            private set;
        }

        /// <summary>
        /// 결과 설명.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 사운드 반복 여부.
        /// True:사운드 반복, False:사운드 반복 없음.
        /// </summary>
        public bool IsLoopSound
        {
            get;
            private set;
        }

        /// <summary>
        /// 사운드 리소스.
        /// </summary>
        public ComparisonSound Sound
        {
            get;
            private set;
        }

        #endregion

        #region Override methods

        public static bool operator == (ComparisonResult left, ComparisonResult right)
        {
            return left.Key == right.Key;
        }

        public static bool operator != (ComparisonResult left, ComparisonResult right)
        {
            return (left == right) == false;
        }

        public override bool Equals(object obj)
        {
            if (obj is ComparisonResult result)
            {
                return Key == result.Key;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Key;
        }

        #endregion
    }
}
