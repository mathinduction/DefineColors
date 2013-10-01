using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DefineColors.Methods
{
	public abstract class IMethod
	{
		private const double _norm = 441.672955930063709849498817084;//=Math.Sqrt(195075)=Math.Sqrt(255*255 + 255*255 + 255*255); - максимально возможное расстояние
		protected double _eps = _norm * 0.5;

		protected List<Color> _pixels = new List<Color>();
		protected List<List<Color>> _clusters = new List<List<Color>>(); 
		protected List<Color> _colors = new List<Color>(); 

		public abstract string MethodName();
		public virtual void FindColors(Bitmap bitmap)
		{
			_colors = new List<Color>();
			_clusters = new List<List<Color>>();
			_pixels = BitmapToList(bitmap);
		}

		public List<List<Color>> GetClusters()
		{
			return _clusters;
		}

		public List<Color> GetColors()
		{
			return _colors;
		}

		public void SetEps(double eps)
		{
			_eps = eps*_norm;
		}

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
			_colors = new List<Color>();
			foreach (List<Color> cluster in _clusters)
			{
				double r = 0, b = 0, g = 0;
				foreach (Color color in cluster)//TODO возможо переволнение!!
				{
					r += color.R;
					g += color.G;
					b += color.B;
				}
				int size = cluster.Count();
				r /= size;
				g /= size;
				b /= size;
				_colors.Add(Color.FromArgb(255, (int)r, (int)g, (int)b));
			}
		}

		private List<Color> BitmapToList(Bitmap bitmap)
		{
			if (bitmap == null)
				return null;

			for (int i = 0; i < bitmap.Width; i++)
				for (int j = 0; j < bitmap.Height; j++)
				{
					_pixels.Add(bitmap.GetPixel(i,j));
				}

			return _pixels;//TODO убрать повторы
		}
	}
}

