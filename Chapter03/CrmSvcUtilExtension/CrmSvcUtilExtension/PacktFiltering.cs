using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;

namespace Packt.Xrm.CrmSvcUtilExensions
{
    public sealed class PacktFiltering : ICodeWriterFilterService
    {
        private ICodeWriterFilterService DefaultService { get; }
        public PacktFiltering(ICodeWriterFilterService defaultService)
        {
            DefaultService = defaultService;
        }

        bool ICodeWriterFilterService.GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            return DefaultService.GenerateAttribute(attributeMetadata, services);
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
            return DefaultService.GenerateOptionSet(optionSetMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateRelationship(RelationshipMetadataBase relationshipMetadata,
            EntityMetadata otherEntityMetadata,
            IServiceProvider services)
        {
            return DefaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateServiceContext(IServiceProvider services)
        {
            return DefaultService.GenerateServiceContext(services);
        }

        private static string GetFilter()
        {
            var filterArgument = Environment.GetCommandLineArgs().FirstOrDefault(p => p.ToLower().StartsWith("/filter"));
            return filterArgument?.Substring(filterArgument.IndexOf(":") + 1).Trim('"').Trim() ?? string.Empty;

        }
    }
}