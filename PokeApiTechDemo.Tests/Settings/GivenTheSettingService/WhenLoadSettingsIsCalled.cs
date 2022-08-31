using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PokeApiTechDemo.Settings;
using PokeApiTechDemo.Settings.Types;

namespace PokeApiTechDemo.Tests.Settings.GivenTheSettingService
{
    [TestFixture]
    [Parallelizable]
    public sealed class WhenLoadSettingsIsCalled
    {
        private Mock<ISettingRepository> _repository;
        private Dictionary<string, Setting> _result;

        [OneTimeSetUp]
        public void Setup()
        {
            _repository = new Mock<ISettingRepository>();
            _repository.Setup(x => x.GetAll())
                .Returns(new Dictionary<string, Setting>
                {
                    {
                        "Test1", new Setting
                        {
                            Key = "Test1",
                            Type = SettingType.Freetext,
                            Value = "TestValue1"
                        }
                    },
                    {
                        "Test2", new Setting
                        {
                            Key = "Test2",
                            Type = SettingType.Freetext,
                            Value = "TestValue2"
                        }
                    }
                });

            var subject = new SettingService(_repository.Object);
            _result = subject.LoadSettings();

        }
        
        [Test]
        public void ThenTheCorrectNumberOfSettingsIsReturned()
        {
            Assert.That(_result.Count, Is.EqualTo(2));
        }

        [Test]
        public void ThenASampleSettingIsMappedCorrectly()
        {
            var setting = _result.Values.First();
            Assert.That(setting.Key, Is.EqualTo("Test1"));
            Assert.That(setting.Type, Is.EqualTo(SettingType.Freetext));
            Assert.That(setting.Value, Is.EqualTo("TestValue1"));
        }
    }
}