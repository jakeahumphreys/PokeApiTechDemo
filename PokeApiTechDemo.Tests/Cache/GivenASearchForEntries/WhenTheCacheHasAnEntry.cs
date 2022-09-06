using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PokeApiTechDemo.Data.Cache;
using PokeApiTechDemo.Data.Cache.Types;

namespace PokeApiTechDemo.Tests.Cache.GivenASearchForEntries
{
    [TestFixture]
    public class WhenTheCacheHasAValidEntry
    {
        private Mock<ICacheRepository> _repository;
        private List<CacheEntry> _result;

        [OneTimeSetUp]
        public void Setup()
        {
            _repository = new Mock<ICacheRepository>();
            _repository.Setup(x => x.GetEntriesForName(It.IsAny<string>()))
                .Returns(new List<CacheEntry>
                {
                    new CacheEntry
                    {
                        Time = DateTime.Parse("02/10/1997"),
                        Name = "CacheEntry",
                        Blob = "CacheEntryBlob"
                    }
                });

            var subject = new CacheService(_repository.Object);
            _result = subject.GetCacheEntriesForName("test");
        }

        [Test]
        public void ThenTheRepositoryIsCalled()
        {
            _repository.Verify(x => x.GetEntriesForName(It.Is<string>(y => y == "test")), Times.Once);
        }

        [Test]
        public void ThenTheEntryIsReturned()
        {
            Assert.That(_result.First().Name, Is.EqualTo("CacheEntry"));
            Assert.That(_result.First().Time, Is.EqualTo(DateTime.Parse("02/10/1997")));
            Assert.That(_result.First().Blob, Is.EqualTo("CacheEntryBlob"));
        }
    }
}