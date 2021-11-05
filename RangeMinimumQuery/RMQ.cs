using System;
using System.Collections.Generic;
using System.Linq;

namespace RangeMinimumQuery
{
    public class RMQ<T>
    {
        private T[] _nodes;
        private int _itemsCount;
        public IComparer<T> Comparer { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">values for building tree</param>
        public RMQ(IEnumerable<T> values)
            : this(values, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">values for building tree</param>
        /// <param name="comparer">comparer</param>
        public RMQ(IEnumerable<T> values, Comparer<T> comparer)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }
            if (!values.Any())
            {
                throw new ArgumentException($"{nameof(values)} can not be empty");
            }
            Comparer = comparer;
            BuildTree(values.ToArray());
        }

        private void BuildTree(T[] values)
        {
            _itemsCount = values.Length;
            var height = (int)Math.Ceiling(Math.Log2(_itemsCount));

            var maxItemsCount = (1 << (height + 1)) - 1;
            _nodes = new T[maxItemsCount];

            _ = BuildTree(values, 0, _itemsCount - 1, 0);
        }

        private T BuildTree(T[] values, int start, int end, int index)
        {
            if (start == end)
            {
                return _nodes[index] = values[start];
            }

            var mid = start + end >> 1;
            _nodes[index] = Min(
                BuildTree(values, start, mid, (index << 1) + 1),
                BuildTree(values, mid + 1, end, (index << 1) + 2));

            return _nodes[index];
        }

        private T Min(T left, T right)
        {
            return Comparer.Compare(left, right) < 0 ? left : right;
        }

        /// <summary>
        /// Returns minimum item in [start, end)
        /// </summary>
        /// <param name="start">start of the range (inclusive)</param>
        /// <param name="end">end of the range (exclusive)</param>
        public T Query(int start, int end)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(start)} can not be negative");
            }
            if (end > _itemsCount)
            {
                throw new ArgumentOutOfRangeException($"{nameof(end)} can not exceed the values count");
            }
            if (start >= end)
            {
                throw new ArgumentOutOfRangeException($"{nameof(start)} can not be equal or greater than {nameof(end)}");
            }

            return _nodes[Query(0, _itemsCount - 1, start, end - 1, 0)];
        }

        private int Query(int left, int right, int start, int end, int index)
        {
            if (start <= left && end >= right)
            {
                return index;
            }

            if (right < start || left > end)
            {
                return -1;
            }

            var mid = left + right >> 1;
            var leftRangeIndex = Query(left, mid, start, end, (index << 1) + 1);
            var rightRangeIndex = Query(mid + 1, right, start, end, (index << 1) + 2);
            switch (leftRangeIndex)
            {
                case >= 0 when rightRangeIndex >= 0:
                    if (Comparer.Compare(_nodes[leftRangeIndex], _nodes[rightRangeIndex]) < 0)
                    {
                        return leftRangeIndex;
                    }
                    return rightRangeIndex;
                case >= 0:
                    return leftRangeIndex;
                default:
                    return rightRangeIndex;
            }
        }
    }
}
