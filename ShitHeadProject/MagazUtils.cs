using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShitHeadProject
{
    public class MagazUtils
    {

        public static void ResetToFalse(bool[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = false;
            }
        }

    }
}
