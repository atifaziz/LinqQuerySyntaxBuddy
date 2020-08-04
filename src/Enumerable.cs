#region Copyright 2020 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace LinqQuerySyntaxBuddy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Collections;

    public static class Enumerable
    {
        public static BinaryList<T> Seq<T>(T item) => new BinaryList<T>(item);

        public static bool Any<T>(IEnumerable<T> source) => source.Any();
        public static bool Any<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.Any(predicate);

        public static bool All<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.All(predicate);

        public static T First<T>(IEnumerable<T> source) => source.First();
        public static T First<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.First(predicate);
        public static T FirstOrDefault<T>(IEnumerable<T> source) => source.FirstOrDefault();
        public static T FirstOrDefault<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.FirstOrDefault(predicate);

        public static T Last<T>(IEnumerable<T> source) => source.Last();
        public static T Last<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.Last(predicate);
        public static T LastOrDefault<T>(IEnumerable<T> source) => source.LastOrDefault();
        public static T LastOrDefault<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.LastOrDefault(predicate);

        public static T Single<T>(IEnumerable<T> source) => source.Single();
        public static T Single<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.Single(predicate);
        public static T SingleOrDefault<T>(IEnumerable<T> source) => source.SingleOrDefault();
        public static T SingleOrDefault<T>(Func<T, bool> predicate, IEnumerable<T> source) => source.SingleOrDefault(predicate);

        public static T[] Array<T>(IEnumerable<T> source) => source.ToArray();

        public static List<T> List<T>(IEnumerable<T> source) => source.ToList();

        public static HashSet<T> HashSet<T>(IEnumerable<T> source) =>
            HashSet(null, source);

        public static HashSet<T> HashSet<T>(IEqualityComparer<T> comparer,
                                            IEnumerable<T> source) =>
            new HashSet<T>(source, comparer);

        public static Dictionary<TKey, TValue>
            Dictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> source) =>
            Dictionary(null, source);

        public static Dictionary<TKey, TValue>
            Dictionary<TKey, TValue>(IEqualityComparer<TKey> comparer,
                                     IEnumerable<KeyValuePair<TKey, TValue>> source) =>
            source.ToDictionary(e => e.Key, e => e.Value, comparer);
    }
}

namespace LinqQuerySyntaxBuddy.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// An immutable list that can represent either an empty list
    /// of <typeparamref name="T"/> or a list containing exactly
    /// one <typeparamref name="T"/>.
    /// </summary>

    public readonly struct BinaryList<T> : IList<T>, IReadOnlyList<T>
    {
        public static BinaryList<T> Empty = new BinaryList<T>();

        readonly bool _one;
        readonly T _item;

        public BinaryList(T item) { _one = true; _item = item; }

        public int Count => _one ? 1 : 0;
        public bool IsReadOnly => true;

        public T this[int index]
        {
            get => _one && index == 0 ? _item : throw new ArgumentOutOfRangeException();
            set => throw ReadOnlyException();
        }

        public int IndexOf(T item) => Contains(item) ? 0 : -1;
        public bool Contains(T item) => _one && EqualityComparer<T>.Default.Equals(_item, item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (arrayIndex + Count > array.Length) throw new ArgumentException(nameof(array));

            if (_one)
                array[arrayIndex] = _item;
        }

        public Enumerator GetEnumerator() => _one ? new Enumerator(_item) : Enumerator.Empty;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator : IEnumerator<T>
        {
            public static readonly Enumerator Empty = new Enumerator();

            enum State
            {
                EmptyInitialized,
                SingletonInitialized,
                SingletonIterated,
                Disposed
            }

            State _state;

            public Enumerator(T item) : this()
            {
                _state = State.SingletonInitialized;
                Current = item;
            }

            public bool MoveNext()
            {
                switch (_state)
                {
                    case State.SingletonInitialized:
                        _state = State.SingletonIterated;
                        return true;
                    default:
                        return false;
                }
            }

            public void Reset() =>
                _state = _state switch
                {
                    State.SingletonIterated => _state = State.SingletonInitialized,
                    var s => s,
                };

            public T Current { get; private set; }
            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _state = State.Disposed;
                Current = default;
            }
        }

        // Following methods are unsupported as this is a read-only list.

        void ICollection<T>.Add(T item)         => throw ReadOnlyException();
        void ICollection<T>.Clear()             => throw ReadOnlyException();
        bool ICollection<T>.Remove(T item)      => throw ReadOnlyException();
        void IList<T>.Insert(int index, T item) => throw ReadOnlyException();
        void IList<T>.RemoveAt(int index)       => throw ReadOnlyException();

        static NotSupportedException ReadOnlyException() =>
            new NotSupportedException("Single element list is immutable.");
    }
}
