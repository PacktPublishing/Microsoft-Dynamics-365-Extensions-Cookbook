var Xrm = {};
Xrm.Page = {}

var Attribute = function (attributeName) {
    this.attributeName = attributeName,
    this.setValueCounter = 0;
    this.getValue = function () {
        if (this.attributeName === ("packt_supervisor")) {
            return "Sample Value";
        }
        return null;
    },
    this.setValue = function (value) {
        this.setValueCounter += 1;
    },
    this.getCountFunctionCalls = function () {
        return this.setValueCounter;
    }
}

var pageAttributes = {}

Xrm.Page.getAttribute = function (attributeName) {
    if (pageAttributes[attributeName]) {
        return pageAttributes[attributeName];
    }

    pageAttributes[attributeName] = new Attribute(attributeName);
    return pageAttributes[attributeName];
}
