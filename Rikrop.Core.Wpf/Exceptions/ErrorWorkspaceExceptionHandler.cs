using System;
using System.Windows;
using Rikrop.Core.Framework.Exceptions;
using Rikrop.Core.Wpf.Controls.ErrorReport;

namespace Rikrop.Core.Wpf.Exceptions
{
    public class ErrorWorkspaceExceptionHandler : IExceptionHandler
    {
        private Exception _unhandledException;

        public bool Handle(Exception exception)
        {
            try
            {
                if (IsTheSameException(exception))
                {
                    return true;
                }
                _unhandledException = exception;

                if (!Application.Current.Dispatcher.CheckAccess())
                {
                    Application.Current.Dispatcher.Invoke(() => ShowErrorWindow(exception));
                }
                else
                {
                    ShowErrorWindow(exception);
                }
                return true;
            }
            finally
            {
                _unhandledException = null;
            }
        }

        private bool IsTheSameException(Exception ex)
        {
            return _unhandledException != null && _unhandledException.GetType() == ex.GetType();
        }

        private void ShowErrorWindow(Exception exception)
        {
            var vm = new ErrorReportWorkspace(exception);
            vm.TypedContent.ShowDialog();
        }
    }
}