using System.ComponentModel;
using System.Globalization;

namespace GeneratedEnums
{
    // Only for LinqPad debugging purposes
    public class Program { public static void Main() => Culture.NB.GetCultureInfo().NumberFormat.Dump();}

    public static class CultureExtensions { public static CultureInfo GetCultureInfo(this Culture culture) => CultureInfo.GetCultureInfo(culture.ToString()); }

    ///<summary>Contains a list of .net cultures to simplify use of CultureInfo</summary>
    public enum Culture
    {
        /// <summary>Invariant Language (Invariant Country)</summary>
        [Description("Invariant Language (Invariant Country)")]
        IV = 0,

        /// <summary>Afar</summary>
        [Description("Afar")]
        AA = 1,

        /// <summary>Afrikaans</summary>
        [Description("Afrikaans")]
        AF = 2,

        /// <summary>Aghem</summary>
        [Description("Aghem")]
        AGQ = 3,

        /// <summary>Akan</summary>
        [Description("Akan")]
        AK = 4,

        /// <summary>Amharic</summary>
        [Description("አማርኛ")]
        AM = 5,

        /// <summary>Arabic</summary>
        [Description("العربية")]
        AR = 6,

        /// <summary>Mapuche</summary>
        [Description("Mapudungun")]
        ARN = 7,

        /// <summary>Assamese</summary>
        [Description("অসমীয়া")]
        AS = 8,

        /// <summary>Asu</summary>
        [Description("Kipare")]
        ASA = 9,

        /// <summary>Asturian</summary>
        [Description("asturianu")]
        AST = 10,

        /// <summary>Azerbaijani</summary>
        [Description("azərbaycan")]
        AZ = 11,

        /// <summary>Bashkir</summary>
        [Description("Bashkir")]
        BA = 12,

        /// <summary>Basaa</summary>
        [Description("Ɓàsàa")]
        BAS = 13,

        /// <summary>Belarusian</summary>
        [Description("беларуская")]
        BE = 14,

        /// <summary>Bemba</summary>
        [Description("Ichibemba")]
        BEM = 15,

        /// <summary>Bena</summary>
        [Description("Hibena")]
        BEZ = 16,

        /// <summary>Bulgarian</summary>
        [Description("български")]
        BG = 17,

        /// <summary>Bamanankan</summary>
        [Description("bamanakan")]
        BM = 18,

        /// <summary>Bangla</summary>
        [Description("বাংলা")]
        BN = 19,

        /// <summary>Tibetan</summary>
        [Description("བོད་སྐད་")]
        BO = 20,

        /// <summary>Breton</summary>
        [Description("brezhoneg")]
        BR = 21,

        /// <summary>Bodo</summary>
        [Description("बड़ो")]
        BRX = 22,

        /// <summary>Bosnian</summary>
        [Description("bosanski")]
        BS = 23,

        /// <summary>Blin</summary>
        [Description("Blin")]
        BYN = 24,

        /// <summary>Catalan</summary>
        [Description("català")]
        CA = 25,

        /// <summary>Chakma</summary>
        [Description("𑄌𑄋𑄴𑄟𑄳𑄦")]
        CCP = 26,

        /// <summary>Chechen</summary>
        [Description("нохчийн")]
        CE = 27,

        /// <summary>Cebuano</summary>
        [Description("Cebuano")]
        CEB = 28,

        /// <summary>Chiga</summary>
        [Description("Rukiga")]
        CGG = 29,

        /// <summary>Cherokee</summary>
        [Description("ᏣᎳᎩ")]
        CHR = 30,

        /// <summary>Central Kurdish</summary>
        [Description("کوردیی ناوەندی")]
        CKB = 31,

        /// <summary>Corsican</summary>
        [Description("Corsican")]
        CO = 32,

        /// <summary>Czech</summary>
        [Description("čeština")]
        CS = 33,

