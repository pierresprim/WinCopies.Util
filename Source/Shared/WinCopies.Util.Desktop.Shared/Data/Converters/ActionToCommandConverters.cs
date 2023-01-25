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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

using WinCopies.Commands;
using WinCopies.Util.Commands;

using static WinCopies.Util.Data.ConverterHelper;

namespace WinCopies.Util.Data
{
    public class ActionToCommandConverter : AlwaysConvertibleTwoWayConverter<Action<object>, object, ICommand>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;

        public override IReadOnlyConversionOptions ConvertBackOptions => ParameterCanBeNull;

        protected override ICommand Convert(Action<object> value, object parameter, CultureInfo culture) => new DelegateCommand3(value);

        protected override Action<object> ConvertBack(ICommand value, object parameter, CultureInfo culture) => value.Execute;
    }

    [ValueConversion(typeof(Action), typeof(DelegateCommand2))]
    public class ActionToDelegateCommand2Converter : AlwaysConvertibleTwoWayConverter<Action, object, DelegateCommand2>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;
        public override IReadOnlyConversionOptions ConvertBackOptions => ParameterCanBeNull;

        protected override DelegateCommand2 Convert(Action value, object parameter, CultureInfo culture) => new DelegateCommand2(value);

        protected override Action ConvertBack(DelegateCommand2 value, object parameter, CultureInfo culture) => value.ExecuteDelegate;
    }

    public class CommandConverter : AlwaysConvertibleOneWayConverter<Commands.Primitives.ICommand, object, ICommand>
    {
        public override IReadOnlyConversionOptions ConvertOptions => ParameterCanBeNull;

        protected override ICommand Convert(Commands.Primitives.ICommand value, object parameter, CultureInfo culture) => new CommandModel<Commands.Primitives.ICommand>(value);
    }
}
