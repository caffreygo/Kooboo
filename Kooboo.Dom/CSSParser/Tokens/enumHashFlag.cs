﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Dom.CSS.Tokens
{
    /// <summary>
    /// hash tokens have a type flag set to either "id" or "unrestricted". The type flag defaults to "unrestricted" if not otherwise set.
    /// </summary>
 public   enum enumHashFlag
    {
        unrestricted=0,
        id =1
    }
}
