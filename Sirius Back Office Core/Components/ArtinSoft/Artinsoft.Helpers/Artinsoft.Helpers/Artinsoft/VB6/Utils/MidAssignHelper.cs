using System;
using System.Collections.Generic;
using System.Text;

namespace Artinsoft.VB6.Utils
{
	/// <summary>
	/// Helper used to manage assignments to MID calls.
	/// </summary>
	public class MidAssignHelper
	{
		/// <summary>
		/// Assign a value to a string using MID.
		/// </summary>
		/// <param name="o">The object that contains the string.</param>
		/// <param name="start">The position at which to start adding characters.</param>
		/// <param name="length">The amount of characters to add.</param>
		/// <param name="value">The value to assign.</param>
		public static void Assign(object o, int start, int length, string value)
		{
			string propValue = DefaultPropHelper.GetDefaultProperty<string>(o);
			if (value.Length > length)
			{
				value = value.Substring(0, length);
			}
			propValue = propValue.Insert(start, value);
			DefaultPropHelper.SetDefaultProperty(ref o, propValue);
		}
	}
}
