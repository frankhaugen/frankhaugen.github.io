using System.Globalization;
using System.ComponentModel;
using System.Numerics;

///<summary>Contains a list of Two-letter country (region) codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence compatible with CultureInfo</summary>
public enum Region
{
    /// <summary>Andorra</summary>
    [NativeName("Andorra")]
    [EnglishName("Andorra")]
    [Coordinates(42.546245, 1.601554)]
    [Description("Andorra")]
    AD = 8,

    /// <summary>United Arab Emirates</summary>
    [NativeName("الإمارات العربية المتحدة")]
    [EnglishName("United Arab Emirates")]
    [Coordinates(23.424076, 53.847818)]
    [Description("United Arab Emirates")]
    AE = 224,

    /// <summary>Afghanistan</summary>
    [NativeName("افغانستان")]
    [EnglishName("Afghanistan")]
    [Coordinates(33.93911, 67.709953)]
    [Description("Afghanistan")]
    AF = 3,

    /// <summary>Antigua & Barbuda</summary>
    [NativeName("Antigua & Barbuda")]
    [EnglishName("Antigua & Barbuda")]
    [Coordinates(17.060816, -61.796428)]
    [Description("Antigua & Barbuda")]
    AG = 2,

    /// <summary>Anguilla</summary>
    [NativeName("Anguilla")]
    [EnglishName("Anguilla")]
    [Coordinates(18.220554, -63.068615)]
    [Description("Anguilla")]
    AI = 300,

    /// <summary>Albania</summary>
    [NativeName("Shqipëri")]
    [EnglishName("Albania")]
    [Coordinates(41.153332, 20.168331)]
    [Description("Albania")]
    AL = 6,

    /// <summary>Armenia</summary>
    [NativeName("Հայաստան")]
    [EnglishName("Armenia")]
    [Coordinates(40.069099, 45.038189)]
    [Description("Armenia")]
    AM = 7,

    /// <summary>Angola</summary>
    [NativeName("Angóla")]
    [EnglishName("Angola")]
    [Coordinates(-11.202692, 17.873887)]
    [Description("Angola")]
    AO = 9,

    /// <summary>Argentina</summary>
    [NativeName("Argentina")]
    [EnglishName("Argentina")]
    [Coordinates(-38.416097, -63.616672)]
    [Description("Argentina")]
    AR = 11,

    /// <summary>American Samoa</summary>
    [NativeName("American Samoa")]
    [EnglishName("American Samoa")]
    [Coordinates(-14.270972, -170.132217)]
    [Description("American Samoa")]
    AS = 10,

    /// <summary>Austria</summary>
    [NativeName("Österreich")]
    [EnglishName("Austria")]
    [Coordinates(47.516231, 14.550072)]
    [Description("Austria")]
    AT = 14,

    /// <summary>Australia</summary>
    [NativeName("Australia")]
    [EnglishName("Australia")]
    [Coordinates(-25.274398, 133.775136)]
    [Description("Australia")]
    AU = 12,

    /// <summary>Aruba</summary>
    [NativeName("Aruba")]
    [EnglishName("Aruba")]
    [Coordinates(12.52111, -69.968338)]
    [Description("Aruba")]
    AW = 302,

    /// <summary>Åland Islands</summary>
    [NativeName("Åland")]
    [EnglishName("Åland Islands")]
    [Description("Åland Islands")]
    AX = 10028789,

    /// <summary>Azerbaijan</summary>
    [NativeName("Азәрбајҹан")]
    [EnglishName("Azerbaijan")]
    [Coordinates(40.143105, 47.576927)]
    [Description("Azerbaijan")]
    AZ = 5,

    /// <summary>Bosnia & Herzegovina</summary>
    [NativeName("Bosna i Hercegovina")]
    [EnglishName("Bosnia & Herzegovina")]
    [Coordinates(43.915886, 17.679076)]
    [Description("Bosnia & Herzegovina")]
    BA = 25,

    /// <summary>Barbados</summary>
    [NativeName("Barbados")]
    [EnglishName("Barbados")]
    [Coordinates(13.193887, -59.543198)]
    [Description("Barbados")]
    BB = 18,

    /// <summary>Bangladesh</summary>
    [NativeName("বাংলাদেশ")]
    [EnglishName("Bangladesh")]
    [Coordinates(23.684994, 90.356331)]
    [Description("Bangladesh")]
    BD = 23,

    /// <summary>Belgium</summary>
    [NativeName("België")]
    [EnglishName("Belgium")]
    [Coordinates(50.503887, 4.469936)]
    [Description("Belgium")]
    BE = 21,

    /// <summary>Burkina Faso</summary>
    [NativeName("Burkina Faso")]
    [EnglishName("Burkina Faso")]
    [Coordinates(12.238333, -1.561593)]
    [Description("Burkina Faso")]
    BF = 245,

    /// <summary>Bulgaria</summary>
    [NativeName("България")]
    [EnglishName("Bulgaria")]
    [Coordinates(42.733883, 25.48583)]
    [Description("Bulgaria")]
    BG = 35,

    /// <summary>Bahrain</summary>
    [NativeName("البحرين")]
    [EnglishName("Bahrain")]
    [Coordinates(25.930414, 50.637772)]
    [Description("Bahrain")]
    BH = 17,

    /// <summary>Burundi</summary>
    [NativeName("Burundi")]
    [EnglishName("Burundi")]
    [Coordinates(-3.373056, 29.918886)]
    [Description("Burundi")]
    BI = 38,

    /// <summary>Benin</summary>
    [NativeName("Bénin")]
    [EnglishName("Benin")]
    [Coordinates(9.30769, 2.315834)]
    [Description("Benin")]
    BJ = 28,

    /// <summary>St. Barthélemy</summary>
    [NativeName("Saint-Barthélemy")]
    [EnglishName("St. Barthélemy")]
    [Description("St. Barthélemy")]
    BL = 161832015,

    /// <summary>Bermuda</summary>
    [NativeName("Bermuda")]
    [EnglishName("Bermuda")]
    [Coordinates(32.321384, -64.75737)]
    [Description("Bermuda")]
    BM = 20,

    /// <summary>Brunei</summary>
    [NativeName("Brunei")]
    [EnglishName("Brunei")]
    [Coordinates(4.535277, 114.727669)]
    [Description("Brunei")]
    BN = 37,

    /// <summary>Bolivia</summary>
    [NativeName("Bolivia")]
    [EnglishName("Bolivia")]
    [Coordinates(-16.290154, -63.588653)]
    [Description("Bolivia")]
    BO = 26,

    /// <summary>Bonaire, Sint Eustatius and Saba</summary>
    [NativeName("Bonaire, Sint Eustatius en Saba")]
    [EnglishName("Bonaire, Sint Eustatius and Saba")]
    [Description("Bonaire, Sint Eustatius and Saba")]
    BQ = 161832258,

