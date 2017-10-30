using System;
using System.Collections.Generic;
using System.Text;

namespace iMotto.Service
{
    public interface IObjectStorageService
    {
        string UploadFile(string key, string filePath);
    }
}
