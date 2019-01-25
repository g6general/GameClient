using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class main : MonoBehaviour
{
    public GameObject welcomeScreen;
    public GameObject mainScreen;

    public Button buttonSave;
    public InputField nickInputField;
    
    public Dropdown languageField;
    public Dropdown facebookField;
    public Dropdown appleField;
    public Dropdown googleField;
    
    public Text nickText;
    public Text titleText;
    
    public Text typeText;
    public Text type1Text;
    public Text type2Text;
    public Text type3Text;
    public Text type4Text;
    public Text type5Text;
    public Text type6Text;
    public Text type7Text;
    public Text type8Text;
    public Text type9Text;

    public Button buttonPlus;
    public Button buttonPlus1;
    public Button buttonPlus2;
    public Button buttonPlus3;
    public Button buttonPlus4;
    public Button buttonPlus5;
    public Button buttonPlus6;
    public Button buttonPlus7;
    public Button buttonPlus8;
    public Button buttonPlus9;
    
    public Button buttonMinus;
    public Button buttonMinus1;
    public Button buttonMinus2;
    public Button buttonMinus3;
    public Button buttonMinus4;
    public Button buttonMinus5;
    public Button buttonMinus6;
    public Button buttonMinus7;
    public Button buttonMinus8;
    public Button buttonMinus9;

    public InputField statusField;
    public InputField status1Field;
    public InputField status2Field;
    public InputField status3Field;
    public InputField status4Field;
    public InputField status5Field;
    public InputField status6Field;
    public InputField status7Field;
    public InputField status8Field;
    public InputField status9Field;

    public Button buttonSendData;
    public Button buttonReset;

    private Token token;
    private AccountInfo accountInfo;
    private AccountId accountId;
    private Status status;

    private enum lang
    {
        RUSSIAN,
        ENGLISH
    };
    
    private string urlLocalServer = "http://127.0.0.1:8000/";
    private string urlWebServer = "http://gameserver.pythonanywhere.com/";
    private string urlServer;

    private int state_id;
    private string state_nickname;
    private lang state_language;

    void Start()
    {
        urlServer = urlWebServer;
        StartCoroutine(tokenRequestGET());
        
        if (PlayerPrefs.HasKey("AccountId"))
        {
            state_id = PlayerPrefs.GetInt("AccountId");
            state_nickname = PlayerPrefs.GetString("Nickname");
            
            nickText.text = state_nickname;
            
            if (PlayerPrefs.GetInt("Language") == 0)
                state_language = lang.RUSSIAN;
            else
                state_language = lang.ENGLISH;

            if (state_language == lang.ENGLISH)
            {
                titleText.text = "Gamer data:";
                typeText.text = "Level";
                type1Text.text = "Coins";
                type2Text.text = "Crystals";
                type3Text.text = "Energy";
                type4Text.text = "Inventory";
                type5Text.text = "Consumables";
                type6Text.text = "Skill";
                type7Text.text = "Payed";
                type8Text.text = "Paying";
                type9Text.text = "Banned";
            }
            else
            {
                titleText.text = "Данные игрока:";
                typeText.text = "Уровень";
                type1Text.text = "Монеты";
                type2Text.text = "Кристаллы";
                type3Text.text = "Энергия";
                type4Text.text = "Инвентарь";
                type5Text.text = "Расходники";
                type6Text.text = "Мастерство";
                type7Text.text = "Оставил денег";
                type8Text.text = "Платящий";
                type9Text.text = "Забанен";
            }
            
            welcomeScreen.SetActive(false);
            mainScreen.SetActive(true);
            
            StartCoroutine(infoRequestGET(state_id));
        }
        else
        {
            state_id = -1;
            welcomeScreen.SetActive(true);
            mainScreen.SetActive(false);
            
            buttonSave.interactable = false;
        }
        
        Debug.Log("ACCOUNT ID: " + state_id);

        registration();
    }

    private void resetDataFromDisk(bool perform)
    {
        if (perform)
            PlayerPrefs.DeleteAll();
    }

    private void registration()
    {
        buttonSave.onClick.AddListener(() => onButtonSaveClick());
        
        buttonPlus.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus.name, incrementValue));
        buttonPlus1.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus1.name, incrementValue));
        buttonPlus2.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus2.name, incrementValue));
        buttonPlus3.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus3.name, incrementValue));
        buttonPlus4.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus4.name, incrementValue));
        buttonPlus5.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus5.name, incrementValue));
        buttonPlus6.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus6.name, incrementValue));
        buttonPlus7.onClick.AddListener(() => onButtonPlusMinusClick(buttonPlus7.name, incrementValue));
        
        buttonPlus8.onClick.AddListener(() => onButtonOnOffClick(buttonPlus8.name, setTrue));
        buttonPlus9.onClick.AddListener(() => onButtonOnOffClick(buttonPlus9.name, setTrue));
        
        buttonMinus.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus.name, decrementValue));
        buttonMinus1.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus1.name, decrementValue));
        buttonMinus2.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus2.name, decrementValue));
        buttonMinus3.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus3.name, decrementValue));
        buttonMinus4.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus4.name, decrementValue));
        buttonMinus5.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus5.name, decrementValue));
        buttonMinus6.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus6.name, decrementValue));
        buttonMinus7.onClick.AddListener(() => onButtonPlusMinusClick(buttonMinus7.name, decrementValue));
        
        buttonMinus8.onClick.AddListener(() => onButtonOnOffClick(buttonMinus8.name, setFalse));
        buttonMinus9.onClick.AddListener(() => onButtonOnOffClick(buttonMinus9.name, setFalse));

        nickInputField.onValueChange.AddListener(delegate { onInputNickname(); });

        buttonSendData.onClick.AddListener(() => onButtonSaveDataClick());
        
        buttonReset.onClick.AddListener(() => onButtonResetClick());
    }

    private void onInputNickname()
    {
        string value = nickInputField.text;
        
        if (value.Length > 0)
            buttonSave.interactable = true;
        else
            buttonSave.interactable = false;
    }

    private int incrementValue(int value) { return ++value; }
    private int decrementValue(int value) { return --value; }

    private string setTrue() { return "True"; }
    private string setFalse() { return "False"; }

    delegate int incrFunction(int value);
    delegate string onFunction();

    private void onButtonPlusMinusClick(string buttonName, incrFunction action)
    {
        if (buttonName == "Button_plus" || buttonName == "Button_minus")
        {
            int value = Convert.ToInt32(statusField.text);
            statusField.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_1" || buttonName == "Button_minus_1")
        {
            int value = Convert.ToInt32(status1Field.text);
            status1Field.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_2" || buttonName == "Button_minus_2")
        {
            int value = Convert.ToInt32(status2Field.text);
            status2Field.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_3" || buttonName == "Button_minus_3")
        {
            int value = Convert.ToInt32(status3Field.text);
            status3Field.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_4" || buttonName == "Button_minus_4")
        {
            int value = Convert.ToInt32(status4Field.text);
            status4Field.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_5" || buttonName == "Button_minus_5")
        {
            int value = Convert.ToInt32(status5Field.text);
            status5Field.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_6" || buttonName == "Button_minus_6")
        {
            int value = Convert.ToInt32(status6Field.text);
            status6Field.text = action(value).ToString();
        }
        else if (buttonName == "Button_plus_7" || buttonName == "Button_minus_7")
        {
            int value = Convert.ToInt32(status7Field.text);
            status7Field.text = action(value).ToString();
        }
    }

    private void onButtonOnOffClick(string buttonName, onFunction action)
    {
        if (buttonName == "Button_plus_8" || buttonName == "Button_minus_8")
        {
            status8Field.text = action();
        }
        else if (buttonName == "Button_plus_9" || buttonName == "Button_minus_9")
        {
            status9Field.text = action();
        }
    }
    
    private void onButtonSaveClick()
    {
        state_nickname = nickInputField.text;
        nickText.text = nickInputField.text;

        if (languageField.value == 0)
            state_language = lang.RUSSIAN;
        else
            state_language = lang.ENGLISH;

        if (state_language == lang.ENGLISH)
        {
            titleText.text = "Gamer data:";
            typeText.text = "Level";
            type1Text.text = "Coins";
            type2Text.text = "Crystals";
            type3Text.text = "Energy";
            type4Text.text = "Inventory";
            type5Text.text = "Consumables";
            type6Text.text = "Skill";
            type7Text.text = "Payed";
            type8Text.text = "Paying";
            type9Text.text = "Banned";
        }
        else
        {
            titleText.text = "Данные игрока:";
            typeText.text = "Уровень";
            type1Text.text = "Монеты";
            type2Text.text = "Кристаллы";
            type3Text.text = "Энергия";
            type4Text.text = "Инвентарь";
            type5Text.text = "Расходники";
            type6Text.text = "Мастерство";
            type7Text.text = "Оставил денег";
            type8Text.text = "Платящий";
            type9Text.text = "Забанен";
        }

        string nick = nickInputField.text;
        string currentLang = (state_language == main.lang.ENGLISH) ? "english" : "russian";
        string facebook = (facebookField.value == 0) ? "true" : "false";
        string apple = (appleField.value == 0) ? "true" : "false";
        string google = (googleField.value == 0) ? "true" : "false";
        
        StartCoroutine(addRequestPOST(nick, currentLang, facebook, apple, google));        
    }

    private void onButtonSaveDataClick()
    {
        StartCoroutine(saveRequestPOST());
        buttonSendData.GetComponentInChildren<Text>().text = "Сохраняю...";
    }

    private void onButtonResetClick()
    {
        resetDataFromDisk(true);
        
        state_id = -1;
        state_language = lang.RUSSIAN;
        state_nickname = "";

        languageField.value = 0;
        facebookField.value = 0;
        appleField.value = 0;
        googleField.value = 0;

        nickInputField.text = "";
        buttonSave.interactable = false;
        
        welcomeScreen.SetActive(true);
        mainScreen.SetActive(false);

        Debug.Log("ACCOUNT ID: " + state_id);
    }

    private void loadMainScreen(int newId, string newNick, lang newLang)
    {
        if (accountId.isValid)
        {
            PlayerPrefs.SetInt("AccountId", newId);
            PlayerPrefs.SetString("Nickname", newNick);
            PlayerPrefs.SetInt("Language", (int)newLang);            
            PlayerPrefs.Save();
            
            welcomeScreen.SetActive(false);
            mainScreen.SetActive(true);
        }
        else
        {
            Debug.Log("ERROR: can not load main screen!");
        }
    }

    private void fillMainScreen()
    {
        if (accountInfo.isValid)
        {
            statusField.text = $"{accountInfo.getLevel()}";
            status1Field.text = $"{accountInfo.getCoins()}";
            status2Field.text = $"{accountInfo.getCrystals()}";
            status3Field.text = $"{accountInfo.getEnergy()}";
            status4Field.text = $"{accountInfo.getInventory()}";
            status5Field.text = $"{accountInfo.getConsumables()}";
            status6Field.text = $"{accountInfo.getSkill()}";
            status7Field.text = $"{accountInfo.getPayed()}";
            status8Field.text = $"{accountInfo.getPaying()}";
            status9Field.text = $"{accountInfo.getBanned()}";
        }
        else
        {
            Debug.Log("ERROR: can not fill main screen!");
        }
    }

    IEnumerator tokenRequestGET()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(urlServer + "token/"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                token = JsonUtility.FromJson<Token>(www.downloadHandler.text);
                token.isValid = www.isDone;

                Debug.Log("TOKEN: " + token.csrf_token);
            }
        }
    }

    IEnumerator infoRequestGET(int accountId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(urlServer + "info/" + $"{accountId}/"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                accountInfo = JsonUtility.FromJson<AccountInfo>(www.downloadHandler.text);
                accountInfo.isValid = www.isDone;

                Debug.Log("LEVEL: " + accountInfo.getLevel());
                Debug.Log("COINS: " + accountInfo.getCoins());
                Debug.Log("CRYSTALS: " + accountInfo.getCrystals());
                Debug.Log("ENERGY: " + accountInfo.getEnergy());
                Debug.Log("INVENTORY: " + accountInfo.getInventory());
                Debug.Log("CONSUMABLES: " + accountInfo.getConsumables());
                Debug.Log("SKILL: " + accountInfo.getSkill());
                Debug.Log("PAYED: " + accountInfo.getPayed());
                Debug.Log("PAYING: " + accountInfo.getPaying());
                Debug.Log("BANNED: " + accountInfo.getBanned());

                fillMainScreen();
            }
        }
    }

    IEnumerator addRequestPOST(string nick, string language, string facebook, string apple, string google)
    {
        string contentType = "application/x-www-form-urlencoded";
        DateTime date = DateTime.Today;
        int timeInGame = UnityEngine.Random.Range(1, 1000);
        int numberOfSessions = UnityEngine.Random.Range(1, 500);
        int sessionTime = UnityEngine.Random.Range(1, 100);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("from-whom", "game", contentType));
        formData.Add(new MultipartFormDataSection("nick", nick, contentType));
        formData.Add(new MultipartFormDataSection("date", date.ToString(), contentType));
        formData.Add(new MultipartFormDataSection("language", language, contentType));
        formData.Add(new MultipartFormDataSection("facebook", facebook, contentType));
        formData.Add(new MultipartFormDataSection("apple", apple, contentType));
        formData.Add(new MultipartFormDataSection("google", google, contentType));
        formData.Add(new MultipartFormDataSection("time-in-game", $"{timeInGame}", contentType));
        formData.Add(new MultipartFormDataSection("number-of-sessions", $"{numberOfSessions}", contentType));
        formData.Add(new MultipartFormDataSection("session-time", $"{sessionTime}", contentType));

        UnityWebRequest www = UnityWebRequest.Post(urlServer + "add/", formData);
        
        www.SetRequestHeader("X-CSRFToken", $"{token.csrf_token}");        
        
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            accountId = JsonUtility.FromJson<AccountId>(www.downloadHandler.text);
            accountId.isValid = www.isDone;
            
            Debug.Log("New ID: " + accountId.getId());

            loadMainScreen(accountId.getId(), state_nickname, state_language);
        }
    }

    IEnumerator saveRequestPOST()
    {
        string contentType = "application/x-www-form-urlencoded";
        int accountId = PlayerPrefs.GetInt("AccountId");
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("from-whom", "game", contentType));
        formData.Add(new MultipartFormDataSection("id", $"{accountId}", contentType));
        formData.Add(new MultipartFormDataSection("level", statusField.text, contentType));
        formData.Add(new MultipartFormDataSection("coins", status1Field.text, contentType));
        formData.Add(new MultipartFormDataSection("crystals", status2Field.text, contentType));
        formData.Add(new MultipartFormDataSection("energy", status3Field.text, contentType));
        formData.Add(new MultipartFormDataSection("inventory", status4Field.text, contentType));
        formData.Add(new MultipartFormDataSection("consumables", status5Field.text, contentType));
        formData.Add(new MultipartFormDataSection("skill", status6Field.text, contentType));
        formData.Add(new MultipartFormDataSection("payed", status7Field.text, contentType));
        formData.Add(new MultipartFormDataSection("paying", status8Field.text, contentType));
        formData.Add(new MultipartFormDataSection("banned", status9Field.text, contentType));
        
        UnityWebRequest www = UnityWebRequest.Post(urlServer + "save/", formData);
        
        www.SetRequestHeader("X-CSRFToken", $"{token.csrf_token}");        
        
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            status = JsonUtility.FromJson<Status>(www.downloadHandler.text);
            status.isValid = www.isDone;
            
            buttonSendData.GetComponentInChildren<Text>().text = "Сохранить";

            Debug.Log("Status: " + status.status);
        }
    }
}

