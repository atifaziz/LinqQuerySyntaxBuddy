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

namespace LinqQuerySyntaxBuddy.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Collections;
    using static Enumerable;

    public class TestBinarySequence
    {
        public class Empty
        {
            static readonly BinaryList<int> Sample = BinaryList<int>.Empty;

            [Test]
            public void Count()
            {
                Assert.That(Sample.Count, Is.Zero);
            }

            [Test]
            public void IsReadOnly()
            {
                Assert.That(Sample.IsReadOnly, Is.True);
            }

            [TestCase(-1)]
            [TestCase(0)]
            [TestCase(1)]
            public void Index(int i)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => _ = Sample[i]);
            }

            [TestCase(0)]
            public void IndexOf(int x)
            {
                Assert.That(Sample.IndexOf(x), Is.EqualTo(-1));
            }

            [TestCase(0)]
            public void Contains(int x)
            {
                Assert.That(Sample.Contains(x), Is.False);
            }

            [TestCase(0, 0)]
            [TestCase(1, 0)]
            [TestCase(1, 1)]
            public void CopyTo(int length, int index)
            {
                Sample.CopyTo(new int[length], index);
            }

            [Test]
            public void Iteration()
            {
                foreach (var _ in Sample)
                    Assert.Fail("Empty sequence must not yield any elements.");
            }

            [Test]
            public void GenericIteration()
            {
                using var e = Sample.AsEnumerable().GetEnumerator();
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.MoveNext(), Is.False);
                e.Reset();
                Assert.That(e.MoveNext(), Is.False);
            }

            [Test]
            public void NonGenericIteration()
            {
                var e = ((IEnumerable)Sample).GetEnumerator();
                using var _ = (IDisposable)e;
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.MoveNext(), Is.False);
                e.Reset();
                Assert.That(e.MoveNext(), Is.False);
            }
        }

        public class Singleton
        {
            static readonly BinaryList<int> Sample = Seq(42);

            [Test]
            public void Count()
            {
                Assert.That(Sample.Count, Is.EqualTo(1));
            }

            [Test]
            public void IsReadOnly()
            {
                Assert.That(Sample.IsReadOnly, Is.True);
            }

            [Test]
            public void IndexZero()
            {
                Assert.That(Sample[0], Is.EqualTo(42));
            }

            [TestCase(-1)]
            [TestCase(1)]
            public void NonZeroIndex(int i)
            {
                Assert.That(i, Is.Not.Zero);
                Assert.Throws<ArgumentOutOfRangeException>(() => _ = Sample[i]);
            }

            [TestCase(0, -1)]
            [TestCase(42, 0)]
            [TestCase(24, -1)]
            public void IndexOf(int x, int expected)
            {
                Assert.That(Sample.IndexOf(x), Is.EqualTo(expected));
            }

            [TestCase(0, false)]
            [TestCase(42, true)]
            [TestCase(24, false)]
            public void Contains(int x, bool expected)
            {
                Assert.That(Sample.Contains(x), Is.EqualTo(expected));
            }

            [TestCase(1, 0)]
            [TestCase(2, 0)]
            [TestCase(2, 1)]
            [TestCase(3, 0)]
            [TestCase(3, 1)]
            [TestCase(3, 2)]
            public void CopyTo(int length, int index)
            {
                var array = new int[length];
                Sample.CopyTo(array, index);

                Assert.That(array[index], Is.EqualTo(42));
                Assert.That(from i in Enumerable.Range(0, array.Length)
                            where i != index
                            select array[i],
                            Is.EqualTo(Enumerable.Repeat(0, length - 1)));
            }

            [Test]
            public void Iteration()
            {
                // ReSharper disable once SuggestVarOrType_SimpleTypes
                using BinaryList<int>.Enumerator e = Sample.GetEnumerator();
                Assert.That(e.MoveNext(), Is.True);
                Assert.That(e.Current, Is.EqualTo(42));
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.MoveNext(), Is.False);
            }

            [Test]
            public void Reset()
            {
                // ReSharper disable once SuggestVarOrType_SimpleTypes
                using BinaryList<int>.Enumerator e = Sample.GetEnumerator();
                for (var i = 0; i < 2; i++)
                {
                    Assert.That(e.MoveNext(), Is.True);
                    Assert.That(e.Current, Is.EqualTo(42));
                    Assert.That(e.MoveNext(), Is.False);
                    Assert.That(e.MoveNext(), Is.False);
                    e.Reset();
                }
            }

            [Test]
            public void GenericIteration()
            {
                using var e = Sample.AsEnumerable().GetEnumerator();
                Assert.That(e.MoveNext(), Is.True);
                Assert.That(e.Current, Is.EqualTo(42));
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.MoveNext(), Is.False);
            }

            [Test]
            public void NonGenericIteration()
            {
                var e = ((IEnumerable)Sample).GetEnumerator();
                using var _ = (IDisposable)e;
                Assert.That(e.MoveNext(), Is.True);
                Assert.That(e.Current, Is.EqualTo(42));
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.MoveNext(), Is.False);
            }

            [Test]
            public void Dispose()
            {
                var e = Sample.GetEnumerator();
                e.Dispose();
                e.Dispose(); // idempotent
            }

            [Test]
            public void IterationYieldsNothingIfEnumeratorIsDisposed()
            {
                var e = Sample.GetEnumerator();
                e.Dispose();
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.Current, Is.Zero);
            }

            [Test]
            public void IterationYieldsNothingIfDisposedEnumeratorIsReset()
            {
                var e = Sample.GetEnumerator();
                e.Dispose();
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.Current, Is.Zero);
                e.Reset();
                Assert.That(e.MoveNext(), Is.False);
                Assert.That(e.Current, Is.Zero);
            }
        }

        [TestCase(null)]
        [TestCase(42)]
        public void Unsupported(int? x)
        {
            var xs = x is {} n ? Seq(n) : BinaryList<int>.Empty;
            ICollection<int> collection = xs;
            IList<int> list = xs;

            Assert.Throws<NotSupportedException>(() => xs[0] = 1);
            Assert.Throws<NotSupportedException>(() => collection.Add(42));
            Assert.Throws<NotSupportedException>(() => collection.Remove(42));
            Assert.Throws<NotSupportedException>(() => collection.Clear());
            Assert.Throws<NotSupportedException>(() => list.Insert(0, 42));
            Assert.Throws<NotSupportedException>(() => list.RemoveAt(0));
        }
    }
}
