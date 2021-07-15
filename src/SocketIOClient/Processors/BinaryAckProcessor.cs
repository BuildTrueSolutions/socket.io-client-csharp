﻿using System.Linq;
using System.Text.Json;

namespace SocketIOClient.Processors
{
    public class BinaryAckProcessor : Processor
    {
        public override void Process(MessageContext ctx)
        {
            int index = ctx.Message.IndexOf('-');
            if (index > 0)
            {
                if (int.TryParse(ctx.Message.Substring(0, index), out int totalCount))
                {
                    ctx.Message = ctx.Message.Substring(index + 1);
                    if (!string.IsNullOrEmpty(ctx.Namespace))
                    {
                        ctx.Message = ctx.Message.Substring(ctx.Namespace.Length + 1);
                    }
                    int packetIndex = ctx.Message.IndexOf('[');
                    if (int.TryParse(ctx.Message.Substring(0, packetIndex), out int packetId))
                    {
                        string data = ctx.Message.Substring(packetIndex);
                        var doc = JsonDocument.Parse(data);
                        var array = doc.RootElement.EnumerateArray().ToList();
                        ctx.BinaryAckHandler(packetId, totalCount, array);
                    }
                }
            }
        }
    }
}
