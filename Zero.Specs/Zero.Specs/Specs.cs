using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace Zero.Specs
{
    public static class Specs
    {
        public static string FormatValue(this object value)
        {
            var s = value.ToString();

            if (value.GetType().ToString() != s)
            {
                return s;
            }

            var sb = new StringBuilder();

            if (value is IList)
            {
                var list = (IList)value;
                sb.Append("{");

                foreach (var e in list)
                {
                    sb.Append(e.FormatValue());
                    sb.Append(',');
                }

                sb.Remove(sb.Length - 1, 1);

                sb.Append("}");
            }

            return sb.ToString();
        }

        public static Scenario<T> Given<T>(T value)
        {
            Console.WriteLine("Given: " + value.FormatValue());

            return new Scenario<T>(value);
        }

        public static Scenario<T> Should<T>(this Scenario<T> scenario, Expression<Func<T, bool>> f)
        {
            var expr = f.Body;
            var s = string.Format("  + {0} should {1} [{2}]", expr.GetSubject(), expr.GetCond(), expr.GetValue());
            var success = f.Compile()(scenario.Value);

            if (!success) Assert.Fail(s + expr.Explain(scenario.Value));

            Console.WriteLine(s);

            return scenario;
        }

        public static Scenario<T> WhenAssign<T, TProperty>(this Scenario<T> scenario, Expression<Func<T, TProperty>> e, TProperty value)
        {
            var expr = (MemberExpression)e.Body;

            var property = (PropertyInfo)expr.Member;

            Console.WriteLine(" - When set [{0}] to [{1}]", property.Name, value.FormatValue());

            property.SetValue(scenario.Value, value, null);

            return scenario;
        }

        public static Scenario<T> When<T>(
            this Scenario<T> scenario,
            Expression<Func<T, T>> e)
        {
            var expr = e.Body;

            Console.WriteLine(" - When {0} [{1}]", expr.GetCond(), expr.GetValue());

            scenario.Value = e.Compile()(scenario.Value);

            return scenario;
        }


        public static Scenario<T> When<T>(
            this Scenario<T> scenario,
            Expression<Action<T>> e)
        {
            var expr = e.Body;

            Console.WriteLine(" - When {0} [{1}]", expr.GetCond(), expr.GetValue());

            e.Compile()(scenario.Value);

            return scenario;
        }
    }
}