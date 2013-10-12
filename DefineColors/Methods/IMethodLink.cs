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

			var Matrix = MakeMatrix(_clusters);
			while (_clusters.Count > _numberColors)
			{
				#region До тех пор, пока кластеров больше, чем необходимо

				var coords = FindMin(Matrix);
				var cluster1 = _clusters[coords.Item1];
				var cluster2 = _clusters[coords.Item2];

				#region Сдвигаем строки так, что бы строки, соответствующие найденным координатам были последней и предпоследней
				
				int last = Matrix.Count - 1;
				if (coords.Item2 != last)
				{
					SwapRow(Matrix, coords.Item1, last);
					_clusters[coords.Item1] = _clusters[last];
					_clusters[last] = cluster1;

					SwapRow(Matrix, coords.Item2, last - 1);
					_clusters[coords.Item2] = _clusters[last - 1];
					_clusters[last - 1] = cluster2;
				}
				else
				{
					SwapRow(Matrix, coords.Item1, last - 1);
					_clusters[coords.Item1] = _clusters[last - 1];
					_clusters[last - 1] = cluster1;
				}
				#endregion

				_clusters.RemoveAt(last);
				_clusters.RemoveAt(last - 1);
				Matrix.RemoveAt(last);
				Matrix.RemoveAt(last - 1);

				AddRow(Matrix, cluster1.Concat(cluster2).ToList(), _clusters);
				_clusters.Add(cluster1.Concat(cluster2).ToList());

				#endregion
			}
			
			GetColorsFromClusters();
		}

#region Работа с матрицей

		/// <summary>
		/// Создаем матрицу расстояний
		/// </summary>
		private List<List<Double>> MakeMatrix(List<List<Tuple<Color, int>>> clusters)
		{
			List<List<Double>> Matrix = new List<List<double>>();

			int i = 0;
			foreach (List<Tuple<Color, int>> cluster in clusters)
			{
				List<double> row = new List<double>();
				for (int j = 0; j < i; j++)//Нижний треугольник
				{
					double d = ClusterDintance(cluster, clusters[j]);
					row.Add(d);
				}
				Matrix.Add(row);
				i++;
			}

			return Matrix;
		}

		private Tuple<int, int> FindMin (List<List<Double>> Matrix)
		{
			int min1 = 0, min2 = 0;
			double minD = double.MaxValue;
			int i = 0, j = 0;

			foreach (List<double> row in Matrix)
			{
				j = 0;
				foreach (double d in row)
				{
					if (d < minD)
					{
						minD = d;
						min1 = i;
						min2 = j;
					}
					j++;
				}
				i++;
			}

			return new Tuple<int, int>(min1, min2);
		}

		/// <summary>
		/// Добавляет строку в конец матрицы
		/// </summary>
		private void AddRow(List<List<Double>> Matrix, List<Tuple<Color, int>> cluster, List<List<Tuple<Color, int>>> clusters)
		{
			List<double> row = new List<double>();
			foreach (List<Tuple<Color, int>> c in clusters)
			{
				double d = ClusterDintance(cluster, c);
				row.Add(d);
			}
			Matrix.Add(row);
		}

		/// <summary>
		/// Меняет местами строки
		/// </summary>
		private void SwapRow(List<List<Double>> Matrix, int index1, int index2)
		{
			if (index1 == index2)
				return; 

			if (index1 > index2)//Упорядочиваем индексы по возрастанию
			{
				int ii = index2;
				index2 = index1;
				index1 = ii;
			}

			for (int i = 0; i < index1; i++)
			{
				double d = Matrix[index1][i];
				Matrix[index1][i] = Matrix[index2][i];
				Matrix[index2][i] = d;
			}
			for (int i = index1 + 1; i < index2; i++)
			{
				double d = Matrix[i][index1];
				Matrix[i][index1] = Matrix[index2][i];
				Matrix[index2][i] = d;
			}
			for (int i = index2 + 1; i < Matrix.Count; i++)
			{
				double d = Matrix[i][index1];
				Matrix[i][index1] = Matrix[i][index2];
				Matrix[i][index2] = d;
			}
		}

#endregion

		/// <summary>
		/// Вычисляет расстояние между двумя кластерами
		/// </summary>
		protected abstract double ClusterDintance(List<Tuple<Color, int>> cluster1, List<Tuple<Color, int>> cluster2);
	}
}
