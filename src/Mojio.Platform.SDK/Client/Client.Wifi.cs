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

using Mojio.Platform.SDK.Contracts;
using Mojio.Platform.SDK.Contracts.Client;
using Mojio.Platform.SDK.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mojio.Platform.SDK
{
    public partial class Client : IClientWifi, IClientCapabilities
    {
        public async Task<IPlatformResponse<IUpdateWifiSettingsStatus>> UpdateWifiSettings(Guid mojioId, string ssid = null, string password = null, WifiRadioStatus? radioStatus = null,
            TimeSpan? timeToLive = null, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null,
            bool skipCache = false)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if (timeToLive == null || timeToLive == default(TimeSpan)) timeToLive = TimeSpan.FromSeconds(15);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                var b = new
                {
                    SSID = ssid,
                    Password = password,
                    Status = radioStatus,
                    TimeToLive = timeToLive
                };
                var body = _serializer.SerializeToString(b);

                return await _clientBuilder.Request<IUpdateWifiSettingsStatus>(ApiEndpoint.Api, $"/v2/mojios/{mojioId}/wifiradio", tokenP.CancellationToken, tokenP.Progress, HttpMethod.Put, body);
            }

            return await Task.FromResult<IPlatformResponse<IUpdateWifiSettingsStatus>>(null);
        }

        public async Task<IPlatformResponse<IUpdateWifiSettingsStatus>> GetWifiSettingsAfterUpdate(Guid mojioId, Guid transactionId, CancellationToken? cancellationToken = null,
            IProgress<ISDKProgress> progress = null, bool skipCache = false)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                return await _clientBuilder.Request<IUpdateWifiSettingsStatus>(ApiEndpoint.Api, $"/v2/mojios/{mojioId}/transactions/{transactionId}", tokenP.CancellationToken, tokenP.Progress);
            }

            return await Task.FromResult<IPlatformResponse<IUpdateWifiSettingsStatus>>(null);
        }

        public async Task<IPlatformResponse<ICapabilities>> Capabilities(Guid mojioId, CancellationToken? cancellationToken = null, IProgress<ISDKProgress> progress = null,
            bool skipCache = false)
        {
            var tokenP = IssueNewTokenAndProgressContainer(cancellationToken, progress);

            if ((await Login(Authorization, cancellationToken, progress)).Success)
            {
                return await _clientBuilder.Request<ICapabilities>(ApiEndpoint.Api, $"/v2/mojios/{mojioId}/capabilities", tokenP.CancellationToken, tokenP.Progress);
            }

            return await Task.FromResult<IPlatformResponse<ICapabilities>>(null);
        }
    }
}