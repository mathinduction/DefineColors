using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodOneLink : IMethod
	{
		public override string MethodName()
		{
			return "Метод одиночной связи";
		}

		public override List<Color> FindColors(Bitmap bitmap)
		{
			List<Color> colors = new List<Color>();
			return colors;
		}
	}
}
