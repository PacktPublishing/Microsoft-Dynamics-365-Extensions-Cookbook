var organizationURI = "https://.crm6.dynamics.com"; 
var tenant = ".onmicrosoft.com"; 
var clientId = "00000000-0000-0000-0000-000000000000"; 
var pageUrl = "http://localhost/corssample.htm";

var endpoints = {
orgUri: organizationURI
};
window.config = {
tenant: tenant,
clientId: clientId,
postLogoutRedirectUri: pageUrl,
endpoints: endpoints,
cacheLocation: 'localStorage',
};
var user, authContext, message, errorMessage ;
document.onreadystatechange = function () {
if (document.readyState == "complete") {
authenticate();
}
}
function authenticate() {
authContext = new AuthenticationContext(config);
var isCallback = authContext.isCallback(window.location.hash);
if (isCallback) {
authContext.handleWindowCallback();
}
var loginError = authContext.getLoginError();
if (isCallback && !loginError) {
window.location =
authContext._getItem(authContext.CONSTANTS.STORAGE.LOGIN_REQUEST);

}
else {
	//TODO handle error
}
user = authContext.getCachedUser();
}
function login() {
authContext.login();
}
function getAccounts() {
authContext.acquireToken(organizationURI, retrieveAccounts)
}
function retrieveAccounts(error, token) {
if (error || !token) {
//TODO handle error
return;
}
var req = new XMLHttpRequest()
req.open("GET", encodeURI(organizationURI +"/api/data/v8.2/accounts?$top=2&$select=name&$orderby=name"),true);
req.setRequestHeader("Authorization", "Bearer " + token);
req.setRequestHeader("Content-Type", "application/json;charset=utf-8");
req.setRequestHeader("OData-MaxVersion", "4.0");
req.setRequestHeader("OData-Version", "4.0");
req.onreadystatechange = function () {
if (this.readyState == 4) {
req.onreadystatechange = null;
if (this.status == 200) {
var accounts = JSON.parse(this.response);
document.getElementById("list").innerHTML = "<ol>";
for (index = 0; index < accounts["value"].length;++index) {
document.getElementById("list").innerHTML +="<li>" + accounts["value"][index].name + "</li>";
}
document.getElementById("list").innerHTML +="</ol>";
}
else {
//TODO handle error
}
}
};
req.send();
}