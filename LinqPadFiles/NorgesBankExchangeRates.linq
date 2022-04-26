<Query Kind="Statements">
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>RestSharp</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>CsvHelper.Expressions</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
  <Namespace>RestSharp</Namespace>
  <Namespace>RestSharp.Authenticators</Namespace>
  <Namespace>RestSharp.Authenticators.OAuth</Namespace>
  <Namespace>RestSharp.Authenticators.OAuth2</Namespace>
  <Namespace>RestSharp.Extensions</Namespace>
  <Namespace>RestSharp.Serializers</Namespace>
  <Namespace>RestSharp.Serializers.Json</Namespace>
  <Namespace>RestSharp.Serializers.Xml</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

var fromDate = new DateOnly(2022, 2,17);
var toDate = new DateOnly(2022, 2,18);

var url = new Uri($"https://data.norges-bank.no/api/data/EXR/B..NOK.SP?startPeriod={fromDate.ToString("yyyy-MM-dd")}&endPeriod={toDate.ToString("yyyy-MM-dd")}&format=csv&bom=include&locale=iv");

var client = new RestClient();
var request = new RestRequest(url);
var response = await client.DownloadDataAsync(request);

var csvString = Encoding.UTF8.GetString(response);
csvString.Dump();
var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
	HasHeaderRecord = true,
	Delimiter = ";",
	HeaderValidated = null,
	MissingFieldFound = null
};

using (var reader = new StringReader(csvString))
using (var csv = new CsvReader(reader, config))
{
	csv.Context.RegisterClassMap<ExchangeRateClassMap>();	
	var records = csv.GetRecords<ExchangeRate>();
	records.ToList().Dump();
}

public class ExchangeRate
{
	public string FrequencyCode { get; set; }
	public string FrequencyName { get; set; }
	public string BaseCurrencyCode { get; set; }
	public string BaseCurrencyName { get; set; }
	public string CurrencyCode { get; set; }
	public string CurrencyName { get; set; }
	public string TenorCode { get; set; }
	public string TenorName { get; set; }
	public int DecimalPrecision { get; set; }
	public bool IsCalculated { get; set; }
	public int UnitTypeCode { get; set; }
	public string UnitTypeName { get; set; }
	public string CollectionCode { get; set; }
	public string CollectedFromAt { get; set; }
	public DateTime ValidFromDate { get; set; }
	public decimal Rate { get; set; }
}

public class ExchangeRateClassMap : ClassMap<ExchangeRate>
{
	public ExchangeRateClassMap()
	{
		Map(m => m.FrequencyCode).Name("FREQ");
		Map(m => m.FrequencyName).Name("Frequency");
		Map(m => m.BaseCurrencyCode).Name("BASE_CUR");
		Map(m => m.BaseCurrencyName).Name("Base Currency");
		Map(m => m.CurrencyCode).Name("QUOTE_CUR");
		Map(m => m.CurrencyName).Name("Quote Currency");
		Map(m => m.TenorCode).Name("TENOR");
		Map(m => m.TenorName).Name("Tenor");
		Map(m => m.DecimalPrecision).Name("DECIMALS");
		Map(m => m.IsCalculated).Name("CALCULATED");
		Map(m => m.UnitTypeCode).Name("UNIT_MULT");
		Map(m => m.UnitTypeName).Name("Unit Multiplier");
		Map(m => m.CollectionCode).Name("COLLECTION");
		Map(m => m.CollectedFromAt).Name("Collection Indicator");
		Map(m => m.ValidFromDate).Name("TIME_PERIOD");
		Map(m => m.Rate).Name("OBS_VALUE");
	}
}

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

public static class CurrencyExtensions
{
	public static string GetNativeName(this Currency currency) => (currency.GetType().GetMember(currency.ToString())[0].GetCustomAttributes(typeof(NativeNameAttribute), false)[0] as NativeNameAttribute)?.LocalName ?? string.Empty;
	public static string GetEnglishName(this Currency currency) => (currency.GetType().GetMember(currency.ToString())[0].GetCustomAttributes(typeof(EnglishNameAttribute), false)[0] as EnglishNameAttribute)?.EnglishName ?? string.Empty;
}

