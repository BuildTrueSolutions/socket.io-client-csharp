﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SocketIOClient.Converters
{
    public static class CvtFactory
    {
        public static ICvtMessage GetByType(int eio, CvtMessageType type)
        {
            switch (type)
            {
                case CvtMessageType.Opened:
                    return new OpenedMessage();
                case CvtMessageType.Ping:
                    return new PingMessage();
                case CvtMessageType.Pong:
                    return new PongMessage();
                case CvtMessageType.Connected:
                    if (eio == 3)
                        return new Eio3ConnectedMessage();
                    return new Eio4ConnectedMessage();
                case CvtMessageType.Disconnected:
                    return new DisconnectedMessage();
                case CvtMessageType.MessageEvent:
                    return new EventMessage();
                case CvtMessageType.MessageAck:
                    return new AckMessage();
                case CvtMessageType.MessageError:
                    if (eio == 3)
                        return new Eio3ErrorMessage();
                    return new Eio4ErrorMessage();
                case CvtMessageType.MessageBinary:
                    return new BinaryMessage();
                case CvtMessageType.MessageBinaryAck:
                    return new BinaryAckMessage();
            }
            return null;
        }

        public static ICvtMessage GetMessage(int eio, string msg)
        {
            var enums = Enum.GetValues(typeof(CvtMessageType));
            foreach (CvtMessageType item in enums)
            {
                string prefix = ((int)item).ToString();
                if (msg.StartsWith(prefix))
                {
                    ICvtMessage result = GetByType(eio, item);
                    if (result != null)
                    {
                        result.Read(msg.Substring(prefix.Length));
                        return result;
                    }
                }
            }
            return null;
        }
    }
}