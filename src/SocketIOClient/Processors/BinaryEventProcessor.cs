﻿using System.Linq;
using System.Text.Json;

namespace SocketIOClient.Processors
{
    public class BinaryEventProcessor : Processor
    {
        public override void Process(MessageContext ctx)
        {
            int index = ctx.Message.IndexOf('-');
            if (index > 0)
            {
                if (int.TryParse(ctx.Message.Substring(0, index), out int totalCount))
                {
                    ctx.Message = ctx.Message.Substring(index + 1);
                    if (!string.IsNullOrEmpty(ctx.Namespace) && ctx.Message.StartsWith(ctx.Namespace + ','))
                    {
                        ctx.Message = ctx.Message.Substring(ctx.Namespace.Length+1);
                    }
                    int packetIndex = ctx.Message.IndexOf('[');
                    string id = null;
                    if (packetIndex > 0)
                    {
                        id = ctx.Message.Substring(0, packetIndex);
                        ctx.Message = ctx.Message.Substring(packetIndex);
                    }
                    var doc = JsonDocument.Parse(ctx.Message);
                    var array = doc.RootElement.EnumerateArray().ToList();
                    string eventName = array[0].GetString();
                    array.RemoveAt(0);
                    int.TryParse(id, out int packetId);
                    ctx.BinaryReceivedHandler(packetId, totalCount, eventName, array);
                }
            }
        }
    }
}
