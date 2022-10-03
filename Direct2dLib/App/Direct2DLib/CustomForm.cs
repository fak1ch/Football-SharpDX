using Direct2dLib.App.CustomUnity;
using SharpDX.Windows;
using System;
using System.Windows.Forms;

namespace Direct2dLib
{
    public class CustomForm
    {
        public event Action OnFormClosed;

        private RenderForm renderForm;
        public RenderForm RenderForm { get => renderForm; }

        private SceneRoot rendererScene;

        public CustomForm()
        {
            renderForm = new RenderForm("Football");
            renderForm.WindowState = FormWindowState.Normal;
            renderForm.FormBorderStyle = FormBorderStyle.None;
            renderForm.WindowState = FormWindowState.Maximized;

            renderForm.FormClosed += FormClosed;
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            renderForm.FormClosed -= FormClosed;
            OnFormClosed?.Invoke();
        }

        public void Initialize()
        {
            DX2D dx2d = new DX2D(renderForm);
            Input dInput = new Input(renderForm);
            CollisionHandler collisionHandler = new CollisionHandler();
            rendererScene = new SceneRoot();
            RenderLoop.Run(renderForm, Update);
        }

        public void Update()
        {
            rendererScene.Update();
        }
    }
}
