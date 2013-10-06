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
		private const double _norm = 441.672955930063709849498817084;//=Math.Sqrt(195075)=Math.Sqrt(255*255 + 255*255 + 255*255); - максимально возможное расстояние

		public override void SetEps(double eps)
		{
			_eps = eps * _norm;
		}
		/// <summary>
		/// Поиск цветов
		/// </summary>
		public override void FindColors(Bitmap bitmap)//TODO исправить алгоритм
		{
			base.FindColors(bitmap);

			List<Tuple<Color, int>> cluster = new List<Tuple<Color, int>>();

			while (_pixels.Count() > 0)
			{
				int i = 0;
				int newColorsInCluster = 0;
				cluster = new List<Tuple<Color, int>>();
				cluster.Add(_pixels[0]);
				_pixels.RemoveAt(0);
				int size = _pixels.Count();

				#region //Повторять до тех пор, пока есть цвета, достаточно близкие хотя бы к одному цвету в кластере
				do
				{
					newColorsInCluster = 0;
					while (i < size)//До тех пор пока есть непросмотренные цвета
					{
						if (AddPredicate(_pixels[i], cluster))//Добавляем цвет
						{
							cluster.Add(_pixels[i]);
							_pixels.RemoveAt(i);
							size--;
							newColorsInCluster++;
						}
						else
						{
							i++;
						}
					}

				} while (newColorsInCluster > 0);
				_clusters.Add(cluster);
				#endregion
			}

			GetColorsFromClusters();
		}

		private bool AddPredicate(Tuple<Color, int> color, List<Tuple<Color, int>> cluster)
		{
			return true;//TODO
		}

		/// <summary>
		/// Вычисляет расстояние между заданныи цветом и кластером
		/// </summary>
		protected abstract double ColorClusterDintance(Tuple<Color, int> color, List<Tuple<Color, int>> cluster);
	}
}
