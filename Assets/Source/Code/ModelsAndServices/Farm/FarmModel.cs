using System;
using System.Collections.Generic;
using Source.Code.StaticData;

namespace Source.Code.ModelsAndServices.Farm
{
    public class FarmModel
    {
        public Dictionary<CharacterTypeId, int> CharactersLevel;

        public FarmModel()
        {
            CharactersLevel = new();

            foreach (CharacterTypeId typeId in Enum.GetValues(typeof(CharacterTypeId)))
            {
                if(typeId == CharacterTypeId.None)
                    continue;

                CharactersLevel[typeId] = 0;
            }
        }
    }
}