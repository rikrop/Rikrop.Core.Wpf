using System.Threading.Tasks;
using Rikrop.Core.Wpf.Mvvm.Navigation;
using NUnit.Framework;

namespace Rikrop.Core.Wpf.Test.Mvvm.Navigation
{
    [TestFixture, Timeout(155000)]
    public class NavigationSequenceTest
    {
        [Test]
        public async void NavigationSequenceShouldReturnTrueForFirstNavigatedTaskWhenCompleted()
        {
            var navigationSequence = new NavigationSequence();

            var task1 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task2 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task3 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));

            await navigationSequence.Complete();

            var result1 = task1.Result;
            var result2 = task2.Result;
            var result3 = task3.Result;

            Assert.True(result1, "First task return not valid result");
            Assert.False(result2, "Second task return not valid result");
            Assert.False(result3, "Third task return not valid result");
        }

        [Test]
        public async void NavigationSequenceShouldReturnFalseForAllNavigatedTaskWhenInterrupted()
        {
            var navigationSequence = new NavigationSequence();

            var task1 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task2 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task3 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));

            await navigationSequence.Interrupt();

            var result1 = task1.Result;
            var result2 = task2.Result;
            var result3 = task3.Result;

            Assert.False(result1, "First task return not valid result");
            Assert.False(result2, "Second task return not valid result");
            Assert.False(result3, "Third task return not valid result");
        }

        [Test]
        public async void NavigationSequenceShouldReturnTrueIfHasWorkspace()
        {
            var navigationSequence = new NavigationSequence();

            var w1 = new NavigationFakeWorkspace();
            var task1 = navigationSequence.Navigate(new NavigationTask(w1));
            var task2 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task3 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));

            Assert.True(await navigationSequence.TryNavigateBack(w1));

            var result2 = task2.Result;
            var result3 = task3.Result;

            Assert.True(task1.Status != TaskStatus.RanToCompletion, "First task return not valid result");
            Assert.False(result2, "Second task return not valid result");
            Assert.False(result3, "Third task return not valid result");
        }

        [Test]
        public async void NavigationSequenceShouldReturnFasleNotHasWorkspace()
        {
            var navigationSequence = new NavigationSequence();

            var w0 = new NavigationFakeWorkspace();
            var task1 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task2 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task3 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));

            Assert.False(await navigationSequence.TryNavigateBack(w0));

            var result1 = task1.Result;
            var result2 = task2.Result;
            var result3 = task3.Result;

            Assert.False(result1, "First task return not valid result");
            Assert.False(result2, "Second task return not valid result");
            Assert.False(result3, "Third task return not valid result");
        }

        [Test]
        public async void NavigationSequenceShouldReturnTrueIfNavigatedLastWorkspace()
        {
            var navigationSequence = new NavigationSequence();

            var w3 = new NavigationFakeWorkspace();
            var task1 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task2 = navigationSequence.Navigate(new NavigationTask(new NavigationFakeWorkspace()));
            var task3 = navigationSequence.Navigate(new NavigationTask(w3));

            Assert.True(await navigationSequence.TryNavigateBack(w3));

            Assert.True(task1.Status != TaskStatus.RanToCompletion, "First task return not valid result");
            Assert.True(task2.Status != TaskStatus.RanToCompletion, "Second task return not valid result");
            Assert.True(task3.Status != TaskStatus.RanToCompletion, "Third task return not valid result");
        }
    }
}