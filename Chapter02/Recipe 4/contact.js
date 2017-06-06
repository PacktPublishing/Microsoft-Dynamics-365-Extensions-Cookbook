var packtNs = packtNs || {}; 
packtNs.contact = packtNs.contact || {};

packtNs.contact.checkActiveTasksAssignedToMe = function()
{
    var uniqueNotificationId = "OutstandingTasks"; 
    
    Xrm.Page.ui.clearFormNotification(uniqueNotificationId);
    
    var contactId = Xrm.Page.data.entity.getId().replace(/[{}]/g,"");
    var currentUserId = Xrm.Page.context.getUserId().replace(/[{}]/g,"");
    
    var req = new XMLHttpRequest();
    var requestUrl = Xrm.Page.context.getClientUrl() + "/api/data/v8.2/tasks?$filter=_regardingobjectid_value eq " + contactId + " and _ownerid_value eq " + currentUserId + " and  statecode eq 0";
        
    req.open("GET", requestUrl, true);
    req.setRequestHeader("OData-MaxVersion", "4.0");
    req.setRequestHeader("OData-Version", "4.0");
    req.setRequestHeader("Accept", "application/json");
    req.setRequestHeader("Prefer", "odata.include-annotations=\"OData.Community.Display.V1.FormattedValue\"");
    req.onreadystatechange = function () {
        if (this.readyState === 4) {
            req.onreadystatechange = null;
            if (this.status === 200) {
                var results = JSON.parse(this.response);
                
                if(results.value.length > 0){
                    Xrm.Page.ui.setFormNotification("You have outstanding tasks assigned to you.", "WARNING", uniqueNotificationId)
                }                
            }
            else {
                //.. handle error
            }
        }
    };
    req.send();
} 
