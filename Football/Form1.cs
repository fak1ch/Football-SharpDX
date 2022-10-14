using Direct2dLib;
using System;
using System.Windows.Forms;

namespace Football
{
    public partial class Football : Form
    {
        private CustomForm _form;

        public Football()
        {
            InitializeComponent();

            Controls.SetChildIndex(button1, 0);
            Controls.SetChildIndex(pictureBox1, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _form = new CustomForm();
            _form.Initialize();
            _form.OnFormClosed += CustomFormClosed;

            Close();
        }

        private void CustomFormClosed()
        {
            Close();
        }
    }
}