        /// <summary>Church Slavic</summary>
        [Description("Church Slavic")]
        CU = 34,

        /// <summary>Welsh</summary>
        [Description("Cymraeg")]
        CY = 35,

        /// <summary>Danish</summary>
        [Description("dansk")]
        DA = 36,

        /// <summary>Taita</summary>
        [Description("Kitaita")]
        DAV = 37,

        /// <summary>German</summary>
        [Description("Deutsch")]
        DE = 38,

        /// <summary>Zarma</summary>
        [Description("Zarmaciine")]
        DJE = 39,

        /// <summary>Lower Sorbian</summary>
        [Description("dolnoserbšćina")]
        DSB = 40,

        /// <summary>Duala</summary>
        [Description("duálá")]
        DUA = 41,

        /// <summary>Divehi</summary>
        [Description("Divehi")]
        DV = 42,

        /// <summary>Jola-Fonyi</summary>
        [Description("joola")]
        DYO = 43,

        /// <summary>Dzongkha</summary>
        [Description("རྫོང་ཁ")]
        DZ = 44,

        /// <summary>Embu</summary>
        [Description("Kĩembu")]
        EBU = 45,

        /// <summary>Ewe</summary>
        [Description("Eʋegbe")]
        EE = 46,

        /// <summary>Greek</summary>
        [Description("Ελληνικά")]
        EL = 47,

        /// <summary>English</summary>
        [Description("English")]
        EN = 48,

        /// <summary>Esperanto</summary>
        [Description("esperanto")]
        EO = 49,

        /// <summary>Spanish</summary>
        [Description("español")]
        ES = 50,

        /// <summary>Estonian</summary>
        [Description("eesti")]
        ET = 51,

        /// <summary>Basque</summary>
        [Description("euskara")]
        EU = 52,

        /// <summary>Ewondo</summary>
        [Description("ewondo")]
        EWO = 53,

        /// <summary>Persian</summary>
        [Description("فارسی")]
        FA = 54,

        /// <summary>Fulah</summary>
        [Description("Pulaar")]
        FF = 55,

        /// <summary>Finnish</summary>
        [Description("suomi")]
        FI = 56,

        /// <summary>Filipino</summary>
        [Description("Filipino")]
        FIL = 57,

        /// <summary>Faroese</summary>
        [Description("føroyskt")]
        FO = 58,

        /// <summary>French</summary>
        [Description("français")]
        FR = 59,

        /// <summary>Friulian</summary>
        [Description("furlan")]
        FUR = 60,

        /// <summary>Western Frisian</summary>
        [Description("Frysk")]
        FY = 61,

        /// <summary>Irish</summary>
        [Description("Gaeilge")]
        GA = 62,

        /// <summary>Scottish Gaelic</summary>
        [Description("Gàidhlig")]
        GD = 63,

        /// <summary>Galician</summary>
        [Description("galego")]
        GL = 64,

        /// <summary>Guarani</summary>
        [Description("Guarani")]
        GN = 65,

        /// <summary>Swiss German</summary>
        [Description("Schwiizertüütsch")]
        GSW = 66,

        /// <summary>Gujarati</summary>
        [Description("ગુજરાતી")]
        GU = 67,

        /// <summary>Gusii</summary>
        [Description("Ekegusii")]
        GUZ = 68,

        /// <summary>Manx</summary>
        [Description("Gaelg")]
        GV = 69,

        /// <summary>Hausa</summary>
        [Description("Hausa")]
        HA = 70,

        /// <summary>Hawaiian</summary>
        [Description("ʻŌlelo Hawaiʻi")]
        HAW = 71,

        /// <summary>Hebrew</summary>
        [Description("עברית")]
        HE = 72,

        /// <summary>Hindi</summary>
        [Description("हिन्दी")]
        HI = 73,

        /// <summary>Croatian</summary>
        [Description("hrvatski")]
        HR = 74,

        /// <summary>Upper Sorbian</summary>
        [Description("hornjoserbšćina")]
        HSB = 75,

