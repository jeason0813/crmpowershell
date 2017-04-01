﻿/*
CRM PowerShell Library
Copyright (C) 2017 Arjan Meskers / AMSoftware

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.ComponentModel;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace AMSoftware.Crm.PowerShell.Common.Converters
{
    public class LabelConverter : StringConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return new Label((string)base.ConvertFrom(context, culture, value), CrmContext.Language);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            LocalizedLabel languageLabel = ((Label)value).LocalizedLabels.SingleOrDefault(l => l.LanguageCode == CrmContext.Language);
            if (languageLabel != null)
            {
                return base.ConvertTo(context, culture, languageLabel.Label, destinationType);
            }
            return null;
        }
    }
}