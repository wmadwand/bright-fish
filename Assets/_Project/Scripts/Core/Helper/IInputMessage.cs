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

using System;

namespace Terminus.Core.Helper
{
    public interface IInputMessage
    {
        event Action Receive;
    }

    public interface IInputMessage<T>
    {
        event Action<T> Receive;
    }

    public interface IInputMessage<T1, T2>
    {
        event Action<T1, T2> Receive;
    }

    public interface IInputMessage<T1, T2, T3>
    {
        event Action<T1, T2, T3> Receive;
    }

    public interface IInputMessage<T1, T2, T3, T4>
    {
        event Action<T1, T2, T3, T4> Receive;
    }
}