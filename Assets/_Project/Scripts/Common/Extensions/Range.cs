// 
//  Mirosoft LLC CONFIDENTIAL
//  Copyright (C) 2016 Mirosoft LLC
//  All Rights Reserved.
//  
//  NOTICE:  All information contained herein is, and remains the property
//  of Mirofost LLC and its suppliers, if any. The intellectual and 
//  technical concepts contained herein are proprietary to Mirofost LLC 
//  and its suppliers and may be covered by U.S. and Foreign Patents,
//  patents in process, and are protected by trade secret or copyright law.
//  Dissemination of this information or reproduction of this material
//  is strictly forbidden unless prior written permission is obtained
//  from Mirosoft LLC.
// 	
using System;

namespace Terminus.Core.Helper
{
    public struct Range<T>
    {
        public readonly T Min;
        public readonly T Max;

        public Range(T value)
        {
            Min = Max = value;
        }

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public override String ToString()
        {
            if (Equals(Min, Max))
            {
                return $"{Min}";
            }
            else
            {
                return $"{Min}..{Max}";
            }
        }
    }
}