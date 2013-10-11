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

		protected override double ColorClusterDintance(List<Tuple<Color, int>> cluster)
		{
			double maxD = double.MinValue;
			for (int i = 0; i < cluster.Count; i++ )
			{
				Tuple<Color, int> c = cluster[i];
				for (int j = i + 1; j < cluster.Count; j++ )
				{
					double d = ColorDistance(cluster[j].Item1, c.Item1);
					if (d > maxD)
						maxD = d;
				}
			}

			return maxD;
		}
	}
}
