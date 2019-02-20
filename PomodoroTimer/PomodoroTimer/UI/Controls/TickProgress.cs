using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// #laterusable
namespace PomodoroTimer.Controls
{

    public enum Direction
    {
        Counterclockwise,
        Clockwise
    }
    /// <summary>
    /// Progress control
    /// </summary>
    public class TickProgress : ContentView
    {
        public Direction Direction { get; set; } = Direction.Clockwise;

        public float StartAngleDegree { get; set; } = 0;
        public float FinishAngleDegree { get; set; } = 360;

        public bool IsAntialias { get; set; } = true;
        public bool Effect { get; set; } = false;

        public float TickWidth { get; set; } = 1;
        public float TickSize { get; set; } = 20;
        public float CurrentTickSize { get; set; } = 30;

        public Func<object, object, object, int> TickFucntion = null;

        public Color TickColor { get; set; }
        public Color EllapsedTickColor { get; set; }
        public Color CurrentTickColor { get; set; }

        private SKCanvasView canvasView;
        private SKPaint CurrentTickPaint { get; set; }
        private SKPaint EllepsedTickPaint { get; set; }
        private SKPaint TickPaint { get; set; }

        private static void ItemChanged(BindableObject bindable, object oldValue, object newValue)
        {

            var control = bindable as TickProgress;
            control.ReDraw();
        }

        public static readonly BindableProperty CurrentTickProperty = BindableProperty.Create(
                                                   propertyName: nameof(CurrentTick),
                                                   returnType: typeof(object),
                                                   declaringType: typeof(TickProgress),
                                                   defaultValue: 0,
                                                   defaultBindingMode: BindingMode.TwoWay,
                                                   propertyChanged: ItemChanged);

        public static readonly BindableProperty TickCountProperty = BindableProperty.Create(
                                                   propertyName: nameof(TickCount),
                                                   returnType: typeof(int),
                                                   declaringType: typeof(TickProgress),
                                                   defaultValue: 100,
                                                   defaultBindingMode: BindingMode.TwoWay,
                                                   propertyChanged: ItemChanged);

        public float CurrentTick
        {
            get { return (float)GetValue(CurrentTickProperty); }
            set
            {
                SetValue(CurrentTickProperty, value);
                OnPropertyChanged(nameof(CurrentTick));
            }
        }
        public int TickCount
        {
            get { return (int)GetValue(TickCountProperty); }
            set
            {
                SetValue(TickCountProperty, value);
                OnPropertyChanged(nameof(TickCount));
            }
        }



        //TODO ReDraw draw all tick again try to draw  new tick      
        public void ReDraw()
        {
            canvasView.InvalidateSurface();
        }

        public TickProgress()
        {
            canvasView = new SKCanvasView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            canvasView.PaintSurface += SKCanvasView_PaintSurface;

            Content = canvasView;
        }


        private void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {

            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Transparent);

            int width = e.Info.Width;
            int height = e.Info.Height;

            double heightAsDp = Height;
            double widthAsDp = Width;
            var density = width / widthAsDp;

            float tickSizeAsPixel = (float)density * TickSize;
            float currentTickSizeAsPixel = (float)density * CurrentTickSize;
            float tickWidthAsPixel = (float)density * TickWidth;

            TickPaint = new SKPaint()
            {
                Color = SKColors.Wheat,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = tickWidthAsPixel,
                IsAntialias = IsAntialias,
            };
            CurrentTickPaint = new SKPaint()
            {
                Color = SKColors.MediumVioletRed,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = tickWidthAsPixel * 1.5f,
                IsAntialias = IsAntialias,
            };
            EllepsedTickPaint = new SKPaint()
            {
                Color = SKColors.MidnightBlue,
                Style = SKPaintStyle.StrokeAndFill,
                StrokeWidth = tickWidthAsPixel,
                IsAntialias = IsAntialias,
            };


            int radius = (width <= height ? width : height) / 2;
            float TickMargin = currentTickSizeAsPixel - tickSizeAsPixel;
            float degreeStep = (Direction == Direction.Clockwise ? 1 : -1) * (FinishAngleDegree - StartAngleDegree) / TickCount;
            float currentTickDegree = CurrentTick * (Direction == Direction.Clockwise ? 1 : -1) * (FinishAngleDegree - StartAngleDegree) / TickCount;
            var tickStartRadius = radius - currentTickSizeAsPixel;
            var tickEndRadius = radius - TickMargin;
            canvas.Translate(width / 2, height / 2);
            canvas.Save();
            canvas.RotateDegrees(StartAngleDegree);
            
            for (int i = 0; i < TickCount; i++)
            {
                if (i < CurrentTick)
                {
                    canvas.DrawLine(0, tickStartRadius, 0, tickEndRadius, EllepsedTickPaint);
                }
                else
                {
                    canvas.DrawLine(0, tickStartRadius, 0, tickEndRadius, TickPaint);
                }
                canvas.RotateDegrees(degreeStep);
            }


            canvas.Restore();

            canvas.Save();
            canvas.RotateDegrees(StartAngleDegree + currentTickDegree);
            canvas.DrawLine(0, tickStartRadius, 0, radius, CurrentTickPaint);
            canvas.Restore();

        }
    }
}