        /// <summary>Hungarian</summary>
        [Description("magyar")]
        HU = 76,

        /// <summary>Armenian</summary>
        [Description("հայերեն")]
        HY = 77,

        /// <summary>Interlingua</summary>
        [Description("interlingua")]
        IA = 78,

        /// <summary>Indonesian</summary>
        [Description("Indonesia")]
        ID = 79,

        /// <summary>Igbo</summary>
        [Description("Asụsụ Igbo")]
        IG = 80,

        /// <summary>Yi</summary>
        [Description("ꆈꌠꉙ")]
        II = 81,

        /// <summary>Icelandic</summary>
        [Description("íslenska")]
        IS = 82,

        /// <summary>Italian</summary>
        [Description("italiano")]
        IT = 83,

        /// <summary>Inuktitut</summary>
        [Description("Inuktitut")]
        IU = 84,

        /// <summary>Japanese</summary>
        [Description("日本語")]
        JA = 85,

        /// <summary>Ngomba</summary>
        [Description("Ndaꞌa")]
        JGO = 86,

        /// <summary>Machame</summary>
        [Description("Kimachame")]
        JMC = 87,

        /// <summary>Javanese</summary>
        [Description("Jawa")]
        JV = 88,

        /// <summary>Georgian</summary>
        [Description("ქართული")]
        KA = 89,

        /// <summary>Kabyle</summary>
        [Description("Taqbaylit")]
        KAB = 90,

        /// <summary>Kamba</summary>
        [Description("Kikamba")]
        KAM = 91,

        /// <summary>Makonde</summary>
        [Description("Chimakonde")]
        KDE = 92,

        /// <summary>Kabuverdianu</summary>
        [Description("kabuverdianu")]
        KEA = 93,

        /// <summary>Koyra Chiini</summary>
        [Description("Koyra ciini")]
        KHQ = 94,

        /// <summary>Kikuyu</summary>
        [Description("Gikuyu")]
        KI = 95,

        /// <summary>Kazakh</summary>
        [Description("қазақ тілі")]
        KK = 96,

        /// <summary>Kako</summary>
        [Description("kakɔ")]
        KKJ = 97,

        /// <summary>Kalaallisut</summary>
        [Description("kalaallisut")]
        KL = 98,

        /// <summary>Kalenjin</summary>
        [Description("Kalenjin")]
        KLN = 99,

        /// <summary>Khmer</summary>
        [Description("ខ្មែរ")]
        KM = 100,

        /// <summary>Kannada</summary>
        [Description("ಕನ್ನಡ")]
        KN = 101,

        /// <summary>Korean</summary>
        [Description("한국어")]
        KO = 102,

        /// <summary>Konkani</summary>
        [Description("कोंकणी")]
        KOK = 103,

        /// <summary>Kashmiri</summary>
        [Description("کٲشُر")]
        KS = 104,

        /// <summary>Shambala</summary>
        [Description("Kishambaa")]
        KSB = 105,

        /// <summary>Bafia</summary>
        [Description("rikpa")]
        KSF = 106,

        /// <summary>Colognian</summary>
        [Description("Kölsch")]
        KSH = 107,

        /// <summary>Cornish</summary>
        [Description("kernewek")]
        KW = 108,

        /// <summary>Kyrgyz</summary>
        [Description("кыргызча")]
        KY = 109,

        /// <summary>Langi</summary>
        [Description("Kɨlaangi")]
        LAG = 110,

        /// <summary>Luxembourgish</summary>
        [Description("Lëtzebuergesch")]
        LB = 111,

        /// <summary>Ganda</summary>
        [Description("Luganda")]
        LG = 112,

        /// <summary>Lakota</summary>
        [Description("Lakȟólʼiyapi")]
        LKT = 113,

        /// <summary>Lingala</summary>
        [Description("lingála")]
        LN = 114,

