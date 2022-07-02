<Query Kind="Program">
  <Namespace>System.ComponentModel</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

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

///<summary>Contains a list of Two-letter culture codes stored in .net BCL. Enum integer value is based on universal Microsofts LCID a standardized numbersequence from RFC</summary>
public enum CultureCode
{
    /// <summary>Afrikaans (Namibia)</summary>
    [NativeName("Afrikaans (Namibië)")]
    [EnglishName("Afrikaans (Namibia)")]
    [CultureInfoName("af-NA")]
    [Description("Afrikaans (Namibia)")]
    AF = 4096, // NA 4096

    /// <summary>Aghem (Cameroon)</summary>
    [NativeName("Aghem (Kàmàlûŋ)")]
    [EnglishName("Aghem (Cameroon)")]
    [CultureInfoName("agq-CM")]
    [Description("Aghem (Cameroon)")]
    AGQ = 4096, // CM 4096

    /// <summary>Akan (Ghana)</summary>
    [NativeName("Akan (Gaana)")]
    [EnglishName("Akan (Ghana)")]
    [CultureInfoName("ak-GH")]
    [Description("Akan (Ghana)")]
    AK = 4096, // GH 4096

    /// <summary>Amharic (Ethiopia)</summary>
    [NativeName("አማርኛ (ኢትዮጵያ)")]
    [EnglishName("Amharic (Ethiopia)")]
    [CultureInfoName("am-ET")]
    [Description("Amharic (Ethiopia)")]
    AM = 1118, // ET 1118

    /// <summary>Arabic (World)</summary>
    [NativeName("العربية (العالم)")]
    [EnglishName("Arabic (World)")]
    [CultureInfoName("ar-001")]
    [Description("Arabic (World)")]
    AR = 4096, // 001 4096

    /// <summary>Mapuche (Chile)</summary>
    [NativeName("Mapuche (Chile)")]
    [EnglishName("Mapuche (Chile)")]
    [CultureInfoName("arn-CL")]
    [Description("Mapuche (Chile)")]
    ARN = 1146, // CL 1146

    /// <summary>Assamese (India)</summary>
    [NativeName("অসমীয়া (ভাৰত)")]
    [EnglishName("Assamese (India)")]
    [CultureInfoName("as-IN")]
    [Description("Assamese (India)")]
    AS = 1101, // IN 1101

    /// <summary>Asu (Tanzania)</summary>
    [NativeName("Kipare (Tadhania)")]
    [EnglishName("Asu (Tanzania)")]
    [CultureInfoName("asa-TZ")]
    [Description("Asu (Tanzania)")]
    ASA = 4096, // TZ 4096

    /// <summary>Asturian (Spain)</summary>
    [NativeName("asturianu (España)")]
    [EnglishName("Asturian (Spain)")]
    [CultureInfoName("ast-ES")]
    [Description("Asturian (Spain)")]
    AST = 4096, // ES 4096

    /// <summary>Azerbaijani (Cyrillic, Azerbaijan)</summary>
    [NativeName("азәрбајҹан (Кирил, Азәрбајҹан)")]
    [EnglishName("Azerbaijani (Cyrillic, Azerbaijan)")]
    [CultureInfoName("az-Cyrl-AZ")]
    [Description("Azerbaijani (Cyrillic, Azerbaijan)")]
    AZ = 2092, // AZ 2092

    /// <summary>Bashkir (Russia)</summary>
    [NativeName("Bashkir (Russia)")]
    [EnglishName("Bashkir (Russia)")]
    [CultureInfoName("ba-RU")]
    [Description("Bashkir (Russia)")]
    BA = 1133, // RU 1133

    /// <summary>Basaa (Cameroon)</summary>
    [NativeName("Ɓàsàa (Kàmɛ̀rûn)")]
    [EnglishName("Basaa (Cameroon)")]
    [CultureInfoName("bas-CM")]
    [Description("Basaa (Cameroon)")]
    BAS = 4096, // CM 4096

    /// <summary>Belarusian (Belarus)</summary>
    [NativeName("беларуская (Беларусь)")]
    [EnglishName("Belarusian (Belarus)")]
    [CultureInfoName("be-BY")]
    [Description("Belarusian (Belarus)")]
    BE = 1059, // BY 1059

    /// <summary>Bemba (Zambia)</summary>
    [NativeName("Ichibemba (Zambia)")]
    [EnglishName("Bemba (Zambia)")]
    [CultureInfoName("bem-ZM")]
    [Description("Bemba (Zambia)")]
    BEM = 4096, // ZM 4096

    /// <summary>Bena (Tanzania)</summary>
    [NativeName("Hibena (Hutanzania)")]
    [EnglishName("Bena (Tanzania)")]
    [CultureInfoName("bez-TZ")]
    [Description("Bena (Tanzania)")]
    BEZ = 4096, // TZ 4096

    /// <summary>Bulgarian (Bulgaria)</summary>
    [NativeName("български (България)")]
    [EnglishName("Bulgarian (Bulgaria)")]
    [CultureInfoName("bg-BG")]
    [Description("Bulgarian (Bulgaria)")]
    BG = 1026, // BG 1026

    /// <summary>Bamanankan (Mali)</summary>
    [NativeName("bamanakan (Mali)")]
    [EnglishName("Bamanankan (Mali)")]
    [CultureInfoName("bm-ML")]
    [Description("Bamanankan (Mali)")]
    BM = 4096, // ML 4096

    /// <summary>Bangla (Bangladesh)</summary>
    [NativeName("বাংলা (বাংলাদেশ)")]
    [EnglishName("Bangla (Bangladesh)")]
    [CultureInfoName("bn-BD")]
    [Description("Bangla (Bangladesh)")]
    BN = 2117, // BD 2117

    /// <summary>Tibetan (China)</summary>
    [NativeName("བོད་སྐད་ (རྒྱ་ནག)")]
    [EnglishName("Tibetan (China)")]
    [CultureInfoName("bo-CN")]
    [Description("Tibetan (China)")]
    BO = 1105, // CN 1105

    /// <summary>Breton (France)</summary>
    [NativeName("brezhoneg (Frañs)")]
    [EnglishName("Breton (France)")]
    [CultureInfoName("br-FR")]
    [Description("Breton (France)")]
    BR = 1150, // FR 1150

    /// <summary>Bodo (India)</summary>
    [NativeName("बड़ो (भारत)")]
    [EnglishName("Bodo (India)")]
    [CultureInfoName("brx-IN")]
    [Description("Bodo (India)")]
    BRX = 4096, // IN 4096

    /// <summary>Bosnian (Cyrillic, Bosnia & Herzegovina)</summary>
    [NativeName("босански (ћирилица, Босна и Херцеговина)")]
    [EnglishName("Bosnian (Cyrillic, Bosnia & Herzegovina)")]
    [CultureInfoName("bs-Cyrl-BA")]
    [Description("Bosnian (Cyrillic, Bosnia & Herzegovina)")]
    BS = 8218, // BA 8218

    /// <summary>Blin (Eritrea)</summary>
    [NativeName("Blin (Eritrea)")]
    [EnglishName("Blin (Eritrea)")]
    [CultureInfoName("byn-ER")]
    [Description("Blin (Eritrea)")]
    BYN = 4096, // ER 4096

    /// <summary>Catalan (Andorra)</summary>
    [NativeName("català (Andorra)")]
    [EnglishName("Catalan (Andorra)")]
    [CultureInfoName("ca-AD")]
    [Description("Catalan (Andorra)")]
    CA = 4096, // AD 4096

    /// <summary>Chakma (Bangladesh)</summary>
    [NativeName("𑄌𑄋𑄴𑄟𑄳𑄦 (𑄝𑄁𑄣𑄘𑄬𑄌𑄴)")]
    [EnglishName("Chakma (Bangladesh)")]
    [CultureInfoName("ccp-BD")]
    [Description("Chakma (Bangladesh)")]
    CCP = 4096, // BD 4096

    /// <summary>Chechen (Russia)</summary>
    [NativeName("нохчийн (Росси)")]
    [EnglishName("Chechen (Russia)")]
    [CultureInfoName("ce-RU")]
    [Description("Chechen (Russia)")]
    CE = 4096, // RU 4096

    /// <summary>Cebuano (Philippines)</summary>
    [NativeName("Cebuano (Pilipinas)")]
    [EnglishName("Cebuano (Philippines)")]
    [CultureInfoName("ceb-PH")]
    [Description("Cebuano (Philippines)")]
    CEB = 4096, // PH 4096

    /// <summary>Chiga (Uganda)</summary>
    [NativeName("Rukiga (Uganda)")]
    [EnglishName("Chiga (Uganda)")]
    [CultureInfoName("cgg-UG")]
    [Description("Chiga (Uganda)")]
    CGG = 4096, // UG 4096

