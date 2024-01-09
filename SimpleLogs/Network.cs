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

            // Parse the packet
            var packet = Marshal.PtrToStructure<Packet>(dataPtr);

            // Handle the packet
            HandlePacket(packet);
        }

        private void HandlePacket(Packet packet)
        {
            // read the packet array 8 bytes at a time and put each into a byte array
            // that is part of a larger array
            var packetArray = new byte[packet.Length][];
            for (var i = 0; i < packet.Length; i++)
            {
                packetArray[i] = new byte[8];
                Array.Copy(packet.Data, i * 8, packetArray[i], 0, 8);
            }

            // read the first 64 bits of the packet into a ulong variable
            var magic = BitConverter.ToUInt64(packet.Data, 0);
            // read the next 64 bits into a ulong variable
            var timestamp = BitConverter.ToUInt64(packet.Data, 8);
            // read the next 32 bits into a uint variable
            var size01 = BitConverter.ToUInt32(packet.Data, 16);
            // read the next 16 bits into a ushort variable
            var connection_type = BitConverter.ToUInt16(packet.Data, 20);
            // read the next 16 bits into a ushort variable
            var unknown01 = BitConverter.ToUInt16(packet.Data, 22);
            // read the next 8 bits into a integer byte variable
            var isCompressed = packet.Data[24];
            // read the next 32 bits into a uint variable
            var unknown02 = BitConverter.ToUInt32(packet.Data, 25);
            // read the next 32 bits into a uint variable
            var size02 = BitConverter.ToUInt32(packet.Data, 29);
            // if its compressed, decompress the rest of the packet
            if (isCompressed ==1)
            {
                byte[] data = packet.Data[33, packet.Length];
                byte[] comp_data = ZlibStream.CompressBuffer(data);
                byte[] decomp_data = ZlibStream.UncompressBuffer(comp_data);

            }
            if (size02 != 0)
            {
                // read the next 32 bits into a uint variable
                var source_actor = BitConverter.ToUInt32(decomp_data, 0);
                // read the next 32 bits into a uint variable
                var target_actor = BitConverter.ToUInt32(decomp_data, 4);
                // read the next 16 bits into a ushort variable
                var type02 = BitConverter.ToUInt16(decomp_data, 8);
                // read the next 16 bits into a ushort variable
                var padding = BitConverter.ToUInt16(decomp_data, 10);

                if (type02 = 3)
                {
                    // read the next 16 bits into a ushort variable
                    var reserved = BitConverter.ToUInt16(decomp_data, 12);
                    // read the next 16 bits into a ushort variable
                    var type03 = BitConverter.ToUInt16(pdecomp_data, 14);
                    // read the next 16 bits into a ushort variable
                    var padding01 = BitConverter.ToUInt16(decomp_data, 16);
                    // read the next 16 bits into a ushort variable
                    var serverId = BitConverter.ToUInt16(decomp_data, 18);
                    // read the next 32 bits into a uint variable
                    var timestamp = BitConverter.ToUInt32(decomp_data, 20);
                    // read the next 32 bits into a uint variable
                    var padding02 = BitConverter.ToUInt32(decomp_data, 24);
                }
            }



        }

        private void HandlePacketTesting(Packet packet)
        {
            // read the packet array 8 bits at a time and put each 
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct Packet
        {
            public ushort Length;
            public byte[] Data;
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
    }
}