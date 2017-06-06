var packtNs = packtNs || {};
packtNs.concurrency = packtNs.concurrency || {};

var _recordLoadedVersion;
var _schemaName = "";
var _saving = false;

packtNs.concurrency.init = function (schemaName) {
    _schemaName = schemaName;
    var modifiedResult = packtNs.concurrency.checkLastModified("0");
    _recordLoadedVersion = modifiedResult.modifiedVersion;
    Xrm.Page.ui.clearFormNotification("concurrency");
    Xrm.Page.data.entity.removeOnSave(packtNs.concurrency.checkConcurrency);
    Xrm.Page.data.entity.addOnSave(packtNs.concurrency.checkConcurrency);
}

packtNs.concurrency.checkConcurrency = function (executionObj) {
    if (_saving) {
        return;
    }

    var saveMode = executionObj.getEventArgs().getSaveMode();
    executionObj.getEventArgs().preventDefault();

    var modifiedObject = packtNs.concurrency.checkLastModified(_recordLoadedVersion);

    if (!modifiedObject.hasChanged) {
        packtNs.concurrency.callSecondSave(executionObj, saveMode);
        return;
    }

    if (saveMode === 70) {
        Xrm.Page.ui.setFormNotification("Seems like this record has been updated by " + modifiedObject.modifiedBy + ". AutoSave has been aborted.", "WARNING", "concurrency");
    }
    else if (saveMode === 1) {
        Xrm.Utility.confirmDialog("Seems like this record has been updated by " + modifiedObject.modifiedBy + ".\nAre you sure you still want to save it?",
        function () {
            packtNs.concurrency.callSecondSave(executionObj, saveMode);
        },
        null);
    }
    else if (saveMode === 2) {
        Xrm.Utility.confirmDialog("Seems like this record has been updated by " + modifiedObject.modifiedBy + ".\nDo you want to save while closing?",
            function () {
                _saving = true;
                packtNs.concurrency.callSecondSave(executionObj, saveMode);
            },
            null);
    }
    else if (saveMode === 59) {
        Xrm.Utility.confirmDialog("Seems like this record has been updated by " + modifiedObject.modifiedBy + ".\nDo you want to save while closing?",
            function () {
                _saving = true;
                packtNs.concurrency.callSecondSave(executionObj, saveMode);
            },
            null);
    }
}

packtNs.concurrency.callSecondSave = function (executionObj, saveType) {
    setTimeout(function () {
        _saving = true;

        if (saveType === 2) {
            Xrm.Page.data.entity.save("saveandclose");
        }
        else if (saveType === 58) {
            Xrm.Page.data.entity.save("saveandnew");
        }
        else {

            Xrm.Page.data.save().then(function () {
                _saving = false;
                var modifiedObject = packtNs.concurrency.checkLastModified("0");
                _recordLoadedVersion = modifiedObject.modifiedVersion;
                Xrm.Page.ui.clearFormNotification("concurrency");
            }, null);
        }
    }, 300);
}

packtNs.concurrency.checkLastModified = function (recordLoadedVersion) {
    var modifiedObject = {};
    var req = new XMLHttpRequest();
    req.open("GET", encodeURI(Xrm.Page.context.getClientUrl() + "/api/data/v8.2/" + _schemaName + "(" + Xrm.Page.data.entity.getId().substring(1, 37) + ")?$select=_modifiedby_value&$expand=modifiedby($select=fullname)"), false);
    req.send();
    if (req.status === 200 || req.status === 204) {
        var data = JSON.parse(req.responseText);
        modifiedObject.modifiedVersion = data["@odata.etag"];
        modifiedObject.modifiedBy = data.modifiedby.fullname;
        modifiedObject.hasChanged = modifiedObject.modifiedVersion !== recordLoadedVersion;
        return modifiedObject;
    }
    return null;
}