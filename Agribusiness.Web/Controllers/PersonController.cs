using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Agribusiness.WS;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Person class
    /// </summary>
    [Authorize]
    public class PersonController : ApplicationController
    {
	    private readonly IRepository<Person> _personRepository;
        private readonly IRepositoryWithTypedId<User, Guid> _userRepository;
        private readonly IRepositoryWithTypedId<SeminarRole, string> _seminarRoleRepository;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IPictureService _pictureService;
        private readonly IPersonService _personService;
        private readonly IFirmService _firmService;
        private readonly ISeminarService _seminarService;
        private readonly IRegistrationService _registrationService;
        private readonly IMembershipService _membershipService;

        public PersonController(IRepository<Person> personRepository, IRepositoryWithTypedId<User, Guid> userRepository, IRepositoryWithTypedId<SeminarRole, string> seminarRoleRepository
            , IRepository<SeminarPerson> seminarPersonRepository, IRepository<Seminar> seminarRepository
            , IPictureService pictureService, IPersonService personService, IFirmService firmService, ISeminarService seminarService, IRegistrationService registrationService)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _seminarRoleRepository = seminarRoleRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _seminarRepository = seminarRepository;
            _pictureService = pictureService;
            _personService = personService;
            _firmService = firmService;
            _seminarService = seminarService;
            _registrationService = registrationService;

            _membershipService = new AccountMembershipService();
        }

        //
        // GET: /Person/
        public ActionResult Index()
        {
            var viewModel = PersonListViewModel.Create(_personRepository, _personService);
            return View(viewModel);
        }

        public ActionResult Profile(int id)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = "Could not locate person.";
                return this.RedirectToAction(a => a.Index());
            }

            var displayPerson = _personService.GetDisplayPerson(person);
            return View(displayPerson);
        }

        #region Administration Functions
        /// <summary>
        /// Create a new attendee
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        [UserOnly]
        public ActionResult Create(int id)
        {
            var seminar = _seminarRepository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = PersonViewModel.Create(Repository, seminar);
            return View(viewModel);
        }

        /// <summary>
        /// Create a new attendee
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <param name="personEditModel"></param>
        /// <param name="profilepic"></param>
        /// <returns></returns>
        [UserOnly]
        [HttpPost]
        public ActionResult Create(int id, PersonEditModel personEditModel, HttpPostedFileBase profilepic)
        {
            var seminar = _seminarRepository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var person = personEditModel.Person;

            // create an account
            var createStatus =  _membershipService.CreateUser(personEditModel.Email
                                          , Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)
                                          , personEditModel.Email);

            // save only if user creation was successful
            if (createStatus == MembershipCreateStatus.Success)
            {
                var user = _userRepository.Queryable.Where(a => a.UserName == personEditModel.Email).FirstOrDefault();
                person.User = user;

                var seminarPerson = new SeminarPerson(seminar, person){Invite = true};
                
                person = SetPerson(personEditModel, seminarPerson, ModelState, person, profilepic);
                person.AddSeminarPerson(seminarPerson);

                if (ModelState.IsValid)
                {
                    _personRepository.EnsurePersistent(person);
                    Message = string.Format(Messages.Saved, "Person");
                    return this.RedirectToAction(a => a.UpdateProfilePicture(person.Id, null));
                }
            }
            else
            {
                ModelState.AddModelError("Create User", AccountValidation.ErrorCodeToString(createStatus));
            }
            
            var viewModel = PersonViewModel.Create(Repository, seminar, person, personEditModel.Email);
            viewModel.Addresses = personEditModel.Addresses;
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult AdminEdit(Guid id, int seminarId)
        {
            var user = _userRepository.GetNullableById(id);

            if (user == null)
            {
                Message = string.Format(Messages.NotFound, "user", id);
                return this.RedirectToAction<AttendeeController>(a => a.Index(seminarId));
            }

            var viewModel = AdminPersonViewModel.Create(Repository, _seminarService, seminarId, user.Person, user.LoweredUserName);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult AdminEdit(Guid id, int seminarId, PersonEditModel personEditModel, HttpPostedFileBase profilepic)
        {
            var user = _userRepository.GetNullableById(id);
            
            if (user == null)
            {
                Message = string.Format(Messages.NotFound, "user", id);
                return this.RedirectToAction<AttendeeController>(a => a.Index(seminarId));
            }

            var seminarPerson = _seminarPersonRepository.GetNullableById(personEditModel.SeminarPersonId);
            var person = SetPerson(personEditModel, seminarPerson, ModelState, user.Person, profilepic);

            if (ModelState.IsValid)
            {
                _personRepository.EnsurePersistent(person);
                _seminarPersonRepository.EnsurePersistent(seminarPerson);
                Message = string.Format(Messages.Saved, "Person");

                // send to crop photo if one was uploaded
                if (profilepic != null) return this.RedirectToAction(a => a.UpdateProfilePicture(person.Id, seminarId));

                return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId));
            }

            var viewModel = AdminPersonViewModel.Create(Repository, _seminarService, seminarId, user.Person, user.LoweredUserName);
            return View(viewModel);
        }

        /// <summary>
        /// Update the biography
        /// </summary>
        /// <param name="personId">Person Id</param>
        /// <param name="biography">Biography Text</param>
        /// <returns></returns>
        [UserOnly]
        [HttpPost]
        public ActionResult UpdateBiography(int personId, int seminarId, string biography)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index());
            }

            person.Biography = biography;

            _personRepository.EnsurePersistent(person);
            Message = string.Format(Messages.Saved, "Biography");

            var url = Url.Action("AdminEdit", new {id = person.User.Id, seminarId = seminarId});
            return Redirect(string.Format("{0}#biography", url));
        }

        /// <summary>
        /// Updating roles
        /// </summary>
        /// <param name="personId">Person Id</param>
        /// <returns></returns>
        [UserOnly]
        [HttpPost]
        public ActionResult UpdateRoles(int personId, int seminarId, List<string> roles)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index());
            }

            // merge the roles
            var reg = person.GetLatestRegistration();
            var seminar = _seminarService.GetCurrent();

            // check if user is registered for the current seminar
            if (reg.Seminar != seminar)
            {
                Message = "User is not a part of the current seminar.  Roles cannot be assigned.";
                return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId));
            }

            var existingRoles = reg.SeminarRoles.Select(a => a.Id).ToList();

            // add the roles
            foreach (var a in roles)
            {
                var role = _seminarRoleRepository.GetNullableById(a);
                if (role != null && !existingRoles.Contains(a))
                {
                    reg.SeminarRoles.Add(role);
                }
            }

            // remove the ones they shouldn't have
            var remove = reg.SeminarRoles.Where(a => !roles.ToList().Contains(a.Id)).ToList();
            foreach (var a in remove)
            {
                reg.SeminarRoles.Remove(a);
            }

            Repository.OfType<SeminarPerson>().EnsurePersistent(reg);
            Message = string.Format(Messages.Saved, "Seminar Roles");

            var url = Url.Action("AdminEdit", new { id = person.User.Id, seminarId = seminarId });
            return Redirect(string.Format("{0}#roles", url));
        }

        [UserOnly]
        [HttpPost]
        public ActionResult UpdateCoupon(int personId, int seminarId, decimal couponAmount)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index());
            }

            var reg = person.GetLatestRegistration();
            var seminar = _seminarService.GetCurrent();

            // check if user is registered for the current seminar
            if (reg.Seminar != seminar)
            {
                Message = "User is not a part of the current seminar.  Coupon cannot be created.";
                return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId));
            }

            // check for a current coupon
            if (!string.IsNullOrWhiteSpace(reg.CouponCode))
            {
                _registrationService.CancelCoupon(seminar.RegistrationId.Value, reg.CouponCode);
                reg.CouponCode = null;
                reg.CouponAmount = null;
            }

            // create new coupon
            var coupon = _registrationService.GenerateCoupon(seminar.RegistrationId.Value, person.User.LoweredUserName, couponAmount);

            if (!string.IsNullOrWhiteSpace(coupon))
            {
                reg.CouponCode = coupon;
                reg.CouponAmount = couponAmount;
            }

            _seminarPersonRepository.EnsurePersistent(reg);
            Message = string.Format(Messages.Saved, "coupon");

            var url = Url.Action("AdminEdit", new { id = person.User.Id, seminarId = seminarId });
            return Redirect(string.Format("{0}#registration", url));
        }

        [UserOnly]
        [HttpPost]
        public ActionResult CancelCoupon(int personId, int seminarId)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index());
            }

            var reg = person.GetLatestRegistration();
            var seminar = _seminarService.GetCurrent();

            // check if user is registered for the current seminar
            if (reg.Seminar != seminar)
            {
                Message = "User is not a part of the current seminar.  Coupon cannot be created.";
                return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId));
            }

            var result = _registrationService.CancelCoupon(seminar.RegistrationId.Value, reg.CouponCode);

            if (result)
            {
                reg.CouponCode = null;
                reg.CouponAmount = null;

                _seminarPersonRepository.EnsurePersistent(reg);
                Message = string.Format(Messages.Saved, "coupon");
            }

            var url = Url.Action("AdminEdit", new { id = person.User.Id, seminarId = seminarId });
            return Redirect(string.Format("{0}#registration", url));
        }

        [UserOnly]
        [HttpPost]
        public ActionResult UpdateRegistrationStatus(int personId, int seminarId)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index());
            }

            var reg = person.GetLatestRegistration();
            var seminar = _seminarService.GetCurrent();

            // check if user is registered for the current seminar
            if (reg.Seminar != seminar)
            {
                Message = "User is not a part of the current seminar.  Coupon cannot be created.";
                return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId));
            }

            // make the web service call
            string transactionId;
            bool paid;
            _registrationService.RefreshRegistration(seminar.RegistrationId.Value, reg.ReferenceId, out transactionId, out paid);

            reg.TransactionId = transactionId;
            reg.Paid = paid;

            _seminarPersonRepository.EnsurePersistent(reg);

            Message = "Registration information has been updated.";
            var url = Url.Action("AdminEdit", new { id = person.User.Id, seminarId = seminarId });
            return Redirect(string.Format("{0}#registration", url));
        }

        [UserOnly]
        [HttpPost]
        public ActionResult UpdateComments(int personId, int seminarId, string comments)
        {
            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index());
            }

            var reg = person.GetLatestRegistration();
            var seminar = _seminarService.GetCurrent();

            // check if user is registered for the current seminar
            if (reg.Seminar != seminar)
            {
                Message = "User is not a part of the current seminar.  Coupon cannot be created.";
                return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId));
            }

            reg.Comments = comments;
            _seminarPersonRepository.EnsurePersistent(reg);

            Message = "Comments have been updated.";
            var url = Url.Action("AdminEdit", new { id = person.User.Id, seminarId = seminarId });
            return Redirect(string.Format("{0}#comments", url));
        }
        #endregion

        #region Profile Editing Functions
        /// <summary>
        /// Attendee's page to update their own profile, Limited editing page
        /// </summary>
        /// <param name="id">Person Id for admin editing</param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            User user;

            // admin is trying to edit, authorize them
            if (id.HasValue)
            {
                user = _userRepository.GetNullableById(id.Value);

                // current user must be in User role)
                if (!Roles.IsUserInRole(RoleNames.User))
                {
                    return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
                }
            }
            else
            {
                user = Repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).FirstOrDefault();    
            }

            if (user == null)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            var person = user.Person;

            var viewModel = PersonViewModel.Create(Repository, null, person, user.LoweredUserName);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(Guid? id, PersonEditModel personEditModel, HttpPostedFileBase profilepic)
        {
            User user = null;

            // admin is trying to edit, authorize them
            if (id.HasValue)
            {
                // current user must be in User role
                if (Roles.IsUserInRole(RoleNames.User))
                {
                    user = _userRepository.GetNullableById(id.Value);
                }
            }
            else
            {
                user = Repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).FirstOrDefault();
            }

            if (user == null)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            var seminarPerson = _seminarPersonRepository.GetNullableById(personEditModel.SeminarPersonId);
            var person = SetPerson(personEditModel, seminarPerson, ModelState, user.Person, profilepic);

            if (ModelState.IsValid)
            {
                _personRepository.EnsurePersistent(person);
                Message = string.Format(Messages.Saved, "Person");

                // send to crop photo if one was uploaded
                if (profilepic != null) return this.RedirectToAction(a => a.UpdateProfilePicture(person.Id, null));
            }

            var viewModel = PersonViewModel.Create(Repository, null, person, user.LoweredUserName);
            return View(viewModel);
        }
        #endregion

        #region Profile Picture Actions
        public ActionResult UpdateProfilePicture(int id, int? seminarId)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", id);
                return this.RedirectToAction(a => a.Index());
            }

            // validate this is the person or is a person in user role
            if (person.User.LoweredUserName != CurrentUser.Identity.Name.ToLower() && !Roles.IsUserInRole(RoleNames.User))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            // set this to check for admin routing back to attendee edit page
            ViewBag.SeminarId = seminarId;

            return View(person);
        }

        [HttpPost]
        public ActionResult UpdateProfilePicture(int id, int? seminarId, int x, int y, int height, int width)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", id);
                return this.RedirectToAction(a => a.Index());
            }

            // validate this is the person or is a person in user role
            if (person.User.LoweredUserName != CurrentUser.Identity.Name.ToLower() && !Roles.IsUserInRole(RoleNames.User))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }
            
            // crop the image
            var cropped = _pictureService.Crop(person.OriginalPicture, x, y, width, height);

            // get the main profile picture
            person.MainProfilePicture = _pictureService.MakeMainProfile(cropped);

            // get the thumbnail
            person.ThumbnailPicture = _pictureService.MakeThumbnail(cropped);

            person.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Message = string.Format(Messages.Saved, "Person");
                _personRepository.EnsurePersistent(person);

                if (seminarId.HasValue)
                {
                    return this.RedirectToAction(a => a.AdminEdit(person.User.Id, seminarId.Value));
                }

                return this.RedirectToAction(a => a.Index());
            }

            return View(person);
        }

        public ActionResult GetOriginalPicture(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.OriginalPicture != null) return File(person.OriginalPicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }

        public ActionResult GetProfilePicture(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.MainProfilePicture != null) return File(person.MainProfilePicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }

        public ActionResult GetThumbnail(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.MainProfilePicture != null) return File(person.ThumbnailPicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder_thumb.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }
        #endregion

        #region Private Helpers
        private Person SetPerson(PersonEditModel personEditModel, SeminarPerson seminarPerson, ModelStateDictionary modelState, Person person = null, HttpPostedFileBase profilePic = null)
        {
            person = person ?? personEditModel.Person;

            // copy all the fields
            Mapper.Map(personEditModel, person);

            SetAddresses(person, personEditModel.Addresses);
            SetContacts(person, personEditModel.Contacts);
            SetCommodities(seminarPerson, personEditModel.Commodities);

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
            modelState.Clear();
            person.TransferValidationMessagesTo(modelState);

            return person;
        }

        private void SetAddresses(Person person, IList<Address> addresses)
        {
            // remove the blank address
            var remove = addresses.Where(a => !a.HasAddress()).ToList();
            foreach (var a in remove) addresses.Remove(a);

            // update/add updated addresses
            foreach (var addr in addresses)
            {
                var type = addr.AddressType;
                var origAddress = person.Addresses.Where(a => a.AddressType == type).FirstOrDefault();

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
        private void SetContacts(Person person, IList<Contact> contacts)
        {
            // remove the blanks
            var remove = contacts.Where(a => !a.HasContact).ToList();
            foreach (var a in remove) contacts.Remove(a);

            // update/add contacts
            foreach (var ct in contacts)
            {
                var type = ct.ContactType;
                var origCt = person.Contacts.Where(a => a.ContactType == type).FirstOrDefault();

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
        private void SetCommodities(SeminarPerson seminarPerson, IList<Commodity> commodities)
        {
            seminarPerson.Commodities.Clear();

            seminarPerson.Commodities = new List<Commodity>(commodities);
        }
        #endregion
    }
}
