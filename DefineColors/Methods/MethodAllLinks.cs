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

		protected override double ColorClusterDintance(Tuple<Color, int> color, List<Tuple<Color, int>> cluster)
		{
			double maxD = ColorDistance(color.Item1, cluster[0].Item1);
			foreach (Tuple<Color, int> c in cluster)
			{
				double d = ColorDistance(color.Item1, c.Item1);
				if (d > maxD)
					maxD = d;
			}

			return maxD;
		}
	}
}
