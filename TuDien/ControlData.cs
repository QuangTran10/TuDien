using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDien
{
    public class ControlData
    {
        private IKeyboardMouseEvents m_Events;
        private string data;
        private TextBox txtResult;

        public ControlData(IKeyboardMouseEvents events, string data, TextBox txtResult)
        {
            m_Events = events;
            this.data = data;
            this.txtResult = txtResult;
        }

        public void SubscribeGlobal()
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;

            m_Events.MouseDragStarted += OnMouseDragStarted;
            m_Events.MouseDragFinished += OnMouseDragFinished;
        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.MouseDragStarted -= OnMouseDragStarted;
            m_Events.MouseDragFinished -= OnMouseDragFinished;
            m_Events.Dispose();
            m_Events = null;
        }

        private void OnMouseDragStarted(object sender, MouseEventArgs e)
        {
            //Log("MouseDragStarted\n");
        }
        private async void OnMouseDragFinished(object sender, MouseEventArgs e)
        {
            CancellationToken token = new CancellationToken();
            try
            {
                Clipboard.Clear();
                SendKeys.SendWait("^c");
                if (Clipboard.GetText().Equals(""))
                {
                    return;
                }
                else
                {
                    txtResult.Text = Clipboard.GetText();
                }
                //Clipboard.Clear();
            }
            catch (Exception)
            {

            }
        }

    }
}
