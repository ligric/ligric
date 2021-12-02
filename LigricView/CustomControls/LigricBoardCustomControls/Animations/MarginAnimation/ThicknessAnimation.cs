//using System;
//using System.Linq;
//using Uno;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Media.Animation;

//namespace LigricBoardCustomControls.Animations.ThicknessAnimation
//{
//#if UAP10_0_18362
//    public interface IEasingFunction
//    {
//        double Ease(double currentTime, double startValue, double finalValue, double duration);
//    }
//    internal interface IAnimation<T> where T : struct
//    {
//        T? To { get; }

//        T? From { get; }

//        T? By { get; }

//        bool EnableDependentAnimation { get; }

//        IEasingFunction EasingFunction { get; }

//        T Subtract(T minuend, T subtrahend);

//        T Add(T first, T second);

//        T Multiply(float multiplier, T t);

//        T Convert(object value);
//    }
//    public class ThicknessAnimation : Timeline, IAnimation<Thickness>
//    {       
//        bool IAnimation<Thickness>.EnableDependentAnimation => EnableDependentAnimation;

//        IEasingFunction IAnimation<Thickness>.EasingFunction => null;

//        public EasingFunctionBase EasingFunction
//        {
//            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
//            set { SetValue(EasingFunctionProperty, value); }
//        }


//        public static DependencyProperty EasingFunctionProperty { get; } =
//            DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(ThicknessAnimation), new PropertyMetadata(null));


//        public Thickness? To
//        {
//            get { return (Thickness?)GetValue(ToProperty); }
//            set { SetValue(ToProperty, value); }
//        }

//        public Thickness? From
//        {
//            get { return (Thickness?)GetValue(FromProperty); }
//            set { SetValue(FromProperty, value); }
//        }


//        public bool EnableDependentAnimation
//        {
//            get { return (bool)GetValue(EnableDependentAnimationProperty); }
//            set { SetValue(EnableDependentAnimationProperty, value); }
//        }

//        public Thickness? By
//        {
//            get { return (Thickness?)GetValue(ByProperty); }
//            set { SetValue(ByProperty, value); }
//        }

//        public static DependencyProperty ByProperty { get; } =
//            DependencyProperty.Register("By", typeof(Thickness?), typeof(ThicknessAnimation), new PropertyMetadata(null));


//        public static DependencyProperty EnableDependentAnimationProperty { get; } =
//            DependencyProperty.Register("EnableDependentAnimation", typeof(bool), typeof(ThicknessAnimation), new PropertyMetadata(false));


//        public static DependencyProperty FromProperty { get; } =
//            DependencyProperty.Register("From", typeof(Thickness?), typeof(ThicknessAnimation), new PropertyMetadata(null));


//        public static DependencyProperty ToProperty { get; } =
//            DependencyProperty.Register("To", typeof(Thickness?), typeof(ThicknessAnimation), new PropertyMetadata(null));


//        Thickness IAnimation<Thickness>.Subtract(Thickness minuend, Thickness subtrahend)
//        {
//            var left = minuend.Left - subtrahend.Left;
//            var top = minuend.Top - subtrahend.Top;
//            var right = minuend.Right - subtrahend.Right;
//            var bottom = minuend.Bottom - subtrahend.Bottom;

//            return new Thickness(left, top, right, bottom);
//        }

//        Thickness IAnimation<Thickness>.Add(Thickness first, Thickness second)
//        {
//            var left = first.Left + second.Left;
//            var top = first.Top + second.Top;
//            var right = first.Right + second.Right;
//            var bottom = first.Bottom + second.Bottom;

//            return new Thickness(left, top, right, bottom);
//        }

//        Thickness IAnimation<Thickness>.Multiply(float multiplier, Thickness thickness)
//        {
//            var left = multiplier * (float)thickness.Left;
//            var top = multiplier * (float)thickness.Top;
//            var right = multiplier * (float)thickness.Right;
//            var bottom = multiplier * (float)thickness.Bottom;

//            return new Thickness(left, top, right, bottom);
//        }

//        Thickness IAnimation<Thickness>.Convert(object value)
//        {
//            string text = value as string;

//            if (text != null)
//            {
//                return text.ToThickness();
//            }

//            return default(Thickness);
//        }
//    }
//#endif

//#if NET6_0_ANDROID || NETSTANDARD2_0
//    internal interface IAnimation<T> where T : struct
//    {
//        T? To { get; }

//        T? From { get; }

//        T? By { get; }

//        bool EnableDependentAnimation { get; }

//        IEasingFunction EasingFunction { get; }

//        T Subtract(T minuend, T subtrahend);

//        T Add(T first, T second);

//        T Multiply(float multiplier, T t);

//        T Convert(object value);
//    }

//    public class ThicknessAnimation : Timeline, IAnimation<Thickness>
//    {
//        bool IAnimation<Thickness>.EnableDependentAnimation => EnableDependentAnimation;