    /// <summary>Brazil</summary>
    [NativeName("Brasil")]
    [EnglishName("Brazil")]
    [Coordinates(-14.235004, -51.92528)]
    [Description("Brazil")]
    BR = 32,

    /// <summary>Bahamas</summary>
    [NativeName("Bahamas")]
    [EnglishName("Bahamas")]
    [Coordinates(25.03428, -77.39628)]
    [Description("Bahamas")]
    BS = 22,

    /// <summary>Bhutan</summary>
    [NativeName("འབྲུག")]
    [EnglishName("Bhutan")]
    [Coordinates(27.514162, 90.433601)]
    [Description("Bhutan")]
    BT = 34,

    /// <summary>Botswana</summary>
    [NativeName("Botswana")]
    [EnglishName("Botswana")]
    [Coordinates(-22.328474, 24.684866)]
    [Description("Botswana")]
    BW = 19,

    /// <summary>Belarus</summary>
    [NativeName("Беларусь")]
    [EnglishName("Belarus")]
    [Coordinates(53.709807, 27.953389)]
    [Description("Belarus")]
    BY = 29,

    /// <summary>Belize</summary>
    [NativeName("Belize")]
    [EnglishName("Belize")]
    [Coordinates(17.189877, -88.49765)]
    [Description("Belize")]
    BZ = 24,

    /// <summary>Canada</summary>
    [NativeName("Canada")]
    [EnglishName("Canada")]
    [Coordinates(56.130366, -106.346771)]
    [Description("Canada")]
    CA = 39,

    /// <summary>Cocos (Keeling) Islands</summary>
    [NativeName("Cocos (Keeling) Islands")]
    [EnglishName("Cocos (Keeling) Islands")]
    [Coordinates(-12.164165, 96.870956)]
    [Description("Cocos (Keeling) Islands")]
    CC = 311,

    /// <summary>Congo (DRC)</summary>
    [NativeName("Congo (République démocratique du)")]
    [EnglishName("Congo (DRC)")]
    [Coordinates(-4.038333, 21.758664)]
    [Description("Congo (DRC)")]
    CD = 44,

    /// <summary>Central African Republic</summary>
    [NativeName("République centrafricaine")]
    [EnglishName("Central African Republic")]
    [Coordinates(6.611111, 20.939444)]
    [Description("Central African Republic")]
    CF = 55,

    /// <summary>Congo</summary>
    [NativeName("Congo")]
    [EnglishName("Congo")]
    [Coordinates(-0.228021, 15.827659)]
    [Description("Congo")]
    CG = 43,

    /// <summary>Switzerland</summary>
    [NativeName("Schweiz")]
    [EnglishName("Switzerland")]
    [Coordinates(46.818188, 8.227512)]
    [Description("Switzerland")]
    CH = 223,

    /// <summary>Côte d’Ivoire</summary>
    [NativeName("Côte d’Ivoire")]
    [EnglishName("Côte d’Ivoire")]
    [Coordinates(7.539989, -5.54708)]
    [Description("Côte d’Ivoire")]
    CI = 119,

    /// <summary>Cook Islands</summary>
    [NativeName("Cook Islands")]
    [EnglishName("Cook Islands")]
    [Coordinates(-21.236736, -159.777671)]
    [Description("Cook Islands")]
    CK = 312,

    /// <summary>Chile</summary>
    [NativeName("Chile")]
    [EnglishName("Chile")]
    [Coordinates(-35.675147, -71.542969)]
    [Description("Chile")]
    CL = 46,

    /// <summary>Cameroon</summary>
    [NativeName("Kàmàlûŋ")]
    [EnglishName("Cameroon")]
    [Coordinates(7.369722, 12.354722)]
    [Description("Cameroon")]
    CM = 49,

    /// <summary>China</summary>
    [NativeName("རྒྱ་ནག")]
    [EnglishName("China")]
    [Coordinates(35.86166, 104.195397)]
    [Description("China")]
    CN = 45,

    /// <summary>Colombia</summary>
    [NativeName("Colombia")]
    [EnglishName("Colombia")]
    [Coordinates(4.570868, -74.297333)]
    [Description("Colombia")]
    CO = 51,

    /// <summary>Costa Rica</summary>
    [NativeName("Costa Rica")]
    [EnglishName("Costa Rica")]
    [Coordinates(9.748917, -83.753428)]
    [Description("Costa Rica")]
    CR = 54,

    /// <summary>Serbia</summary>
    [NativeName("Србија")]
    [EnglishName("Serbia")]
    [Description("Serbia")]
    CS = 269,

    /// <summary>Cuba</summary>
    [NativeName("Cuba")]
    [EnglishName("Cuba")]
    [Coordinates(21.521757, -77.781167)]
    [Description("Cuba")]
    CU = 56,

    /// <summary>Cabo Verde</summary>
    [NativeName("Kabu Verdi")]
    [EnglishName("Cabo Verde")]
    [Coordinates(16.002082, -24.013197)]
    [Description("Cabo Verde")]
    CV = 57,

    /// <summary>Curaçao</summary>
    [NativeName("Curaçao")]
    [EnglishName("Curaçao")]
    [Description("Curaçao")]
    CW = 273,

    /// <summary>Christmas Island</summary>
    [NativeName("Christmas Island")]
    [EnglishName("Christmas Island")]
    [Coordinates(-10.447525, 105.690449)]
    [Description("Christmas Island")]
    CX = 309,

    /// <summary>Cyprus</summary>
    [NativeName("Κύπρος")]
    [EnglishName("Cyprus")]
    [Coordinates(35.126413, 33.429859)]
    [Description("Cyprus")]
    CY = 59,

    /// <summary>Czechia</summary>
    [NativeName("Česko")]
    [EnglishName("Czechia")]
    [Coordinates(49.817492, 15.472962)]
    [Description("Czechia")]
    CZ = 75,

    /// <summary>Germany</summary>
    [NativeName("Deutschland")]
    [EnglishName("Germany")]
    [Coordinates(51.165691, 10.451526)]
    [Description("Germany")]
    DE = 94,

    /// <summary>Djibouti</summary>
    [NativeName("Djibouti")]
    [EnglishName("Djibouti")]
    [Coordinates(11.825138, 42.590275)]
    [Description("Djibouti")]
    DJ = 62,

    /// <summary>Denmark</summary>
    [NativeName("Danmark")]
    [EnglishName("Denmark")]
    [Coordinates(56.26392, 9.501785)]
    [Description("Denmark")]
    DK = 61,

    /// <summary>Dominica</summary>
    [NativeName("Dominica")]
    [EnglishName("Dominica")]
    [Coordinates(15.414999, -61.370976)]
    [Description("Dominica")]
    DM = 63,

    /// <summary>Dominican Republic</summary>
    [NativeName("República Dominicana")]
    [EnglishName("Dominican Republic")]
    [Coordinates(18.735693, -70.162651)]
    [Description("Dominican Republic")]
    DO = 65,

