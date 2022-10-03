using System;
using System.Runtime.Remoting.Contexts;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.IO;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using SharpDX.Windows;
using Bitmap = SharpDX.Direct2D1.Bitmap;

namespace Direct2dLib
{
    public class DX2D
    {
        private static DX2D _dx2d;
        public static DX2D Instance => _dx2d;

        // Фабрика для создания 2D объектов
        private SharpDX.Direct2D1.Factory factory;
        public SharpDX.Direct2D1.Factory Factory { get => factory; }

        // Фабрика для работы с текстом
        private SharpDX.DirectWrite.Factory writeFactory;
        public SharpDX.DirectWrite.Factory WriteFactory { get => writeFactory; }

        // "Цель" отрисовки
        private WindowRenderTarget renderTarget;
        public WindowRenderTarget RenderTarget { get => renderTarget; }

        // Фабрика (уже третяя?) для работы с изображениями (WIC = Windows Imaging Component)
        private ImagingFactory imagingFactory;
        public ImagingFactory ImagingFactory { get => imagingFactory; }

        // Формат текста для сообщения по центру окна (пока не используется)
        private TextFormat textFormatMessage;
        public TextFormat TextFormatMessage { get => textFormatMessage; }

        private TextFormat textFormatMessageSmall;
        public TextFormat TextFormatMessageSmall { get => textFormatMessageSmall; }

        //Настройки для кистей
        private BrushProperties bp;

        // Кисти для разных задач
        private Brush redBrush;
        private Brush whiteBrush;
        private Brush purpleBrush;
        private Brush grayBrush;
        private Brush blueBrush;
        private Brush blackBrush;
        private Brush greenBrush;

        public Brush RedBrush { get => redBrush; }
        public Brush WhiteBrush { get => whiteBrush; }
        public Brush PurpleBrush { get => purpleBrush; }
        public Brush GrayBrush { get => grayBrush; }
        public Brush BlueBrush { get => blueBrush; }
        public Brush BlackBrush { get => blackBrush; }
        public Brush GreenBrush { get => greenBrush; }

        // Счетчик кол-ва кадров
        private int countspeed;
        public int CountSpeed { get => countspeed; }
        public RawRectangleF ScreenSize { get; private set; }
        public Vector3 ScreenCenter { get; private set; }
        public RenderForm RenderForm { get; private set; }

        // В конструкторе создаем все объекты
        public DX2D(RenderForm renderForm)
        {
            RenderForm = renderForm;

            // Создание фабрик для 2D объектов и текста
            factory = new SharpDX.Direct2D1.Factory();
            writeFactory = new SharpDX.DirectWrite.Factory();
            imagingFactory = new ImagingFactory();

            // Инициализация "прямой рисовалки":
            //   Свойства отрисовщика
            RenderTargetProperties renderProp = new RenderTargetProperties()
            {
                DpiX = 0,
                DpiY = 0,
                MinLevel = FeatureLevel.Level_10,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };
            //Свойства области рендеринга
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = renderForm.Handle,
                PixelSize = new Size2(renderForm.Width, renderForm.Height),
                PresentOptions = PresentOptions.None
            };
            //   Создание "цели" и задание свойств сглаживания
            renderTarget = new WindowRenderTarget(Factory, renderProp, winProp);
            renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            renderTarget.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

            // Задание формата текста
            textFormatMessage = new TextFormat(writeFactory, "Calibri", 100);
            textFormatMessage.ParagraphAlignment = ParagraphAlignment.Center;
            textFormatMessage.TextAlignment = TextAlignment.Center;

            textFormatMessageSmall = new TextFormat(writeFactory, "Calibri", 50);
            textFormatMessageSmall.ParagraphAlignment = ParagraphAlignment.Center;
            textFormatMessageSmall.TextAlignment = TextAlignment.Center;

            //Создание настроей для кистей
            bp.Opacity = 0.8f;

            // Создание кистей для текста
            redBrush = new SolidColorBrush(renderTarget, Color.Red, bp);
            whiteBrush = new SolidColorBrush(renderTarget, Color.White);
            purpleBrush = new SolidColorBrush(renderTarget, Color.Purple);
            grayBrush = new SolidColorBrush(renderTarget, Color.Gray, bp);
            blueBrush = new SolidColorBrush(renderTarget, Color.DarkSlateBlue);
            blackBrush = new SolidColorBrush(renderTarget, Color.Black);
            greenBrush = new SolidColorBrush(renderTarget, Color.Green);

            ScreenSize = new RawRectangleF(0, 0, renderTarget.Size.Width, renderTarget.Size.Height);
            ScreenCenter = new Vector3(renderTarget.Size.Width / 2, renderTarget.Size.Height / 2, 0);

            _dx2d = this;
        }

        // Чтение изображения
        public Bitmap LoadBitmap(string fileName)
        {
            NativeFileStream fileStream = new NativeFileStream($@"..\..\..\Direct2dLib\App\Media\Sprites\{fileName}",
                NativeFileMode.Open, NativeFileAccess.Read);
            // Декодер формата
            BitmapDecoder decoder = new BitmapDecoder(imagingFactory, fileStream, DecodeOptions.CacheOnDemand);
            // Берем первый фрейм
            BitmapFrameDecode frame = decoder.GetFrame(0);
            // Также нужен конвертер формата 
            FormatConverter converter = new FormatConverter(imagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            // Вот теперь можно и bitmap
            Bitmap bitmap = Bitmap.FromWicBitmap(renderTarget, converter);

            // Освобождаем неуправляемые ресурсы
            Utilities.Dispose(ref fileStream);
            Utilities.Dispose(ref converter);
            Utilities.Dispose(ref frame);
            Utilities.Dispose(ref decoder);

            return bitmap;
        }

        public void Timer()
        {
            countspeed++;
        }
    }
}