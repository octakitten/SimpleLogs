using Dalamud.Game;
using Dalamud.Game.Network;
using Dalamud.Plugin;
using System;
using System.Runtime.InteropServices;
using Ionic.Zlib;

namespace SimpleLogs
{
    public class Network
    {
        private DalamudPluginInterface pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            // Subscribe to the network events
            this.pluginInterface.Framework.Network.OnNetworkMessage += OnNetworkMessage;
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
            if (pluginInterface.Config.GetBool("network_testing"))
            {
                // Parse the packet
                var packet = Marshal.PtrToStructure<Packet>(dataPtr);

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
                header = Marshal.PtrToStructure<PacketHeader>(packet.Data);

                IntPtr segmentHeaderPtr = IntPtr.Add(packet.Data, Marshal.SizeOf(typeof(PacketHeader)));
                segmentHeader = Marshal.PtrToStructure<PacketSegmentHeader>(segmentHeaderPtr);

                IntPtr ipcHeaderPtr = IntPtr.Add(segmentHeaderPtr, Marshal.SizeOf(typeof(PacketSegmentHeader)));
                ipcHeader = Marshal.PtrToStructure<PacketIPCHeader>(ipcHeaderPtr);

                IntPtr payloadPtr = IntPtr.Add(ipcHeaderPtr, Marshal.SizeOf(typeof(PacketIPCHeader)));
                payload = Marshal.PtrToStructure<PacketPayload>(payloadPtr);
            }
            catch (Exception e)
            {
                // if we get an exception, log it and return
                PluginLog.LogError(e.ToString());
                return;
            }
            
            if (PacketHeader.isCompressed == 1)
            {
                PacketPayloadDecomp.Data = DecompressPayload(PacketPayload.Data);
            }



        }

        private void HandlePacketTesting(Packet packet)
        {
            // read the packet array 1 byte at a time and put each into a byte array

            var packetArray = new byte[packet.Length][];
            for (var i = 0; i < packet.Length; i++)
            {
                packetArray[i] = new byte[8];
                Array.Copy(packet.Data, i * 8, packetArray[i], 0, 8);
            }
        }


        // implement the IsCombatEvent method
        private bool IsCombatEvent(ushort opCode, NetworkMessageDirection direction)
        {

            // if its from the server, go forward with the packet
            if (direction == NetworkMessageDirection.Server)
            {
                // TODO figure out opCodes for combat events
            }

            return false;
        }

        // decompress packet payload here
        private byte[] DecompressPayload(byte[] payload)
        {
            byte[] decomp_data = ZlibStream.UncompressBuffer(payload);
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
            public PacketPayloadDecomp PayloadDecomp
        }
    }
}