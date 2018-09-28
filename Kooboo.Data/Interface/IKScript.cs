﻿using Kooboo.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Data.Interface
{
    public interface IkScript
    { 
        string Name { get;  }
         
        RenderContext context { get; set; }
    }
}
