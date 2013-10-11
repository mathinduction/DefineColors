using System;
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

		protected override double ColorClusterDintance(List<Tuple<Color, int>> cluster)
		{
			double minD = double.MaxValue;
			for (int i = 0; i < cluster.Count; i++)
			{
				Tuple<Color, int> c = cluster[i];
				for (int j = i + 1; j < cluster.Count; j++)
				{
					double d = ColorDistance(cluster[j].Item1, c.Item1);
					if (d < minD)
						minD = d;
				}
			}

			return minD;
		}
	}
}
