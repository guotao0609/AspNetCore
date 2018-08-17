// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Buffers.Binary;

namespace Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2
{
    /* https://tools.ietf.org/html/rfc7540#section-6.8
        +-+-------------------------------------------------------------+
        |R|                  Last-Stream-ID (31)                        |
        +-+-------------------------------------------------------------+
        |                      Error Code (32)                          |
        +---------------------------------------------------------------+
        |                  Additional Debug Data (*)                    |
        +---------------------------------------------------------------+
    */
    public partial class Http2Frame
    {
        private const int ErrorCodeOffset = 4;

        public int GoAwayLastStreamId
        {
            get => (int)Bitshifter.ReadUInt31BigEndian(Payload);
            set => Bitshifter.WriteUInt31BigEndian(Payload, (uint)value);
        }

        public Http2ErrorCode GoAwayErrorCode
        {
            get => (Http2ErrorCode)BinaryPrimitives.ReadUInt32BigEndian(Payload.Slice(ErrorCodeOffset));
            set => BinaryPrimitives.WriteUInt32BigEndian(Payload.Slice(ErrorCodeOffset), (uint)value);
        }

        public void PrepareGoAway(int lastStreamId, Http2ErrorCode errorCode)
        {
            PayloadLength = 8;
            Type = Http2FrameType.GOAWAY;
            Flags = 0;
            StreamId = 0;
            GoAwayLastStreamId = lastStreamId;
            GoAwayErrorCode = errorCode;
        }
    }
}
