using System.Collections.Concurrent;
using System.Text;

namespace GooseHtml
{
    public sealed class StringBuilderPool
    {
        private readonly ConcurrentBag<StringBuilder> _pool = [];
        private const int MaxCapacity = 1024 * 4; // cap for reused builders

        public static readonly StringBuilderPool Shared = new();

        private StringBuilderPool() { }

        public StringBuilder Rent()
        {
            if (_pool.TryTake(out var sb))
            {
                return sb;
            }

            return new StringBuilder();
        }

        public void Return(StringBuilder sb)
        {
            if (sb.Capacity > MaxCapacity) return;

            sb.Clear();
            _pool.Add(sb);
        }

        public string GetStringAndReturn(StringBuilder sb)
        {
            var result = sb.ToString();
            Return(sb);
            return result;
        }
    }
}