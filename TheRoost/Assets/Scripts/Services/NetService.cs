using System;
using UnityEngine;
using Game.Controllers.Network.Enums;
using System.Collections.Generic;
using Game.Enums;
using System.Text;
using Game.Controllers.Network.Types;
using Models;
using Events;

namespace Services
{
	public class NetService : MonoBehaviour
	{
		private enum NetworkStatus
		{
			WaitingForOpponent,
			Loading,
			LoadingDone,
			Playing
		}

		//TODO: Add support for more rooms, so more than 1 group can play at a given time.
		private const string ROOM_NAME = "TheRoost";

		private const string EVENT_SEP = "|";
		private readonly string[] EVENT_SEP_SPLIT = { "|" };
		private const string TITLE_SEP = "::";
		private readonly string[] TITLE_SEP_SPLIT = { "::" };
		private const string DATA_SEP = "#";
		private readonly string[] DATA_SEP_SPLIT = { "#" };

		public int PlayerId { get { return PhotonNetwork.player.ID; } }
		public bool IsMaster { get{ return PhotonNetwork.player.isMasterClient; } set {}}
		public bool IsInitialized { get{ return m_isInitialized; } set{} }

		private bool m_isInitialized;
		private bool m_isOpponenetLoaded;
		private NetworkStatus m_netStatus;

		private string m_eventBatch;
		private int m_updateStep;
		private float m_localStepTime;

		public delegate void NetUpdateDelegate(float dt);

		private Action m_onStartBattleLoad;
		private Action m_onStartBattleReady;

		private float m_lastUpdateTime;

		private List<Entity> opponents;

		public void Awake()
		{
			if (Service.Network == null)
			{
				Service.Network = this;
			}
		}

		//Connect to the server and wait for an opponent to connect.
		public void Connect(Action startBattleCallback)
		{
			if(!m_isInitialized)
			{
				//Serialize player team
				m_onStartBattleLoad = startBattleCallback;
				m_updateStep = 0;
				m_eventBatch = "";
				m_netStatus = NetworkStatus.WaitingForOpponent;
				m_isInitialized = true;
				PhotonNetwork.ConnectUsingSettings("0.01");
			}
		}

		public void OnJoinedLobby()
		{
			RoomOptions options = new RoomOptions();
			options.isOpen = true;
			options.isVisible = true;
			PhotonNetwork.JoinOrCreateRoom(ROOM_NAME, options, TypedLobby.Default);
			
			PhotonNetwork.OnEventCall += OnNetworkEvent;
		}

		public void OnJoinedRoom()
		{
			string content = (object)NetworkEvents.InitialHandshake.ToString() + TITLE_SEP + PlayerId;
			PhotonNetwork.RaiseEvent(1, content, true, RaiseEventOptions.Default);
		}

		public void OnNetworkEvent(byte eventCode, object content, int senderID)
		{
			string[] rawEvents = content.ToString().Split(EVENT_SEP_SPLIT, StringSplitOptions.None);
			for(int i = 0, count = rawEvents.Length; i < count; i++)
			{
				string[] eventParts = rawEvents[i].Split(TITLE_SEP_SPLIT, StringSplitOptions.None);
				NetworkEvents eventType = (NetworkEvents)Enum.Parse(typeof(NetworkEvents), eventParts[0]);
				string eventData = eventParts[1];
				ParseAndBroadcastEvent(eventType, eventData);
			}
		}

