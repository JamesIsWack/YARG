﻿using System;
using System.Collections.Generic;

namespace YARG.Settings.Types
{
    public class DropdownSetting : AbstractSetting<string>
    {
        public override string AddressableName => "Setting/Dropdown";

        private readonly List<string> _possibleValues;
        public IReadOnlyList<string> PossibleValues => _possibleValues;

        public DropdownSetting(List<string> possibleValues, string value, Action<string> onChange = null) :
            base(onChange)
        {
            _possibleValues = possibleValues;
            DataField = value;
        }

        public int IndexOfOption(string option)
        {
            return _possibleValues.IndexOf(option);
        }

        public override bool IsSettingDataEqual(object obj)
        {
            if (obj is not string other)
            {
                return false;
            }

            return other == Data;
        }
    }
}