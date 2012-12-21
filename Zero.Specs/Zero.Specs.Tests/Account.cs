using System;

namespace Zero.Specs.Tests
{
    public class Account
    {
        int _balance;
        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Account(string name)
        {
            _name = name;
        }

        public int Balance
        {
            get { return _balance; }
        }

        public void AddBalance(int amount)
        {
            _balance += amount;
        }

        public void Pay(int amount)
        {
            _balance -= amount;
        }

        public bool CanPay(int amount)
        {
            return _balance >= amount;
        }

        public override string ToString()
        {
            return "{" + _name + "}";
        }
    }
}