///<summary>Contains a list of currency codes stored in .net BCL. Enum integer value is based on universal GeoId to support a standardized numbersequence</summary>
public enum Currency
{
	/// <summary>East Caribbean Dollar</summary>
	[NativeName("East Caribbean Dollar")]
	[EnglishName("East Caribbean Dollar")]
	XCD = 2,

	/// <summary>Afghan Afghani</summary>
	[NativeName("افغانی افغانستان")]
	[EnglishName("Afghan Afghani")]
	AFN = 3,

	/// <summary>Algerian Dinar</summary>
	[NativeName("دينار جزائري")]
	[EnglishName("Algerian Dinar")]
	DZD = 4,

	/// <summary>Azerbaijani Manat</summary>
	[NativeName("AZN")]
	[EnglishName("Azerbaijani Manat")]
	AZN = 5,

	/// <summary>Albanian Lek</summary>
	[NativeName("Leku shqiptar")]
	[EnglishName("Albanian Lek")]
	ALL = 6,

	/// <summary>Armenian Dram</summary>
	[NativeName("հայկական դրամ")]
	[EnglishName("Armenian Dram")]
	AMD = 7,

	/// <summary>Angolan Kwanza</summary>
	[NativeName("Kwanza ya Angóla")]
	[EnglishName("Angolan Kwanza")]
	AOA = 9,

	/// <summary>US Dollar</summary>
	[NativeName("US Dollar")]
	[EnglishName("US Dollar")]
	USD = 10,

	/// <summary>Argentine Peso</summary>
	[NativeName("peso argentino")]
	[EnglishName("Argentine Peso")]
	ARS = 11,

	/// <summary>Australian Dollar</summary>
	[NativeName("Australian Dollar")]
	[EnglishName("Australian Dollar")]
	AUD = 12,

	/// <summary>Bahraini Dinar</summary>
	[NativeName("دينار بحريني")]
	[EnglishName("Bahraini Dinar")]
	BHD = 17,

	/// <summary>Barbadian Dollar</summary>
	[NativeName("Barbadian Dollar")]
	[EnglishName("Barbadian Dollar")]
	BBD = 18,

	/// <summary>Botswanan Pula</summary>
	[NativeName("Botswanan Pula")]
	[EnglishName("Botswanan Pula")]
	BWP = 19,

	/// <summary>Bermudan Dollar</summary>
	[NativeName("Bermudan Dollar")]
	[EnglishName("Bermudan Dollar")]
	BMD = 20,

	/// <summary>Bahamian Dollar</summary>
	[NativeName("Bahamian Dollar")]
	[EnglishName("Bahamian Dollar")]
	BSD = 22,

	/// <summary>Bangladeshi Taka</summary>
	[NativeName("বাংলাদেশী টাকা")]
	[EnglishName("Bangladeshi Taka")]
	BDT = 23,

	/// <summary>Belize Dollar</summary>
	[NativeName("Belize Dollar")]
	[EnglishName("Belize Dollar")]
	BZD = 24,

	/// <summary>Bosnia-Herzegovina Convertible Mark</summary>
	[NativeName("Bosanskohercegovačka konvertibilna marka")]
	[EnglishName("Bosnia-Herzegovina Convertible Mark")]
	BAM = 25,

	/// <summary>Bolivian Boliviano</summary>
	[NativeName("boliviano")]
	[EnglishName("Bolivian Boliviano")]
	BOB = 26,

	/// <summary>Myanmar Kyat</summary>
	[NativeName("မြန်မာ ကျပ်")]
	[EnglishName("Myanmar Kyat")]
	MMK = 27,

	/// <summary>Belarusian Ruble</summary>
	[NativeName("беларускі рубель")]
	[EnglishName("Belarusian Ruble")]
	BYN = 29,

	/// <summary>Solomon Islands Dollar</summary>
	[NativeName("Solomon Islands Dollar")]
	[EnglishName("Solomon Islands Dollar")]
	SBD = 30,

	/// <summary>Brazilian Real</summary>
	[NativeName("Real brasileiro")]
	[EnglishName("Brazilian Real")]
	BRL = 32,

	/// <summary>Bhutanese Ngultrum</summary>
	[NativeName("དངུལ་ཀྲམ")]
	[EnglishName("Bhutanese Ngultrum")]
	BTN = 34,

	/// <summary>Bulgarian Lev</summary>
	[NativeName("Български лев")]
	[EnglishName("Bulgarian Lev")]
	BGN = 35,

