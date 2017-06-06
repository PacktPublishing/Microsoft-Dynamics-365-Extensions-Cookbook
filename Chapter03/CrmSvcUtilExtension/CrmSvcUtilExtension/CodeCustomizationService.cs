using System;
using System.CodeDom;
using Microsoft.Crm.Services.Utility;

namespace Packt.Xrm.CrmSvcUtilExensions
{
    public sealed class CodeCustomizationService : ICustomizeCodeDomService
    {
        public void CustomizeCodeDom(CodeCompileUnit codeUnit, IServiceProvider services)
        {
            for (var i = 0; i < codeUnit.Namespaces.Count; ++i)
            {
                var types = codeUnit.Namespaces[i].Types;
                for (var j = 0; j < types.Count;)
                {
                    if (!types[j].IsEnum || types[j].Members.Count == 0)
                    {
                        types.RemoveAt(j);
                    }
                    else
                    {
                        j += 1;
                    }
                }
            }
        }
    }
}