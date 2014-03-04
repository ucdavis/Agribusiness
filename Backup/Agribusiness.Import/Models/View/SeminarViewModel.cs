using System.Collections.Generic;

namespace Agribusiness.Import.Models.View
{
    public class SeminarViewModel
    {
        public IList<Seminar> Seminars { get; set; }
        public IList<KeyValuePair<string, string>> Errors { get; set; }
        public bool AlreadyImported { get; set; }

        public static SeminarViewModel Create(IList<Seminar> seminars, IList<KeyValuePair<string, string>> errors, bool alreadyImported)
        {
            var viewModel = new SeminarViewModel(){Seminars = seminars, Errors = errors, AlreadyImported = alreadyImported};

            return viewModel;
        }
    }
}