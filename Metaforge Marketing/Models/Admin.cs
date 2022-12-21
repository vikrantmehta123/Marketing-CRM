
namespace Metaforge_Marketing.Models
{
    public class Admin
    {
        #region Fields
        private int _id;
        private string _name;
        #endregion Fields

        #region Properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion Properties

        #region Methods
        public override string ToString() { return Name; }
        #endregion Methods
    }
}
