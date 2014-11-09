using System;
using System.Linq.Expressions;

namespace Rikrop.Core.Wpf
{
    internal class PropertyCallHelper
    {
        public static PropertyCall GetPropertyCall(Expression<Func<object>> property)
        {
            var memberExpression = GetMemberExpression(property.Body);

            object targetObject;

            if (memberExpression.Expression is ConstantExpression)
            {
                // ��������� ���� () => MyProperty, targetObject - ��� �������� �������� MyProperty, Myproperty ����� �� value-���

                targetObject = (memberExpression.Expression as ConstantExpression).Value;
            }
            else if (memberExpression.Expression is MemberExpression)
            {
                // ��������� ���� () => someExpr.SomeProperty, ��� someExpr ����� ���� ������������ ����������.
                // ��� ��������� targetObject ����� ��������� ���������, �������� ���������.

                targetObject = GetValue(memberExpression.Expression as MemberExpression);
            }
            else
            {
                throw new ArgumentException("Expected sequence of property calls.");
            }

            return new PropertyCall(targetObject, memberExpression.Member.Name);
        }

        private static MemberExpression GetMemberExpression(Expression body)
        {
            // ���� ������������ �������� value-���, �� UnaryExpression.

            if (body is UnaryExpression)
            {
                return (MemberExpression) (body as UnaryExpression).Operand;
            }

            if (body is MemberExpression)
            {
                return body as MemberExpression;
            }

            throw new ArgumentException("Expected sequence of property calls.");
        }

        private static object GetValue(Expression expression)
        {
            var convertedExpr = Expression.Convert(expression, typeof (object));
            var getterLambda = Expression.Lambda<Func<object>>(convertedExpr);
            var getter = getterLambda.Compile();
            return getter();
        }
    }
}