/* Copyright © Pierre Sprimont, 2021
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

using System;
using System.Reflection;

namespace WinCopies
#if !WinCopies3
.Util
#endif
{
    public struct ConversionStruct<TIn, TOut>
    {
        public Converter<TIn, TOut> Converter { get; }

        public Converter<TOut, TIn> BackConverter { get; }

        public ConversionStruct(in Converter<TIn, TOut> converter, Converter<TOut, TIn> backConverter)
        {
            Converter = converter;

            BackConverter = backConverter;
        }
    }

    public class Conversion<TIn, TOut>
    {
        public Converter<TIn, TOut> Converter { get; }

        public Converter<TOut, TIn> BackConverter { get; }

        public Conversion(in Converter<TIn, TOut> converter, Converter<TOut, TIn> backConverter)
        {
            Converter = converter;

            BackConverter = backConverter;
        }
    }

#if !WinCopies3
    /// <summary>
    /// This delegate represents the action that is performed for each iteration of a loop.
    /// </summary>
    /// <param name="obj">The object or value retrieved by the current iteration.</param>
    /// <returns><see langword="true"/> to break the loop; otherwise <see langword="false"/>.</returns>
    public delegate bool LoopIteration(object obj);

    /// <summary>
    /// This delegate represents the action that is performed for each iteration of a loop.
    /// </summary>
    /// <typeparam name="T">The type of the object or value that is retrieved.</typeparam>
    /// <param name="obj">The object or value retrieved by the current iteration.</param>
    /// <returns><see langword="true"/> to break the loop; otherwise <see langword="false"/>.</returns>
    public delegate bool LoopIteration<T>(T obj);
#endif

    public delegate
#if CS7
        (
#else
        ValueTuple<
#endif
        bool
#if CS7
            result
#endif
            , Exception
#if CS7
            ex)
#else
            >
#endif
            FieldValidateValueCallback(object obj, object value, FieldInfo field, string paramName);

    public delegate void FieldValueChangedCallback(object obj, object value, FieldInfo field, string paramName);

    public delegate
#if CS7
        (
#else
        ValueTuple<
#endif
        bool
#if CS7
            result
#endif
            , Exception
#if CS7
            ex)
#else
            >
#endif
       PropertyValidateValueCallback(object obj, object value, PropertyInfo property, string paramName);

    public delegate void PropertyValueChangedCallback(object obj, object value, PropertyInfo property, string paramName);

    /// <summary>
    /// Delegate for a non-generic predicate.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <returns><see langword="true"/> if the predicate success, otherwise <see langword="false"/>.</returns>
    public delegate bool Predicate(object value);

    public delegate T Converter<
#if WinCopies3
        out
#endif
        T>(object obj);

    public delegate TOut ConverterIn<TIn, out TOut>(in TIn value);

    public delegate void EventHandler<T>(T sender, EventArgs e);

    public delegate void EventHandler<TSender, TEventArgs>(TSender sender, TEventArgs e);



    public delegate void ActionParams(params object[] args);

    public delegate void ActionParams<in T>(params T[] args);

    /// <summary>
    /// Represents a delegate that returns an object.
    /// </summary>
    /// <returns>Any object.</returns>
    public delegate object Func();

    public delegate object FuncParams(params object[] args);

    public delegate TOut FuncParams<in TParams, out TOut>(params TParams[] args);

    public delegate T FuncIn<T>(in T param);
    public delegate TOut FuncIn<T1, out TOut>(in T1 p1);
    public delegate TOut FuncIn<T1, T2, out TOut>(in T1 p1, in T2 p2);
    public delegate TOut FuncIn<T1, T2, T3, out TOut>(in T1 p1, in T2 p2, in T3 p3);
    public delegate TOut FuncIn<T1, T2, T3, T4, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15);
    public delegate TOut FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15, in T16 p16);

    public delegate TOut FuncIn2<in T1, T2, out TOut>(T1 value1, in T2 value2);
    public delegate TOut FuncIn2<in T1, T2, T3, out TOut>(T1 value1, in T2 value2, in T3 value3);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10, in T11 value11);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10, in T11 value11, in T12 value12);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10, in T11 value11, in T12 value12, in T13 value13);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10, in T11 value11, in T12 value12, in T13 value13, in T14 value14);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10, in T11 value11, in T12 value12, in T13 value13, in T14 value14, in T15 value15);
    public delegate TOut FuncIn2<in T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, out TOut>(T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7, in T8 value8, in T9 value9, in T10 value10, in T11 value11, in T12 value12, in T13 value13, in T14 value14, in T15 value15, in T16 value16);

    public delegate TOut FuncOut<TParam, out TOut>(out TParam param);
    public delegate TOut FuncOut<in T1, T2, out TOut>(T1 p1, out T2 p2);
    public delegate TOut FuncOut<in T1, in T2, T3, out TOut>(T1 p1, T2 p2, out T3 p3);
    public delegate TOut FuncOut<in T1, in T2, in T3, T4, out TOut>(T1 p1, T2 p2, T3 p3, out T4 p4);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, T5, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, out T5 p5);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, T6, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, out T6 p6);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, T7, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, out T7 p7);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, T8, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, out T8 p8);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, T9, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, out T9 p9);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, T10, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, out T10 p10);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, T11, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, out T11 p11);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, T12, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, out T12 p12);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, T13, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, out T13 p13);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, T14, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, out T14 p14);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, T15, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, out T15 p15);
    public delegate TOut FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, T16, out TOut>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, out T16 p16);

    public delegate TOut FuncInOut<T1, T2, out TOut>(in T1 p1, out T2 p2);
    public delegate TOut FuncInOut<T1, T2, T3, out TOut>(in T1 p1, in T2 p2, out T3 p3);
    public delegate TOut FuncInOut<T1, T2, T3, T4, out TOut>(in T1 p1, in T2 p2, in T3 p3, out T4 p4);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, out T5 p5);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, out T6 p6);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, out T7 p7);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, out T8 p8);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, out T9 p9);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, out T10 p10);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, out T11 p11);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, out T12 p12);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, out T13 p13);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, out T14 p14);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, out T15 p15);
    public delegate TOut FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, out TOut>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15, out T16 p16);

    public delegate void ActionIn<T>(in T parameter);
    public delegate void ActionIn<T1, T2>(in T1 p1, in T2 p2);
    public delegate void ActionIn<T1, T2, T3>(in T1 p1, in T2 p2, in T3 p3);
    public delegate void ActionIn<T1, T2, T3, T4>(in T1 p1, in T2 p2, in T3 p3, in T4 p4);
    public delegate void ActionIn<T1, T2, T3, T4, T5>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15);
    public delegate void ActionIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15, in T16 p16);

    public delegate void ActionRef<T>(ref T parameter);
    public delegate void ActionRef<T1, T2>(ref T1 p1, ref T2 p2);
    public delegate void ActionRef<T1, T2, T3>(ref T1 p1, ref T2 p2, ref T3 p3);
    public delegate void ActionRef<T1, T2, T3, T4>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4);
    public delegate void ActionRef<T1, T2, T3, T4, T5>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10, ref T11 p11);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10, ref T11 p11, ref T12 p12);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10, ref T11 p11, ref T12 p12, ref T13 p13);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10, ref T11 p11, ref T12 p12, ref T13 p13, ref T14 p14);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10, ref T11 p11, ref T12 p12, ref T13 p13, ref T14 p14, ref T15 p15);
    public delegate void ActionRef<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(ref T1 p1, ref T2 p2, ref T3 p3, ref T4 p4, ref T5 p5, ref T6 p6, ref T7 p7, ref T8 p8, ref T9 p9, ref T10 p10, ref T11 p11, ref T12 p12, ref T13 p13, ref T14 p14, ref T15 p15, ref T16 p16);
}
