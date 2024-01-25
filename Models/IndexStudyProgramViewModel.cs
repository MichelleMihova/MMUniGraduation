using AutoMapper;
using SmartProfil.AutoMapper;
using System.Linq;

namespace MMUniGraduation.Models
{
    public class IndexStudyProgramViewModel : IMapFrom<StudyProgram>, IHaveCustomMappings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Model { get; set; }
        public string StudyProgramName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<StudyProgram, IndexStudyProgramViewModel>()
                .ForMember(x => x.Image, opt =>
                    opt.MapFrom(x =>
                        "/img/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault().Extension));
        }
    }
}
