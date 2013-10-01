using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	class MethodWard : IMethod
	{
		public override string MethodName()
		{
			return "Метод Уорда";
		}

		/// <summary>
		/// Поиск цветов
		/// </summary>
		public override void FindColors(Bitmap bitmap)
		{
			base.FindColors(bitmap);

			//TODO

			GetColorsFromClusters();
		}
	}
}
