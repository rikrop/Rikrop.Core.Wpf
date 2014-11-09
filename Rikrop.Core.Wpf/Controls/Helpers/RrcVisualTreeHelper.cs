using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Rikrop.Core.Wpf.Controls.Helpers
{
    public static class RrcVisualTreeHelper
    {
        public static TItem FindVisualChild<TItem>(this DependencyObject obj) where TItem : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is TItem)
                {
                    return (TItem) child;
                }

                var childOfChild = FindVisualChild<TItem>(child);

                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        public static TItem FindVisualChild<TItem>(this DependencyObject obj, string childName = null)
            where TItem : FrameworkElement
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TItem &&
                    (string.IsNullOrEmpty(childName) || childName == ((TItem) child).Name))
                {
                    return (TItem) child;
                }
                var childOfChild = FindVisualChild<TItem>(child, childName);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        public static TItem FindVisualChild<TItem>(this DependencyObject obj, Predicate<TItem> condition)
            where TItem : FrameworkElement
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is TItem && condition(child as TItem))
                {
                    return (TItem) child;
                }
                var childOfChild = FindVisualChild(child, condition);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }

            return null;
        }

        public static IEnumerable<TItem> FindVisualChildren<TItem>(this DependencyObject obj)
            where TItem : DependencyObject
        {
            var result = new List<TItem>();
            FindVisualChildren(obj, result);
            return result;
        }

        public static TItem FindVisualParent<TItem>(this DependencyObject obj) where TItem : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(obj);

            if (parent != null && parent is TItem)
            {
                return (TItem) parent;
            }
            if (parent == null)
            {
                return null;
            }

            var parentOfParent = FindVisualParent<TItem>(parent);

            if (parentOfParent != null)
            {
                return parentOfParent;
            }

            return null;
        }

        /// <summary>
        ///     Метод ищет визуальный элемент имеющий DataContext определенного типа, поднимаясь под визуальному дереву
        /// </summary>
        /// <param name="obj"> Визуальный элемент, с которого начинается поиск </param>
        /// <param name="dataContextType"> Требуемый тип DataContext-а </param>
        /// <returns> </returns>
        public static FrameworkElement FindVisualParentByDataContextType(this DependencyObject obj, Type dataContextType)
        {
            var parent = VisualTreeHelper.GetParent(obj) as FrameworkElement;

            if (parent != null && parent.DataContext != null &&
                dataContextType.IsAssignableFrom(parent.DataContext.GetType()))
            {
                return parent;
            }
            if (parent == null)
            {
                return null;
            }

            var parentOfParent = FindVisualParentByDataContextType(parent, dataContextType);

            if (parentOfParent != null)
            {
                return parentOfParent;
            }

            return null;
        }

        public static bool HasVisualChild(this DependencyObject obj, DependencyObject child)
        {
            if (child == null)
            {
                return false;
            }

            if (obj == child)
            {
                return true;
            }

            return obj.HasVisualChild(VisualTreeHelper.GetParent(child));
        }

        /// <summary>
        ///     Строит визуальное дерево для элемента (полезно для дебага)
        /// </summary>
        public static string BuildVisualTree(this DependencyObject obj)
        {
            var res = new StringBuilder();

            Action<object, int> write = (o, level) =>
                                        res.AppendLine(string.Format("{0}{1} ({2}){3}",
                                                                     string.Empty.PadRight(level, o == obj
                                                                                                      ? '-'
                                                                                                      : ' '),
                                                                     o.GetType().Name,
                                                                     o.GetHashCode(),
                                                                     o == obj
                                                                         ? " this"
                                                                         : string.Empty));

            Func<DependencyObject, int> buildUp = null;
            buildUp = (o) =>
                {
                    if (o == null)
                    {
                        return 0;
                    }
                    var level = buildUp(VisualTreeHelper.GetParent(o));
                    write(o, level);
                    return level + 1;
                };

            Action<DependencyObject, int> buildDown = null;
            buildDown = (o, level) =>
                {
                    for (var i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
                    {
                        var child = VisualTreeHelper.GetChild(o, i);
                        write(child, level);
                        buildDown(child, level + 1);
                    }
                };

            buildDown(obj, buildUp(obj));

            return res.ToString();
        }

        /// <summary>
        ///     Строит логическое дерево для элемента (полезно для дебага)
        /// </summary>
        public static string BuildLogicalTree(this DependencyObject obj)
        {
            var res = new StringBuilder();

            Action<object, int> write = (o, level) =>
                                        res.AppendLine(string.Format("{0}{1} ({2}){3}",
                                                                     string.Empty.PadRight(level, o == obj
                                                                                                      ? '-'
                                                                                                      : ' '),
                                                                     o.GetType().Name,
                                                                     o.GetHashCode(),
                                                                     o == obj
                                                                         ? " this"
                                                                         : string.Empty));

            Func<DependencyObject, int> buildUp = null;
            buildUp = (o) =>
                {
                    if (o == null)
                    {
                        return 0;
                    }
                    var level = buildUp(LogicalTreeHelper.GetParent(o));
                    write(o, level);
                    return level + 1;
                };

            Action<DependencyObject, int> buildDown = null;
            buildDown = (o, level) =>
                {
                    foreach (var child in LogicalTreeHelper.GetChildren(o))
                    {
                        write(child, level);
                        if (child is DependencyObject)
                        {
                            buildDown((DependencyObject) child, level + 1);
                        }
                    }
                };

            buildDown(obj, buildUp(obj));

            return res.ToString();
        }

        public static FrameworkElement FindAncestor(Type ancestorType, Visual visual)
        {
            while (visual != null && !ancestorType.IsInstanceOfType(visual))
            {
                visual = (Visual) VisualTreeHelper.GetParent(visual);
            }
            return visual as FrameworkElement;
        }

        public static bool IsMovementBigEnough(Point initialMousePosition, Point currentPosition)
        {
            return (Math.Abs(currentPosition.X - initialMousePosition.X) >=
                    SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPosition.Y - initialMousePosition.Y) >= SystemParameters.MinimumVerticalDragDistance);
        }

        private static void FindVisualChildren<TItem>(DependencyObject obj, List<TItem> items)
            where TItem : DependencyObject
        {
            if (obj is TItem)
            {
                items.Add((TItem) obj);
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                FindVisualChildren(child, items);
            }
        }
    }
}