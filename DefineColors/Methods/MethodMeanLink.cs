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
		protected override bool AddPredicate(Tuple<Color, int> color, List<Tuple<Color, int>> cluster)
		{
			//Цвет добавляется, если цвета в кластере в среднем достаточно близки к данному
			double d = 0;
			foreach (Tuple<Color, int> c in cluster)
			{
				d += ColorDistance(color.Item1, c.Item1);
			}
			d /= cluster.Count();

			return d <= _eps;
		}
	}
}
