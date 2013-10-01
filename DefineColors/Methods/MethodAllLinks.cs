﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodAllLinks : IMethod
	{
		public override string MethodName()
		{
			return "Метод полных связей";
		}

		public override void FindColors(Bitmap bitmap)
		{
			base.FindColors(bitmap);


			GetColorsFromClusters();
		}
	}
}
