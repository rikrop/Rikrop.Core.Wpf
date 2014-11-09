using System.ComponentModel;

namespace Rikrop.Core.Wpf.TestApplication
{
    [TypeConverter(typeof(EnumToStringConverter))]
    public enum TestEnum
    {
        [Description("Hahaha")]
        Test
    }
}