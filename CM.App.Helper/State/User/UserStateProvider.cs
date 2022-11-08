using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class UserStateProvider: BaseStateProvider, IUserStateProvider
{
    private readonly IUserService _userService;

    #region Fields & Properties

    #region User List

    private List<UserDto>? _userList;

    public List<UserDto>? UserList
    {
        get => _userList;
        set
        {
            _userList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Selected User

    private UserDto? _selectedUser;

    public UserDto? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            NotifyChanges();
        }
    }

    #endregion

    #region User Table

    private TableData<UserDto>? _userTable;

    public TableData<UserDto>? UserTable
    {
        get => _userTable;
        set
        {
            _userTable = value;
            NotifyChanges();
        }
    }

    #endregion

    #region UserCount

    private int? _userCount;

    public int? UserCount
    {
        get => _userCount;
        set
        {
            _userCount = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Role List

    private List<RoleDto>? _roleList;
    public List<RoleDto>? RoleList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Assigned Roles List

    private List<RoleDto?> _assignedRoleList;

    public List<RoleDto?> AssignedRoleList
    {
        get => _assignedRoleList;
        set
        {
            _assignedRoleList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Available Roles List

    private List<RoleDto>? _availableRoleList;

    public List<RoleDto>? AvailableRoleList
    {
        get => _availableRoleList;
        set
        {
            _availableRoleList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Assigned Roles Table

    private TableData<RoleDto>? _assignedRoleTable;

    public TableData<RoleDto>? AssignedRoleTable
    {
        get => _assignedRoleTable;
        set
        {
            _assignedRoleTable = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Available Roles Table

    private TableData<RoleDto>? _availableRoleTable;

    public TableData<RoleDto>? AvailableRoleTable
    {
        get => _availableRoleTable;
        set
        {
            _availableRoleTable = value;
            NotifyChanges();
        }
    }

    #endregion

    #region FilterAssigned

    private string? _filterAssigned;

    public string? FilterAssigned
    {
        get => _filterAssigned;
        set
        {
            _filterAssigned = value;
            //FilterAssignedRoles();
            NotifyChanges();
        }
    }

    #endregion

    #region FilterAvailable

    private string? _filterAvailable;

    public string FilterAvailable
    {
        get => _filterAvailable ?? "";
        set
        {
            _filterAvailable = value;
            //FilterAvailableRoles();
            NotifyChanges();
        }
    }

    #endregion

    #endregion

    public UserStateProvider(IUserService userService)  //, IRoleService roleService, IUserRoleService userRoleService)
    {
        _userService = userService;

        _availableRoleList = new List<RoleDto>();
        _assignedRoleList = new List<RoleDto?>();

        _filterAssigned = "";
        _filterAvailable = "";
    }

    public async Task GetUsers()
    {
        await GetRoles();
        UserList = await _userService.Get();
    }

    public async Task LoadUserPage(QueryParams parameters)
    {
        await GetRoles();
        var page = await _userService.GetPage(parameters);
        UserList = page.Items.ToList();
        UserCount = page.Count;
        UserTable = new TableData<UserDto>()
        {
            Items = page.Items,
            TotalItems = page.Count
        };

        NotifyChanges();
    }

    public void FilterAssignedRoles()
    {
        var filter = FilterAssigned ?? "";
        var items = AssignedRoleList
            .Where(f => (f?.Name.ToLower()??"").Contains(filter.ToLower()) ||
                        (f?.Description != null &&
                         f.Description.ToLower().Contains(filter.ToLower())))
            .OrderBy(o => o?.Name)
            .ToList();
        
        var count = AssignedRoleList
            .Count(f => (f?.Name.ToLower()??"").Contains(filter.ToLower()) ||
                        (f?.Description != null &&
                         f.Description.ToLower().Contains(filter.ToLower())));
        
        AssignedRoleTable = new TableData<RoleDto>
        {
            Items = items!,
            TotalItems = count
        };
    }

    public void FilterAvailableRoles()
    {
        var filter = FilterAvailable.ToLower();
        var items = AvailableRoleList?
            .Where(f => f.Name.ToLower().Contains(filter) ||
                        (f.Description != null
                         && f.Description.ToLower().Contains(filter)))
            .OrderBy(o => o.Name)
            .ToList();
        
        var count = AvailableRoleList?
            .Count(f => f.Name.ToLower().Contains(filter) ||
                        (f.Description != null &&
                         f.Description.ToLower().Contains(filter))); 
        
        AvailableRoleTable = new TableData<RoleDto>
        {
            Items = items,
            TotalItems = count ?? 0
        };
    }

    public async Task AssignRole(RoleDto role, string filterAssigned, string filterAvailable)
    {
        if (SelectedUser != null)
        {
            var userRoleToAdd = new UserRoleDto
            {
                UserId = SelectedUser?.Id ?? 0,
                User = null,
                RoleId = role.Id,
                Role = null,
                Timestamp = Array.Empty<byte>()
            };

            await _userService.AddRoleToUser(userRoleToAdd);

            var toRemoveAvailable = AvailableRoleList?
                .First(f => f.Id == role.Id);

            if (toRemoveAvailable != null)
            {
                AvailableRoleList?.Remove(toRemoveAvailable);
            }

            AssignedRoleList.Add(role);

            FilterAssigned = filterAssigned;
            FilterAvailable = filterAvailable;

            FilterAssignedRoles();
            FilterAvailableRoles();
        }
    }

    public async Task RemoveRole(RoleDto role, string filterAssigned, string filterAvailable)
    {
        return;
        /*var userRoleToRemove = SelectedUser?.UserRoles
            .FirstOrDefault(f => f.RoleId == role.Id);

        if (userRoleToRemove != null)
        {
            await _userService.RemoveRoleFromUser(userRoleToRemove.Id);
            SelectedUser?.UserRoles.Remove(userRoleToRemove);
        }

        var toRemoveAssigned = AssignedRoleList
            .First(f => f?.Id == role.Id);

        if (toRemoveAssigned != null)
        {
            AssignedRoleList.Remove(toRemoveAssigned);
        }
        
        AvailableRoleList?.Add(role);

        FilterAssigned = filterAssigned;
        FilterAvailable = filterAvailable;
        
        FilterAssignedRoles();
        FilterAvailableRoles();*/
    }

    public async Task CreateUser()
    {
        if (SelectedUser != null)
        {
            var createdUser = await _userService.Create(SelectedUser);
            if (createdUser != null)
                UserList?.Add(createdUser);

            SelectedUser = null;
        }
    }

    public async Task UpdateUser()
    {
        if (SelectedUser != null)
        {
            var updatedUser = await _userService.Update(SelectedUser);

            if (updatedUser != null)
            {
                var userToUpdate = UserList?.First(f => f.Id == updatedUser.Id);
                if (userToUpdate != null)
                {
                    var index = UserList?.IndexOf(userToUpdate);
                    if (index != null && index != -1)
                    {
                        UserList![(int) index] = updatedUser;
                        NotifyChanges();
                    }
                }
            }
        }
    }

    public async Task DeleteUser(long userId)
    {
        await _userService.Delete(userId);

        var userToDelete = UserList?.First(f => f.Id == userId);
        if (userToDelete != null)
            UserList?.Remove(userToDelete);

        SelectedUser = null;

        NotifyChanges();
    }

    public UserDto GetNewUser()
    {
        return new UserDto
        {
            Id = 0,
            FirstName = string.Empty,
            LastName = string.Empty,
            UserName = string.Empty,
            Timestamp = Array.Empty<byte>()
        };
    }

    public async Task SetSelectedUser(long userId)
    {
        /*try
        {
            SelectedUser = userId != 0 
                ? UserList?.FirstOrDefault(f => f.Id == userId)
                : GetNewUser();

            if (SelectedUser?.Id == 0)
            {
                AssignedRoleList = new List<RoleDto?>();
            }
            else
            {
                var userRoles = await _userRoleService.GetByUser(userId);
                AssignedRoleList = userRoles?.Select(s => s.Role).ToList() ?? new List<RoleDto?>();
            }
        
            AvailableRoleList = RoleList?
                .Where(r => AssignedRoleList.All(a => a?.Id != r.Id))
                .ToList();

            AvailableRoleTable = new TableData<RoleDto>
            {
                Items = AvailableRoleList,
                TotalItems = AvailableRoleList?.Count ?? 0
            };

            AssignedRoleTable = new TableData<RoleDto>
            {
                Items = AssignedRoleList!,
                TotalItems = AssignedRoleList.Count
            };
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }*/
    }

    public async Task GetRoles()
    {
        /*if (RoleList == null || RoleList.Count == 0)
        {
            RoleList = await _roleService.Get();
        }*/
    }
}