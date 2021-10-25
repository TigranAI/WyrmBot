﻿using System.Collections.Generic;
using CardCollector.DataBase.Entity;

namespace CardCollector.Session.Modules
{
    public class UploadedStickersModule : Module
    {
        public List<StickerEntity> StickersList = new();
        public int MessageId = 0;
        public int Count => StickersList.Count;
        
        public void Reset()
        {
            StickersList.Clear();
            MessageId = 0;
        }
    }
}