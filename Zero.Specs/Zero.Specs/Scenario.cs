using System.Collections.Generic;
using System.Linq;

namespace Zero.Specs
{
    public class Scenario<T>
    {
        public T Value { get; set; }

        public Scenario(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("Given: " + Value.FormatValue());
        }
    }
}
