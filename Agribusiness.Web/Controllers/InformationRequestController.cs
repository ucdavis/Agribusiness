﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the InformationRequest class
    /// </summary>
    [UserOnly]
    public class InformationRequestController : ApplicationController
    {
	    private readonly IRepository<InformationRequest> _informationrequestRepository;
        private readonly IRepository<InformationRequestNote> _informationRequestNoteRepository;
        private readonly IRepositoryWithTypedId<AddressType, char> _addressTypeRepository;
        private readonly IFirmService _firmService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Person> _personRepository;

        private readonly IMembershipService _membershipService;

        public InformationRequestController(IRepository<InformationRequest> informationrequestRepository, IRepository<InformationRequestNote> informationRequestNoteRepository, IRepositoryWithTypedId<AddressType, char> addressTypeRepository, IFirmService firmService, IRepository<User> userRepository, IRepository<Person> personRepository )
        {
            _informationrequestRepository = informationrequestRepository;
            _informationRequestNoteRepository = informationRequestNoteRepository;
            _addressTypeRepository = addressTypeRepository;
            _firmService = firmService;
            _userRepository = userRepository;
            _personRepository = personRepository;

            _membershipService = new AccountMembershipService();
        }

        //
        // GET: /InformationRequest/
        public ActionResult Index()
        {
            var seminar = SiteService.GetLatestSeminar(Site);
            var informationrequestList = _informationrequestRepository.Queryable.Where(a => a.Seminar == seminar).OrderBy(a=>a.Responded);

            return View(informationrequestList);
        }

        public ActionResult Edit(int id)
        {
            var informationRequest = _informationrequestRepository.GetNullableById(id);

            if (informationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            return View(InformationRequestViewModel.Create(RepositoryFactory, informationRequest));
        }

        [HttpPost]
        public ActionResult Edit(int? id, InformationRequest informationRequest)
        {
            var editInformationRequest = _informationrequestRepository.GetNullableById(id.Value);

            if (editInformationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            Mapper.Map(informationRequest, editInformationRequest);

            ModelState.Clear();
            editInformationRequest.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _informationrequestRepository.EnsurePersistent(editInformationRequest);
                Message = string.Format(Messages.Saved, "Information request");
            }

            return View(InformationRequestViewModel.Create(RepositoryFactory,editInformationRequest));
        }

        public ActionResult AddNote(int id)
        {
            var informationRequest = _informationrequestRepository.GetNullableById(id);

            if (informationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            return View(new InformationRequestNote(informationRequest, string.Empty, CurrentUser.Identity.Name));
        }

        [HttpPost]
        public ActionResult AddNote(int id, InformationRequestNote informationRequestNote)
        {
            var informationRequest = _informationrequestRepository.GetNullableById(id);

            if (informationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            ModelState.Clear();

            var irn = new InformationRequestNote(informationRequest, informationRequestNote.Notes, CurrentUser.Identity.Name);
            irn.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _informationRequestNoteRepository.EnsurePersistent(irn);

                Message = string.Format(Messages.Saved, "Note");
                return this.RedirectToAction(a => a.Edit(id));
            }
            

            return View(irn);
        }

        /// <summary>
        /// Create an account from Information Request
        /// </summary>
        /// <param name="id">Information Request Id</param>
        /// <returns></returns>
        public ActionResult CreatePerson(int id)
        {
            var ir = _informationrequestRepository.GetNullableById(id);

            if (ir == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = PersonViewModel.Create(Repository, _firmService, Site);
            viewModel.Person.FirstName = ir.FirstName.Trim();
            viewModel.Person.LastName = ir.LastName.Trim();
            viewModel.Email = ir.Email;
            viewModel.UserName = string.Format("{0}.{1}", ir.FirstName.Trim(), ir.LastName.Trim());

            // fake phone number
            viewModel.Person.Phone = ir.Phone;    

            var atype = _addressTypeRepository.GetNullableById((char)StaticIndexes.Address_Business[0]);

            var address = viewModel.Addresses.FirstOrDefault(a => a.AddressType == atype);
            if (address != null)
            {
                address.Line1 = ir.AddressLine1;
                address.Line2 = ir.AddressLine2;
                address.City = ir.City;
                address.State = ir.State;
                address.Zip = ir.Zip;
                address.Country = ir.Country;    
            }

            if (ir.Site.CollectExtended)
            {
                // get the assistant contact
                var ctype = RepositoryFactory.ContactTypeRepsitory.GetNullableById((char)StaticIndexes.Contact_Assistant[0]);
                var assistant = viewModel.Contacts.FirstOrDefault(a => a.ContactType == ctype);

                if (assistant != null)
                {
                    assistant.FirstName = ir.AssistantFirstName;
                    assistant.LastName = ir.AssistantLastName;
                    assistant.Email = ir.AssistantEmail;
                    assistant.Phone = ir.AssistantPhone;

                    var comoption = RepositoryFactory.CommunicationOptionRepository.GetNullableById(StaticIndexes.Communication_Assistant);
                    viewModel.Person.CommunicationOption = comoption;
                }
            }
            else
            {
                var comoption = RepositoryFactory.CommunicationOptionRepository.GetNullableById(StaticIndexes.Communication_Directly);
                viewModel.Person.CommunicationOption = comoption;
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreatePerson(int id, PersonEditModel personEditModel, HttpPostedFileBase profilepic)
        {
            ModelState.Clear();

            var person = personEditModel.Person;

            var user = _userRepository.Queryable.FirstOrDefault(a => a.LoweredUserName == personEditModel.UserName.ToLower());
            person.User = user;

            SeminarPerson seminarPerson = null;
            
            person = SetPerson(personEditModel, seminarPerson, ModelState, person, profilepic);

            ModelState.Remove("Person.User");

            if (ModelState.IsValid)
            {

                // try to create the user
                var createStatus = _membershipService.CreateUser(personEditModel.UserName
                                              , Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)
                                              , personEditModel.Email);

                // retrieve the user to assign
                var createdUser = _userRepository.Queryable.FirstOrDefault(a => a.LoweredUserName == personEditModel.UserName.ToLower());
                person.User = createdUser;

                // save only if user creation was successful
                if (createStatus == MembershipCreateStatus.Success)
                {
                    person.AddSite(SiteService.LoadSite(Site));

                    // we're good save the person object
                    _personRepository.EnsurePersistent(person);
                    Message = string.Format(Messages.Saved, "Person");

                    if (person.OriginalPicture != null) return this.RedirectToAction<PersonController>(a => a.UpdateProfilePicture(person.Id, null, false));

                    return this.RedirectToAction<PersonController>(a => a.AdminEdit(person.User.Id, null, true));
                }

                ModelState.AddModelError("Create User", AccountValidation.ErrorCodeToString(createStatus));
            }

            var viewModel = PersonViewModel.Create(Repository, _firmService, Site, null, person, personEditModel.Email);
            viewModel.Addresses = personEditModel.Addresses;
            viewModel.UserName = personEditModel.UserName;
            return View(viewModel);
        }

        private Person SetPerson(PersonEditModel personEditModel, SeminarPerson seminarPerson, ModelStateDictionary modelState, Person person = null, HttpPostedFileBase profilePic = null)
        {
            modelState.Clear();

            person = person ?? personEditModel.Person;

            // copy all the fields
            Mapper.Map(personEditModel, person);

            SetAddresses(person, personEditModel.Addresses, ModelState);
            SetContacts(person, personEditModel.Contacts, ModelState);

            if (seminarPerson != null)
            {
                SetCommodities(seminarPerson, personEditModel.Commodities);

                seminarPerson.Firm = personEditModel.Firm ?? new Firm(personEditModel.FirmName, personEditModel.FirmDescription) { WebAddress = personEditModel.FirmWebAddress };
                seminarPerson.Title = personEditModel.Title;
            }

            // deal with the image))
            if (profilePic != null)
            {
                // blank out existing image files
                person.OriginalPicture = null;
                person.MainProfilePicture = null;
                person.ThumbnailPicture = null;

                // read the file and set the original picture
                var reader = new BinaryReader(profilePic.InputStream);
                person.OriginalPicture = reader.ReadBytes(profilePic.ContentLength);
                person.ContentType = profilePic.ContentType;
            }

            // run the validation
            person.TransferValidationMessagesTo(modelState);

            return person;
        }

        private static void SetAddresses(Person person, IList<Address> addresses, ModelStateDictionary modelState)
        {
            // remove the blank address
            var remove = addresses.Where(a => !a.HasAddress()).ToList();
            foreach (var a in remove) addresses.Remove(a);

            // update/add updated addresses
            foreach (var addr in addresses)
            {
                var type = addr.AddressType;
                var origAddress = person.Addresses.Where(a => a.AddressType == type).FirstOrDefault();

                // run validation if required
                if (type.Required)
                {
                    addr.Person = person;
                    addr.TransferValidationMessagesTo(modelState);
                }

                // address was entered
                if (addr.HasAddress())
                {
                    // person did not have this address in the first place, add it in
                    if (origAddress == null)
                    {
                        person.AddAddress(addr);
                    }
                    // update existing address
                    else
                    {
                        Mapper.Map(addr, origAddress);
                    }
                }
                // address was blanked out/removed
                else
                {
                    if (origAddress != null) person.Addresses.Remove(origAddress);
                }
            }

        }
        private static void SetContacts(Person person, IList<Contact> contacts, ModelStateDictionary modelState)
        {
            // remove the blanks
            var remove = contacts.Where(a => !a.HasContact).ToList();
            foreach (var a in remove) contacts.Remove(a);

            // update/add contacts
            foreach (var ct in contacts)
            {
                var type = ct.ContactType;
                var origCt = person.Contacts.Where(a => a.ContactType == type).FirstOrDefault();

                if (type.Required)
                {
                    ct.Person = person;
                    ct.TransferValidationMessagesTo(modelState);
                }

                if (ct.HasContact)
                {
                    if (origCt == null)
                    {
                        person.AddContact(ct);
                    }
                    else
                    {
                        Mapper.Map(ct, origCt);
                    }
                }
                else
                {
                    if (origCt != null) person.Contacts.Remove(ct);
                }
            }
        }
        private static void SetCommodities(SeminarPerson seminarPerson, IList<Commodity> commodities)
        {

            if (seminarPerson.Commodities != null) seminarPerson.Commodities.Clear();

            seminarPerson.Commodities = new List<Commodity>(commodities);
        }
    }

	
	/// <summary>
    /// ViewModel for the InformationRequest class
    /// </summary>
    public class InformationRequestViewModel
	{
		public InformationRequest InformationRequest { get; set; }
        public IEnumerable<Country> Countries { get; set; }
 
		public static InformationRequestViewModel Create(IRepositoryFactory repositoryFactory, InformationRequest informationRequest = null)
		{
			var viewModel = new InformationRequestViewModel
			                    {
			                        InformationRequest = informationRequest ?? new InformationRequest(),
                                    Countries = repositoryFactory.CountryRepository.Queryable.OrderBy(a => a.Name).ToList()
			                    };
 
			return viewModel;
		}
	}
}
