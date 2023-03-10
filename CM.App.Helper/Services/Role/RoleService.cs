using CM.App.Helper.State;
using CM.Model.Dto;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CM.App.Helper.Services;

public class RoleService : BaseService<RoleDto>, IRoleService
{
    public RoleService(NavigationManager navigation, IGeneralStateProvider state, HttpClient client,
        ISnackbar snackbar) : base(navigation, state, client, snackbar)
    {
        BaseUrl = "api/role/v1";
    }
}