	/// <summary>Brunei Dollar</summary>
	[NativeName("Dolar Brunei")]
	[EnglishName("Brunei Dollar")]
	BND = 37,

	/// <summary>Burundian Franc</summary>
	[NativeName("Burundian Franc")]
	[EnglishName("Burundian Franc")]
	BIF = 38,

	/// <summary>Canadian Dollar</summary>
	[NativeName("Canadian Dollar")]
	[EnglishName("Canadian Dollar")]
	CAD = 39,

	/// <summary>Cambodian Riel</summary>
	[NativeName("រៀល​កម្ពុជា")]
	[EnglishName("Cambodian Riel")]
	KHR = 40,

	/// <summary>Sri Lankan Rupee</summary>
	[NativeName("ශ්රී ලංකා රුපියල")]
	[EnglishName("Sri Lankan Rupee")]
	LKR = 42,

	/// <summary>Congolese Franc</summary>
	[NativeName("franc congolais")]
	[EnglishName("Congolese Franc")]
	CDF = 44,

	/// <summary>Chinese Yuan</summary>
	[NativeName("ཡུ་ཨན་")]
	[EnglishName("Chinese Yuan")]
	CNY = 45,

	/// <summary>Chilean Peso</summary>
	[NativeName("CLP")]
	[EnglishName("Chilean Peso")]
	CLP = 46,

	/// <summary>Central African CFA Franc</summary>
	[NativeName("CFA Fàlâŋ BEAC")]
	[EnglishName("Central African CFA Franc")]
	XAF = 49,

	/// <summary>Comorian Franc</summary>
	[NativeName("فرنك جزر القمر")]
	[EnglishName("Comorian Franc")]
	KMF = 50,

	/// <summary>Colombian Peso</summary>
	[NativeName("peso colombiano")]
	[EnglishName("Colombian Peso")]
	COP = 51,

	/// <summary>Costa Rican Colón</summary>
	[NativeName("colón costarricense")]
	[EnglishName("Costa Rican Colón")]
	CRC = 54,

	/// <summary>Cuban Peso</summary>
	[NativeName("peso cubano")]
	[EnglishName("Cuban Peso")]
	CUP = 56,

	/// <summary>Cape Verdean Escudo</summary>
	[NativeName("Skudu Kabuverdianu")]
	[EnglishName("Cape Verdean Escudo")]
	CVE = 57,

	/// <summary>Danish Krone</summary>
	[NativeName("dansk krone")]
	[EnglishName("Danish Krone")]
	DKK = 61,

	/// <summary>Djiboutian Franc</summary>
	[NativeName("فرنك جيبوتي")]
	[EnglishName("Djiboutian Franc")]
	DJF = 62,

	/// <summary>Dominican Peso</summary>
	[NativeName("peso dominicano")]
	[EnglishName("Dominican Peso")]
	DOP = 65,

	/// <summary>Egyptian Pound</summary>
	[NativeName("جنيه مصري")]
	[EnglishName("Egyptian Pound")]
	EGP = 67,

	/// <summary>Eritrean Nakfa</summary>
	[NativeName("ناكفا أريتري")]
	[EnglishName("Eritrean Nakfa")]
	ERN = 71,

	/// <summary>Ethiopian Birr</summary>
	[NativeName("የኢትዮጵያ ብር")]
	[EnglishName("Ethiopian Birr")]
	ETB = 73,

	/// <summary>Czech Koruna</summary>
	[NativeName("česká koruna")]
	[EnglishName("Czech Koruna")]
	CZK = 75,

	/// <summary>Fijian Dollar</summary>
	[NativeName("Fijian Dollar")]
	[EnglishName("Fijian Dollar")]
	FJD = 78,

	/// <summary>Gambian Dalasi</summary>
	[NativeName("Gambian Dalasi")]
	[EnglishName("Gambian Dalasi")]
	GMD = 86,

	/// <summary>Georgian Lari</summary>
	[NativeName("ქართული ლარი")]
	[EnglishName("Georgian Lari")]
	GEL = 88,

	/// <summary>Ghanaian Cedi</summary>
	[NativeName("Ghana Sidi")]
	[EnglishName("Ghanaian Cedi")]
	GHS = 89,

	/// <summary>Gibraltar Pound</summary>
	[NativeName("Gibraltar Pound")]
	[EnglishName("Gibraltar Pound")]
	GIP = 90,