    /// <summary>Cherokee (United States)</summary>
    [NativeName("ᏣᎳᎩ (ᏌᏊ ᎢᏳᎾᎵᏍᏔᏅ ᏍᎦᏚᎩ)")]
    [EnglishName("Cherokee (United States)")]
    [CultureInfoName("chr-US")]
    [Description("Cherokee (United States)")]
    CHR = 4096, // US 4096

    /// <summary>Central Kurdish (Iraq)</summary>
    [NativeName("کوردیی ناوەندی (عێراق)")]
    [EnglishName("Central Kurdish (Iraq)")]
    [CultureInfoName("ckb-IQ")]
    [Description("Central Kurdish (Iraq)")]
    CKB = 4096, // IQ 4096

    /// <summary>Corsican (France)</summary>
    [NativeName("Corsican (France)")]
    [EnglishName("Corsican (France)")]
    [CultureInfoName("co-FR")]
    [Description("Corsican (France)")]
    CO = 1155, // FR 1155

    /// <summary>Czech (Czechia)</summary>
    [NativeName("čeština (Česko)")]
    [EnglishName("Czech (Czechia)")]
    [CultureInfoName("cs-CZ")]
    [Description("Czech (Czechia)")]
    CS = 1029, // CZ 1029

    /// <summary>Church Slavic (Russia)</summary>
    [NativeName("Church Slavic (Russia)")]
    [EnglishName("Church Slavic (Russia)")]
    [CultureInfoName("cu-RU")]
    [Description("Church Slavic (Russia)")]
    CU = 4096, // RU 4096

    /// <summary>Welsh (United Kingdom)</summary>
    [NativeName("Cymraeg (Y Deyrnas Unedig)")]
    [EnglishName("Welsh (United Kingdom)")]
    [CultureInfoName("cy-GB")]
    [Description("Welsh (United Kingdom)")]
    CY = 1106, // GB 1106

    /// <summary>Danish (Denmark)</summary>
    [NativeName("dansk (Danmark)")]
    [EnglishName("Danish (Denmark)")]
    [CultureInfoName("da-DK")]
    [Description("Danish (Denmark)")]
    DA = 1030, // DK 1030

    /// <summary>Taita (Kenya)</summary>
    [NativeName("Kitaita (Kenya)")]
    [EnglishName("Taita (Kenya)")]
    [CultureInfoName("dav-KE")]
    [Description("Taita (Kenya)")]
    DAV = 4096, // KE 4096

    /// <summary>German (Austria)</summary>
    [NativeName("Deutsch (Österreich)")]
    [EnglishName("German (Austria)")]
    [CultureInfoName("de-AT")]
    [Description("German (Austria)")]
    DE = 3079, // AT 3079

    /// <summary>Zarma (Niger)</summary>
    [NativeName("Zarmaciine (Nižer)")]
    [EnglishName("Zarma (Niger)")]
    [CultureInfoName("dje-NE")]
    [Description("Zarma (Niger)")]
    DJE = 4096, // NE 4096

    /// <summary>Lower Sorbian (Germany)</summary>
    [NativeName("dolnoserbšćina (Nimska)")]
    [EnglishName("Lower Sorbian (Germany)")]
    [CultureInfoName("dsb-DE")]
    [Description("Lower Sorbian (Germany)")]
    DSB = 2094, // DE 2094

    /// <summary>Duala (Cameroon)</summary>
    [NativeName("duálá (Cameroun)")]
    [EnglishName("Duala (Cameroon)")]
    [CultureInfoName("dua-CM")]
    [Description("Duala (Cameroon)")]
    DUA = 4096, // CM 4096

    /// <summary>Divehi (Maldives)</summary>
    [NativeName("Divehi (Maldives)")]
    [EnglishName("Divehi (Maldives)")]
    [CultureInfoName("dv-MV")]
    [Description("Divehi (Maldives)")]
    DV = 1125, // MV 1125

    /// <summary>Jola-Fonyi (Senegal)</summary>
    [NativeName("joola (Senegal)")]
    [EnglishName("Jola-Fonyi (Senegal)")]
    [CultureInfoName("dyo-SN")]
    [Description("Jola-Fonyi (Senegal)")]
    DYO = 4096, // SN 4096

    /// <summary>Dzongkha (Bhutan)</summary>
    [NativeName("རྫོང་ཁ། (འབྲུག།)")]
    [EnglishName("Dzongkha (Bhutan)")]
    [CultureInfoName("dz-BT")]
    [Description("Dzongkha (Bhutan)")]
    DZ = 3153, // BT 3153

    /// <summary>Embu (Kenya)</summary>
    [NativeName("Kĩembu (Kenya)")]
    [EnglishName("Embu (Kenya)")]
    [CultureInfoName("ebu-KE")]
    [Description("Embu (Kenya)")]
    EBU = 4096, // KE 4096

    /// <summary>Ewe (Ghana)</summary>
    [NativeName("Eʋegbe (Ghana nutome)")]
    [EnglishName("Ewe (Ghana)")]
    [CultureInfoName("ee-GH")]
    [Description("Ewe (Ghana)")]
    EE = 4096, // GH 4096

    /// <summary>Greek (Cyprus)</summary>
    [NativeName("Ελληνικά (Κύπρος)")]
    [EnglishName("Greek (Cyprus)")]
    [CultureInfoName("el-CY")]
    [Description("Greek (Cyprus)")]
    EL = 4096, // CY 4096

    /// <summary>English (World)</summary>
    [NativeName("English (World)")]
    [EnglishName("English (World)")]
    [CultureInfoName("en-001")]
    [Description("English (World)")]
    EN = 4096, // 001 4096

    /// <summary>Esperanto (World)</summary>
    [NativeName("esperanto (Mondo)")]
    [EnglishName("Esperanto (World)")]
    [CultureInfoName("eo-001")]
    [Description("Esperanto (World)")]
    EO = 4096, // 001 4096

    /// <summary>Spanish (Latin America)</summary>
    [NativeName("español (Latinoamérica)")]
    [EnglishName("Spanish (Latin America)")]
    [CultureInfoName("es-419")]
    [Description("Spanish (Latin America)")]
    ES = 22538, // 419 22538

    /// <summary>Estonian (Estonia)</summary>
    [NativeName("eesti (Eesti)")]
    [EnglishName("Estonian (Estonia)")]
    [CultureInfoName("et-EE")]
    [Description("Estonian (Estonia)")]
    ET = 1061, // EE 1061

    /// <summary>Basque (Spain)</summary>
    [NativeName("euskara (Espainia)")]
    [EnglishName("Basque (Spain)")]
    [CultureInfoName("eu-ES")]
    [Description("Basque (Spain)")]
    EU = 1069, // ES 1069

    /// <summary>Ewondo (Cameroon)</summary>
    [NativeName("ewondo (Kamərún)")]
    [EnglishName("Ewondo (Cameroon)")]
    [CultureInfoName("ewo-CM")]
    [Description("Ewondo (Cameroon)")]
    EWO = 4096, // CM 4096

    /// <summary>Persian (Afghanistan)</summary>
    [NativeName("فارسی (افغانستان)")]
    [EnglishName("Persian (Afghanistan)")]
    [CultureInfoName("fa-AF")]
    [Description("Persian (Afghanistan)")]
    FA = 4096, // AF 4096

    /// <summary>Fulah (Latin, Burkina Faso)</summary>
    [NativeName("Fulah (Latin, Burkina Faso)")]
    [EnglishName("Fulah (Latin, Burkina Faso)")]
    [CultureInfoName("ff-Latn-BF")]
    [Description("Fulah (Latin, Burkina Faso)")]
    FF = 4096, // BF 4096

    /// <summary>Finnish (Finland)</summary>
    [NativeName("suomi (Suomi)")]
    [EnglishName("Finnish (Finland)")]
    [CultureInfoName("fi-FI")]
    [Description("Finnish (Finland)")]
    FI = 1035, // FI 1035

    /// <summary>Filipino (Philippines)</summary>
    [NativeName("Filipino (Pilipinas)")]
    [EnglishName("Filipino (Philippines)")]
    [CultureInfoName("fil-PH")]
    [Description("Filipino (Philippines)")]
    FIL = 1124, // PH 1124

    /// <summary>Faroese (Denmark)</summary>
    [NativeName("føroyskt (Danmark)")]
    [EnglishName("Faroese (Denmark)")]
    [CultureInfoName("fo-DK")]
    [Description("Faroese (Denmark)")]
    FO = 4096, // DK 4096

    /// <summary>French (Belgium)</summary>
    [NativeName("français (Belgique)")]
    [EnglishName("French (Belgium)")]
    [CultureInfoName("fr-BE")]
    [Description("French (Belgium)")]
    FR = 2060, // BE 2060

