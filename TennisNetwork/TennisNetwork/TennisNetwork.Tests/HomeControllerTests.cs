using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TennisNetwork.Data;
using TennisNetwork.Models;
using System.Linq;
using TennisNetwork.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TennisNetwork.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexMethodShouldReturnProperNumberOfLevels()
        {
            var userLevels = new List<UserLevel>
            {
                new UserLevel{ Id = 1, Level = "2"},
                new UserLevel{ Id = 2, Level = "3"},
            };

            var userLevelRepoMock = new Mock<IRepository<UserLevel>>();
            userLevelRepoMock.Setup(x => x.All()).Returns(userLevels.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.UserLevels).Returns(userLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Index() as ViewResult;

            Assert.IsNotNull(viewResult, "Index action returns null.");
            Assert.IsNotNull(viewResult.ViewBag.UserLevelId, "The ViewBag.UserLevelId is null.");
            Assert.AreEqual(2, viewResult.ViewBag.UserLevelId.Items.Count, "The ViewBag.UserLevelId count is not equal to 2.");
            Assert.AreNotEqual(3, viewResult.ViewBag.UserLevelId.Items.Count, "The ViewBag.UserLevelId count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsers()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = "2", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = null, Addresses = new List<Address>{new Address()}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var userLevelRepoMock = new Mock<IRepository<UserLevel>> { CallBase = true };

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(userLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel(), null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }


        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersByLevel()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { UserLevelId = 1, Addresses = new List<Address>{new Address()}},
                new ApplicationUser { UserLevelId = 1, Addresses = new List<Address>{new Address()}},
                new ApplicationUser { UserLevelId = 2, Addresses = new List<Address>{new Address()}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel { UserLevelId = 1 }, null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersByGender()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Gender = Gender.F, Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Gender = Gender.F, Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Gender = Gender.M, Addresses = new List<Address>{new Address()}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel { Gender = Gender.F }, null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersByCountry()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Addresses = new List<Address>{new Address{ Country = "BG"}}},
                new ApplicationUser { Addresses = new List<Address>{new Address{ Country = "BG"}}},
                new ApplicationUser { Addresses = new List<Address>{new Address{ Country = "CA"}}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel {Country = "BG" }, null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersByTown()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Addresses = new List<Address>{new Address{ Town = "Blagoevgrad"}}},
                new ApplicationUser { Addresses = new List<Address>{new Address{ Town = "Blagoevgrad"}}},
                new ApplicationUser { Addresses = new List<Address>{new Address{ }}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel { Town = "Blagoevgrad" }, null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersByState()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Addresses = new List<Address>{new Address{ State = "Ontario"}}},
                new ApplicationUser { Addresses = new List<Address>{new Address{ State = "Ontario"}}},
                new ApplicationUser { Addresses = new List<Address>{new Address{ }}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel { State = "Ontario" }, null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersByAllFilters()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {UserLevelId = 2, Gender = Gender.M, Addresses = new List<Address>{new Address
                {
                    Country = "BG",
                    Town = "Blagoevgrad",
                    State = "Blagoevgrad"
                }}},
                new ApplicationUser {UserLevelId = 2, Gender = Gender.M, Addresses = new List<Address>{new Address
                {
                    Country = "BG",
                    Town = "Blagoevgrad",
                    State = "Blagoevgrad"
                }}},
                new ApplicationUser {UserLevelId = 2, Gender = Gender.M, Addresses = new List<Address>{new Address
                {
                    Country = "BG",
                    Town = "Blagoevgrad",
                }}},
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);

            var searchUserViewModel = new SearchUserViewModel
            {
                UserLevelId = 2,
                Gender = Gender.M,
                Country = "BG",
                Town = "Blagoevgrad",
                State = "Blagoevgrad"
            };
            var viewResult = controller.Search(searchUserViewModel, null) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfUsersPerPage()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel(), 2) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(2, model.UserViewModel.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnMaxNumberOfUsersPerPage()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}}
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel(), 1) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.IsNotNull(model.UserViewModel, "The model.UserViewModel is null.");
            Assert.AreEqual(5, model.UserViewModel.Count(), "The model count is not equal to 5.");
            Assert.AreNotEqual(3, model.UserViewModel.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfPagesWhenUserNotLogged()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel(), 1) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(2, viewResult.ViewBag.UserPages, "The UserPages count is not equal to 2.");
            Assert.AreNotEqual(3, viewResult.ViewBag.UserPages, "The UserPages count is equal to 3.");
        }

        [TestMethod]
        public void SearchMethodShoulReturnProperNumberOfPagesWhenUserLogged()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                new ApplicationUser {Addresses = new List<Address>{new Address()}},
                
                // Because this.UserId = null, this is like currently logged user.
                new ApplicationUser {Id = null, Addresses = new List<Address>{new Address()}},
            };


            var usersRepoMock = new Mock<IRepository<ApplicationUser>>();
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());
            var usersLevelRepoMock = new Mock<IRepository<UserLevel>>();
            usersLevelRepoMock.Setup(x => x.All()).Returns(new List<UserLevel> { new UserLevel { Id = 1, Level = "2.0" } }.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            uowDataMock.Setup(x => x.UserLevels).Returns(usersLevelRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);
            var viewResult = controller.Search(new SearchUserViewModel(), 1) as ViewResult;
            var model = viewResult.Model as SearchResultViewModel;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(1, viewResult.ViewBag.UserPages, "The UserPages count is not equal to 2.");
            Assert.AreNotEqual(3, viewResult.ViewBag.UserPages, "The UserPages count is equal to 3.");
        }

        [TestMethod]
        public void UserCalendarMethodShoulReturnProperNumberOfEvents()
        {
            var eventList = new List<Event>
            {
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };


            var eventsRepoMock = new Mock<IRepository<Event>>();
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);

            var viewResult = controller.UserCalendar(1, "1") as PartialViewResult;
            var model = viewResult.Model as IEnumerable<CreateEventViewModel>;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(2, model.Count(), "The model count is not equal to 2.");
            Assert.AreNotEqual(3, model.Count(), "The model count is equal to 3.");
        }

        [TestMethod]
        public void UserCalendarMethodShoulReturnProperNumberOfEventsPerPage()
        {
            var eventList = new List<Event>
            {
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };


            var eventsRepoMock = new Mock<IRepository<Event>>();
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);

            var viewResult = controller.UserCalendar(2, "1") as PartialViewResult;
            var model = viewResult.Model as IEnumerable<CreateEventViewModel>;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(3, model.Count(), "The model count is not equal to 3.");
            Assert.AreNotEqual(2, model.Count(), "The model count is equal to 2.");
        }

        [TestMethod]
        public void UserCalendarMethodShoulReturnMaxNumberOfEventsPerPage()
        {
            var eventList = new List<Event>
            {
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };


            var eventsRepoMock = new Mock<IRepository<Event>>();
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);

            var viewResult = controller.UserCalendar(1, "1") as PartialViewResult;
            var model = viewResult.Model as IEnumerable<CreateEventViewModel>;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(4, model.Count(), "The model count is not equal to 4.");
            Assert.AreNotEqual(2, model.Count(), "The model count is equal to 2.");
        }

        [TestMethod]
        public void UserCalendarMethodShoulReturnProperNumberOfPages()
        {
            var eventList = new List<Event>
            {
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };


            var eventsRepoMock = new Mock<IRepository<Event>>();
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>();
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new HomeController(uowDataMock.Object);

            var viewResult = controller.UserCalendar(1, "1") as PartialViewResult;
            var model = viewResult.Model as IEnumerable<CreateEventViewModel>;

            Assert.IsNotNull(viewResult, "Search action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(3, viewResult.ViewBag.Pages, "The model count is not equal to 3.");
            Assert.AreNotEqual(2, viewResult.ViewBag.Pages, "The model count is equal to 2.");
        }
    }
}
