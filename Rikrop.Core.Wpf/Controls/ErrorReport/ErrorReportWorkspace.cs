using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Rikrop.Core.Wpf.Commands;
using Rikrop.Core.Wpf.Mvvm;

namespace Rikrop.Core.Wpf.Controls.ErrorReport
{
    public class ErrorReportWorkspace : Workspace<ErrorReportWindow>
    {
        private readonly ExceptionModel _handledException;
        private readonly RelayCommand _closeApplicationCommand;

        public ICommand CloseApplicationCommand
        {
            get { return _closeApplicationCommand; }
        }

        public string Message
        {
            get { return HandledException.Exception.Message; }
        }

        public string Description
        {
            get { return HandledException.Exception.ToString(); }
        }

        public ExceptionModel HandledException
        {
            get { return _handledException; }
        }

        public IList<ExceptionModel> HandledExceptions
        {
            get { return new List<ExceptionModel>(new[] {HandledException}); }
        }

        public override string DisplayName
        {
            get { return "Отчёт об ошибке"; }
        }

        public ErrorReportWorkspace(Exception handledException)
        {
            Contract.Requires<ArgumentNullException>(handledException != null);

            _handledException = ParseHandledException(handledException);
            _closeApplicationCommand = new RelayCommand(CloseApplication);
        }

        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private ExceptionModel ParseHandledException(Exception ex)
        {
            var exceptionVm = new ExceptionModel(ex);

            if (ex.InnerException != null)
            {
                exceptionVm.AddException(ParseHandledException(ex.InnerException));
            }

            if (ex is ReflectionTypeLoadException)
            {
                var typeLoadException = ex as ReflectionTypeLoadException;
                if (typeLoadException.LoaderExceptions != null)
                {
                    foreach (var loaderException in typeLoadException.LoaderExceptions)
                    {
                        exceptionVm.AddException(ParseHandledException(loaderException));
                    }
                }
            }
            return exceptionVm;
        }
    }
}