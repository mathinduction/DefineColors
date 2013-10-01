using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodMeanLink : IMethodLink
	{
		public override string MethodName()
		{
			return "Метод средней связи";
		}

		/// <summary>
		/// Добавлять ли указанный цвет в кластер
		/// </summary>
		protected override bool AddPredicate(Color color, List<Color> cluster)
		{
			//Цвет добавляется, если цвета в кластере в среднем достаточно близки к данному
			double d = 0;
			foreach (Color c in cluster)
			{
				d += ColorDistance(color, c);
			}
			d /= cluster.Count();

			return d <= _eps;
		}
	}
}
