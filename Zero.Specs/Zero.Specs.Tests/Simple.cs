using System.Collections.Generic;
using NUnit.Framework;

namespace Zero.Specs.Tests
{
    [TestFixture]
    public class Simple
    {
        [Test]
        public void Number()
        {
            Specs.
                Given(10).
                When(it => it + 3).
                Should(it => it == 13);
        }

        [Test]
        public void Remove_from_list()
        {
            Specs.
                Given(new List<int> {1, 2, 3, 4}).
                When(it => it.Remove(3)).
                Should(it => !it.Contains(3)).
                Should(it => it.Count == 3).
                Should(it => it.Contains(4));
        }
    }
}