	/// <summary>Guatemalan Quetzal</summary>
	[NativeName("quetzal")]
	[EnglishName("Guatemalan Quetzal")]
	GTQ = 99,

	/// <summary>Guinean Franc</summary>
	[NativeName("franc guinéen")]
	[EnglishName("Guinean Franc")]
	GNF = 100,

	/// <summary>Guyanaese Dollar</summary>
	[NativeName("Guyanaese Dollar")]
	[EnglishName("Guyanaese Dollar")]
	GYD = 101,

	/// <summary>Haitian Gourde</summary>
	[NativeName("gourde haïtienne")]
	[EnglishName("Haitian Gourde")]
	HTG = 103,

	/// <summary>Hong Kong Dollar</summary>
	[NativeName("Hong Kong Dollar")]
	[EnglishName("Hong Kong Dollar")]
	HKD = 104,

	/// <summary>Honduran Lempira</summary>
	[NativeName("lempira hondureño")]
	[EnglishName("Honduran Lempira")]
	HNL = 106,

	/// <summary>Croatian Kuna</summary>
	[NativeName("hrvatska kuna")]
	[EnglishName("Croatian Kuna")]
	HRK = 108,

	/// <summary>Hungarian Forint</summary>
	[NativeName("magyar forint")]
	[EnglishName("Hungarian Forint")]
	HUF = 109,

	/// <summary>Icelandic Króna</summary>
	[NativeName("íslensk króna")]
	[EnglishName("Icelandic Króna")]
	ISK = 110,

	/// <summary>Indonesian Rupiah</summary>
	[NativeName("Rupiah Indonesia")]
	[EnglishName("Indonesian Rupiah")]
	IDR = 111,

	/// <summary>Indian Rupee</summary>
	[NativeName("ভাৰতীয় ৰুপী")]
	[EnglishName("Indian Rupee")]
	INR = 113,

	/// <summary>Iranian Rial</summary>
	[NativeName("ریال ایران")]
	[EnglishName("Iranian Rial")]
	IRR = 116,

	/// <summary>Israeli New Shekel</summary>
	[NativeName("شيكل إسرائيلي جديد")]
	[EnglishName("Israeli New Shekel")]
	ILS = 117,

	/// <summary>Iraqi Dinar</summary>
	[NativeName("دينار عراقي")]
	[EnglishName("Iraqi Dinar")]
	IQD = 121,

	/// <summary>Japanese Yen</summary>
	[NativeName("日本円")]
	[EnglishName("Japanese Yen")]
	JPY = 122,

	/// <summary>Jamaican Dollar</summary>
	[NativeName("Jamaican Dollar")]
	[EnglishName("Jamaican Dollar")]
	JMD = 124,

	/// <summary>Jordanian Dinar</summary>
	[NativeName("دينار أردني")]
	[EnglishName("Jordanian Dinar")]
	JOD = 126,

	/// <summary>Kenyan Shilling</summary>
	[NativeName("Shilingi ya Kenya")]
	[EnglishName("Kenyan Shilling")]
	KES = 129,

	/// <summary>Kyrgystani Som</summary>
	[NativeName("Кыргызстан сому")]
	[EnglishName("Kyrgystani Som")]
	KGS = 130,

	/// <summary>North Korean Won</summary>
	[NativeName("조선 민주주의 인민 공화국 원")]
	[EnglishName("North Korean Won")]
	KPW = 131,

	/// <summary>South Korean Won</summary>
	[NativeName("대한민국 원")]
	[EnglishName("South Korean Won")]
	KRW = 134,

	/// <summary>Kuwaiti Dinar</summary>
	[NativeName("دينار كويتي")]
	[EnglishName("Kuwaiti Dinar")]
	KWD = 136,

	/// <summary>Kazakhstani Tenge</summary>
	[NativeName("Қазақстан теңгесі")]
	[EnglishName("Kazakhstani Tenge")]
	KZT = 137,

	/// <summary>Laotian Kip</summary>
	[NativeName("ລາວ ກີບ")]
	[EnglishName("Laotian Kip")]
	LAK = 138,

	/// <summary>Lebanese Pound</summary>
	[NativeName("جنيه لبناني")]
	[EnglishName("Lebanese Pound")]
	LBP = 139,

