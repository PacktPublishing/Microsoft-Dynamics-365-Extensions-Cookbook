app.controller('packtController', function ($scope,  packtCrmService) {

    $scope.dataLoading = true;
    packtCrmService.loadAttributesFromCurrentEntity().then(function (result) {
        if (result.status == 200) {
             $scope.attributes = result.data;
        }
        else  {
            alert("Error while loading attributes. " + result.status);
        }
    
    }).finally(function () {
        $scope.dataLoading = false;
    }); 
   
});
