using CM.App.Helper.Model.Enum;
using CM.App.Helper.Services;
using CM.Model.Dto;
using CM.Model.General;
using MudBlazor;

namespace CM.App.Helper.State;

public class MemberStateProvider: BaseStateProvider, IMemberStateProvider
{
    private readonly IMemberService _memberService;
    private readonly IClubService _clubService;

    #region Field & Properties

    #region Member List

    private List<MemberDto> _memberList = new();

    public List<MemberDto> MemberList
    {
        get => _memberList;
        set
        {
            _memberList = value;
            NotifyChanges();
        }
    }

    #endregion

    #region Selected Member

    private MemberDto? _selectedMember;

    public MemberDto? SelectedMember
    {
        get => _selectedMember;
        set
        {
            _selectedMember = value;
            NotifyChanges();
        }
    }

    #endregion

    #region MemberTable

    private TableData<MemberDto> _memberTable = new();

    public TableData<MemberDto> MemberTable
    {
        get => _memberTable;
        set
        {
            _memberTable = value;
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

    #region Clubs

    private List<ClubDto> _clubs = new();

    public List<ClubDto> Clubs
    {
        get
        {
            if(!_clubs.Any())
                Task.FromResult(GetClubs);
            return _clubs;
        }
        set
        {
            _clubs = value;
            NotifyChanges();
        }
    }

    #endregion

    #endregion

    public MemberStateProvider(IMemberService memberService, IClubService clubService)
    {
        _memberService = memberService;
        _clubService = clubService;
    }
    
    public async Task LoadMemberPage(QueryParams parameters)
    {
        var page = await _memberService.GetPage(parameters);
        MemberList = page.Items.ToList();
        MemberTable = new TableData<MemberDto>
        {
            Items = page.Items,
            TotalItems = page.Count
        };
        
        NotifyChanges();
    }

    public async Task CreateMember()
    {
        if (SelectedMember != null)
        {
            var createdMember = await _memberService.Create(SelectedMember);
            if(createdMember != null)
                MemberList.Add(createdMember);

            SelectedMember = null;
        }
    }

    public async Task UpdateMember()
    {
        if (SelectedMember != null)
        {
            var updatedMember = await _memberService.Update(SelectedMember);
            if (updatedMember != null)
            {
                var memberToUpdate = MemberList.First(f => f.Id == updatedMember.Id);
                
                var index = MemberList.IndexOf(memberToUpdate);
                if (index != -1)
                {
                    MemberList[index] = updatedMember;
                    NotifyChanges();
                }
            }

            SelectedMember = null;
        }
    }

    public async Task DeleteMember(long memberId)
    {
        await _memberService.Delete(memberId);
        var memberToDelete = MemberList.First(f => f.Id == memberId);

        MemberList.Remove(memberToDelete);

        SelectedMember = null;
        
        NotifyChanges();
    }

    public MemberDto GetNewMember()
    {
        return new MemberDto
        {
            Code = string.Empty,
            Name = string.Empty,
            Alias = string.Empty,
            ClubId = 0,
            Club = new ClubDto(),
            Timestamp = Array.Empty<byte>()
        };
    }

    public void SetSelectedMember(long memberId)
    {
        SelectedMember = memberId != 0
            ? MemberList.First(f => f.Id == memberId)
            : GetNewMember();
    }

    public List<ClubDto> ClubAutocomplete(AutoCompleteParams parameters)
    {
        var filter = parameters.Filter == null
            ? string.Empty
            : parameters.Filter.ToLower();

        var count = parameters.Count is null or 0
            ? 5
            : (int) parameters.Count;

        return Clubs
            .Where(r => r.Code!.ToLower().Contains(filter) ||
                        r.Name!.ToLower().Contains(filter))
            .Take(count)
            .ToList();
    }

    public async Task GetClubs()
    {
        _clubs = await _clubService.Get();
    }
}