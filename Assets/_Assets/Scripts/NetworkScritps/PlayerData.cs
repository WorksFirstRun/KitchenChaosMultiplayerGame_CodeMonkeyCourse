

using System;
using Unity.Netcode;

public struct PlayerData : IEquatable<PlayerData> , INetworkSerializable
{
   public ulong clientid;
   public int colorid;

   public bool Equals(PlayerData other)
   {
      return clientid == other.clientid && other.colorid == colorid;
   }


   public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
   {
         serializer.SerializeValue(ref clientid);
         serializer.SerializeValue(ref colorid);
   }
}
