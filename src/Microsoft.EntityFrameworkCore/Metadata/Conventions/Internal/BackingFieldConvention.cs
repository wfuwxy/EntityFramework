// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class BackingFieldConvention : IPropertyConvention, INavigationConvention
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder)
        {
            Apply(propertyBuilder.Metadata);

            return propertyBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual InternalRelationshipBuilder Apply(InternalRelationshipBuilder relationshipBuilder, Navigation navigation)
        {
            Apply(navigation);

            return relationshipBuilder;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual void Apply([NotNull] PropertyBase propertyBase)
        {
            if (ConfigurationSource.Convention.Overrides(propertyBase.GetFieldInfoConfigurationSource()))
            {
                foreach (var type in propertyBase.DeclaringEntityType.ClrType.GetTypesInHierarchy().ToList())
                {
                    var fieldInfo = TryMatchFieldName(type, propertyBase.ClrType, propertyBase.Name);
                    if (fieldInfo != null)
                    {
                        propertyBase.SetFieldInfo(fieldInfo, ConfigurationSource.Convention, runConventions: false);
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        protected virtual FieldInfo TryMatchFieldName(
            [NotNull] Type entityType, [NotNull] Type propertyType, [NotNull] string propertyName)
        {
            var fields = entityType.GetRuntimeFields().ToDictionary(f => f.Name);

            var camelized = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);

            var typeInfo = propertyType.GetTypeInfo();

            FieldInfo fieldInfo;
            return (fields.TryGetValue("<" + propertyName + ">k__BackingField", out fieldInfo)
                    && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                   || (fields.TryGetValue(propertyName, out fieldInfo)
                       && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                   || (fields.TryGetValue(camelized, out fieldInfo)
                       && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                   || (fields.TryGetValue("_" + camelized, out fieldInfo)
                       && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                   || (fields.TryGetValue("_" + propertyName, out fieldInfo)
                       && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                   || (fields.TryGetValue("m_" + camelized, out fieldInfo)
                       && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                   || (fields.TryGetValue("m_" + propertyName, out fieldInfo)
                       && fieldInfo.FieldType.GetTypeInfo().IsAssignableFrom(typeInfo))
                ? fieldInfo
                : null;
        }
    }
}
