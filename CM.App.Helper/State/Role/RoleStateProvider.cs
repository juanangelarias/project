using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class RoleStateProvider: BaseStateProvider, IRoleStateProvider
{
    private readonly IRoleService _roleService;

    #region Fields & Properties

    #region Role List

    private List<RoleDto> _roleList = new();

    public List<RoleDto> RoleList
    {
        get => _roleList;
        set
        {
            _roleList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Selected Role

    private RoleDto? _selectedRole;

    public RoleDto? SelectedRole
    {
        get => _selectedRole;
        set
        {
            _selectedRole = value;
            NotifyChanges();
        }
    }

    #endregion

    #region RoleTable

    private TableData<RoleDto> _roleTable = new();

    public TableData<RoleDto> RoleTable
    {
        get => _roleTable;
        set
        {
            _roleTable = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Active Component

    private ManagementFunction _activeComponent = ManagementFunction.List;

    public ManagementFunction ActiveComponent
    {
        get => _activeComponent;
        set
        {
            _activeComponent = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Filter

    private string _filter = string.Empty;

    public string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            NotifyChanges();
        }
    }

    #endregion
    
    #endregion

    public RoleStateProvider(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task LoadRolePage(QueryParams parameters)
    {
        var page = await _roleService.GetPage(parameters);
        RoleList = page.Items.ToList();
        RoleTable = new TableData<RoleDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateRole()
    {
        if (SelectedRole != null)
        {
            var createdRole = await _roleService.Create(SelectedRole);
            if(createdRole != null)
                RoleList.Add(createdRole);

            SelectedRole = null;
        }
    }

    public async Task UpdateRole()
    {
        if (SelectedRole != null)
        {
            var updatedRole = await _roleService.Update(SelectedRole);
            if (updatedRole != null)
            {
                var roleToUpdate = RoleList.First(f => f.Id == updatedRole.Id);
                
                var index = RoleList.IndexOf(roleToUpdate);
                if (index != -1)
                {
                    RoleList[index] = updatedRole;
                    NotifyChanges();
                }
            }

            SelectedRole = null;
        }
    }

    public async Task DeleteRole(long roleId)
    {
        await _roleService.Delete(roleId);
        var roleToDelete = RoleList.First(f => f.Id == roleId);

        RoleList.Remove(roleToDelete);

        SelectedRole = null;
        
        NotifyChanges();
    }

    public RoleDto GetNewRole()
    {
        return new RoleDto
        {
            Code = string.Empty,
            Name = string.Empty,
            Description = string.Empty,
            Timestamp = Array.Empty<byte>()
        };
    }

    public void SetSelectedRole(long roleId)
    {
        SelectedRole = roleId != 0
            ? RoleList.First(f => f.Id == roleId)
            : GetNewRole();
    }
}