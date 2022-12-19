

namespace Metaforge_Marketing.Models
{
    public class Machine
    {
        private string _machineName, _machineDescription;
        private int _id;

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

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
    }
}
