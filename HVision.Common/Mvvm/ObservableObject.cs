// http://www.markwithall.com/programming/2013/03/01/worlds-simplest-csharp-wpf-mvvm-example.html

using System.ComponentModel;

namespace HVision.Common.Mvvm
{
    /// <summary>
    /// 변경 감시 객체.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Protected methods

        /// <summary>
        /// 프로퍼티 변경 알림 메소드.
        /// </summary>
        /// <param name="propertyName">변경된 프로퍼티 이름.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
