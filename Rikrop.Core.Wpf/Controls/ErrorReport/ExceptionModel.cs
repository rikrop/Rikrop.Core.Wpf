using System;
using System.Collections.Generic;

namespace Rikrop.Core.Wpf.Controls.ErrorReport
{
    public class ExceptionModel
    {
        private List<ExceptionModel> _childExceptions;
        private Exception _exception;
        private IList<ExceptionModel> _onlyReadChildExceptions;

        public ExceptionModel(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            ChildExceptions = new List<ExceptionModel>();
            Exception = exception;
        }

        public List<ExceptionModel> ChildExceptions
        {
            get { return _childExceptions ?? (_childExceptions = new List<ExceptionModel>()); }
            private set { _childExceptions = value; }
        }

        public Exception Exception
        {
            get { return _exception; }
            private set
            {
                if (_exception == value)
                {
                    return;
                }
                _exception = value;
                if (_exception != null)
                {
                    StackTrace = _exception.StackTrace;
                    Message = _exception.Message;
                    TypeFullName = _exception.GetType().FullName;
                }
            }
        }

        public string Message { get; set; }

        public IList<ExceptionModel> OnlyReadChildExceptions
        {
            get { return _onlyReadChildExceptions ?? (_onlyReadChildExceptions = ChildExceptions.AsReadOnly()); }
        }

        public string StackTrace { get; set; }

        public string TypeFullName { get; set; }

        public string TypeName
        {
            get { return Exception.GetType().Name; }
        }

        public void AddException(ExceptionModel exceptionModel)
        {
            ChildExceptions.Add(exceptionModel);
        }
    }
}