    /// <summary>Friulian (Italy)</summary>
    [NativeName("furlan (Italie)")]
    [EnglishName("Friulian (Italy)")]
    [CultureInfoName("fur-IT")]
    [Description("Friulian (Italy)")]
    FUR = 4096, // IT 4096

    /// <summary>Western Frisian (Netherlands)</summary>
    [NativeName("Frysk (Nederlân)")]
    [EnglishName("Western Frisian (Netherlands)")]
    [CultureInfoName("fy-NL")]
    [Description("Western Frisian (Netherlands)")]
    FY = 1122, // NL 1122

    /// <summary>Irish (Ireland)</summary>
    [NativeName("Gaeilge (Éire)")]
    [EnglishName("Irish (Ireland)")]
    [CultureInfoName("ga-IE")]
    [Description("Irish (Ireland)")]
    GA = 2108, // IE 2108

    /// <summary>Scottish Gaelic (United Kingdom)</summary>
    [NativeName("Gàidhlig (An Rìoghachd Aonaichte)")]
    [EnglishName("Scottish Gaelic (United Kingdom)")]
    [CultureInfoName("gd-GB")]
    [Description("Scottish Gaelic (United Kingdom)")]
    GD = 1169, // GB 1169

    /// <summary>Galician (Spain)</summary>
    [NativeName("galego (España)")]
    [EnglishName("Galician (Spain)")]
    [CultureInfoName("gl-ES")]
    [Description("Galician (Spain)")]
    GL = 1110, // ES 1110

    /// <summary>Guarani (Paraguay)</summary>
    [NativeName("Guarani (Paraguay)")]
    [EnglishName("Guarani (Paraguay)")]
    [CultureInfoName("gn-PY")]
    [Description("Guarani (Paraguay)")]
    GN = 1140, // PY 1140

    /// <summary>Swiss German (Switzerland)</summary>
    [NativeName("Schwiizertüütsch (Schwiiz)")]
    [EnglishName("Swiss German (Switzerland)")]
    [CultureInfoName("gsw-CH")]
    [Description("Swiss German (Switzerland)")]
    GSW = 4096, // CH 4096

    /// <summary>Gujarati (India)</summary>
    [NativeName("ગુજરાતી (ભારત)")]
    [EnglishName("Gujarati (India)")]
    [CultureInfoName("gu-IN")]
    [Description("Gujarati (India)")]
    GU = 1095, // IN 1095

    /// <summary>Gusii (Kenya)</summary>
    [NativeName("Ekegusii (Kenya)")]
    [EnglishName("Gusii (Kenya)")]
    [CultureInfoName("guz-KE")]
    [Description("Gusii (Kenya)")]
    GUZ = 4096, // KE 4096

    /// <summary>Manx (Isle of Man)</summary>
    [NativeName("Gaelg (Ellan Vannin)")]
    [EnglishName("Manx (Isle of Man)")]
    [CultureInfoName("gv-IM")]
    [Description("Manx (Isle of Man)")]
    GV = 4096, // IM 4096

    /// <summary>Hausa (Ghana)</summary>
    [NativeName("Hausa (Gana)")]
    [EnglishName("Hausa (Ghana)")]
    [CultureInfoName("ha-GH")]
    [Description("Hausa (Ghana)")]
    HA = 4096, // GH 4096

    /// <summary>Hawaiian (United States)</summary>
    [NativeName("ʻŌlelo Hawaiʻi (ʻAmelika Hui Pū ʻIa)")]
    [EnglishName("Hawaiian (United States)")]
    [CultureInfoName("haw-US")]
    [Description("Hawaiian (United States)")]
    HAW = 1141, // US 1141

    /// <summary>Hebrew (Israel)</summary>
    [NativeName("עברית (ישראל)")]
    [EnglishName("Hebrew (Israel)")]
    [CultureInfoName("he-IL")]
    [Description("Hebrew (Israel)")]
    HE = 1037, // IL 1037

    /// <summary>Hindi (India)</summary>
    [NativeName("हिन्दी (भारत)")]
    [EnglishName("Hindi (India)")]
    [CultureInfoName("hi-IN")]
    [Description("Hindi (India)")]
    HI = 1081, // IN 1081

    /// <summary>Croatian (Bosnia & Herzegovina)</summary>
    [NativeName("hrvatski (Bosna i Hercegovina)")]
    [EnglishName("Croatian (Bosnia & Herzegovina)")]
    [CultureInfoName("hr-BA")]
    [Description("Croatian (Bosnia & Herzegovina)")]
    HR = 4122, // BA 4122

    /// <summary>Upper Sorbian (Germany)</summary>
    [NativeName("hornjoserbšćina (Němska)")]
    [EnglishName("Upper Sorbian (Germany)")]
    [CultureInfoName("hsb-DE")]
    [Description("Upper Sorbian (Germany)")]
    HSB = 1070, // DE 1070

    /// <summary>Hungarian (Hungary)</summary>
    [NativeName("magyar (Magyarország)")]
    [EnglishName("Hungarian (Hungary)")]
    [CultureInfoName("hu-HU")]
    [Description("Hungarian (Hungary)")]
    HU = 1038, // HU 1038

    /// <summary>Armenian (Armenia)</summary>
    [NativeName("հայերեն (Հայաստան)")]
    [EnglishName("Armenian (Armenia)")]
    [CultureInfoName("hy-AM")]
    [Description("Armenian (Armenia)")]
    HY = 1067, // AM 1067

    /// <summary>Interlingua (World)</summary>
    [NativeName("interlingua (Mundo)")]
    [EnglishName("Interlingua (World)")]
    [CultureInfoName("ia-001")]
    [Description("Interlingua (World)")]
    IA = 4096, // 001 4096

    /// <summary>Indonesian (Indonesia)</summary>
    [NativeName("Indonesia (Indonesia)")]
    [EnglishName("Indonesian (Indonesia)")]
    [CultureInfoName("id-ID")]
    [Description("Indonesian (Indonesia)")]
    ID = 1057, // ID 1057

    /// <summary>Igbo (Nigeria)</summary>
    [NativeName("Asụsụ Igbo (Naịjịrịa)")]
    [EnglishName("Igbo (Nigeria)")]
    [CultureInfoName("ig-NG")]
    [Description("Igbo (Nigeria)")]
    IG = 1136, // NG 1136

    /// <summary>Yi (China)</summary>
    [NativeName("ꆈꌠꉙ (ꍏꇩ)")]
    [EnglishName("Yi (China)")]
    [CultureInfoName("ii-CN")]
    [Description("Yi (China)")]
    II = 1144, // CN 1144

    /// <summary>Icelandic (Iceland)</summary>
    [NativeName("íslenska (Ísland)")]
    [EnglishName("Icelandic (Iceland)")]
    [CultureInfoName("is-IS")]
    [Description("Icelandic (Iceland)")]
    IS = 1039, // IS 1039

    /// <summary>Italian (Switzerland)</summary>
    [NativeName("italiano (Svizzera)")]
    [EnglishName("Italian (Switzerland)")]
    [CultureInfoName("it-CH")]
    [Description("Italian (Switzerland)")]
    IT = 2064, // CH 2064

    /// <summary>Inuktitut (Canada)</summary>
    [NativeName("Inuktitut (Canada)")]
    [EnglishName("Inuktitut (Canada)")]
    [CultureInfoName("iu-CA")]
    [Description("Inuktitut (Canada)")]
    IU = 4096, // CA 4096

    /// <summary>Japanese (Japan)</summary>
    [NativeName("日本語 (日本)")]
    [EnglishName("Japanese (Japan)")]
    [CultureInfoName("ja-JP")]
    [Description("Japanese (Japan)")]
    JA = 1041, // JP 1041

    /// <summary>Ngomba (Cameroon)</summary>
    [NativeName("Ndaꞌa (Kamɛlûn)")]
    [EnglishName("Ngomba (Cameroon)")]
    [CultureInfoName("jgo-CM")]
    [Description("Ngomba (Cameroon)")]
    JGO = 4096, // CM 4096

    /// <summary>Machame (Tanzania)</summary>
    [NativeName("Kimachame (Tanzania)")]
    [EnglishName("Machame (Tanzania)")]
    [CultureInfoName("jmc-TZ")]
    [Description("Machame (Tanzania)")]
    JMC = 4096, // TZ 4096

    /// <summary>Javanese (Indonesia)</summary>
    [NativeName("Jawa (Indonésia)")]
    [EnglishName("Javanese (Indonesia)")]
    [CultureInfoName("jv-ID")]
    [Description("Javanese (Indonesia)")]
    JV = 4096, // ID 4096

    /// <summary>Georgian (Georgia)</summary>
    [NativeName("ქართული (საქართველო)")]
    [EnglishName("Georgian (Georgia)")]
    [CultureInfoName("ka-GE")]
    [Description("Georgian (Georgia)")]
    KA = 1079, // GE 1079

