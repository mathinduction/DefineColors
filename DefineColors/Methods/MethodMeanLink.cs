using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodMeanLink : IMethod
	{
		public override string MethodName()
		{
			return "Метод средней связи";
		}

		public override void FindColors(Bitmap bitmap)
		{
			base.FindColors(bitmap);


			GetColorsFromClusters();
		}
	}
}
