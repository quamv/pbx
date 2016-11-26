using pbx_shared.dto;

namespace pbx_shared.serverpush
{
    /*
     * iphoneswitch_msgpropagator
     * 
     * interface to forward pbx events
     * 
     * so the process is:
     * an event occurs in the pbx (e.g. a new call comes in)
     * it is processed by the pbx (updating internal state)
     * it is captured into a local processing queue by the pbx
     * it is forwarded (propagated) to a listener via a iphoneswitch_msgpropagator
     * implementation (injected into a pbx via constructor or otherwise).
     */
    public interface ipbx_msgpropagator
    {
        bool enabled { get; set; }
        void propagatepbxevent(pbx_dto dto);
    }


}