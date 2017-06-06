app.service('packtCrmService', function ($http, $location)
{
    this.loadAttributesFromCurrentEntity = function () {

        var entityGuid = window.location.href.substring(window.location.href.indexOf("=")+1);
        var attributesUrl = window.location.origin + '/api/data/v8.2/EntityDefinitions(' + entityGuid + ')?$select=LogicalName&$expand=Attributes($select=LogicalName;$filter=AttributeType eq Microsoft.Dynamics.CRM.AttributeTypeCode\'Picklist\')';
        
        var attributesRequest = {
            method: 'GET',
            url: attributesUrl,
            headers: {
                'Prefer': 'odata.include-annotations="OData.Community.Display.V1.FormattedValue"'
            }
        }
        
        var attributesPromise = $http(attributesRequest).then(
            function (response) {                    
                return {
                    "status": response.status,
                     "data": response.data.Attributes
                };                            
            }, 
            function (response) {
                return {
                    "status": response.status,
                    "data": response.statusText
                };
            }
        );
        
        return attributesPromise;
    }
});
