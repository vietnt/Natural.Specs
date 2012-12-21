using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Zero.Specs.Tests
{
    [TestFixture]
    public class Account_Specs
    {
        [Test]
        public void Add_balance_to_account()
        {
            Specs.
                Given(new Account("John")).
                When(it => it.AddBalance(3000)).
                Should(it => it.Balance == 3000);
        }

        [Test]
        public void Pay()
        {
            Specs.
                Given(new Account("John")).
                When(it => it.AddBalance(3000)).
                When(it => it.Pay(1000)).
                Should(it => it.Balance == 2000);
        }

        [Test]
        public void Change_name()
        {
            Specs.Given(new Account("John")).
                WhenAssign(it => it.Name, "Smith").
                Should(it => it.Name == "Smith");
        }
    }
}
