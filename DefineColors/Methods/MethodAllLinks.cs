using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodAllLinks : IMethodLink
	{
		public override string MethodName()
		{
			return "Метод полных связей";
		}

		/// <summary>
		/// Добавлять ли указанный цвет в кластер
		/// </summary>
		protected override bool AddPredicate(Tuple<Color, int> color, List<Tuple<Color, int>> cluster)
		{
			//Цвет добавляется, если в кластере все цвета достаточно близки к данному
			double d = 0;
			int i = 0;
			do
			{
				d = ColorDistance(color.Item1, cluster[i].Item1);
				i++;
			} while (d <= _eps && i < cluster.Count());

			return d <= _eps;
		}
	}
}
