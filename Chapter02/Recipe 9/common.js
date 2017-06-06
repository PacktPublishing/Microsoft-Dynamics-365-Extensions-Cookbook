var packtNs = packtNs || {};
packtNs.common = packtNs.common || {};

packtNs.common.populateWithTodaysDate =  function()
{
    if (Xrm.Page.getAttribute("packt_supervisor").getValue() !== null && Xrm.Page.getAttribute("packt_postgraduatestartdate").getValue() === null)
    {
        Xrm.Page.getAttribute("packt_postgraduatestartdate").setValue(new Date());
    }
}
