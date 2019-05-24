// // Mirosoft LLC CONFIDENTIAL
// // Copyright (C) 2016 Mirosoft LLC
// // All Rights Reserved.
// // 
// // NOTICE:  All information contained herein is, and remains the property
// // of Mirosoft LLC and its suppliers, if any. The intellectual and 
// // technical concepts contained herein are proprietary to Mirosoft LLC 
// // and its suppliers and may be covered by U.S. and Foreign Patents,
// // patents in process, and are protected by trade secret or copyright law.
// // Dissemination of this information or reproduction of this material
// // is strictly forbidden unless prior written permission is obtained
// // from Mirosoft LLC.

using System;
using Terminus.Extensions;

using UnityEngineInternal.Input;

namespace Terminus.Core.Helper
{
    public sealed class Message : IInputMessage, IOutputMessage
    {
        public event Action Receive;

        public void Send()
        {
            Receive.Call();
        }
    }

    public sealed class Message<T> : IInputMessage<T>, IOutputMessage<T>
    {
        public event Action<T> Receive;

        public void Send(T arg)
        {
            Receive.Call(arg);
        }
    }

    public sealed class Message<T0, T1> : IInputMessage<T0, T1>, IOutputMessage<T0, T1>
    {
        public event Action<T0, T1> Receive;

        public void Send(T0 arg0, T1 arg1)
        {
            Receive.Call(arg0, arg1);
        }
    }

    public sealed class Message<T0, T1, T2> : IInputMessage<T0, T1, T2>, IOutputMessage<T0, T1, T2>
    {
        public event Action<T0, T1, T2> Receive;

        public void Send(T0 arg0, T1 arg1, T2 arg2)
        {
            Receive.Call(arg0, arg1, arg2);
        }
    }

    public sealed class Message<T0, T1, T2, T3> : IInputMessage<T0, T1, T2, T3>, IOutputMessage<T0, T1, T2, T3>
    {
        public event Action<T0, T1, T2, T3> Receive;

        public void Send(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
        {
            Receive.Call(arg0, arg1, arg2, arg3);
        }
    }

    public sealed class MessagePromise
        : IMessagePromise
    {
        public bool IsReceived { get; private set; }

        private IInputMessage _message;
        
        public MessagePromise(IInputMessage message)
        {
            _message = message;
            _message.Receive += OnReceive;
        }

        public void Reset()
        {
            IsReceived = false;
        }

        private void OnReceive()
        {
            IsReceived = true;
            _message.Receive -= OnReceive;
        }
    }
    
    public sealed class MessagePromise<T>
        : IMessagePromise
    {
        public bool IsReceived { get; private set; }
        
        public T Result { get; private set; }

        private IInputMessage<T> _message;
        
        public MessagePromise(IInputMessage<T> message)
        {
            _message = message;
            _message.Receive += OnReceive;
        }
        
        public void Reset()
        {
            IsReceived = false;
        }

        private void OnReceive(T result)
        {
            IsReceived = true;
            Result = result;
            _message.Receive -= OnReceive;
        }
    }

    public interface IMessagePromise
    {
        bool IsReceived { get; }
    }

    public static class MessagePromiseExtensions
    {
        public static MessagePromise GetPromise(this IInputMessage inputMessage)
        {
            return new MessagePromise(inputMessage);
        }
        
        public static MessagePromise<T> GetPromise<T>(this IInputMessage<T> inputMessage)
        {
            return new MessagePromise<T>(inputMessage);
        }
    }
}