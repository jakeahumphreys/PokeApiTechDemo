using NUnit.Framework;
using PokeApiTechDemo.Data.Settings;
using PokeApiTechDemo.Data.Settings.Types;

namespace PokeApiTechDemo.Tests.Settings.GivenTheSettingService
{
    [TestFixture]
    [Parallelizable]
    public sealed class WhenGetToggleValueIsCalled
    {
        [TestCase("True", true)]
        [TestCase("False", false)]
        public void ThenTheCorrectValueIsReturned(string value, bool expectedResult)
        {
            var setting = new Setting
            {
                Key = "ToggleValue",
                Type = SettingType.Toggle,
                Value = value
            };

            var subject = new SettingService(null);
            var result = subject.GetToggleValue(setting);
            
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}