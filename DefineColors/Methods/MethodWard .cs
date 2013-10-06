using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	class MethodWard : IMethod
	{
		private const double _norm = 4e5;//TODO подобрать нормальное значение для нормировки

		public override void SetEps(double eps)
		{
			_eps = eps * _norm;
		}

		public override string MethodName()
		{
			return "Метод Уорда";
		}

		/// <summary>
		/// Поиск цветов
		/// </summary>
		public override void FindColors(Bitmap bitmap)//TODO сделать другую нормировку Eps?
		{
			base.FindColors(bitmap);

			#region Вначале каждый элемент - кластер

			foreach (Tuple<Color, int> pixel in _pixels)
			{
				List<Tuple<Color, int>> cluster = new List<Tuple<Color, int>>();
				cluster.Add(pixel);
				_clusters.Add(cluster);
			}
			_pixels = new List<Tuple<Color, int>>();

			#endregion

			#region Продолжать до тех пор, пока есть достаточно близкие кластеры

			int size;
			do
			{
				size = _clusters.Count;

				#region Ищем самыe близкие друг к другу кластеры

				int count = _clusters.Count(), i = 0;
				while (i < count)
				{
					var cluster = _clusters[i];
					_clusters.RemoveAt(i);

					List<Tuple<Color, int>> newCluster = new List<Tuple<Color, int>>();
					double currentV = V(cluster);
					double minV = int.MaxValue;
					List<Tuple<Color, int>> minCluster = new List<Tuple<Color, int>>();
					foreach (List<Tuple<Color, int>> c in _clusters)//Ищем самый близкий к данному кластер
					{
						newCluster = cluster.Concat(c).ToList();
						double newV = V(newCluster);
						if (newV < minV)
						{
							minV = newV;
							minCluster = c;
						}
					}

					if ((minV - currentV) > _eps)//Если найденный кластер недостаточно близок
					{
						_clusters.Insert(i, cluster);
						i++;
						count = _clusters.Count();
						continue;
					}

					//Иначе, выясняем, является ли данный кластер самый близкий к найденному и если да, то объединяем их
					_clusters.Remove(minCluster);
					int j = 0;
					double v = int.MaxValue; 
					while (j < _clusters.Count() && minV < v)
					{
						newCluster = minCluster.Concat(_clusters[j]).ToList();
						v = V(newCluster);
						j++;
					}

					if (minV < v)//Если просмотрели все кластеры и не нашли ничего ближе
					{
						_clusters.Add(cluster.Concat(minCluster).ToList());
					}
					else
					{
						_clusters.Add(minCluster);
						_clusters.Insert(i, cluster);
						i++;
						count = _clusters.Count();
						continue;
					}
					count = _clusters.Count();
				}
				
				#endregion

			} while ((size - _clusters.Count()) > 0);

			#endregion
			GetColorsFromClusters();
		}

		/// <summary>
		/// Расчитывает сумму внутригрупповых квадратов отклонения для кластера
		/// </summary>
		private double V(List<Tuple<Color, int>> cluster)
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
