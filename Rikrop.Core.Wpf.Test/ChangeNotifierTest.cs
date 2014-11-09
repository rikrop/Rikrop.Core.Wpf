using System.Collections.Generic;
using Rikrop.Core.Framework;
using NUnit.Framework;

namespace Rikrop.Core.Wpf.Test
{
    [TestFixture]
    public class ChangeNotifierTest
    {
        [Test]
        public void ShouldNotifyAllLinkedPropertiesAfterSourcePropertyIsChanged()
        {
            var notifier = new TestChangeNotifier();

            var expectedOrder = new List<string>
                                             {
                                                 ExpressionHelper.GetName<TestChangeNotifier>(o => o.SourceAfterProperty),
                                                 ExpressionHelper.GetName<TestChangeNotifier>(o => o.AfterProperty1),
                                                 ExpressionHelper.GetName<TestChangeNotifier>(o => o.AfterProperty2)
                                             };
            
            var realOrder = new List<string>();
            notifier.PropertyChanged += (sender, args) => realOrder.Add(args.PropertyName);

            notifier.SourceAfterProperty = "Test";

            Assert.AreEqual(expectedOrder, realOrder, "Порядок нотификации свойств не совпадает");
        }

        [Test]
        public void ShouldNotifyAllLinkedPropertiesBeforeSourcePropertyIsChanged()
        {
            var notifier = new TestChangeNotifier();

            var expectedOrder = new List<string>
                                             {
                                                 ExpressionHelper.GetName<TestChangeNotifier>(o => o.BeforeProperty1),
                                                 ExpressionHelper.GetName<TestChangeNotifier>(o => o.BeforeProperty2),
                                                 ExpressionHelper.GetName<TestChangeNotifier>(o => o.SourceBeforeProperty),
                                             };

            var realOrder = new List<string>();
            notifier.PropertyChanged += (sender, args) => realOrder.Add(args.PropertyName);

            notifier.SourceBeforeProperty = "Test";

            Assert.AreEqual(expectedOrder, realOrder, "Порядок нотификации свойств не совпадает");
        }

        [Test]
        public void ShouldNotifyTargetPropertyAfterAnyOfDependentPropertyIsChanged()
        {
            bool isDependentProperty1Changed = false;
            bool isDependentProperty2Changed = false;
            var obj1 = new DependentObject1();
            var obj2 = new DependentObject2();
            var notifier = new TestChangeNotifier(obj1, obj2, true);

            obj1.PropertyChanged += (sender, args) => isDependentProperty1Changed = true;
            obj2.PropertyChanged += (sender, args) => isDependentProperty2Changed = true;
            int calledCount = 0;
            notifier.PropertyChanged +=
                (sender, args) =>
                    {
                        Assert.True(isDependentProperty1Changed || isDependentProperty2Changed, "Перед нотификацией целевого свойства не были нотифицированы зависимые свойства.");

                        calledCount++;

                        Assert.AreEqual(ExpressionHelper.GetName<TestChangeNotifier>(o => o.TargetAfterProperty),
                                        args.PropertyName);
                    };

            obj1.DependentProperty1 = true;
            Assert.AreEqual(1, calledCount, "Событие об изменении было вызвано неверное число раз");
            obj2.DependentProperty2 = true;
            Assert.AreEqual(2, calledCount, "Событие об изменении было вызвано неверное число раз");
        }

        [Test]
        public void ShouldNotifyTargetPropertyBeforeAnyOfDependentPropertyIsChanged()
        {
            bool isDependentProperty1Changed = false;
            bool isDependentProperty2Changed = false;
            var obj1 = new DependentObject1();
            var obj2 = new DependentObject2();
            var notifier = new TestChangeNotifier(obj1, obj2, false);

            obj1.PropertyChanged += (sender, args) => isDependentProperty1Changed = true;
            obj2.PropertyChanged += (sender, args) => isDependentProperty2Changed = true;
            int calledCount = 0;
            notifier.PropertyChanged +=
                (sender, args) =>
                {
                    Assert.False(isDependentProperty1Changed && isDependentProperty2Changed, "Перед нотификацией целевого свойства не должны быть нотифицированы зависимые свойства.");

                    calledCount++;

                    Assert.AreEqual(ExpressionHelper.GetName<TestChangeNotifier>(o => o.TargetBeforeProperty),
                                    args.PropertyName);
                };

            obj1.DependentProperty1 = true;
            Assert.AreEqual(1, calledCount, "Событие об изменении было вызвано неверное число раз");
            obj2.DependentProperty2 = true;
            Assert.AreEqual(2, calledCount, "Событие об изменении было вызвано неверное число раз");
        }


