using PostalTracking.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using PostalTracking.API.Controllers;

namespace PostalTracking.Tests
{
    public class Statuses_Should
    {
        DbContextOptions<PostalTrackingContext> _dbContextOptions;

        public Statuses_Should()
        {
            _dbContextOptions = new DbContextOptionsBuilder<PostalTrackingContext>()
                            .UseInMemoryDatabase(databaseName: "Test_database")
                            .Options;
        }

        // Testiranje dodavanja novih statusa u bazu podataka
        [Fact]
        public async void PostStatus()
        {
            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                for (int i = 0; i < 10; ++i)
                {
                    Status tmpStatus= new Status();
                    tmpStatus.StatusDescription = $"Status { i + 1 }";
                    tmpStatus.Active = true;
                    var result = await statusesAPI.PostStatus(tmpStatus);
                    var badRequest = result as BadRequestObjectResult;

                    Assert.Null(badRequest);    // Ako API ne vraca BadRequest, to znaci da je poziv uspjesan
                }
            }
        }

        // Testiranje dohvata statusa
        [Fact]
        public async void GetStatus()
        {
            // Dodavanje statusa prije dohvacanja
            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                for (int i = 0; i < 10; ++i)
                {
                    Status tmpStatus = new Status();
                    tmpStatus.StatusDescription = $"Status { i + 1 }";
                    tmpStatus.Active = true;
                    var result = await statusesAPI.PostStatus(tmpStatus);
                    var badRequest = result as BadRequestObjectResult;

                    Assert.Null(badRequest);    // Ako API ne vraca BadRequest, to znaci da je poziv uspjesan
                }
            }

            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                var result = await statusesAPI.GetStatus(5);
                var okResult = result as OkObjectResult;

                // Ako je rezultat Ok i status kod je 200, tada je poziv uspjesan
                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);

                // Ako je dohvacen status sa ispravnim ID-em, poziv je uspjesan
                Status status = okResult.Value as Status;
                Assert.NotNull(status);
                Assert.Equal(5, status.Id);
            }
        }

        // Testiranje brisanja statusa
        [Fact]
        public async void DeleteStatus()
        {
            // Dodavanje statusa prije dohvacanja
            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                for (int i = 0; i < 3; ++i)
                {
                    Status tmpStatus = new Status();
                    tmpStatus.StatusDescription = $"Status { i + 1 }";
                    tmpStatus.Active = true;
                    var result = await statusesAPI.PostStatus(tmpStatus);
                    var badRequest = result as BadRequestObjectResult;

                    Assert.Null(badRequest);    // Ako API ne vraca BadRequest, to znaci da je poziv uspjesan
                }
            }

            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                var result = await statusesAPI.DeleteStatus(1);
                var okResult = result as OkObjectResult;

                // Ako je rezultat Ok i status kod je 200, tada je poziv uspjesan
                Assert.NotNull(okResult);
                Assert.Equal(200, okResult.StatusCode);
            }
        }

        //Testiranje update-a statusa
        [Fact]
        public async void PutStatus()
        {
            // Dodavanje statusa prije dohvacanja
            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                for (int i = 0; i < 3; ++i)
                {
                    Status tmpStatus = new Status();
                    tmpStatus.StatusDescription = $"Status { i + 1 }";
                    tmpStatus.Active = true;
                    var result = await statusesAPI.PostStatus(tmpStatus);
                    var badRequest = result as BadRequestObjectResult;

                    Assert.Null(badRequest);    // Ako API ne vraca BadRequest, to znaci da je poziv uspjesan
                }
            }

            using (var context = new PostalTrackingContext(_dbContextOptions))
            {
                var statusesAPI = new StatusController(context);
                Status updateStatus = new Status {
                    Id = 1,
                    Active = false,
                    StatusDescription = "Updated status"
                };
                var result = await statusesAPI.PutStatus(1, updateStatus);
                var okResult = result as OkObjectResult;

                // Ako je dohvacen status sa ispravnim ID-em i vrijednosti su promijenjene, poziv je uspjesan
                Status status = okResult.Value as Status;
                Assert.NotNull(status);
                Assert.Equal(1, status.Id);
                Assert.False(status.Active);
                Assert.Equal("Updated status", status.StatusDescription);
            }
        }
    }
}
