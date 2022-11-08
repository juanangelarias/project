using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public interface IUserStateProvider
{
    public List<UserDto>? UserList { get; set; }
    public UserDto? SelectedUser { get; set; }
    public TableData<UserDto>? UserTable { get; set; }
    public int? UserCount { get; set; }

    //public List<RoleDto>? RoleList { get; set; }
    public List<RoleDto?> AssignedRoleList { get; set; }
    public List<RoleDto>? AvailableRoleList { get; set; }
    public TableData<RoleDto>? AssignedRoleTable { get; set; }
    public TableData<RoleDto>? AvailableRoleTable { get; set; }
    
    public string? FilterAssigned { get; set; }
    public string FilterAvailable { get; set; }

    Task GetUsers();
    Task LoadUserPage(QueryParams parameters);
    Task GetRoles();
    Task CreateUser();
    Task UpdateUser();
    Task DeleteUser(long userId);
    void FilterAssignedRoles();
    void FilterAvailableRoles();
    Task AssignRole(RoleDto role, string filterAssigned, string filterAvailable);
    Task RemoveRole(RoleDto role, string filterAssigned, string filterAvailable);
    UserDto GetNewUser();
    Task SetSelectedUser(long userId);
}