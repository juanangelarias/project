using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class ClubTypeStateProvider: BaseStateProvider, IClubTypeStateProvider
{
    private readonly IClubTypeService _clubTypeService;
    
    #region Fields & Properties

    #region ClubType List

    private List<ClubTypeDto> _clubTypeList = new();

    public List<ClubTypeDto> ClubTypeList
    {
        get => _clubTypeList;
        set
        {
            _clubTypeList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Selected ClubType

    private ClubTypeDto? _selectedClubType;

    public ClubTypeDto? SelectedClubType
    {
        get => _selectedClubType;
        set
        {
            _selectedClubType = value;
            NotifyChanges();
        }
    }

    #endregion

    #region ClubTypeTable

    private TableData<ClubTypeDto> _clubTypeTable = new();

    public TableData<ClubTypeDto> ClubTypeTable
    {
        get => _clubTypeTable;
        set
        {
            _clubTypeTable = value;
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

    public ClubTypeStateProvider(IClubTypeService clubTypeService)
    {
        _clubTypeService = clubTypeService;
    }

    public async Task LoadClubTypePage(QueryParams parameters)
    {
        var page = await _clubTypeService.GetPage(parameters);
        ClubTypeList = page.Items.ToList();
        ClubTypeTable = new TableData<ClubTypeDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateClubType()
    {
        if (SelectedClubType != null)
        {
            var createdClubType = await _clubTypeService.Create(SelectedClubType);
            if(createdClubType != null)
                ClubTypeList.Add(createdClubType);

            SelectedClubType = null;
        }
    }

    public async Task UpdateClubType()
    {
        if (SelectedClubType != null)
        {
            var updatedClubType = await _clubTypeService.Update(SelectedClubType);
            if (updatedClubType != null)
            {
                var clubTypeToUpdate = ClubTypeList.First(f => f.Id == updatedClubType.Id);
                
                var index = ClubTypeList.IndexOf(clubTypeToUpdate);
                if (index != -1)
                {
                    ClubTypeList[index] = updatedClubType;
                    NotifyChanges();
                }
            }

            SelectedClubType = null;
        }
    }

    public async Task DeleteClubType(long clubTypeId)
    {
        await _clubTypeService.Delete(clubTypeId);
        var clubTypeToDelete = ClubTypeList.First(f => f.Id == clubTypeId);

        ClubTypeList.Remove(clubTypeToDelete);

        SelectedClubType = null;
        
        NotifyChanges();
    }

    public ClubTypeDto GetNewClubType()
    {
        return new ClubTypeDto
        {
            Name = string.Empty,
            Timestamp = Array.Empty<byte>()
        };
    }

    public void SetSelectedClubType(long clubTypeId)
    {
        SelectedClubType = clubTypeId != 0
            ? ClubTypeList.First(f => f.Id == clubTypeId)
            : GetNewClubType();
    }
}