using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DefineColors.Methods
{
	public abstract class IMethod
	{
		public abstract string MethodName();
		public abstract List<Color> FindColors(Bitmap bitmap);
	}
}
