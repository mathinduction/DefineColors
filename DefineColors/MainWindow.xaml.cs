using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DefineColors.Methods;
using System.Drawing;		//TODO а нужен ли вообще?
using Microsoft.Win32;
using Color = System.Drawing.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace DefineColors
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private List<IMethod> _methods = new List<IMethod>();
		private Bitmap _sourseBitmap;
		private string _file = "";
		
		public MainWindow()
		{
			InitializeComponent();

			#region Список методов

			_methods.Add(new MethodOneLink());
			_methods.Add(new MethodAllLinks());
			_methods.Add(new MethodMeanLink());
			_methods.Add(new MethodWard());
			_methods.Add(new MethodSearchCondensations());

			foreach (var m in _methods)
			{
				comboBoxMethod.Items.Add(m.MethodName());
			}

			comboBoxMethod.SelectedIndex = 0;

			#endregion
		}

		private void buttonStart_Click(object sender, RoutedEventArgs e)
		{
			if (_file == "")
			{
				MessageBox.Show("Не указан фаил!", "Ошибка!");
				return;
			}

			IMethod method = _methods[comboBoxMethod.SelectedIndex];
			if (comboBoxMethod.SelectedIndex == _methods.Count() - 1)
			{
				method.SetEps(sliderEps.Value);
			}
			else
			{
				int num = 0;
				if (!int.TryParse(textBoxNumberColors.Text, out num) || num < 1 || num > 10)
				{
					MessageBox.Show("Введено некорректное число цветов! Это должно быть целое число от 1 до 10", "Ошибка!");
					return;
				}
				method.SetNumberColors(num);
			}
			method.FindColors(_sourseBitmap);
			var colors = method.GetColors();

			#region Рисуем на Canvas найденные цвета

			double h = canvasMediumColors.ActualHeight;//Высота
			double w = canvasMediumColors.ActualWidth;//Ширина

			double oneColorH = h/colors.Count();//Высота для одного цвета

			int count = 0;
			foreach (Color color in colors)
			{
				System.Windows.Shapes.Rectangle rect = new Rectangle();
				rect.Height = oneColorH;
				rect.Width = w;

				SolidColorBrush brush = new SolidColorBrush();
				brush.Color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
				rect.Fill = brush;

				Canvas.SetTop(rect, count*oneColorH);
				Canvas.SetLeft(rect, 0);

				canvasMediumColors.Children.Add(rect);
				count++;
			}

			#endregion
		}

		private void buttonOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			dlg.InitialDirectory = "D:\\";
			dlg.Filter = "All files (*.*)|*.*|PNG Photos (*.png)|*.png";
			dlg.FilterIndex = 2;
			dlg.RestoreDirectory = true;

			// Show open file dialog box
			Nullable<bool> result = dlg.ShowDialog();

			// Process open file dialog box results
			if (result == true)
			{
				// Open document
				_file = dlg.FileName;
			}

			PrepareImage(_file);
		}

		private void comboBoxMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (comboBoxMethod.SelectedIndex == _methods.Count() - 1)
			{
				textBlockNumber.Text = "Eps";

				textBoxNumberColors.Visibility = Visibility.Collapsed;

				sliderEps.Visibility = Visibility.Visible;
				textBlockSliderValue.Visibility = Visibility.Visible;
			}
			else
			{
				sliderEps.Visibility = Visibility.Collapsed;
				textBlockSliderValue.Visibility = Visibility.Collapsed;

				textBlockNumber.Text = "Число цветов";

				textBoxNumberColors.Visibility = Visibility.Visible;
			}
		}

		/// <summary>
		/// Подготовка изображения
		/// </summary>
		private void PrepareImage(string file)
		{
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(_file);
			bi.EndInit();
			imageSourse.Source = bi;

			_sourseBitmap = new Bitmap(file);
		}
	}
}