    /// <summary>Algeria</summary>
    [NativeName("الجزائر")]
    [EnglishName("Algeria")]
    [Coordinates(28.033886, 1.659626)]
    [Description("Algeria")]
    DZ = 4,

    /// <summary>Ecuador</summary>
    [NativeName("Ecuador")]
    [EnglishName("Ecuador")]
    [Coordinates(-1.831239, -78.183406)]
    [Description("Ecuador")]
    EC = 66,

    /// <summary>Estonia</summary>
    [NativeName("Eesti")]
    [EnglishName("Estonia")]
    [Coordinates(58.595272, 25.013607)]
    [Description("Estonia")]
    EE = 70,

    /// <summary>Egypt</summary>
    [NativeName("مصر")]
    [EnglishName("Egypt")]
    [Coordinates(26.820553, 30.802498)]
    [Description("Egypt")]
    EG = 67,

    /// <summary>Eritrea</summary>
    [NativeName("Eritrea")]
    [EnglishName("Eritrea")]
    [Coordinates(15.179384, 39.782334)]
    [Description("Eritrea")]
    ER = 71,

    /// <summary>Spain</summary>
    [NativeName("España")]
    [EnglishName("Spain")]
    [Coordinates(40.463667, -3.74922)]
    [Description("Spain")]
    ES = 217,

    /// <summary>Ethiopia</summary>
    [NativeName("Ethiopia")]
    [EnglishName("Ethiopia")]
    [Coordinates(9.145, 40.489673)]
    [Description("Ethiopia")]
    ET = 73,

    /// <summary>Finland</summary>
    [NativeName("Finland")]
    [EnglishName("Finland")]
    [Coordinates(61.92411, 25.748151)]
    [Description("Finland")]
    FI = 77,

    /// <summary>Fiji</summary>
    [NativeName("Fiji")]
    [EnglishName("Fiji")]
    [Coordinates(-16.578193, 179.414413)]
    [Description("Fiji")]
    FJ = 78,

    /// <summary>Falkland Islands</summary>
    [NativeName("Falkland Islands")]
    [EnglishName("Falkland Islands")]
    [Coordinates(-51.796253, -59.523613)]
    [Description("Falkland Islands")]
    FK = 315,

    /// <summary>Micronesia</summary>
    [NativeName("Micronesia")]
    [EnglishName("Micronesia")]
    [Coordinates(7.425554, 150.550812)]
    [Description("Micronesia")]
    FM = 80,

    /// <summary>Faroe Islands</summary>
    [NativeName("Føroyar")]
    [EnglishName("Faroe Islands")]
    [Coordinates(61.892635, -6.911806)]
    [Description("Faroe Islands")]
    FO = 81,

    /// <summary>France</summary>
    [NativeName("Frañs")]
    [EnglishName("France")]
    [Coordinates(46.227638, 2.213749)]
    [Description("France")]
    FR = 84,

    /// <summary>Gabon</summary>
    [NativeName("Gabon")]
    [EnglishName("Gabon")]
    [Coordinates(-0.803689, 11.609444)]
    [Description("Gabon")]
    GA = 87,

    /// <summary>United Kingdom</summary>
    [NativeName("Y Deyrnas Unedig")]
    [EnglishName("United Kingdom")]
    [Coordinates(55.378051, -3.435973)]
    [Description("United Kingdom")]
    GB = 242,

    /// <summary>Grenada</summary>
    [NativeName("Grenada")]
    [EnglishName("Grenada")]
    [Coordinates(12.262776, -61.604171)]
    [Description("Grenada")]
    GD = 91,

    /// <summary>Georgia</summary>
    [NativeName("საქართველო")]
    [EnglishName("Georgia")]
    [Coordinates(42.315407, 43.356892)]
    [Description("Georgia")]
    GE = 88,

    /// <summary>French Guiana</summary>
    [NativeName("Guyane française")]
    [EnglishName("French Guiana")]
    [Coordinates(3.933889, -53.125782)]
    [Description("French Guiana")]
    GF = 317,

    /// <summary>Guernsey</summary>
    [NativeName("Guernsey")]
    [EnglishName("Guernsey")]
    [Coordinates(49.465691, -2.585278)]
    [Description("Guernsey")]
    GG = 324,

    /// <summary>Ghana</summary>
    [NativeName("Gaana")]
    [EnglishName("Ghana")]
    [Coordinates(7.946527, -1.023194)]
    [Description("Ghana")]
    GH = 89,

    /// <summary>Gibraltar</summary>
    [NativeName("Gibraltar")]
    [EnglishName("Gibraltar")]
    [Coordinates(36.137741, -5.345374)]
    [Description("Gibraltar")]
    GI = 90,

    /// <summary>Greenland</summary>
    [NativeName("Grønland")]
    [EnglishName("Greenland")]
    [Coordinates(71.706936, -42.604303)]
    [Description("Greenland")]
    GL = 93,

    /// <summary>Gambia</summary>
    [NativeName("Gambia")]
    [EnglishName("Gambia")]
    [Coordinates(13.443182, -15.310139)]
    [Description("Gambia")]
    GM = 86,

    /// <summary>Guinea</summary>
    [NativeName("Guinée")]
    [EnglishName("Guinea")]
    [Coordinates(9.945587, -9.696645)]
    [Description("Guinea")]
    GN = 100,

    /// <summary>Guadeloupe</summary>
    [NativeName("Guadeloupe")]
    [EnglishName("Guadeloupe")]
    [Coordinates(16.995971, -62.067641)]
    [Description("Guadeloupe")]
    GP = 321,

    /// <summary>Equatorial Guinea</summary>
    [NativeName("Guinea Ecuatorial")]
    [EnglishName("Equatorial Guinea")]
    [Coordinates(1.650801, 10.267895)]
    [Description("Equatorial Guinea")]
    GQ = 69,

    /// <summary>Greece</summary>
    [NativeName("Ελλάδα")]
    [EnglishName("Greece")]
    [Coordinates(39.074208, 21.824312)]
    [Description("Greece")]
    GR = 98,

    /// <summary>Guatemala</summary>
    [NativeName("Guatemala")]
    [EnglishName("Guatemala")]
    [Coordinates(15.783471, -90.230759)]
    [Description("Guatemala")]
    GT = 99,

    /// <summary>Guam</summary>
    [NativeName("Guam")]
    [EnglishName("Guam")]
    [Coordinates(13.444304, 144.793731)]
    [Description("Guam")]
    GU = 322,

    /// <summary>Guinea-Bissau</summary>
    [NativeName("Guiné-Bissau")]
    [EnglishName("Guinea-Bissau")]
    [Coordinates(11.803749, -15.180413)]
    [Description("Guinea-Bissau")]
    GW = 196,

    /// <summary>Guyana</summary>
    [NativeName("Guyana")]
    [EnglishName("Guyana")]
    [Coordinates(4.860416, -58.93018)]
    [Description("Guyana")]
    GY = 101,

