﻿using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace BLL.Interfaces.Services {
    public interface IFileService {
        FileEntity GetFileEntity(int id);
        IEnumerable<FileEntity> GetFiles(string substring = null, int? userId = null);
        IEnumerable<FileEntity> GetAllFiles();
        IEnumerable<FileEntity> GetAllFiles(int userId);
        IEnumerable<FileEntity> GetFilesBySubstring(string substring, int? userId = null);
        void CreateFile(FileEntity fileEntity);
        void UpdateFile(FileEntity fileEntity);
        void DeleteFile(int id);
        byte[] GetPhysicalFile(int id);
    }
}