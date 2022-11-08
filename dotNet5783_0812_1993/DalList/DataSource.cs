using System;
namespace Dal;


static class DataSource
{
    readonly static random randNum;

    internal static class Config
    {
        internal static int indexParit = 0;//index in array
        private static int paritId = 100000;
        internal static int ParitId { get => ++paritId; }
    }
    static int[] ParitArray = new int[50];
    static DataSource() => s_Initialize();

    static private void s_Initialize()
    {
        initParitArray();
    }
    static private void initParitArray()
    {

    }
}