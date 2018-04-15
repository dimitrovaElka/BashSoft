using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Contracts
{
    public interface ISimpleOrderedBag<T> : IEnumerable<T> where T : IComparable<T>
    {
        int Size { get; }

        int Capacity { get; }

        void Add(T element);

        void AddAll(ICollection<T> collection);

        string JoinWith(string joiner);

        bool Remove(T element);
    }
}
