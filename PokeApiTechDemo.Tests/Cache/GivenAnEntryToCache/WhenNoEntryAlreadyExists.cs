using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PokeApiTechDemo.Data.Cache;
using PokeApiTechDemo.Data.Cache.Types;

namespace PokeApiTechDemo.Tests.Cache.GivenAnEntryToCache
{
   [TestFixture]
   [Parallelizable]
   public sealed class WhenNoEntryAlreadyExists
   {
      private Mock<ICacheRepository> _repository;

      [OneTimeSetUp]
      public void Setup()
      {
         _repository = new Mock<ICacheRepository>();
         _repository.Setup(x => x.GetEntriesForName(It.IsAny<string>()))
            .Returns(new List<CacheEntry>());
         
          var subject = new CacheService(_repository.Object);
          subject.CacheResult("testname", "testblob");
      }
      
      [Test]
      public void ThenTheGetEntriesMethodIsCalled()
      {
         _repository.Verify(x => x.GetEntriesForName(It.Is<string>(y => y == "testname")), Times.Once);
      }

      [Test]
      public void ThenTheUpdateMethodIsNotCalled()
      {
         _repository.Verify(x => x.Update(It.IsAny<CacheEntry>()), Times.Never);
      }
      
      [Test]
      public void ThenTheInsertMethodIsCalled()
      {
         _repository.Verify(x => x.Insert(It.Is<string>(y => y == "testname"), It.Is<string>(y => y == "testblob")), Times.Once);
      }
   }
}