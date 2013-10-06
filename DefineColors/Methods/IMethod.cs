using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DefineColors.Methods
{
	public abstract class IMethod
	{
		protected List<Tuple<Color, int>> _pixels = new List<Tuple<Color, int>>();
		protected List<List<Tuple<Color, int>>> _clusters = new List<List<Tuple<Color, int>>>(); 
		protected List<Color> _colors = new List<Color>();
		protected double _eps = 0;
		private const double _eps_same = 30;

		public virtual string MethodName()
		{
			return "";
		}

		public virtual void FindColors(Bitmap bitmap)
		{
			_colors = new List<Color>();
			_clusters = new List<List<Tuple<Color, int>>>();
			_pixels = BitmapToList(bitmap);
		}

		public List<List<Tuple<Color, int>>> GetClusters()
		{
			return _clusters;
		}

		public List<Color> GetColors()
		{
			return _colors;
		}

		/// <summary>
		/// Нормировка Eps
		/// </summary>
		public abstract void SetEps(double eps);

		/// <summary>
		/// "Расстояние" между цветами
		/// </summary>
		protected double ColorDistance(Color color1, Color color2)
		{
			double rez =
				Math.Sqrt((color1.R - color2.R)*(color1.R - color2.R) +
				          (color1.G - color2.G)*(color1.G - color2.G) +
				          (color1.B - color2.B)*(color1.B - color2.B));

			return rez;
		}

		/// <summary>
		/// Вычисляет средний цвет по каждому кластеру
		/// </summary>
		protected void GetColorsFromClusters()
		{
			//TODO брать не средний, а медиану?
			_colors = new List<Color>();
			foreach (List<Tuple<Color, int>> cluster in _clusters)
			{
				double r = 0, b = 0, g = 0;
				int size = 0;
				foreach (Tuple<Color, int> color in cluster)//TODO возможо переволнение!!
				{
					r += color.Item1.R * color.Item2;//TODO проверить как теперь работает
					g += color.Item1.G * color.Item2;
					b += color.Item1.B * color.Item2;
					size += color.Item2;
				}
				r /= size;
				g /= size;
				b /= size;
				_colors.Add(Color.FromArgb(255, (int)r, (int)g, (int)b));
			}
		}

		private List<Tuple<Color, int>> BitmapToList(Bitmap bitmap)
		{
			if (bitmap == null)
				return null;

			for (int i = 0; i < bitmap.Width; i++)
				for (int j = 0; j < bitmap.Height; j++)
				{
					Color color = bitmap.GetPixel(i, j);
					//int num = _pixels.FindIndex(x => x.Item1.R == color.R && x.Item1.G == color.G && x.Item1.B == color.B);
					int num = _pixels.FindIndex(x => ((x.Item1.R - color.R) * (x.Item1.R - color.R) + 
						(x.Item1.G - color.G) * (x.Item1.G - color.G) + 
						(x.Item1.B - color.B) * (x.Item1.B - color.B)) < _eps_same);
					if (num >= 0)//Такой цвет уже есть
					{
						int oldCount = _pixels[num].Item2;
						_pixels[num] = new Tuple<Color, int>(color, oldCount + 1);
					}
					else
					{
						_pixels.Add(new Tuple<Color, int>(color, 1));
					}
				}

			return _pixels;
		}
	}
}

