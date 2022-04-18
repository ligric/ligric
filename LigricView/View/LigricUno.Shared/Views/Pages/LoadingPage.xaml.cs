using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace LigricUno.Views.Pages
{
    public sealed partial class LoadingPage : Page
    {
        public LoadingPage()
        {
            this.InitializeComponent();
            SizeChanged += MainPage_SizeChanged;

        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Part1.Point3 = new Point(500, e.NewSize.Height / 2);
            Part2.Point2 = new Point(50, e.NewSize.Height - 100);
            Part2.Point3 = new Point(0, e.NewSize.Height);
            EndLine.Point = new Point(0, e.NewSize.Height);
        }

        const int StartWidth = 50;
        const int MaxDiffX = 400;
        const double HeightToWidthRatio = 1.5;
        private void path_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            // The location of the center is where the mouse/touch point is
            var newCenter = e.GetCurrentPoint(this).Position;
            var part1_point3 = newCenter;

            // Next determine the width of the shape
            var startX = Math.Max(StartWidth, newCenter.X - MaxDiffX);
            var width = newCenter.X - startX;

            // Calculate the height of the shape using the ratio
            var height = width * HeightToWidthRatio;

            // As we approach the other size, calculate how much space there is
            // and if close, reduce the width of the shape accordingly
            var remainingWidth = ActualWidth - newCenter.X;
            if (remainingWidth < width)
            {
                width = remainingWidth;
                startX = Math.Max(StartWidth, newCenter.X - width);
            }

            // The first line goes from 0,0 out along the X axis
            var start_point = new Point(startX, 0);
            // The last line goes from 0,<actual height> out along the X axis
            var end_point = new Point(startX, this.ActualHeight);


            // Calculate top and bottom of the shape
            var top = newCenter.Y - height / 2;
            var top_point = new Point(start_point.X, top);
            var bottom = newCenter.Y + height / 2;
            var part2_point3 = new Point(start_point.X, bottom);

            // Spread the remaining points in the shape
            var part1_point1 = new Point(start_point.X, top + height * 0.25);
            var part1_point2 = new Point(newCenter.X, top + height * 0.35);
            var part2_point1 = new Point(newCenter.X, top + height * 0.65);
            var part2_point2 = new Point(start_point.X, top + height * 0.75);


            StartLinePoint.Value = start_point;
            TopLinePoint.Value = top_point;
            Part1Point1.Value = part1_point1;
            Part1Point2.Value = part1_point2;
            Part1Point3.Value = part1_point3;
            Part2Point1.Value = part2_point1;
            Part2Point2.Value = part2_point2;
            Part2Point3.Value = part2_point3;
            BottomLinePoint.Value = end_point;

            if (customStoryboard.GetCurrentState() != Windows.UI.Xaml.Media.Animation.ClockState.Stopped)
            {
                customStoryboard.Pause();
                customStoryboard.Resume();
            }
            else
            {
                customStoryboard.Begin();

            }
        }
    }
}
