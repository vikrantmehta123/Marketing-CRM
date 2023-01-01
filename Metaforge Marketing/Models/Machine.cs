

using System.ComponentModel;
using System.Windows;

namespace Metaforge_Marketing.Models
{
    public class Machine : ModelsBase
    {
        private string _machineName, _machineDescription;
        private int _id;
        private float _efficiency, _mchr, _costPerPiece;
        private int _cycleTime;

        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = value; }
        }

        public string MachineDescription
        {
            get { return _machineDescription; }
            set { _machineDescription = value; }
        }

        public float Efficiency
        {
            get { return _efficiency;}
            set { 
                _efficiency = value;
                OnPropertyChanged(nameof(Efficiency));
                OnPropertyChanged(nameof(CostPerPiece));
            }
        }
        public float MCHr
        {
            get { return _mchr;}
            set { 
                _mchr = value;
                OnPropertyChanged(nameof(MCHr));
                OnPropertyChanged(nameof(CostPerPiece));
            }
        }

        public int CycleTime
        {
            get { return _cycleTime; }
            set { 
                _cycleTime = value;
                OnPropertyChanged(nameof(CycleTime));
                OnPropertyChanged(nameof(CostPerPiece));
            }
        }

        public float CostPerPiece
        {
            get
            {
                if(_cycleTime != 0)
                {
                    _costPerPiece = ComputeCostPerPiece();
                }
                return _costPerPiece;
            }
            set
            {
                _costPerPiece = value;
                OnPropertyChanged(nameof(CostPerPiece));
            }
        }

        public float ComputeCostPerPiece()
        {
            int SecondsInAnHour = 3600;
            if (Efficiency> 0)
            {
                return MCHr / ((SecondsInAnHour / CycleTime) * (Efficiency / 100));
            }
            return CostPerPiece;
        }
    }
}
