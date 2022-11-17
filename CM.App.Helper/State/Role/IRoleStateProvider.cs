using CM.App.Helper.Model.Enum;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface IRoleStateProvider
{
    List<RoleDto> RoleList { get; set; }
    RoleDto? SelectedRole { get; set; }
    TableData<RoleDto> RoleTable { get; set; }
    ManagementFunction ActiveComponent { get; set; }
    string Filter { get; set; }

    Task LoadRolePage(QueryParams parameters);
    Task CreateRole();
    Task UpdateRole();
    Task DeleteRole(long roleId);
    RoleDto GetNewRole();
    void SetSelectedRole(long roleId);
}