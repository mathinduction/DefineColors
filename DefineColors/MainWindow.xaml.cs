using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DefineColors.Methods;
using System.Drawing;

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
			var colors = method.FindColors(_sourseBitmap);
		}

		private void buttonOpenFile_Click(object sender, RoutedEventArgs e)
		{
			//TODO
			PrepareImage(_file);
		}

		/// <summary>
		/// Подготовка изображения
		/// </summary>
		private void PrepareImage(string file)
		{
			_sourseBitmap = new Bitmap(file);
			//var c = _sourseBitmap.GetPixel(0,0);
		}
	}
}