    /// <summary>Hong Kong SAR</summary>
    [NativeName("Hong Kong SAR")]
    [EnglishName("Hong Kong SAR")]
    [Coordinates(22.396428, 114.109497)]
    [Description("Hong Kong SAR")]
    HK = 104,

    /// <summary>Honduras</summary>
    [NativeName("Honduras")]
    [EnglishName("Honduras")]
    [Coordinates(15.199999, -86.241905)]
    [Description("Honduras")]
    HN = 106,

    /// <summary>Croatia</summary>
    [NativeName("Hrvatska")]
    [EnglishName("Croatia")]
    [Coordinates(45.1, 15.2)]
    [Description("Croatia")]
    HR = 108,

    /// <summary>Haiti</summary>
    [NativeName("Haïti")]
    [EnglishName("Haiti")]
    [Coordinates(18.971187, -72.285215)]
    [Description("Haiti")]
    HT = 103,

    /// <summary>Hungary</summary>
    [NativeName("Magyarország")]
    [EnglishName("Hungary")]
    [Coordinates(47.162494, 19.503304)]
    [Description("Hungary")]
    HU = 109,

    /// <summary>Indonesia</summary>
    [NativeName("Indonesia")]
    [EnglishName("Indonesia")]
    [Coordinates(-0.789275, 113.921327)]
    [Description("Indonesia")]
    ID = 111,

    /// <summary>Ireland</summary>
    [NativeName("Ireland")]
    [EnglishName("Ireland")]
    [Coordinates(53.41291, -8.24389)]
    [Description("Ireland")]
    IE = 68,

    /// <summary>Israel</summary>
    [NativeName("إسرائيل")]
    [EnglishName("Israel")]
    [Coordinates(31.046051, 34.851612)]
    [Description("Israel")]
    IL = 117,

    /// <summary>Isle of Man</summary>
    [NativeName("Isle of Man")]
    [EnglishName("Isle of Man")]
    [Coordinates(54.236107, -4.548056)]
    [Description("Isle of Man")]
    IM = 15126,

    /// <summary>India</summary>
    [NativeName("ভাৰত")]
    [EnglishName("India")]
    [Coordinates(20.593684, 78.96288)]
    [Description("India")]
    IN = 113,

    /// <summary>British Indian Ocean Territory</summary>
    [NativeName("British Indian Ocean Territory")]
    [EnglishName("British Indian Ocean Territory")]
    [Coordinates(-6.343194, 71.876519)]
    [Description("British Indian Ocean Territory")]
    IO = 114,

    /// <summary>Iraq</summary>
    [NativeName("العراق")]
    [EnglishName("Iraq")]
    [Coordinates(33.223191, 43.679291)]
    [Description("Iraq")]
    IQ = 121,

    /// <summary>Iran</summary>
    [NativeName("ایران")]
    [EnglishName("Iran")]
    [Coordinates(32.427908, 53.688046)]
    [Description("Iran")]
    IR = 116,

    /// <summary>Iceland</summary>
    [NativeName("Ísland")]
    [EnglishName("Iceland")]
    [Coordinates(64.963051, -19.020835)]
    [Description("Iceland")]
    IS = 110,

    /// <summary>Italy</summary>
    [NativeName("Itàlia")]
    [EnglishName("Italy")]
    [Coordinates(41.87194, 12.56738)]
    [Description("Italy")]
    IT = 118,

    /// <summary>Jersey</summary>
    [NativeName("Jersey")]
    [EnglishName("Jersey")]
    [Coordinates(49.214439, -2.13125)]
    [Description("Jersey")]
    JE = 328,

    /// <summary>Jamaica</summary>
    [NativeName("Jamaica")]
    [EnglishName("Jamaica")]
    [Coordinates(18.109581, -77.297508)]
    [Description("Jamaica")]
    JM = 124,

    /// <summary>Jordan</summary>
    [NativeName("الأردن")]
    [EnglishName("Jordan")]
    [Coordinates(30.585164, 36.238414)]
    [Description("Jordan")]
    JO = 126,

    /// <summary>Japan</summary>
    [NativeName("日本")]
    [EnglishName("Japan")]
    [Coordinates(36.204824, 138.252924)]
    [Description("Japan")]
    JP = 122,

    /// <summary>Kenya</summary>
    [NativeName("Kenya")]
    [EnglishName("Kenya")]
    [Coordinates(-0.023559, 37.906193)]
    [Description("Kenya")]
    KE = 129,

    /// <summary>Kyrgyzstan</summary>
    [NativeName("Кыргызстан")]
    [EnglishName("Kyrgyzstan")]
    [Coordinates(41.20438, 74.766098)]
    [Description("Kyrgyzstan")]
    KG = 130,

    /// <summary>Cambodia</summary>
    [NativeName("កម្ពុជា")]
    [EnglishName("Cambodia")]
    [Coordinates(12.565679, 104.990963)]
    [Description("Cambodia")]
    KH = 40,

    /// <summary>Kiribati</summary>
    [NativeName("Kiribati")]
    [EnglishName("Kiribati")]
    [Coordinates(-3.370417, -168.734039)]
    [Description("Kiribati")]
    KI = 133,

    /// <summary>Comoros</summary>
    [NativeName("جزر القمر")]
    [EnglishName("Comoros")]
    [Coordinates(-11.875001, 43.872219)]
    [Description("Comoros")]
    KM = 50,

    /// <summary>St. Kitts & Nevis</summary>
    [NativeName("St. Kitts & Nevis")]
    [EnglishName("St. Kitts & Nevis")]
    [Coordinates(17.357822, -62.782998)]
    [Description("St. Kitts & Nevis")]
    KN = 207,

    /// <summary>North Korea</summary>
    [NativeName("조선민주주의인민공화국")]
    [EnglishName("North Korea")]
    [Coordinates(40.339852, 127.510093)]
    [Description("North Korea")]
    KP = 131,

    /// <summary>Korea</summary>
    [NativeName("대한민국")]
    [EnglishName("Korea")]
    [Coordinates(35.907757, 127.766922)]
    [Description("Korea")]
    KR = 134,

    /// <summary>Kuwait</summary>
    [NativeName("الكويت")]
    [EnglishName("Kuwait")]
    [Coordinates(29.31166, 47.481766)]
    [Description("Kuwait")]
    KW = 136,

    /// <summary>Cayman Islands</summary>
    [NativeName("Cayman Islands")]
    [EnglishName("Cayman Islands")]
    [Coordinates(19.513469, -80.566956)]
    [Description("Cayman Islands")]
    KY = 307,

    /// <summary>Kazakhstan</summary>
    [NativeName("Қазақстан")]
    [EnglishName("Kazakhstan")]
    [Coordinates(48.019573, 66.923684)]
    [Description("Kazakhstan")]
    KZ = 137,

    /// <summary>Laos</summary>
    [NativeName("ລາວ")]
    [EnglishName("Laos")]
    [Coordinates(19.85627, 102.495496)]
    [Description("Laos")]
    LA = 138,

