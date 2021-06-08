﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocketIOClient.EventArguments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocketIOClient.Test.SocketIOTests
{
    public abstract class OnReceivedEventTest : SocketIOTest
    {
        public virtual async Task Test()
        {
            ReceivedEventArgs args = null;
            var client = new SocketIO(Url, new SocketIOOptions
            {
                Reconnection = false,
                Query = new Dictionary<string, string>
                {
                    { "token", Version }
                }
            });
            client.OnConnected += async (sender, e) =>
            {
                await client.EmitAsync("hi", "unit test");
            };
            client.OnReceivedEvent += (sender, e) => args = e;
            await client.ConnectAsync();
            await Task.Delay(200);
            await client.DisconnectAsync();

            Assert.AreEqual("hi", args.Event);
            Assert.AreEqual($"{Prefix}unit test", args.Response.GetValue<string>());
        }
    }
}
