﻿/* Source: https://dzone.com/articles/how-get-eventargs ; Author of original code: Marlon Grech
 * 
 * This is free and unencumbered software released into the public domain.
 * 
 * Anyone is free to copy, modify, publish, use, compile, sell, or
 * distribute this software, either in source code form or as a compiled
 * binary, for any purpose, commercial or non-commercial, and by any
 * means.
 *
 * In jurisdictions that recognize copyright laws, the author or authors
 * of this software dedicate any and all copyright interest in the
 * software to the public domain. We make this dedication for the benefit
 * of the public at large and to the detriment of our heirs and
 * successors. We intend this dedication to be an overt act of
 * relinquishment in perpetuity of all present and future rights to this
 * software under copyright law.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * For more information, please refer to <http://unlicense.org> */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

#if WinCopies2
namespace WinCopies.Util.Commands
#else
namespace WinCopies.Commands
#endif
{
    /// <summary>
    /// Generates delegates according to the specified signature on runtime
    /// </summary>
    public static class EventHandlerGenerator
    {
        /// <summary>
        /// Generates a delegate with a matching signature of the supplied eventHandlerType
        /// This method only supports Events that have a delegate of type void
        /// </summary>
        /// <param name="eventHandlerType">The delegate type to wrap. Note that this must always be a void delegate</param>
        /// <param name="methodToInvoke">The method to invoke</param>
        /// <param name="methodInvoker">The object where the method resides</param>
        /// <returns>Returns a delegate with the same signature as eventHandlerType that calls the methodToInvoke inside</returns>
        public static Delegate CreateDelegate(Type eventHandlerType, MethodInfo methodToInvoke, object methodInvoker)
        {
            if (typeof(Delegate).IsAssignableFrom(eventHandlerType))
            {
                //Get the eventHandlerType signature
                MethodInfo eventHandlerInfo = eventHandlerType.GetMethod("Invoke");
                Type returnType = eventHandlerInfo.ReturnParameter.ParameterType;

                if (returnType == typeof(void))
                {
                    ParameterInfo[] delegateParameters = eventHandlerInfo.GetParameters();
                    //Get the list of type of parameters. Please note that we do + 1 because we have to push the object where the method resides i.e methodInvoker parameter
                    var hookupParameters = new Type[delegateParameters.Length + 1];
                    hookupParameters[0] = methodInvoker.GetType();

                    for (int i = 0; i < delegateParameters.Length; i++)

                        hookupParameters[i + 1] = delegateParameters[i].ParameterType;

                    var handler = new DynamicMethod("", null,
                        hookupParameters, typeof(EventHandlerGenerator));

                    ILGenerator eventIL = handler.GetILGenerator();

                    //load the parameters or everything will just BAM :)
                    LocalBuilder local = eventIL.DeclareLocal(typeof(object[]));
                    eventIL.Emit(OpCodes.Ldc_I4, delegateParameters.Length + 1);
                    eventIL.Emit(OpCodes.Newarr, typeof(object));
                    eventIL.Emit(OpCodes.Stloc, local);

                    //start from 1 because the first item is the instance. Load up all the arguments
                    for (int i = 1; i < delegateParameters.Length + 1; i++)
                    {
                        eventIL.Emit(OpCodes.Ldloc, local);
                        eventIL.Emit(OpCodes.Ldc_I4, i);
                        eventIL.Emit(OpCodes.Ldarg, i);
                        eventIL.Emit(OpCodes.Stelem_Ref);
                    }

                    eventIL.Emit(OpCodes.Ldloc, local);

                    //Load as first argument the instance of the object for the methodToInvoke i.e methodInvoker
                    eventIL.Emit(OpCodes.Ldarg_0);

                    //Now that we have it all set up call the actual method that we want to call for the binding
                    eventIL.EmitCall(OpCodes.Call, methodToInvoke, null);

                    eventIL.Emit(OpCodes.Pop);
                    eventIL.Emit(OpCodes.Ret);

                    //create a delegate from the dynamic method
                    return handler.CreateDelegate(eventHandlerType, methodInvoker);
                }
                else
                    throw new ApplicationException("Delegate has a return type. This only supprts event handlers that are void");
            }
            else
                throw new ArgumentException($"The {nameof(Delegate)} type must be assignable from {nameof(eventHandlerType)}");
        }
    }
}