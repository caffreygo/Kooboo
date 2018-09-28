﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Mail.Helper
{
  public static  class IDHelper
    {

        public static  int ToInt(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return 0; 
            } 
            var id =  Lib.Security.Hash.ComputeInt(input);  
            if (id <0 )
            {
                return 0 - id; 
            }
            
            if (id ==0)
            {
                return ToInt(input + "0"); 
            }
            return id; 
        }
    }
}
