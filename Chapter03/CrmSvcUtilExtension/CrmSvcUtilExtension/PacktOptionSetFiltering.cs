using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;

namespace Packt.Xrm.CrmSvcUtilExensions
{
    public sealed class PacktOptionSetFiltering : ICodeWriterFilterService
    {
        private ICodeWriterFilterService DefaultService { get; }
        private Dictionary<string, bool> GeneratedOptionSets { get; }
        public PacktOptionSetFiltering(ICodeWriterFilterService defaultService)
        {
            DefaultService = defaultService;
            GeneratedOptionSets = new Dictionary<string, bool>();
        }

        bool ICodeWriterFilterService.GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return (attributeMetadata.AttributeType == AttributeTypeCode.Picklist
                || attributeMetadata.AttributeType == AttributeTypeCode.State
                || attributeMetadata.AttributeType == AttributeTypeCode.Status);
        }

        bool ICodeWriterFilterService.GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {

            return entityMetadata.SchemaName.StartsWith(GetFilter()) && DefaultService.GenerateEntity(entityMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            return DefaultService.GenerateOption(optionMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateOptionSet(OptionSetMetadataBase optionSetMetadata,
            IServiceProvider services)
        {
            if (!optionSetMetadata.Name.StartsWith(GetFilter()) || GeneratedOptionSets.ContainsKey(optionSetMetadata.Name))
                return false;
            if (optionSetMetadata.IsGlobal.HasValue && optionSetMetadata.IsGlobal.Value)
                GeneratedOptionSets[optionSetMetadata.Name] = true;
            return true;
        }

        bool ICodeWriterFilterService.GenerateRelationship(RelationshipMetadataBase relationshipMetadata,
            EntityMetadata otherEntityMetadata,
            IServiceProvider services)
        {
            return false;
        }

        bool ICodeWriterFilterService.GenerateServiceContext(IServiceProvider services)
        {
            return false;
        }

        private string GetFilter()
        {
            var filterArgument = Environment.GetCommandLineArgs().FirstOrDefault(p => p.ToLower().StartsWith("/filter"));
            return filterArgument?.Substring(filterArgument.IndexOf(":") + 1).Trim() ?? string.Empty;

        }
    }
}