    /// <summary>Kabyle (Algeria)</summary>
    [NativeName("Taqbaylit (Lezzayer)")]
    [EnglishName("Kabyle (Algeria)")]
    [CultureInfoName("kab-DZ")]
    [Description("Kabyle (Algeria)")]
    KAB = 4096, // DZ 4096

    /// <summary>Kamba (Kenya)</summary>
    [NativeName("Kikamba (Kenya)")]
    [EnglishName("Kamba (Kenya)")]
    [CultureInfoName("kam-KE")]
    [Description("Kamba (Kenya)")]
    KAM = 4096, // KE 4096

    /// <summary>Makonde (Tanzania)</summary>
    [NativeName("Chimakonde (Tanzania)")]
    [EnglishName("Makonde (Tanzania)")]
    [CultureInfoName("kde-TZ")]
    [Description("Makonde (Tanzania)")]
    KDE = 4096, // TZ 4096

    /// <summary>Kabuverdianu (Cabo Verde)</summary>
    [NativeName("kabuverdianu (Kabu Verdi)")]
    [EnglishName("Kabuverdianu (Cabo Verde)")]
    [CultureInfoName("kea-CV")]
    [Description("Kabuverdianu (Cabo Verde)")]
    KEA = 4096, // CV 4096

    /// <summary>Koyra Chiini (Mali)</summary>
    [NativeName("Koyra ciini (Maali)")]
    [EnglishName("Koyra Chiini (Mali)")]
    [CultureInfoName("khq-ML")]
    [Description("Koyra Chiini (Mali)")]
    KHQ = 4096, // ML 4096

    /// <summary>Kikuyu (Kenya)</summary>
    [NativeName("Gikuyu (Kenya)")]
    [EnglishName("Kikuyu (Kenya)")]
    [CultureInfoName("ki-KE")]
    [Description("Kikuyu (Kenya)")]
    KI = 4096, // KE 4096

    /// <summary>Kazakh (Kazakhstan)</summary>
    [NativeName("қазақ тілі (Қазақстан)")]
    [EnglishName("Kazakh (Kazakhstan)")]
    [CultureInfoName("kk-KZ")]
    [Description("Kazakh (Kazakhstan)")]
    KK = 1087, // KZ 1087

    /// <summary>Kako (Cameroon)</summary>
    [NativeName("kakɔ (Kamɛrun)")]
    [EnglishName("Kako (Cameroon)")]
    [CultureInfoName("kkj-CM")]
    [Description("Kako (Cameroon)")]
    KKJ = 4096, // CM 4096

    /// <summary>Kalaallisut (Greenland)</summary>
    [NativeName("kalaallisut (Kalaallit Nunaat)")]
    [EnglishName("Kalaallisut (Greenland)")]
    [CultureInfoName("kl-GL")]
    [Description("Kalaallisut (Greenland)")]
    KL = 1135, // GL 1135

    /// <summary>Kalenjin (Kenya)</summary>
    [NativeName("Kalenjin (Emetab Kenya)")]
    [EnglishName("Kalenjin (Kenya)")]
    [CultureInfoName("kln-KE")]
    [Description("Kalenjin (Kenya)")]
    KLN = 4096, // KE 4096

    /// <summary>Khmer (Cambodia)</summary>
    [NativeName("ខ្មែរ (កម្ពុជា)")]
    [EnglishName("Khmer (Cambodia)")]
    [CultureInfoName("km-KH")]
    [Description("Khmer (Cambodia)")]
    KM = 1107, // KH 1107

    /// <summary>Kannada (India)</summary>
    [NativeName("ಕನ್ನಡ (ಭಾರತ)")]
    [EnglishName("Kannada (India)")]
    [CultureInfoName("kn-IN")]
    [Description("Kannada (India)")]
    KN = 1099, // IN 1099

    /// <summary>Korean (North Korea)</summary>
    [NativeName("한국어(조선민주주의인민공화국)")]
    [EnglishName("Korean (North Korea)")]
    [CultureInfoName("ko-KP")]
    [Description("Korean (North Korea)")]
    KO = 4096, // KP 4096

    /// <summary>Konkani (India)</summary>
    [NativeName("कोंकणी (भारत)")]
    [EnglishName("Konkani (India)")]
    [CultureInfoName("kok-IN")]
    [Description("Konkani (India)")]
    KOK = 1111, // IN 1111

    /// <summary>Kashmiri (India)</summary>
    [NativeName("کٲشُر (ہِندوستان)")]
    [EnglishName("Kashmiri (India)")]
    [CultureInfoName("ks-IN")]
    [Description("Kashmiri (India)")]
    KS = 4096, // IN 4096

    /// <summary>Shambala (Tanzania)</summary>
    [NativeName("Kishambaa (Tanzania)")]
    [EnglishName("Shambala (Tanzania)")]
    [CultureInfoName("ksb-TZ")]
    [Description("Shambala (Tanzania)")]
    KSB = 4096, // TZ 4096

    /// <summary>Bafia (Cameroon)</summary>
    [NativeName("rikpa (kamɛrún)")]
    [EnglishName("Bafia (Cameroon)")]
    [CultureInfoName("ksf-CM")]
    [Description("Bafia (Cameroon)")]
    KSF = 4096, // CM 4096

    /// <summary>Colognian (Germany)</summary>
    [NativeName("Kölsch en Doütschland")]
    [EnglishName("Colognian (Germany)")]
    [CultureInfoName("ksh-DE")]
    [Description("Colognian (Germany)")]
    KSH = 4096, // DE 4096

    /// <summary>Cornish (United Kingdom)</summary>
    [NativeName("kernewek (Rywvaneth Unys)")]
    [EnglishName("Cornish (United Kingdom)")]
    [CultureInfoName("kw-GB")]
    [Description("Cornish (United Kingdom)")]
    KW = 4096, // GB 4096

    /// <summary>Kyrgyz (Kyrgyzstan)</summary>
    [NativeName("кыргызча (Кыргызстан)")]
    [EnglishName("Kyrgyz (Kyrgyzstan)")]
    [CultureInfoName("ky-KG")]
    [Description("Kyrgyz (Kyrgyzstan)")]
    KY = 1088, // KG 1088

    /// <summary>Langi (Tanzania)</summary>
    [NativeName("Kɨlaangi (Taansanía)")]
    [EnglishName("Langi (Tanzania)")]
    [CultureInfoName("lag-TZ")]
    [Description("Langi (Tanzania)")]
    LAG = 4096, // TZ 4096

    /// <summary>Luxembourgish (Luxembourg)</summary>
    [NativeName("Lëtzebuergesch (Lëtzebuerg)")]
    [EnglishName("Luxembourgish (Luxembourg)")]
    [CultureInfoName("lb-LU")]
    [Description("Luxembourgish (Luxembourg)")]
    LB = 1134, // LU 1134

    /// <summary>Ganda (Uganda)</summary>
    [NativeName("Luganda (Yuganda)")]
    [EnglishName("Ganda (Uganda)")]
    [CultureInfoName("lg-UG")]
    [Description("Ganda (Uganda)")]
    LG = 4096, // UG 4096

    /// <summary>Lakota (United States)</summary>
    [NativeName("Lakȟólʼiyapi (Mílahaŋska Tȟamákȟočhe)")]
    [EnglishName("Lakota (United States)")]
    [CultureInfoName("lkt-US")]
    [Description("Lakota (United States)")]
    LKT = 4096, // US 4096

    /// <summary>Lingala (Angola)</summary>
    [NativeName("lingála (Angóla)")]
    [EnglishName("Lingala (Angola)")]
    [CultureInfoName("ln-AO")]
    [Description("Lingala (Angola)")]
    LN = 4096, // AO 4096

    /// <summary>Lao (Laos)</summary>
    [NativeName("ລາວ (ລາວ)")]
    [EnglishName("Lao (Laos)")]
    [CultureInfoName("lo-LA")]
    [Description("Lao (Laos)")]
    LO = 1108, // LA 1108

    /// <summary>Northern Luri (Iraq)</summary>
    [NativeName("Northern Luri (Iraq)")]
    [EnglishName("Northern Luri (Iraq)")]
    [CultureInfoName("lrc-IQ")]
    [Description("Northern Luri (Iraq)")]
    LRC = 4096, // IQ 4096

    /// <summary>Lithuanian (Lithuania)</summary>
    [NativeName("lietuvių (Lietuva)")]
    [EnglishName("Lithuanian (Lithuania)")]
    [CultureInfoName("lt-LT")]
    [Description("Lithuanian (Lithuania)")]
    LT = 1063, // LT 1063

