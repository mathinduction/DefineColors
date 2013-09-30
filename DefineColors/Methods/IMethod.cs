using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DefineColors.Methods
{
	public abstract class IMethod
	{
		protected List<Color> _pixels = new List<Color>();
		protected List<List<Color>> _clusters = new List<List<Color>>(); 

		public abstract string MethodName();
		public virtual List<Color> FindColors(Bitmap bitmap)
		{
			_pixels = BitmapToList(bitmap);
			return null;
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

