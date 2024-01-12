using MMUniGraduation.ViewModels;
using System.Threading.Tasks;

namespace MMUniGraduation.Services.Interfaces
{
    public interface ILectorService
    {
        public Task EditInfo(EditLectorViewModel input);
    }
}
