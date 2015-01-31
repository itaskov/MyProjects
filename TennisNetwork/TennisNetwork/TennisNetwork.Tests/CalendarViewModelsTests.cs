using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TennisNetwork.Data;
using TennisNetwork.Models;
using System.Linq;
using TennisNetwork.Controllers;

namespace TennisNetwork.Tests
{
    [TestClass]
    public class CalendarViewModelsTests
    {
        [TestMethod]
        public void CreateEventViewModelToEventMethodShouldReturnCorrectEventProperties()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = null, Addresses = new List<Address>{new Address()}}
            };

            var usersRepoMock = new Mock<IRepository<ApplicationUser>>(MockBehavior.Strict);
            usersRepoMock.Setup(x => x.All()).Returns(userList.AsQueryable());

            var uowDataMock = new Mock<IUowData>(MockBehavior.Strict);
            uowDataMock.Setup(x => x.Users).Returns(usersRepoMock.Object);

            var controller = new EventsController(uowDataMock.Object);

            var model = new CreateEventViewModel
            {
                EndDate = new DateTime(2014, 12, 14),
                EndTime = new DateTime(2014, 12, 14, 12, 0, 0),
                EventType = EventType.Single,
                StartDate = new DateTime(2014, 12, 13),
                StartTime = new DateTime(2014, 12, 13, 10, 30, 0),
            };

            var ev = CreateEventViewModel.ToEvent(model, controller);

            Assert.AreEqual(model.EndDate, ev.EndDateTime.Date);
            Assert.AreEqual(model.EndTime.ToShortTimeString(), ev.EndDateTime.ToShortTimeString());
            Assert.AreEqual(model.EventType, ev.EventType);
            Assert.AreEqual(model.StartDate, ev.StartDateTime.Date);
            Assert.AreEqual(model.StartTime.ToShortTimeString(), ev.StartDateTime.ToShortTimeString());
        }

        [TestMethod]
        public void CreateEventViewModelFromEventPropertyShouldReturnCorrectCreateEventViewModelProperties()
        {
            var events = new List<Event>
                                {
                                    new Event
                                    {
                                        AuthorId = "1",
                                        EndDateTime = new DateTime(2014, 12, 14, 12, 0, 0),
                                        EventType = EventType.Double,
                                        Id = 101,
                                        StartDateTime = new DateTime(2014, 12, 13, 10, 30, 0),
                                    }
                                };
            var ev = events[0];

            var model = events.AsQueryable().Select(CreateEventViewModel.FromEvent).First();

            Assert.AreEqual(ev.EndDateTime, model.EndDate);
            Assert.AreEqual(ev.EndDateTime, model.EndTime);
            Assert.AreEqual(ev.EventType, model.EventType);
            Assert.AreEqual(ev.StartDateTime, model.StartDate);
            Assert.AreEqual(ev.StartDateTime, model.StartTime);
            Assert.AreEqual(ev.Id, model.Id);
            Assert.AreEqual(ev.AuthorId, model.UserId);
        }
    }
}
