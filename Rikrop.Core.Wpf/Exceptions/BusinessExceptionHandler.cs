using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using Rikrop.Core.Framework.Exceptions;

namespace Rikrop.Core.Wpf.Exceptions
{
    public class BusinessExceptionHandler : IExceptionHandler
    {
        private readonly IDialogShower _dialogShower;
        private readonly IBusinessExceptionDetailsConverter _typeConverter;

        public BusinessExceptionHandler(IDialogShower dialogShower)
            :this(dialogShower, new EnumBusinessExceptionDetailsConverter())
        {
        }

        public BusinessExceptionHandler(IDialogShower dialogShower, IBusinessExceptionDetailsConverter typeConverter)
        {
            Contract.Requires<ArgumentNullException>(dialogShower != null);
            Contract.Requires<ArgumentNullException>(typeConverter != null);

            _dialogShower = dialogShower;
            _typeConverter = typeConverter;
        }

        public bool Handle(Exception exception)
        {
            var businessException = exception as BusinessException;
            if (businessException == null)
            {
                return false;
            }

            var detailValue = businessException.Details;
            if (detailValue == null)
            {
                return false;
            }

            if (!_typeConverter.CanConvert(detailValue))
            {
                return false;
            }

            _dialogShower.ShowError(_typeConverter.Convert(detailValue));
            return true;
        }
    }
}