//        [NotImplemented(new string[]
//        {
//                    "__ANDROID__",
//                    "__IOS__",
//                    "NET461",
//                    "__WASM__",
//                    "__SKIA__",
//                    "__NETSTD_REFERENCE__",
//                    "__MACOS__"
//        })]
//        IEasingFunction IAnimation<Thickness>.EasingFunction => null;

//        public EasingFunctionBase EasingFunction
//        {
//            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
//            set { SetValue(EasingFunctionProperty, value); }
//        }


//        public static DependencyProperty EasingFunctionProperty { get; } =
//            DependencyProperty.Register("EasingFunction", typeof(EasingFunctionBase), typeof(ThicknessAnimation), new FrameworkPropertyMetadata((object)null));


//        public Thickness? To
//        {
//            get { return (Thickness?)GetValue(ToProperty); }
//            set { SetValue(ToProperty, value); }
//        }

//        public Thickness? From
//        {
//            get { return (Thickness?)GetValue(FromProperty); }
//            set { SetValue(FromProperty, value); }
//        }


//        public bool EnableDependentAnimation
//        {
//            get { return (bool)GetValue(EnableDependentAnimationProperty); }
//            set { SetValue(EnableDependentAnimationProperty, value); }
//        }

//        public Thickness? By
//        {
//            get { return (Thickness?)GetValue(ByProperty); }
//            set { SetValue(ByProperty, value); }
//        }

//        public static DependencyProperty ByProperty { get; } =
//            DependencyProperty.Register("By", typeof(Thickness?), typeof(ThicknessAnimation), new FrameworkPropertyMetadata((object)null));


//        public static DependencyProperty EnableDependentAnimationProperty { get; } =
//            DependencyProperty.Register("EnableDependentAnimation", typeof(bool), typeof(ThicknessAnimation), new FrameworkPropertyMetadata(false));


//        public static DependencyProperty FromProperty { get; } =
//            DependencyProperty.Register("From", typeof(Thickness?), typeof(ThicknessAnimation), new FrameworkPropertyMetadata((object)null));


//        public static DependencyProperty ToProperty { get; } =
//            DependencyProperty.Register("To", typeof(Thickness?), typeof(ThicknessAnimation), new FrameworkPropertyMetadata((object)null, FrameworkPropertyMetadataOptions.Default));


//        Thickness IAnimation<Thickness>.Subtract(Thickness minuend, Thickness subtrahend)
//        {
//            var left = minuend.Left - subtrahend.Left;
//            var top = minuend.Top - subtrahend.Top;
//            var right = minuend.Right - subtrahend.Right;
//            var bottom = minuend.Bottom - subtrahend.Bottom;

//            return new Thickness(left, top, right, bottom);
//        }

//        Thickness IAnimation<Thickness>.Add(Thickness first, Thickness second)
//        {
//            var left = first.Left + second.Left;
//            var top = first.Top + second.Top;
//            var right = first.Right + second.Right;
//            var bottom = first.Bottom + second.Bottom;

//            return new Thickness(left, top, right, bottom);
//        }

//        Thickness IAnimation<Thickness>.Multiply(float multiplier, Thickness thickness)
//        {
//            var left = multiplier * (float)thickness.Left;
//            var top = multiplier * (float)thickness.Top;
//            var right = multiplier * (float)thickness.Right;
//            var bottom = multiplier * (float)thickness.Bottom;

//            return new Thickness(left, top, right, bottom);
//        }

//        Thickness IAnimation<Thickness>.Convert(object value)
//        {
//            string text = value as string;

//            if (text != null)
//            {
//                return text.ToThickness();
//            }

//            return default(Thickness);
//        }
//    }
//#endif

//    public static class ThicknessExtentions
//    {
//        private static readonly char[] _valueSeparator = new char[] { ',', ' ', '\t', '\r', '\n' };
//        public static Thickness ToThickness(this object value)
//        {
//            if (value is string stringValue)
//            {
//                if (string.IsNullOrEmpty(stringValue))
//                    return default(Thickness);

//                var values = stringValue
//                    .Split(_valueSeparator, StringSplitOptions.RemoveEmptyEntries)
//                    .Select(s => double.Parse(s))
//                    .ToArray();

//                if (values.Length == 4)
//                {
//                    return new Thickness(values[0], values[1], values[2], values[3]);
//                }
//                else if (values.Length == 2)
//                {
//                    return new Thickness(values[0], values[1], values[0], values[1]);
//                }
//                else
//                {
//                    return new Thickness(values[0], values[0], values[0], values[0]);
//                }
//            }
//            else if (value is double doubleValue)
//            {
//                return new Thickness(doubleValue);
//            }
//            else if (value is float floatValue)
//            {
//                return new Thickness(floatValue);
//            }
//            else if (value is int intValue)
//            {
//                return new Thickness(intValue);
//            }

//            return default(Thickness);
//        }
//    }
//}
