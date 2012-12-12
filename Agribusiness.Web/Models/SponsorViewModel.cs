using System.Collections.Generic;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Sponsor class
    /// </summary>
    public class SponsorViewModel
    {
        public Sponsor Sponsor { get; set; }
        public List<KeyValuePair<int, string>> SponsorLevels { get; set; }
 
        public static SponsorViewModel Create(Sponsor sponsor = null)
        {
            var viewModel = new SponsorViewModel {Sponsor = sponsor ?? new Sponsor()};
            
            viewModel.SponsorLevels = new List<KeyValuePair<int, string>>();
            viewModel.SponsorLevels.Add(new KeyValuePair<int, string>((int)SponsorLevel.Bronze, "$10,000"));
            viewModel.SponsorLevels.Add(new KeyValuePair<int, string>((int)SponsorLevel.Silver, "$15,000"));
            viewModel.SponsorLevels.Add(new KeyValuePair<int, string>((int)SponsorLevel.Gold, "$25,000"));
            viewModel.SponsorLevels.Add(new KeyValuePair<int, string>((int)SponsorLevel.BroughtToYouBy, "Brought To You By"));

            return viewModel;
        }
    }
}