using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TennisNetwork.Controllers;
using TennisNetwork.Data;
using TennisNetwork.Models;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Net;

namespace TennisNetwork.Tests
{
    [TestClass]
    public class EventsControllerTests
    {
        [TestMethod]
        public void IndexMethodShouldReturnNotNullResult()
        {
            var controller = new EventsController();
            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result, "Index action returns null.");
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
            
            var controller = new EventsController(uowDataMock.Object);

            var viewResult = controller.UserCalendar(1, "1") as PartialViewResult;
            var model = viewResult.Model as IEnumerable<CreateEventViewModel>;

            Assert.IsNotNull(viewResult, "UserCalendar action returns null.");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(3, model.Count(), "The model count is not equal to 3.");
            Assert.AreNotEqual(2, model.Count(), "The model count is equal to 2 but expects 3.");
        }

        [TestMethod]
        public void CreateMethodShouldReturnNotNullResult()
        {
            var controller = new EventsController();
            var result = controller.Create(2) as PartialViewResult;

            Assert.IsNotNull(result, "Index action returns null.");
            Assert.AreEqual(2, result.ViewBag.PageNumber, "Incorrect ViewBag.PageNumber.");
        }

        [TestMethod]
        public void CreateMethodShouldCreateEvent()
        {
            var eventList = new List<Event>
            {
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };


            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = "2", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = null, Addresses = new List<Address>{new Address()}}
            };
            
            bool isItemAdded = false;
            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.Add(It.IsAny<Event>())).Callback(() => isItemAdded = true);
            var usersRepoMock = new Mock<IRepository<ApplicationUser>>(MockBehavior.Strict);
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);
            bool isSaveChangesExecuted = false;
            uowDataMock.Setup(x => x.SaveChangesAsync()).Returns(It.IsAny<Task<int>>).Callback(() => isSaveChangesExecuted = true);
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);

            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/Events", UriKind.Absolute));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(s => s.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);
            context.Setup(x => x.GetService(It.IsAny<Type>())).Returns(It.IsAny<Type>());

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            context.SetupGet(x => x.User).Returns(principal);

            var controller = new EventsController(uowDataMock.Object);
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);

            var model = new CreateEventViewModel
            {
                EndDate = new DateTime(2014, 12, 13),
                EndTime = new DateTime(2014, 12, 13, 12, 0, 0),
                EventType = EventType.Single,
                StartDate = new DateTime(2014, 12, 13),
                StartTime = new DateTime(2014, 12, 13, 10, 30, 0),
            };
            var jsonResult = controller.Create(model) as JsonResult;

            Assert.IsNotNull(jsonResult, "UserCalendar action returns null.");
            Assert.AreEqual(true, isItemAdded, "Add method of Events is not invoked.");
            Assert.AreEqual(true, isSaveChangesExecuted, "SaveChanges method is not invoked.");
        }

        [TestMethod]
        public void JoinMethodShouldReturnJsonWhenEventIsTaken()
        {
            var ev = new Event { Taken = true };
                       
            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ev);
            
            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new EventsController(uowDataMock.Object);
            
            var jsonResult = controller.Join(100) as JsonResult;

            dynamic jsonResultData = jsonResult.Data;
            Assert.IsNotNull(jsonResult, "JsonResult is null.");
            Assert.AreEqual(true, jsonResultData.taken);
        }

        [TestMethod]
        public void JoinMethodShouldReturnJsonWhenEventIsNotTaken()
        {
            var ev = new Event { Taken = false, Users = new List<ApplicationUser> { new ApplicationUser { Id = "null" } } };
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = "2", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = null, Addresses = new List<Address>{new Address()}}
            };

            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ev);
            var usersRepoMock = new Mock<IRepository<ApplicationUser>>(MockBehavior.Strict);
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            bool isSaveChangesAsyncExecuted = false;
            uowDataMock.Setup(x => x.SaveChangesAsync()).Returns(It.IsAny<Task<int>>).Callback(() => isSaveChangesAsyncExecuted = true);
            
            var controller = new EventsController(uowDataMock.Object);

            #region To test Url.Action() method.
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("/Events", UriKind.Relative));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(s => s.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);
            context.Setup(x => x.GetService(It.IsAny<Type>())).Returns(It.IsAny<Type>());

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            context.SetupGet(x => x.User).Returns(principal);

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes); 
            #endregion

            var jsonResult = controller.Join(100) as JsonResult;

            dynamic jsonResultData = jsonResult.Data;
            Assert.IsNotNull(jsonResult, "JsonResult is null.");
            Assert.AreEqual(false, jsonResultData.taken);
            Assert.AreEqual("/Events", jsonResultData.url);
            Assert.AreEqual(true, isSaveChangesAsyncExecuted, "SaveChanges method is not invoked.");
        }

        [TestMethod]
        public void DeleteMethodShouldReturnBadRequest()
        {
            var controller = new EventsController();

            var result = controller.Delete(null, null) as HttpStatusCodeResult;

            Assert.AreEqual(HttpStatusCode.BadRequest, (HttpStatusCode)result.StatusCode);
        }

        [TestMethod]
        public void DeleteMethodShouldReturnHttpNotFound()
        {
            var eventList = new List<Event>
            {
                new Event{ Id = 1, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 2, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 3, Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 4, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };

            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new EventsController();

            var result = controller.Delete(5, null) as HttpNotFoundResult;

            Assert.AreEqual(new HttpNotFoundResult().StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void DeleteMethodShouldReturnEventToDelete()
        {
            var eventList = new List<Event>
            {
                new Event
                { 
                    Id = 1, EndDateTime = new DateTime(2014, 1, 12, 12, 0, 0), 
                    EventType = Models.EventType.Single,
                    StartDateTime = new DateTime(2014, 1, 12, 11, 0, 0),
                    Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}
                },
                 new Event
                { 
                    Id = 2, EndDateTime = new DateTime(2014, 1, 12, 12, 0, 0), 
                    EventType = Models.EventType.Single,
                    StartDateTime = new DateTime(2014, 1, 12, 11, 0, 0),
                    Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}
                },
                new Event
                { 
                    Id = 3, EndDateTime = new DateTime(2014, 1, 12, 12, 0, 0), 
                    EventType = Models.EventType.Single,
                    StartDateTime = new DateTime(2014, 1, 12, 11, 0, 0),
                    Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}
                },
                 new Event
                { 
                    Id = 4, EndDateTime = new DateTime(2014, 1, 12, 12, 0, 0), 
                    EventType = Models.EventType.Double,
                    StartDateTime = new DateTime(2014, 1, 12, 11, 0, 0),
                    Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}
                },
            };

            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new EventsController(uowDataMock.Object);

            var viewResult = controller.Delete(4, null) as PartialViewResult;
            var model = viewResult.Model as CreateEventViewModel;

            Assert.IsNotNull(viewResult, "The result of delete method is null");
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(4, model.Id, "The expected event is different.");
            Assert.AreEqual(EventType.Double, model.EventType, "EventType is different than expected.");
        }

        [TestMethod]
        public void DeleteConfirmedMethodShouldReturnHttpNotFoundBecauseOfMissingEvent()
        {
            var eventList = new List<Event>
            {
                new Event{ Id = 1, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 2, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 3, Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 4, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };

            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());
            
            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);
           
            var controller = new EventsController(uowDataMock.Object);

            var result = controller.DeleteConfirmed(5, 1) as HttpNotFoundResult;

            Assert.AreEqual(new HttpNotFoundResult().StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void DeleteConfirmedMethodShouldReturnJsonWhenEventIsDeleted()
        {
            var eventList = new List<Event>
            {
                new Event{ Id = 1, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 2, Users = new List<ApplicationUser>{new ApplicationUser{ Id = null}, new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 3, Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 4, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
                new Event
                { 
                    Id = 5, EndDateTime = new DateTime(2014, 1, 12, 12, 0, 0), 
                    EventType = Models.EventType.Double,
                    StartDateTime = new DateTime(2014, 1, 12, 11, 0, 0),
                    Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}, new ApplicationUser{ Id = null}}
                }
            };

            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = "2", Addresses = new List<Address>{new Address()}},
                new ApplicationUser { Id = null, Addresses = new List<Address>{new Address()}}
            };

            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());
            var usersRepoMock = new Mock<IRepository<ApplicationUser>>(MockBehavior.Strict);
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);
            bool isSaveChangesExecuted = false;
            uowDataMock.Setup(x => x.SaveChanges()).Returns(It.IsAny<int>).Callback(() => isSaveChangesExecuted = true);

            var controller = new EventsController(uowDataMock.Object);

            #region To test Url.Action() method.
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("/Events", UriKind.Relative));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(s => s.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);
            context.Setup(x => x.GetService(It.IsAny<Type>())).Returns(It.IsAny<Type>());

            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            context.SetupGet(x => x.User).Returns(principal);

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);
            #endregion

            int pageNumber = 1;
            var jsonResult = controller.DeleteConfirmed(5, pageNumber) as JsonResult;

            dynamic jsonResultData = jsonResult.Data;
            Assert.IsNotNull(jsonResult, "JsonResult is null.");
            Assert.AreEqual(true, jsonResultData.success);
            Assert.AreEqual("/Events/UserCalendar?pageNumber=" + pageNumber.ToString(), jsonResultData.url);
            Assert.AreEqual(true, isSaveChangesExecuted, "SaveChanges method is not invoked.");
        }

        [TestMethod]
        public void DeleteConfirmedMethodShouldReturnHttpNotFoundBecauseOfMissingUser()
        {
            // The controller.UserId is null.
            var eventList = new List<Event>
            {
                new Event{ Id = 1, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 2, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 3, Taken = true, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "1"}}},
                new Event{ Id = 4, Users = new List<ApplicationUser>{new ApplicationUser{ Id = "2"}}},
            };

            var eventsRepoMock = new Mock<IRepository<Event>>(MockBehavior.Strict);
            eventsRepoMock.Setup(x => x.All()).Returns(eventList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Events).Returns(eventsRepoMock.Object);

            var controller = new EventsController(uowDataMock.Object);

            var result = controller.DeleteConfirmed(1, 1) as HttpNotFoundResult;

            Assert.AreEqual(new HttpNotFoundResult().StatusCode, result.StatusCode);
        }
    }
}
