/// <reference path="typings/knockout/knockout.d.ts" />

class phonelinestable_vm {
    public phonenumbers: KnockoutObservableArray<phonenumber>;

    public selected_phonelinesview: KnockoutObservable<string>;

    public sortfield: KnockoutObservable<string> = ko.observable('phonenbr');
    public sortdirection: KnockoutObservable<string> = ko.observable('asc');
    public sortedphonelines: KnockoutComputed<phonenumber[]>;// = ko.computed({
    public sorton = (field: string) => {
        if (this.sortfield() == field) {
            if (this.sortdirection() == "asc") {
                this.sortdirection("desc");
            } else {
                this.sortdirection("asc");
            }
        } else {
            this.sortdirection("asc");
            this.sortfield(field);
        }
    }


    constructor(trunks: KnockoutObservableArray<phonenumber>, selectedview: KnockoutObservable<string>) {
        var that = this;
        that.phonenumbers = trunks;
        that.selected_phonelinesview = selectedview;
        this.sortedphonelines = ko.computed({
            read: () => {

                let newsortfield = that.sortfield().toLocaleLowerCase();
                let newsortdirection = that.sortdirection().toLocaleLowerCase();
                let _i = (newsortdirection == 'asc') ? -1 : 1;

                let alltrunks = that.phonenumbers().slice();

                let sortedtrunksresult = alltrunks.sort((line1: phonenumber, line2: phonenumber) => {
                    return ((line1[newsortfield] < line2[newsortfield]) ? _i : -_i);
                    //if (newsortfield == 'phonenbr') {
                    //    return ((line1[newsortfield] < line2[newsortfield]) ? _i : -_i);
                    //} else {
                    //    return (
                    //        (line1[newsortfield] < line2[newsortfield]) ?
                    //                ((line1.linetype < line2.linetype) ? _i : -_i)
                    //            :
                    //                -_i);

                    //}
                });

                let filteredresult = sortedtrunksresult;

                if (that.selected_phonelinesview() == 'calls only') {
                    filteredresult = filteredresult.filter((line: phonenumber) => {
                        return line.linestate == 1 || line.linestate == 2;
                    });
                }

                return filteredresult;
                //return sortedtrunksresult;
            }
        });
    }


}

class linestable_vmparams {
    public vmobj: phonelines_vm;
}

/* components registration */
ko.components.register('phonelines_table', {
    viewModel: function (params: linestable_vmparams) { return params.vmobj; }
    , template: { element: 'phonelinestabletemplate' }
});
ko.components.register('phonelines_vlist', {
    viewModel: function (params: linestable_vmparams) { return params.vmobj; }
    , template: { element: 'phonelinesvlisttemplate' }
});

declare class hubts {
    public start(): any;
    public stateChanged: any;
    public reconnected: any;
    public logging: boolean;
}

class phonenumber_dto {
    public phonenbr: string;
    public linestate: number;
    public linetype: number;

    public constructor(trunk: phonenumber) {
        this.phonenbr = trunk.phonenbr;
        this.linestate = trunk.linestate;
        this.linetype = trunk.linetype;
    }
}
class phonenumber {
    public phonenbr: string;
    public linestate: number;
    public linetype: number;

    public activecall: KnockoutObservable<call> = ko.observable(null);

    public linestatestr: KnockoutComputed<string>;
    public linetypestr: KnockoutComputed<string>;
    public linecss: KnockoutComputed<string>;

    public constructor(dto: phonenumber_dto) {
        this.phonenbr = dto.phonenbr;
        this.linestate = dto.linestate;
        this.linetype = dto.linetype;

        var that = this;
        this.linestatestr = ko.computed({
            read: () => {
                switch (that.linestate) {
                    case 0:
                        return "idle";
                    case 1:
                        return "ringing";
                    case 2:
                        return "connected";
                    case 3:
                        return "cleanup";
                    case 4:
                        return "unknown";
                    case 5:
                        return "offline";
                    case 6:
                        return "hold";
                    case 7:
                        return "busy";
                    default:
                        return "unknown state: " + that.linestate.toString();
                }

                //return "Unknown: " + this.linetype.toString();
            }
        });
        this.linetypestr = ko.computed({
            read: () => {
                switch (this.linetype) {
                    case 0:
                        return "analog";
                    case 1:
                        return "t1";
                }

                return "Unknown: " + this.linetype.toString();
            }
        });
        this.linecss = ko.computed({
            read: () => {
                let final_css = "vlistdiv list-group-item ";

                let _call = this.activecall();
                if (_call == null) {
                    final_css += "idle ";
                } else {
                    switch (_call.callstateobs()) {
                        case 0:
                            final_css += "list-group-item-info ";
                            break;
                        case 1:
                            final_css += "hold list-group-item-danger ";
                            break;
                        case 2:
                            final_css += "list-group-item-success ";
                            break;
                        default:
                            return "unknown" + _call.callstateobs(); 
                    }
                }

                return final_css;
            }
        });
    }
}

