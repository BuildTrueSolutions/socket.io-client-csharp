﻿using System.Text;

namespace SocketIOClient.Converters
{
    public class Eio3ConnectedMessage : ICvtMessage
    {
        public CvtMessageType Type => CvtMessageType.Connected;

        public string QueryString { get; set; }

        public string Namespace { get; set; }

        public void Read(string msg)
        {
            Namespace = msg.TrimEnd(',');
        }

        public string Write()
        {
            var builder = new StringBuilder("40");
            if (!string.IsNullOrEmpty(Namespace))
            {
                builder.Append(Namespace);
            }
            if (!string.IsNullOrEmpty(QueryString))
            {
                if (QueryString[0] != '?')
                {
                    builder.Append('?');
                }
                builder.Append(QueryString);
            }
            return builder.ToString();
        }
    }
}