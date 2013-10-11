using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace DefineColors.Methods
{
	public abstract class IMethodLink : IMethod
	{
		/// <summary>
		/// Поиск цветов
		/// </summary>
		public override void FindColors(Bitmap bitmap)
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

			string msg = "stop";

			int size;
			try
			{
				#region Продолжать до тех пор, пока кластеров больше, чем необходимо
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
						double currentV = ColorClusterDintance(cluster);
						double minV = int.MaxValue;
						List<Tuple<Color, int>> minCluster = new List<Tuple<Color, int>>();
						foreach (List<Tuple<Color, int>> c in _clusters)//Ищем самый близкий к данному кластер
						{
							newCluster = cluster.Concat(c).ToList();
							double newV = ColorClusterDintance(newCluster);
							if (newV < minV)
							{
								minV = newV;
								minCluster = c;
							}
						}

						//Выясняем, является ли данный кластер самый близкий к найденному и если да, то объединяем их
						_clusters.Remove(minCluster);
						int j = 0;
						double v = int.MaxValue;
						while (j < _clusters.Count() && minV < v)
						{
							newCluster = minCluster.Concat(_clusters[j]).ToList();
							v = ColorClusterDintance(newCluster);
							j++;
						}

						if (minV < v)//Если просмотрели все кластеры и не нашли ничего ближе
						{
							_clusters.Add(cluster.Concat(minCluster).ToList());
							if (_clusters.Count() == _numberColors)//Если нашли необходимое число кластеров
								throw new Exception(msg);
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
			}
			catch (Exception e)
			{
				if (e.Message == msg)//Если нашли достаточное число цветов
					GetColorsFromClusters();
			}
		}

		/// <summary>
		/// Вычисляет расстояние между элементами кластера
		/// </summary>
		protected abstract double ColorClusterDintance(List<Tuple<Color, int>> cluster);
	}
}
