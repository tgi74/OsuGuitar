using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WiimoteLib;

namespace OsuGuitar
{
    public partial class MainForm : Form
    {
        private List<Keys> _oldKeys = new List<Keys>();
        internal List<Keys> keys = new List<Keys>();
        internal KeyData KeyData = new KeyData();

        internal bool UseStrum = false;
        internal WiimoteManager Manager;
        internal Thread InputThread;

        public MainForm()
        {
            InitializeComponent();
            KeyPreview = true;
            Manager = new WiimoteManager(delegate ()
            {
                Manager.OnButtonPressed(OnChange);


                (InputThread = new Thread(inputThread)).Start();
            });
        }

        internal void OnChange(object sender, WiimoteChangedEventArgs arg)
        {
            List<Keys> keys = new List<Keys>();

            if (!UseStrum || (Manager.IsGuitarButtonPressed(GuitarButton.Strum_Down) || Manager.IsGuitarButtonPressed(GuitarButton.Strum_Up)))
            {
                if (Manager.IsGuitarButtonPressed(GuitarButton.Fret_Green))
                    keys.Add(KeyData.Key1);
                if (Manager.IsGuitarButtonPressed(GuitarButton.Fret_Red))
                    keys.Add(KeyData.Key2);
                if (Manager.IsGuitarButtonPressed(GuitarButton.Fret_Yellow))
                    keys.Add(KeyData.Key3);
                if (Manager.IsGuitarButtonPressed(GuitarButton.Fret_Blue))
                    keys.Add(KeyData.Key4);
                if (Manager.IsGuitarButtonPressed(GuitarButton.Fret_Orange))
                    keys.Add(KeyData.Key5);
            }

            this.keys = keys;
        }

        private void inputThread()
        {
            while (true)
            {
                Thread.Sleep(1);

                List<Keys> keys = this.keys; // Buffer to prevent modifications
                if ((keys.Count != _oldKeys.Count && keys.Count == 0) || keys.Except(_oldKeys).Any())
                {
                    _oldKeys = keys;

                    OSLink.UpdateInputs(_oldKeys);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            KeyData.ApplyKey(e.KeyCode);

            button1.Text = KeyData.Key1.ToString();
            button2.Text = KeyData.Key2.ToString();
            button3.Text = KeyData.Key3.ToString();
            button4.Text = KeyData.Key4.ToString();
            button5.Text = KeyData.Key5.ToString();
        }

        protected override bool IsInputKey(Keys keyData)
        {
            KeyData.ApplyKey(keyData);

            button1.Text = KeyData.Key1.ToString();
            button2.Text = KeyData.Key2.ToString();
            button3.Text = KeyData.Key3.ToString();
            button4.Text = KeyData.Key4.ToString();
            button5.Text = KeyData.Key5.ToString();

            return base.IsInputKey(keyData);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            InputThread.Abort();
            Manager.WiimoteSearchThread.Abort();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KeyData.ModifyingKey = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            KeyData.ModifyingKey = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            KeyData.ModifyingKey = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            KeyData.ModifyingKey = 4;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            KeyData.ModifyingKey = 5;
        }

        private void strumCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UseStrum = strumCheckBox.Checked;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            statusLabel.Text = Manager.Wiimote == null ? "No Wiimotes, Searching..." : "Ready !";
        }
    }
}
