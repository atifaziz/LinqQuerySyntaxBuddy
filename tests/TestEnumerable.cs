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
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Collections;
    using static Enumerable;

    public class EnumerableTests
    {
        public class Any
        {
            [Test]
            public void ReturnsTrueForNonEmptySequence()
            {
                Assert.That(Any(new int[1]), Is.True);
            }

            [Test]
            public void PredicatedReturnsTrueWhenAnyElementMatches()
            {
                var r = Any(x => x > 5,
                            from x in Enumerable.Range(1, 10)
                            where x % 2 == 0
                            select x);

                Assert.That(r, Is.True);
            }

            [Test]
            public void ReturnsFalseForEmptySequence()
            {
                Assert.That(Any(new int[0]), Is.False);
            }

            [Test]
            public void PredicatedReturnsFalseWhenNoneMatch()
            {
                var r = Any(x => x % 2 == 1,
                            from x in Enumerable.Range(1, 10)
                            where x % 2 == 0
                            select x);

                Assert.That(r, Is.False);
            }
        }

        public class All
        {
            [Test]
            public void ReturnsTrueWhenAllElementMatch()
            {
                var r = All(x => x % 2 == 0,
                            from x in Enumerable.Range(1, 10)
                            where x % 2 == 0
                            select x);

                Assert.That(r, Is.True);
            }

            [Test]
            public void ReturnsFalseWhenAnyOneElementMismatches()
            {
                var r = All(x => x % 2 == 1,
                            from x in Enumerable.Range(1, 10)
                            where x % 2 == 0
                            select x);

                Assert.That(r, Is.False);
            }
        }

        public class First
        {
            [Test]
            public void ReturnsFirstElement()
            {
                var r = First(from x in Enumerable.Range(1, 10)
                              where x % 2 == 0
                              select x);

                Assert.That(r, Is.EqualTo(2));
            }

            [Test]
            public void ThrowsForEmptySequence()
            {
                Assert.Throws<InvalidOperationException>(() =>
                    First(Enumerable.Empty<int>()));
            }

            [Test]
            public void PredicatedReturnsFirstMatchingElement()
            {
                var r = First(x => x > 5,
                              from x in Enumerable.Range(1, 10)
                              where x % 2 == 0
                              select x);

                Assert.That(r, Is.EqualTo(6));
            }

            [Test]
            public void PredicatedThrowsWhenNoneMatching()
            {
                Assert.Throws<InvalidOperationException>(() =>
                    First(x => x % 2 == 1,
                          from x in Enumerable.Range(1, 10)
                          where x % 2 == 0
                          select x));
            }
        }

        public class FirstOrDefault
        {
            [Test]
            public void ReturnsFirstElement()
            {
                var r = FirstOrDefault(from x in Enumerable.Range(1, 10)
                                       where x % 2 == 0
                                       select x);

                Assert.That(r, Is.EqualTo(2));
            }

            [Test]
            public void ReturnsDefaultForEmptySequence()
            {
                var r = FirstOrDefault(from x in Enumerable.Range(1, 10)
                                       where x > 10
                                       select x);

                Assert.That(r, Is.Zero);
            }

            [Test]
            public void PredicatedReturnsDefaultWhenNoneMatch()
            {
                var r = FirstOrDefault(x => x % 2 == 1,
                                       from x in Enumerable.Range(1, 10)
                                       where x % 2 == 0
                                       select x);

                Assert.That(r, Is.Zero);
            }
        }

        public class Last
        {
            [Test]
            public void ReturnsLastElement()
            {
                var r = Last(from x in Enumerable.Range(1, 10)
                             where x % 2 == 0
                             select x);

                Assert.That(r, Is.EqualTo(10));
            }

            [Test]
            public void ThrowsForEmptySequence()
            {
                Assert.Throws<InvalidOperationException>(() =>
                    Last(Enumerable.Empty<int>()));
            }

            [Test]
            public void PredicatedReturnsLastMatchingElement()
            {
                var r = Last(x => x < 6,
                             from x in Enumerable.Range(1, 10)
                             where x % 2 == 0
                             select x);

                Assert.That(r, Is.EqualTo(4));
            }

            [Test]
            public void PredicatedThrowsWhenNoneMatching()
            {
                Assert.Throws<InvalidOperationException>(() =>
                    Last(x => x % 2 == 1,
                         from x in Enumerable.Range(1, 10)
                         where x % 2 == 0
                         select x));
            }
        }

        public class LastOrDefault
        {
            [Test]
            public void ReturnsLastElement()
            {
                var r = LastOrDefault(from x in Enumerable.Range(1, 10)
                                      where x % 2 == 0
                                      select x);

                Assert.That(r, Is.EqualTo(10));
            }

            [Test]
            public void ReturnsDefaultForEmptySequence()
            {
                var r = LastOrDefault(from x in Enumerable.Range(1, 10)
                                      where x > 10
                                      select x);

                Assert.That(r, Is.Zero);
            }

            [Test]
            public void PredicatedReturnsDefaultWhenNoneMatch()
            {
                var r = LastOrDefault(x => x % 2 == 1,
                                      from x in Enumerable.Range(1, 10)
                                      where x % 2 == 0
                                      select x);

                Assert.That(r, Is.Zero);
            }
        }

        public class Single
        {
            [Test]
            public void ReturnsSingleElement()
            {
                var r = Single(from x in Enumerable.Range(1, 10)
                               where x >= 10
                               select x);

                Assert.That(r, Is.EqualTo(10));
            }

            [Test]
            public void ThrowsForEmptySequence()
            {
                Assert.Throws<InvalidOperationException>(() =>
                    Single(Enumerable.Empty<int>()));
            }

            [Test]
            public void PredicatedReturnsSingleMatchingElement()
            {
                var r = Single(x => x >= 10,
                               from x in Enumerable.Range(1, 10)
                               where x % 2 == 0
                               select x);

                Assert.That(r, Is.EqualTo(10));
            }
        }

        public class SingleOrDefault
        {
            [Test]
            public void ReturnsSingleElement()
            {
                var r = SingleOrDefault(from x in Enumerable.Range(1, 10)
                                        where x >= 10
                                        select x);

                Assert.That(r, Is.EqualTo(10));
            }

            [Test]
            public void ReturnsDefaultWhenZeroElements()
            {
                var r = SingleOrDefault(from x in Enumerable.Range(1, 10)
                                        where x > 10
                                        select x);

                Assert.That(r, Is.Zero);
            }

            [Test]
            public void PredicatedReturnsSingleMatchingElement()
            {
                var r = SingleOrDefault(x => x >= 10,
                                        from x in Enumerable.Range(1, 10)
                                        where x % 2 == 0
                                        select x);

                Assert.That(r, Is.EqualTo(10));
            }

            [Test]
            public void PredicatedReturnsDefaultWhenZeroMatch()
            {
                var r = SingleOrDefault(x => x % 2 == 1,
                                        from x in Enumerable.Range(1, 10)
                                        where x % 2 == 0
                                        select x);

                Assert.That(r, Is.Zero);
            }
        }

        [Test]
        public void ArrayConversion()
        {
            // ReSharper disable once SuggestVarOrType_Elsewhere
            int[] array = Array(from x in Enumerable.Range(1, 10)
                                where x % 2 == 0
                                select x);

            Assert.That(array, Is.EqualTo(new[] { 2, 4, 6, 8, 10 }));
        }

        [Test]
        public void ListConversion()
        {
            // ReSharper disable once SuggestVarOrType_Elsewhere
            List<int> list = List(from x in Enumerable.Range(1, 10)
                                  where x % 2 == 0
                                  select x);

            Assert.That(list, Is.EqualTo(new[] { 2, 4, 6, 8, 10 }));
        }

        [Test]
        public void DictionaryConversion()
        {
            // ReSharper disable once SuggestVarOrType_Elsewhere
            Dictionary<int, int> dict =
                Dictionary(from x in Enumerable.Range(1, 10)
                           select KeyValuePair.Create(x, x * 2));

            Assert.That(dict, Is.EqualTo(new Dictionary<int, int>
            {
                { 1, 2 },
                { 2, 4 },
                { 3, 6 },
                { 4, 8 },
                { 5, 10 },
                { 6, 12 },
                { 7, 14 },
                { 8, 16 },
                { 9, 18 },
                { 10, 20 },
            }));
        }

        [Test]
        public void DictionaryConversionWithKeyComparer()
        {
            // ReSharper disable once SuggestVarOrType_Elsewhere
            Dictionary<string, string> dict =
                Dictionary(StringComparer.OrdinalIgnoreCase,
                           from s in new[] { "foo", "bar", "baz" }
                           select KeyValuePair.Create(s, s + "!"));

            Assert.That(dict.Count, Is.EqualTo(3));
            Assert.That(dict["FOO"], Is.EqualTo("foo!"));
            Assert.That(dict["BAR"], Is.EqualTo("bar!"));
            Assert.That(dict["BAZ"], Is.EqualTo("baz!"));
        }

        [Test]
        public void HashSetConversion()
        {
            // ReSharper disable once SuggestVarOrType_Elsewhere
            HashSet<int> set = HashSet(from x in Enumerable.Range(1, 10)
                                       from y in Enumerable.Range(1, x)
                                       select x + y
                                       into x
                                       orderby x descending
                                       select x);

            Assert.That(set, Is.EquivalentTo(Enumerable.Range(2, 19)));
        }

        [Test]
        public void HashSetConversionWithComparer()
        {
            var words = new[] { "foo", "bar", "baz" };

            // ReSharper disable once SuggestVarOrType_Elsewhere
            HashSet<string> set =
                HashSet(StringComparer.OrdinalIgnoreCase,
                        from n in Enumerable.Range(1, 10)
                        from s in words
                        select n % 2 == 0 ? s : s.ToUpperInvariant()
                        into s
                        orderby s descending
                        select s);

            Assert.That(set, Is.EquivalentTo(words).IgnoreCase);
        }

        [Test]
        public void Singleton()
        {
            // ReSharper disable once SuggestVarOrType_Elsewhere
            BinaryList<int> seq = Seq(42);

            Assert.That(seq, Is.EqualTo(new[] { 42 }));
            Assert.That(seq.Count, Is.EqualTo(1));
            Assert.That(seq[0], Is.EqualTo(42));
        }
    }
}
