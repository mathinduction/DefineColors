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

		protected override double ClusterDintance(List<Tuple<Color, int>> cluster1, List<Tuple<Color, int>> cluster2)
		{
			double d = 0;

			foreach (Tuple<Color, int> c1 in cluster1)
			{
				foreach (Tuple<Color, int> c2 in cluster2)
				{
					d += ColorDistance(c1.Item1, c2.Item1);
				}
			}

			return d / (cluster1.Count * cluster2.Count);
		}
	}
}
