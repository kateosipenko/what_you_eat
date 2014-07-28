using System;
using System.Net;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using LinqToVisualTree;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace MetroInMotionUtils
{
	public static class MetroInMotion
	{
		#region AnimationLevel

		public static int GetAnimationLevel(DependencyObject obj)
		{
			return (int)obj.GetValue(AnimationLevelProperty);
		}

		public static void SetAnimationLevel(DependencyObject obj, int value)
		{
			obj.SetValue(AnimationLevelProperty, value);
		}


		public static readonly DependencyProperty AnimationLevelProperty =
			DependencyProperty.RegisterAttached("AnimationLevel", typeof(int),
			typeof(MetroInMotion), new PropertyMetadata(-1));

		#endregion


		#region IsPivotAnimated

		public static bool GetIsPivotAnimated(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsPivotAnimatedProperty);
		}

		public static void SetIsPivotAnimated(DependencyObject obj, bool value)
		{
			obj.SetValue(IsPivotAnimatedProperty, value);
		}

		public static readonly DependencyProperty IsPivotAnimatedProperty =
			DependencyProperty.RegisterAttached("IsPivotAnimated", typeof(bool),
			typeof(MetroInMotion), new PropertyMetadata(false, OnIsPivotAnimatedChanged));

		private static void OnIsPivotAnimatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			ItemsControl list = d as ItemsControl;

			list.Loaded += (s2, e2) =>
			  {
				  // locate the pivot control that this list is within
				  Pivot pivot = list.Ancestors<Pivot>().Single() as Pivot;

				  // and its index within the pivot
				  int pivotIndex = pivot.Items.IndexOf(list.Ancestors<PivotItem>().Single());

				  bool selectionChanged = false;

				  pivot.SelectionChanged += (s3, e3) =>
					{
						selectionChanged = true;
					};

				  // handle manipulation events which occur when the user
				  // moves between pivot items
				  pivot.ManipulationCompleted += (s, e) =>
					{
						if (!selectionChanged)
							return;

						selectionChanged = false;

						if (pivotIndex != pivot.SelectedIndex)
							return;

						// determine which direction this tab will be scrolling in from
						bool fromRight = e.TotalManipulation.Translation.X <= 0;


						// iterate over each of the items in view
						var items = list.GetItemsInView().ToList();
						for (int index = 0; index < items.Count; index++)
						{
							var lbi = items[index];

							list.Dispatcher.BeginInvoke(() =>
							{
								var animationTargets = lbi.Descendants()
													   .Where(p => MetroInMotion.GetAnimationLevel(p) > -1);
								foreach (FrameworkElement target in animationTargets)
								{
									// trigger the required animation
									GetSlideAnimation(target, fromRight).Begin();
								}
							});
						};

					};
			  };
		}


		#endregion

		/// <summary>
		/// Animates each element in order, creating a 'peel' effect. The supplied action
		/// is invoked when the animation ends.
		/// </summary>
		public static void Peel(this IEnumerable<FrameworkElement> elements, Action endAction)
		{
			var elementList = elements.ToList();
			//var lastElement = elementList.Last();
			var lastElement = elementList.First();

			// iterate over all the elements, animating each of them
			double delay = 0;
			foreach (FrameworkElement element in elementList)
			{
				var sb = GetPeelAnimation(element, delay);

				// add a Completed event handler to the last element
				if (element.Equals(lastElement))
				{
					sb.Completed += (s, e) =>
					  {
						  endAction();
					  };
				}

				sb.Begin();
				delay += 50;
			}
		}

		public static void PeelBack(this IEnumerable<FrameworkElement> elements, Action endAction)
		{
			var elementList = elements.ToList();
			var lastElement = elementList.Last();

			// iterate over all the elements, animating each of them
			double delay = 0;
			foreach (FrameworkElement element in elementList)
			{
				var sb = GetPeelBackAnimation(element, delay);

				// add a Completed event handler to the last element
				if (element.Equals(lastElement))
				{
					sb.Completed += (s, e) =>
					{
						endAction();
					};
				}

				sb.Begin();
				delay += 50;
			}
		}


		/// <summary>
		/// Enumerates all the items that are currently visible in am ItemsControl. This implementation assumes
		/// that a VirtualizingStackPanel is being used as the ItemsPanel.
		/// </summary>
		public static IEnumerable<FrameworkElement> GetItemsInView(this ItemsControl itemsControl)
		{
			// locate the stack panel that hosts the items
			VirtualizingStackPanel vsp = itemsControl.Descendants<VirtualizingStackPanel>().First() as VirtualizingStackPanel;

			// iterate over each of the items in view
			int firstVisibleItem = (int)vsp.VerticalOffset;
			int visibleItemCount = (int)vsp.ViewportHeight;
			for (int index = firstVisibleItem; index <= firstVisibleItem + visibleItemCount + 1; index++)
			{
				var item = itemsControl.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement;
				if (item == null)
					continue;

				yield return item;
			}
		}

		public static IEnumerable<FrameworkElement> GetItemsInView(this ListBox listBox)
		{
			// locate the stack panel that hosts the items
			VirtualizingStackPanel vsp = listBox.Descendants<VirtualizingStackPanel>().First() as VirtualizingStackPanel;

			// iterate over each of the items in view
			int firstVisibleItem = (int)vsp.VerticalOffset;
			int visibleItemCount = (int)vsp.ViewportHeight;
			for (int index = firstVisibleItem; index <= firstVisibleItem + visibleItemCount + 1; index++)
			{
				var item = listBox.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement;
				if (item == null)
					continue;

				yield return item;
			}
		}

		/// <summary>
		/// Creates a PlaneProjection and associates it with the given element, returning
		/// a Storyboard which will animate the PlaneProjection to 'peel' the item
		/// from the screen.
		/// </summary>
		private static Storyboard GetPeelAnimation(FrameworkElement element, double delay)
		{
			Storyboard sb;

			var projection = new PlaneProjection()
			{
				CenterOfRotationX = -0.1
			};
			element.Projection = projection;

			// compute the angle of rotation required to make this element appear
			// at a 90 degree angle at the edge of the screen.
			var width = element.ActualWidth;
			var targetAngle = Math.Atan(1000 / (width / 2));
			targetAngle = targetAngle * 180 / Math.PI;

			// animate the projection
			sb = new Storyboard();
			sb.BeginTime = TimeSpan.FromMilliseconds(delay);
			sb.Children.Add(CreateAnimation(0, -(180 - targetAngle), 0.3, "RotationY", projection));
			sb.Children.Add(CreateAnimation(0, 23, 0.3, "RotationZ", projection));
			sb.Children.Add(CreateAnimation(0, -23, 0.3, "GlobalOffsetZ", projection));
			return sb;
		}

		private static Storyboard GetPeelBackAnimation(FrameworkElement element, double delay)
		{
			Storyboard sb;

			var projection = new PlaneProjection()
			{
				CenterOfRotationX = -0.1
			};
			element.Projection = projection;

			// compute the angle of rotation required to make this element appear
			// at a 90 degree angle at the edge of the screen.
			var width = element.ActualWidth;
			var targetAngle = Math.Atan(1000 / (width / 2));
			targetAngle = targetAngle * 180 / Math.PI;

			// animate the projection
			sb = new Storyboard();
			sb.BeginTime = TimeSpan.FromMilliseconds(delay);
			sb.Children.Add(CreateBackAnimation(0, -(180 - targetAngle), 0.3, "RotationY", projection));
			sb.Children.Add(CreateBackAnimation(0, 23, 0.3, "RotationZ", projection));
			sb.Children.Add(CreateBackAnimation(0, -23, 0.3, "GlobalOffsetZ", projection));
			return sb;
		}

		private static DoubleAnimation CreateAnimation(double from, double to, double duration,
		  string targetProperty, DependencyObject target)
		{
			var db = new DoubleAnimation();
			db.To = to;
			db.From = from;
			db.EasingFunction = new SineEase();
			db.Duration = TimeSpan.FromSeconds(duration);
			Storyboard.SetTarget(db, target);
			Storyboard.SetTargetProperty(db, new PropertyPath(targetProperty));
			return db;
		}

		private static DoubleAnimation CreateBackAnimation(double from, double to, double duration,
		string targetProperty, DependencyObject target)
		{
			var db = new DoubleAnimation();
			db.To = from;
			db.From = to;
			db.EasingFunction = new SineEase();
			db.Duration = TimeSpan.FromSeconds(duration);
			Storyboard.SetTarget(db, target);
			Storyboard.SetTargetProperty(db, new PropertyPath(targetProperty));
			return db;
		}

		/// <summary>
		/// Creates a TranslateTransform and associates it with the given element, returning
		/// a Storyboard which will animate the TranslateTransform with a SineEase function
		/// </summary>
		private static Storyboard GetSlideAnimation(FrameworkElement element, bool fromRight)
		{
			double from = fromRight ? 80 : -80;

			Storyboard sb;
			double delay = (MetroInMotion.GetAnimationLevel(element)) * 0.1 + 0.1;

			TranslateTransform trans = new TranslateTransform() { X = from };
			element.RenderTransform = trans;

			sb = new Storyboard();
			sb.BeginTime = TimeSpan.FromSeconds(delay);
			sb.Children.Add(CreateAnimation(from, 0, 0.8, "X", trans));
			return sb;
		}

	}

	public static class ExtensionMethods
	{
		public static Point GetRelativePosition(this UIElement element, UIElement other)
		{
			return element.TransformToVisual(other)
			  .Transform(new Point(0, 0));
		}
	}

	/// <summary>
	/// Animates an element so that it flies out and flies in!
	/// </summary>
	public class ItemFlyInAndOutAnimations
	{
		private Popup _popup;

		private Canvas _popupCanvas;

		private FrameworkElement _targetElement;

		private Point _targetElementPosition;

		private Image _targetElementClone;

		private Rectangle _backgroundMask;

		private static TimeSpan _flyInSpeed = TimeSpan.FromMilliseconds(700);

		private static TimeSpan _flyOutSpeed = TimeSpan.FromMilliseconds(300);

		public ItemFlyInAndOutAnimations()
		{
			// construct a popup, with a Canvas as its child
			_popup = new Popup();
			_popupCanvas = new Canvas();
			_popup.Child = _popupCanvas;
		}

		public static void TitleFlyIn(FrameworkElement title)
		{
			TranslateTransform trans = new TranslateTransform();
			trans.X = 300;
			trans.Y = 0;
			title.RenderTransform = trans;

			var sb = new Storyboard();

			// animate the X position
			var db = CreateDoubleAnimation(300, 0,
				new QuarticEase { EasingMode = EasingMode.EaseOut }, trans, TranslateTransform.XProperty, _flyInSpeed);
			sb.Children.Add(db);

			// animate the Y position
			//db = CreateDoubleAnimation(0, 0,
			//	new SineEase(), trans, TranslateTransform.YProperty, _flyInSpeed);
			//sb.Children.Add(db);

			sb.Begin();
		}

		public static void VerticalTitleFlyIn(FrameworkElement title)
		{
			TranslateTransform trans = new TranslateTransform();
			trans.X = 0;
			trans.Y = -200;
			title.RenderTransform = trans;

			var sb = new Storyboard();

			// animate the X position
			//var db = CreateDoubleAnimation(0, 0,
			//	new SineEase(), trans, TranslateTransform.XProperty, _flyInSpeed);
			//sb.Children.Add(db);

			// animate the Y position
			var db = CreateDoubleAnimation(-200, 0,
				new QuarticEase { EasingMode = EasingMode.EaseOut }, trans, TranslateTransform.YProperty, _flyInSpeed);
			sb.Children.Add(db);

			sb.Begin();
		}

		public static void VerticalContentFlyIn(FrameworkElement content, double delay = 0)
		{
			TranslateTransform trans = new TranslateTransform();
			trans.X = 0;
			trans.Y = 200;
			content.RenderTransform = trans;

			var sb = new Storyboard();

			// animate the X position
			//var db = CreateDoubleAnimation(0, 0,
			//	new SineEase(), trans, TranslateTransform.XProperty, _flyInSpeed);
			//sb.Children.Add(db);

			// animate the Y position
			var db = CreateDoubleAnimation(200, 0,
				new QuarticEase { EasingMode = EasingMode.EaseOut }, trans, TranslateTransform.YProperty, _flyInSpeed, delay);
			sb.Children.Add(db);

			sb.Begin();
		}

		/// <summary>
		/// Animate the previously 'flown-out' element back to its original location.
		/// </summary>
		public void ItemFlyIn()
		{
			if (_targetElement != null && _targetElement.RenderTransform is CompositeTransform)
			{
				(_targetElement.RenderTransform as CompositeTransform).TranslateX = 500;

				Storyboard storyboard = new Storyboard();

				DoubleAnimation translateAnimation = new DoubleAnimation();
				PropertyPath path = new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
				Storyboard.SetTargetProperty(translateAnimation, path);
				translateAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
				translateAnimation.From = 500;
				translateAnimation.To = 0;
				translateAnimation.BeginTime = TimeSpan.FromMilliseconds(200);
				Storyboard.SetTarget(translateAnimation, _targetElement);
				storyboard.Children.Add(translateAnimation);

				DoubleAnimation opacityAnimation = new DoubleAnimation();
				PropertyPath pahtOpacity = new PropertyPath("(UIElement.Opacity)");
				Storyboard.SetTargetProperty(opacityAnimation, pahtOpacity);
				opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
				opacityAnimation.From = 0;
				opacityAnimation.To = 1;
				opacityAnimation.BeginTime = TimeSpan.FromMilliseconds(200);
				Storyboard.SetTarget(opacityAnimation, _targetElement);
				storyboard.Children.Add(opacityAnimation);

				storyboard.Begin();
			}
		}

		public void ItemFlyOut(FrameworkElement element, Action action)
		{
			_targetElement = element;
			(_targetElement.RenderTransform as CompositeTransform).TranslateX = 0;

			Storyboard storyboard = new Storyboard();

			DoubleAnimation translateAnimation = new DoubleAnimation();
			PropertyPath path = new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
			Storyboard.SetTargetProperty(translateAnimation, path);
			translateAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
			translateAnimation.From = 0;
			translateAnimation.To = 500;
			translateAnimation.BeginTime = TimeSpan.FromMilliseconds(200);
			Storyboard.SetTarget(translateAnimation, _targetElement);
			storyboard.Children.Add(translateAnimation);

			DoubleAnimation opacityAnimation = new DoubleAnimation();
			PropertyPath pahtOpacity = new PropertyPath("(UIElement.Opacity)");
			Storyboard.SetTargetProperty(opacityAnimation, pahtOpacity);
			opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
			opacityAnimation.From = 1;
			opacityAnimation.To = 0;
			opacityAnimation.BeginTime = TimeSpan.FromMilliseconds(200);
			Storyboard.SetTarget(opacityAnimation, _targetElement);
			storyboard.Children.Add(opacityAnimation);
		

			storyboard.Completed += (sender, args) => action();

			storyboard.Begin();
		}

		private static DoubleAnimation CreateDoubleAnimation(double from, double to, IEasingFunction easing,
		  DependencyObject target, object propertyPath, TimeSpan duration, double delay = 0)
		{
			var db = new DoubleAnimation();
			db.To = to;
			db.From = from;
			db.BeginTime = TimeSpan.FromMilliseconds(delay);
			db.EasingFunction = easing;
			db.Duration = duration;
			Storyboard.SetTarget(db, target);
			Storyboard.SetTargetProperty(db, new PropertyPath(propertyPath));
			return db;
		}
	}
}
