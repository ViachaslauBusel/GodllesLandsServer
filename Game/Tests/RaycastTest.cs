using Game.NetworkTransmission;
using Game.Physics;
using Game.Physics.Transform;
using NetworkGameEngine;
using Protocol;
using Protocol.MSG.Game.ToClient;
using Protocol.MSG.Game.ToServer;
using RUCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Game.Tests
{
    public class RaycastTest : Component
    {
        private NetworkTransmissionComponent m_networkTransmission;
        private RaycastingService m_raycastingService;
        private TransformComponent m_transform;

        [Inject]
        private void Construct(RaycastingService raycastingService)
        {
            m_raycastingService = raycastingService;
        }

        public override void Init()
        {
            m_transform = GetComponent<TransformComponent>();
            m_networkTransmission = GetComponent<NetworkTransmissionComponent>();
            m_networkTransmission.RegisterHandler(Opcode.MSG_RAYCAST_TEST, RaycastTestProcces);
        }

        private void RaycastTestProcces(Packet packet)
        {
            packet.Read(out MSG_RAYCAST_TEST_CS request);

            MSG_RAYCAST_TEST_SC response = new MSG_RAYCAST_TEST_SC();
            response.RaycastReslut = new List<Vector3>();

            for(int x = -2; x <= 2; x++) 
            { 
                for(int y = -2; y <= 2; y++)
                {
                    Vector3 start = m_transform.Position + new Vector3(x, 0, y);

                    if (m_raycastingService.GetTerrainPoint(start, out Vector3 point))
                    {
                        response.RaycastReslut.Add(point);
                    }
                }
            }
            m_networkTransmission.Socket.Send(response);
        }

        public override void OnDestroy()
        {
            m_networkTransmission.UnregisterHandler(Opcode.MSG_RAYCAST_TEST);
        }
    }
}