    /// <summary>Lebanon</summary>
    [NativeName("لبنان")]
    [EnglishName("Lebanon")]
    [Coordinates(33.854721, 35.862285)]
    [Description("Lebanon")]
    LB = 139,

    /// <summary>St. Lucia</summary>
    [NativeName("St. Lucia")]
    [EnglishName("St. Lucia")]
    [Coordinates(13.909444, -60.978893)]
    [Description("St. Lucia")]
    LC = 218,

    /// <summary>Liechtenstein</summary>
    [NativeName("Liechtenstein")]
    [EnglishName("Liechtenstein")]
    [Coordinates(47.166, 9.555373)]
    [Description("Liechtenstein")]
    LI = 145,

    /// <summary>Sri Lanka</summary>
    [NativeName("ශ්‍රී ලංකාව")]
    [EnglishName("Sri Lanka")]
    [Coordinates(7.873054, 80.771797)]
    [Description("Sri Lanka")]
    LK = 42,

    /// <summary>Liberia</summary>
    [NativeName("Liberia")]
    [EnglishName("Liberia")]
    [Coordinates(6.428055, -9.429499)]
    [Description("Liberia")]
    LR = 142,

    /// <summary>Lesotho</summary>
    [NativeName("Lesotho")]
    [EnglishName("Lesotho")]
    [Coordinates(-29.609988, 28.233608)]
    [Description("Lesotho")]
    LS = 146,

    /// <summary>Lithuania</summary>
    [NativeName("Lietuva")]
    [EnglishName("Lithuania")]
    [Coordinates(55.169438, 23.881275)]
    [Description("Lithuania")]
    LT = 141,

    /// <summary>Luxembourg</summary>
    [NativeName("Luxemburg")]
    [EnglishName("Luxembourg")]
    [Coordinates(49.815273, 6.129583)]
    [Description("Luxembourg")]
    LU = 147,

    /// <summary>Latvia</summary>
    [NativeName("Latvija")]
    [EnglishName("Latvia")]
    [Coordinates(56.879635, 24.603189)]
    [Description("Latvia")]
    LV = 140,

    /// <summary>Libya</summary>
    [NativeName("ليبيا")]
    [EnglishName("Libya")]
    [Coordinates(26.3351, 17.228331)]
    [Description("Libya")]
    LY = 148,

    /// <summary>Morocco</summary>
    [NativeName("المغرب")]
    [EnglishName("Morocco")]
    [Coordinates(31.791702, -7.09262)]
    [Description("Morocco")]
    MA = 159,

    /// <summary>Monaco</summary>
    [NativeName("Monaco")]
    [EnglishName("Monaco")]
    [Coordinates(43.750298, 7.412841)]
    [Description("Monaco")]
    MC = 158,

    /// <summary>Moldova</summary>
    [NativeName("Republica Moldova")]
    [EnglishName("Moldova")]
    [Coordinates(47.411631, 28.369885)]
    [Description("Moldova")]
    MD = 152,

    /// <summary>Montenegro</summary>
    [NativeName("Црна Гора")]
    [EnglishName("Montenegro")]
    [Coordinates(42.708678, 19.37439)]
    [Description("Montenegro")]
    ME = 270,

    /// <summary>St. Martin</summary>
    [NativeName("Saint-Martin")]
    [EnglishName("St. Martin")]
    [Description("St. Martin")]
    MF = 31706,

    /// <summary>Madagascar</summary>
    [NativeName("Madagascar")]
    [EnglishName("Madagascar")]
    [Coordinates(-18.766947, 46.869107)]
    [Description("Madagascar")]
    MG = 149,

    /// <summary>Marshall Islands</summary>
    [NativeName("Marshall Islands")]
    [EnglishName("Marshall Islands")]
    [Coordinates(7.131474, 171.184478)]
    [Description("Marshall Islands")]
    MH = 199,

    /// <summary>North Macedonia</summary>
    [NativeName("Северна Македонија")]
    [EnglishName("North Macedonia")]
    [Coordinates(41.608635, 21.745275)]
    [Description("North Macedonia")]
    MK = 19618,

    /// <summary>Mali</summary>
    [NativeName("Mali")]
    [EnglishName("Mali")]
    [Coordinates(17.570692, -3.996166)]
    [Description("Mali")]
    ML = 244,

    /// <summary>Mali</summary>
    [NativeName("Mali")]
    [EnglishName("Mali")]
    [Coordinates(17.570692, -3.996166)]
    [Description("Mali")]
    ML = 157,

    /// <summary>Myanmar</summary>
    [NativeName("မြန်မာ")]
    [EnglishName("Myanmar")]
    [Coordinates(21.913965, 95.956223)]
    [Description("Myanmar")]
    MM = 27,

    /// <summary>Mongolia</summary>
    [NativeName("Монгол")]
    [EnglishName("Mongolia")]
    [Coordinates(46.862496, 103.846656)]
    [Description("Mongolia")]
    MN = 154,

    /// <summary>Macao SAR</summary>
    [NativeName("Macao SAR")]
    [EnglishName("Macao SAR")]
    [Coordinates(22.198745, 113.543873)]
    [Description("Macao SAR")]
    MO = 151,

    /// <summary>Northern Mariana Islands</summary>
    [NativeName("Northern Mariana Islands")]
    [EnglishName("Northern Mariana Islands")]
    [Coordinates(17.33083, 145.38469)]
    [Description("Northern Mariana Islands")]
    MP = 337,

    /// <summary>Martinique</summary>
    [NativeName("Martinique")]
    [EnglishName("Martinique")]
    [Coordinates(14.641528, -61.024174)]
    [Description("Martinique")]
    MQ = 330,

    /// <summary>Mauritania</summary>
    [NativeName("موريتانيا")]
    [EnglishName("Mauritania")]
    [Coordinates(21.00789, -10.940835)]
    [Description("Mauritania")]
    MR = 162,

    /// <summary>Montserrat</summary>
    [NativeName("Montserrat")]
    [EnglishName("Montserrat")]
    [Coordinates(16.742498, -62.187366)]
    [Description("Montserrat")]
    MS = 332,

    /// <summary>Malta</summary>
    [NativeName("Malta")]
    [EnglishName("Malta")]
    [Coordinates(35.937496, 14.375416)]
    [Description("Malta")]
    MT = 163,

    /// <summary>Mauritius</summary>
    [NativeName("Mauritius")]
    [EnglishName("Mauritius")]
    [Coordinates(-20.348404, 57.552152)]
    [Description("Mauritius")]
    MU = 160,

    /// <summary>Maldives</summary>
    [NativeName("Maldives")]
    [EnglishName("Maldives")]
    [Coordinates(3.202778, 73.22068)]
    [Description("Maldives")]
    MV = 165,

