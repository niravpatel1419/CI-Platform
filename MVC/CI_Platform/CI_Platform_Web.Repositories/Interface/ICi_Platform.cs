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
        public bool SaveStrory(long userId, int missionId, string title, string stext, string date);
        public StoryListViewModel FetchStoryDetails();

    }
}
