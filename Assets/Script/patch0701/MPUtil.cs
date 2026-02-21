using UnityEngine;
using System.Collections;
using System.Text;


public class MPUtil{

	// Use this for initialization

    //배열을 랜덤으로 섞어준다. 
    static public void RandomShuffle(Object[] data)
    {
        for (int t = 0; t < data.Length; t++)
        {
            Object tmp = data[t];
            int r = Random.Range(t, data.Length);
            data[t] = data[r];
            data[r] = tmp;
        }
    }


    //숫자에 1,000,000  콤마 넣어준다. 
    static public string MoneyFormatString(string str)
    {
        StringBuilder sb = new StringBuilder(str);

        for (int i = sb.Length, j = 0; i > 0; i--, j++)
        {
            if (j != 0 && j % 3 == 0)
            {
                sb.Insert(i, ",");
            }
        }

        str = sb.ToString();
        sb = null;
        return str;
    }
 

    static public SystemLanguage GetSystemLanguage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass localeClass = new AndroidJavaClass("java/util/Locale");
        AndroidJavaObject defaultLocale = localeClass.CallStatic<AndroidJavaObject>("getDefault");
        AndroidJavaObject usLocale = localeClass.GetStatic<AndroidJavaObject>("US");
        string systemLanguage = defaultLocale.Call<string>("getDisplayLanguage", usLocale);
        SystemLanguage code;
        try
        {
            code = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), systemLanguage);
        }
        catch
        {
            code = SystemLanguage.English;
        }
#else

        SystemLanguage code = Application.systemLanguage;
#endif
        return code;
    }
 
    
}