    /// <summary>Malawi</summary>
    [NativeName("Malawi")]
    [EnglishName("Malawi")]
    [Coordinates(-13.254308, 34.301525)]
    [Description("Malawi")]
    MW = 156,

    /// <summary>Mexico</summary>
    [NativeName("México")]
    [EnglishName("Mexico")]
    [Coordinates(23.634501, -102.552784)]
    [Description("Mexico")]
    MX = 166,

    /// <summary>Malaysia</summary>
    [NativeName("Malaysia")]
    [EnglishName("Malaysia")]
    [Coordinates(4.210484, 101.975766)]
    [Description("Malaysia")]
    MY = 167,

    /// <summary>Mozambique</summary>
    [NativeName("Umozambiki")]
    [EnglishName("Mozambique")]
    [Coordinates(-18.665695, 35.529562)]
    [Description("Mozambique")]
    MZ = 168,

    /// <summary>Namibia</summary>
    [NativeName("Namibië")]
    [EnglishName("Namibia")]
    [Coordinates(-22.95764, 18.49041)]
    [Description("Namibia")]
    NA = 254,

    /// <summary>New Caledonia</summary>
    [NativeName("Nouvelle-Calédonie")]
    [EnglishName("New Caledonia")]
    [Coordinates(-20.904305, 165.618042)]
    [Description("New Caledonia")]
    NC = 334,

    /// <summary>Niger</summary>
    [NativeName("Nižer")]
    [EnglishName("Niger")]
    [Coordinates(17.607789, 8.081666)]
    [Description("Niger")]
    NE = 173,

    /// <summary>Norfolk Island</summary>
    [NativeName("Norfolk Island")]
    [EnglishName("Norfolk Island")]
    [Coordinates(-29.040835, 167.954712)]
    [Description("Norfolk Island")]
    NF = 336,

    /// <summary>Nigeria</summary>
    [NativeName("Nigeria")]
    [EnglishName("Nigeria")]
    [Coordinates(9.081999, 8.675277)]
    [Description("Nigeria")]
    NG = 175,

    /// <summary>Nicaragua</summary>
    [NativeName("Nicaragua")]
    [EnglishName("Nicaragua")]
    [Coordinates(12.865416, -85.207229)]
    [Description("Nicaragua")]
    NI = 182,

    /// <summary>Netherlands</summary>
    [NativeName("Netherlands")]
    [EnglishName("Netherlands")]
    [Coordinates(52.132633, 5.291266)]
    [Description("Netherlands")]
    NL = 176,

    /// <summary>Norway</summary>
    [NativeName("Norge")]
    [EnglishName("Norway")]
    [Coordinates(60.472024, 8.468946)]
    [Description("Norway")]
    NO = 177,

    /// <summary>Nepal</summary>
    [NativeName("नेपाल")]
    [EnglishName("Nepal")]
    [Coordinates(28.394857, 84.124008)]
    [Description("Nepal")]
    NP = 178,

    /// <summary>Nauru</summary>
    [NativeName("Nauru")]
    [EnglishName("Nauru")]
    [Coordinates(-0.522778, 166.931503)]
    [Description("Nauru")]
    NR = 180,

    /// <summary>Niue</summary>
    [NativeName("Niue")]
    [EnglishName("Niue")]
    [Coordinates(-19.054445, -169.867233)]
    [Description("Niue")]
    NU = 335,

    /// <summary>New Zealand</summary>
    [NativeName("New Zealand")]
    [EnglishName("New Zealand")]
    [Coordinates(-40.900557, 174.885971)]
    [Description("New Zealand")]
    NZ = 183,

    /// <summary>Oman</summary>
    [NativeName("عُمان")]
    [EnglishName("Oman")]
    [Coordinates(21.512583, 55.923255)]
    [Description("Oman")]
    OM = 164,

    /// <summary>Panama</summary>
    [NativeName("Panamá")]
    [EnglishName("Panama")]
    [Coordinates(8.537981, -80.782127)]
    [Description("Panama")]
    PA = 192,

    /// <summary>Peru</summary>
    [NativeName("Perú")]
    [EnglishName("Peru")]
    [Coordinates(-9.189967, -75.015152)]
    [Description("Peru")]
    PE = 187,

    /// <summary>French Polynesia</summary>
    [NativeName("Polynésie française")]
    [EnglishName("French Polynesia")]
    [Coordinates(-17.679742, -149.406843)]
    [Description("French Polynesia")]
    PF = 318,

    /// <summary>Papua New Guinea</summary>
    [NativeName("Papua New Guinea")]
    [EnglishName("Papua New Guinea")]
    [Coordinates(-6.314993, 143.95555)]
    [Description("Papua New Guinea")]
    PG = 194,

    /// <summary>Philippines</summary>
    [NativeName("Philippines")]
    [EnglishName("Philippines")]
    [Coordinates(12.879721, 121.774017)]
    [Description("Philippines")]
    PH = 201,

    /// <summary>Pakistan</summary>
    [NativeName("Pakistan")]
    [EnglishName("Pakistan")]
    [Coordinates(30.375321, 69.345116)]
    [Description("Pakistan")]
    PK = 190,

    /// <summary>Poland</summary>
    [NativeName("Polska")]
    [EnglishName("Poland")]
    [Coordinates(51.919438, 19.145136)]
    [Description("Poland")]
    PL = 191,

    /// <summary>St. Pierre & Miquelon</summary>
    [NativeName("Saint-Pierre-et-Miquelon")]
    [EnglishName("St. Pierre & Miquelon")]
    [Coordinates(46.941936, -56.27111)]
    [Description("St. Pierre & Miquelon")]
    PM = 206,

    /// <summary>Pitcairn Islands</summary>
    [NativeName("Pitcairn Islands")]
    [EnglishName("Pitcairn Islands")]
    [Coordinates(-24.703615, -127.439308)]
    [Description("Pitcairn Islands")]
    PN = 339,

    /// <summary>Puerto Rico</summary>
    [NativeName("Puerto Rico")]
    [EnglishName("Puerto Rico")]
    [Coordinates(18.220833, -66.590149)]
    [Description("Puerto Rico")]
    PR = 202,

    /// <summary>Palestinian Authority</summary>
    [NativeName("السلطة الفلسطينية")]
    [EnglishName("Palestinian Authority")]
    [Coordinates(31.952162, 35.233154)]
    [Description("Palestinian Authority")]
    PS = 184,

    /// <summary>Portugal</summary>
    [NativeName("Portugal")]
    [EnglishName("Portugal")]
    [Coordinates(39.399872, -8.224454)]
    [Description("Portugal")]
    PT = 193,

    /// <summary>Palau</summary>
    [NativeName("Palau")]
    [EnglishName("Palau")]
    [Coordinates(7.51498, 134.58252)]
    [Description("Palau")]
    PW = 195,

    /// <summary>Paraguay</summary>
    [NativeName("Paraguay")]
    [EnglishName("Paraguay")]
    [Coordinates(-23.442503, -58.443832)]
    [Description("Paraguay")]
    PY = 185,