    /// <summary>Luba-Katanga (Congo [DRC])</summary>
    [NativeName("Tshiluba (Ditunga wa Kongu)")]
    [EnglishName("Luba-Katanga (Congo [DRC])")]
    [CultureInfoName("lu-CD")]
    [Description("Luba-Katanga (Congo [DRC])")]
    LU = 4096, // CD 4096

    /// <summary>Luo (Kenya)</summary>
    [NativeName("Dholuo (Kenya)")]
    [EnglishName("Luo (Kenya)")]
    [CultureInfoName("luo-KE")]
    [Description("Luo (Kenya)")]
    LUO = 4096, // KE 4096

    /// <summary>Luyia (Kenya)</summary>
    [NativeName("Luluhia (Kenya)")]
    [EnglishName("Luyia (Kenya)")]
    [CultureInfoName("luy-KE")]
    [Description("Luyia (Kenya)")]
    LUY = 4096, // KE 4096

    /// <summary>Latvian (Latvia)</summary>
    [NativeName("latviešu (Latvija)")]
    [EnglishName("Latvian (Latvia)")]
    [CultureInfoName("lv-LV")]
    [Description("Latvian (Latvia)")]
    LV = 1062, // LV 1062

    /// <summary>Masai (Kenya)</summary>
    [NativeName("Maa (Kenya)")]
    [EnglishName("Masai (Kenya)")]
    [CultureInfoName("mas-KE")]
    [Description("Masai (Kenya)")]
    MAS = 4096, // KE 4096

    /// <summary>Meru (Kenya)</summary>
    [NativeName("Kĩmĩrũ (Kenya)")]
    [EnglishName("Meru (Kenya)")]
    [CultureInfoName("mer-KE")]
    [Description("Meru (Kenya)")]
    MER = 4096, // KE 4096

    /// <summary>Morisyen (Mauritius)</summary>
    [NativeName("kreol morisien (Moris)")]
    [EnglishName("Morisyen (Mauritius)")]
    [CultureInfoName("mfe-MU")]
    [Description("Morisyen (Mauritius)")]
    MFE = 4096, // MU 4096

    /// <summary>Malagasy (Madagascar)</summary>
    [NativeName("Malagasy (Madagasikara)")]
    [EnglishName("Malagasy (Madagascar)")]
    [CultureInfoName("mg-MG")]
    [Description("Malagasy (Madagascar)")]
    MG = 4096, // MG 4096

    /// <summary>Makhuwa-Meetto (Mozambique)</summary>
    [NativeName("Makua (Umozambiki)")]
    [EnglishName("Makhuwa-Meetto (Mozambique)")]
    [CultureInfoName("mgh-MZ")]
    [Description("Makhuwa-Meetto (Mozambique)")]
    MGH = 4096, // MZ 4096

    /// <summary>Metaʼ (Cameroon)</summary>
    [NativeName("metaʼ (Kamalun)")]
    [EnglishName("Metaʼ (Cameroon)")]
    [CultureInfoName("mgo-CM")]
    [Description("Metaʼ (Cameroon)")]
    MGO = 4096, // CM 4096

    /// <summary>Maori (New Zealand)</summary>
    [NativeName("Māori (Aotearoa)")]
    [EnglishName("Maori (New Zealand)")]
    [CultureInfoName("mi-NZ")]
    [Description("Maori (New Zealand)")]
    MI = 1153, // NZ 1153

    /// <summary>Macedonian (North Macedonia)</summary>
    [NativeName("македонски (Северна Македонија)")]
    [EnglishName("Macedonian (North Macedonia)")]
    [CultureInfoName("mk-MK")]
    [Description("Macedonian (North Macedonia)")]
    MK = 1071, // MK 1071

    /// <summary>Malayalam (India)</summary>
    [NativeName("മലയാളം (ഇന്ത്യ)")]
    [EnglishName("Malayalam (India)")]
    [CultureInfoName("ml-IN")]
    [Description("Malayalam (India)")]
    ML = 1100, // IN 1100

    /// <summary>Mongolian (Mongolia)</summary>
    [NativeName("монгол (Монгол)")]
    [EnglishName("Mongolian (Mongolia)")]
    [CultureInfoName("mn-MN")]
    [Description("Mongolian (Mongolia)")]
    MN = 1104, // MN 1104

    /// <summary>Mohawk (Canada)</summary>
    [NativeName("Mohawk (Canada)")]
    [EnglishName("Mohawk (Canada)")]
    [CultureInfoName("moh-CA")]
    [Description("Mohawk (Canada)")]
    MOH = 1148, // CA 1148

    /// <summary>Marathi (India)</summary>
    [NativeName("मराठी (भारत)")]
    [EnglishName("Marathi (India)")]
    [CultureInfoName("mr-IN")]
    [Description("Marathi (India)")]
    MR = 1102, // IN 1102

    /// <summary>Malay (Brunei)</summary>
    [NativeName("Melayu (Brunei)")]
    [EnglishName("Malay (Brunei)")]
    [CultureInfoName("ms-BN")]
    [Description("Malay (Brunei)")]
    MS = 2110, // BN 2110

    /// <summary>Maltese (Malta)</summary>
    [NativeName("Malti (Malta)")]
    [EnglishName("Maltese (Malta)")]
    [CultureInfoName("mt-MT")]
    [Description("Maltese (Malta)")]
    MT = 1082, // MT 1082

    /// <summary>Mundang (Cameroon)</summary>
    [NativeName("MUNDAŊ (kameruŋ)")]
    [EnglishName("Mundang (Cameroon)")]
    [CultureInfoName("mua-CM")]
    [Description("Mundang (Cameroon)")]
    MUA = 4096, // CM 4096

    /// <summary>Burmese (Myanmar)</summary>
    [NativeName("မြန်မာ (မြန်မာ)")]
    [EnglishName("Burmese (Myanmar)")]
    [CultureInfoName("my-MM")]
    [Description("Burmese (Myanmar)")]
    MY = 1109, // MM 1109

    /// <summary>Mazanderani (Iran)</summary>
    [NativeName("مازرونی (ایران)")]
    [EnglishName("Mazanderani (Iran)")]
    [CultureInfoName("mzn-IR")]
    [Description("Mazanderani (Iran)")]
    MZN = 4096, // IR 4096

    /// <summary>Nama (Namibia)</summary>
    [NativeName("Khoekhoegowab (Namibiab)")]
    [EnglishName("Nama (Namibia)")]
    [CultureInfoName("naq-NA")]
    [Description("Nama (Namibia)")]
    NAQ = 4096, // NA 4096

    /// <summary>Norwegian Bokmål (Norway)</summary>
    [NativeName("norsk bokmål (Norge)")]
    [EnglishName("Norwegian Bokmål (Norway)")]
    [CultureInfoName("nb-NO")]
    [Description("Norwegian Bokmål (Norway)")]
    NB = 1044, // NO 1044

    /// <summary>North Ndebele (Zimbabwe)</summary>
    [NativeName("isiNdebele (Zimbabwe)")]
    [EnglishName("North Ndebele (Zimbabwe)")]
    [CultureInfoName("nd-ZW")]
    [Description("North Ndebele (Zimbabwe)")]
    ND = 4096, // ZW 4096

    /// <summary>Low German (Germany)</summary>
    [NativeName("Low German (Germany)")]
    [EnglishName("Low German (Germany)")]
    [CultureInfoName("nds-DE")]
    [Description("Low German (Germany)")]
    NDS = 4096, // DE 4096

    /// <summary>Nepali (India)</summary>
    [NativeName("नेपाली (भारत)")]
    [EnglishName("Nepali (India)")]
    [CultureInfoName("ne-IN")]
    [Description("Nepali (India)")]
    NE = 2145, // IN 2145

    /// <summary>Dutch (Aruba)</summary>
    [NativeName("Nederlands (Aruba)")]
    [EnglishName("Dutch (Aruba)")]
    [CultureInfoName("nl-AW")]
    [Description("Dutch (Aruba)")]
    NL = 4096, // AW 4096

    /// <summary>Kwasio (Cameroon)</summary>
    [NativeName("Kwasio (Cameroon)")]
    [EnglishName("Kwasio (Cameroon)")]
    [CultureInfoName("nmg-CM")]
    [Description("Kwasio (Cameroon)")]
    NMG = 4096, // CM 4096

    /// <summary>Norwegian Nynorsk (Norway)</summary>
    [NativeName("nynorsk (Noreg)")]
    [EnglishName("Norwegian Nynorsk (Norway)")]
    [CultureInfoName("nn-NO")]
    [Description("Norwegian Nynorsk (Norway)")]
    NN = 2068, // NO 2068

    /// <summary>Ngiemboon (Cameroon)</summary>
    [NativeName("Shwóŋò ngiembɔɔn (Kàmalûm)")]
    [EnglishName("Ngiemboon (Cameroon)")]
    [CultureInfoName("nnh-CM")]
    [Description("Ngiemboon (Cameroon)")]
    NNH = 4096, // CM 4096

