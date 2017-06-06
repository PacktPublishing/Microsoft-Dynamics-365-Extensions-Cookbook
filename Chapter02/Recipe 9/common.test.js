/// <reference path="common.js"/>
/// <reference path="qunit-2.0.1.js" />
/// <reference path="Fake.Xrm.js"/>

test("Xrm test populateWithTodaysDate", function () {
    // Act
    packtNs.common.populateWithTodaysDate();

    //Assert
    equal(Xrm.Page.getAttribute("packt_postgraduatestartdate"). getCountSetValueCalls(), 1, "PostGraduate attribute setValue called exactly once");
});
