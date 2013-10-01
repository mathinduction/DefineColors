﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodOneLink : IMethodLink
	{
		public override string MethodName()
		{
			return "Метод одиночной связи";
		}

		/// <summary>
		/// Добавлять ли указанный цвет в кластер
		/// </summary>
		protected override bool AddPredicate(Tuple<Color, int> color, List<Tuple<Color, int>> cluster)
		{
			//Цвет добавляется, если в кластере есть хотябы один цвет достаточно близкий к данному
			double d = _eps + 1;
			int i = -1;
			while (d > _eps && i < cluster.Count() - 1)
			{
				i++;
				d = ColorDistance(color.Item1, cluster[i].Item1);
			}

			return d <= _eps;
		}
	}
}