	/// <summary>Liberian Dollar</summary>
	[NativeName("Liberian Dollar")]
	[EnglishName("Liberian Dollar")]
	LRD = 142,

	/// <summary>Libyan Dinar</summary>
	[NativeName("دينار ليبي")]
	[EnglishName("Libyan Dinar")]
	LYD = 148,

	/// <summary>Malagasy Ariary</summary>
	[NativeName("Malagasy Ariary")]
	[EnglishName("Malagasy Ariary")]
	MGA = 149,

	/// <summary>Macanese Pataca</summary>
	[NativeName("Macanese Pataca")]
	[EnglishName("Macanese Pataca")]
	MOP = 151,

	/// <summary>Moldovan Leu</summary>
	[NativeName("leu moldovenesc")]
	[EnglishName("Moldovan Leu")]
	MDL = 152,

	/// <summary>Mongolian Tugrik</summary>
	[NativeName("төгрөг")]
	[EnglishName("Mongolian Tugrik")]
	MNT = 154,

	/// <summary>Malawian Kwacha</summary>
	[NativeName("Malawian Kwacha")]
	[EnglishName("Malawian Kwacha")]
	MWK = 156,

	/// <summary>Moroccan Dirham</summary>
	[NativeName("درهم مغربي")]
	[EnglishName("Moroccan Dirham")]
	MAD = 159,

	/// <summary>Mauritian Rupee</summary>
	[NativeName("Mauritian Rupee")]
	[EnglishName("Mauritian Rupee")]
	MUR = 160,

	/// <summary>Mauritanian Ouguiya</summary>
	[NativeName("أوقية موريتانية")]
	[EnglishName("Mauritanian Ouguiya")]
	MRU = 162,

	/// <summary>Omani Rial</summary>
	[NativeName("ريال عماني")]
	[EnglishName("Omani Rial")]
	OMR = 164,

	/// <summary>Maldivian Rufiyaa</summary>
	[NativeName("MVR")]
	[EnglishName("Maldivian Rufiyaa")]
	MVR = 165,

	/// <summary>Mexican Peso</summary>
	[NativeName("peso mexicano")]
	[EnglishName("Mexican Peso")]
	MXN = 166,

	/// <summary>Malaysian Ringgit</summary>
	[NativeName("Malaysian Ringgit")]
	[EnglishName("Malaysian Ringgit")]
	MYR = 167,

	/// <summary>Mozambican Metical</summary>
	[NativeName("MZN")]
	[EnglishName("Mozambican Metical")]
	MZN = 168,

	/// <summary>Vanuatu Vatu</summary>
	[NativeName("Vanuatu Vatu")]
	[EnglishName("Vanuatu Vatu")]
	VUV = 174,

	/// <summary>Nigerian Naira</summary>
	[NativeName("Nigerian Naira")]
	[EnglishName("Nigerian Naira")]
	NGN = 175,

	/// <summary>Norwegian Krone</summary>
	[NativeName("norske kroner")]
	[EnglishName("Norwegian Krone")]
	NOK = 177,

	/// <summary>Nepalese Rupee</summary>
	[NativeName("नेपाली रूपैयाँ")]
	[EnglishName("Nepalese Rupee")]
	NPR = 178,

	/// <summary>Surinamese Dollar</summary>
	[NativeName("Surinaamse dollar")]
	[EnglishName("Surinamese Dollar")]
	SRD = 181,

	/// <summary>Nicaraguan Córdoba</summary>
	[NativeName("córdoba nicaragüense")]
	[EnglishName("Nicaraguan Córdoba")]
	NIO = 182,

	/// <summary>Paraguayan Guarani</summary>
	[NativeName("guaraní paraguayo")]
	[EnglishName("Paraguayan Guarani")]
	PYG = 185,

	/// <summary>Peruvian Sol</summary>
	[NativeName("sol peruano")]
	[EnglishName("Peruvian Sol")]
	PEN = 187,

	/// <summary>Pakistani Rupee</summary>
	[NativeName("Pakistani Rupee")]
	[EnglishName("Pakistani Rupee")]
	PKR = 190,

	/// <summary>Polish Zloty</summary>
	[NativeName("złoty polski")]
	[EnglishName("Polish Zloty")]
	PLN = 191,

