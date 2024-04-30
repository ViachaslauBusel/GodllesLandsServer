using Game.GridMap;
using Game.Physics.Transform;
using Godless_Lands_Game.Quests.Components;
using NetworkGameEngine;
using Protocol.Data.Quests;
using Protocol.Data.Quests.Nodes;
using Protocol.Data.Replicated.Skins;
using Protocol.Data.Replicated.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Godless_Lands_Game.Quests.QuestNodeHandlers
{
    internal class NearbyNpcCheckQuestNodeHandler : IQuestNodeHandler
    {
        private TransformComponent _transform;
        private IGridMapService _mapService;

        [Inject] 
        public void InjectServices(IGridMapService mapService)
        {
            _mapService = mapService;
        }

        public void Init(Component component)
        {
            _transform = component.GetComponent<TransformComponent>();
        }

        public QuestNode Handle(Quest quest, QuestNode node)
        {
            if (node is NearbyNpcCheckQuestNode nearbyNpcCheckNode)
            {
                int npcId = nearbyNpcCheckNode.NpcId;

                GameObject npc = _mapService.GetGameObjectsAround(_transform.GameObject.ID).FirstOrDefault(g =>
                {
                    g.ReadData(out NpcSkinData npcSkin);
                    return npcSkin.SkinID == npcId;
                });

                if (npc != null)
                {
                    npc.ReadData(out TransformData transform);

                    if(Vector3.Distance(transform.Position, _transform.Position) < 10f)
                    {
                        return quest.Data.GetNode(nearbyNpcCheckNode.SuccessNodeId);
                    }
                }
            }

            return null;
        }
    }
}
