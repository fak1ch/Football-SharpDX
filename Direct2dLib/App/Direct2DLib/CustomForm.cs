using Direct2dLib.App.CustomUnity;
using Direct2dLib.App.CustomUnity.Scenes;
using SharpDX.Windows;
using System;
using System.Windows.Forms;

namespace Direct2dLib
{
    public class CustomForm
    {
        public event Action OnFormClosed;

        private RenderForm _renderForm;
        private SceneManager _sceneManager;


        public CustomForm()
        {
            _renderForm = new RenderForm("Football");
            _renderForm.FormBorderStyle = FormBorderStyle.None;
            _renderForm.WindowState = FormWindowState.Maximized;
            _renderForm.Width = 1360 / 2;
            _renderForm.Height = 768;

            _renderForm.FormClosed += FormClosed;
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            _renderForm.FormClosed -= FormClosed;
            OnFormClosed?.Invoke();
        }

        public void Initialize()
        {
            DX2D dx2d = new DX2D(_renderForm);
            Input dInput = new Input(_renderForm);
            CollisionHandler collisionHandler = new CollisionHandler();
            _sceneManager = new SceneManager();
            RenderLoop.Run(_renderForm, Update);
        }

        public void Update()
        {
            _sceneManager.ActiveScene.Update();
        }
    }
}
