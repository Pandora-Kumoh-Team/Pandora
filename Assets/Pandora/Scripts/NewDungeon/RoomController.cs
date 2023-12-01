using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pandora.Scripts.NewDungeon.Rooms;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.NewDungeon
{
    public class RoomInfo
    {
        public string name;
        public int X;
        public int Y;
    }

    public class RoomController : MonoBehaviour
    {
        public static RoomController Instance;
        private const string CurrentWorldName = "Basement";
        private RoomInfo _currentLoadRoomData;
        private Queue<RoomInfo> _loadRoomQueue = new Queue<RoomInfo>();
        public List<Room> loadedRooms = new List<Room>();
        private bool _isLoadingRoom = false;
        private int _notLoadedRoomCount = 0;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if(_isLoadingRoom)
            {
                return;
            }

            if(_loadRoomQueue.Count == 0)
            {
                return;
            }

            _currentLoadRoomData = _loadRoomQueue.Dequeue();
            _isLoadingRoom = true;

            LoadRoomScene(_currentLoadRoomData);
        }

        public void EnqueueRoomToGeneration(string name, int x, int y)
        {
            if(DoesRoomExist(x, y))
            {
                return;
            }

            var newRoomData = new RoomInfo
            {
                name = name,
                X = x,
                Y = y
            };

            _loadRoomQueue.Enqueue(newRoomData);
        }

        private void LoadRoomScene(RoomInfo info)
        {
            string roomName = CurrentWorldName + info.name;
            SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
            _notLoadedRoomCount++;
        }

        public void RegisterRoom(Room room)
        {
            if(!DoesRoomExist(_currentLoadRoomData.X, _currentLoadRoomData.Y))
            {
                room.transform.position = new Vector3(_currentLoadRoomData.X * room.Width, _currentLoadRoomData.Y * room.Height, 0);

                room.X = _currentLoadRoomData.X;
                room.Y = _currentLoadRoomData.Y;
                room.name = CurrentWorldName + "-" + _currentLoadRoomData.name + " " + room.X + ", " + room.Y;
                room.transform.parent = transform;

                _isLoadingRoom = false;

                loadedRooms.Add(room);
            }
            else
            {
                Destroy(room.gameObject);
                _isLoadingRoom = false;
            }
            _notLoadedRoomCount--;
            if (_notLoadedRoomCount == 0 && _loadRoomQueue.Count == 0)
            {
                var graphToScan = AstarPath.active.data.gridGraph;
                AstarPath.active.Scan(graphToScan);
                foreach (var loadedRoom in loadedRooms)
                {
                    loadedRoom.RemoveUnconnectedDoors();
                }
                EventManager.Instance.TriggerEvent(PandoraEventType.MapGenerateComplete);
            }
        }

        public bool DoesRoomExist(int x, int y)
        {
            return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
        }

        public Room FindRoom(int x, int y)
        {
            return loadedRooms.Find(item => item.X == x && item.Y == y);
        }
    }
}