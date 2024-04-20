using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Utility;
using System.Threading.Tasks;
using System.Threading;
using TwitchLib.EventSub.Websockets;
using TwitchLib.EventSub.Websockets.Core.EventArgs;
using TwitchLib.EventSub.Websockets.Core.EventArgs.Channel;
using TwitchLib.Api;

namespace Koto.Scripts
{
    public class WebsocketHostedService : IHostedService
    {
        private readonly EventSubWebsocketClient _eventSubWebsocketClient;
        private static List<string> scopes = new List<string> { "chat:read", "whispers:read", "whispers:edit", "chat:edit", "channel:moderate" };


        public WebsocketHostedService(ILogger<WebsocketHostedService> logger, EventSubWebsocketClient eventSubWebsocketClient)
        {
            _eventSubWebsocketClient = eventSubWebsocketClient ?? throw new ArgumentNullException(nameof(eventSubWebsocketClient));
            _eventSubWebsocketClient.WebsocketConnected += OnWebsocketConnected;
            _eventSubWebsocketClient.WebsocketDisconnected += OnWebsocketDisconnected;
            _eventSubWebsocketClient.WebsocketReconnected += OnWebsocketReconnected;
            _eventSubWebsocketClient.ErrorOccurred += OnErrorOccurred;

            _eventSubWebsocketClient.ChannelFollow += OnChannelFollow;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _eventSubWebsocketClient.ConnectAsync(new (uriString: "ws://127.0.0.1:8081/ws"));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _eventSubWebsocketClient.DisconnectAsync();
        }

        private async void OnWebsocketConnected(object? sender, WebsocketConnectedArgs e)
        {
            Logger.Debug($"Websocket {_eventSubWebsocketClient.SessionId} connected!");

            Dictionary<string, string> conditions = new()
            {
                {"broadcaster_user_id", "1048304002" }, 
                {"user_id", "1053533208" }
            };

            if (!e.IsRequestedReconnect)
            {

            }
        }

        private async void OnWebsocketDisconnected(object? sender, EventArgs e)
        {
            Logger.Debug($"Websocket {_eventSubWebsocketClient.SessionId} disconnected!");

            // Don't do this in production. You should implement a better reconnect strategy with exponential backoff
            while (!await _eventSubWebsocketClient.ReconnectAsync())
            {
                Logger.Debug("Websocket reconnect failed!");
                await Task.Delay(1000);
            }
        }

        private void OnWebsocketReconnected(object? sender, EventArgs e)
        {
            Logger.Debug($"Websocket {_eventSubWebsocketClient.SessionId} reconnected");
        }

        private void OnErrorOccurred(object? sender, ErrorOccuredArgs e)
        {
            Logger.Debug($"Websocket {_eventSubWebsocketClient.SessionId} - Error occurred!");
            Logger.Debug($"Websocket {e.Exception}!");
        }

        private void OnChannelFollow(object? sender, ChannelFollowArgs e)
        {
            var eventData = e.Notification.Payload.Event;
            Logger.Debug($"{eventData.UserName} followed {eventData.BroadcasterUserName} at {eventData.FollowedAt}");
        }
    }
}