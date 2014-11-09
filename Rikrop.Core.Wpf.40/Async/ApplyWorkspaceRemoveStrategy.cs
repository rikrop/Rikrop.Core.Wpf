using System;
using Rikrop.Core.Wpf.Mvvm;

namespace Rikrop.Core.Wpf.Async
{
    public class ApplyWorkspaceRemoveStrategy : IBusyItemRemoveStrategy
    {
        private readonly IApplyWorkspace _workspace;

        public ApplyWorkspaceRemoveStrategy(IApplyWorkspace workspace)
        {
            _workspace = workspace;
            _workspace.RequestClose +=
                (sender, args) =>
                    {
                        if (!_workspace.IsApplying)
                        {
                            RaiseRequestRemove();
                        }
                    };
        }

        public event Action RequestRemove;

        private void RaiseRequestRemove()
        {
            Action handler = RequestRemove;
            if (handler != null)
            {
                handler();
            }
        }
    }
}