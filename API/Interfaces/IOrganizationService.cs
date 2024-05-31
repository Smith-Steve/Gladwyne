using Gladwyne.Models;

namespace Gladwyne.API.Interfaces
{
    public interface IOrganizationService
    {
        void Add(OrganizationDTO organization);
        Organization GetById(int organizationId);
		// void Update(OrganizationDTO organization);
		// Organization Get(int Id);
		// Organization GetDetails(int Id);
    }
}