using System.Timers;

namespace HVision.Common.Extension
{
    /// <summary>
    /// 타이머 이벤트 발생 최대 횟수를 설정 가능한 타이머.
    /// </summary>
    public class CountTimer : Timer
    {
        #region Fields

        public new event ElapsedEventHandler Elapsed;

        private int _maxLoopCount = 1;
        private int _currentLoopCount = 1;

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public CountTimer() : this(100)
        {
            
        }

        /// <summary>
        /// 인터벌 입력 생성자.
        /// </summary>
        /// <param name="interval"></param>
        public CountTimer(double interval) : base(interval)
        {
            base.Elapsed += CountTimer_Elapsed;
            Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 최대 반복 횟수.
        /// </summary>
        public int MaxLoopCount
        {
            get => _maxLoopCount;
            set
            {
                _maxLoopCount = value;
                Reset();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 초기화.
        /// </summary>
        public void Reset()
        {
            Stop();
            _currentLoopCount = 1;
        }

        #endregion

        #region Event handlers

        private void CountTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_maxLoopCount != 0 && _maxLoopCount <= _currentLoopCount++)
            {
                Reset();
            }
            else
            {
                this.Elapsed?.Invoke(sender, e);
            }
        }

        #endregion
    }
}
