using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PokeApiTool.Data.Cache;
using PokeApiTool.Data.Cache.Types;

namespace PokeApiTool.Tests.Cache.GivenAnEntryToCache
{
   [TestFixture]
   [Parallelizable]
   public sealed class WhenAnEntryAlreadyExists
   {
      private Mock<ICacheRepository> _repository;

      [OneTimeSetUp]
      public void Setup()
      {
         _repository = new Mock<ICacheRepository>();
         _repository.Setup(x => x.GetEntriesForName(It.IsAny<string>()))
            .Returns(new List<CacheEntry>
            {
               new CacheEntry
               {
                  Name = "testname",
                  Time = DateTime.Parse("02-10-1997"),
                  Blob = "testblob"
               }
            });
         
          var subject = new CacheService(_repository.Object);
          subject.CacheResult("testname", "testblob");
      }

      [Test]
      public void ThenTheGetEntriesMethodIsCalled()
      {
         _repository.Verify(x => x.GetEntriesForName(It.Is<string>(y => y == "testname")), Times.Once);
      }

      [Test]
      public void ThenTheInsertMethodIsNotCalled()
      {
         _repository.Verify(x => x.Insert(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
      }
      
      [Test]
      public void ThenTheUpdateMethodIsCalled()
      {
         _repository.Verify(x => x.Update(It.Is<CacheEntry>(y => 
            y.Name == "testname" &&
            y.Blob == "testblob")), Times.Once);
      }
   }
}