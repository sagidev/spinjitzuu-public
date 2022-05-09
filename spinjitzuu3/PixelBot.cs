using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace spinjitzuu3
{
	class PixelBot
	{
		[DllImport("user32.dll")]
		static extern IntPtr GetDC(IntPtr hwnd);

		[DllImport("user32.dll")]
		static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("gdi32.dll")]
		static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

		public System.Drawing.Color GetPixelColor(int x, int y)
		{
			IntPtr hdc = GetDC(IntPtr.Zero);
			uint pixel = GetPixel(hdc, x, y);
			ReleaseDC(IntPtr.Zero, hdc);
			Color color = Color.FromArgb((int)(pixel & 0x000000FF),
						 (int)(pixel & 0x0000FF00) >> 8,
						 (int)(pixel & 0x00FF0000) >> 16);
			return color;
		}
		Bitmap bmp = new Bitmap(1, 1);
		public Color GetColorAt(int x, int y)
		{
			Rectangle bounds = new Rectangle(x, y, 1, 1);
			using (Graphics g = Graphics.FromImage(bmp))
				g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
			return bmp.GetPixel(0, 0);
		}



		private int monitor;
		public unsafe Point[] PixelSearch(Rectangle rect, Color PixelColor, int ShadeVariation)
		{
			ArrayList arrayList = new ArrayList();
			using (var tile = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb))
			{
				if (this.monitor >= Screen.AllScreens.Length)
				{
					this.monitor = 0;
				}
				int left = Screen.AllScreens[this.monitor].Bounds.Left;
				int top = Screen.AllScreens[this.monitor].Bounds.Top;
				using (var g = Graphics.FromImage(tile))
				{
					g.CopyFromScreen(rect.X + left, rect.Y + top, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
				}
				BitmapData bitmapData = tile.LockBits(new Rectangle(0, 0, tile.Width, tile.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
				int[] array = new int[]
					{
					(int) PixelColor.B,
					(int) PixelColor.G,
					(int) PixelColor.R
				};

				for (int i = 0; i < bitmapData.Height; i++)
				{
					byte* ptr = (byte*)((void*)bitmapData.Scan0) + i * bitmapData.Stride;
					for (int j = 0; j < bitmapData.Width; j++)
					{
						if (((int)ptr[j * 3] >= array[0] - ShadeVariation & (int)ptr[j * 3] <= array[0] + ShadeVariation) && ((int)ptr[j * 3 + 1] >= array[1] - ShadeVariation & (int)ptr[j * 3 + 1] <= array[1] + ShadeVariation) && ((int)ptr[j * 3 + 2] >= array[2] - ShadeVariation & (int)ptr[j * 3 + 2] <= array[2] + ShadeVariation))
						{
							arrayList.Add(new Point(j + rect.X, i + rect.Y));
						}
					}
				}
				return (Point[])arrayList.ToArray(typeof(Point));
			}
		}
		public Point[] Search(Rectangle rect, Color Pixel_Color, int Shade_Variation)
		{
			ArrayList points = new ArrayList();
			Bitmap RegionIn_Bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);

			if (monitor >= Screen.AllScreens.Length)
			{
				monitor = 0;
				//UpdateUI();
			}

			int xOffset = Screen.AllScreens[monitor].Bounds.Left;
			int yOffset = Screen.AllScreens[monitor].Bounds.Top;

			using (Graphics GFX = Graphics.FromImage(RegionIn_Bitmap))
			{
				GFX.CopyFromScreen(rect.X + xOffset, rect.Y + yOffset, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
			}
			BitmapData RegionIn_BitmapData = RegionIn_Bitmap.LockBits(new Rectangle(0, 0, RegionIn_Bitmap.Width, RegionIn_Bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
			int[] Formatted_Color = new int[3] { Pixel_Color.B, Pixel_Color.G, Pixel_Color.R }; //bgr

			unsafe
			{
				for (int y = 0; y < RegionIn_BitmapData.Height; y++)
				{
					byte* row = (byte*)RegionIn_BitmapData.Scan0 + (y * RegionIn_BitmapData.Stride);
					for (int x = 0; x < RegionIn_BitmapData.Width; x++)
					{
						if (row[x * 3] >= (Formatted_Color[0] - Shade_Variation) & row[x * 3] <= (Formatted_Color[0] + Shade_Variation)) //blue
							if (row[(x * 3) + 1] >= (Formatted_Color[1] - Shade_Variation) & row[(x * 3) + 1] <= (Formatted_Color[1] + Shade_Variation)) //green
								if (row[(x * 3) + 2] >= (Formatted_Color[2] - Shade_Variation) & row[(x * 3) + 2] <= (Formatted_Color[2] + Shade_Variation)) //red
									points.Add(new Point(x + rect.X, y + rect.Y));
					}
				}
			}
			RegionIn_Bitmap.Dispose();
			return (Point[])points.ToArray(typeof(Point));
		}
	}
}
