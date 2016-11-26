/// <reference path="typings/knockout/knockout.d.ts" />
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var phonelinestable_vm = (function () {
    function phonelinestable_vm(trunks, selectedview) {
        var _this = this;
        this.sortfield = ko.observable('phonenbr');
        this.sortdirection = ko.observable('asc');
        this.sorton = function (field) {
            if (_this.sortfield() == field) {
                if (_this.sortdirection() == "asc") {
                    _this.sortdirection("desc");
                }
                else {
                    _this.sortdirection("asc");
                }
            }
            else {
                _this.sortdirection("asc");
                _this.sortfield(field);
            }
        };
        var that = this;
        that.phonenumbers = trunks;
        that.selected_phonelinesview = selectedview;
        this.sortedphonelines = ko.computed({
            read: function () {
                var newsortfield = that.sortfield().toLocaleLowerCase();
                var newsortdirection = that.sortdirection().toLocaleLowerCase();
                var _i = (newsortdirection == 'asc') ? -1 : 1;
                var alltrunks = that.phonenumbers().slice();
                var sortedtrunksresult = alltrunks.sort(function (line1, line2) {
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
                var filteredresult = sortedtrunksresult;
                if (that.selected_phonelinesview() == 'calls only') {
                    filteredresult = filteredresult.filter(function (line) {
                        return line.linestate == 1 || line.linestate == 2;
                    });
                }
                return filteredresult;
                //return sortedtrunksresult;
            }
        });
    }
    return phonelinestable_vm;
}());
var linestable_vmparams = (function () {
    function linestable_vmparams() {
    }
    return linestable_vmparams;
}());
/* components registration */
ko.components.register('phonelines_table', {
    viewModel: function (params) { return params.vmobj; },
    template: { element: 'phonelinestabletemplate' }
});
ko.components.register('phonelines_vlist', {
    viewModel: function (params) { return params.vmobj; },
    template: { element: 'phonelinesvlisttemplate' }
});
var phonenumber_dto = (function () {
    function phonenumber_dto(trunk) {
        this.phonenbr = trunk.phonenbr;
        this.linestate = trunk.linestate;
        this.linetype = trunk.linetype;
    }
    return phonenumber_dto;
}());
var phonenumber = (function () {
    function phonenumber(dto) {
        var _this = this;
        this.activecall = ko.observable(null);
        this.phonenbr = dto.phonenbr;
        this.linestate = dto.linestate;
        this.linetype = dto.linetype;
        var that = this;
        this.linestatestr = ko.computed({
            read: function () {
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
            read: function () {
                switch (_this.linetype) {
                    case 0:
                        return "analog";
                    case 1:
                        return "t1";
                }
                return "Unknown: " + _this.linetype.toString();
            }
        });
        this.linecss = ko.computed({
            read: function () {
                var final_css = "vlistdiv list-group-item ";
                var _call = _this.activecall();
                if (_call == null) {
                    final_css += "idle ";
                }
                else {
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
    return phonenumber;
}());
var pbx_dto_phonecall = (function () {
    function pbx_dto_phonecall(dto) {
        this.callstate = 0;
        this.callid = dto.callid;
        this.localnbr = dto.localnbr;
        this.remotenbr = dto.remotenbr;
        this.callstate = dto.callstate;
        this.callstateobs = ko.observable(dto.callstate);
        this.connected_extensions_obs = ko.observableArray(dto.connected_extensions);
    }
    return pbx_dto_phonecall;
}());
var call = (function (_super) {
    __extends(call, _super);
    function call(dto) {
        _super.call(this, dto);
        this.currentstate = ko.observable(0);
    }
    return call;
}(pbx_dto_phonecall));
var extensionconnected_dto = (function () {
    function extensionconnected_dto() {
    }
    return extensionconnected_dto;
}());
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
var pbx_dto_types;
(function (pbx_dto_types) {
    pbx_dto_types[pbx_dto_types["callreceived"] = 0] = "callreceived";
    pbx_dto_types[pbx_dto_types["callanswered"] = 1] = "callanswered";
    pbx_dto_types[pbx_dto_types["calltransferred"] = 2] = "calltransferred";
    pbx_dto_types[pbx_dto_types["callstatechanged"] = 3] = "callstatechanged";
    pbx_dto_types[pbx_dto_types["callended"] = 4] = "callended";
    pbx_dto_types[pbx_dto_types["extensionconnected"] = 5] = "extensionconnected";
    pbx_dto_types[pbx_dto_types["fullstatus"] = 6] = "fullstatus";
})(pbx_dto_types || (pbx_dto_types = {}));
var signalrconnection = (function () {
    function signalrconnection() {
    }
    return signalrconnection;
}());
var phonelines_vm = (function () {
    function phonelines_vm(conn) {
        this.operators = ko.observableArray([]);
        this.trunks = ko.observableArray([]);
        this.selected_trunksview = ko.observable('all phonenumbers');
        this.laststatusupdate = ko.observable(new Date());
        this.calls = ko.observableArray([]);
        var that = this;
        this.conn = conn;
        conn.pbx.client.pbxevent = function (dto) {
            that.processpbxevent(dto);
        };
        conn.pbx.client.fullStatusUpdate = function (dto) {
            that.processstatusupdate(dto);
        };
        conn.hub.start().then(function () {
            //that.vmstate(vmstates.loading);
            that.getinitialdatasets();
            //that.hubstarted();
        });
        ko.applyBindings(this);
    }
    phonelines_vm.prototype.getinitialdatasets = function () {
        var that = this;
        that.conn.pbx.server.getsystemstate()
            .done(function (dto) {
            that.processstatusupdate(dto);
        });
    };
    phonelines_vm.prototype.processstatusupdate = function (dto) {
        var that = this;
        that.laststatusupdate(dto.timestamp);
        that.trunks([]);
        //that.phonenumbers(dto.phonenumbers);
        dto.phonenumbers.forEach(function (linedto) {
            //dto.phonenumbers.forEach(function (line: phonenumber_dto) {
            //newdto.phonenbr = line.phonenbr;
            //newdto.linestate = line.linestate;
            //newdto.linetype = line.linetype;
            var newtrunk = new phonenumber(linedto);
            that.trunks.push(newtrunk);
        });
        that.calls([]);
        dto.calls.forEach(function (newcalldto) {
            that.addcall(new call(newcalldto));
            //that.processpbxnewcallreceived(newcalldto);
            //let newcall = new call(newcalldto);
            //that.calls.push(newcall);
        });
    };
    phonelines_vm.prototype.gettrunk = function (phonenbr) {
        var trnk = null;
        var trnks = this.trunks();
        for (var i = 0; i < trnks.length; i++) {
            if (trnks[i].phonenbr == phonenbr) {
                return this.trunks()[i];
            }
        }
        return null;
    };
    phonelines_vm.prototype.addcall = function (newcall) {
        this.calls.push(newcall);
        for (var i = 0; i < this.trunks().length; i++) {
            if (this.trunks()[i].phonenbr == newcall.localnbr) {
                var trnk = this.trunks()[i];
                trnk.activecall(newcall);
                i = this.trunks().length; // exit loop
            }
        }
    };
    phonelines_vm.prototype.processpbxnewcallreceived = function (dto) {
        var newcall = new call(dto.call_dto);
        this.addcall(new call(dto.call_dto));
    };
    phonelines_vm.prototype.processpbxcallended = function (dto) {
        for (var i = 0; i < this.calls().length; i++) {
            if (this.calls()[i].callid == dto.callid) {
                var _call = this.calls()[i];
                if (_call.callid == dto.callid) {
                    var _nbr = _call.localnbr;
                    var trnk = this.gettrunk(_nbr);
                    trnk.activecall(null);
                    i = this.calls().length;
                }
            }
        }
    };
    phonelines_vm.prototype.processpbxcallstatechange = function (dto) {
        var _call2 = this.getcall(dto.callid);
        _call2.callstateobs(dto.newstate);
    };
    phonelines_vm.prototype.processpbxcalltransferred = function (dto) {
        var _call = this.getcall(dto.callid);
        _call.connected_extensions_obs.remove(dto.from_extension);
        _call.connected_extensions_obs.push(dto.to_extension);
    };
    phonelines_vm.prototype.processpbxevent = function (dto) {
        var s = "";
        switch (dto._dto_type) {
            case pbx_dto_types.callreceived:
                this.processpbxnewcallreceived(dto);
                break;
            case pbx_dto_types.callended:
                this.processpbxcallended(dto);
                break;
            case pbx_dto_types.callstatechanged:
                this.processpbxcallstatechange(dto);
                break;
            case pbx_dto_types.calltransferred:
                this.processpbxcalltransferred(dto);
                break;
            default:
                s += "unknown";
                break;
        }
        console.log('receieved: ' + dto._dto_type + ' (' + s + ')');
    };
    phonelines_vm.prototype.getcall = function (callid) {
        var that = this;
        for (var i = 0; i < this.calls().length; i++) {
            var _call = this.calls()[i];
            if (_call.callid == callid) {
                return _call;
            }
        }
        return null;
    };
    // set the displayed view
    phonelines_vm.prototype.callsonly = function () { this.selected_trunksview('calls only'); };
    phonelines_vm.prototype.alltrunks = function () { this.selected_trunksview('all phonenumbers'); };
    phonelines_vm.prototype.answercall = function (callid) { this.conn.pbx.server.answercall(callid); };
    phonelines_vm.prototype.holdcall = function (callid) {
        this.conn.pbx.server.holdcall(callid);
    };
    phonelines_vm.prototype.resumecall = function (callid) { this.conn.pbx.server.resumecall(callid); };
    phonelines_vm.prototype.endcall = function (callid) { this.conn.pbx.server.endcall(callid); };
    phonelines_vm.prototype.testing = function () { this.conn.pbx.server.testing(); };
    phonelines_vm.prototype.transfer = function (callid) {
        var extension = prompt("enter the extension");
        if (extension) {
            this.conn.pbx.server.connect_extension(callid, extension);
        }
    };
    phonelines_vm.prototype.modal = function () {
        var extension = prompt("enter the extension");
        //$('#btntrunkmodal').click();
    };
    return phonelines_vm;
}());
//# sourceMappingURL=_phonelines_vm.js.map