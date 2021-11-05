using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace RangeMinimumQuery.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class RMQTests
    {
        [Fact]
        public void Create_with_null_values_throws_ArgumentNullException()
        {
            var exception = Record.Exception(() =>
            {
                _ = new RMQ<int>(null);
            });

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Create_with_empty_values_throws_ArgumentException()
        {
            var values = Array.Empty<int>();
            var exception = Record.Exception(() =>
            {
                _ = new RMQ<int>(values);
            });

            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Query_on_negative_starting_range_throws_ArgumentOutOfRangeException()
        {
            var values = new int[] { 1, 2 };
            var exception = Record.Exception(() =>
            {
                var rmq = new RMQ<int>(values);
                _ = rmq.Query(-1, default);
            });

            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'start can not be negative')", exception.Message);
        }

        [Fact]
        public void Query_on_greater_ending_range_than_items_total_count_throws_ArgumentOutOfRangeException()
        {
            var values = new int[] { 1, 2 };
            var exception = Record.Exception(() =>
            {
                var rmq = new RMQ<int>(values);
                _ = rmq.Query(default, values.Length + 1);
            });

            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'end can not exceed the values count')", exception.Message);
        }

        [Fact]
        public void Query_on_range_with_reverse_parameters_throws_ArgumentOutOfRangeException()
        {
            var values = new int[] { 1, 2 };
            var exception = Record.Exception(() =>
            {
                var rmq = new RMQ<int>(values);
                _ = rmq.Query(2, 0);
            });

            Assert.NotNull(exception);
            Assert.IsType<ArgumentOutOfRangeException>(exception);
            Assert.Equal("Specified argument was out of the range of valid values. (Parameter 'start can not be equal or greater than end')", exception.Message);
        }

        [Theory]
        [InlineData(0, 0, 10)]
        [InlineData(1, 0, 9)]
        [InlineData(1, 0, 8)]
        [InlineData(1, 0, 7)]
        [InlineData(1, 0, 6)]
        [InlineData(1, 0, 5)]
        [InlineData(1, 0, 4)]
        [InlineData(2, 0, 3)]
        [InlineData(3, 0, 2)]
        [InlineData(5, 0, 1)]
        [InlineData(4, 4, 8)]
        [InlineData(1, 3, 4)]
        [InlineData(6, 6, 9)]
        public void Query_returns_expected_value_using_default_comparer(int expected, int start, int end)
        {
            var values = new int[] { 5, 3, 2, 1, 9, 4, 8, 7, 6, 0 };
            var rmq = new RMQ<int>(values);

            Assert.Equal(expected, rmq.Query(start, end));
        }

        [Theory]
        [InlineData(9, 0, 10)]
        [InlineData(9, 0, 9)]
        [InlineData(9, 0, 8)]
        [InlineData(9, 0, 7)]
        [InlineData(9, 0, 6)]
        [InlineData(9, 0, 5)]
        [InlineData(5, 0, 4)]
        [InlineData(5, 0, 3)]
        [InlineData(5, 0, 2)]
        [InlineData(5, 0, 1)]
        [InlineData(9, 4, 8)]
        [InlineData(1, 3, 4)]
        [InlineData(8, 6, 9)]
        public void Query_returns_expected_value_using_custom_comparer(int expected, int start, int end)
        {
            var values = new int[] { 5, 3, 2, 1, 9, 4, 8, 7, 6, 0 };
            var rmq = new RMQ<int>(values, new ReverseIntegerComparer());

            Assert.Equal(expected, rmq.Query(start, end));
        }

        private class ReverseIntegerComparer : Comparer<int>
        {
            public override int Compare(int x, int y)
            {
                return y - x;
            }
        }
    }
}
