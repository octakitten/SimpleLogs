using Dalamud.Game;
using Dalamud.Game.Network;
using Dalamud.Plugin;
using System;
using System.Runtime.InteropServices;
using Dalamud.Logging;
using System.IO.Compression;
using SimpleLogs;

namespace SimpleLogs
{
    public class Network
    {
        private Plugin plugin;
        private Timer timer;

        public Network(Plugin plugin, Timer tmr)
        {
            timer = new Timer();
            this.plugin = plugin;
            this.timer = tmr;

            // Subscribe to the network events
        }

        private void OnNetworkMessage(IntPtr dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
        {
            // Check if the message is a combat event from the server
            if (IsCombatEvent(opCode, direction))
            {
                // Parse the packet
                var packet = Marshal.PtrToStructure<Packet>(dataPtr);

                // Handle the packet
                HandlePacket(packet);
            }

            // here we're going to handle all packets if the switch "network_testing" is true
            if (plugin.Configuration.network_testing)
            {
                // Parse the packet
                var packet = Marshal.PtrToStructure<TestPacket>(dataPtr);

                // Handle the packet
                HandlePacketTesting(packet);
            }
            return;
        }

        private void HandlePacket(Packet packet)
        {
            // create the structs we need to decode the packet
            PacketHeader header = new PacketHeader();
            PacketSegmentHeader segmentHeader = new PacketSegmentHeader();
            PacketIPCHeader ipcHeader = new PacketIPCHeader();
            PacketPayload payload = new PacketPayload();
            PacketPayloadDecomp payloadDecomp = new PacketPayloadDecomp();
            PacketDecode packetDecode = new PacketDecode();


            // try block here
            try
            {
                // decode the packet

                packetDecode.Header = header;
                packetDecode.SegmentHeader = segmentHeader;
                packetDecode.IPCHeader = ipcHeader;
                packetDecode.Payload = payload;

                if (header.IsCompressed == 1)
                {
                    var decomp = DecompressPayload(payload.Data);
                    packetDecode.PayloadDecomp = payloadDecomp;
                }
            }
            catch (Exception e)
            {
                // if we get an exception, log it and return
                PluginLog.LogError(e.ToString());
                return;
            }
            



        }

        private void HandlePacketTesting(TestPacket packet)
        {
            var timestamp = timer.GetElapsedTime();
            


            return;
        }


        // implement the IsCombatEvent method
        private bool IsCombatEvent(ushort opCode, NetworkMessageDirection direction)
        {

            // if its from the server, go forward with the packet
            if (direction == NetworkMessageDirection.ZoneUp)
            {
                // TODO figure out opCodes for combat events
            }
            {
                // TODO figure out opCodes for combat events
            }

            return false;
        }

        // decompress packet payload here
        private System.IO.Stream DecompressPayload(byte[] payload)
        {
            System.IO.Stream stream = new System.IO.MemoryStream(payload);
            var decomp_data = ZLibStream.Synchronized(stream);
            var archive = new ZipArchive(decomp_data);
            archive.Entries[0].Open().CopyTo(decomp_data);
            return decomp_data;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Packet
        {
            public ushort Length;
            public byte[] Data;
        }

        // if the instructions laid out in the document i looked at are right, then these headers should work out just fine.
        // otherwise, we'll need to fix things
        private struct PacketHeader
        {
            public ulong Magic;
            public ulong Magic2;
            public ulong Timestamp;
            public uint Size01;
            public ushort ConnectionType;
            public ushort Unknown01;
            public ushort IsCompressed;
            public uint Unknown02;
            public uint Size02;
        }

        private struct PacketSegmentHeader
        {
            public uint SourceActor;
            public uint TargetActor;
            public ushort Type02;
            public ushort Padding;
        }
        
        private struct PacketIPCHeader
        {
            public ushort Reserved;
            public ushort Type03;
            public ushort Padding01;
            public ushort ServerId;
            public uint Timestamp;
            public uint Padding02;
        }

        private struct PacketPayload
        {
            public byte[] Data;
        }

        private struct PacketPayloadDecomp
        {
            public byte[] Data;
        }

        private struct PacketDecode
        {
            public PacketHeader Header;
            public PacketSegmentHeader SegmentHeader;
            public PacketIPCHeader IPCHeader;
            public PacketPayload Payload;
            public PacketPayloadDecomp PayloadDecomp;
        }

        private struct TestPacket
        {
            public ushort Length;
            public byte[] Data;
            public ushort opCode;
            public uint sourceActorId;
            public uint targetActorId;
            public NetworkMessageDirection direction;
        }
    }
}