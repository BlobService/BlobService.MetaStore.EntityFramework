﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class EfStoreOptions
    {
        public string ConnectionString { get; set; }
        public void TryValidate()
        {
            if (string.IsNullOrEmpty(ConnectionString)) throw new ArgumentNullException(nameof(ConnectionString));
        }
    }
}