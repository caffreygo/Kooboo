﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Dom.CSS.Tokens
{
  public  class string_token : cssToken
    {

      public string_token()
      {
          this.Type = enumTokenType.String;
      }

      public string value;
    }
}
