using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Repositories.Interface
{
    public interface ICI_Platform
    {
        public User Login(User userLogin);
        public bool IsRegister(User userRegister);
        public bool IsEmailExist(string email);
        public void ForgetPassword(string email, string token);
        public MissionListViewModel DisplayMissions();
        public MissionListViewModel DisplayMissions(long? themeId, long? cityId, long? countryId);
        public User GetUserDetails(long userId);
        public int GetRating(int missionId);
        public bool IsFav(long userId, int missionId);
        public string IsApplied(long userId, int missionId);
        public int RatedValue(long userId, int missionId);
        public VolunteeringMissionPageViewModel VolunteeringPageLoad(int missionId);
        public bool AddRating(MissionRating r);
        public int AddRemoveFav(long userId, int missionId);
        public bool ApplyForMission(long userId, int missionId);
        public bool AddComment(Comment c);
        public List<User> GetRecentVol(int missionId);
        public bool SaveStrory(long userId, int missionId, string title, string stext, string date, string url, string status);
        public IEnumerable<StoryListViewModel> FetchStoryDetails();
        public IEnumerable<Story> GetStoryListData();
        public ShareStoryViewModel GetSavedStory(long UserId,int missionId);
        public void AddStoryMedia(string mediaType, string mediaPath, long mid, long uid);
        public StoryDetailsVM GetStoryDetails(long storyId);
        public bool UpdateUserDetails(User u, List<int> usersSkills);
        public bool changeUserPassword(UserDetailsViewModel u);
        public List<Country> GetCountryList();
        public List<Skill> GetAllSkills();
        public List<UserSkill> GetUsersSkills(long userId);
        public volunteeringTimeSheetViewModel GetVolunteerTimeDetails(long userId);
        public bool AddTimeSheetEntry(long userId, volunteeringTimeSheetViewModel vm);
        public bool DeleteTimesheetRecord(int timesheetId);
        public Timesheet EditTimesheetRecord(long timesheetId);
    }
}
