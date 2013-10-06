using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	class MethodSearchCondensations : IMethod
	{
		private const double _eps_stop = 0.00000001;
		private const double _norm = 441.672955930063709849498817084;//=Math.Sqrt(195075)=Math.Sqrt(255*255 + 255*255 + 255*255); - максимально возможное расстояние

		public override void SetEps(double eps)
		{
			_eps = eps * _norm;
		}

		public override string MethodName()
		{
			return "Метод поиска сгущений";
		}

		public override void FindColors(System.Drawing.Bitmap bitmap)
		{
			base.FindColors(bitmap);

			while (_pixels.Count() > 0)//До тех пор, пока есть пиксели, не отнесенные ни к одному из кластеров
			{
				double delta = 0;
				Random rand = new Random();
				int index = rand.Next(_pixels.Count());
				Color center = _pixels[index].Item1;
				Color newCenter = center;
				List<Tuple<Color, int>> currentCluster = new List<Tuple<Color, int>>();
				#region Передвигаем центр сферы
				do
				{
					center = newCenter;
					var newData = CountNewCenter(center);
					newCenter = newData.Item1;
					currentCluster = newData.Item2;
					delta = ColorDistance(center, newCenter);
				} while (delta > _eps_stop);
				#endregion
				_clusters.Add(currentCluster);
				//Удаляем из рассмотрения пиксели, вошедшие в кластер
				foreach (Tuple<Color, int> pixel in currentCluster)
				{
					int num = _pixels.FindIndex(x => x.Item1.R == pixel.Item1.R && x.Item1.G == pixel.Item1.G && x.Item1.B == pixel.Item1.B && x.Item2 == pixel.Item2);
					if (num >= 0)
					{
						_pixels.RemoveAt(num);
					}
				}
			}

			GetColorsFromClusters();
		}

		/// <summary>
		/// Вычисляет новый центр сферы и список цветов, попавших в текущую сферу
		/// </summary>
		/// <param name="currentCenter"></param>
		/// <returns></returns>
		private Tuple<Color, List<Tuple<Color, int>>> CountNewCenter(Color currentCenter)
		{
			List<Tuple<Color, int>> cluster = new List<Tuple<Color, int>>();
			Color center = new Color();
			double r = 0, g = 0, b = 0;

			foreach (Tuple<Color, int> pixel in _pixels)
			{
				double d = ColorDistance(currentCenter, pixel.Item1);
				if (d < _eps)//Если цвет входит в сферу с текущим центром
				{
					cluster.Add(pixel);
					r += pixel.Item1.R;
					g += pixel.Item1.G;
					b += pixel.Item1.B;
				}
			}
			r /= cluster.Count;
			g /= cluster.Count;
			b /= cluster.Count;

			center = Color.FromArgb(255, (int)r, (int)g, (int)b);

			return new Tuple<Color, List<Tuple<Color, int>>>(center, cluster);
		}
	}
}