    /// <summary>N’Ko (Guinea)</summary>
    [NativeName("N’Ko (Guinea)")]
    [EnglishName("N’Ko (Guinea)")]
    [CultureInfoName("nqo-GN")]
    [Description("N’Ko (Guinea)")]
    NQO = 4096, // GN 4096

    /// <summary>South Ndebele (South Africa)</summary>
    [NativeName("South Ndebele (South Africa)")]
    [EnglishName("South Ndebele (South Africa)")]
    [CultureInfoName("nr-ZA")]
    [Description("South Ndebele (South Africa)")]
    NR = 4096, // ZA 4096

    /// <summary>Sesotho sa Leboa (South Africa)</summary>
    [NativeName("Sesotho sa Leboa (South Africa)")]
    [EnglishName("Sesotho sa Leboa (South Africa)")]
    [CultureInfoName("nso-ZA")]
    [Description("Sesotho sa Leboa (South Africa)")]
    NSO = 1132, // ZA 1132

    /// <summary>Nuer (South Sudan)</summary>
    [NativeName("Nuer (South Sudan)")]
    [EnglishName("Nuer (South Sudan)")]
    [CultureInfoName("nus-SS")]
    [Description("Nuer (South Sudan)")]
    NUS = 4096, // SS 4096

    /// <summary>Nyankole (Uganda)</summary>
    [NativeName("Runyankore (Uganda)")]
    [EnglishName("Nyankole (Uganda)")]
    [CultureInfoName("nyn-UG")]
    [Description("Nyankole (Uganda)")]
    NYN = 4096, // UG 4096

    /// <summary>Occitan (France)</summary>
    [NativeName("Occitan (France)")]
    [EnglishName("Occitan (France)")]
    [CultureInfoName("oc-FR")]
    [Description("Occitan (France)")]
    OC = 1154, // FR 1154

    /// <summary>Oromo (Ethiopia)</summary>
    [NativeName("Oromoo (Itoophiyaa)")]
    [EnglishName("Oromo (Ethiopia)")]
    [CultureInfoName("om-ET")]
    [Description("Oromo (Ethiopia)")]
    OM = 1138, // ET 1138

    /// <summary>Odia (India)</summary>
    [NativeName("ଓଡ଼ିଆ (ଭାରତ)")]
    [EnglishName("Odia (India)")]
    [CultureInfoName("or-IN")]
    [Description("Odia (India)")]
    OR = 1096, // IN 1096

    /// <summary>Ossetic (Georgia)</summary>
    [NativeName("ирон (Гуырдзыстон)")]
    [EnglishName("Ossetic (Georgia)")]
    [CultureInfoName("os-GE")]
    [Description("Ossetic (Georgia)")]
    OS = 4096, // GE 4096

    /// <summary>Punjabi (Arabic, Pakistan)</summary>
    [NativeName("پنجابی (عربی, پاکستان)")]
    [EnglishName("Punjabi (Arabic, Pakistan)")]
    [CultureInfoName("pa-Arab-PK")]
    [Description("Punjabi (Arabic, Pakistan)")]
    PA = 2118, // PK 2118

    /// <summary>Polish (Poland)</summary>
    [NativeName("polski (Polska)")]
    [EnglishName("Polish (Poland)")]
    [CultureInfoName("pl-PL")]
    [Description("Polish (Poland)")]
    PL = 1045, // PL 1045

    /// <summary>Prussian (World)</summary>
    [NativeName("Prussian (World)")]
    [EnglishName("Prussian (World)")]
    [CultureInfoName("prg-001")]
    [Description("Prussian (World)")]
    PRG = 4096, // 001 4096

    /// <summary>Pashto (Afghanistan)</summary>
    [NativeName("پښتو (افغانستان)")]
    [EnglishName("Pashto (Afghanistan)")]
    [CultureInfoName("ps-AF")]
    [Description("Pashto (Afghanistan)")]
    PS = 1123, // AF 1123

    /// <summary>Portuguese (Angola)</summary>
    [NativeName("português (Angola)")]
    [EnglishName("Portuguese (Angola)")]
    [CultureInfoName("pt-AO")]
    [Description("Portuguese (Angola)")]
    PT = 4096, // AO 4096

    /// <summary>Quechua (Bolivia)</summary>
    [NativeName("Runasimi (Bolivia)")]
    [EnglishName("Quechua (Bolivia)")]
    [CultureInfoName("qu-BO")]
    [Description("Quechua (Bolivia)")]
    QU = 4096, // BO 4096

    /// <summary>Kʼicheʼ (Guatemala)</summary>
    [NativeName("Kʼicheʼ (Guatemala)")]
    [EnglishName("Kʼicheʼ (Guatemala)")]
    [CultureInfoName("quc-GT")]
    [Description("Kʼicheʼ (Guatemala)")]
    QUC = 4096, // GT 4096

    /// <summary>Romansh (Switzerland)</summary>
    [NativeName("rumantsch (Svizra)")]
    [EnglishName("Romansh (Switzerland)")]
    [CultureInfoName("rm-CH")]
    [Description("Romansh (Switzerland)")]
    RM = 1047, // CH 1047

    /// <summary>Rundi (Burundi)</summary>
    [NativeName("Ikirundi (Uburundi)")]
    [EnglishName("Rundi (Burundi)")]
    [CultureInfoName("rn-BI")]
    [Description("Rundi (Burundi)")]
    RN = 4096, // BI 4096

    /// <summary>Romanian (Moldova)</summary>
    [NativeName("română (Republica Moldova)")]
    [EnglishName("Romanian (Moldova)")]
    [CultureInfoName("ro-MD")]
    [Description("Romanian (Moldova)")]
    RO = 2072, // MD 2072

    /// <summary>Rombo (Tanzania)</summary>
    [NativeName("Kihorombo (Tanzania)")]
    [EnglishName("Rombo (Tanzania)")]
    [CultureInfoName("rof-TZ")]
    [Description("Rombo (Tanzania)")]
    ROF = 4096, // TZ 4096

    /// <summary>Russian (Belarus)</summary>
    [NativeName("русский (Беларусь)")]
    [EnglishName("Russian (Belarus)")]
    [CultureInfoName("ru-BY")]
    [Description("Russian (Belarus)")]
    RU = 4096, // BY 4096

    /// <summary>Kinyarwanda (Rwanda)</summary>
    [NativeName("Kinyarwanda (U Rwanda)")]
    [EnglishName("Kinyarwanda (Rwanda)")]
    [CultureInfoName("rw-RW")]
    [Description("Kinyarwanda (Rwanda)")]
    RW = 1159, // RW 1159

    /// <summary>Rwa (Tanzania)</summary>
    [NativeName("Kiruwa (Tanzania)")]
    [EnglishName("Rwa (Tanzania)")]
    [CultureInfoName("rwk-TZ")]
    [Description("Rwa (Tanzania)")]
    RWK = 4096, // TZ 4096

    /// <summary>Sanskrit (India)</summary>
    [NativeName("Sanskrit (India)")]
    [EnglishName("Sanskrit (India)")]
    [CultureInfoName("sa-IN")]
    [Description("Sanskrit (India)")]
    SA = 1103, // IN 1103

    /// <summary>Sakha (Russia)</summary>
    [NativeName("саха тыла (Арассыыйа)")]
    [EnglishName("Sakha (Russia)")]
    [CultureInfoName("sah-RU")]
    [Description("Sakha (Russia)")]
    SAH = 1157, // RU 1157

    /// <summary>Samburu (Kenya)</summary>
    [NativeName("Kisampur (Kenya)")]
    [EnglishName("Samburu (Kenya)")]
    [CultureInfoName("saq-KE")]
    [Description("Samburu (Kenya)")]
    SAQ = 4096, // KE 4096

    /// <summary>Sangu (Tanzania)</summary>
    [NativeName("Ishisangu (Tansaniya)")]
    [EnglishName("Sangu (Tanzania)")]
    [CultureInfoName("sbp-TZ")]
    [Description("Sangu (Tanzania)")]
    SBP = 4096, // TZ 4096

    /// <summary>Sindhi (Pakistan)</summary>
    [NativeName("سنڌي (پاڪستان)")]
    [EnglishName("Sindhi (Pakistan)")]
    [CultureInfoName("sd-PK")]
    [Description("Sindhi (Pakistan)")]
    SD = 4096, // PK 4096

    /// <summary>Northern Sami (Finland)</summary>
    [NativeName("davvisámegiella (Suopma)")]
    [EnglishName("Northern Sami (Finland)")]
    [CultureInfoName("se-FI")]
    [Description("Northern Sami (Finland)")]
    SE = 3131, // FI 3131

