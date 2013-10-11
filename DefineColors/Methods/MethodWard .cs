using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	class MethodWard : IMethodLink
	{
		public override string MethodName()
		{
			return "Метод Уорда";
		}

		/// <summary>
		/// Расчитывает сумму внутригрупповых квадратов отклонения для кластера
		/// </summary>
		protected override double ColorClusterDintance(List<Tuple<Color, int>> cluster)
		{
			double r = 0, b = 0, g = 0;
			int size = 0;
			foreach (Tuple<Color, int> color in cluster)
			{
				r += color.Item1.R * color.Item2;
				g += color.Item1.G * color.Item2;
				b += color.Item1.B * color.Item2;
				size += color.Item2;
			}
			r /= size;
			g /= size;
			b /= size;

			double v = 0;
			foreach (Tuple<Color, int> color in cluster)
			{
				v += (color.Item1.R - r) * (color.Item1.R - r);
				v += (color.Item1.G - g) * (color.Item1.G - g);
				v += (color.Item1.B - b) * (color.Item1.B - b);
			}

			return v;
		}
	}
}
