using pbx_shared.dto;

namespace pbx_shared.serverpush
{

    /*
     * ipbx_directclients_msgdistributor
     * 
     * specific clients (e.g. signalr) can incorporate a different api than 
     * the msg propagators. 
     */
    public interface ipbx_directclients_msgdistributor
    {
        void BroadcastPbxEvent(pbx_dto dto);
    }

}
