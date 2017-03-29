using System;
using System.Threading;
using WiimoteLib;

namespace OsuGuitar
{
    internal class WiimoteManager
    {
        internal Wiimote Wiimote;
        internal Thread WiimoteSearchThread;

        internal WiimoteManager(Action onConnect)
        {
            WiimoteSearchThread = new Thread(() =>
            {
                while (Wiimote == null)
                    try
                    {
                        findAll();
                        setup();

                        onConnect.Invoke();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No WIIMOTE of HID connected !");
                        Thread.Sleep(1000);
                    }
            });
            WiimoteSearchThread.Start();
        }

        private void findAll()
        {
            var collection = new WiimoteCollection();
            collection.FindAllWiimotes();
            Wiimote = collection[0];
        }

        private void setup()
        {
            Wiimote.Connect();
            Wiimote.SetReportType(InputReport.Buttons, true);
            Wiimote.SetReportType(InputReport.ButtonsExtension, true);
        }

        internal void Rumble(double milliseconds)
        {
            new Thread(() =>
            {
                var startTime = DateTime.Now;
                Wiimote.SetRumble(true);

                while ((DateTime.Now - startTime).TotalMilliseconds < milliseconds)
                    Thread.Sleep(1);

                Wiimote.SetRumble(false);
            }).Start();
        }

        internal bool IsButtonPressed(WiimoteButton button)
        {
            switch (button)
            {
                case (WiimoteButton.A):
                    return Wiimote.WiimoteState.ButtonState.A;
                case (WiimoteButton.B):
                    return Wiimote.WiimoteState.ButtonState.B;
                case (WiimoteButton.Left):
                    return Wiimote.WiimoteState.ButtonState.Left;
                case (WiimoteButton.Right):
                    return Wiimote.WiimoteState.ButtonState.Right;
                case (WiimoteButton.Down):
                    return Wiimote.WiimoteState.ButtonState.Down;
                case (WiimoteButton.Up):
                    return Wiimote.WiimoteState.ButtonState.Up;
                case (WiimoteButton.Home):
                    return Wiimote.WiimoteState.ButtonState.Home;
                case (WiimoteButton.Plus):
                    return Wiimote.WiimoteState.ButtonState.Plus;
                case (WiimoteButton.Minus):
                    return Wiimote.WiimoteState.ButtonState.Minus;
                case (WiimoteButton.One):
                    return Wiimote.WiimoteState.ButtonState.One;
                case (WiimoteButton.Two):
                    return Wiimote.WiimoteState.ButtonState.Two;
            }
            return false;
        }

        internal bool IsGuitarButtonPressed(GuitarButton button)
        {
            switch (button)
            {
                case (GuitarButton.Button_Minus):
                    return Wiimote.WiimoteState.GuitarState.ButtonState.Minus;
                case (GuitarButton.Button_Plus):
                    return Wiimote.WiimoteState.GuitarState.ButtonState.Minus;
                case (GuitarButton.Strum_Down):
                    return Wiimote.WiimoteState.GuitarState.ButtonState.StrumDown;
                case (GuitarButton.Strum_Up):
                    return Wiimote.WiimoteState.GuitarState.ButtonState.StrumUp;
                case (GuitarButton.Fret_Blue):
                    return Wiimote.WiimoteState.GuitarState.FretButtonState.Blue;
                case (GuitarButton.Fret_Green):
                    return Wiimote.WiimoteState.GuitarState.FretButtonState.Green;
                case (GuitarButton.Fret_Orange):
                    return Wiimote.WiimoteState.GuitarState.FretButtonState.Orange;
                case (GuitarButton.Fret_Red):
                    return Wiimote.WiimoteState.GuitarState.FretButtonState.Red;
                case (GuitarButton.Fret_Yellow):
                    return Wiimote.WiimoteState.GuitarState.FretButtonState.Yellow;
            }
            return false;
        }

        internal void OnButtonPressed(EventHandler<WiimoteChangedEventArgs> function)
        {
            Wiimote.WiimoteChanged += function;
        }

        internal float Whammy { get { return Wiimote.WiimoteState.GuitarState.WhammyBar; } }
        internal Point Joystick { get { return Wiimote.WiimoteState.GuitarState.RawJoystick; } }
    }

    internal enum WiimoteButton
    {
        A,
        B,
        Left,
        Right,
        Down,
        Up,
        Home,
        Plus,
        Minus,
        One,
        Two
    }

    internal enum GuitarButton
    {
        Button_Minus,
        Button_Plus,
        Strum_Down,
        Strum_Up,
        Fret_Blue,
        Fret_Green,
        Fret_Orange,
        Fret_Red,
        Fret_Yellow,
    }
}
