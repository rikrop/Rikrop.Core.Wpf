using System.Threading;
using Rikrop.Core.Wpf.Mvvm.Navigation;
using NUnit.Framework;

namespace Rikrop.Core.Wpf.Test.Mvvm.Navigation
{
    [TestFixture, Timeout(5000)]
    public class NavigationTaskTest
    {
        [Test]
        public void NavigationTaskShouldReturnFalseWhenInterrupted()
        {
            var navigationTask = new NavigationTask(new NavigationFakeWorkspace());

            ThreadPool.QueueUserWorkItem(o => navigationTask.Interrupt());

            var result = navigationTask.Task.Result;

            Assert.False(result);
        }

        [Test]
        public void NavigationTaskShouldReturnTrueWhenCompleted()
        {
            var navigationTask = new NavigationTask(new NavigationFakeWorkspace());

            ThreadPool.QueueUserWorkItem(o => navigationTask.Complete());

            var result = navigationTask.Task.Result;

            Assert.True(result);
        }

        [Test]
        public void NavigationTaskShouldReturnTrueWhenWorkspaceClosed()
        {
            var navigationTask = new NavigationTask(new NavigationFakeWorkspace());

            ThreadPool.QueueUserWorkItem(o => navigationTask.Workspace.CloseCommand.Execute(null));

            var result = navigationTask.Task.Result;

            Assert.True(result);
        }
    }
}