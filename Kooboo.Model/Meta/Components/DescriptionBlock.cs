﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Model.Meta
{
    public class Description
    {
        public Localizable Title { get; set; }

        public Localizable Content { get; set; }

        public bool Closable { get; set; }
    }
}