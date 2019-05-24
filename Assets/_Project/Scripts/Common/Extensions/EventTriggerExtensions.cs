// Mirosoft LLC CONFIDENTIAL
// Copyright (C) 2018 Mirosoft LLC
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

using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Terminus.Extensions
{
    public static class EventTriggerExtensions
    {
        public static void On(this EventTrigger eventTrigger,
            EventTriggerType eventTriggerType,
            UnityAction<BaseEventData> action)
        {
            var entry = eventTrigger.triggers.Find(t => t.eventID == eventTriggerType);
            if (entry == null)
            {
                entry = new EventTrigger.Entry {eventID = eventTriggerType};

                eventTrigger.triggers.Add(entry);
            }

            entry.callback.AddListener(action);
        }

        public static void Clear(this EventTrigger eventTrigger,
            EventTriggerType eventTriggerType)
        {
            var index = 0;
            var triggers = eventTrigger.triggers;
            while (index < triggers.Count)
            {
                var entry = triggers[index];
                if (entry.eventID == eventTriggerType)
                {
                    triggers.RemoveAt(index);
                }
                else
                {
                    index++;
                }
            }
        }
    }
}