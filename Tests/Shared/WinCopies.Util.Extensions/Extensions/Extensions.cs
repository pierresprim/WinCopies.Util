/* Copyright © Pierre Sprimont, 2020
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using WinCopies.Collections;
using WinCopies.Collections.Generic;
using WinCopies.Extensions;

namespace WinCopies.Util.Tests
{
    public interface IA { }

    public interface IA<T> : IA { }

    public interface IB : IA { }

    public interface IB<T> : IB, IA<T> { }

    public class A : IA { }

    public class A<T> : IA<T> { }

    public class B : A, IB { }

    public class B<T> : B, IB<T> { }

    public interface IC { }

    public interface IC<T> { }

    public class C : B, IC { }

    public class C<T> : B<T>, IC, IC<T> { }

    public class D : IA, IB { }

    public class D<T> : IA<T>, IB<T> { }

    public class E : D { }

    public class E<T> : D<T> { }

    public class F : C { }

    public class F<T> : C<T> { }

    public class G { }

    public class G<T> { }

    delegate (Type t, bool ignoreGenerics, bool directTypeOnly, Type[] types) UpdateArrayBuilder();

    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void GetDirectInterfacesTests()
        {
            Type[] interfaces;

            ArrayBuilder<Type> arrayBuilder;

            void assertInterfaces()
            {
                Assert.AreEqual((uint)interfaces.Length, arrayBuilder.Count, $"Interfaces found: {arrayBuilder.ToString(true)}");

                int i = -1;

                foreach (Type t in arrayBuilder.AsFromType<IEnumerable<Type>>())

                    Assert.AreEqual(interfaces[++i].Name, t.Name);
            }

            void updateArrayBuilderAndAssert(UpdateArrayBuilder parameters)
            {
                (Type t, bool ignoreGenerics, bool directTypeOnly, Type[] types) = parameters();

                interfaces = types;

                arrayBuilder = new ArrayBuilder<Type>(t.GetDirectInterfaces(ignoreGenerics, directTypeOnly, doNotExclude: true));

                assertInterfaces();
            }

            // TODO: add tests for all booleans

            updateArrayBuilderAndAssert(() => (typeof(A), false, false, new Type[] { typeof(IA) }));
            updateArrayBuilderAndAssert(() => (typeof(A<int>), false, false, new Type[] { typeof(IA<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(B), false, false, new Type[] { typeof(IA), typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(B<int>), false, false, new Type[] { typeof(IA), typeof(IB), typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(C), false, false, new Type[] { typeof(IA), typeof(IB), typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(C<int>), false, false, new Type[] { typeof(IA), typeof(IB), typeof(IB<int>), typeof(IC), typeof(IC<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(D), false, false, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(D<int>), false, false, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(E), false, false, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(E<int>), false, false, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(F), false, false, new Type[] { typeof(IA), typeof(IB), typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(F<int>), false, false, new Type[] { typeof(IA), typeof(IB), typeof(IB<int>), typeof(IC), typeof(IC<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(G), false, false, System.Array.Empty<Type>()));
            updateArrayBuilderAndAssert(() => (typeof(G<int>), false, false, System.Array.Empty<Type>()));



            updateArrayBuilderAndAssert(() => (typeof(A), true, false, new Type[] { typeof(IA) }));
            updateArrayBuilderAndAssert(() => (typeof(A<int>), true, false, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(B), true, false, new Type[] { typeof(IA), typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(B<int>), true, false, new Type[] { typeof(IA), typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(C), true, false, new Type[] { typeof(IA), typeof(IB), typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(C<int>), true, false, new Type[] { typeof(IA), typeof(IB), typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(D), true, false, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(D<int>), true, false, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(E), true, false, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(E<int>), true, false, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(F), true, false, new Type[] { typeof(IA), typeof(IB), typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(F<int>), true, false, new Type[] { typeof(IA), typeof(IB), typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(G), true, false, System.Array.Empty<Type>()));
            updateArrayBuilderAndAssert(() => (typeof(G<int>), true, false, System.Array.Empty<Type>()));



            updateArrayBuilderAndAssert(() => (typeof(A), false, true, new Type[] { typeof(IA) }));
            updateArrayBuilderAndAssert(() => (typeof(A<int>), false, true, new Type[] { typeof(IA<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(B), false, true, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(B<int>), false, true, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(C), false, true, new Type[] { typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(C<int>), false, true, new Type[] { typeof(IC), typeof(IC<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(D), false, true, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(D<int>), false, true, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(E), false, true, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(E<int>), false, true, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(F), false, true, new Type[] { typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(F<int>), false, true, new Type[] { typeof(IC), typeof(IC<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(G), false, true, System.Array.Empty<Type>()));
            updateArrayBuilderAndAssert(() => (typeof(G<int>), false, true, System.Array.Empty<Type>()));



            updateArrayBuilderAndAssert(() => (typeof(A), true, true, new Type[] { typeof(IA) }));
            updateArrayBuilderAndAssert(() => (typeof(A<int>), true, true, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(B), true, true, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(B<int>), true, true, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(C), true, true, new Type[] { typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(C<int>), true, true, new Type[] { typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(D), true, true, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(D<int>), true, true, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(E), true, true, new Type[] { typeof(IB) }));
            updateArrayBuilderAndAssert(() => (typeof(E<int>), true, true, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(F), true, true, new Type[] { typeof(IC) }));
            updateArrayBuilderAndAssert(() => (typeof(F<int>), true, true, new Type[] { typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(G), true, true, System.Array.Empty<Type>()));
            updateArrayBuilderAndAssert(() => (typeof(G<int>), true, true, System.Array.Empty<Type>()));
        }
    }
}
