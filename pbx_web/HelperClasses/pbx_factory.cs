using Microsoft.AspNet.SignalR;
using pbx_lib;
using pbx_msmq_integration;
using pbx_shared;
using pbx_shared.serverpush;
using pbx_signalr;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pbx_web.HelperClasses
{

    /* factory class to create a new pbx object */
    public class pbx_factory
    {
        /* manufacture a new pbx object */
        public static pbx getpbx()
        {
            // load the data from project settings
            var settings = Properties.Settings.Default;
            var trunks = parsesettingcollection<phonenumber>(settings.trunks, (s) => { return trunkfromsettingsline(s); });
            var extensions = parsesettingcollection<extension>(settings.extensions, (s) => { return extensionfromsettingsline(s); });

            // propagators distribute event notifications
            var propagatorcollection = new propagator_collection();

            // direct clients are the web/signalr clients connected directly to this web app.
            propagatorcollection.propagators.Add(new signalr_propagator(GlobalHost.ConnectionManager.GetHubContext<PbxHub>().Clients));

            // pbx_msmq_msg_propagators send events via msmq to a monitoring system
            // settings.queues is a project setting (stringcollection) of queue parameters in csv format. 
            // each line is hostname,queue_name
            // ex:
            // hostname1,queue1
            // hostname2,queue2
            //
            // the queues must exist and be writeable
            var queues = parsesettingcollection<msmq_pbx_event_propagator>(settings.queues, (q) => { return msmq_propagatorfromsettingsline(q); });
            queues.ForEach((q) => { propagatorcollection.propagators.Add(q); });

            return new pbx(trunks, extensions, propagatorcollection);
        }


        /* parse a stringcollection using the injected line parser */
        private static List<T> parsesettingcollection<T>(System.Collections.Specialized.StringCollection strings, Func<string, T> lineparser)
        {
            var thelist = new List<T>();
            foreach (var str in strings) { thelist.Add(lineparser(str)); }
            return thelist;
        }

        /* parse an extension from a csv string */
        private static extension extensionfromsettingsline(string line2parse)
        {
            var splitted = line2parse.Split(',').ToArray();
            var _extension = new extension()
            {
                extension_nbr = splitted[0],
                extension_name = splitted[1],
                extension_type = (splitted[2] == "ip" ? extension.extension_types.ip : extension.extension_types.digital)
            };

            return _extension;
        }

        /* parse a phonenumber from a csv string */
        private static phonenumber trunkfromsettingsline(string line2parse)
        {
            var splitted = line2parse.Split(',').ToArray();
            var _trunk = new phonenumber()
            {
                phonenbr = splitted[0],
                linetype = (splitted[1] == "t1" ? phonenumber.trunktypes.t1 : phonenumber.trunktypes.analog)
            };

            return _trunk;
        }

        /* parse a msmq propagator from a csv string (localhost,queue_name) */
        private static msmq_pbx_event_propagator msmq_propagatorfromsettingsline(string line2parse)
        {
            var values = line2parse.Split(',');
            return new msmq_pbx_event_propagator(values[0], values[1]);

        }
    }
}