        /// <summary>Lao</summary>
        [Description("ລາວ")]
        LO = 115,

        /// <summary>Northern Luri</summary>
        [Description("لۊری شومالی")]
        LRC = 116,

        /// <summary>Lithuanian</summary>
        [Description("lietuvių")]
        LT = 117,

        /// <summary>Luba-Katanga</summary>
        [Description("Tshiluba")]
        LU = 118,

        /// <summary>Luo</summary>
        [Description("Dholuo")]
        LUO = 119,

        /// <summary>Luyia</summary>
        [Description("Luluhia")]
        LUY = 120,

        /// <summary>Latvian</summary>
        [Description("latviešu")]
        LV = 121,

        /// <summary>Masai</summary>
        [Description("Maa")]
        MAS = 122,

        /// <summary>Meru</summary>
        [Description("Kĩmĩrũ")]
        MER = 123,

        /// <summary>Morisyen</summary>
        [Description("kreol morisien")]
        MFE = 124,

        /// <summary>Malagasy</summary>
        [Description("Malagasy")]
        MG = 125,

        /// <summary>Makhuwa-Meetto</summary>
        [Description("Makua")]
        MGH = 126,

        /// <summary>Metaʼ</summary>
        [Description("metaʼ")]
        MGO = 127,

        /// <summary>Maori</summary>
        [Description("Māori")]
        MI = 128,

        /// <summary>Macedonian</summary>
        [Description("македонски")]
        MK = 129,

        /// <summary>Malayalam</summary>
        [Description("മലയാളം")]
        ML = 130,

        /// <summary>Mongolian</summary>
        [Description("монгол")]
        MN = 131,

        /// <summary>Mohawk</summary>
        [Description("Kanienʼkéha")]
        MOH = 132,

        /// <summary>Marathi</summary>
        [Description("मराठी")]
        MR = 133,

        /// <summary>Malay</summary>
        [Description("Melayu")]
        MS = 134,

        /// <summary>Maltese</summary>
        [Description("Malti")]
        MT = 135,

        /// <summary>Mundang</summary>
        [Description("MUNDAŊ")]
        MUA = 136,

        /// <summary>Burmese</summary>
        [Description("မြန်မာ")]
        MY = 137,

        /// <summary>Mazanderani</summary>
        [Description("مازرونی")]
        MZN = 138,

        /// <summary>Nama</summary>
        [Description("Khoekhoegowab")]
        NAQ = 139,

        /// <summary>Norwegian Bokmål</summary>
        [Description("norsk bokmål")]
        NB = 140,

        /// <summary>North Ndebele</summary>
        [Description("isiNdebele")]
        ND = 141,

        /// <summary>Low German</summary>
        [Description("Low German")]
        NDS = 142,

        /// <summary>Nepali</summary>
        [Description("नेपाली")]
        NE = 143,

        /// <summary>Dutch</summary>
        [Description("Nederlands")]
        NL = 144,

        /// <summary>Kwasio</summary>
        [Description("Kwasio")]
        NMG = 145,

        /// <summary>Norwegian Nynorsk</summary>
        [Description("nynorsk")]
        NN = 146,

        /// <summary>Ngiemboon</summary>
        [Description("Shwóŋò ngiembɔɔn")]
        NNH = 147,

        /// <summary>N’Ko</summary>
        [Description("N’Ko")]
        NQO = 148,

        /// <summary>South Ndebele</summary>
        [Description("South Ndebele")]
        NR = 149,

        /// <summary>Sesotho sa Leboa</summary>
        [Description("Sesotho sa Leboa")]
        NSO = 150,

        /// <summary>Nuer</summary>
        [Description("Thok Nath")]
        NUS = 151,

        /// <summary>Nyankole</summary>
        [Description("Runyankore")]
        NYN = 152,

        /// <summary>Occitan</summary>
        [Description("Occitan")]
        OC = 153,

