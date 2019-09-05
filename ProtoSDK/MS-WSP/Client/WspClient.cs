﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// WSP client.
    /// </summary>
    public class WspClient : IDisposable
    {
        #region Constructors
        public WspClient()
        {

        }
        #endregion

        #region Fields
        public RequestSender sender;

        private IWspInMessage lastRequest;

        private byte[] lastResponseBytes;

        private CPMSetBindingsIn lastSetBindingsInMessage;

        private bool is64bitClientVersion;

        private bool is64bitServerVersion;
        #endregion

        #region Properties
        /// <summary>
        /// Indicates if we use 64-bit or 32-bit when validating responses.
        /// </summary>
        public bool Is64bit
        {
            get
            {
                return is64bitClientVersion && is64bitServerVersion;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send CPMConnectIn.
        /// </summary>
        /// <param name="clientVersion">clientVersion field to be used.</param>
        /// <param name="clientIsRemote">clientIsRemote field to be used.</param>
        /// <param name="machineName">machineName field to be used.</param>
        /// <param name="userName">userName field to be used.</param>
        /// <param name="propertySet1">propertySet1 field to be used.</param>
        /// <param name="propertySet2">propertySet2 field to be used.</param>
        /// <param name="aPropertySet">aPropertySet field to be used.</param>
        public void SendCPMConnectIn(
            UInt32 clientVersion,
            UInt32 clientIsRemote,
            string machineName,
            string userName,
            CDbPropSet propertySet1,
            CDbPropSet propertySet2,
            CDbPropSet[] aPropertySet
            )
        {
            if ((clientVersion & WspConsts.Is64bitVersion) != 0)
            {
                is64bitClientVersion = true;
            }
            else
            {
                is64bitClientVersion = false;
            }

            var request = new CPMConnectIn()
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMConnectIn,
                },
                _iClientVersion = clientVersion,
                _fClientIsRemote = clientIsRemote,
                MachineName = machineName,
                UserName = userName,
                cPropSets = 2,
                PropertySet1 = propertySet1,
                PropertySet2 = propertySet2,
                cExtPropSet = (UInt32)aPropertySet.Length,
                aPropertySets = aPropertySet,
            };

            Send(request);
        }

        /// <summary>
        /// Send CPMCreateQueryIn.
        /// </summary>
        /// <param name="columnSet">columnSet field to be used.</param>
        /// <param name="restrictionArray">restrictionArray field to be used.</param>
        /// <param name="sortSet">sortSet field to be used.</param>
        /// <param name="categorizationSet">categorizationSet field to be used.</param>
        /// <param name="rowsetProperties">rowsetProperties field to be used.</param>
        /// <param name="pidMapper">pidMapper field to be used.</param>
        /// <param name="columnGroupArray">columnGroupArray field to be used.</param>
        /// <param name="lcid">lcid field to be used.</param>
        public void SendCPMCreateQueryIn(
            CColumnSet? columnSet,
            CRestrictionArray? restrictionArray,
            CInGroupSortAggregSets? sortSet,
            CCategorizationSet? categorizationSet,
            CRowsetProperties rowsetProperties,
            CPidMapper pidMapper,
            CColumnGroupArray columnGroupArray,
            UInt32 lcid
            )
        {
            var request = new CPMCreateQueryIn
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMCreateQueryIn,
                },
                ColumnSet = columnSet,
                RestrictionArray = restrictionArray,
                SortSet = sortSet,
                CCategorizationSet = categorizationSet,
                RowSetProperties = rowsetProperties,
                PidMapper = pidMapper,
                GroupArray = columnGroupArray,
                Lcid = lcid,
            };

            Send(request);
        }

        /// <summary>
        /// Send CPMSetBindingsIn.
        /// </summary>
        /// <param name="_hCursor">_hCursor field to be used.</param>
        /// <param name="_cbRow">_cbRow field to be used.</param>
        /// <param name="_cbBindingDesc">_cbBindingDesc field to be used.</param>
        /// <param name="_dummy">_dummy field to be used.</param>
        /// <param name="cColumns">cColumns field to be used.</param>
        /// <param name="aColumns">aColumns field to be used.</param>
        public void SendCPMSetBindingsIn(
            uint _hCursor,
            uint _cbRow,
            uint _cbBindingDesc,
            uint _dummy,
            uint cColumns,
            CTableColumn[] aColumns
            )
        {
            var request = new CPMSetBindingsIn
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMSetBindingsIn,
                },
                _hCursor = _hCursor,
                _cbRow = _cbRow,
                _cbBindingDesc = _cbBindingDesc,
                _dummy = _dummy,
                cColumns = cColumns,
                aColumns = aColumns,
            };

            lastSetBindingsInMessage = request;

            Send(request);
        }

        /// <summary>
        /// Send CPMGetRowsIn.
        /// </summary>
        /// <param name="_hCursor">_hCursor field to be used.</param>
        /// <param name="_cRowsToTransfer">_cRowsToTransfer field to be used.</param>
        /// <param name="_cbRowWidth">_cbRowWidth field to be used.</param>
        /// <param name="_cbSeek">_cbSeek field to be used.</param>
        /// <param name="_cbReserved">_cbReserved field to be used.</param>
        /// <param name="_cbReadBuffer">_cbReadBuffer field to be used.</param>
        /// <param name="_ulClientBase">_ulClientBase field to be used.</param>
        /// <param name="_fBwdFetch">_fBwdFetch field to be used.</param>
        /// <param name="eType">eType field to be used.</param>
        /// <param name="_chapt">_chapt field to be used.</param>
        /// <param name="seekDescription">SeekDescription field to be used.</param>
        public void SendCPMGetRowsIn(
            uint _hCursor,
            uint _cRowsToTransfer,
            uint _cbRowWidth,
            uint _cbSeek,
            uint _cbReserved,
            uint _cbReadBuffer,
            uint _ulClientBase,
            uint _fBwdFetch,
            eType_Values eType,
            uint _chapt,
            object seekDescription
            )
        {
            var request = new CPMGetRowsIn
            {
                Header = new WspMessageHeader
                {
                    _msg = WspMessageHeader_msg_Values.CPMGetRowsIn,
                },
                _hCursor = _hCursor,
                _cRowsToTransfer = _cRowsToTransfer,
                _cbRowWidth = _cbRowWidth,
                _cbSeek = _cbSeek,
                _cbReserved = _cbReserved,
                _cbReadBuffer = _cbReadBuffer,
                _ulClientBase = _ulClientBase,
                _fBwdFetch = _fBwdFetch,
                eType = eType,
                _chapt = _chapt,
                SeekDescription = seekDescription,
            };

            Send(request);
        }

        /// <summary>
        /// Send message to server.
        /// </summary>
        /// <param name="request">Message to be sent.</param>
        public void Send(IWspInMessage request)
        {
            lastRequest = request;

            sender.SendMessage(Helper.ToBytes(request), out lastResponseBytes);
        }

        /// <summary>
        /// Expect reponse from server.
        /// </summary>
        /// <typeparam name="T">Response type.</typeparam>
        /// <param name="response">Response from server.</param>
        /// <returns>Status</returns>
        public UInt32 ExpectMessage<T>(out T response) where T : struct, IWspOutMessage
        {
            response = new T();

            response.Request = lastRequest;

            var header = new WspMessageHeader();

            Helper.FromBytes(ref header, lastResponseBytes);

            if (header._msg != lastRequest.Header._msg)
            {
                throw new InvalidOperationException("Unexpected response from server!");
            }

            if (header._status != 0 && header._status != (uint)WspErrorCode.DB_S_ENDOFROWSET)
            {
                response.Header = header;
                return header._status;
            }

            if (response is CPMGetRowsOut getRowsOut)
            {
                getRowsOut.Is64Bit = Is64bit;
                getRowsOut.BindingRequest = lastSetBindingsInMessage;
            }

            var buffer = new WspBuffer(lastResponseBytes);

            response.FromBytes(buffer);

            // Update the state of client according to response.
            UpdateContext(response);

            return 0;
        }
        #endregion

        private void UpdateContext(IWspOutMessage response)
        {
            switch (response.Header._msg)
            {
                case WspMessageHeader_msg_Values.CPMConnectOut:
                    {
                        var connectOutMessage = (CPMConnectOut)response;
                        if ((connectOutMessage._serverVersion & WspConsts.Is64bitVersion) != 0)
                        {
                            is64bitServerVersion = true;
                        }
                        else
                        {
                            is64bitServerVersion = false;
                        }
                    }
                    break;
            }
        }


        public void Dispose()
        {

        }       
    }
}