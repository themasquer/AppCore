using AppCore.Business.Concretes.Models.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Business.Concretes.Models.File;
using AppCore.Business.Configs;

namespace AppCore.Business.Abstracts.Utils.File
{
    public abstract class FileUtilBase
    {
        private Dictionary<string, string> _mimeTypes;

        protected FileUtilBase()
        {
            _mimeTypes = new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.ms-word" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".csv", "text/csv" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" }
            };
        }

        public virtual Dictionary<string, string> GetMimeTypes()
        {
            return _mimeTypes;
        }

        public virtual void AddMimeType(string fileExtension, string mimeType)
        {
            if (!_mimeTypes.Keys.Contains(fileExtension))
                _mimeTypes.Add(fileExtension, mimeType);
        }

        public virtual Result UploadFileByEntityId(int entityId, IFormFile file, bool overwrite = true, params string[] hierarchicalDirectories)
        {
			try
            {
                string fileName = file.FileName;
                string fileExtension = Path.GetExtension(fileName);
                fileName = entityId + fileExtension;
                string filePath = CreatePath(fileName, hierarchicalDirectories);
                var fileMode = FileMode.Create;
                if (!overwrite)
                    fileMode = FileMode.CreateNew;
                using (FileStream fileStream = new FileStream(filePath, fileMode))
                {
                    file.CopyTo(fileStream);
                }
                return new SuccessResult();
            }
			catch (Exception exc)
			{
				return new ExceptionResult(exc);
			}
        }

        public virtual async Task<Result> UploadFileByEntityIdAsync(int entityId, IFormFile file, bool overwrite = true, params string[] hierarchicalDirectories)
        {
            try
            {
                string fileName = file.FileName;
                string fileExtension = Path.GetExtension(fileName);
                fileName = entityId + fileExtension;
                string filePath = CreatePath(fileName, hierarchicalDirectories);
                var fileMode = FileMode.Create;
                if (!overwrite)
                    fileMode = FileMode.CreateNew;
                using (FileStream fileStream = new FileStream(filePath, fileMode))
                {
                    await file.CopyToAsync(fileStream);
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public virtual Result UploadFile(IFormFile file, bool overwrite = true, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(file.FileName, hierarchicalDirectories);
                var fileMode = FileMode.Create;
                if (!overwrite)
                    fileMode = FileMode.CreateNew;
                using (FileStream fileStream = new FileStream(filePath, fileMode))
                {
                    file.CopyTo(fileStream);
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public virtual async Task<Result> UploadFileAsync(IFormFile file, bool overwrite = true, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(file.FileName, hierarchicalDirectories);
                var fileMode = FileMode.Create;
                if (!overwrite)
                    fileMode = FileMode.CreateNew;
                using (FileStream fileStream = new FileStream(filePath, fileMode))
                {
                    await file.CopyToAsync(fileStream);
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public virtual Result CheckFileExtension(IFormFile file, params string[] acceptedExtensions)
        {
            try
            {
                bool result = CheckFileExtensionByFileName(file.FileName, acceptedExtensions);
                if (result)
                    return new SuccessResult();
                return new ErrorResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public virtual Result<string> GetFileRelativePathByEntityId(int entityId, params string[] hierarchicalDirectories)
        {
            try
            {
                string[] directories = new string[hierarchicalDirectories.Length - 1];
                for (int i = 1; i < hierarchicalDirectories.Length; i++)
                {
                    directories[i - 1] = hierarchicalDirectories[i];
                }    
                string filePath = CreatePath(hierarchicalDirectories);
                string fileNameWithoutPath = GetFileNameWithoutPath(entityId.ToString(), filePath);
                string fileRelativePath = "";
                if (fileNameWithoutPath != null)
                {
                    for (int i = 0; i < directories.Length; i++)
                    {
                        fileRelativePath += "/" + directories[i];
                    }
                    fileRelativePath += "/" + fileNameWithoutPath;
                }
                if (String.IsNullOrWhiteSpace(fileRelativePath))
                    return new ErrorResult<string>();
                return new SuccessResult<string>("", fileRelativePath);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<string>(exc);
            }
        }

        public virtual Result DeleteFilesByEntityId(int entityId, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(hierarchicalDirectories);
                string[] fileNamesWithPath = GetFileNamesWithPath(entityId.ToString(), filePath);
                foreach (string fileNameWithPath in fileNamesWithPath)
                {
                    System.IO.File.Delete(fileNameWithPath);
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public virtual Result DeleteFile(string fileName, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(fileName, hierarchicalDirectories);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ExceptionResult(exc);
            }
        }

        public virtual Result<FileDownload> GetFileToDownloadByEntityId(int entityId,
            bool useOctetStreamContentType = false, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(hierarchicalDirectories);
                string fileNameWithoutPath = GetFileNameWithoutPath(entityId.ToString(), filePath);
                if (String.IsNullOrWhiteSpace(fileNameWithoutPath))
                    return new ErrorResult<FileDownload>(FileUtilConfig.FileNotFoundMessage);
                FileDownload fileDownload = new FileDownload
                {
                    FileStream = new FileStream(Path.Combine(filePath, fileNameWithoutPath), FileMode.Open),
                    ContentType = useOctetStreamContentType ? "application/octet-stream" : GetContentType(fileNameWithoutPath),
                    FileName = fileNameWithoutPath
                };
                return new SuccessResult<FileDownload>(fileDownload);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<FileDownload>(exc);
            }
        }

        public virtual Result<FileDownload> GetFileToDownloadByEntityId(int entityId, string downloadFileNameWithoutExtension,
            bool useOctetStreamContentType = false, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(hierarchicalDirectories);
                string fileNameWithoutPath = GetFileNameWithoutPath(entityId.ToString(), filePath);
                if (String.IsNullOrWhiteSpace(fileNameWithoutPath))
                    return new ErrorResult<FileDownload>(FileUtilConfig.FileNotFoundMessage);
                string fileExtension = Path.GetExtension(fileNameWithoutPath);
                FileDownload fileDownload = new FileDownload
                {
                    FileStream = new FileStream(Path.Combine(filePath, fileNameWithoutPath), FileMode.Open),
                    ContentType = useOctetStreamContentType ? "application/octet-stream" : GetContentType(fileNameWithoutPath),
                    FileName = downloadFileNameWithoutExtension + fileExtension
                };
                return new SuccessResult<FileDownload>(fileDownload);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<FileDownload>(exc);
            }
        }

        public virtual Result<FileDownload> GetFileToDownload(string fileName,
            bool useOctetStreamContentType = false, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(fileName, hierarchicalDirectories);
                if (!System.IO.File.Exists(filePath))
                    return new ErrorResult<FileDownload>(FileUtilConfig.FileNotFoundMessage);
                FileDownload fileDownload = new FileDownload
                {
                    FileStream = new FileStream(filePath, FileMode.Open),
                    ContentType = useOctetStreamContentType ? "application/octet-stream" : GetContentType(fileName),
                    FileName = fileName
                };
                return new SuccessResult<FileDownload>(fileDownload);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<FileDownload>(exc);
            }
        }

        public virtual Result<FileDownload> GetFileToDownload(string fileName, string downloadFileNameWithoutExtension,
            bool useOctetStreamContentType = false, params string[] hierarchicalDirectories)
        {
            try
            {
                string filePath = CreatePath(fileName, hierarchicalDirectories);
                if (!System.IO.File.Exists(filePath))
                    return new ErrorResult<FileDownload>(FileUtilConfig.FileNotFoundMessage);
                string fileExtension = Path.GetExtension(fileName);
                FileDownload fileDownload = new FileDownload
                {
                    FileStream = new FileStream(filePath, FileMode.Open),
                    ContentType = useOctetStreamContentType ? "application/octet-stream" : GetContentType(fileName),
                    FileName = downloadFileNameWithoutExtension + fileExtension
                };
                return new SuccessResult<FileDownload>(fileDownload);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<FileDownload>(exc);
            }
        }

        private string GetContentType(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath).ToLower();
            return _mimeTypes[fileExtension];
        }

        private string CreatePath(params string[] hierarchicalDirectories)
        {
            return Path.Combine(hierarchicalDirectories);
        }

        private string CreatePath(string fileName, params string[] hierarchicalDirectories)
        {
            string[] path = new string[hierarchicalDirectories.Length + 1];
            int i;
            for (i = 0; i < hierarchicalDirectories.Length; i++)
            {
                path[i] = hierarchicalDirectories[i];
            }
            path[i] = fileName;
            return Path.Combine(path);
        }

        private string[] GetFileNamesWithPath(string fileNameWithoutExtension, string filePath)
        {
            string[] files = Directory.GetFiles(filePath);
            return files.Where(e => Path.GetFileNameWithoutExtension(e) == fileNameWithoutExtension).ToArray();
        }

        private string GetFileNameWithoutPath(string fileNameWithoutExtension, string filePath)
        {
            string[] files = Directory.GetFiles(filePath);
            string file = files.FirstOrDefault(e => Path.GetFileNameWithoutExtension(e) == fileNameWithoutExtension);
            if (file == null)
                return null;
            return Path.GetFileName(file);
        }

        private bool CheckFileExtensionByFileName(string fileName, string[] acceptedExtensions)
        {
            string fileExtension = Path.GetExtension(fileName);
            if (acceptedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}
