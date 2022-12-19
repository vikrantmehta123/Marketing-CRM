

namespace Metaforge_Marketing.Models
{
    public class Operation
    {
        private string _operationName;
        private int _id;

        public string OperationName { get { return _operationName;} set { _operationName = value; } }   
        public int Id { get { return _id;} set { _id = value;} } 
    }
}
