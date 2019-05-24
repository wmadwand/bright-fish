// Mirosoft LLC CONFIDENTIAL
// Copyright (C) 2017 Mirosoft LLC
// All Rights Reserved.
// 
// NOTICE:  All information contained herein is, and remains the property
// of Mirosoft LLC and its suppliers, if any. The intellectual and 
// technical concepts contained herein are proprietary to Mirosoft LLC 
// and its suppliers and may be covered by U.S. and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from Mirosoft LLC.

namespace Terminus.Core.Helper
{
    public interface IOutputMessage
    {
        void Send();
    }

    public interface IOutputMessage<T>
    {
        void Send(T arg);
    }

    public interface IOutputMessage<T1, T2>
    {
        void Send(T1 arg1, T2 arg2);
    }

    public interface IOutputMessage<T1, T2, T3>
    {
        void Send(T1 arg1, T2 arg2, T3 arg3);
    }

    public interface IOutputMessage<T1, T2, T3, T4>
    {
        void Send(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    }
}