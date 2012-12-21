using System;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Zero.Specs
{
    public static class Formatter
    {
        public static string Ident(this string s, int space)
        {
            var sb = new StringBuilder();
            var sr = new StringReader(s);

            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                for (var i = 0; i < space; i++) sb.Append(' ');
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        public static string GetSubject(this Expression expr)
        {
            return expr.Switch(
                c => "It",
                b =>
                    {

                        if (b.Left is MemberExpression)
                        {
                            var mem = (MemberExpression)b.Left;
                            return "Its [" + mem.Member.Name + "]";
                        }
                        else
                        {
                            var param = (ParameterExpression)b.Left;
                            return param.Name;
                        }
                    });
        }

        public static object GetValue(this Expression expr)
        {
            if (expr is ConstantExpression)
            {
                return ((ConstantExpression)expr).Value;
            }
            if (expr is MemberExpression)
            {
                var m = (MemberExpression) expr;
                if (m.Member is FieldInfo)
                {
                    var fi = (FieldInfo) m.Member;
                    var v = m.Expression.GetValue();

                    return fi.GetValue(v);
                }

                return m.Expression.GetValue();
            }

            return expr.Switch(
                call => call.FormatArguments(),
                bin => bin.Right.GetValue());
        }

        public static string GetCond(this Expression expr)
        {
            if (expr is ConstantExpression)
            {
                return "assign ";
            }

            return expr.Switch(
                call => call.Method.Name,
                bin => bin.NodeType.ToString(),
                unary => unary.NodeType + " " + unary.Operand.GetCond()).
                ToNaturalLanguage();
        }

        public static string Explain(this Expression expr, object value)
        {
            if (expr is BinaryExpression)
            {
                var bin = (BinaryExpression)expr;
                var left = bin.Left;

                if (left is MemberExpression)
                {
                    var member = (MemberExpression)left;
                    var property = (PropertyInfo)member.Member;

                    return string.Format(" (actual: {0})", property.GetValue(value, null));
                }
                if (left is ParameterExpression)
                {
                    return string.Format(" (actual: {0})", value);
                }
            }
            return null;
        }

        public static string FormatArguments(this MethodCallExpression call)
        {
            var arguments = call.Arguments;
            var parameters = call.Method.GetParameters();
            var sb = new StringBuilder();

            if (arguments.Count > 1)
            {

                for (var i = 0; i < arguments.Count; i++)
                {
                    var arg = arguments[i];
                    var par = parameters[i];

                    sb.Append(par.Name);
                    sb.Append(":");
                    sb.Append(arg.ToString());
                    sb.Append(",");
                }

                sb.Remove(sb.Length - 1, 1);
            }
            else
            {
                sb.Append(arguments[0].ToString());
            }

            return sb.ToString();
        }

        static TR Switch<TR>(
            this Expression expr,
            Func<MethodCallExpression, TR> method,
            Func<BinaryExpression, TR> binary,
            Func<UnaryExpression, TR> unary = null)
        {
            if (expr is MethodCallExpression)
            {
                var call = (MethodCallExpression)expr;
                return method(call);
            }
            else if (expr is BinaryExpression)
            {
                var bin = (BinaryExpression)expr;
                return binary(bin);
            }
            else if (expr is UnaryExpression)
            {
                if (unary != null) return unary((UnaryExpression)expr);
                else
                {
                    var unaryExpr = (UnaryExpression)expr;
                    return unaryExpr.Operand.Switch(method, binary);
                }
            }
            throw new NotSupportedException(expr.GetType().ToString());
        }

        public static string ToNaturalLanguage(this string method)
        {
            var r = new StringBuilder();

            for (var i = 0; i < method.Length; i++)
            {
                var c = method[i];
                if (Char.IsUpper(c) && i > 0)
                {
                    r.Append(' ');
                }
                r.Append(Char.ToLower(c));
            }

            return r.ToString();
        }
    }
}