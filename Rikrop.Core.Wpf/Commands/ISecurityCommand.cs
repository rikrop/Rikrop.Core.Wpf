using System.Windows.Input;

namespace Rikrop.Core.Wpf.Commands
{
    /// <summary>
    /// Описывает команду, выполняемую при наличии прав доступа
    /// </summary>
    public interface ISecurityCommand : ICommand
    {
        /// <summary>
        /// Достаточно ли прав для выполнения команды
        /// </summary>
        bool EnoughRights { get; }
    }
}