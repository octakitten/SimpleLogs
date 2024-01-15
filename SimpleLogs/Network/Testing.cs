namespace SimpleLogs.Network;
#if DEBUG

using System.Collections.Generic;

/*
 * The Testing class is only reachable when the DEBUG preproc variable is defined
 * It's used for testing the packets we receive from the game through Dalamud
 * to see what information we can get out of them.
 * It's not actually used for debugging though, just figuring out opcodes
 * and packet structure.
 */
public class Testing
{
    private Utilities.Timer timer;
    private Plugin plugin;
    private List<Packet> packetLog;
    private List<double> timestampLog;

    public Testing(Plugin plugin, Utilities.Timer timer)
    {
        this.plugin = plugin;
        this.timer = timer;
    }

    public void HandlePacket(Packet packet)
    {
        timestampLog.Add(timer.GetElapsedTime().TotalSeconds);
        packetLog.Add(packet);


    }

    /*
     * Here we'll try decoding the packet.
     *
     * This is according to the packet layout outlined on the XIV Developer Wiki
     * We'll see if this works, I'm guessing more needs to be done.
     * It also might not be applicable to the data object that Dalamud's API
     * exposes to us with the method we're using. Not sure yet.
     *
     */
    public PacketDecode TryDecode(Packet packet)
    {
        // I'll try this method later.
        return (new PacketDecode());
    }
}
#endif