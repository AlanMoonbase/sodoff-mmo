﻿using sodoffmmo.Attributes;
using sodoffmmo.Core;
using sodoffmmo.Data;

namespace sodoffmmo.CommandHandlers;

[CommandHandler(7)]
class GenericMessageHandler : ICommandHandler {
    public void Handle(Client client, NetworkObject receivedObject) {
        NetworkPacket packet = NetworkObject.WrapObject(0, 7, receivedObject).Serialize();
        foreach (var roomClient in client.Room.Clients)
            roomClient.Send(packet);
    }
}
