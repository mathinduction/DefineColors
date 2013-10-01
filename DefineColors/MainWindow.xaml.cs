﻿using System;
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
		private string _file = "D:\\img.bmp";
		
		public MainWindow()
		{
			InitializeComponent();

			#region Список методов

			_methods.Add(new MethodOneLink());
			_methods.Add(new MethodAllLinks());
			_methods.Add(new MethodMeanLink());
			_methods.Add(new MethodWard());

			foreach (var m in _methods)
			{
				comboBoxMethod.Items.Add(m.MethodName());
			}

			comboBoxMethod.SelectedIndex = 0;

			#endregion

			PrepareImage(_file);
		}

		private void buttonStart_Click(object sender, RoutedEventArgs e)
		{
			IMethod method = _methods[comboBoxMethod.SelectedIndex];
			method.SetEps(sliderEps.Value);
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
