﻿using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using Coding4Fun.Toolkit.Net;
using S1Parser;

namespace S1Nyan.Model
{
    public class NetResourceService : IResourceService
    {
        const string defaultDir = "cache";

        // TODO fix cache issue with <img src="images/post/smile/goose/13.gif" />
        public static async Task<Stream> GetResourceStreamStatic(Uri uri, string path = null, int expireDays = 3)
        {
            Stream s = null;
            IsolatedStorageFile local = null;

            if (path != null)
            {
                local = IsolatedStorageFile.GetUserStoreForApplication();
                path = path.Replace('/', '\\');
                if (!CreateDirIfNecessary(local, path))
                    path = defaultDir;

                if (local.FileExists(path))
                {
                    if (expireDays < 0 || (DateTime.Now - local.GetLastWriteTime(path) < TimeSpan.FromDays(expireDays)))
                    {
                        return new IsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, local);
                    }
                }
            }
            s = await new GzipWebClient().OpenReadTaskAsync(uri);
            if (path != null && s != null)
            {
                using (var fileStream = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, local))
                {
                    using (var isoFileWriter = new BinaryWriter(fileStream))
                    {
                        var reader = new BinaryReader(s);
                        isoFileWriter.Write(reader.ReadBytes((int)s.Length));
                        isoFileWriter.Flush();
                    }
                }
                s.Seek(0, SeekOrigin.Begin);
            }
            return s;
        }

        private static bool CreateDirIfNecessary(IsolatedStorageFile local, string path)
        {
            int pos;
            bool hasDir = false;
            string dir = path;

            while ((pos = dir.IndexOf("\\")) != -1)
            {
                var dirname = dir.Substring(0, pos);
                if (!local.DirectoryExists(dirname))
                    local.CreateDirectory(dirname);
                hasDir = true;
                dir = dir.Substring(pos + 1);
            }
            return hasDir;
        }
        
        public Task<Stream> GetResourceStream(Uri uri, string path = null)
        {
            return GetResourceStreamStatic(uri, path);
        }
    }

    public class ApplicationResourceService : IResourceService
    {
        public static Task<Stream> GetResourceStreamStatic(Uri uri, string path = null)
        {
            return Task<Stream>.Factory.StartNew(() => Application.GetResourceStream(uri).Stream);
        }

        public Task<Stream> GetResourceStream(Uri uri, string path = null)
        {
            return GetResourceStreamStatic(uri, path);
        }
    }

}
