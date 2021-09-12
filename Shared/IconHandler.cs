using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Shared
{
    class IconHandler
    {
       public BitmapImage MainIcon()
        {
            var directory = Environment.CurrentDirectory;
            return new BitmapImage(new Uri(directory + @"\Icon.png"));
        }
    }
}
