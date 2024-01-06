using Dalamud.Game;
using Dalamud.Game.Network;
using Dalamud.Plugin;
using System;
using System.Runtime.InteropServices;

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
            // Implement your packet handling logic here
        }

        private void HandlePacketTesting(Packet packet)
        {
            // Implement your packet handling logic here
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