        /// <summary>Oromo</summary>
        [Description("Oromoo")]
        OM = 154,

        /// <summary>Odia</summary>
        [Description("ଓଡ଼ିଆ")]
        OR = 155,

        /// <summary>Ossetic</summary>
        [Description("ирон")]
        OS = 156,

        /// <summary>Punjabi</summary>
        [Description("ਪੰਜਾਬੀ")]
        PA = 157,

        /// <summary>Polish</summary>
        [Description("polski")]
        PL = 158,

        /// <summary>Prussian</summary>
        [Description("prūsiskan")]
        PRG = 159,

        /// <summary>Pashto</summary>
        [Description("پښتو")]
        PS = 160,

        /// <summary>Portuguese</summary>
        [Description("português")]
        PT = 161,

        /// <summary>Quechua</summary>
        [Description("Runasimi")]
        QU = 162,

        /// <summary>Kʼicheʼ</summary>
        [Description("Kʼicheʼ")]
        QUC = 163,

        /// <summary>Romansh</summary>
        [Description("rumantsch")]
        RM = 164,

        /// <summary>Rundi</summary>
        [Description("Ikirundi")]
        RN = 165,

        /// <summary>Romanian</summary>
        [Description("română")]
        RO = 166,

        /// <summary>Rombo</summary>
        [Description("Kihorombo")]
        ROF = 167,

        /// <summary>Russian</summary>
        [Description("русский")]
        RU = 168,

        /// <summary>Kinyarwanda</summary>
        [Description("Kinyarwanda")]
        RW = 169,

        /// <summary>Rwa</summary>
        [Description("Kiruwa")]
        RWK = 170,

        /// <summary>Sanskrit</summary>
        [Description("संस्कृत भाषा")]
        SA = 171,

        /// <summary>Sakha</summary>
        [Description("саха тыла")]
        SAH = 172,

        /// <summary>Samburu</summary>
        [Description("Kisampur")]
        SAQ = 173,

        /// <summary>Sangu</summary>
        [Description("Ishisangu")]
        SBP = 174,

        /// <summary>Sindhi</summary>
        [Description("سنڌي")]
        SD = 175,

        /// <summary>Northern Sami</summary>
        [Description("davvisámegiella")]
        SE = 176,

        /// <summary>Sena</summary>
        [Description("sena")]
        SEH = 177,

        /// <summary>Koyraboro Senni</summary>
        [Description("Koyraboro senni")]
        SES = 178,

        /// <summary>Sango</summary>
        [Description("Sängö")]
        SG = 179,

        /// <summary>Tachelhit</summary>
        [Description("ⵜⴰⵛⵍⵃⵉⵜ")]
        SHI = 180,

        /// <summary>Sinhala</summary>
        [Description("සිංහල")]
        SI = 181,

        /// <summary>Slovak</summary>
        [Description("slovenčina")]
        SK = 182,

        /// <summary>Slovenian</summary>
        [Description("slovenščina")]
        SL = 183,

        /// <summary>Southern Sami</summary>
        [Description("Åarjelsaemien gïele")]
        SMA = 184,

        /// <summary>Lule Sami</summary>
        [Description("julevsámegiella")]
        SMJ = 185,

        /// <summary>Inari Sami</summary>
        [Description("anarâškielâ")]
        SMN = 186,

        /// <summary>Skolt Sami</summary>
        [Description("Skolt Sami")]
        SMS = 187,

        /// <summary>Shona</summary>
        [Description("chiShona")]
        SN = 188,

        /// <summary>Somali</summary>
        [Description("Soomaali")]
        SO = 189,

        /// <summary>Albanian</summary>
        [Description("shqip")]
        SQ = 190,

        /// <summary>Serbian</summary>
        [Description("српски")]
        SR = 191,

        /// <summary>siSwati</summary>
        [Description("siSwati")]
        SS = 192,

        /// <summary>Saho</summary>
        [Description("Saho")]
        SSY = 193,