		private void ParseAndBroadcastEvent(NetworkEvents eventType, string eventData)
		{
			string[] dataParts = null;
			if(!string.IsNullOrEmpty(eventData))
			{
				dataParts = eventData.Split(DATA_SEP_SPLIT, StringSplitOptions.None);
			}

			if(eventType == NetworkEvents.GameStartHandshake)
			{
				m_isOpponenetLoaded = true;

				if(m_netStatus == NetworkStatus.LoadingDone)
				{
					m_netStatus = NetworkStatus.Playing;
					string handshakeEvent = (object)NetworkEvents.GameStartHandshake.ToString () + TITLE_SEP + "1";
					PhotonNetwork.RaiseEvent(1, handshakeEvent, true, RaiseEventOptions.Default);
					ProcessNextUpdate(0.016666667f);
					if(m_onStartBattleReady != null)
					{
						m_onStartBattleReady();
					}
				}
			}
			else if(eventType == NetworkEvents.InitialHandshake)
			{
				//Generate enemy hero list...
				opponents = new List<Entity>();
				if(m_netStatus == NetworkStatus.WaitingForOpponent)
				{
					m_netStatus = NetworkStatus.Loading;
					string content = 
						(object)NetworkEvents.InitialHandshake.ToString () + TITLE_SEP + PlayerId;
					PhotonNetwork.RaiseEvent(1, content, true, RaiseEventOptions.Default);
					m_onStartBattleLoad();
				}
			}
			else if(eventType == NetworkEvents.PlayerDisconnect)
			{
				Service.Events.SendEvent (EventId.NetPlayerDisconnected, null);
			}
			else if(eventType == NetworkEvents.PlayerMove)
			{
				Vector3 movePos = new Vector3(Convert.ToSingle(dataParts[0]), 
											  Convert.ToSingle(dataParts[1]), 
										   	  Convert.ToSingle(dataParts[2]));
				Service.Events.SendEvent (EventId.EntityMoved, movePos);
			}
			else if(eventType == NetworkEvents.EntityHealth)
			{
				NetEntityHealthUpdateType healthUpdate = 
					new NetEntityHealthUpdateType(dataParts[0], dataParts[1], Convert.ToSingle(dataParts[2]));
				Service.Events.SendEvent(EventId.EntityHealthUpdate, healthUpdate);
			}
			else if(eventType == NetworkEvents.EntityDeath)
			{
				NetEntityHealthUpdateType healthUpdate = 
					new NetEntityHealthUpdateType(dataParts[0], dataParts[1], 0f);
				Service.Events.SendEvent(EventId.EntityDestroyed, healthUpdate);
			}
			else if(eventType == NetworkEvents.EntitySpawned)
			{
//				m_eventBatch += NetworkEvents.EntitySpawned + TITLE_SEP +
//					entity.Id + DATA_SEP +
//					Vector3Encode (entity.SpawnPos) + DATA_SEP +
//					Vector3Encode (entity.SpawnRotation) + DATA_SEP +
//					entity.EntityType.ToString () + DATA_SEP +
//					entity.EntryName;
				NetSpawnEntityType spawnInfo = new NetSpawnEntityType(dataParts[0], 
																	  TeamID.Enemy, 
																	  Vector3Parse(dataParts[1]), 
																	  Vector3Parse(dataParts[2]),
																	  dataParts[3],
																	  dataParts[4]);
				Service.Events.SendEvent (EventId.EntitySpawned, spawnInfo);
			}
			else if(eventType == NetworkEvents.EntityMoved)
			{
				Vector3 movePos = new Vector3(Convert.ToSingle(dataParts[1]), 
											  Convert.ToSingle(dataParts[2]), 
										   	  Convert.ToSingle(dataParts[3]));
				NetEntityMoveToType moveUpdate = new NetEntityMoveToType(dataParts[0], movePos);
				Service.Events.SendEvent(EventId.EntityMoved, moveUpdate);
			}
			else if(eventType == NetworkEvents.EntityAtPosition)
			{
				string unitId = dataParts[0];
				Vector3 pos = new Vector3(Convert.ToSingle(dataParts[1]), 
										  Convert.ToSingle(dataParts[2]), 
										  Convert.ToSingle(dataParts[3]));
				float rotation = Convert.ToSingle(dataParts[4]);
				NetEntityTransformType transformType = new NetEntityTransformType(unitId, pos, rotation);
				Service.Events.SendEvent(EventId.EntityTransformUpdated, transformType);
			}
		}

		//Called when the battle has been loaded and we're ready to start our update loop.
		public bool GameLoaded(Action onGameStartReady)
		{
			m_onStartBattleReady = onGameStartReady;

			PhotonNetwork.RaiseEvent(1, (object)NetworkEvents.GameStartHandshake.ToString() + TITLE_SEP + "1", true, RaiseEventOptions.Default);
			if(m_isOpponenetLoaded)
			{
				m_netStatus = NetworkStatus.Playing;
				ProcessNextUpdate(0.016666667f);
				if(m_onStartBattleReady != null)
				{
					m_onStartBattleReady();
					return true;
				}
			}
			else
			{
				m_netStatus = NetworkStatus.LoadingDone;
			}
			return false;
		}

