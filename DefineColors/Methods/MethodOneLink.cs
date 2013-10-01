using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DefineColors.Methods
{
	public class MethodOneLink : IMethod
	{
		private double _eps = 100;//TODO задание из интерфейса + нормировку на sqrt(195075) = sqrt(255*255 + 255*255 + 255*255)

		public override string MethodName()
		{
			return "Метод одиночной связи";
		}

		public override void FindColors(Bitmap bitmap)
		{
			base.FindColors(bitmap);
	
			List<Color> cluster = new List<Color>();

			while (_pixels.Count() != 0)
			{
				int i = 0; 
				int newColorsInCluster = 0;
				cluster = new List<Color>();
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

		/// <summary>
		/// Добавлять ли указанный цвет в кластер
		/// </summary>
		private bool AddPredicate(Color color, List<Color> cluster)
		{
			double d = _eps + 1;
			int i = -1;
			while (d > _eps && i < cluster.Count() - 1)
			{
				i++;
				d = ColorDistance(color, cluster[i]);
			}

			return d <= _eps;
		}
	}
}
