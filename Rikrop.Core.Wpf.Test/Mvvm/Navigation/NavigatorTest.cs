using Rikrop.Core.Framework;
using Rikrop.Core.Wpf.Mvvm.Navigation;
using NUnit.Framework;

namespace Rikrop.Core.Wpf.Test.Mvvm.Navigation
{
    //[TestFixture, Timeout(5000)]
    [TestFixture]
    public class NavigatorTest
    {
        [Test]
        public void NavigatorShouldReturnToWorkspaceThatStartSequenceWhenSequenceCompleted()
        {
            var navigator = new Navigator();
            var sequenceWorkspace = new NavigationFakeWorkspace("Workspace that start sequence");
            var firstWorkspace = new NavigationFakeWorkspace("First Workspace in sequence");
            var secondWorkspace = new NavigationFakeWorkspace("Second Workspace in sequence");
            var thirdWorkspace = new NavigationFakeWorkspace("Third Workspace in sequence");


            navigator.NavigateTo(sequenceWorkspace);
            navigator.StartNewSequenceFrom(firstWorkspace);
            navigator.NavigateTo(secondWorkspace);
            navigator.NavigateTo(thirdWorkspace);

            navigator.CompleteCurrentSequence();

            Assert.AreEqual(sequenceWorkspace, navigator.Workspace);
        }

        [Test]
        public void NavigatorShouldTrackOpenedWorkspaces()
        {
            var navigator = new Navigator();
            var sequenceWorkspace = new NavigationFakeWorkspace("Workspace that start sequence");
            var firstWorkspace = new NavigationFakeWorkspace("First Workspace in sequence");
            var secondWorkspace = new NavigationFakeWorkspace("Second Workspace in sequence");
            var thirdWorkspace = new NavigationFakeWorkspace("Third Workspace in sequence");


            navigator.NavigateTo(sequenceWorkspace);
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {sequenceWorkspace}));

            navigator.StartNewSequenceFrom(firstWorkspace);
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {sequenceWorkspace, firstWorkspace}));

            navigator.NavigateTo(secondWorkspace);
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {sequenceWorkspace, firstWorkspace, secondWorkspace}));

            secondWorkspace.Close();
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {sequenceWorkspace, firstWorkspace}));

            firstWorkspace.Close();
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {sequenceWorkspace}));

            navigator.NavigateTo(thirdWorkspace);
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {sequenceWorkspace, thirdWorkspace}));

            thirdWorkspace.Close();
            sequenceWorkspace.Close();
            Assert.True(navigator.Workspaces.Count == 0);
        }


        [Test]
        public void NavigatorShouldNavigateBackToWorkspace()
        {
            var navigator = new Navigator();
            var sequenceWorkspace = new NavigationFakeWorkspace("Workspace that start sequence");
            var firstWorkspace = new NavigationFakeWorkspace("First Workspace in sequence");
            var secondWorkspace = new NavigationFakeWorkspace("Second Workspace in sequence");
            var thirdWorkspace = new NavigationFakeWorkspace("Third Workspace in sequence");


            navigator.NavigateTo(sequenceWorkspace);
            navigator.StartNewSequenceFrom(firstWorkspace);
            navigator.NavigateTo(secondWorkspace);

            navigator.NavigateBack(firstWorkspace).Wait();
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] { sequenceWorkspace, firstWorkspace }));
            Assert.AreEqual(navigator.Workspace, firstWorkspace);

            navigator.NavigateTo(thirdWorkspace);
            navigator.NavigateBack(sequenceWorkspace).Wait();
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] { sequenceWorkspace }));
            Assert.AreEqual(navigator.Workspace, sequenceWorkspace);
        }

        [Test]
        public void NavigatorShouldNavigateBackToRoot()
        {
            var navigator = new Navigator();
            var rootWorkspace = new NavigationFakeWorkspace("Root Workspace");
            var firstWorkspace = new NavigationFakeWorkspace("First Workspace in sequence");
            var secondWorkspace = new NavigationFakeWorkspace("Second Workspace in sequence");
            var thirdWorkspace = new NavigationFakeWorkspace("Third Workspace in sequence");

            navigator.StartNewSequenceFrom(rootWorkspace);
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] { rootWorkspace }));
            Assert.AreEqual(navigator.Workspace, rootWorkspace);

            navigator.NavigateTo(firstWorkspace);
            navigator.StartNewSequenceFrom(secondWorkspace);
            navigator.NavigateTo(thirdWorkspace);

            navigator.BackToRoot();
            Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] { rootWorkspace }));
            Assert.AreEqual(navigator.Workspace, rootWorkspace);
        }

        [Test]
        public void WorkspaceIsInWorkspacesWhenNotified()
        {
            var navigator = new Navigator();
            var rootWorkspace = new NavigationFakeWorkspace("Root Workspace");
            var firstWorkspace = new NavigationFakeWorkspace("First Workspace in sequence");

            navigator.StartNewSequenceFrom(rootWorkspace);

            navigator.PropertyChanged += (sender, args) =>
                                             {
                                                 if (args.PropertyName == ExpressionHelper.GetName<Navigator>(navigator1 => navigator1.Workspace))
                                                 {
                                                     Assert.That(navigator.Workspaces, Is.EquivalentTo(new[] {rootWorkspace, firstWorkspace}));
                                                 }
                                             };

            navigator.NavigateTo(firstWorkspace);
        }
    }
}