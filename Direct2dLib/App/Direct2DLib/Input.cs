using System;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace Direct2dLib
{
    public class Input
    {
        private static Input _dInput;
        public static Input Instance => _dInput;

        // Экземпляр объекта "прямого ввода"
        private DirectInput directInput;

        // Поля и свойства, связанные с клавиатурой
        private Keyboard keyboard;
        private KeyboardState keyboardState;
        public KeyboardState KeyboardState { get => keyboardState; }
        private bool keyboardUpdated;
        public bool KeyboardUpdated { get => keyboardUpdated; }
        private bool keyboardAcquired;

        // Поля и свойства, связанные с грызуном
        private Mouse mouse;
        private MouseState mouseState;
        public MouseState MouseState { get => mouseState; }
        private bool mouseUpdated;
        public bool MouseUpdated { get => mouseUpdated; }
        private bool mouseAcquired;

        // В конструкторе создаем все объекты и пробуем получить доступ к устройствам
        public Input(RenderForm renderForm)
        {
            directInput = new DirectInput();

            keyboard = new Keyboard(directInput);
            keyboard.Properties.BufferSize = 16;
            keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireKeyboard();
            keyboardState = new KeyboardState();

            mouse = new Mouse(directInput);
            mouse.Properties.AxisMode = DeviceAxisMode.Relative;
            mouse.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireMouse();
            mouseState = new MouseState();

            _dInput = this;
        }

        // Получение доступа к клавиатуре
        private void AcquireKeyboard()
        {
            try
            {
                keyboard.Acquire();
                keyboardAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    keyboardAcquired = false;
            }
        }

        // Получение доступа к грызуну
        private void AcquireMouse()
        {
            try
            {
                mouse.Acquire();
                mouseAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    mouseAcquired = false;
            }
        }

        // Обновление состояния клавиатуры
        public void UpdateKeyboardState()
        {
            // Если доступ не был получен, пробуем здесь
            if (!keyboardAcquired) AcquireKeyboard();

            // Пробуем обновить состояние
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                keyboard.GetCurrentState(ref keyboardState);
                // Успех
                keyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                // Отказ
                keyboardUpdated = false;
            }

            // В большинстве случаев отказ из-за потери фокуса ввода
            // Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                keyboardAcquired = false;
        }

        // Обновление состояния грызуна
        public void UpdateMouseState()
        {
            // Если доступ не был получен, пробуем здесь
            if (!mouseAcquired) AcquireMouse();

            // Пробуем обновить состояние
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                mouse.GetCurrentState(ref mouseState);
                // Успех
                mouseUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                // Отказ
                mouseUpdated = false;
            }
            // В большинстве случаев отказ из-за потери фокуса ввода
            // Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                mouseAcquired = false;
        }

        public bool GetKey(Key key)
        {
            return KeyboardState.IsPressed(key);
        }

        public bool GetMouseClicked()
        {
            return mouseState.Buttons[0];
        }
    }
}