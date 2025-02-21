using Dalamud.Game;
using Dalamud.Game.Network;
using Dalamud.Plugin;
using System;
using System.Runtime.InteropServices;
using Dalamud.Logging;
using System.IO.Compression;
using SimpleLogs;

namespace SimpleLogs.Network
{
    /*
     * This class is responsible for handling network events
     *
     * The network events we're interested in are combat events
     * We'll subscribe to the network events, sift out the ones we're interested in, then
     * parse the information in them. Once we have the packet's information, we'll use it
     * to calculate the damage of the combat event and send it as an event to the DamageMeter.
     */
    public class Network
    {
        private Plugin plugin;
        private Utilities.Timer timer;

        public Network(Plugin plugin, Utilities.Timer tmr)
        {
            timer = new Utilities.Timer();
            this.plugin = plugin;
            this.timer = tmr;

            // Subscribe to the network events
            plugin.GameNetwork.NetworkMessage += OnNetworkMessage;
        }

        /*
         * This method is called whenever a network event is received, duh.
         *
         * We're checking if it's a combat message we're interested in, if so, we send it on
         * down the line.
         */
        private void OnNetworkMessage(IntPtr dataPtr, ushort opCode, uint sourceActorId, uint targetActorId, NetworkMessageDirection direction)
        {
            Packet packet = new Packet()
            {
                Data = dataPtr, opCode = opCode, sourceActorId = sourceActorId, targetActorId = targetActorId,
                direction = direction
            };
            // send it to Testing if we're debugging
            #if DEBUG
            plugin.Testing.HandlePacket(packet);
            #endif
            
            // otherwise, go down normal pipeline.
            // Check if the message is a combat event from the server
            if (IsCombatEvent(opCode, direction))
            {
                // Parse the packet
                

                // Handle the packet
                HandlePacket(packet);
            }
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
                // TODO: come back to this after we've tested out how to decode the packet headers
                // We'll need to make sure we're decoding the header correctly and that we 
                // have the right opcodes.
                

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
                // PluginLog.LogError(e.ToString());
                return;
            }
            



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
        private IntPtr DecompressPayload(IntPtr payload)
        {
            System.IO.Stream stream = new System.IO.MemoryStream(payload.ToInt32());
            var decomp_data = ZLibStream.Synchronized(stream);
            var archive = new ZipArchive(decomp_data);
            archive.Entries[0].Open().CopyTo(decomp_data);
            byte[] buffer = new byte[decomp_data.Length];
            var dummy = decomp_data.Read(buffer, 0, buffer.Length);
            Marshal.Copy(buffer, 0, payload, buffer.Length);
            return payload;
        }


        
    }
        public struct Packet
        {
            public IntPtr Data;
            public ushort opCode;
            public uint sourceActorId;
            public uint targetActorId;
            public NetworkMessageDirection direction;
        }

        // if the instructions laid out in the document i looked at are right, then these headers should work out just fine.
        // otherwise, we'll need to fix things
        public struct PacketHeader
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

        public struct PacketSegmentHeader
        {
            public uint SourceActor;
            public uint TargetActor;
            public ushort Type02;
            public ushort Padding;
        }
        
        public struct PacketIPCHeader
        {
            public ushort Reserved;
            public ushort Type03;
            public ushort Padding01;
            public ushort ServerId;
            public uint Timestamp;
            public uint Padding02;
        }

        public struct PacketPayload
        {
            public IntPtr Data;
        }

        public struct PacketPayloadDecomp
        {
            public IntPtr Data;
        }

        public struct PacketDecode
        {
            public PacketHeader Header;
            public PacketSegmentHeader SegmentHeader;
            public PacketIPCHeader IPCHeader;
            public PacketPayload Payload;
            public PacketPayloadDecomp PayloadDecomp;
        }

}