declare class statusupdate_dto {
    public phonenumbers: phonenumber[];
    public timestamp: Date;
    public calls: pbx_dto_phonecall[];
}

declare class callstatechange_dto {
    public callid: number;
    public newstate: number;
}

class pbx_dto_phonecall {
    public callid: number;
    public localnbr: string;
    public remotenbr: string;
    public callstate: number = 0;
    public callstateobs: KnockoutObservable<number>;
    public connected_extensions: string[];
    public connected_extensions_obs: KnockoutObservableArray<string>;

    constructor(dto: pbx_dto_phonecall) {
        this.callid = dto.callid;
        this.localnbr = dto.localnbr;
        this.remotenbr = dto.remotenbr;
        this.callstate = dto.callstate;
        this.callstateobs = ko.observable(dto.callstate);
        this.connected_extensions_obs = ko.observableArray(dto.connected_extensions);
    }
}

class call extends pbx_dto_phonecall {
    public currentstate: KnockoutObservable<number> = ko.observable(0);

    public constructor(dto: pbx_dto_phonecall) {
        super(dto);
    }
}


class extensionconnected_dto {
    public callid: number;
    public extension_nbr: string;
    public addremove: string;
}

// dto_type codes
//enum dto_type {
//    callreceived,
//    callanswered,
//    calltransferred,
//    callstatechanged,
//    callended,
//    secondary_extension_added,
//    secondary_extension_removed,
//    unknown
//}

enum pbx_dto_types {callreceived, callanswered, calltransferred, callstatechanged, callended, extensionconnected, fullstatus }
declare class pbx_dto {
    public _dto_type: number; 
    public createdate: Date;
}
declare class pbx_dto_callupdate_base extends pbx_dto {
    public callid: number;
}
declare class pbx_dto_callreceived extends pbx_dto{
    public call_dto: pbx_dto_phonecall;
}
declare class pbx_dto_callstatechange extends pbx_dto_callupdate_base {
    public newstate: number;
}
declare class pbx_dto_callended extends pbx_dto_callupdate_base { }
declare class pbx_dto_callandextension_base extends pbx_dto_callupdate_base {
    public extension_nbr: string;
}
declare class pbx_dto_secondary_extension_added extends pbx_dto_callandextension_base { }
declare class pbx_dto_secondary_extension_removed extends pbx_dto_callandextension_base { }

declare class pbx_dto_fullstatus {

}

declare class pbx_dto_calltransferred extends pbx_dto {
    public callid: number;
    public from_extension: string;
    public to_extension: string;
}

declare class pbx_client_api {
    public fullStatusUpdate(dto: statusupdate_dto);
    public newCall(dto: pbx_dto_phonecall);
    public callReceived(dto: pbx_dto_callreceived);
    public callEnded(dto: callstatechange_dto);
    public callStateChange(dto: callstatechange_dto);
    public callTransferred(dto: pbx_dto_calltransferred);
    public extensionConnected(dto: extensionconnected_dto);
    public pbxevent(dto: pbx_dto);
    public testing();
}

declare class pbx_server_api {
    public getsystemstate(): any; // any allows for promises. statusupdatedto;
    public answercall(callid: number): void;
    public holdcall(callid: number): void;
    public connect_extension(callid: number, extension_nbr: string);
    public disconnect_extension(callid: number, extension_nbr: string);
    public resumecall(callid: number): void;
    public endcall(callid: number): void;
    public testing(): void;
}

declare class pbx {
    public client: pbx_client_api;
    public server: pbx_server_api;
}

class signalrconnection {

    // custom api (enable trunk, disable trunk)
    public pbx: pbx;

    // basic hub start/stop functionality
    public hub: hubts;

    // log goes to console output
    public logging: boolean;
}


class phonelines_vm {
    public title: string;
    private conn: signalrconnection;
    public operators: KnockoutObservableArray<string> = ko.observableArray([]);
    public trunks: KnockoutObservableArray<phonenumber> = ko.observableArray([]);
    public selected_trunksview: KnockoutObservable<string> = ko.observable('all phonenumbers');
    public laststatusupdate: KnockoutObservable<Date> = ko.observable(new Date());
    public calls: KnockoutObservableArray<call> = ko.observableArray([]);

    getinitialdatasets() {
        let that = this;

        that.conn.pbx.server.getsystemstate()
            .done(function (dto: statusupdate_dto) {
                that.processstatusupdate(dto);
            });
    }

