using System.Globalization;
using System.ComponentModel;

public class NativeNameAttribute : Attribute
{
    public string LocalName { get; }
    public NativeNameAttribute(string localName) => LocalName = localName;
}

public class EnglishNameAttribute : Attribute
{
    public string EnglishName { get; }
    public EnglishNameAttribute(string englishName) => EnglishName = englishName;
}

public class CultureInfoNameAttribute : Attribute
{
    public string CultureInfoName { get; }
    public CultureInfoNameAttribute(string cultureInfoName) => CultureInfoName = cultureInfoName;
}

public static class CultureCodeExtensions
{
    public static string GetCultureInfoName(this CultureCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(CultureInfoNameAttribute), false)[0] as CultureInfoNameAttribute)?.CultureInfoName ?? string.Empty;
    public static string GetNativeName(this CultureCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;
    public static string GetEnglishName(this CultureCode code) => (code.GetType().GetMember(code.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;
    public static CultureInfo GetCultureInfo(this CultureCode code) => CultureInfo.GetCultureInfoByIetfLanguageTag(code.GetCultureInfoName());
    public static RegionInfo GetRegionInfo(this CultureCode code) => new RegionInfo(code.GetCultureInfo().Name);
}

///<summary>Contains a list of Two-letter culture codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with regioninfo</summary>
public enum CultureCode
{
    /// <summary>Afrikaans (Namibia)</summary>
    [NativeName("Afrikaans (Namibië)")]
    [EnglishName("Afrikaans (Namibia)")]
    [CultureInfoName("af-NA")]
    [Description("Afrikaans (Namibia)")]
    AF = 254, // NA 4096

    /// <summary>Aghem (Cameroon)</summary>
    [NativeName("Aghem (Kàmàlûŋ)")]
    [EnglishName("Aghem (Cameroon)")]
    [CultureInfoName("agq-CM")]
    [Description("Aghem (Cameroon)")]
    AGQ = 49, // CM 4096

    /// <summary>Akan (Ghana)</summary>
    [NativeName("Akan (Gaana)")]
    [EnglishName("Akan (Ghana)")]
    [CultureInfoName("ak-GH")]
    [Description("Akan (Ghana)")]
    AK = 89, // GH 4096

    /// <summary>Amharic (Ethiopia)</summary>
    [NativeName("አማርኛ (ኢትዮጵያ)")]
    [EnglishName("Amharic (Ethiopia)")]
    [CultureInfoName("am-ET")]
    [Description("Amharic (Ethiopia)")]
    AM = 73, // ET 1118

    /// <summary>Arabic (World)</summary>
    [NativeName("العربية (العالم)")]
    [EnglishName("Arabic (World)")]
    [CultureInfoName("ar-001")]
    [Description("Arabic (World)")]
    AR = 39070, // 001 4096

    /// <summary>Mapuche (Chile)</summary>
    [NativeName("Mapuche (Chile)")]
    [EnglishName("Mapuche (Chile)")]
    [CultureInfoName("arn-CL")]
    [Description("Mapuche (Chile)")]
    ARN = 46, // CL 1146

    /// <summary>Assamese (India)</summary>
    [NativeName("অসমীয়া (ভাৰত)")]
    [EnglishName("Assamese (India)")]
    [CultureInfoName("as-IN")]
    [Description("Assamese (India)")]
    AS = 113, // IN 1101

    /// <summary>Asu (Tanzania)</summary>
    [NativeName("Kipare (Tadhania)")]
    [EnglishName("Asu (Tanzania)")]
    [CultureInfoName("asa-TZ")]
    [Description("Asu (Tanzania)")]
    ASA = 239, // TZ 4096

    /// <summary>Asturian (Spain)</summary>
    [NativeName("asturianu (España)")]
    [EnglishName("Asturian (Spain)")]
    [CultureInfoName("ast-ES")]
    [Description("Asturian (Spain)")]
    AST = 217, // ES 4096

    /// <summary>Azerbaijani (Cyrillic, Azerbaijan)</summary>
    [NativeName("азәрбајҹан (Кирил, Азәрбајҹан)")]
    [EnglishName("Azerbaijani (Cyrillic, Azerbaijan)")]
    [CultureInfoName("az-Cyrl-AZ")]
    [Description("Azerbaijani (Cyrillic, Azerbaijan)")]
    AZ = 5, // AZ 2092

    /// <summary>Bashkir (Russia)</summary>
    [NativeName("Bashkir (Russia)")]
    [EnglishName("Bashkir (Russia)")]
    [CultureInfoName("ba-RU")]
    [Description("Bashkir (Russia)")]
    BA = 203, // RU 1133

    /// <summary>Basaa (Cameroon)</summary>
    [NativeName("Ɓàsàa (Kàmɛ̀rûn)")]
    [EnglishName("Basaa (Cameroon)")]
    [CultureInfoName("bas-CM")]
    [Description("Basaa (Cameroon)")]
    BAS = 49, // CM 4096

    /// <summary>Belarusian (Belarus)</summary>
    [NativeName("беларуская (Беларусь)")]
    [EnglishName("Belarusian (Belarus)")]
    [CultureInfoName("be-BY")]
    [Description("Belarusian (Belarus)")]
    BE = 29, // BY 1059

    /// <summary>Bemba (Zambia)</summary>
    [NativeName("Ichibemba (Zambia)")]
    [EnglishName("Bemba (Zambia)")]
    [CultureInfoName("bem-ZM")]
    [Description("Bemba (Zambia)")]
    BEM = 263, // ZM 4096

    /// <summary>Bena (Tanzania)</summary>
    [NativeName("Hibena (Hutanzania)")]
    [EnglishName("Bena (Tanzania)")]
    [CultureInfoName("bez-TZ")]
    [Description("Bena (Tanzania)")]
    BEZ = 239, // TZ 4096

    /// <summary>Bulgarian (Bulgaria)</summary>
    [NativeName("български (България)")]
    [EnglishName("Bulgarian (Bulgaria)")]
    [CultureInfoName("bg-BG")]
    [Description("Bulgarian (Bulgaria)")]
    BG = 35, // BG 1026

    /// <summary>Bamanankan (Mali)</summary>
    [NativeName("bamanakan (Mali)")]
    [EnglishName("Bamanankan (Mali)")]
    [CultureInfoName("bm-ML")]
    [Description("Bamanankan (Mali)")]
    BM = 244, // ML 4096

    /// <summary>Bangla (Bangladesh)</summary>
    [NativeName("বাংলা (বাংলাদেশ)")]
    [EnglishName("Bangla (Bangladesh)")]
    [CultureInfoName("bn-BD")]
    [Description("Bangla (Bangladesh)")]
    BN = 23, // BD 2117

    /// <summary>Tibetan (China)</summary>
    [NativeName("བོད་སྐད་ (རྒྱ་ནག)")]
    [EnglishName("Tibetan (China)")]
    [CultureInfoName("bo-CN")]
    [Description("Tibetan (China)")]
    BO = 45, // CN 1105

    /// <summary>Breton (France)</summary>
    [NativeName("brezhoneg (Frañs)")]
    [EnglishName("Breton (France)")]
    [CultureInfoName("br-FR")]
    [Description("Breton (France)")]
    BR = 84, // FR 1150

    /// <summary>Bodo (India)</summary>
    [NativeName("बड़ो (भारत)")]
    [EnglishName("Bodo (India)")]
    [CultureInfoName("brx-IN")]
    [Description("Bodo (India)")]
    BRX = 113, // IN 4096

    /// <summary>Bosnian (Cyrillic, Bosnia & Herzegovina)</summary>
    [NativeName("босански (ћирилица, Босна и Херцеговина)")]
    [EnglishName("Bosnian (Cyrillic, Bosnia & Herzegovina)")]
    [CultureInfoName("bs-Cyrl-BA")]
    [Description("Bosnian (Cyrillic, Bosnia & Herzegovina)")]
    BS = 25, // BA 8218

    /// <summary>Blin (Eritrea)</summary>
    [NativeName("Blin (Eritrea)")]
    [EnglishName("Blin (Eritrea)")]
    [CultureInfoName("byn-ER")]
    [Description("Blin (Eritrea)")]
    BYN = 71, // ER 4096

    /// <summary>Catalan (Andorra)</summary>
    [NativeName("català (Andorra)")]
    [EnglishName("Catalan (Andorra)")]
    [CultureInfoName("ca-AD")]
    [Description("Catalan (Andorra)")]
    CA = 8, // AD 4096

    /// <summary>Chakma (Bangladesh)</summary>
    [NativeName("𑄌𑄋𑄴𑄟𑄳𑄦 (𑄝𑄁𑄣𑄘𑄬𑄌𑄴)")]
    [EnglishName("Chakma (Bangladesh)")]
    [CultureInfoName("ccp-BD")]
    [Description("Chakma (Bangladesh)")]
    CCP = 244, // BD 4096

    /// <summary>Chechen (Russia)</summary>
    [NativeName("нохчийн (Росси)")]
    [EnglishName("Chechen (Russia)")]
    [CultureInfoName("ce-RU")]
    [Description("Chechen (Russia)")]
    CE = 203, // RU 4096

    /// <summary>Cebuano (Philippines)</summary>
    [NativeName("Cebuano (Pilipinas)")]
    [EnglishName("Cebuano (Philippines)")]
    [CultureInfoName("ceb-PH")]
    [Description("Cebuano (Philippines)")]
    CEB = 244, // PH 4096

    /// <summary>Chiga (Uganda)</summary>
    [NativeName("Rukiga (Uganda)")]
    [EnglishName("Chiga (Uganda)")]
    [CultureInfoName("cgg-UG")]
    [Description("Chiga (Uganda)")]
    CGG = 240, // UG 4096

    /// <summary>Cherokee (United States)</summary>
    [NativeName("ᏣᎳᎩ (ᏌᏊ ᎢᏳᎾᎵᏍᏔᏅ ᏍᎦᏚᎩ)")]
    [EnglishName("Cherokee (United States)")]
    [CultureInfoName("chr-US")]
    [Description("Cherokee (United States)")]
    CHR = 244, // US 4096

    /// <summary>Central Kurdish (Iraq)</summary>
    [NativeName("کوردیی ناوەندی (عێراق)")]
    [EnglishName("Central Kurdish (Iraq)")]
    [CultureInfoName("ckb-IQ")]
    [Description("Central Kurdish (Iraq)")]
    CKB = 244, // IQ 4096

    /// <summary>Corsican (France)</summary>
    [NativeName("Corsican (France)")]
    [EnglishName("Corsican (France)")]
    [CultureInfoName("co-FR")]
    [Description("Corsican (France)")]
    CO = 84, // FR 1155

    /// <summary>Czech (Czechia)</summary>
    [NativeName("čeština (Česko)")]
    [EnglishName("Czech (Czechia)")]
    [CultureInfoName("cs-CZ")]
    [Description("Czech (Czechia)")]
    CS = 75, // CZ 1029

    /// <summary>Church Slavic (Russia)</summary>
    [NativeName("Church Slavic (Russia)")]
    [EnglishName("Church Slavic (Russia)")]
    [CultureInfoName("cu-RU")]
    [Description("Church Slavic (Russia)")]
    CU = 203, // RU 4096

    /// <summary>Welsh (United Kingdom)</summary>
    [NativeName("Cymraeg (Y Deyrnas Unedig)")]
    [EnglishName("Welsh (United Kingdom)")]
    [CultureInfoName("cy-GB")]
    [Description("Welsh (United Kingdom)")]
    CY = 242, // GB 1106

    /// <summary>Danish (Denmark)</summary>
    [NativeName("dansk (Danmark)")]
    [EnglishName("Danish (Denmark)")]
    [CultureInfoName("da-DK")]
    [Description("Danish (Denmark)")]
    DA = 61, // DK 1030

    /// <summary>Taita (Kenya)</summary>
    [NativeName("Kitaita (Kenya)")]
    [EnglishName("Taita (Kenya)")]
    [CultureInfoName("dav-KE")]
    [Description("Taita (Kenya)")]
    DAV = 129, // KE 4096

    /// <summary>German (Austria)</summary>
    [NativeName("Deutsch (Österreich)")]
    [EnglishName("German (Austria)")]
    [CultureInfoName("de-AT")]
    [Description("German (Austria)")]
    DE = 14, // AT 3079

    /// <summary>Zarma (Niger)</summary>
    [NativeName("Zarmaciine (Nižer)")]
    [EnglishName("Zarma (Niger)")]
    [CultureInfoName("dje-NE")]
    [Description("Zarma (Niger)")]
    DJE = 173, // NE 4096

    /// <summary>Lower Sorbian (Germany)</summary>
    [NativeName("dolnoserbšćina (Nimska)")]
    [EnglishName("Lower Sorbian (Germany)")]
    [CultureInfoName("dsb-DE")]
    [Description("Lower Sorbian (Germany)")]
    DSB = 94, // DE 2094

    /// <summary>Duala (Cameroon)</summary>
    [NativeName("duálá (Cameroun)")]
    [EnglishName("Duala (Cameroon)")]
    [CultureInfoName("dua-CM")]
    [Description("Duala (Cameroon)")]
    DUA = 49, // CM 4096

    /// <summary>Divehi (Maldives)</summary>
    [NativeName("Divehi (Maldives)")]
    [EnglishName("Divehi (Maldives)")]
    [CultureInfoName("dv-MV")]
    [Description("Divehi (Maldives)")]
    DV = 165, // MV 1125

    /// <summary>Jola-Fonyi (Senegal)</summary>
    [NativeName("joola (Senegal)")]
    [EnglishName("Jola-Fonyi (Senegal)")]
    [CultureInfoName("dyo-SN")]
    [Description("Jola-Fonyi (Senegal)")]
    DYO = 210, // SN 4096

    /// <summary>Dzongkha (Bhutan)</summary>
    [NativeName("རྫོང་ཁ། (འབྲུག།)")]
    [EnglishName("Dzongkha (Bhutan)")]
    [CultureInfoName("dz-BT")]
    [Description("Dzongkha (Bhutan)")]
    DZ = 34, // BT 3153

    /// <summary>Embu (Kenya)</summary>
    [NativeName("Kĩembu (Kenya)")]
    [EnglishName("Embu (Kenya)")]
    [CultureInfoName("ebu-KE")]
    [Description("Embu (Kenya)")]
    EBU = 129, // KE 4096

    /// <summary>Ewe (Ghana)</summary>
    [NativeName("Eʋegbe (Ghana nutome)")]
    [EnglishName("Ewe (Ghana)")]
    [CultureInfoName("ee-GH")]
    [Description("Ewe (Ghana)")]
    EE = 89, // GH 4096

    /// <summary>Greek (Cyprus)</summary>
    [NativeName("Ελληνικά (Κύπρος)")]
    [EnglishName("Greek (Cyprus)")]
    [CultureInfoName("el-CY")]
    [Description("Greek (Cyprus)")]
    EL = 59, // CY 4096

    /// <summary>English (World)</summary>
    [NativeName("English (World)")]
    [EnglishName("English (World)")]
    [CultureInfoName("en-001")]
    [Description("English (World)")]
    EN = 39070, // 001 4096

    /// <summary>Esperanto (World)</summary>
    [NativeName("esperanto (Mondo)")]
    [EnglishName("Esperanto (World)")]
    [CultureInfoName("eo-001")]
    [Description("Esperanto (World)")]
    EO = 39070, // 001 4096

    /// <summary>Spanish (Latin America)</summary>
    [NativeName("español (Latinoamérica)")]
    [EnglishName("Spanish (Latin America)")]
    [CultureInfoName("es-419")]
    [Description("Spanish (Latin America)")]
    ES = 161832257, // 419 22538

    /// <summary>Estonian (Estonia)</summary>
    [NativeName("eesti (Eesti)")]
    [EnglishName("Estonian (Estonia)")]
    [CultureInfoName("et-EE")]
    [Description("Estonian (Estonia)")]
    ET = 70, // EE 1061

    /// <summary>Basque (Spain)</summary>
    [NativeName("euskara (Espainia)")]
    [EnglishName("Basque (Spain)")]
    [CultureInfoName("eu-ES")]
    [Description("Basque (Spain)")]
    EU = 217, // ES 1069

    /// <summary>Ewondo (Cameroon)</summary>
    [NativeName("ewondo (Kamərún)")]
    [EnglishName("Ewondo (Cameroon)")]
    [CultureInfoName("ewo-CM")]
    [Description("Ewondo (Cameroon)")]
    EWO = 49, // CM 4096

    /// <summary>Persian (Afghanistan)</summary>
    [NativeName("فارسی (افغانستان)")]
    [EnglishName("Persian (Afghanistan)")]
    [CultureInfoName("fa-AF")]
    [Description("Persian (Afghanistan)")]
    FA = 244, // AF 4096

    /// <summary>Fulah (Latin, Burkina Faso)</summary>
    [NativeName("Fulah (Latin, Burkina Faso)")]
    [EnglishName("Fulah (Latin, Burkina Faso)")]
    [CultureInfoName("ff-Latn-BF")]
    [Description("Fulah (Latin, Burkina Faso)")]
    FF = 244, // BF 4096

    /// <summary>Finnish (Finland)</summary>
    [NativeName("suomi (Suomi)")]
    [EnglishName("Finnish (Finland)")]
    [CultureInfoName("fi-FI")]
    [Description("Finnish (Finland)")]
    FI = 77, // FI 1035

    /// <summary>Filipino (Philippines)</summary>
    [NativeName("Filipino (Pilipinas)")]
    [EnglishName("Filipino (Philippines)")]
    [CultureInfoName("fil-PH")]
    [Description("Filipino (Philippines)")]
    FIL = 201, // PH 1124

    /// <summary>Faroese (Denmark)</summary>
    [NativeName("føroyskt (Danmark)")]
    [EnglishName("Faroese (Denmark)")]
    [CultureInfoName("fo-DK")]
    [Description("Faroese (Denmark)")]
    FO = 61, // DK 4096

    /// <summary>French (Belgium)</summary>
    [NativeName("français (Belgique)")]
    [EnglishName("French (Belgium)")]
    [CultureInfoName("fr-BE")]
    [Description("French (Belgium)")]
    FR = 21, // BE 2060

    /// <summary>Friulian (Italy)</summary>
    [NativeName("furlan (Italie)")]
    [EnglishName("Friulian (Italy)")]
    [CultureInfoName("fur-IT")]
    [Description("Friulian (Italy)")]
    FUR = 118, // IT 4096

    /// <summary>Western Frisian (Netherlands)</summary>
    [NativeName("Frysk (Nederlân)")]
    [EnglishName("Western Frisian (Netherlands)")]
    [CultureInfoName("fy-NL")]
    [Description("Western Frisian (Netherlands)")]
    FY = 176, // NL 1122

    /// <summary>Irish (Ireland)</summary>
    [NativeName("Gaeilge (Éire)")]
    [EnglishName("Irish (Ireland)")]
    [CultureInfoName("ga-IE")]
    [Description("Irish (Ireland)")]
    GA = 68, // IE 2108

    /// <summary>Scottish Gaelic (United Kingdom)</summary>
    [NativeName("Gàidhlig (An Rìoghachd Aonaichte)")]
    [EnglishName("Scottish Gaelic (United Kingdom)")]
    [CultureInfoName("gd-GB")]
    [Description("Scottish Gaelic (United Kingdom)")]
    GD = 242, // GB 1169

    /// <summary>Galician (Spain)</summary>
    [NativeName("galego (España)")]
    [EnglishName("Galician (Spain)")]
    [CultureInfoName("gl-ES")]
    [Description("Galician (Spain)")]
    GL = 217, // ES 1110

    /// <summary>Guarani (Paraguay)</summary>
    [NativeName("Guarani (Paraguay)")]
    [EnglishName("Guarani (Paraguay)")]
    [CultureInfoName("gn-PY")]
    [Description("Guarani (Paraguay)")]
    GN = 185, // PY 1140

    /// <summary>Swiss German (Switzerland)</summary>
    [NativeName("Schwiizertüütsch (Schwiiz)")]
    [EnglishName("Swiss German (Switzerland)")]
    [CultureInfoName("gsw-CH")]
    [Description("Swiss German (Switzerland)")]
    GSW = 223, // CH 4096

    /// <summary>Gujarati (India)</summary>
    [NativeName("ગુજરાતી (ભારત)")]
    [EnglishName("Gujarati (India)")]
    [CultureInfoName("gu-IN")]
    [Description("Gujarati (India)")]
    GU = 113, // IN 1095

    /// <summary>Gusii (Kenya)</summary>
    [NativeName("Ekegusii (Kenya)")]
    [EnglishName("Gusii (Kenya)")]
    [CultureInfoName("guz-KE")]
    [Description("Gusii (Kenya)")]
    GUZ = 129, // KE 4096

    /// <summary>Manx (Isle of Man)</summary>
    [NativeName("Gaelg (Ellan Vannin)")]
    [EnglishName("Manx (Isle of Man)")]
    [CultureInfoName("gv-IM")]
    [Description("Manx (Isle of Man)")]
    GV = 15126, // IM 4096

    /// <summary>Hausa (Ghana)</summary>
    [NativeName("Hausa (Gana)")]
    [EnglishName("Hausa (Ghana)")]
    [CultureInfoName("ha-GH")]
    [Description("Hausa (Ghana)")]
    HA = 244, // GH 4096

    /// <summary>Hawaiian (United States)</summary>
    [NativeName("ʻŌlelo Hawaiʻi (ʻAmelika Hui Pū ʻIa)")]
    [EnglishName("Hawaiian (United States)")]
    [CultureInfoName("haw-US")]
    [Description("Hawaiian (United States)")]
    HAW = 244, // US 1141

    /// <summary>Hebrew (Israel)</summary>
    [NativeName("עברית (ישראל)")]
    [EnglishName("Hebrew (Israel)")]
    [CultureInfoName("he-IL")]
    [Description("Hebrew (Israel)")]
    HE = 117, // IL 1037

    /// <summary>Hindi (India)</summary>
    [NativeName("हिन्दी (भारत)")]
    [EnglishName("Hindi (India)")]
    [CultureInfoName("hi-IN")]
    [Description("Hindi (India)")]
    HI = 113, // IN 1081

    /// <summary>Croatian (Bosnia & Herzegovina)</summary>
    [NativeName("hrvatski (Bosna i Hercegovina)")]
    [EnglishName("Croatian (Bosnia & Herzegovina)")]
    [CultureInfoName("hr-BA")]
    [Description("Croatian (Bosnia & Herzegovina)")]
    HR = 25, // BA 4122

    /// <summary>Upper Sorbian (Germany)</summary>
    [NativeName("hornjoserbšćina (Němska)")]
    [EnglishName("Upper Sorbian (Germany)")]
    [CultureInfoName("hsb-DE")]
    [Description("Upper Sorbian (Germany)")]
    HSB = 94, // DE 1070

    /// <summary>Hungarian (Hungary)</summary>
    [NativeName("magyar (Magyarország)")]
    [EnglishName("Hungarian (Hungary)")]
    [CultureInfoName("hu-HU")]
    [Description("Hungarian (Hungary)")]
    HU = 109, // HU 1038

    /// <summary>Armenian (Armenia)</summary>
    [NativeName("հայերեն (Հայաստան)")]
    [EnglishName("Armenian (Armenia)")]
    [CultureInfoName("hy-AM")]
    [Description("Armenian (Armenia)")]
    HY = 7, // AM 1067

    /// <summary>Interlingua (World)</summary>
    [NativeName("interlingua (Mundo)")]
    [EnglishName("Interlingua (World)")]
    [CultureInfoName("ia-001")]
    [Description("Interlingua (World)")]
    IA = 39070, // 001 4096

    /// <summary>Indonesian (Indonesia)</summary>
    [NativeName("Indonesia (Indonesia)")]
    [EnglishName("Indonesian (Indonesia)")]
    [CultureInfoName("id-ID")]
    [Description("Indonesian (Indonesia)")]
    ID = 111, // ID 1057

    /// <summary>Igbo (Nigeria)</summary>
    [NativeName("Asụsụ Igbo (Naịjịrịa)")]
    [EnglishName("Igbo (Nigeria)")]
    [CultureInfoName("ig-NG")]
    [Description("Igbo (Nigeria)")]
    IG = 175, // NG 1136

    /// <summary>Yi (China)</summary>
    [NativeName("ꆈꌠꉙ (ꍏꇩ)")]
    [EnglishName("Yi (China)")]
    [CultureInfoName("ii-CN")]
    [Description("Yi (China)")]
    II = 45, // CN 1144

    /// <summary>Icelandic (Iceland)</summary>
    [NativeName("íslenska (Ísland)")]
    [EnglishName("Icelandic (Iceland)")]
    [CultureInfoName("is-IS")]
    [Description("Icelandic (Iceland)")]
    IS = 110, // IS 1039

    /// <summary>Italian (Switzerland)</summary>
    [NativeName("italiano (Svizzera)")]
    [EnglishName("Italian (Switzerland)")]
    [CultureInfoName("it-CH")]
    [Description("Italian (Switzerland)")]
    IT = 223, // CH 2064

    /// <summary>Inuktitut (Canada)</summary>
    [NativeName("Inuktitut (Canada)")]
    [EnglishName("Inuktitut (Canada)")]
    [CultureInfoName("iu-CA")]
    [Description("Inuktitut (Canada)")]
    IU = 244, // CA 4096

    /// <summary>Japanese (Japan)</summary>
    [NativeName("日本語 (日本)")]
    [EnglishName("Japanese (Japan)")]
    [CultureInfoName("ja-JP")]
    [Description("Japanese (Japan)")]
    JA = 122, // JP 1041

    /// <summary>Ngomba (Cameroon)</summary>
    [NativeName("Ndaꞌa (Kamɛlûn)")]
    [EnglishName("Ngomba (Cameroon)")]
    [CultureInfoName("jgo-CM")]
    [Description("Ngomba (Cameroon)")]
    JGO = 49, // CM 4096

    /// <summary>Machame (Tanzania)</summary>
    [NativeName("Kimachame (Tanzania)")]
    [EnglishName("Machame (Tanzania)")]
    [CultureInfoName("jmc-TZ")]
    [Description("Machame (Tanzania)")]
    JMC = 239, // TZ 4096

    /// <summary>Javanese (Indonesia)</summary>
    [NativeName("Jawa (Indonésia)")]
    [EnglishName("Javanese (Indonesia)")]
    [CultureInfoName("jv-ID")]
    [Description("Javanese (Indonesia)")]
    JV = 244, // ID 4096

    /// <summary>Georgian (Georgia)</summary>
    [NativeName("ქართული (საქართველო)")]
    [EnglishName("Georgian (Georgia)")]
    [CultureInfoName("ka-GE")]
    [Description("Georgian (Georgia)")]
    KA = 88, // GE 1079

    /// <summary>Kabyle (Algeria)</summary>
    [NativeName("Taqbaylit (Lezzayer)")]
    [EnglishName("Kabyle (Algeria)")]
    [CultureInfoName("kab-DZ")]
    [Description("Kabyle (Algeria)")]
    KAB = 4, // DZ 4096

    /// <summary>Kamba (Kenya)</summary>
    [NativeName("Kikamba (Kenya)")]
    [EnglishName("Kamba (Kenya)")]
    [CultureInfoName("kam-KE")]
    [Description("Kamba (Kenya)")]
    KAM = 129, // KE 4096

    /// <summary>Makonde (Tanzania)</summary>
    [NativeName("Chimakonde (Tanzania)")]
    [EnglishName("Makonde (Tanzania)")]
    [CultureInfoName("kde-TZ")]
    [Description("Makonde (Tanzania)")]
    KDE = 239, // TZ 4096

    /// <summary>Kabuverdianu (Cabo Verde)</summary>
    [NativeName("kabuverdianu (Kabu Verdi)")]
    [EnglishName("Kabuverdianu (Cabo Verde)")]
    [CultureInfoName("kea-CV")]
    [Description("Kabuverdianu (Cabo Verde)")]
    KEA = 57, // CV 4096

    /// <summary>Koyra Chiini (Mali)</summary>
    [NativeName("Koyra ciini (Maali)")]
    [EnglishName("Koyra Chiini (Mali)")]
    [CultureInfoName("khq-ML")]
    [Description("Koyra Chiini (Mali)")]
    KHQ = 157, // ML 4096

    /// <summary>Kikuyu (Kenya)</summary>
    [NativeName("Gikuyu (Kenya)")]
    [EnglishName("Kikuyu (Kenya)")]
    [CultureInfoName("ki-KE")]
    [Description("Kikuyu (Kenya)")]
    KI = 129, // KE 4096

    /// <summary>Kazakh (Kazakhstan)</summary>
    [NativeName("қазақ тілі (Қазақстан)")]
    [EnglishName("Kazakh (Kazakhstan)")]
    [CultureInfoName("kk-KZ")]
    [Description("Kazakh (Kazakhstan)")]
    KK = 137, // KZ 1087

    /// <summary>Kako (Cameroon)</summary>
    [NativeName("kakɔ (Kamɛrun)")]
    [EnglishName("Kako (Cameroon)")]
    [CultureInfoName("kkj-CM")]
    [Description("Kako (Cameroon)")]
    KKJ = 49, // CM 4096

    /// <summary>Kalaallisut (Greenland)</summary>
    [NativeName("kalaallisut (Kalaallit Nunaat)")]
    [EnglishName("Kalaallisut (Greenland)")]
    [CultureInfoName("kl-GL")]
    [Description("Kalaallisut (Greenland)")]
    KL = 93, // GL 1135

    /// <summary>Kalenjin (Kenya)</summary>
    [NativeName("Kalenjin (Emetab Kenya)")]
    [EnglishName("Kalenjin (Kenya)")]
    [CultureInfoName("kln-KE")]
    [Description("Kalenjin (Kenya)")]
    KLN = 129, // KE 4096

    /// <summary>Khmer (Cambodia)</summary>
    [NativeName("ខ្មែរ (កម្ពុជា)")]
    [EnglishName("Khmer (Cambodia)")]
    [CultureInfoName("km-KH")]
    [Description("Khmer (Cambodia)")]
    KM = 40, // KH 1107

    /// <summary>Kannada (India)</summary>
    [NativeName("ಕನ್ನಡ (ಭಾರತ)")]
    [EnglishName("Kannada (India)")]
    [CultureInfoName("kn-IN")]
    [Description("Kannada (India)")]
    KN = 113, // IN 1099

    /// <summary>Korean (North Korea)</summary>
    [NativeName("한국어(조선민주주의인민공화국)")]
    [EnglishName("Korean (North Korea)")]
    [CultureInfoName("ko-KP")]
    [Description("Korean (North Korea)")]
    KO = 131, // KP 4096

    /// <summary>Konkani (India)</summary>
    [NativeName("कोंकणी (भारत)")]
    [EnglishName("Konkani (India)")]
    [CultureInfoName("kok-IN")]
    [Description("Konkani (India)")]
    KOK = 113, // IN 1111

    /// <summary>Kashmiri (India)</summary>
    [NativeName("کٲشُر (ہِندوستان)")]
    [EnglishName("Kashmiri (India)")]
    [CultureInfoName("ks-IN")]
    [Description("Kashmiri (India)")]
    KS = 244, // IN 4096

    /// <summary>Shambala (Tanzania)</summary>
    [NativeName("Kishambaa (Tanzania)")]
    [EnglishName("Shambala (Tanzania)")]
    [CultureInfoName("ksb-TZ")]
    [Description("Shambala (Tanzania)")]
    KSB = 239, // TZ 4096

    /// <summary>Bafia (Cameroon)</summary>
    [NativeName("rikpa (kamɛrún)")]
    [EnglishName("Bafia (Cameroon)")]
    [CultureInfoName("ksf-CM")]
    [Description("Bafia (Cameroon)")]
    KSF = 49, // CM 4096

    /// <summary>Colognian (Germany)</summary>
    [NativeName("Kölsch en Doütschland")]
    [EnglishName("Colognian (Germany)")]
    [CultureInfoName("ksh-DE")]
    [Description("Colognian (Germany)")]
    KSH = 94, // DE 4096

    /// <summary>Cornish (United Kingdom)</summary>
    [NativeName("kernewek (Rywvaneth Unys)")]
    [EnglishName("Cornish (United Kingdom)")]
    [CultureInfoName("kw-GB")]
    [Description("Cornish (United Kingdom)")]
    KW = 242, // GB 4096

    /// <summary>Kyrgyz (Kyrgyzstan)</summary>
    [NativeName("кыргызча (Кыргызстан)")]
    [EnglishName("Kyrgyz (Kyrgyzstan)")]
    [CultureInfoName("ky-KG")]
    [Description("Kyrgyz (Kyrgyzstan)")]
    KY = 130, // KG 1088

    /// <summary>Langi (Tanzania)</summary>
    [NativeName("Kɨlaangi (Taansanía)")]
    [EnglishName("Langi (Tanzania)")]
    [CultureInfoName("lag-TZ")]
    [Description("Langi (Tanzania)")]
    LAG = 239, // TZ 4096

    /// <summary>Luxembourgish (Luxembourg)</summary>
    [NativeName("Lëtzebuergesch (Lëtzebuerg)")]
    [EnglishName("Luxembourgish (Luxembourg)")]
    [CultureInfoName("lb-LU")]
    [Description("Luxembourgish (Luxembourg)")]
    LB = 147, // LU 1134

    /// <summary>Ganda (Uganda)</summary>
    [NativeName("Luganda (Yuganda)")]
    [EnglishName("Ganda (Uganda)")]
    [CultureInfoName("lg-UG")]
    [Description("Ganda (Uganda)")]
    LG = 240, // UG 4096

    /// <summary>Lakota (United States)</summary>
    [NativeName("Lakȟólʼiyapi (Mílahaŋska Tȟamákȟočhe)")]
    [EnglishName("Lakota (United States)")]
    [CultureInfoName("lkt-US")]
    [Description("Lakota (United States)")]
    LKT = 244, // US 4096

    /// <summary>Lingala (Angola)</summary>
    [NativeName("lingála (Angóla)")]
    [EnglishName("Lingala (Angola)")]
    [CultureInfoName("ln-AO")]
    [Description("Lingala (Angola)")]
    LN = 9, // AO 4096

    /// <summary>Lao (Laos)</summary>
    [NativeName("ລາວ (ລາວ)")]
    [EnglishName("Lao (Laos)")]
    [CultureInfoName("lo-LA")]
    [Description("Lao (Laos)")]
    LO = 138, // LA 1108

    /// <summary>Northern Luri (Iraq)</summary>
    [NativeName("Northern Luri (Iraq)")]
    [EnglishName("Northern Luri (Iraq)")]
    [CultureInfoName("lrc-IQ")]
    [Description("Northern Luri (Iraq)")]
    LRC = 121, // IQ 4096

    /// <summary>Lithuanian (Lithuania)</summary>
    [NativeName("lietuvių (Lietuva)")]
    [EnglishName("Lithuanian (Lithuania)")]
    [CultureInfoName("lt-LT")]
    [Description("Lithuanian (Lithuania)")]
    LT = 141, // LT 1063

    /// <summary>Luba-Katanga (Congo [DRC])</summary>
    [NativeName("Tshiluba (Ditunga wa Kongu)")]
    [EnglishName("Luba-Katanga (Congo [DRC])")]
    [CultureInfoName("lu-CD")]
    [Description("Luba-Katanga (Congo [DRC])")]
    LU = 44, // CD 4096

    /// <summary>Luo (Kenya)</summary>
    [NativeName("Dholuo (Kenya)")]
    [EnglishName("Luo (Kenya)")]
    [CultureInfoName("luo-KE")]
    [Description("Luo (Kenya)")]
    LUO = 129, // KE 4096

    /// <summary>Luyia (Kenya)</summary>
    [NativeName("Luluhia (Kenya)")]
    [EnglishName("Luyia (Kenya)")]
    [CultureInfoName("luy-KE")]
    [Description("Luyia (Kenya)")]
    LUY = 129, // KE 4096

    /// <summary>Latvian (Latvia)</summary>
    [NativeName("latviešu (Latvija)")]
    [EnglishName("Latvian (Latvia)")]
    [CultureInfoName("lv-LV")]
    [Description("Latvian (Latvia)")]
    LV = 140, // LV 1062

    /// <summary>Masai (Kenya)</summary>
    [NativeName("Maa (Kenya)")]
    [EnglishName("Masai (Kenya)")]
    [CultureInfoName("mas-KE")]
    [Description("Masai (Kenya)")]
    MAS = 129, // KE 4096

    /// <summary>Meru (Kenya)</summary>
    [NativeName("Kĩmĩrũ (Kenya)")]
    [EnglishName("Meru (Kenya)")]
    [CultureInfoName("mer-KE")]
    [Description("Meru (Kenya)")]
    MER = 129, // KE 4096

    /// <summary>Morisyen (Mauritius)</summary>
    [NativeName("kreol morisien (Moris)")]
    [EnglishName("Morisyen (Mauritius)")]
    [CultureInfoName("mfe-MU")]
    [Description("Morisyen (Mauritius)")]
    MFE = 160, // MU 4096

    /// <summary>Malagasy (Madagascar)</summary>
    [NativeName("Malagasy (Madagasikara)")]
    [EnglishName("Malagasy (Madagascar)")]
    [CultureInfoName("mg-MG")]
    [Description("Malagasy (Madagascar)")]
    MG = 149, // MG 4096

    /// <summary>Makhuwa-Meetto (Mozambique)</summary>
    [NativeName("Makua (Umozambiki)")]
    [EnglishName("Makhuwa-Meetto (Mozambique)")]
    [CultureInfoName("mgh-MZ")]
    [Description("Makhuwa-Meetto (Mozambique)")]
    MGH = 168, // MZ 4096

    /// <summary>Metaʼ (Cameroon)</summary>
    [NativeName("metaʼ (Kamalun)")]
    [EnglishName("Metaʼ (Cameroon)")]
    [CultureInfoName("mgo-CM")]
    [Description("Metaʼ (Cameroon)")]
    MGO = 49, // CM 4096

    /// <summary>Maori (New Zealand)</summary>
    [NativeName("Māori (Aotearoa)")]
    [EnglishName("Maori (New Zealand)")]
    [CultureInfoName("mi-NZ")]
    [Description("Maori (New Zealand)")]
    MI = 183, // NZ 1153

    /// <summary>Macedonian (North Macedonia)</summary>
    [NativeName("македонски (Северна Македонија)")]
    [EnglishName("Macedonian (North Macedonia)")]
    [CultureInfoName("mk-MK")]
    [Description("Macedonian (North Macedonia)")]
    MK = 19618, // MK 1071

    /// <summary>Malayalam (India)</summary>
    [NativeName("മലയാളം (ഇന്ത്യ)")]
    [EnglishName("Malayalam (India)")]
    [CultureInfoName("ml-IN")]
    [Description("Malayalam (India)")]
    ML = 113, // IN 1100

    /// <summary>Mongolian (Mongolia)</summary>
    [NativeName("монгол (Монгол)")]
    [EnglishName("Mongolian (Mongolia)")]
    [CultureInfoName("mn-MN")]
    [Description("Mongolian (Mongolia)")]
    MN = 154, // MN 1104

    /// <summary>Mohawk (Canada)</summary>
    [NativeName("Mohawk (Canada)")]
    [EnglishName("Mohawk (Canada)")]
    [CultureInfoName("moh-CA")]
    [Description("Mohawk (Canada)")]
    MOH = 39, // CA 1148

    /// <summary>Marathi (India)</summary>
    [NativeName("मराठी (भारत)")]
    [EnglishName("Marathi (India)")]
    [CultureInfoName("mr-IN")]
    [Description("Marathi (India)")]
    MR = 113, // IN 1102

    /// <summary>Malay (Brunei)</summary>
    [NativeName("Melayu (Brunei)")]
    [EnglishName("Malay (Brunei)")]
    [CultureInfoName("ms-BN")]
    [Description("Malay (Brunei)")]
    MS = 37, // BN 2110

    /// <summary>Maltese (Malta)</summary>
    [NativeName("Malti (Malta)")]
    [EnglishName("Maltese (Malta)")]
    [CultureInfoName("mt-MT")]
    [Description("Maltese (Malta)")]
    MT = 163, // MT 1082

    /// <summary>Mundang (Cameroon)</summary>
    [NativeName("MUNDAŊ (kameruŋ)")]
    [EnglishName("Mundang (Cameroon)")]
    [CultureInfoName("mua-CM")]
    [Description("Mundang (Cameroon)")]
    MUA = 49, // CM 4096

    /// <summary>Burmese (Myanmar)</summary>
    [NativeName("မြန်မာ (မြန်မာ)")]
    [EnglishName("Burmese (Myanmar)")]
    [CultureInfoName("my-MM")]
    [Description("Burmese (Myanmar)")]
    MY = 27, // MM 1109

    /// <summary>Mazanderani (Iran)</summary>
    [NativeName("مازرونی (ایران)")]
    [EnglishName("Mazanderani (Iran)")]
    [CultureInfoName("mzn-IR")]
    [Description("Mazanderani (Iran)")]
    MZN = 116, // IR 4096

    /// <summary>Nama (Namibia)</summary>
    [NativeName("Khoekhoegowab (Namibiab)")]
    [EnglishName("Nama (Namibia)")]
    [CultureInfoName("naq-NA")]
    [Description("Nama (Namibia)")]
    NAQ = 254, // NA 4096

    /// <summary>Norwegian Bokmål (Norway)</summary>
    [NativeName("norsk bokmål (Norge)")]
    [EnglishName("Norwegian Bokmål (Norway)")]
    [CultureInfoName("nb-NO")]
    [Description("Norwegian Bokmål (Norway)")]
    NB = 177, // NO 1044

    /// <summary>North Ndebele (Zimbabwe)</summary>
    [NativeName("isiNdebele (Zimbabwe)")]
    [EnglishName("North Ndebele (Zimbabwe)")]
    [CultureInfoName("nd-ZW")]
    [Description("North Ndebele (Zimbabwe)")]
    ND = 264, // ZW 4096

    /// <summary>Low German (Germany)</summary>
    [NativeName("Low German (Germany)")]
    [EnglishName("Low German (Germany)")]
    [CultureInfoName("nds-DE")]
    [Description("Low German (Germany)")]
    NDS = 94, // DE 4096

    /// <summary>Nepali (India)</summary>
    [NativeName("नेपाली (भारत)")]
    [EnglishName("Nepali (India)")]
    [CultureInfoName("ne-IN")]
    [Description("Nepali (India)")]
    NE = 113, // IN 2145

    /// <summary>Dutch (Aruba)</summary>
    [NativeName("Nederlands (Aruba)")]
    [EnglishName("Dutch (Aruba)")]
    [CultureInfoName("nl-AW")]
    [Description("Dutch (Aruba)")]
    NL = 302, // AW 4096

    /// <summary>Kwasio (Cameroon)</summary>
    [NativeName("Kwasio (Cameroon)")]
    [EnglishName("Kwasio (Cameroon)")]
    [CultureInfoName("nmg-CM")]
    [Description("Kwasio (Cameroon)")]
    NMG = 49, // CM 4096

    /// <summary>Norwegian Nynorsk (Norway)</summary>
    [NativeName("nynorsk (Noreg)")]
    [EnglishName("Norwegian Nynorsk (Norway)")]
    [CultureInfoName("nn-NO")]
    [Description("Norwegian Nynorsk (Norway)")]
    NN = 177, // NO 2068

    /// <summary>Ngiemboon (Cameroon)</summary>
    [NativeName("Shwóŋò ngiembɔɔn (Kàmalûm)")]
    [EnglishName("Ngiemboon (Cameroon)")]
    [CultureInfoName("nnh-CM")]
    [Description("Ngiemboon (Cameroon)")]
    NNH = 49, // CM 4096

    /// <summary>N’Ko (Guinea)</summary>
    [NativeName("N’Ko (Guinea)")]
    [EnglishName("N’Ko (Guinea)")]
    [CultureInfoName("nqo-GN")]
    [Description("N’Ko (Guinea)")]
    NQO = 100, // GN 4096

    /// <summary>South Ndebele (South Africa)</summary>
    [NativeName("South Ndebele (South Africa)")]
    [EnglishName("South Ndebele (South Africa)")]
    [CultureInfoName("nr-ZA")]
    [Description("South Ndebele (South Africa)")]
    NR = 209, // ZA 4096

    /// <summary>Sesotho sa Leboa (South Africa)</summary>
    [NativeName("Sesotho sa Leboa (South Africa)")]
    [EnglishName("Sesotho sa Leboa (South Africa)")]
    [CultureInfoName("nso-ZA")]
    [Description("Sesotho sa Leboa (South Africa)")]
    NSO = 209, // ZA 1132

    /// <summary>Nuer (South Sudan)</summary>
    [NativeName("Nuer (South Sudan)")]
    [EnglishName("Nuer (South Sudan)")]
    [CultureInfoName("nus-SS")]
    [Description("Nuer (South Sudan)")]
    NUS = 276, // SS 4096

    /// <summary>Nyankole (Uganda)</summary>
    [NativeName("Runyankore (Uganda)")]
    [EnglishName("Nyankole (Uganda)")]
    [CultureInfoName("nyn-UG")]
    [Description("Nyankole (Uganda)")]
    NYN = 240, // UG 4096

    /// <summary>Occitan (France)</summary>
    [NativeName("Occitan (France)")]
    [EnglishName("Occitan (France)")]
    [CultureInfoName("oc-FR")]
    [Description("Occitan (France)")]
    OC = 84, // FR 1154

    /// <summary>Oromo (Ethiopia)</summary>
    [NativeName("Oromoo (Itoophiyaa)")]
    [EnglishName("Oromo (Ethiopia)")]
    [CultureInfoName("om-ET")]
    [Description("Oromo (Ethiopia)")]
    OM = 73, // ET 1138

    /// <summary>Odia (India)</summary>
    [NativeName("ଓଡ଼ିଆ (ଭାରତ)")]
    [EnglishName("Odia (India)")]
    [CultureInfoName("or-IN")]
    [Description("Odia (India)")]
    OR = 113, // IN 1096

    /// <summary>Ossetic (Georgia)</summary>
    [NativeName("ирон (Гуырдзыстон)")]
    [EnglishName("Ossetic (Georgia)")]
    [CultureInfoName("os-GE")]
    [Description("Ossetic (Georgia)")]
    OS = 88, // GE 4096

    /// <summary>Punjabi (Arabic, Pakistan)</summary>
    [NativeName("پنجابی (عربی, پاکستان)")]
    [EnglishName("Punjabi (Arabic, Pakistan)")]
    [CultureInfoName("pa-Arab-PK")]
    [Description("Punjabi (Arabic, Pakistan)")]
    PA = 190, // PK 2118

    /// <summary>Polish (Poland)</summary>
    [NativeName("polski (Polska)")]
    [EnglishName("Polish (Poland)")]
    [CultureInfoName("pl-PL")]
    [Description("Polish (Poland)")]
    PL = 191, // PL 1045

    /// <summary>Prussian (World)</summary>
    [NativeName("Prussian (World)")]
    [EnglishName("Prussian (World)")]
    [CultureInfoName("prg-001")]
    [Description("Prussian (World)")]
    PRG = 39070, // 001 4096

    /// <summary>Pashto (Afghanistan)</summary>
    [NativeName("پښتو (افغانستان)")]
    [EnglishName("Pashto (Afghanistan)")]
    [CultureInfoName("ps-AF")]
    [Description("Pashto (Afghanistan)")]
    PS = 3, // AF 1123

    /// <summary>Portuguese (Angola)</summary>
    [NativeName("português (Angola)")]
    [EnglishName("Portuguese (Angola)")]
    [CultureInfoName("pt-AO")]
    [Description("Portuguese (Angola)")]
    PT = 9, // AO 4096

    /// <summary>Quechua (Bolivia)</summary>
    [NativeName("Runasimi (Bolivia)")]
    [EnglishName("Quechua (Bolivia)")]
    [CultureInfoName("qu-BO")]
    [Description("Quechua (Bolivia)")]
    QU = 244, // BO 4096

    /// <summary>Kʼicheʼ (Guatemala)</summary>
    [NativeName("Kʼicheʼ (Guatemala)")]
    [EnglishName("Kʼicheʼ (Guatemala)")]
    [CultureInfoName("quc-GT")]
    [Description("Kʼicheʼ (Guatemala)")]
    QUC = 244, // GT 4096

    /// <summary>Romansh (Switzerland)</summary>
    [NativeName("rumantsch (Svizra)")]
    [EnglishName("Romansh (Switzerland)")]
    [CultureInfoName("rm-CH")]
    [Description("Romansh (Switzerland)")]
    RM = 223, // CH 1047

    /// <summary>Rundi (Burundi)</summary>
    [NativeName("Ikirundi (Uburundi)")]
    [EnglishName("Rundi (Burundi)")]
    [CultureInfoName("rn-BI")]
    [Description("Rundi (Burundi)")]
    RN = 38, // BI 4096

    /// <summary>Romanian (Moldova)</summary>
    [NativeName("română (Republica Moldova)")]
    [EnglishName("Romanian (Moldova)")]
    [CultureInfoName("ro-MD")]
    [Description("Romanian (Moldova)")]
    RO = 152, // MD 2072

    /// <summary>Rombo (Tanzania)</summary>
    [NativeName("Kihorombo (Tanzania)")]
    [EnglishName("Rombo (Tanzania)")]
    [CultureInfoName("rof-TZ")]
    [Description("Rombo (Tanzania)")]
    ROF = 239, // TZ 4096

    /// <summary>Russian (Belarus)</summary>
    [NativeName("русский (Беларусь)")]
    [EnglishName("Russian (Belarus)")]
    [CultureInfoName("ru-BY")]
    [Description("Russian (Belarus)")]
    RU = 29, // BY 4096

    /// <summary>Kinyarwanda (Rwanda)</summary>
    [NativeName("Kinyarwanda (U Rwanda)")]
    [EnglishName("Kinyarwanda (Rwanda)")]
    [CultureInfoName("rw-RW")]
    [Description("Kinyarwanda (Rwanda)")]
    RW = 204, // RW 1159

    /// <summary>Rwa (Tanzania)</summary>
    [NativeName("Kiruwa (Tanzania)")]
    [EnglishName("Rwa (Tanzania)")]
    [CultureInfoName("rwk-TZ")]
    [Description("Rwa (Tanzania)")]
    RWK = 239, // TZ 4096

    /// <summary>Sanskrit (India)</summary>
    [NativeName("Sanskrit (India)")]
    [EnglishName("Sanskrit (India)")]
    [CultureInfoName("sa-IN")]
    [Description("Sanskrit (India)")]
    SA = 113, // IN 1103

    /// <summary>Sakha (Russia)</summary>
    [NativeName("саха тыла (Арассыыйа)")]
    [EnglishName("Sakha (Russia)")]
    [CultureInfoName("sah-RU")]
    [Description("Sakha (Russia)")]
    SAH = 203, // RU 1157

    /// <summary>Samburu (Kenya)</summary>
    [NativeName("Kisampur (Kenya)")]
    [EnglishName("Samburu (Kenya)")]
    [CultureInfoName("saq-KE")]
    [Description("Samburu (Kenya)")]
    SAQ = 129, // KE 4096

    /// <summary>Sangu (Tanzania)</summary>
    [NativeName("Ishisangu (Tansaniya)")]
    [EnglishName("Sangu (Tanzania)")]
    [CultureInfoName("sbp-TZ")]
    [Description("Sangu (Tanzania)")]
    SBP = 239, // TZ 4096

    /// <summary>Sindhi (Pakistan)</summary>
    [NativeName("سنڌي (پاڪستان)")]
    [EnglishName("Sindhi (Pakistan)")]
    [CultureInfoName("sd-PK")]
    [Description("Sindhi (Pakistan)")]
    SD = 244, // PK 4096

    /// <summary>Northern Sami (Finland)</summary>
    [NativeName("davvisámegiella (Suopma)")]
    [EnglishName("Northern Sami (Finland)")]
    [CultureInfoName("se-FI")]
    [Description("Northern Sami (Finland)")]
    SE = 77, // FI 3131

    /// <summary>Sena (Mozambique)</summary>
    [NativeName("sena (Moçambique)")]
    [EnglishName("Sena (Mozambique)")]
    [CultureInfoName("seh-MZ")]
    [Description("Sena (Mozambique)")]
    SEH = 168, // MZ 4096

    /// <summary>Koyraboro Senni (Mali)</summary>
    [NativeName("Koyraboro senni (Maali)")]
    [EnglishName("Koyraboro Senni (Mali)")]
    [CultureInfoName("ses-ML")]
    [Description("Koyraboro Senni (Mali)")]
    SES = 157, // ML 4096

    /// <summary>Sango (Central African Republic)</summary>
    [NativeName("Sängö (Ködörösêse tî Bêafrîka)")]
    [EnglishName("Sango (Central African Republic)")]
    [CultureInfoName("sg-CF")]
    [Description("Sango (Central African Republic)")]
    SG = 55, // CF 4096

    /// <summary>Tachelhit (Latin, Morocco)</summary>
    [NativeName("Tachelhit (Latin, Morocco)")]
    [EnglishName("Tachelhit (Latin, Morocco)")]
    [CultureInfoName("shi-Latn-MA")]
    [Description("Tachelhit (Latin, Morocco)")]
    SHI = 159, // MA 4096

    /// <summary>Sinhala (Sri Lanka)</summary>
    [NativeName("සිංහල (ශ්‍රී ලංකාව)")]
    [EnglishName("Sinhala (Sri Lanka)")]
    [CultureInfoName("si-LK")]
    [Description("Sinhala (Sri Lanka)")]
    SI = 42, // LK 1115

    /// <summary>Slovak (Slovakia)</summary>
    [NativeName("slovenčina (Slovensko)")]
    [EnglishName("Slovak (Slovakia)")]
    [CultureInfoName("sk-SK")]
    [Description("Slovak (Slovakia)")]
    SK = 143, // SK 1051

    /// <summary>Slovenian (Slovenia)</summary>
    [NativeName("slovenščina (Slovenija)")]
    [EnglishName("Slovenian (Slovenia)")]
    [CultureInfoName("sl-SI")]
    [Description("Slovenian (Slovenia)")]
    SL = 212, // SI 1060

    /// <summary>Southern Sami (Norway)</summary>
    [NativeName("Southern Sami (Norway)")]
    [EnglishName("Southern Sami (Norway)")]
    [CultureInfoName("sma-NO")]
    [Description("Southern Sami (Norway)")]
    SMA = 177, // NO 6203

    /// <summary>Lule Sami (Norway)</summary>
    [NativeName("Lule Sami (Norway)")]
    [EnglishName("Lule Sami (Norway)")]
    [CultureInfoName("smj-NO")]
    [Description("Lule Sami (Norway)")]
    SMJ = 177, // NO 4155

    /// <summary>Inari Sami (Finland)</summary>
    [NativeName("anarâškielâ (Suomâ)")]
    [EnglishName("Inari Sami (Finland)")]
    [CultureInfoName("smn-FI")]
    [Description("Inari Sami (Finland)")]
    SMN = 77, // FI 9275

    /// <summary>Skolt Sami (Finland)</summary>
    [NativeName("Skolt Sami (Finland)")]
    [EnglishName("Skolt Sami (Finland)")]
    [CultureInfoName("sms-FI")]
    [Description("Skolt Sami (Finland)")]
    SMS = 77, // FI 8251

    /// <summary>Shona (Zimbabwe)</summary>
    [NativeName("chiShona (Zimbabwe)")]
    [EnglishName("Shona (Zimbabwe)")]
    [CultureInfoName("sn-ZW")]
    [Description("Shona (Zimbabwe)")]
    SN = 244, // ZW 4096

    /// <summary>Somali (Djibouti)</summary>
    [NativeName("Soomaali (Jabuuti)")]
    [EnglishName("Somali (Djibouti)")]
    [CultureInfoName("so-DJ")]
    [Description("Somali (Djibouti)")]
    SO = 62, // DJ 4096

    /// <summary>Albanian (Albania)</summary>
    [NativeName("shqip (Shqipëri)")]
    [EnglishName("Albanian (Albania)")]
    [CultureInfoName("sq-AL")]
    [Description("Albanian (Albania)")]
    SQ = 6, // AL 1052

    /// <summary>Serbian (Cyrillic, Bosnia & Herzegovina)</summary>
    [NativeName("српски (ћирилица, Босна и Херцеговина)")]
    [EnglishName("Serbian (Cyrillic, Bosnia & Herzegovina)")]
    [CultureInfoName("sr-Cyrl-BA")]
    [Description("Serbian (Cyrillic, Bosnia & Herzegovina)")]
    SR = 25, // BA 7194

    /// <summary>siSwati (Eswatini)</summary>
    [NativeName("siSwati (eSwatini)")]
    [EnglishName("siSwati (Eswatini)")]
    [CultureInfoName("ss-SZ")]
    [Description("siSwati (Eswatini)")]
    SS = 260, // SZ 4096

    /// <summary>Saho (Eritrea)</summary>
    [NativeName("Saho (Eritrea)")]
    [EnglishName("Saho (Eritrea)")]
    [CultureInfoName("ssy-ER")]
    [Description("Saho (Eritrea)")]
    SSY = 71, // ER 4096

    /// <summary>Sesotho (Lesotho)</summary>
    [NativeName("Sesotho (Lesotho)")]
    [EnglishName("Sesotho (Lesotho)")]
    [CultureInfoName("st-LS")]
    [Description("Sesotho (Lesotho)")]
    ST = 146, // LS 4096

    /// <summary>Swedish (Åland Islands)</summary>
    [NativeName("svenska (Åland)")]
    [EnglishName("Swedish (Åland Islands)")]
    [CultureInfoName("sv-AX")]
    [Description("Swedish (Åland Islands)")]
    SV = 10028789, // AX 4096

    /// <summary>Kiswahili (Congo [DRC])</summary>
    [NativeName("Kiswahili (Jamhuri ya Kidemokrasia ya Kongo)")]
    [EnglishName("Kiswahili (Congo [DRC])")]
    [CultureInfoName("sw-CD")]
    [Description("Kiswahili (Congo [DRC])")]
    SW = 44, // CD 4096

    /// <summary>Syriac (Syria)</summary>
    [NativeName("Syriac (Syria)")]
    [EnglishName("Syriac (Syria)")]
    [CultureInfoName("syr-SY")]
    [Description("Syriac (Syria)")]
    SYR = 222, // SY 1114

    /// <summary>Tamil (India)</summary>
    [NativeName("தமிழ் (இந்தியா)")]
    [EnglishName("Tamil (India)")]
    [CultureInfoName("ta-IN")]
    [Description("Tamil (India)")]
    TA = 113, // IN 1097

    /// <summary>Telugu (India)</summary>
    [NativeName("తెలుగు (భారతదేశం)")]
    [EnglishName("Telugu (India)")]
    [CultureInfoName("te-IN")]
    [Description("Telugu (India)")]
    TE = 113, // IN 1098

    /// <summary>Teso (Kenya)</summary>
    [NativeName("Kiteso (Kenia)")]
    [EnglishName("Teso (Kenya)")]
    [CultureInfoName("teo-KE")]
    [Description("Teso (Kenya)")]
    TEO = 129, // KE 4096

    /// <summary>Tajik (Tajikistan)</summary>
    [NativeName("тоҷикӣ (Тоҷикистон)")]
    [EnglishName("Tajik (Tajikistan)")]
    [CultureInfoName("tg-TJ")]
    [Description("Tajik (Tajikistan)")]
    TG = 244, // TJ 4096

    /// <summary>Thai (Thailand)</summary>
    [NativeName("ไทย (ไทย)")]
    [EnglishName("Thai (Thailand)")]
    [CultureInfoName("th-TH")]
    [Description("Thai (Thailand)")]
    TH = 227, // TH 1054

    /// <summary>Tigrinya (Eritrea)</summary>
    [NativeName("ትግርኛ (ኤርትራ)")]
    [EnglishName("Tigrinya (Eritrea)")]
    [CultureInfoName("ti-ER")]
    [Description("Tigrinya (Eritrea)")]
    TI = 71, // ER 2163

    /// <summary>Tigre (Eritrea)</summary>
    [NativeName("Tigre (Eritrea)")]
    [EnglishName("Tigre (Eritrea)")]
    [CultureInfoName("tig-ER")]
    [Description("Tigre (Eritrea)")]
    TIG = 71, // ER 4096

    /// <summary>Turkmen (Turkmenistan)</summary>
    [NativeName("türkmen dili (Türkmenistan)")]
    [EnglishName("Turkmen (Turkmenistan)")]
    [CultureInfoName("tk-TM")]
    [Description("Turkmen (Turkmenistan)")]
    TK = 238, // TM 1090

    /// <summary>Setswana (Botswana)</summary>
    [NativeName("Setswana (Botswana)")]
    [EnglishName("Setswana (Botswana)")]
    [CultureInfoName("tn-BW")]
    [Description("Setswana (Botswana)")]
    TN = 19, // BW 2098

    /// <summary>Tongan (Tonga)</summary>
    [NativeName("lea fakatonga (Tonga)")]
    [EnglishName("Tongan (Tonga)")]
    [CultureInfoName("to-TO")]
    [Description("Tongan (Tonga)")]
    TO = 231, // TO 4096

    /// <summary>Turkish (Cyprus)</summary>
    [NativeName("Türkçe (Kıbrıs)")]
    [EnglishName("Turkish (Cyprus)")]
    [CultureInfoName("tr-CY")]
    [Description("Turkish (Cyprus)")]
    TR = 59, // CY 4096

    /// <summary>Xitsonga (South Africa)</summary>
    [NativeName("Xitsonga (South Africa)")]
    [EnglishName("Xitsonga (South Africa)")]
    [CultureInfoName("ts-ZA")]
    [Description("Xitsonga (South Africa)")]
    TS = 209, // ZA 1073

    /// <summary>Tatar (Russia)</summary>
    [NativeName("татар (Россия)")]
    [EnglishName("Tatar (Russia)")]
    [CultureInfoName("tt-RU")]
    [Description("Tatar (Russia)")]
    TT = 203, // RU 1092

    /// <summary>Tasawaq (Niger)</summary>
    [NativeName("Tasawaq senni (Nižer)")]
    [EnglishName("Tasawaq (Niger)")]
    [CultureInfoName("twq-NE")]
    [Description("Tasawaq (Niger)")]
    TWQ = 173, // NE 4096

    /// <summary>Central Atlas Tamazight (Morocco)</summary>
    [NativeName("Tamaziɣt n laṭlaṣ (Meṛṛuk)")]
    [EnglishName("Central Atlas Tamazight (Morocco)")]
    [CultureInfoName("tzm-MA")]
    [Description("Central Atlas Tamazight (Morocco)")]
    TZM = 244, // MA 4096

    /// <summary>Uyghur (China)</summary>
    [NativeName("ئۇيغۇرچە (جۇڭگو)")]
    [EnglishName("Uyghur (China)")]
    [CultureInfoName("ug-CN")]
    [Description("Uyghur (China)")]
    UG = 45, // CN 1152

    /// <summary>Ukrainian (Ukraine)</summary>
    [NativeName("українська (Україна)")]
    [EnglishName("Ukrainian (Ukraine)")]
    [CultureInfoName("uk-UA")]
    [Description("Ukrainian (Ukraine)")]
    UK = 241, // UA 1058

    /// <summary>Urdu (India)</summary>
    [NativeName("اردو (بھارت)")]
    [EnglishName("Urdu (India)")]
    [CultureInfoName("ur-IN")]
    [Description("Urdu (India)")]
    UR = 113, // IN 2080

    /// <summary>Uzbek (Arabic, Afghanistan)</summary>
    [NativeName("اوزبیک (عربی, افغانستان)")]
    [EnglishName("Uzbek (Arabic, Afghanistan)")]
    [CultureInfoName("uz-Arab-AF")]
    [Description("Uzbek (Arabic, Afghanistan)")]
    UZ = 3, // AF 4096

    /// <summary>Vai (Latin, Liberia)</summary>
    [NativeName("Vai (Latin, Liberia)")]
    [EnglishName("Vai (Latin, Liberia)")]
    [CultureInfoName("vai-Latn-LR")]
    [Description("Vai (Latin, Liberia)")]
    VAI = 142, // LR 4096

    /// <summary>Venda (South Africa)</summary>
    [NativeName("Venda (South Africa)")]
    [EnglishName("Venda (South Africa)")]
    [CultureInfoName("ve-ZA")]
    [Description("Venda (South Africa)")]
    VE = 209, // ZA 1075

    /// <summary>Vietnamese (Vietnam)</summary>
    [NativeName("Tiếng Việt (Việt Nam)")]
    [EnglishName("Vietnamese (Vietnam)")]
    [CultureInfoName("vi-VN")]
    [Description("Vietnamese (Vietnam)")]
    VI = 251, // VN 1066

    /// <summary>Volapük (World)</summary>
    [NativeName("Volapük (World)")]
    [EnglishName("Volapük (World)")]
    [CultureInfoName("vo-001")]
    [Description("Volapük (World)")]
    VO = 39070, // 001 4096

    /// <summary>Vunjo (Tanzania)</summary>
    [NativeName("Kyivunjo (Tanzania)")]
    [EnglishName("Vunjo (Tanzania)")]
    [CultureInfoName("vun-TZ")]
    [Description("Vunjo (Tanzania)")]
    VUN = 239, // TZ 4096

    /// <summary>Walser (Switzerland)</summary>
    [NativeName("Walser (Schwiz)")]
    [EnglishName("Walser (Switzerland)")]
    [CultureInfoName("wae-CH")]
    [Description("Walser (Switzerland)")]
    WAE = 223, // CH 4096

    /// <summary>Wolaytta (Ethiopia)</summary>
    [NativeName("Wolaytta (Ethiopia)")]
    [EnglishName("Wolaytta (Ethiopia)")]
    [CultureInfoName("wal-ET")]
    [Description("Wolaytta (Ethiopia)")]
    WAL = 73, // ET 4096

    /// <summary>Wolof (Senegal)</summary>
    [NativeName("Wolof (Senegaal)")]
    [EnglishName("Wolof (Senegal)")]
    [CultureInfoName("wo-SN")]
    [Description("Wolof (Senegal)")]
    WO = 210, // SN 1160

    /// <summary>isiXhosa (South Africa)</summary>
    [NativeName("isiXhosa (eMzantsi Afrika)")]
    [EnglishName("isiXhosa (South Africa)")]
    [CultureInfoName("xh-ZA")]
    [Description("isiXhosa (South Africa)")]
    XH = 209, // ZA 1076

    /// <summary>Soga (Uganda)</summary>
    [NativeName("Olusoga (Yuganda)")]
    [EnglishName("Soga (Uganda)")]
    [CultureInfoName("xog-UG")]
    [Description("Soga (Uganda)")]
    XOG = 240, // UG 4096

    /// <summary>Yangben (Cameroon)</summary>
    [NativeName("nuasue (Kemelún)")]
    [EnglishName("Yangben (Cameroon)")]
    [CultureInfoName("yav-CM")]
    [Description("Yangben (Cameroon)")]
    YAV = 49, // CM 4096

    /// <summary>Yiddish (World)</summary>
    [NativeName("ייִדיש (וועלט)")]
    [EnglishName("Yiddish (World)")]
    [CultureInfoName("yi-001")]
    [Description("Yiddish (World)")]
    YI = 39070, // 001 1085

    /// <summary>Yoruba (Benin)</summary>
    [NativeName("Èdè Yorùbá (Orílɛ́ède Bɛ̀nɛ̀)")]
    [EnglishName("Yoruba (Benin)")]
    [CultureInfoName("yo-BJ")]
    [Description("Yoruba (Benin)")]
    YO = 28, // BJ 4096

    /// <summary>Standard Moroccan Tamazight (Morocco)</summary>
    [NativeName("ⵜⴰⵎⴰⵣⵉⵖⵜ (ⵍⵎⵖⵔⵉⴱ)")]
    [EnglishName("Standard Moroccan Tamazight (Morocco)")]
    [CultureInfoName("zgh-MA")]
    [Description("Standard Moroccan Tamazight (Morocco)")]
    ZGH = 244, // MA 4096

    /// <summary>Chinese (Simplified, China)</summary>
    [NativeName("中文（简体，中国）")]
    [EnglishName("Chinese (Simplified, China)")]
    [CultureInfoName("zh-Hans-CN")]
    [Description("Chinese (Simplified, China)")]
    ZH = 244, // CN 4096

    /// <summary>isiZulu (South Africa)</summary>
    [NativeName("isiZulu (iNingizimu Afrika)")]
    [EnglishName("isiZulu (South Africa)")]
    [CultureInfoName("zu-ZA")]
    [Description("isiZulu (South Africa)")]
    ZU = 209, // ZA 1077

    /// <summary>Afar (Djibouti)</summary>
    [NativeName("Afar (Djibouti)")]
    [EnglishName("Afar (Djibouti)")]
    [CultureInfoName("aa-DJ")]
    [Description("Afar (Djibouti)")]
    AA = 62, // DJ 4096

}

