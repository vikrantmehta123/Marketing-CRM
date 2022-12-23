using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Metaforge_Marketing.Models
{
    /// <summary>
    /// Serves as a container of properties that need to be shared across models
    /// </summary>
    public abstract class SharedModelsBase : ModelsBase
    {
        #region INPC Static
        public static event PropertyChangedEventHandler StaticPropertyChanged;

        public static void NotifyStaticPropertyChanged([CallerMemberName] string name = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(name));
        }
        #endregion INPC Static

        private static DateTime _startDate = DateTime.Today, _endDate = DateTime.Today;

        public static DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                NotifyStaticPropertyChanged(nameof(StartDate));
            }
        }

        public static DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                NotifyStaticPropertyChanged(nameof(EndDate));
            }
        }
    }
}
