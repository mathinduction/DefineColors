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

		protected override double ClusterDintance(List<Tuple<Color, int>> cluster1, List<Tuple<Color, int>> cluster2)
		{
			double maxD = double.MinValue;

			foreach (Tuple<Color, int> c1 in cluster1)
				foreach (Tuple<Color, int> c2 in cluster2)
				{
					double d = ColorDistance(c1.Item1, c2.Item1);
					if (d > maxD)
						maxD = d;
				}

			return maxD;
		}
	}
}
