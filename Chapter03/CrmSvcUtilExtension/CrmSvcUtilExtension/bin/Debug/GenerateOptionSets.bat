 CrmSvcUtil.exe ^
/codewriterfilter:"Packt.Xrm.CrmSvcUtilExensions.PacktFiltering, Packt.Xrm.CrmSvcUtilExensions" ^
/connectionstring:"AuthType=Office365;Username=ramim@.onmicrosoft.com; Password=;Url=https://.crm6.dynamics.com" ^
/namespace:Packt.Xrm.Entities ^
/out:Entities.cs ^
/filter:packt_

 CrmSvcUtil.exe ^
/codewriterfilter:"Packt.Xrm.CrmSvcUtilExensions.PacktOptionSetFiltering, Packt.Xrm.CrmSvcUtilExensions" ^
/codecustomization:"Packt.Xrm.CrmSvcUtilExensions.CodeCustomizationService, Packt.Xrm.CrmSvcUtilExensions" ^
/connectionstring:"AuthType=Office365;Username=ramim@.onmicrosoft.com; Password=;Url=https://.crm6.dynamics.com" ^
/namespace:Packt.Xrm.Entities ^
/out:OptionSets.cs ^
/filter:packt_