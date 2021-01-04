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

    delegate (Type t, bool ignoreGenerics, bool directTypeOnly, Type[] types) UpdateArrayBuilder();

#if WinCopies3
    [TestClass]
    public
#else
    public partial
#endif
        class ExtensionsTests
    {
        [TestMethod]
        public void GetDirectInterfacesTests()
        {
            Type[] interfaces;

            ArrayBuilder<Type> arrayBuilder;

            void assertInterfaces()
            {
                Assert.AreEqual(
#if WinCopies3
(uint)
#endif
                    interfaces.Length, arrayBuilder.Count, $"Interfaces found: {((System.Collections.Generic.IEnumerable<Type>)arrayBuilder).ToString(true)}");

                int i = -1;

                foreach (
#if WinCopies3
Type t 
#else
                    LinkedListNode<Type> node
#endif
                    in arrayBuilder)

                    Assert.AreEqual(interfaces[++i].Name,
#if WinCopies3
                        t
#else
                        node.Value
#endif
                        .Name);
            }

            void updateArrayBuilderAndAssert(UpdateArrayBuilder parameters)
            {
                (Type t, bool ignoreGenerics, bool directTypeOnly, Type[] types) = parameters();

                interfaces = types;

                arrayBuilder = new ArrayBuilder<Type>(t.GetDirectInterfaces(ignoreGenerics, directTypeOnly));

                assertInterfaces();
            }



            updateArrayBuilderAndAssert(() => (typeof(A), false, false, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(A<int>), false, false, new Type[] { typeof(IA<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(B), false, false, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(B<int>), false, false, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(C), false, false, new Type[] { typeof(IB), typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(C<int>), false, false, new Type[] { typeof(IB<int>), typeof(IC), typeof(IC<int>) }));



            updateArrayBuilderAndAssert(() => (typeof(A), true, false, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(A<int>), true, false, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(B), true, false, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(B<int>), true, false, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(C), true, false, new Type[] { typeof(IB), typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(C<int>), true, false, new Type[] { typeof(IB), typeof(IC) }));



            updateArrayBuilderAndAssert(() => (typeof(A), false, true, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(A<int>), false, true, new Type[] { typeof(IA<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(B), false, true, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(B<int>), false, true, new Type[] { typeof(IB<int>) }));

            updateArrayBuilderAndAssert(() => (typeof(C), false, true, new Type[] { typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(C<int>), false, true, new Type[] { typeof(IC), typeof(IC<int>) }));



            updateArrayBuilderAndAssert(() => (typeof(A), true, true, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(A<int>), true, true, new Type[] { typeof(IA) }));

            updateArrayBuilderAndAssert(() => (typeof(B), true, true, new Type[] { typeof(IB) }));

            updateArrayBuilderAndAssert(() => (typeof(B<int>), true, true, Array.Empty<Type>()));

            updateArrayBuilderAndAssert(() => (typeof(C), true, true, new Type[] { typeof(IC) }));

            updateArrayBuilderAndAssert(() => (typeof(C<int>), true, true, new Type[] { typeof(IC) }));
        }
    }
}