    /// <summary>Sena (Mozambique)</summary>
    [NativeName("sena (Moçambique)")]
    [EnglishName("Sena (Mozambique)")]
    [CultureInfoName("seh-MZ")]
    [Description("Sena (Mozambique)")]
    SEH = 4096, // MZ 4096

    /// <summary>Koyraboro Senni (Mali)</summary>
    [NativeName("Koyraboro senni (Maali)")]
    [EnglishName("Koyraboro Senni (Mali)")]
    [CultureInfoName("ses-ML")]
    [Description("Koyraboro Senni (Mali)")]
    SES = 4096, // ML 4096

    /// <summary>Sango (Central African Republic)</summary>
    [NativeName("Sängö (Ködörösêse tî Bêafrîka)")]
    [EnglishName("Sango (Central African Republic)")]
    [CultureInfoName("sg-CF")]
    [Description("Sango (Central African Republic)")]
    SG = 4096, // CF 4096

    /// <summary>Tachelhit (Latin, Morocco)</summary>
    [NativeName("Tachelhit (Latin, Morocco)")]
    [EnglishName("Tachelhit (Latin, Morocco)")]
    [CultureInfoName("shi-Latn-MA")]
    [Description("Tachelhit (Latin, Morocco)")]
    SHI = 4096, // MA 4096

    /// <summary>Sinhala (Sri Lanka)</summary>
    [NativeName("සිංහල (ශ්රී ලංකාව)")]
    [EnglishName("Sinhala (Sri Lanka)")]
    [CultureInfoName("si-LK")]
    [Description("Sinhala (Sri Lanka)")]
    SI = 1115, // LK 1115

    /// <summary>Slovak (Slovakia)</summary>
    [NativeName("slovenčina (Slovensko)")]
    [EnglishName("Slovak (Slovakia)")]
    [CultureInfoName("sk-SK")]
    [Description("Slovak (Slovakia)")]
    SK = 1051, // SK 1051

    /// <summary>Slovenian (Slovenia)</summary>
    [NativeName("slovenščina (Slovenija)")]
    [EnglishName("Slovenian (Slovenia)")]
    [CultureInfoName("sl-SI")]
    [Description("Slovenian (Slovenia)")]
    SL = 1060, // SI 1060

    /// <summary>Southern Sami (Norway)</summary>
    [NativeName("Southern Sami (Norway)")]
    [EnglishName("Southern Sami (Norway)")]
    [CultureInfoName("sma-NO")]
    [Description("Southern Sami (Norway)")]
    SMA = 6203, // NO 6203

    /// <summary>Lule Sami (Norway)</summary>
    [NativeName("Lule Sami (Norway)")]
    [EnglishName("Lule Sami (Norway)")]
    [CultureInfoName("smj-NO")]
    [Description("Lule Sami (Norway)")]
    SMJ = 4155, // NO 4155

    /// <summary>Inari Sami (Finland)</summary>
    [NativeName("anarâškielâ (Suomâ)")]
    [EnglishName("Inari Sami (Finland)")]
    [CultureInfoName("smn-FI")]
    [Description("Inari Sami (Finland)")]
    SMN = 9275, // FI 9275

    /// <summary>Skolt Sami (Finland)</summary>
    [NativeName("Skolt Sami (Finland)")]
    [EnglishName("Skolt Sami (Finland)")]
    [CultureInfoName("sms-FI")]
    [Description("Skolt Sami (Finland)")]
    SMS = 8251, // FI 8251

    /// <summary>Shona (Zimbabwe)</summary>
    [NativeName("chiShona (Zimbabwe)")]
    [EnglishName("Shona (Zimbabwe)")]
    [CultureInfoName("sn-ZW")]
    [Description("Shona (Zimbabwe)")]
    SN = 4096, // ZW 4096

    /// <summary>Somali (Djibouti)</summary>
    [NativeName("Soomaali (Jabuuti)")]
    [EnglishName("Somali (Djibouti)")]
    [CultureInfoName("so-DJ")]
    [Description("Somali (Djibouti)")]
    SO = 4096, // DJ 4096

    /// <summary>Albanian (Albania)</summary>
    [NativeName("shqip (Shqipëri)")]
    [EnglishName("Albanian (Albania)")]
    [CultureInfoName("sq-AL")]
    [Description("Albanian (Albania)")]
    SQ = 1052, // AL 1052

    /// <summary>Serbian (Cyrillic, Bosnia & Herzegovina)</summary>
    [NativeName("српски (ћирилица, Босна и Херцеговина)")]
    [EnglishName("Serbian (Cyrillic, Bosnia & Herzegovina)")]
    [CultureInfoName("sr-Cyrl-BA")]
    [Description("Serbian (Cyrillic, Bosnia & Herzegovina)")]
    SR = 7194, // BA 7194

    /// <summary>siSwati (Eswatini)</summary>
    [NativeName("siSwati (eSwatini)")]
    [EnglishName("siSwati (Eswatini)")]
    [CultureInfoName("ss-SZ")]
    [Description("siSwati (Eswatini)")]
    SS = 4096, // SZ 4096

    /// <summary>Saho (Eritrea)</summary>
    [NativeName("Saho (Eritrea)")]
    [EnglishName("Saho (Eritrea)")]
    [CultureInfoName("ssy-ER")]
    [Description("Saho (Eritrea)")]
    SSY = 4096, // ER 4096

    /// <summary>Sesotho (Lesotho)</summary>
    [NativeName("Sesotho (Lesotho)")]
    [EnglishName("Sesotho (Lesotho)")]
    [CultureInfoName("st-LS")]
    [Description("Sesotho (Lesotho)")]
    ST = 4096, // LS 4096

    /// <summary>Swedish (Åland Islands)</summary>
    [NativeName("svenska (Åland)")]
    [EnglishName("Swedish (Åland Islands)")]
    [CultureInfoName("sv-AX")]
    [Description("Swedish (Åland Islands)")]
    SV = 4096, // AX 4096

    /// <summary>Kiswahili (Congo [DRC])</summary>
    [NativeName("Kiswahili (Jamhuri ya Kidemokrasia ya Kongo)")]
    [EnglishName("Kiswahili (Congo [DRC])")]
    [CultureInfoName("sw-CD")]
    [Description("Kiswahili (Congo [DRC])")]
    SW = 4096, // CD 4096

    /// <summary>Syriac (Syria)</summary>
    [NativeName("Syriac (Syria)")]
    [EnglishName("Syriac (Syria)")]
    [CultureInfoName("syr-SY")]
    [Description("Syriac (Syria)")]
    SYR = 1114, // SY 1114

    /// <summary>Tamil (India)</summary>
    [NativeName("தமிழ் (இந்தியா)")]
    [EnglishName("Tamil (India)")]
    [CultureInfoName("ta-IN")]
    [Description("Tamil (India)")]
    TA = 1097, // IN 1097

    /// <summary>Telugu (India)</summary>
    [NativeName("తెలుగు (భారతదేశం)")]
    [EnglishName("Telugu (India)")]
    [CultureInfoName("te-IN")]
    [Description("Telugu (India)")]
    TE = 1098, // IN 1098

    /// <summary>Teso (Kenya)</summary>
    [NativeName("Kiteso (Kenia)")]
    [EnglishName("Teso (Kenya)")]
    [CultureInfoName("teo-KE")]
    [Description("Teso (Kenya)")]
    TEO = 4096, // KE 4096

    /// <summary>Tajik (Tajikistan)</summary>
    [NativeName("тоҷикӣ (Тоҷикистон)")]
    [EnglishName("Tajik (Tajikistan)")]
    [CultureInfoName("tg-TJ")]
    [Description("Tajik (Tajikistan)")]
    TG = 4096, // TJ 4096

    /// <summary>Thai (Thailand)</summary>
    [NativeName("ไทย (ไทย)")]
    [EnglishName("Thai (Thailand)")]
    [CultureInfoName("th-TH")]
    [Description("Thai (Thailand)")]
    TH = 1054, // TH 1054

    /// <summary>Tigrinya (Eritrea)</summary>
    [NativeName("ትግርኛ (ኤርትራ)")]
    [EnglishName("Tigrinya (Eritrea)")]
    [CultureInfoName("ti-ER")]
    [Description("Tigrinya (Eritrea)")]
    TI = 2163, // ER 2163

    /// <summary>Tigre (Eritrea)</summary>
    [NativeName("Tigre (Eritrea)")]
    [EnglishName("Tigre (Eritrea)")]
    [CultureInfoName("tig-ER")]
    [Description("Tigre (Eritrea)")]
    TIG = 4096, // ER 4096

    /// <summary>Turkmen (Turkmenistan)</summary>
    [NativeName("türkmen dili (Türkmenistan)")]
    [EnglishName("Turkmen (Turkmenistan)")]
    [CultureInfoName("tk-TM")]
    [Description("Turkmen (Turkmenistan)")]
    TK = 1090, // TM 1090

