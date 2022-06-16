using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDien
{
    public class KeyItem
    {
        public static IReadOnlyList<KeyItem> List { get; } = CreateList();

        private static IReadOnlyList<KeyItem> CreateList()
        {
            String[] names = Enum.GetNames(typeof(System.Windows.Forms.Keys));
            int[] values = (int[])Enum.GetValues(typeof(System.Windows.Forms.Keys));

            return Enumerable
                .Range(0, names.Length)
                .Select(i => new KeyItem(values[i], names[i]))
                .ToList();
        }

        private KeyItem(int key, String name)
        {
            this.KeyCode = key;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public static string find(int keycode)
        {
            string name="";
            foreach (var item in List)
            {
                if (keycode == item.KeyCode)
                {
                    name = item.Name;
                }  
            }
            return name;
        }

        public static int findCode(string name)
        {
            int keycode=0;
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            foreach (var item in List)
            {
                if (item.Name.Equals(name))
                {
                    keycode = item.KeyCode;
                }
            }

            return keycode;
        }

        public int KeyCode { get; }
        public String Name { get; }

    }
}