	/// <summary>Panamanian Balboa</summary>
	[NativeName("balboa panameño")]
	[EnglishName("Panamanian Balboa")]
	PAB = 192,

	/// <summary>Papua New Guinean Kina</summary>
	[NativeName("Papua New Guinean Kina")]
	[EnglishName("Papua New Guinean Kina")]
	PGK = 194,

	/// <summary>Qatari Rial</summary>
	[NativeName("ريال قطري")]
	[EnglishName("Qatari Rial")]
	QAR = 197,

	/// <summary>Romanian Leu</summary>
	[NativeName("leu românesc")]
	[EnglishName("Romanian Leu")]
	RON = 200,

	/// <summary>Philippine Piso</summary>
	[NativeName("Philippine Piso")]
	[EnglishName("Philippine Piso")]
	PHP = 201,

	/// <summary>Russian Ruble</summary>
	[NativeName("RUB")]
	[EnglishName("Russian Ruble")]
	RUB = 203,

	/// <summary>Rwandan Franc</summary>
	[NativeName("Rwandan Franc")]
	[EnglishName("Rwandan Franc")]
	RWF = 204,

	/// <summary>Saudi Riyal</summary>
	[NativeName("ريال سعودي")]
	[EnglishName("Saudi Riyal")]
	SAR = 205,

	/// <summary>Seychellois Rupee</summary>
	[NativeName("Seychellois Rupee")]
	[EnglishName("Seychellois Rupee")]
	SCR = 208,

	/// <summary>South African Rand</summary>
	[NativeName("Suid-Afrikaanse rand")]
	[EnglishName("South African Rand")]
	ZAR = 209,

	/// <summary>Sierra Leonean Leone</summary>
	[NativeName("Sierra Leonean Leone")]
	[EnglishName("Sierra Leonean Leone")]
	SLL = 213,

	/// <summary>Singapore Dollar</summary>
	[NativeName("Singapore Dollar")]
	[EnglishName("Singapore Dollar")]
	SGD = 215,

	/// <summary>Somali Shilling</summary>
	[NativeName("شلن صومالي")]
	[EnglishName("Somali Shilling")]
	SOS = 216,

	/// <summary>Euro</summary>
	[NativeName("euro")]
	[EnglishName("Euro")]
	EUR = 217,

	/// <summary>Sudanese Pound</summary>
	[NativeName("جنيه سوداني")]
	[EnglishName("Sudanese Pound")]
	SDG = 219,

	/// <summary>Swedish Krona</summary>
	[NativeName("Swedish Krona")]
	[EnglishName("Swedish Krona")]
	SEK = 221,

	/// <summary>Syrian Pound</summary>
	[NativeName("ليرة سورية")]
	[EnglishName("Syrian Pound")]
	SYP = 222,

	/// <summary>Swiss Franc</summary>
	[NativeName("Schweizer Franken")]
	[EnglishName("Swiss Franc")]
	CHF = 223,

	/// <summary>United Arab Emirates Dirham</summary>
	[NativeName("درهم إماراتي")]
	[EnglishName("United Arab Emirates Dirham")]
	AED = 224,

	/// <summary>Trinidad & Tobago Dollar</summary>
	[NativeName("Trinidad & Tobago Dollar")]
	[EnglishName("Trinidad & Tobago Dollar")]
	TTD = 225,

	/// <summary>Thai Baht</summary>
	[NativeName("บาท")]
	[EnglishName("Thai Baht")]
	THB = 227,

	/// <summary>Tongan Paʻanga</summary>
	[NativeName("Tongan Paʻanga")]
	[EnglishName("Tongan Paʻanga")]
	TOP = 231,

	/// <summary>São Tomé & Príncipe Dobra</summary>
	[NativeName("dobra de São Tomé e Príncipe")]
	[EnglishName("São Tomé & Príncipe Dobra")]
	STN = 233,

	/// <summary>Tunisian Dinar</summary>
	[NativeName("دينار تونسي")]
	[EnglishName("Tunisian Dinar")]
	TND = 234,

	/// <summary>Turkish Lira</summary>
	[NativeName("Türk Lirası")]
	[EnglishName("Turkish Lira")]
	TRY = 235,

	/// <summary>Turkmenistani Manat</summary>
	[NativeName("Türkmen manady")]
	[EnglishName("Turkmenistani Manat")]
	TMT = 238,