		//This is the last method called at the end of the update.
		public void SyncUpdateStep()
		{
			//Only send an event if there is data in the eventBatch
			if(m_eventBatch != "")
			{
				m_updateStep++;
				m_localStepTime = Time.realtimeSinceStartup - m_lastUpdateTime;
				m_eventBatch += EVENT_SEP + NetworkEvents.UpdateSync.ToString() + TITLE_SEP + m_updateStep + DATA_SEP + m_localStepTime;
				PhotonNetwork.RaiseEvent(1, (object)m_eventBatch, true, RaiseEventOptions.Default);
				m_eventBatch = "";
			}
		}

		private void ProcessNextUpdate(float updateTime)
		{
			m_lastUpdateTime = Time.realtimeSinceStartup;
		}

		public void BroadcastPlayerMove(Vector3 moveTarget)
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.PlayerMove.ToString() + TITLE_SEP + moveTarget.x.ToString() + DATA_SEP + moveTarget.y.ToString() + DATA_SEP + moveTarget.z.ToString();
		}

		public void BroadcastPlayerAbility(string abilityVoId)
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.PlayerAbility + TITLE_SEP + abilityVoId;
		}

		public void BroadcastCompanionAbility(string companionId, string abilityVoId)
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.CompanionAbility + TITLE_SEP + companionId + DATA_SEP + abilityVoId;
		}

		public void BroadcastEntitySpawned(Entity entity)
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.EntitySpawned + TITLE_SEP +
							entity.Id + DATA_SEP +
							Vector3Encode (entity.SpawnPos) + DATA_SEP +
							Vector3Encode (entity.SpawnRotation) + DATA_SEP +
							entity.Type.ToString () + DATA_SEP +
							entity.EntryName;
		}

		public void BroadcastEntityHealthChanged(string entityId, string enemyEntityId, float health)
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.EntityHealth.ToString() + TITLE_SEP + entityId + DATA_SEP + enemyEntityId + DATA_SEP + health.ToString();
		}

		public void BroadcastEntityDeath(string entityId, string enemyEntityId)
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.EntityDeath.ToString() + TITLE_SEP + entityId + DATA_SEP + enemyEntityId;
		}

		public void BroadcastCurrentTransform(Entity unit)
		{
//			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
//			Vector3 unitPos = unit.CharacterGO.transform.position;
//			m_eventBatch += NetworkEvents.EntityAtPosition.ToString() + TITLE_SEP + unit.EntityID + DATA_SEP + unitPos.x + DATA_SEP + unitPos.y + DATA_SEP + unitPos.z + DATA_SEP + unit.CharacterGO.transform.eulerAngles.y;
		}

		public void BroadcastDisconnect()
		{
			if(!string.IsNullOrEmpty(m_eventBatch)) m_eventBatch += EVENT_SEP;
			m_eventBatch += NetworkEvents.PlayerDisconnect + TITLE_SEP + "";
		}

		public string Vector3Encode(Vector3 input)
		{
			return input.x + "," + input.y + "," + input.z;
		}

		public Vector3 Vector3Parse(string input)
		{
			string[] vectorParts = input.Split(new string[] { "," }, StringSplitOptions.None);
			Vector3 result = new Vector3 (Convert.ToSingle (vectorParts [0]), 
										  Convert.ToSingle (vectorParts [1]), 
										  Convert.ToSingle (vectorParts [2]));
			return result;
		}

		public void Disconnect()
		{
			PhotonNetwork.Disconnect();

			PhotonNetwork.OnEventCall -= OnNetworkEvent;

			m_isInitialized = false;
			m_isOpponenetLoaded = false;
			m_netStatus = NetworkStatus.Loading;
			
			opponents = null;
			
			m_onStartBattleLoad = null;
			m_onStartBattleReady = null;
		}
	}
}
