using System;
using System.Windows.Controls;
using Rikrop.Core.Wpf.Controls.Validation.VBorder;

namespace Rikrop.Core.Wpf.Controls.Validation.VGrid
{
    public class ValidationGrid : Grid
    {
        public static Border CreateValidationBorder(ValidatedRowDefinition validatedRowDefinition)
        {
            var b = new RrcValidationBorder
                {
                    ValidationErrorBrush = validatedRowDefinition.ValidationErrorBrush,
                };

            b.SetBinding(RrcValidationBorder.ValidationBindingProperty, validatedRowDefinition.ValidationBinding);
            return b;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            var rowNum = 0;
            foreach (var row in RowDefinitions)
            {
                var vrow = row as ValidatedRowDefinition;
                if (vrow != null && vrow.ValidationBinding != null)
                {
                    AddValidationElement(vrow, rowNum);
                }
                ++rowNum;
            }
        }

        private void AddValidationElement(ValidatedRowDefinition vrow, int rowNum)
        {
            for (var i = 0; i < InternalChildren.Count; i++)
            {
                var child = InternalChildren[i];
                if (GetRow(child) != rowNum)
                {
                    continue;
                }

                var childColumn = GetColumn(child);
                if (vrow.ValidationElementStartColumn > 0 && childColumn < vrow.ValidationElementStartColumn)
                {
                    continue;
                }
                if (vrow.ValidationColumnSpan > 0 &&
                    childColumn > vrow.ValidationElementStartColumn + vrow.ValidationColumnSpan - 1)
                {
                    continue;
                }

                var valEl = CreateValidationBorder(vrow);
                SetRow(valEl, rowNum);
                SetColumn(valEl, childColumn);
                SetColumnSpan(valEl, GetColumnSpan(child));
                SetRowSpan(valEl, GetRowSpan(child));

                InternalChildren.RemoveAt(i);
                valEl.Child = child;
                InternalChildren.Insert(i, valEl);
            }
        }
    }
}