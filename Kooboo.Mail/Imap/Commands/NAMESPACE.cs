﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.Mail.Imap.Commands
{
    public class NAMESPACE : ICommand
    {
        public string AdditionalResponse
        { get; set; }

        public string CommandName
        {
            get
            {
                return "NAMESPACE"; 
            }
        }

        public bool RequireAuth
        {
            get
            {
                return false; 
            }
        }

        public bool RequireFolder
        {
            get
            {
                return false; 
            }
        }

        public bool RequireTwoPartCommand
        {
            get
            {
                return false; 
            }
        }

        public Task<List<ImapResponse>> Execute(ImapSession session, string args)
        {
            return this.NullResult();
        }
    }
}
