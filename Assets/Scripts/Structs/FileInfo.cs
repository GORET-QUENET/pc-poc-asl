using HandPositionReader.Scripts.Enums;
using System;

namespace HandPositionReader.Scripts.Structs
{
    [Serializable]
    public struct FileWord
    {
        public EHandJointWord Word;
        public string FileName;
    }
}