    processstatusupdate(dto: statusupdate_dto) {
        var that = this;
        that.laststatusupdate(dto.timestamp);
        that.trunks([]);
        //that.phonenumbers(dto.phonenumbers);
        dto.phonenumbers.forEach(function (linedto: any) {
            //dto.phonenumbers.forEach(function (line: phonenumber_dto) {
            //newdto.phonenbr = line.phonenbr;
            //newdto.linestate = line.linestate;
            //newdto.linetype = line.linetype;
            let newtrunk = new phonenumber(linedto);
            that.trunks.push(newtrunk);
        });

        that.calls([]);
        dto.calls.forEach(function (newcalldto: pbx_dto_phonecall) {
            that.addcall(new call(newcalldto));
            //that.processpbxnewcallreceived(newcalldto);
            //let newcall = new call(newcalldto);
            //that.calls.push(newcall);
        });
    }

    gettrunk(phonenbr: string): phonenumber {
        var trnk = null;
        let trnks = this.trunks();
        for (var i = 0; i < trnks.length; i++) {
            if (trnks[i].phonenbr == phonenbr) {
                return this.trunks()[i];
            }
        }

        return null;
    }

    addcall(newcall: call) {
        this.calls.push(newcall);
        for (var i = 0; i < this.trunks().length; i++) {
            if (this.trunks()[i].phonenbr == newcall.localnbr) {
                let trnk = this.trunks()[i];
                trnk.activecall(newcall);
                i = this.trunks().length; // exit loop
            }
        }
    }

    processpbxnewcallreceived(dto: pbx_dto_callreceived) {
        let newcall = new call(dto.call_dto);
        this.addcall(new call(dto.call_dto));
    }

    processpbxcallended(dto: pbx_dto_callended) {
        for (var i = 0; i < this.calls().length; i++) {
            if (this.calls()[i].callid == dto.callid) {
                let _call = this.calls()[i];

                if (_call.callid == dto.callid) {
                    let _nbr = _call.localnbr;
                    let trnk = this.gettrunk(_nbr);
                    trnk.activecall(null);
                    i = this.calls().length;
                }
            }
        }
    }


    processpbxcallstatechange(dto:pbx_dto_callstatechange) {
        let _call2 = this.getcall(dto.callid);
        _call2.callstateobs(dto.newstate);
    }

    processpbxcalltransferred(dto: pbx_dto_calltransferred) {
        let _call = this.getcall(dto.callid);
        _call.connected_extensions_obs.remove(dto.from_extension);
        _call.connected_extensions_obs.push(dto.to_extension);
    }


    processpbxevent(dto: any) {
        let s: string = "";

        switch ((dto as pbx_dto)._dto_type) {
            case pbx_dto_types.callreceived:
                this.processpbxnewcallreceived(dto as pbx_dto_callreceived);
                break;
            case pbx_dto_types.callended:
                this.processpbxcallended(dto as pbx_dto_callended);
                break;
            case pbx_dto_types.callstatechanged:
                this.processpbxcallstatechange(dto as pbx_dto_callstatechange);
                break;
            case pbx_dto_types.calltransferred:
                this.processpbxcalltransferred(dto as pbx_dto_calltransferred);
                break;
            default:
                s += "unknown";
                break;
        }

        console.log('receieved: ' + dto._dto_type + ' (' + s + ')');
    }

    getcall(callid: number): call {
        var that = this;

        for (var i = 0; i < this.calls().length; i++) {
            let _call = this.calls()[i];

            if (_call.callid == callid) {
                return _call;
            }
        }        

        return null;
    }

    constructor(conn: signalrconnection) {

        var that = this;
        this.conn = conn;

        conn.pbx.client.pbxevent = function (dto: pbx_dto) {
            that.processpbxevent(dto);
        }

        conn.pbx.client.fullStatusUpdate = function (dto: statusupdate_dto) {
            that.processstatusupdate(dto);
        }

        conn.hub.start().then(() => {
            //that.vmstate(vmstates.loading);
            that.getinitialdatasets();
            //that.hubstarted();
        });

        ko.applyBindings(this);

    }

    // set the displayed view
    callsonly() { this.selected_trunksview('calls only'); }
    alltrunks() { this.selected_trunksview('all phonenumbers');}

    answercall(callid: number) { this.conn.pbx.server.answercall(callid);}
    holdcall(callid: number) {
        this.conn.pbx.server.holdcall(callid);
    }
    resumecall(callid: number) { this.conn.pbx.server.resumecall(callid);}
    endcall(callid: number) { this.conn.pbx.server.endcall(callid); }

    testing() { this.conn.pbx.server.testing(); }

    transfer(callid: number) {
        var extension = prompt("enter the extension");

        if (extension) {
            this.conn.pbx.server.connect_extension(callid, extension);
            //alert('transferring call ' + callid + ' to extension ' + extension);
        }

    }

    modal() {
        var extension = prompt("enter the extension");

        //$('#btntrunkmodal').click();
    }
    //endcall(callid: number) { alert('clicked: ' + callid);}
    //endcall2(phoneline: phonenumber) {
    //    let _call = phoneline.activecall();
    //    //alert('endcall2, phoneline: ' + phoneline.phonenbr + ' call: ' + _call.callid);
    //    this.conn.pbx.server.endcall(_call.callid);
    //}
}