[Serializable]
public class Token
{
    public string csrf_token;
    public bool isValid = false;
}

[Serializable]
public class AccountInfo
{
    public string level;
    public string coins;
    public string crystals;
    public string energy;
    public string inventory;
    public string consumables;
    public string skill;
    public string payed;
    public string paying;
    public string banned;

    private int castToInt(string value)
    {
        int result = 0;
        Int32.TryParse(value, out result);
        return result;
    }

    public int getLevel() { return castToInt(level); }
    public int getCoins() { return castToInt(coins); }
    public int getCrystals() { return castToInt(crystals); }
    public int getEnergy() { return castToInt(energy); }
    public int getInventory() { return castToInt(inventory); }
    public int getConsumables() { return castToInt(consumables); }
    public int getSkill() { return castToInt(skill); }
    public int getPayed() { return castToInt(payed); }

    public bool getPaying() { return paying == "True" ? true : false; }
    public bool getBanned() { return banned == "True" ? true : false; }

    public bool isValid = false;
}

[Serializable]
public class AccountId
{
    public string new_id;
    public bool isValid = false;

    public int getId()
    {
        int result = 0;
        Int32.TryParse(new_id, out result);
        return result;
    }
}

[Serializable]
public class Status
{
    public string status;
    public bool isValid = false;
}