        /// <summary>Sesotho</summary>
        [Description("Sesotho")]
        ST = 194,

        /// <summary>Swedish</summary>
        [Description("svenska")]
        SV = 195,

        /// <summary>Kiswahili</summary>
        [Description("Kiswahili")]
        SW = 196,

        /// <summary>Syriac</summary>
        [Description("Syriac")]
        SYR = 197,

        /// <summary>Tamil</summary>
        [Description("தமிழ்")]
        TA = 198,

        /// <summary>Telugu</summary>
        [Description("తెలుగు")]
        TE = 199,

        /// <summary>Teso</summary>
        [Description("Kiteso")]
        TEO = 200,

        /// <summary>Tajik</summary>
        [Description("тоҷикӣ")]
        TG = 201,

        /// <summary>Thai</summary>
        [Description("ไทย")]
        TH = 202,

        /// <summary>Tigrinya</summary>
        [Description("ትግርኛ")]
        TI = 203,

        /// <summary>Tigre</summary>
        [Description("Tigre")]
        TIG = 204,

        /// <summary>Turkmen</summary>
        [Description("türkmen dili")]
        TK = 205,

        /// <summary>Setswana</summary>
        [Description("Setswana")]
        TN = 206,

        /// <summary>Tongan</summary>
        [Description("lea fakatonga")]
        TO = 207,

        /// <summary>Turkish</summary>
        [Description("Türkçe")]
        TR = 208,

        /// <summary>Xitsonga</summary>
        [Description("Xitsonga")]
        TS = 209,

        /// <summary>Tatar</summary>
        [Description("татар")]
        TT = 210,

        /// <summary>Tasawaq</summary>
        [Description("Tasawaq senni")]
        TWQ = 211,

        /// <summary>Central Atlas Tamazight</summary>
        [Description("Tamaziɣt n laṭlaṣ")]
        TZM = 212,

        /// <summary>Uyghur</summary>
        [Description("ئۇيغۇرچە")]
        UG = 213,

        /// <summary>Ukrainian</summary>
        [Description("українська")]
        UK = 214,

        /// <summary>Urdu</summary>
        [Description("اردو")]
        UR = 215,

        /// <summary>Uzbek</summary>
        [Description("o‘zbek")]
        UZ = 216,

        /// <summary>Vai</summary>
        [Description("ꕙꔤ")]
        VAI = 217,

        /// <summary>Venda</summary>
        [Description("Venda")]
        VE = 218,

        /// <summary>Vietnamese</summary>
        [Description("Tiếng Việt")]
        VI = 219,

        /// <summary>Volapük</summary>
        [Description("Volapük")]
        VO = 220,

        /// <summary>Vunjo</summary>
        [Description("Kyivunjo")]
        VUN = 221,

        /// <summary>Walser</summary>
        [Description("Walser")]
        WAE = 222,

        /// <summary>Wolaytta</summary>
        [Description("Wolaytta")]
        WAL = 223,

        /// <summary>Wolof</summary>
        [Description("Wolof")]
        WO = 224,

        /// <summary>isiXhosa</summary>
        [Description("isiXhosa")]
        XH = 225,

        /// <summary>Soga</summary>
        [Description("Olusoga")]
        XOG = 226,

        /// <summary>Yangben</summary>
        [Description("nuasue")]
        YAV = 227,

        /// <summary>Yiddish</summary>
        [Description("ייִדיש")]
        YI = 228,

        /// <summary>Yoruba</summary>
        [Description("Èdè Yorùbá")]
        YO = 229,

        /// <summary>Standard Moroccan Tamazight</summary>
        [Description("ⵜⴰⵎⴰⵣⵉⵖⵜ")]
        ZGH = 230,

        /// <summary>Chinese</summary>
        [Description("中文")]
        ZH = 231,

        /// <summary>isiZulu</summary>
        [Description("isiZulu")]
        ZU = 232,

    }

}
