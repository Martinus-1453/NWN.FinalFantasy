﻿using System;
using NWN.FinalFantasy.Data;
using NWN.FinalFantasy.Data.Entity;
using NWN.FinalFantasy.Data.Repository;

namespace NWN.FinalFantasy.Migration.Server
{
    internal class ServerVersion1: IServerMigration
    {
        public int Version { get; }

        public ServerVersion1(int version)
        {
            Version = version;
        }

        public void RunMigration()
        {
            SetDMList();
        }

        private static void SetDMList()
        {
            var dmList = new EntityList<DM>(Guid.NewGuid());
            
            DMRepo.Set(dmList);
        }
    }
}