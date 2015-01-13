using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using Thought.vCards;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Services
{
    public interface IvCardService
    {
        byte[] Create(Person person, string site);
    }

    public class vCardService : IvCardService
    {
        public byte[] Create(Person person, string site)
        {
            Check.Require(person != null, "person is required.");

            return GeneratevCard(person, site);
        }

        private byte[] GeneratevCard(Person person, string site)
        {
            // create the card
            var vCard = new vCard();

            // name information
            vCard.FamilyName = person.LastName;
            vCard.GivenName = person.FirstName;
            vCard.NamePrefix = person.Salutation;

            // job information
            vCard.Organization = person.GetLatestRegistration(site).Firm.Name;
            vCard.Title = person.GetLatestRegistration(site).Title;

            // picture
            if (person.MainProfilePicture != null)
            {
                vCard.Photos.Add(new vCardPhoto(person.MainProfilePicture));    
            }

            // add contact information based on authorization
            if (person.ContactInformationRelease)
            {
                // contact information
                vCard.EmailAddresses.Add(new vCardEmailAddress(person.User.UserName));

                // business address only
                var busAddr = person.Addresses.Where(a => a.AddressType.Id.ToString() == StaticIndexes.Address_Business).FirstOrDefault();
                if (busAddr != null)
                {
                    var addr = new vCardDeliveryAddress()
                                   {
                                       AddressType = vCardDeliveryAddressTypes.Work,
                                       City = busAddr.City,
                                       Country = busAddr.Country.Name,
                                       IsWork = true,
                                       PostalCode = busAddr.Zip,
                                       Street = string.Format("{0} {1}",busAddr.Line1, busAddr.Line2 ?? string.Empty),
                                       Region = busAddr.State
                                   };

                    vCard.DeliveryAddresses.Add(addr);    
                }
            }

            // prepare the writer to write the vcard output
            var writer = new vCardStandardWriter();
            writer.EmbedInternetImages = false;
            writer.EmbedLocalImages = true;
            writer.Options = vCardStandardWriterOptions.IgnoreCommas;

            // put the data into a memory stream
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);

            // write the data to the stream
            writer.Write(vCard, streamWriter);

            // write to the memory stream
            streamWriter.Flush();
            streamWriter.Close();

            var bytes = stream.ToArray();
            return bytes;
        }

        
    }

}
