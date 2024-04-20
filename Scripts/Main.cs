using Godot;
using Godot.Collections;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Api.Core.Enums;
using TwitchLib.Api.Helix;

namespace Koto.Scripts
{
    public partial class Main : Node2D
    {
        public async override void _Ready()
        {
            await OAuthFlow.Main();
            await WebsocketThing.Main(null);
        }
    }
}