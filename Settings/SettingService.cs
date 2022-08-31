using System.Collections.Generic;
using PokeApiTechDemo.Settings.Types;

namespace PokeApiTechDemo.Settings
{
    public sealed class SettingService
    {
        private readonly ISettingRepository _repository;

        public SettingService(ISettingRepository repository)
        {
            _repository = repository;
        }

        public Dictionary<string, Setting> LoadSettings()
        {
            return _repository.GetAll();
        }
    }
}