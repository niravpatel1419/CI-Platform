using CI_Platform_Web.Entities.Data;
using CI_Platform_Web.Entities.Models;
using CI_Platform_Web.Entities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform_Web.Repositories.Interface
{
    public interface IAdmin
    {
        public AdminViewModel GetUserDetails();

        public User FetchUserDetails(long userId);

        public bool AddUpdateUserDetails(AdminViewModel vm);

        public List<CmsPage> FetchCMSPages();

        public CmsPage GetCmsPageDetails(long cmsPageId);

        public bool DeleteCMS(long cmsPageId);

        public bool AddUpdateCMSDetails(CMSViewModel vm);

        public List<Mission> FetchMissionDetails();

        public Mission MissionDetails(long missionId);

        public List<Country> GetCountries();

        public List<MissionTheme> GetMissionThemes();

        public List<Skill> GetSkills();

        public bool AddUpdateMissionDetails(AdminMissionViewModel vm);

        public bool DeleteMission(long missionId);

        public List<MissionTheme> FetchMissionThemes();

        public MissionTheme GetThemeDetail(long themeId);

        public bool AddEditMissionTheme(AdminMissionThemeViewModel themeViewModel);

        public bool DeleteTheme(long themeId);

        public List<Skill> FetchSkillDetails();

        public Skill GetSkillDetail(long skillId);

        public bool AddEditSkill(AdminMissionSkillsViewModel skillVm);

        public bool DeleteSkill(long skillId);

        public List<MissionApplication> FetchMissionApplication();

        public List<User> FetchUser();

        public bool ApproveMissionApplication(int status,long approveId);

        public bool RejectMissionApplication(long approveId);
    }
}