using HVision.Barcompare.Model;
using HVision.Common.Extension;
using System;
using System.Collections.Generic;
using WMPLib;

namespace HVision.Barcompare.Core
{
    /// <summary>
    /// 결과로 결정되는 사운드 음성 플레이어.
    /// </summary>
    public class ComparisonSoundPlayer : IDisposable
    {
        #region Fields

        private readonly int TIMER_INTERVAL = 1500;

        private static volatile WindowsMediaPlayer _currentPlayer = null;
        private CountTimer _timer = new CountTimer();
        private IEnumerable<ComparisonResult> _wavFiles = new List<ComparisonResult>();
        private Dictionary<ComparisonResult, WindowsMediaPlayer> _playerMap = new Dictionary<ComparisonResult, WindowsMediaPlayer>();

        #endregion

        #region Constructors

        /// <summary>
        /// 기본 생성자.
        /// </summary>
        public ComparisonSoundPlayer()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// 설정.
        /// </summary>
        public Configuration Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// 사운드 플레이 wav파일 맵.
        /// </summary>
        public IEnumerable<ComparisonResult> WavFiles
        {
            get => _wavFiles;
            set
            {
                if (_wavFiles == value)
                {
                    return;
                }

                // 파일명맵 재할당.
                _wavFiles = value;

                // 스트림맵 재할당.
                foreach (var pair in _wavFiles)
                {
                    if (pair.Sound.FileName == null)
                    {
                        continue;
                    }

                    WindowsMediaPlayer wplayer = new WindowsMediaPlayer
                    {
                        URL = pair.Sound.FileName,
                    };
                    wplayer.settings.mute = true;

                    _playerMap.Add(pair, wplayer);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 사운드 플레이.
        /// </summary>
        /// <param name="result">결과값.</param>
        public void Play(ComparisonResult result)
        {
            Stop();

            if (_playerMap.TryGetValue(result, out WindowsMediaPlayer findedPlayer) == true)
            {
                _currentPlayer = findedPlayer;
                _currentPlayer.settings.mute = false;
                _currentPlayer.controls.play();

                if (result.IsLoopSound == true)
                {
                    _timer.Interval = TIMER_INTERVAL;
                    _timer.MaxLoopCount = Configuration == null ? 1 : Configuration.MaxLoopCount;
                    _timer.Elapsed += Timer_Elapsed;
                    _timer.Start();
                }
            }
        }

        /// <summary>
        /// 사운드 중지.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();

            if (_currentPlayer != null)
            {
                _currentPlayer.controls.stop();
            }
        }

        #endregion

        #region Event handlers

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _currentPlayer.controls.play();
        }

        #endregion

        #region IDisposable members

        public void Dispose()
        {
            _timer.Dispose();
        }

        #endregion
    }
}
