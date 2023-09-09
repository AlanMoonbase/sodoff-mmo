﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sodoffmmo.Data;
internal class SocketBuffer {
    NetworkData data = new();
    Status status = Status.Header;
    byte header;
    short length;
    byte[] value = new byte[0];
    public void Write(byte[] buffer, int length) {
        data.WriteChunk(buffer, 0, length);
    }

    public bool ReadPacket(out NetworkPacket packet) {
        packet = new();
        if (status == Status.Header)
            ReadHeader();

        if (status == Status.Value) {
            ReadValue();
            if (status == Status.Header) {
                packet = new(header, value);
                return true;
            }
        }

        return false;
    }

    private void ReadHeader() {
        if (data.RemainingLength < 3)
            return;
        header = data.ReadByte();
        length = data.ReadShort();
        status = Status.Value;
    }

    private void ReadValue() {
        if (data.RemainingLength < length)
            return;
        value = data.ReadChunk(length);
        data = new(data.ReadChunk(data.RemainingLength));
        status = Status.Header;
    }

    enum Status {
        Header,
        Value
    }
}