    /// <summary>Qatar</summary>
    [NativeName("قطر")]
    [EnglishName("Qatar")]
    [Coordinates(25.354826, 51.183884)]
    [Description("Qatar")]
    QA = 197,

    /// <summary>Réunion</summary>
    [NativeName("La Réunion")]
    [EnglishName("Réunion")]
    [Coordinates(-21.115141, 55.536384)]
    [Description("Réunion")]
    RE = 198,

    /// <summary>Romania</summary>
    [NativeName("România")]
    [EnglishName("Romania")]
    [Coordinates(45.943161, 24.96676)]
    [Description("Romania")]
    RO = 200,

    /// <summary>Serbia</summary>
    [NativeName("Србија")]
    [EnglishName("Serbia")]
    [Coordinates(44.016521, 21.005859)]
    [Description("Serbia")]
    RS = 271,

    /// <summary>Russia</summary>
    [NativeName("Russia")]
    [EnglishName("Russia")]
    [Coordinates(61.52401, 105.318756)]
    [Description("Russia")]
    RU = 203,

    /// <summary>Rwanda</summary>
    [NativeName("Rwanda")]
    [EnglishName("Rwanda")]
    [Coordinates(-1.940278, 29.873888)]
    [Description("Rwanda")]
    RW = 204,

    /// <summary>Saudi Arabia</summary>
    [NativeName("المملكة العربية السعودية")]
    [EnglishName("Saudi Arabia")]
    [Coordinates(23.885942, 45.079162)]
    [Description("Saudi Arabia")]
    SA = 205,

    /// <summary>Solomon Islands</summary>
    [NativeName("Solomon Islands")]
    [EnglishName("Solomon Islands")]
    [Coordinates(-9.64571, 160.156194)]
    [Description("Solomon Islands")]
    SB = 30,

    /// <summary>Seychelles</summary>
    [NativeName("Seychelles")]
    [EnglishName("Seychelles")]
    [Coordinates(-4.679574, 55.491977)]
    [Description("Seychelles")]
    SC = 208,

    /// <summary>Sudan</summary>
    [NativeName("السودان")]
    [EnglishName("Sudan")]
    [Coordinates(12.862807, 30.217636)]
    [Description("Sudan")]
    SD = 219,

    /// <summary>Sweden</summary>
    [NativeName("Sweden")]
    [EnglishName("Sweden")]
    [Coordinates(60.128161, 18.643501)]
    [Description("Sweden")]
    SE = 221,

    /// <summary>Singapore</summary>
    [NativeName("Singapore")]
    [EnglishName("Singapore")]
    [Coordinates(1.352083, 103.819836)]
    [Description("Singapore")]
    SG = 215,

    /// <summary>St Helena, Ascension, Tristan da Cunha</summary>
    [NativeName("St Helena, Ascension, Tristan da Cunha")]
    [EnglishName("St Helena, Ascension, Tristan da Cunha")]
    [Coordinates(-24.143474, -10.030696)]
    [Description("St Helena, Ascension, Tristan da Cunha")]
    SH = 343,

    /// <summary>Slovenia</summary>
    [NativeName("Slovenia")]
    [EnglishName("Slovenia")]
    [Coordinates(46.151241, 14.995463)]
    [Description("Slovenia")]
    SI = 212,

    /// <summary>Svalbard & Jan Mayen</summary>
    [NativeName("Svalbard og Jan Mayen")]
    [EnglishName("Svalbard & Jan Mayen")]
    [Coordinates(77.553604, 23.670272)]
    [Description("Svalbard & Jan Mayen")]
    SJ = 220,

    /// <summary>Slovakia</summary>
    [NativeName("Slovensko")]
    [EnglishName("Slovakia")]
    [Coordinates(48.669026, 19.699024)]
    [Description("Slovakia")]
    SK = 143,

    /// <summary>Sierra Leone</summary>
    [NativeName("Sierra Leone")]
    [EnglishName("Sierra Leone")]
    [Coordinates(8.460555, -11.779889)]
    [Description("Sierra Leone")]
    SL = 213,

    /// <summary>San Marino</summary>
    [NativeName("San Marino")]
    [EnglishName("San Marino")]
    [Coordinates(43.94236, 12.457777)]
    [Description("San Marino")]
    SM = 214,

    /// <summary>Senegal</summary>
    [NativeName("Senegal")]
    [EnglishName("Senegal")]
    [Coordinates(14.497401, -14.452362)]
    [Description("Senegal")]
    SN = 210,

    /// <summary>Somalia</summary>
    [NativeName("الصومال")]
    [EnglishName("Somalia")]
    [Coordinates(5.152149, 46.199616)]
    [Description("Somalia")]
    SO = 216,

    /// <summary>Suriname</summary>
    [NativeName("Suriname")]
    [EnglishName("Suriname")]
    [Coordinates(3.919305, -56.027783)]
    [Description("Suriname")]
    SR = 181,

    /// <summary>South Sudan</summary>
    [NativeName("جنوب السودان")]
    [EnglishName("South Sudan")]
    [Description("South Sudan")]
    SS = 276,

    /// <summary>São Tomé & Príncipe</summary>
    [NativeName("São Tomé e Príncipe")]
    [EnglishName("São Tomé & Príncipe")]
    [Coordinates(0.18636, 6.613081)]
    [Description("São Tomé & Príncipe")]
    ST = 233,

    /// <summary>El Salvador</summary>
    [NativeName("El Salvador")]
    [EnglishName("El Salvador")]
    [Coordinates(13.794185, -88.89653)]
    [Description("El Salvador")]
    SV = 72,

    /// <summary>Sint Maarten</summary>
    [NativeName("Sint Maarten")]
    [EnglishName("Sint Maarten")]
    [Description("Sint Maarten")]
    SX = 30967,

    /// <summary>Syria</summary>
    [NativeName("سوريا")]
    [EnglishName("Syria")]
    [Coordinates(34.802075, 38.996815)]
    [Description("Syria")]
    SY = 222,

    /// <summary>Eswatini</summary>
    [NativeName("Eswatini")]
    [EnglishName("Eswatini")]
    [Coordinates(-26.522503, 31.465866)]
    [Description("Eswatini")]
    SZ = 260,

    /// <summary>Turks & Caicos Islands</summary>
    [NativeName("Turks & Caicos Islands")]
    [EnglishName("Turks & Caicos Islands")]
    [Coordinates(21.694025, -71.797928)]
    [Description("Turks & Caicos Islands")]
    TC = 349,

    /// <summary>Chad</summary>
    [NativeName("تشاد")]
    [EnglishName("Chad")]
    [Coordinates(15.454166, 18.732207)]
    [Description("Chad")]
    TD = 41,

    /// <summary>Togo</summary>
    [NativeName("Togo nutome")]
    [EnglishName("Togo")]
    [Coordinates(8.619543, 0.824782)]
    [Description("Togo")]
    TG = 232,

