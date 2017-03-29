using System.Windows.Forms;

namespace OsuGuitar
{
    class KeyData
    {
        internal Keys Key1 = Keys.D;
        internal Keys Key2 = Keys.F;
        internal Keys Key3 = Keys.H;
        internal Keys Key4 = Keys.J;
        internal Keys Key5 = Keys.Space;

        internal int ModifyingKey = -1;

        internal void ApplyKey(Keys k)
        {
            if (ModifyingKey == -1) return;

            switch(ModifyingKey)
            {
                case 1:
                    Key1 = k;
                    break;
                case 2:
                    Key2 = k;
                    break;
                case 3:
                    Key3 = k;
                    break;
                case 4:
                    Key4 = k;
                    break;
                case 5:
                    Key5 = k;
                    break;
            }

            ModifyingKey = -1;
        }
    }
}
