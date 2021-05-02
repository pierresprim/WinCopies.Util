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

#if CS7
    public delegate (bool result, Exception ex) FieldValidateValueCallback(object obj, object value, FieldInfo field, string paramName);
#endif

    public delegate void FieldValueChangedCallback(object obj, object value, FieldInfo field, string paramName);

#if CS7
    public delegate (bool result, Exception ex) PropertyValidateValueCallback(object obj, object value, PropertyInfo property, string paramName);
#endif

    public delegate void PropertyValueChangedCallback(object obj, object value, PropertyInfo property, string paramName);

    /// <summary>
    /// Delegate for a non-generic predicate.
    /// </summary>
    /// <param name="value">The value to test</param>
    /// <returns><see langword="true"/> if the predicate success, otherwise <see langword="false"/>.</returns>
    public delegate bool Predicate(object value);

    public delegate T Converter<T>(object obj);

    public delegate void ActionParams(params object[] args);

    public delegate void ActionParams<in T>(params T[] args);

    /// <summary>
    /// Represents a delegate that returns an object.
    /// </summary>
    /// <returns>Any object.</returns>
    public delegate object Func();

    public delegate object FuncParams(params object[] args);

    public delegate TResult FuncParams<in TParams, out TResult>(params TParams[] args);

    public delegate TResult FuncOut<in T1, T2, out TResult>(T1 p1, out T2 p2);
    public delegate TResult FuncOut<in T1, in T2, T3, out TResult>(T1 p1, T2 p2, out T3 p3);
    public delegate TResult FuncOut<in T1, in T2, in T3, T4, out TResult>(T1 p1, T2 p2, T3 p3, out T4 p4);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, T5, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, out T5 p5);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, T6, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, out T6 p6);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, T7, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, out T7 p7);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, T8, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, out T8 p8);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, T9, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, out T9 p9);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, T10, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, out T10 p10);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, T11, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, out T11 p11);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, T12, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, out T12 p12);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, T13, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, out T13 p13);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, T14, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, out T14 p14);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, T15, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, out T15 p15);
    public delegate TResult FuncOut<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, T16, out TResult>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, out T16 p16);

    public delegate TResult FuncIn<T1, out TResult>(in T1 p1);
    public delegate TResult FuncIn<T1, T2, out TResult>(in T1 p1, in T2 p2);
    public delegate TResult FuncIn<T1, T2, T3, out TResult>(in T1 p1, in T2 p2, in T3 p3);
    public delegate TResult FuncIn<T1, T2, T3, T4, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15);
    public delegate TResult FuncIn<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15, in T16 p16);

    public delegate TResult FuncInOut<T1, T2, out TResult>(in T1 p1, out T2 p2);
    public delegate TResult FuncInOut<T1, T2, T3, out TResult>(in T1 p1, in T2 p2, out T3 p3);
    public delegate TResult FuncInOut<T1, T2, T3, T4, out TResult>(in T1 p1, in T2 p2, in T3 p3, out T4 p4);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, out T5 p5);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, out T6 p6);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, out T7 p7);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, out T8 p8);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, out T9 p9);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, out T10 p10);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, out T11 p11);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, out T12 p12);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, out T13 p13);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, out T14 p14);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, out T15 p15);
    public delegate TResult FuncInOut<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, out TResult>(in T1 p1, in T2 p2, in T3 p3, in T4 p4, in T5 p5, in T6 p6, in T7 p7, in T8 p8, in T9 p9, in T10 p10, in T11 p11, in T12 p12, in T13 p13, in T14 p14, in T15 p15, out T16 p16);

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
}
