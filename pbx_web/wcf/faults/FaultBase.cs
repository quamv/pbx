using pbx_shared.misc;
using System;
using System.Runtime.Serialization;

namespace pbx_web.wcf
{
    /*
     * FaultBase
     * 
     * base class for wcf faults
     */
    [DataContract]
    [Serializable()]
    public class FaultBase
    {
        public int _excode;

        [DataMember]
        public int excode { get { return _excode; } set { _excode = value; } }

        public FaultBase(CustomExceptionCodes excode)
        {
            this.excode = (int)excode;
        }
    }

}