using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace InterestOrganiser.Extensions
{
    public static class Extensions
    {
        public static void RemoveAll(this IList list)
        {
            while (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