	/// <summary>Tanzanian Shilling</summary>
	[NativeName("shilingi ya Tandhania")]
	[EnglishName("Tanzanian Shilling")]
	TZS = 239,

	/// <summary>Ugandan Shilling</summary>
	[NativeName("Eshiringi ya Uganda")]
	[EnglishName("Ugandan Shilling")]
	UGX = 240,

	/// <summary>Ukrainian Hryvnia</summary>
	[NativeName("украинская гривна")]
	[EnglishName("Ukrainian Hryvnia")]
	UAH = 241,

	/// <summary>British Pound</summary>
	[NativeName("Punt Prydain")]
	[EnglishName("British Pound")]
	GBP = 242,

	/// <summary>West African CFA Franc</summary>
	[NativeName("sefa Fraŋ (BCEAO)")]
	[EnglishName("West African CFA Franc")]
	XOF = 244,

	/// <summary>Uruguayan Peso</summary>
	[NativeName("peso uruguayo")]
	[EnglishName("Uruguayan Peso")]
	UYU = 246,

	/// <summary>Uzbekistani Som</summary>
	[NativeName("Ўзбекистон сўм")]
	[EnglishName("Uzbekistani Som")]
	UZS = 247,

	/// <summary>Venezuelan Bolívar</summary>
	[NativeName("bolívar soberano")]
	[EnglishName("Venezuelan Bolívar")]
	VES = 249,

	/// <summary>Vietnamese Dong</summary>
	[NativeName("Đồng Việt Nam")]
	[EnglishName("Vietnamese Dong")]
	VND = 251,

	/// <summary>Namibian Dollar</summary>
	[NativeName("Namibiese dollar")]
	[EnglishName("Namibian Dollar")]
	NAD = 254,

	/// <summary>Samoan Tala</summary>
	[NativeName("Samoan Tala")]
	[EnglishName("Samoan Tala")]
	WST = 259,

	/// <summary>Swazi Lilangeni</summary>
	[NativeName("Swazi Lilangeni")]
	[EnglishName("Swazi Lilangeni")]
	SZL = 260,

	/// <summary>Yemeni Rial</summary>
	[NativeName("ريال يمني")]
	[EnglishName("Yemeni Rial")]
	YER = 261,

	/// <summary>Zambian Kwacha</summary>
	[NativeName("ZMW")]
	[EnglishName("Zambian Kwacha")]
	ZMW = 263,

	/// <summary>Serbian Dinar (2002–2006)</summary>
	[NativeName("Стари српски динар")]
	[EnglishName("Serbian Dinar (2002–2006)")]
	CSD = 269,

	/// <summary>Serbian Dinar</summary>
	[NativeName("Српски динар")]
	[EnglishName("Serbian Dinar")]
	RSD = 271,

	/// <summary>South Sudanese Pound</summary>
	[NativeName("جنيه جنوب السودان")]
	[EnglishName("South Sudanese Pound")]
	SSP = 276,

	/// <summary>Aruban Florin</summary>
	[NativeName("Arubaanse gulden")]
	[EnglishName("Aruban Florin")]
	AWG = 302,

	/// <summary>Cayman Islands Dollar</summary>
	[NativeName("Cayman Islands Dollar")]
	[EnglishName("Cayman Islands Dollar")]
	KYD = 307,

	/// <summary>New Zealand Dollar</summary>
	[NativeName("New Zealand Dollar")]
	[EnglishName("New Zealand Dollar")]
	NZD = 312,

	/// <summary>Falkland Islands Pound</summary>
	[NativeName("Falkland Islands Pound")]
	[EnglishName("Falkland Islands Pound")]
	FKP = 315,

	/// <summary>CFP Franc</summary>
	[NativeName("franc CFP")]
	[EnglishName("CFP Franc")]
	XPF = 334,

	/// <summary>St. Helena Pound</summary>
	[NativeName("St. Helena Pound")]
	[EnglishName("St. Helena Pound")]
	SHP = 343,

	/// <summary>Macedonian Denar</summary>
	[NativeName("Македонски денар")]
	[EnglishName("Macedonian Denar")]
	MKD = 19618,

	/// <summary>Netherlands Antillean Guilder</summary>
	[NativeName("Netherlands Antillean Guilder")]
	[EnglishName("Netherlands Antillean Guilder")]
	ANG = 30967,

}

