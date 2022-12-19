

using Microsoft.Data.SqlClient;
using System;
using System.Windows.Input;

namespace Metaforge_Marketing.HelperClasses.Commands
{
    public class SaveCommand<T>
    {
        #region Fields
        private Action<SqlConnection, T> _insertFunction;
        private T _objToInsert;
        private ICommand _command;
        #endregion Fields

        public ICommand Command
        {
            get
            {
                if(_command == null)
                {
                    SQLWrapper<T>.InsertWrapper(_insertFunction, _objToInsert);
                }
                return _command;
            }
        }

        public SaveCommand(Action<SqlConnection, T> insertFunction, T objToInsert)
        {
            _insertFunction = insertFunction;
            _objToInsert = objToInsert;
        }

    }
}
