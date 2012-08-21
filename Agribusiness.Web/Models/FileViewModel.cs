using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;

namespace Agribusiness.Web.Models
{
    public class FileViewModel
    {
        public int SeminarId { get; set; }
        public File File { get; set; }

        public static FileViewModel Create(IRepositoryFactory repositoryFactory, int seminarId, File file = null)
        {
            var viewModel = new FileViewModel()
                                {
                                    File = file ?? new File() { Seminar = repositoryFactory.SeminarRepository.GetNullableById(seminarId)},
                                    SeminarId = seminarId
                                };

            return viewModel;
        }

    }
}