    /// <summary>Setswana (Botswana)</summary>
    [NativeName("Setswana (Botswana)")]
    [EnglishName("Setswana (Botswana)")]
    [CultureInfoName("tn-BW")]
    [Description("Setswana (Botswana)")]
    TN = 2098, // BW 2098

    /// <summary>Tongan (Tonga)</summary>
    [NativeName("lea fakatonga (Tonga)")]
    [EnglishName("Tongan (Tonga)")]
    [CultureInfoName("to-TO")]
    [Description("Tongan (Tonga)")]
    TO = 4096, // TO 4096

    /// <summary>Turkish (Cyprus)</summary>
    [NativeName("Türkçe (Kıbrıs)")]
    [EnglishName("Turkish (Cyprus)")]
    [CultureInfoName("tr-CY")]
    [Description("Turkish (Cyprus)")]
    TR = 4096, // CY 4096

    /// <summary>Xitsonga (South Africa)</summary>
    [NativeName("Xitsonga (South Africa)")]
    [EnglishName("Xitsonga (South Africa)")]
    [CultureInfoName("ts-ZA")]
    [Description("Xitsonga (South Africa)")]
    TS = 1073, // ZA 1073

    /// <summary>Tatar (Russia)</summary>
    [NativeName("татар (Россия)")]
    [EnglishName("Tatar (Russia)")]
    [CultureInfoName("tt-RU")]
    [Description("Tatar (Russia)")]
    TT = 1092, // RU 1092

    /// <summary>Tasawaq (Niger)</summary>
    [NativeName("Tasawaq senni (Nižer)")]
    [EnglishName("Tasawaq (Niger)")]
    [CultureInfoName("twq-NE")]
    [Description("Tasawaq (Niger)")]
    TWQ = 4096, // NE 4096

    /// <summary>Central Atlas Tamazight (Morocco)</summary>
    [NativeName("Tamaziɣt n laṭlaṣ (Meṛṛuk)")]
    [EnglishName("Central Atlas Tamazight (Morocco)")]
    [CultureInfoName("tzm-MA")]
    [Description("Central Atlas Tamazight (Morocco)")]
    TZM = 4096, // MA 4096

    /// <summary>Uyghur (China)</summary>
    [NativeName("ئۇيغۇرچە (جۇڭگو)")]
    [EnglishName("Uyghur (China)")]
    [CultureInfoName("ug-CN")]
    [Description("Uyghur (China)")]
    UG = 1152, // CN 1152

    /// <summary>Ukrainian (Ukraine)</summary>
    [NativeName("українська (Україна)")]
    [EnglishName("Ukrainian (Ukraine)")]
    [CultureInfoName("uk-UA")]
    [Description("Ukrainian (Ukraine)")]
    UK = 1058, // UA 1058

    /// <summary>Urdu (India)</summary>
    [NativeName("اردو (بھارت)")]
    [EnglishName("Urdu (India)")]
    [CultureInfoName("ur-IN")]
    [Description("Urdu (India)")]
    UR = 2080, // IN 2080

    /// <summary>Uzbek (Arabic, Afghanistan)</summary>
    [NativeName("اوزبیک (عربی, افغانستان)")]
    [EnglishName("Uzbek (Arabic, Afghanistan)")]
    [CultureInfoName("uz-Arab-AF")]
    [Description("Uzbek (Arabic, Afghanistan)")]
    UZ = 4096, // AF 4096

    /// <summary>Vai (Latin, Liberia)</summary>
    [NativeName("Vai (Latin, Liberia)")]
    [EnglishName("Vai (Latin, Liberia)")]
    [CultureInfoName("vai-Latn-LR")]
    [Description("Vai (Latin, Liberia)")]
    VAI = 4096, // LR 4096

    /// <summary>Venda (South Africa)</summary>
    [NativeName("Venda (South Africa)")]
    [EnglishName("Venda (South Africa)")]
    [CultureInfoName("ve-ZA")]
    [Description("Venda (South Africa)")]
    VE = 1075, // ZA 1075

    /// <summary>Vietnamese (Vietnam)</summary>
    [NativeName("Tiếng Việt (Việt Nam)")]
    [EnglishName("Vietnamese (Vietnam)")]
    [CultureInfoName("vi-VN")]
    [Description("Vietnamese (Vietnam)")]
    VI = 1066, // VN 1066

    /// <summary>Volapük (World)</summary>
    [NativeName("Volapük (World)")]
    [EnglishName("Volapük (World)")]
    [CultureInfoName("vo-001")]
    [Description("Volapük (World)")]
    VO = 4096, // 001 4096

    /// <summary>Vunjo (Tanzania)</summary>
    [NativeName("Kyivunjo (Tanzania)")]
    [EnglishName("Vunjo (Tanzania)")]
    [CultureInfoName("vun-TZ")]
    [Description("Vunjo (Tanzania)")]
    VUN = 4096, // TZ 4096

    /// <summary>Walser (Switzerland)</summary>
    [NativeName("Walser (Schwiz)")]
    [EnglishName("Walser (Switzerland)")]
    [CultureInfoName("wae-CH")]
    [Description("Walser (Switzerland)")]
    WAE = 4096, // CH 4096

    /// <summary>Wolaytta (Ethiopia)</summary>
    [NativeName("Wolaytta (Ethiopia)")]
    [EnglishName("Wolaytta (Ethiopia)")]
    [CultureInfoName("wal-ET")]
    [Description("Wolaytta (Ethiopia)")]
    WAL = 4096, // ET 4096

    /// <summary>Wolof (Senegal)</summary>
    [NativeName("Wolof (Senegaal)")]
    [EnglishName("Wolof (Senegal)")]
    [CultureInfoName("wo-SN")]
    [Description("Wolof (Senegal)")]
    WO = 1160, // SN 1160

    /// <summary>isiXhosa (South Africa)</summary>
    [NativeName("isiXhosa (eMzantsi Afrika)")]
    [EnglishName("isiXhosa (South Africa)")]
    [CultureInfoName("xh-ZA")]
    [Description("isiXhosa (South Africa)")]
    XH = 1076, // ZA 1076

    /// <summary>Soga (Uganda)</summary>
    [NativeName("Olusoga (Yuganda)")]
    [EnglishName("Soga (Uganda)")]
    [CultureInfoName("xog-UG")]
    [Description("Soga (Uganda)")]
    XOG = 4096, // UG 4096

    /// <summary>Yangben (Cameroon)</summary>
    [NativeName("nuasue (Kemelún)")]
    [EnglishName("Yangben (Cameroon)")]
    [CultureInfoName("yav-CM")]
    [Description("Yangben (Cameroon)")]
    YAV = 4096, // CM 4096

    /// <summary>Yiddish (World)</summary>
    [NativeName("ייִדיש (וועלט)")]
    [EnglishName("Yiddish (World)")]
    [CultureInfoName("yi-001")]
    [Description("Yiddish (World)")]
    YI = 1085, // 001 1085

    /// <summary>Yoruba (Benin)</summary>
    [NativeName("Èdè Yorùbá (Orílɛ́ède Bɛ̀nɛ̀)")]
    [EnglishName("Yoruba (Benin)")]
    [CultureInfoName("yo-BJ")]
    [Description("Yoruba (Benin)")]
    YO = 4096, // BJ 4096

    /// <summary>Standard Moroccan Tamazight (Morocco)</summary>
    [NativeName("ⵜⴰⵎⴰⵣⵉⵖⵜ (ⵍⵎⵖⵔⵉⴱ)")]
    [EnglishName("Standard Moroccan Tamazight (Morocco)")]
    [CultureInfoName("zgh-MA")]
    [Description("Standard Moroccan Tamazight (Morocco)")]
    ZGH = 4096, // MA 4096

    /// <summary>Chinese (Simplified, China)</summary>
    [NativeName("中文（简体，中国）")]
    [EnglishName("Chinese (Simplified, China)")]
    [CultureInfoName("zh-Hans-CN")]
    [Description("Chinese (Simplified, China)")]
    ZH = 4096, // CN 4096

    /// <summary>isiZulu (South Africa)</summary>
    [NativeName("isiZulu (iNingizimu Afrika)")]
    [EnglishName("isiZulu (South Africa)")]
    [CultureInfoName("zu-ZA")]
    [Description("isiZulu (South Africa)")]
    ZU = 1077, // ZA 1077

    /// <summary>Afar (Djibouti)</summary>
    [NativeName("Afar (Djibouti)")]
    [EnglishName("Afar (Djibouti)")]
    [CultureInfoName("aa-DJ")]
    [Description("Afar (Djibouti)")]
    AA = 4096, // DJ 4096

}



