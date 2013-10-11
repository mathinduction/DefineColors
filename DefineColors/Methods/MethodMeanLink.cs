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

		protected override double ColorClusterDintance(List<Tuple<Color, int>> cluster)
		{
			double d = 0;
			for (int i = 0; i < cluster.Count; i++)
			{
				Tuple<Color, int> c = cluster[i];
				for (int j = i + 1; j < cluster.Count; j++)
				{
					d += ColorDistance(cluster[j].Item1, c.Item1);
				}
			}

			return d/cluster.Count;
		}
	}
}
