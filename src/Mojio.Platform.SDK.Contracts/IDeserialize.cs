﻿#region copyright
/******************************************************************************
* Moj.io Inc. CONFIDENTIAL
* 2017 Copyright Moj.io Inc.
* All Rights Reserved.
* 
* NOTICE:  All information contained herein is, and remains, the property of 
* Moj.io Inc. and its suppliers, if any.  The intellectual and technical 
* concepts contained herein are proprietary to Moj.io Inc. and its suppliers
* and may be covered by Patents, pending patents, and are protected by trade
* secret or copyright law.
*
* Dissemination of this information or reproduction of this material is strictly
* forbidden unless prior written permission is obtained from Moj.io Inc.
*******************************************************************************/
#endregion

using System;
using System.IO;

namespace Mojio.Platform.SDK.Contracts
{
    public interface IDeserialize
    {
        string ContentType { get; }

        T Deserialize<T>(Stream stream);

        T Deserialize<T>(byte[] input);

        object Deserialize(byte[] input);

        T Deserialize<T>(string input);

        object Deserialize(string input, Type serviceType);

        object Deserialize(string input);
    }
}