    /// <summary>Thailand</summary>
    [NativeName("ไทย")]
    [EnglishName("Thailand")]
    [Coordinates(15.870032, 100.992541)]
    [Description("Thailand")]
    TH = 227,

    /// <summary>Tokelau</summary>
    [NativeName("Tokelau")]
    [EnglishName("Tokelau")]
    [Coordinates(-8.967363, -171.855881)]
    [Description("Tokelau")]
    TK = 347,

    /// <summary>Timor-Leste</summary>
    [NativeName("Timor-Leste")]
    [EnglishName("Timor-Leste")]
    [Coordinates(-8.874217, 125.727539)]
    [Description("Timor-Leste")]
    TL = 7299303,

    /// <summary>Turkmenistan</summary>
    [NativeName("Türkmenistan")]
    [EnglishName("Turkmenistan")]
    [Coordinates(38.969719, 59.556278)]
    [Description("Turkmenistan")]
    TM = 238,

    /// <summary>Tunisia</summary>
    [NativeName("تونس")]
    [EnglishName("Tunisia")]
    [Coordinates(33.886917, 9.537499)]
    [Description("Tunisia")]
    TN = 234,

    /// <summary>Tonga</summary>
    [NativeName("Tonga")]
    [EnglishName("Tonga")]
    [Coordinates(-21.178986, -175.198242)]
    [Description("Tonga")]
    TO = 231,

    /// <summary>Turkey</summary>
    [NativeName("Türkiye")]
    [EnglishName("Turkey")]
    [Coordinates(38.963745, 35.243322)]
    [Description("Turkey")]
    TR = 235,

    /// <summary>Trinidad & Tobago</summary>
    [NativeName("Trinidad & Tobago")]
    [EnglishName("Trinidad & Tobago")]
    [Coordinates(10.691803, -61.222503)]
    [Description("Trinidad & Tobago")]
    TT = 225,

    /// <summary>Tuvalu</summary>
    [NativeName("Tuvalu")]
    [EnglishName("Tuvalu")]
    [Coordinates(-7.109535, 177.64933)]
    [Description("Tuvalu")]
    TV = 236,

    /// <summary>Tanzania</summary>
    [NativeName("Tadhania")]
    [EnglishName("Tanzania")]
    [Coordinates(-6.369028, 34.888822)]
    [Description("Tanzania")]
    TZ = 239,

    /// <summary>Ukraine</summary>
    [NativeName("Украина")]
    [EnglishName("Ukraine")]
    [Coordinates(48.379433, 31.16558)]
    [Description("Ukraine")]
    UA = 241,

    /// <summary>Uganda</summary>
    [NativeName("Uganda")]
    [EnglishName("Uganda")]
    [Coordinates(1.373333, 32.290275)]
    [Description("Uganda")]
    UG = 240,

    /// <summary>U.S. Outlying Islands</summary>
    [NativeName("U.S. Outlying Islands")]
    [EnglishName("U.S. Outlying Islands")]
    [Description("U.S. Outlying Islands")]
    UM = 161832256,

    /// <summary>Uruguay</summary>
    [NativeName("Uruguay")]
    [EnglishName("Uruguay")]
    [Coordinates(-32.522779, -55.765835)]
    [Description("Uruguay")]
    UY = 246,

    /// <summary>Uzbekistan</summary>
    [NativeName("Ўзбекистон")]
    [EnglishName("Uzbekistan")]
    [Coordinates(41.377491, 64.585262)]
    [Description("Uzbekistan")]
    UZ = 247,

    /// <summary>St. Vincent & Grenadines</summary>
    [NativeName("St. Vincent & Grenadines")]
    [EnglishName("St. Vincent & Grenadines")]
    [Coordinates(12.984305, -61.287228)]
    [Description("St. Vincent & Grenadines")]
    VC = 248,

    /// <summary>Venezuela</summary>
    [NativeName("Venezuela")]
    [EnglishName("Venezuela")]
    [Coordinates(6.42375, -66.58973)]
    [Description("Venezuela")]
    VE = 249,

    /// <summary>British Virgin Islands</summary>
    [NativeName("British Virgin Islands")]
    [EnglishName("British Virgin Islands")]
    [Coordinates(18.420695, -64.639968)]
    [Description("British Virgin Islands")]
    VG = 351,

    /// <summary>U.S. Virgin Islands</summary>
    [NativeName("U.S. Virgin Islands")]
    [EnglishName("U.S. Virgin Islands")]
    [Coordinates(18.335765, -64.896335)]
    [Description("U.S. Virgin Islands")]
    VI = 252,

    /// <summary>Vietnam</summary>
    [NativeName("Việt Nam")]
    [EnglishName("Vietnam")]
    [Coordinates(14.058324, 108.277199)]
    [Description("Vietnam")]
    VN = 251,

    /// <summary>Vanuatu</summary>
    [NativeName("Vanuatu")]
    [EnglishName("Vanuatu")]
    [Coordinates(-15.376706, 166.959158)]
    [Description("Vanuatu")]
    VU = 174,

    /// <summary>Wallis & Futuna</summary>
    [NativeName("Wallis-et-Futuna")]
    [EnglishName("Wallis & Futuna")]
    [Coordinates(-13.768752, -177.156097)]
    [Description("Wallis & Futuna")]
    WF = 352,

    /// <summary>Samoa</summary>
    [NativeName("Samoa")]
    [EnglishName("Samoa")]
    [Coordinates(-13.759029, -172.104629)]
    [Description("Samoa")]
    WS = 259,

    /// <summary>Kosovo</summary>
    [NativeName("Kosovë")]
    [EnglishName("Kosovo")]
    [Coordinates(42.602636, 20.902977)]
    [Description("Kosovo")]
    XK = 9914689,

    /// <summary>Yemen</summary>
    [NativeName("اليمن")]
    [EnglishName("Yemen")]
    [Coordinates(15.552727, 48.516388)]
    [Description("Yemen")]
    YE = 261,

    /// <summary>Mayotte</summary>
    [NativeName("Mayotte")]
    [EnglishName("Mayotte")]
    [Coordinates(-12.8275, 45.166244)]
    [Description("Mayotte")]
    YT = 331,

    /// <summary>South Africa</summary>
    [NativeName("Suid-Afrika")]
    [EnglishName("South Africa")]
    [Coordinates(-30.559482, 22.937506)]
    [Description("South Africa")]
    ZA = 209,

    /// <summary>Zambia</summary>
    [NativeName("Zambia")]
    [EnglishName("Zambia")]
    [Coordinates(-13.133897, 27.849332)]
    [Description("Zambia")]
    ZM = 263,

    /// <summary>Zimbabwe</summary>
    [NativeName("Zimbabwe")]
    [EnglishName("Zimbabwe")]
    [Coordinates(-19.015438, 29.154857)]
    [Description("Zimbabwe")]
    ZW = 264,

}
