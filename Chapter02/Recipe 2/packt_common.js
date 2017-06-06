var packtNs = packtNs || {};
packtNs.common = packtNs.common || {};

/**
* A method that populates the post gradute start date 
* when the supervisor lookup is populated.
* @returns {Void} 
*/
packtNs.common.populateWithTodaysDate =  function()
{
    if (Xrm.Page.getAttribute("packt_supervisor").getValue() !== null && Xrm.Page.getAttribute("packt_postgraduatestartdate").getValue() === null)
    {
        Xrm.Page.getAttribute("packt_postgraduatestartdate").setValue(new Date());
    }
}

packtNs.graduateForm = packtNs.graduateForm || {};
packtNs.graduateForm.loadEvent = function(){
	Xrm.Page.getAttribute("packt_supervisor").addOnChange(populateWithTodaysDate);
}
