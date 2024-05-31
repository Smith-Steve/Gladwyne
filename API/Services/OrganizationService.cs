using System.Data;
using Gladwyne.API.Data.Interfaces;
using Gladwyne.API.Interfaces;
using Gladwyne.Models;
using Microsoft.Data.SqlClient;

namespace Gladwyne.API.Services
{
    public class OrganizationService : IOrganizationService
    {
        IDataProvider _data = null;
        public OrganizationService(IDataProvider data)
        {
            _data = data;
        }

        public void Add(OrganizationDTO organization)
        {
            string procedureName = "[GladwyneSchema].[Organization_INSERT_Procedure]";
            _data.ExecuteNonQuery(procedureName,
            inputParamMapper: delegate(SqlParameterCollection collection)
            {
                //adding parameters for query.
                addCommonParameters(organization, collection);
            }, returnParameters: null);
        }

        //Organization Service
        //Get Organization By ID
        public Organization GetById(int organizationId)
        {
            Console.WriteLine("We are now in the serivce.");
            //Our Stored Procedure
            string procedureName = "Execute GladwyneSchema.Organization_GETONE_Procedure";
            Organization organization = null;
            _data.ExecuteCmd(procedureName, inputParamMapper: delegate(SqlParameterCollection parameterCollection)
            {
                //Entering Parameter
                parameterCollection.AddWithValue("@OrgId", organizationId);
            }, delegate(IDataReader reader, short set)
            {
                organization = Map(reader, out int startingIndex);
            });
            return organization;
        }

        private static void addCommonParameters(OrganizationDTO organization, SqlParameterCollection collection)
        {
            collection.AddWithValue("@OrgName", organization.OrgName);
            collection.AddWithValue("@OrgDescription", organization.OrgDescription);
            collection.AddWithValue("@OrgIndustry", organization.OrgIndustry);
            collection.AddWithValue("@OrgWebsite", organization.OrgWebsite);
            collection.AddWithValue("@OrgActive", organization.OrgActive);
        }

        private static Organization Map(IDataReader dataReader, out int index)
        {
            index = 0;
            // Instantiating Our Organization Model.
            Organization organization = new Organization();
            organization.OrgName = dataReader.GetString(index++);
            organization.OrgDescription = dataReader.GetString(index++);
            organization.OrgIndustry = dataReader.GetString(index++);
            organization.OrgWebsite = dataReader.GetString(index++);
            organization.OrgActive = dataReader.GetBoolean(index++);
            return organization;
        }
    }
}