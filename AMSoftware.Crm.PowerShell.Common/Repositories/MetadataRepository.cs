﻿/*
CRM PowerShell Library
Copyright (C) 2017 Arjan Meskers / AMSoftware

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published
by the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace AMSoftware.Crm.PowerShell.Common.Repositories
{
    public class MetadataRepository
    {
        #region Entity
        public EntityMetadata GetEntity(string name)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveEntity");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("LogicalName", name);
            request.Parameters.Add("EntityFilters", EntityFilters.Entity);
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", default(Guid));

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (EntityMetadata)response.Results["EntityMetadata"];
        }

        public EntityMetadata GetEntity(Guid metadataId)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveEntity");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityFilters", EntityFilters.Entity);
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", metadataId);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (EntityMetadata)response.Results["EntityMetadata"];
        }

        public IEnumerable<EntityMetadata> GetEntity(bool customOnly, bool excludeManaged, bool includeIntersects)
        {
            IEnumerable<EntityMetadata> result = CrmContext.Cache.GetEntities();
            if (customOnly)
            {
                result = result.Where(e => e.IsCustomEntity == true);
            }
            if (excludeManaged)
            {
                result = result.Where(e => e.IsManaged != true);
            }
            if (!includeIntersects)
            {
                result = result.Where(e => e.IsIntersect != true);
            }

            return result;
        }

        public Guid AddEntity(EntityMetadata entity, AttributeMetadata attribute, bool? hasNotes = null, bool? hasActivites = null)
        {
            OrganizationRequest request = new OrganizationRequest("CreateEntity");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Entity", entity);
            request.Parameters.Add("PrimaryAttribute", attribute);
            request.Parameters.Add("HasNotes", hasNotes.GetValueOrDefault());
            request.Parameters.Add("HasActivities", hasActivites.GetValueOrDefault());

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (Guid)response.Results["EntityId"];
        }

        public void UpdateEntity(EntityMetadata entity, bool? hasNotes = null, bool? hasActivites = null)
        {
            OrganizationRequest request = new OrganizationRequest("UpdateEntity");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Entity", entity);
            request.Parameters.Add("HasNotes", hasNotes.GetValueOrDefault());
            request.Parameters.Add("HasActivities", hasActivites.GetValueOrDefault());
            request.Parameters.Add("MergeLabels", true);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void DeleteEntity(string name)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteEntity");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("LogicalName", name);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        private EntityMetadata GetEntity(string name, EntityFilters filter)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveEntity");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("LogicalName", name);
            request.Parameters.Add("EntityFilters", filter);
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", default(Guid));

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (EntityMetadata)response.Results["EntityMetadata"];
        }
        #endregion

        #region OptionSet
        public OptionSetMetadataBase GetOptionSet(string name)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveOptionSet");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Name", name);
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", default(Guid));

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (OptionSetMetadataBase)response.Results["OptionSetMetadata"];
        }

        public OptionSetMetadataBase GetOptionSet(Guid metadataId)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveOptionSet");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", metadataId);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (OptionSetMetadataBase)response.Results["OptionSetMetadata"];
        }

        public IEnumerable<OptionSetMetadataBase> GetOptionSet(bool customOnly, bool excludeManaged)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveAllOptionSets");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("RetrieveAsIfPublished", true);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            IEnumerable<OptionSetMetadataBase> result = ((OptionSetMetadataBase[])response.Results["OptionSetMetadata"]).Cast<OptionSetMetadataBase>();
            result = result
                .Where(e => e.IsCustomOptionSet == customOnly || customOnly == false)              // include if custom, or customOnly not active
                .Where(e => e.IsManaged != excludeManaged || excludeManaged == false);           // exclude if is managed, or excludeManaged not active

            return result;
        }

        public Guid AddOptionSet(OptionSetMetadataBase optionSet)
        {
            OrganizationRequest request = new OrganizationRequest("CreateOptionSet");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("OptionSet", optionSet);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (Guid)response.Results["OptionSetId"];
        }

        public void UpdateOptionSet(OptionSetMetadataBase optionSet)
        {
            OrganizationRequest request = new OrganizationRequest("UpdateOptionSet");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("OptionSet", optionSet);
            request.Parameters.Add("MergeLabels", true);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void DeleteOptionSet(string name)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteOptionSet");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Name", name);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public int AddOptionSetValue(string name, string displayName, int? value = null, string description = null)
        {
            OrganizationRequest request = new OrganizationRequest("InsertOptionValue");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("OptionSetName", name);
            request.Parameters.Add("Label", new Label(displayName, CrmContext.Language));
            if (value.HasValue)
            {
                request.Parameters.Add("Value", value.Value);
            }
            if (description != null)
            {
                request.Parameters.Add("Description", new Label(description, CrmContext.Language));
            }

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (int)response.Results["NewOptionValue"];
        }

        public int AddOptionSetValue(string entity, string attribute, string displayName, int? value = null, string description = null)
        {
            OrganizationRequest request = new OrganizationRequest("InsertOptionValue");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("AttributeLogicalName", attribute);
            request.Parameters.Add("Label", new Label(displayName, CrmContext.Language));
            if (value.HasValue)
            {
                request.Parameters.Add("Value", value.Value);
            }
            if (description != null)
            {
                request.Parameters.Add("Description", new Label(description, CrmContext.Language));
            }

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (int)response.Results["NewOptionValue"];
        }

        public void DeleteOptionSetValue(string name, int value)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteOptionValue");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("OptionSetName", name);
            request.Parameters.Add("Value", value);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void DeleteOptionSetValue(string entity, string attribute, int value)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteOptionValue");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("AttributeLogicalName", attribute);
            request.Parameters.Add("Value", value);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void UpdateOptionSetValue(string name, int value, string displayName = null, string description = null)
        {
            OrganizationRequest request = new OrganizationRequest("UpdateOptionValue");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("OptionSetName", name);
            request.Parameters.Add("Value", value);
            request.Parameters.Add("MergeLabels", true);

            if (!string.IsNullOrEmpty(displayName))
            {
                request.Parameters.Add("Label", new Label(displayName, CrmContext.Language));
            }
            if (description != null)
            {
                request.Parameters.Add("Description", new Label(description, CrmContext.Language));
            }

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void UpdateOptionSetValue(string entity, string attribute, int value, string displayName = null, string description = null)
        {
            OrganizationRequest request = new OrganizationRequest("UpdateOptionValue");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("AttributeLogicalName", attribute);
            request.Parameters.Add("Value", value);
            request.Parameters.Add("MergeLabels", true);

            if (!string.IsNullOrEmpty(displayName))
            {
                request.Parameters.Add("Label", new Label(displayName, CrmContext.Language));
            }
            if (description != null)
            {
                request.Parameters.Add("Description", new Label(description, CrmContext.Language));
            }

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }
        #endregion

        #region Attribute
        public AttributeMetadata GetAttribute(Guid id)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveAttribute");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", id);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (AttributeMetadata)response.Results["AttributeMetadata"];
        }

        public AttributeMetadata GetAttribute(string entity, string attribute)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveAttribute");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("LogicalName", attribute);
            request.Parameters.Add("RetrieveAsIfPublished", true);
            request.Parameters.Add("MetadataId", default(Guid));

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (AttributeMetadata)response.Results["AttributeMetadata"];
        }

        public IEnumerable<AttributeMetadata> GetAttribute(string entity, bool customOnly, bool excludeManaged, bool includeLinked)
        {
            EntityMetadata entityData = GetEntity(entity, EntityFilters.Attributes);

            IEnumerable<AttributeMetadata> result = entityData.Attributes.AsEnumerable<AttributeMetadata>();
            if (customOnly)
            {
                result = result.Where(a => a.IsCustomAttribute == true);
            }
            if (excludeManaged)
            {
                result = result.Where(a => a.IsManaged != true);
            }
            if (!includeLinked)
            {
                result = result.Where(a => string.IsNullOrWhiteSpace(a.AttributeOf));
            }

            return result;
        }

        public Guid AddAttribute(string entity, AttributeMetadata attribute)
        {
            OrganizationRequest request = new OrganizationRequest("CreateAttribute");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityName", entity);
            request.Parameters.Add("Attribute", attribute);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (Guid)response.Results["AttributeId"];
        }

        public void UpdateAttribute(string entity, AttributeMetadata attribute)
        {
            OrganizationRequest request = new OrganizationRequest("UpdateAttribute");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityName", entity);
            request.Parameters.Add("Attribute", attribute);
            request.Parameters.Add("MergeLabels", true);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void DeleteAttribute(string entity, string attribute)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteAttribute");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("LogicalName", attribute);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }
        #endregion

        #region EntityKey
        public EntityKeyMetadata GetEntityKey(Guid id)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveEntityKey");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("RetrieveAsIfPublished", false);
            request.Parameters.Add("MetadataId", id);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (EntityKeyMetadata)response.Results["EntityKeyMetadata"];
        }

        public EntityKeyMetadata GetEntityKey(string entity, string key)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveEntityKey");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("LogicalName", key);
            request.Parameters.Add("RetrieveAsIfPublished", false);
            request.Parameters.Add("MetadataId", default(Guid));

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (EntityKeyMetadata)response.Results["EntityKeyMetadata"];
        }

        public IEnumerable<EntityKeyMetadata> GetEntityKey(string entity, bool excludeManaged)
        {
            EntityMetadata entityData = GetEntity(entity, EntityFilters.Attributes);

            IEnumerable<EntityKeyMetadata> result = entityData.Keys.AsEnumerable<EntityKeyMetadata>();
            if (excludeManaged)
            {
                result = result.Where(a => a.IsManaged != true);
            }

            return result;
        }

        public Guid AddEntityKey(string entity, EntityKeyMetadata key)
        {
            OrganizationRequest request = new OrganizationRequest("CreateEntityKey");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityName", entity);
            request.Parameters.Add("EntityKey", key);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (Guid)response.Results["EntityKeyId"];
        }

        public void DeleteEntityKey(string entity, string key)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteEntityKey");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityLogicalName", entity);
            request.Parameters.Add("Name", key);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }
        #endregion

        #region Relationship
        public IEnumerable<RelationshipMetadataBase> GetRelationship(string entity, string relatedEntity, CrmRelationshipType relationshipType, bool customOnly, bool excludeManaged)
        {
            EntityMetadata entityData = GetEntity(entity, EntityFilters.Relationships);
            List<RelationshipMetadataBase> resultAsList = new List<RelationshipMetadataBase>();

            if (string.IsNullOrWhiteSpace(relatedEntity))
            {
                if ((relationshipType == CrmRelationshipType.All || relationshipType == CrmRelationshipType.ManyToMany) && entityData.ManyToManyRelationships != null)
                {
                    resultAsList.AddRange(entityData.ManyToManyRelationships);
                }
                if (relationshipType == CrmRelationshipType.All || relationshipType == CrmRelationshipType.OneToMany)
                {
                    if (entityData.OneToManyRelationships != null)
                        resultAsList.AddRange(entityData.OneToManyRelationships);
                    if (entityData.ManyToOneRelationships != null)
                        resultAsList.AddRange(entityData.ManyToOneRelationships);
                }
            }
            else
            {
                if ((relationshipType == CrmRelationshipType.All || relationshipType == CrmRelationshipType.ManyToMany) && entityData.ManyToManyRelationships != null)
                {
                    resultAsList.AddRange(
                        entityData.ManyToManyRelationships.Where(r =>
                            r.Entity2LogicalName.Equals(relatedEntity, StringComparison.InvariantCultureIgnoreCase) ||
                            r.Entity1LogicalName.Equals(relatedEntity, StringComparison.InvariantCultureIgnoreCase)));
                }
                if (relationshipType == CrmRelationshipType.All || relationshipType == CrmRelationshipType.OneToMany)
                {
                    if (entityData.OneToManyRelationships != null)
                        resultAsList.AddRange(
                                entityData.OneToManyRelationships.Where(r =>
                                    r.ReferencedEntity.Equals(relatedEntity, StringComparison.InvariantCultureIgnoreCase) ||
                                    r.ReferencingEntity.Equals(relatedEntity, StringComparison.InvariantCultureIgnoreCase)));
                    if (entityData.ManyToOneRelationships != null)
                        resultAsList.AddRange(
                            entityData.ManyToOneRelationships.Where(r =>
                                r.ReferencedEntity.Equals(relatedEntity, StringComparison.InvariantCultureIgnoreCase) ||
                                r.ReferencingEntity.Equals(relatedEntity, StringComparison.InvariantCultureIgnoreCase)));
                }
            }

            IEnumerable<RelationshipMetadataBase> result = resultAsList.AsEnumerable<RelationshipMetadataBase>();
            if (customOnly)
            {
                result = result.Where(r => r.IsCustomRelationship == true);
            }
            if (excludeManaged)
            {
                result = result.Where(r => r.IsManaged != true);
            }

            return result;
        }

        public RelationshipMetadataBase GetRelationship(Guid id)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveRelationship");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("MetadataId", id);
            request.Parameters.Add("RetrieveAsIfPublished", true);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (RelationshipMetadataBase)response.Results["RelationshipMetadata"];
        }

        public RelationshipMetadataBase GetRelationship(string name)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveRelationship");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Name", name);
            request.Parameters.Add("RetrieveAsIfPublished", true);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (RelationshipMetadataBase)response.Results["RelationshipMetadata"];
        }

        public Guid AddRelationship(OneToManyRelationshipMetadata relationship, LookupAttributeMetadata lookup)
        {
            bool isReferencingEligible = EligibleForRelationship(relationship.ReferencingEntity, "CanBeReferencing");
            bool isReferencedEligible = EligibleForRelationship(relationship.ReferencedEntity, "CanBeReferenced");

            if (!(isReferencedEligible && isReferencingEligible))
            {
                throw new Exception(string.Format("One-to-Many relationship between '{0}' and '{1}' is not allowed.",
                    relationship.ReferencingEntity, relationship.ReferencedEntity));
            }

            OrganizationRequest request = new OrganizationRequest("CreateOneToMany");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("OneToManyRelationship", relationship);
            request.Parameters.Add("Lookup", lookup);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (Guid)response.Results["RelationshipId"];
        }

        public Guid AddRelationship(ManyToManyRelationshipMetadata relationship, string intersect)
        {
            bool isEligible1 = EligibleForRelationship(relationship.Entity1LogicalName, "CanManyToMany");
            bool isEligible2 = EligibleForRelationship(relationship.Entity2LogicalName, "CanManyToMany");

            if (!(isEligible1 && isEligible2))
            {
                throw new Exception(string.Format("Many-to-Many relationship between '{0}' and '{1}' is not allowed.",
                    relationship.Entity1LogicalName, relationship.Entity2LogicalName));
            }

            OrganizationRequest request = new OrganizationRequest("CreateManyToMany");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("IntersectEntitySchemaName", intersect);
            request.Parameters.Add("ManyToManyRelationship", relationship);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
            return (Guid)response.Results["ManyToManyRelationshipId"];
        }

        public void UpdateRelationship(RelationshipMetadataBase relationship)
        {
            OrganizationRequest request = new OrganizationRequest("UpdateRelationship");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Relationship", relationship);
            request.Parameters.Add("MergeLabels", true);

            if (CrmContext.ActiveSolution != null)
            {
                request.Parameters.Add("SolutionUniqueName", CrmContext.ActiveSolution);
            }

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void DeleteRelationship(string relationship)
        {
            OrganizationRequest request = new OrganizationRequest("DeleteRelationship");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("Name", relationship);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);
        }

        public void DeleteRelationship(string entity, string fromEntity, string attributeHint = null)
        {
            RelationshipMetadataBase relationship = FindAssociationRelationship(fromEntity, entity, attributeHint);
            if (relationship == null)
            {
                // No relationship between entities. Throw exception
                throw new KeyNotFoundException(string.Format("No (unique) relationship from {0} to {1}", entity, fromEntity));
            }

            DeleteRelationship(relationship.SchemaName);
        }

        internal RelationshipMetadataBase FindAssociationRelationship(string referencedEntity, string referencingEntity, string referencingAttribute = null)
        {
            var relationships = this
                .GetRelationship(referencedEntity, referencingEntity, CrmRelationshipType.All, false, false)
                .Where(r =>
                    r.RelationshipType == RelationshipType.ManyToManyRelationship ||
                    (r.RelationshipType == RelationshipType.OneToManyRelationship &&
                        ((OneToManyRelationshipMetadata)r).ReferencedEntity.Equals(referencedEntity, StringComparison.InvariantCultureIgnoreCase) &&
                        ((OneToManyRelationshipMetadata)r).ReferencingEntity.Equals(referencingEntity, StringComparison.InvariantCultureIgnoreCase)))
                .ToList();

            RelationshipMetadataBase relationship = null;
            if (relationships.Count == 1)
            {
                // Exactly one relationship found. Ignore relatedAttribute hint, and use this relationship
                relationship = relationships[0];
            }
            else if (relationships.Count > 1)
            {
                if (!string.IsNullOrWhiteSpace(referencingAttribute))
                {
                    // Assume one-to-many relationship and use relatedAttribute hint to located the correct one.
                    relationship = relationships
                        .Where(r =>
                            r.RelationshipType == RelationshipType.OneToManyRelationship &&
                            ((OneToManyRelationshipMetadata)r).ReferencingAttribute.Equals(referencingAttribute, StringComparison.InvariantCultureIgnoreCase))
                        .SingleOrDefault();
                }

                if (relationship == null)
                {
                    //No relationship found using hint. Assume Many-to-Many relationship. Try to find single one.
                    relationship = relationships
                        .Where(r => r.RelationshipType == RelationshipType.ManyToManyRelationship)
                        .SingleOrDefault();
                }
            }

            return relationship;
        }

        private bool EligibleForRelationship(string entity, string relationCheck)
        {
            OrganizationRequest request = new OrganizationRequest(relationCheck);
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("EntityName", entity);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (bool)response.Results[relationCheck];
        }
        #endregion

        #region Managed Property
        public ManagedPropertyMetadata GetManagedProperty(Guid metadataId)
        {
            OrganizationRequest request = new OrganizationRequest("RetrieveManagedProperty");
            request.Parameters = new ParameterCollection();
            request.Parameters.Add("MetadataId", metadataId);

            OrganizationResponse response = CrmContext.OrganizationProxy.Execute(request);

            return (ManagedPropertyMetadata)response.Results["ManagedPropertyMetadata"];
        }

        #endregion
    }
}