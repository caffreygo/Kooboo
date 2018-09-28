﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Kooboo.HttpServer.Sockets
{
    public interface ISocketsTrace
    {
        void ConnectionReadFin(string connectionId);

        void ConnectionWriteFin(string connectionId);

        void ConnectionError(string connectionId, Exception ex);

        void ConnectionReset(string connectionId);

        void ConnectionPause(string connectionId);

        void ConnectionResume(string connectionId);

        void LogError(string message, Exception ex);
    }
}
