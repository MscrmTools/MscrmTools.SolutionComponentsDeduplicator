using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace MscrmTools.SolutionComponentsDeduplicator.AppCode
{
    /// <summary>
    /// Class for querying Crm Metadata
    /// </summary>
    internal class MetadataHelper
    {
        public static string RetrieveAttributeDisplayName(EntityMetadata emd, string attributeName, string fetchXml, IOrganizationService oService)
        {
            string rAttributeName = attributeName;
            string rEntityName = string.Empty;

            if (attributeName.Contains("."))
            {
                string[] data = attributeName.ToLower().Split('.');

                if (!string.IsNullOrEmpty(fetchXml))
                {
                    XmlDocument fetchDoc = new XmlDocument();
                    fetchDoc.LoadXml(fetchXml);

                    XmlNode aliasNode = fetchDoc.SelectSingleNode("//link-entity[@alias='" + data[0] + "']");
                    if (aliasNode != null)
                    {
                        var lookupAmd = emd.Attributes.First(a => a.LogicalName == aliasNode.Attributes["to"].Value);

                        EntityMetadata relatedEmd = RetrieveEntity(aliasNode.Attributes["name"].Value, oService);

                        AttributeMetadata relatedamd = (from attr in relatedEmd.Attributes
                                                        where attr.LogicalName == data[1]
                                                        select attr).FirstOrDefault();

                        if (relatedamd == null)
                        {
                            return $"(unknown:{attributeName})";
                        }

                        return $"{relatedamd.DisplayName.UserLocalizedLabel.Label} ({lookupAmd.DisplayName.UserLocalizedLabel.Label})";
                    }
                }

                return "(not found)";
            }
            else
            {
                AttributeMetadata attribute = (from attr in emd.Attributes
                                               where attr.LogicalName == attributeName
                                               select attr).FirstOrDefault();

                if (attribute == null)
                {
                    return string.Format("(unknown:{0})", attributeName);
                }

                return attribute.DisplayName.UserLocalizedLabel.Label;
            }
        }

        /// <summary>
        /// Retrieve list of entities
        /// </summary>
        /// <returns></returns>
        public static List<EntityMetadata> RetrieveEntities(IOrganizationService oService)
        {
            List<EntityMetadata> entities = new List<EntityMetadata>();

            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
            {
                RetrieveAsIfPublished = true,
                EntityFilters = EntityFilters.Entity
            };

            RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)oService.Execute(request);

            foreach (EntityMetadata emd in response.EntityMetadata)
            {
                if (emd.DisplayName.UserLocalizedLabel != null && (emd.IsCustomizable.Value || emd.IsManaged.Value == false))
                {
                    entities.Add(emd);
                }
            }

            return entities;
        }

        public static List<EntityMetadata> RetrieveEntities(IOrganizationService oService, Guid solutionId)
        {
            List<EntityMetadata> entities = new List<EntityMetadata>();

            if (solutionId == Guid.Empty)
            {
                RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest
                {
                    EntityFilters = EntityFilters.Entity
                };

                RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)oService.Execute(request);

                foreach (EntityMetadata emd in response.EntityMetadata)
                {
                    if (emd.DisplayName?.UserLocalizedLabel != null &&
                        (emd.IsCustomizable.Value || emd.IsManaged.Value == false))
                    {
                        entities.Add(emd);
                    }
                }
            }
            else
            {
                var components = oService.RetrieveMultiple(new QueryExpression("solutioncomponent")
                {
                    ColumnSet = new ColumnSet("objectid"),
                    NoLock = true,
                    Criteria = new FilterExpression
                    {
                        Conditions =
                        {
                            new ConditionExpression("solutionid", ConditionOperator.Equal, solutionId),
                            new ConditionExpression("componenttype", ConditionOperator.Equal, 1)
                        }
                    }
                }).Entities;

                var list = components.Select(component => component.GetAttributeValue<Guid>("objectid"))
                    .ToList();

                if (list.Count > 0)
                {
                    int i = 0;
                    List<Guid> metadataIds = list.Take(100).ToList();
                    do
                    {
                        EntityQueryExpression entityQueryExpression = new EntityQueryExpression
                        {
                            Criteria = new MetadataFilterExpression(LogicalOperator.Or),
                            Properties = new MetadataPropertiesExpression
                            {
                                AllProperties = false,
                                PropertyNames = { "DisplayName", "LogicalName", "ObjectTypeCode", "IsIntersect" }
                            }
                        };

                        metadataIds.ForEach(id =>
                        {
                            entityQueryExpression.Criteria.Conditions.Add(new MetadataConditionExpression("MetadataId", MetadataConditionOperator.Equals, id));
                        });

                        RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
                        {
                            Query = entityQueryExpression,
                            ClientVersionStamp = null
                        };

                        var response = (RetrieveMetadataChangesResponse)oService.Execute(retrieveMetadataChangesRequest);
                        entities.AddRange(response.EntityMetadata.Where(e => !(e.IsIntersect ?? false)));
                        i++;
                        metadataIds = list.Skip(i * 100).Take(100).ToList();
                    }
                    while (metadataIds.Count > 0);
                }
            }

            return entities;
        }

        public static List<EntityMetadata> RetrieveEntitiesMainInfo(IOrganizationService oService)
        {
            EntityQueryExpression entityQueryExpression = new EntityQueryExpression
            {
                Criteria = new MetadataFilterExpression(LogicalOperator.Or),
                Properties = new MetadataPropertiesExpression
                {
                    AllProperties = false,
                    PropertyNames = { "DisplayName", "LogicalName", "ObjectTypeCode", "PrimaryNameAttribute", "PrimaryIdAttribute", "Attributes", "OneToManyRelationships", "ManyToOneRelationships", "ManyToManyRelationships", "Keys" }
                },
                AttributeQuery = new AttributeQueryExpression
                {
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = false,
                        PropertyNames = { "DisplayName", "MetadataId", "EntityLogicalName" }
                    }
                },
                RelationshipQuery = new RelationshipQueryExpression
                {
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = false,
                        PropertyNames = { "SchemaName", "Entity1LogicalName", "Entity2LogicalName", "ReferencingEntity", "ReferencedEntity", "MetadataId" }
                    }
                },
                KeyQuery = new EntityKeyQueryExpression
                {
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = false,
                        PropertyNames = { "DisplayName", "MetadataId", "EntityLogicalName" }
                    }
                },
                LabelQuery = new LabelQueryExpression
                {
                    FilterLanguages = { 1036 }
                }
            };

            RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
            {
                Query = entityQueryExpression,
                ClientVersionStamp = null
            };

            return ((RetrieveMetadataChangesResponse)oService.Execute(retrieveMetadataChangesRequest)).EntityMetadata.ToList();
        }

        public static EntityMetadata RetrieveEntity(string logicalName, IOrganizationService oService)
        {
            try
            {
                RetrieveEntityRequest request = new RetrieveEntityRequest
                {
                    LogicalName = logicalName,
                    EntityFilters = EntityFilters.Attributes | EntityFilters.Relationships
                };

                RetrieveEntityResponse response = (RetrieveEntityResponse)oService.Execute(request);

                return response.EntityMetadata;
            }
            catch (Exception error)
            {
                // string errorMessage = CrmExceptionHelper.GetErrorMessage(error, false);
                throw new Exception("Error while retrieving entity: " + error.Message);
            }
        }

        public static EntityMetadata RetrieveEntityMetadata(IOrganizationService service, string entityLogicalName)
        {
            EntityQueryExpression entityQueryExpression = new EntityQueryExpression
            {
                Criteria = new MetadataFilterExpression(LogicalOperator.And)
                {
                    Conditions =
                    {
                        new MetadataConditionExpression("LogicalName", MetadataConditionOperator.Equals, entityLogicalName)
                    }
                },
                Properties = new MetadataPropertiesExpression
                {
                    AllProperties = false,
                    PropertyNames = { "MetadataId", "LogicalName", "Attributes", "OneToManyRelationships", "ManyToOneRelationships", "ManyToManyRelationships", "Keys" }
                },
                AttributeQuery = new AttributeQueryExpression
                {
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = false,
                        PropertyNames = { "SchemaName", "MetadataId", "EntityLogicalName" }
                    }
                },
                RelationshipQuery = new RelationshipQueryExpression
                {
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = false,
                        PropertyNames = { "SchemaName", "MetadataId" }
                    }
                },
                KeyQuery = new EntityKeyQueryExpression
                {
                    Properties = new MetadataPropertiesExpression
                    {
                        AllProperties = false,
                        PropertyNames = { "SchemaName", "MetadataId" }
                    }
                }
            };

            RetrieveMetadataChangesRequest retrieveMetadataChangesRequest = new RetrieveMetadataChangesRequest
            {
                Query = entityQueryExpression,
                ClientVersionStamp = null
            };

            return ((RetrieveMetadataChangesResponse)service.Execute(retrieveMetadataChangesRequest)).EntityMetadata.FirstOrDefault();
        }
    }
}