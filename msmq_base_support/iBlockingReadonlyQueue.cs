using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msmq_base_support
{
    public interface iBlockingReadonlyQueue<T>
    {
        T Take();
        void start();
    }

}
