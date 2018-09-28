﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.IndexedDB.Btree.Comparer
{
    /// <summary>
    /// GUID has not bigger or smaller value, as long as it is constant, it is ok. 
    /// </summary>
  public  class GuidComparer : IComparer<byte[]>
    {
      private int len = 16;

        public int Compare(byte[] x, byte[] y)
        {
            for (int i = 0; i < len; i++)
            {
                if (x[i] == y[i])
                {
                    continue;
                }
                else if (x[i] > y[i])
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            } 
            return 0;
        }
    }
}