        private class TestChangeNotifier : ChangeNotifier
        {
            private string _sourceAfterProperty;
            public string SourceAfterProperty
            {
                get { return _sourceAfterProperty; }
                set
                {
                    if(_sourceAfterProperty == value)
                    {
                        return;
                    }
                    _sourceAfterProperty = value;
                    NotifyPropertyChanged(o => SourceAfterProperty);
                }
            }

            private string _sourceBeforeProperty;
            public string SourceBeforeProperty
            {
                get { return _sourceBeforeProperty; }
                set
                {
                    if(_sourceBeforeProperty == value)
                    {
                        return;
                    }
                    _sourceBeforeProperty = value;
                    NotifyPropertyChanged(o => SourceBeforeProperty);
                }
            }

            private string _afterProperty1;
            public string AfterProperty1
            {
                get { return _afterProperty1; }
                set
                {
                    if(_afterProperty1 == value)
                    {
                        return;
                    }
                    _afterProperty1 = value;
                    NotifyPropertyChanged(o => AfterProperty1);
                }
            }

            private string _afterProperty2;
            public string AfterProperty2
            {
                get { return _afterProperty2; }
                set
                {
                    if(_afterProperty2 == value)
                    {
                        return;
                    }
                    _afterProperty2 = value;
                    NotifyPropertyChanged(o => AfterProperty2);
                }
            }

            private string _beforeProperty1;
            public string BeforeProperty1
            {
                get { return _beforeProperty1; }
                set
                {
                    if(_beforeProperty1 == value)
                    {
                        return;
                    }
                    _beforeProperty1 = value;
                    NotifyPropertyChanged(o => BeforeProperty1);
                }
            }

            private string _beforeProperty2;
            public string BeforeProperty2
            {
                get { return _beforeProperty2; }
                set
                {
                    if(_beforeProperty2 == value)
                    {
                        return;
                    }
                    _beforeProperty2 = value;
                    NotifyPropertyChanged(o => BeforeProperty2);
                }
            }

            public bool TargetAfterProperty
            {
                get { return true; }
            }

            public bool TargetBeforeProperty
            {
                get { return true; }
            }

            public TestChangeNotifier()
            {
                AfterNotify(() => SourceAfterProperty)
                    .Notify(() => AfterProperty1)
                    .Notify(() => AfterProperty2);

                BeforeNotify(() => SourceBeforeProperty)
                    .Notify(() => BeforeProperty1)
                    .Notify(() => BeforeProperty2);
            }

            public TestChangeNotifier(DependentObject1 obj1, DependentObject2 obj2, bool isAfter)
            {
                if (isAfter)
                {
                    Notify(() => TargetAfterProperty)
                        .AfterNotify(obj1, o => o.DependentProperty1)
                        .AfterNotify(obj1, o => o.DependentProperty1)
                        .AfterNotify(obj2, o => o.DependentProperty2);
                }
                else
                {
                    Notify(() => TargetBeforeProperty)
                        .BeforeNotify(obj1, o => o.DependentProperty1)
                        .BeforeNotify(obj1, o => o.DependentProperty1)
                        .BeforeNotify(obj2, o => o.DependentProperty2);
                }
            }
        }

        private class DependentObject1 : ChangeNotifier
        {
            private bool _dependentProperty1;
            public bool DependentProperty1
            {
                get { return _dependentProperty1; }
                set
                {
                    if(_dependentProperty1 == value)
                    {
                        return;
                    }
                    _dependentProperty1 = value;
                    NotifyPropertyChanged(o => DependentProperty1);
                }
            }
        }

        private class DependentObject2 : ChangeNotifier
        {
            private bool _dependentProperty2;
            public bool DependentProperty2
            {
                get { return _dependentProperty2; }
                set
                {
                    if(_dependentProperty2 == value)
                    {
                        return;
                    }
                    _dependentProperty2 = value;
                    NotifyPropertyChanged(o => DependentProperty2);
                }
            }

        }
    }
}
