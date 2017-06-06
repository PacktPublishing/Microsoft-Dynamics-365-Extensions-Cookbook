var packtNs = packtNs || {};
packtNs.common = packtNs.common || {};

/**
* A method that populates the post gradute start date 
* when the supervisor lookup is populated.
* @returns {Void} 
*/
packtNs.common.populateWithTodaysDate =  function(attributeToMonitor, dateAttributeToChange)
{
    if (Xrm.Page.getAttribute(attributeToMonitor).getValue() !== null && Xrm.Page.getAttribute(dateAttributeToChange).getValue() === null)
    {
        Xrm.Page.getAttribute(dateAttributeToChange).setValue(new Date());
    }
}

packtNs.graduateForm = packtNs.graduateForm || {};
packtNs.graduateForm.loadEvent = function(){
packtNs.common.wireOnChangeEvents({
{attribute: “packt_supervisor”, 
function: populateWithTodaysDate(“packt_supervisor”, ("packt_postgraduatestartdate”)
}
});
}

packtNs.common.wireOnChangeEvents =  function(eventAttributeTuples){
for (var i in eventAttributeTuples) {

	Xrm.Page.getAttribute(eventAttributeTuples[i].attribute).addOnChange(eventAttributeTuples[i].function);
}
}


