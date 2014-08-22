using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Controls
{
    public class ProgressRound : Control
    {
        #region FIELDS

        private Canvas mainCanvas;

        #endregion FIELDS

        #region CONSTRUCTOR

        public ProgressRound()
        {
            this.DefaultStyleKey = typeof(ProgressRound);
            this.Loaded += this.ProgressRoundLoaded;
        }

        #endregion CONSTRUCTOR

        #region PROPERTIES

        #endregion PROPERTIES

        #region METHODS

        public override void OnApplyTemplate()
        {
            this.mainCanvas = this.GetTemplateChild("MainCanvas") as Canvas;
            base.OnApplyTemplate();
        }

        private void ProgressRoundLoaded(object sender, RoutedEventArgs e)
        {
            this.mainCanvas.Children.Clear();

            Path path = new Path();
            Canvas.SetLeft(path, this.mainCanvas.ActualWidth / 2);
            Canvas.SetTop(path, this.mainCanvas.ActualHeight / 2);
            path.Fill = new SolidColorBrush(Colors.Red);
            path.Stroke = new SolidColorBrush(Colors.Red);
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(0, 0);
            pathFigure.IsClosed = true;
            Double radius = this.mainCanvas.ActualWidth / 2;
            Double angle = 270;

            // Starting Point
            LineSegment lineSegment = new LineSegment { Point = new Point(radius, 0) };

            // Arc
            ArcSegment arcSegment = new ArcSegment();
            arcSegment.IsLargeArc = false;
            arcSegment.Size = new Size(radius, radius);
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(lineSegment);
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);
            path.Data = pathGeometry;

            var timePerFrame = 2000 / angle;

            var pointAnimation = new PointAnimationUsingKeyFrames();
            var isLargeArcAnimation = new ObjectAnimationUsingKeyFrames();

            for (int i = 1; i <= angle; i++)
            {
                var keyFrame = new LinearPointKeyFrame();
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * timePerFrame));
                keyFrame.Value = new Point(Math.Cos(i * Math.PI / 180) * radius, Math.Sin(i * Math.PI / 180) * radius);
                pointAnimation.KeyFrames.Add(keyFrame);

                var objKeyFrame = new DiscreteObjectKeyFrame();
                objKeyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * timePerFrame));
                objKeyFrame.Value = i >= 180.0;
                isLargeArcAnimation.KeyFrames.Add(objKeyFrame);
            }

            Storyboard.SetTarget(pointAnimation, arcSegment);
            Storyboard.SetTargetProperty(pointAnimation, new PropertyPath(ArcSegment.PointProperty));

            Storyboard.SetTarget(isLargeArcAnimation, arcSegment);
            Storyboard.SetTargetProperty(isLargeArcAnimation, new PropertyPath(ArcSegment.IsLargeArcProperty));
            
            Storyboard ellipseStoryboard = new Storyboard();
            ellipseStoryboard.Children.Add(pointAnimation);
            ellipseStoryboard.Children.Add(isLargeArcAnimation);

            path.Loaded += (s, args) =>
            {
                ellipseStoryboard.Begin();
            };

            Canvas containerCanvas = new Canvas();
            this.mainCanvas.Children.Add(path);
        }

        #endregion METHODS
    }
}
