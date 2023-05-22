<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>




var urlRegex = new Regex("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$");




public class Iban
{
	public string Value { get; }
	
	public Iban(string iban)
	{
		Value = iban;
	}
	
	public bool IsValid()
	{
		
	}
	
	public class IbanRegexes
	{

		public Regex Andorra = new Regex("^AD\\d{10}[A-Z0-9]{12}$");
		public Regex UnitedArabEmirates = new Regex("^AE\\d{21}$");
		public Regex Albania = new Regex("^AL\\d{10}[A-Z0-9]{16}$");
		public Regex Austria = new Regex("^AT\\d{18}$");
		public Regex Azerbaijan = new Regex("^AZ\\d{2}[A-Z]{4}[A-Z0-9]{20}$");
		public Regex BosniaAndHerzegovina = new Regex("^BA\\d{18}$");
		public Regex Belgium = new Regex("^BE\\d{14}$");
		public Regex Bulgaria = new Regex("^BG\\d{2}[A-Z]{4}\\d{6}[A-Z0-9]{8}$");
		public Regex Bahrain = new Regex("^BH\\d{2}[A-Z]{4}[A-Z0-9]{14}$");
		public Regex Brazil = new Regex("^BR\\d{25}[A-Z]{1}[A-Z0-9]{1}$");
		public Regex RepublicOfBelarus = new Regex("^BY\\d{2}[A-Z0-9]{4}\\d{4}[A-Z0-9]{16}$");
		public Regex Switzerland = new Regex("^CH\\d{7}[A-Z0-9]{12}$");
		public Regex CostaRica = new Regex("^CR\\d{20}$");
		public Regex Cyprus = new Regex("^CY\\d{10}[A-Z0-9]{16}$");
		public Regex Czechia = new Regex("^CZ\\d{22}$");
		public Regex Germany = new Regex("^DE\\d{20}$");
		public Regex Denmark = new Regex("^DK\\d{16}$");
		public Regex DominicanRepublic = new Regex("^DO\\d{2}[A-Z0-9]{4}\\d{20}$");
		public Regex Estonia = new Regex("^EE\\d{18}$");
		public Regex Egypt = new Regex("^EG\\d{27}$");
		public Regex Spain = new Regex("^ES\\d{22}$");
		public Regex Finland = new Regex("^FI\\d{16}$");
		public Regex FaroeIslands = new Regex("^FO\\d{16}$");
		public Regex France = new Regex("^FR\\d{12}[A-Z0-9]{11}\\d{2}$");
		public Regex UnitedKingdom = new Regex("^GB\\d{2}[A-Z]{4}\\d{14}$");
		public Regex Georgia = new Regex("^GE\\d{2}[A-Z]{2}\\d{16}$");
		public Regex Gibraltar = new Regex("^GI\\d{2}[A-Z]{4}[A-Z0-9]{15}$");
		public Regex Greenland = new Regex("^GL\\d{16}$");
		public Regex Greece = new Regex("^GR\\d{9}[A-Z0-9]{16}$");
		public Regex Guatemala = new Regex("^GT\\d{2}[A-Z0-9]{24}$");
		public Regex Croatia = new Regex("^HR\\d{19}$");
		public Regex Hungary = new Regex("^HU\\d{26}$");
		public Regex Ireland = new Regex("^IE\\d{2}[A-Z]{4}\\d{14}$");
		public Regex Israel = new Regex("^IL\\d{21}$");
		public Regex Iraq = new Regex("^IQ\\d{2}[A-Z]{4}\\d{15}$");
		public Regex Iceland = new Regex("^IS\\d{24}$");
		public Regex Italy = new Regex("^IT\\d{2}[A-Z]{1}\\d{10}[A-Z0-9]{12}$");
		public Regex Jordan = new Regex("^JO\\d{2}[A-Z]{4}\\d{4}[A-Z0-9]{18}$");
		public Regex Kuwait = new Regex("^KW\\d{2}[A-Z]{4}[A-Z0-9]{22}$");
		public Regex Kazakhstan = new Regex("^KZ\\d{5}[A-Z0-9]{13}$");
		public Regex Lebanon = new Regex("^LB\\d{6}[A-Z0-9]{20}$");
		public Regex SaintLucia = new Regex("^LC\\d{2}[A-Z]{4}[A-Z0-9]{24}$");
		public Regex Liechtenstein = new Regex("^LI\\d{7}[A-Z0-9]{12}$");
		public Regex Lithuania = new Regex("^LT\\d{18}$");
		public Regex Luxembourg = new Regex("^LU\\d{5}[A-Z0-9]{13}$");
		public Regex Latvia = new Regex("^LV\\d{2}[A-Z]{4}[A-Z0-9]{13}$");
		public Regex Monaco = new Regex("^MC\\d{12}[A-Z0-9]{11}\\d{2}$");
		public Regex Moldova = new Regex("^MD\\d{2}[A-Z0-9]{20}$");
		public Regex Montenegro = new Regex("^ME\\d{20}$");
		public Regex Macedonia = new Regex("^MK\\d{5}[A-Z0-9]{10}\\d{2}$");
		public Regex Mauritania = new Regex("^MR\\d{25}$");
		public Regex Malta = new Regex("^MT\\d{2}[A-Z]{4}\\d{5}[A-Z0-9]{18}$");
		public Regex Mauritius = new Regex("^MU\\d{2}[A-Z]{4}\\d{19}[A-Z]{3}$");
		public Regex Netherlands = new Regex("^NL\\d{2}[A-Z]{4}\\d{10}$");
		public Regex Norway = new Regex("^NO\\d{13}$");
		public Regex Pakistan = new Regex("^PK\\d{2}[A-Z]{4}[A-Z0-9]{16}$");
		public Regex Poland = new Regex("^PL\\d{26}$");
		public Regex Palestine = new Regex("^PS\\d{2}[A-Z]{4}[A-Z0-9]{21}$");
		public Regex Portugal = new Regex("^PT\\d{23}$");
		public Regex Qatar = new Regex("^QA\\d{2}[A-Z]{4}[A-Z0-9]{21}$");
		public Regex Romania = new Regex("^RO\\d{2}[A-Z]{4}[A-Z0-9]{16}$");
		public Regex Serbia = new Regex("^RS\\d{20}$");
		public Regex SaudiArabia = new Regex("^SA\\d{4}[A-Z0-9]{18}$");
		public Regex Seychelles = new Regex("^SC\\d{2}[A-Z]{4}\\d{20}[A-Z]{3}$");
		public Regex Sweden = new Regex("^SE\\d{22}$");
		public Regex Slovenia = new Regex("^SI\\d{17}$");
		public Regex Slovakia = new Regex("^SK\\d{22}$");
		public Regex SanMarino = new Regex("^SM\\d{2}[A-Z]{1}\\d{10}[A-Z0-9]{12}$");
		public Regex SaoTomeAndPrincipe = new Regex("^ST\\d{23}$");
		public Regex ElSalvador = new Regex("^SV\\d{2}[A-Z]{4}\\d{20}$");
		public Regex TimorLeste = new Regex("^TL\\d{21}$");
		public Regex Tunisia = new Regex("^TN\\d{22}$");
		public Regex Turkey = new Regex("^TR\\d{8}[A-Z0-9]{16}$");
		public Regex Ukraine = new Regex("^UA\\d{8}[A-Z0-9]{19}$");
		public Regex VaticanCityState = new Regex("^VA\\d{20}$");
		public Regex VirginIslands = new Regex("^VG\\d{2}[A-Z]{4}\\d{16}$");
		public Regex Kosovo = new Regex("^XK\\d{18}$");

		public class IbanRegexPatterns
		{
			public const string Andorra = "^AD\\d{10}[A-Z0-9]{12}$";
			public const string UnitedArabEmirates = "^AE\\d{21}$";
			public const string Albania = "^AL\\d{10}[A-Z0-9]{16}$";
			public const string Austria = "^AT\\d{18}$";
			public const string Azerbaijan = "^AZ\\d{2}[A-Z]{4}[A-Z0-9]{20}$";
			public const string BosniaAndHerzegovina = "^BA\\d{18}$";
			public const string Belgium = "^BE\\d{14}$";
			public const string Bulgaria = "^BG\\d{2}[A-Z]{4}\\d{6}[A-Z0-9]{8}$";
			public const string Bahrain = "^BH\\d{2}[A-Z]{4}[A-Z0-9]{14}$";
			public const string Brazil = "^BR\\d{25}[A-Z]{1}[A-Z0-9]{1}$";
			public const string RepublicOfBelarus = "^BY\\d{2}[A-Z0-9]{4}\\d{4}[A-Z0-9]{16}$";
			public const string Switzerland = "^CH\\d{7}[A-Z0-9]{12}$";
			public const string CostaRica = "^CR\\d{20}$";
			public const string Cyprus = "^CY\\d{10}[A-Z0-9]{16}$";
			public const string Czechia = "^CZ\\d{22}$";
			public const string Germany = "^DE\\d{20}$";
			public const string Denmark = "^DK\\d{16}$";
			public const string DominicanRepublic = "^DO\\d{2}[A-Z0-9]{4}\\d{20}$";
			public const string Estonia = "^EE\\d{18}$";
			public const string Egypt = "^EG\\d{27}$";
			public const string Spain = "^ES\\d{22}$";
			public const string Finland = "^FI\\d{16}$";
			public const string FaroeIslands = "^FO\\d{16}$";
			public const string France = "^FR\\d{12}[A-Z0-9]{11}\\d{2}$";
			public const string UnitedKingdom = "^GB\\d{2}[A-Z]{4}\\d{14}$";
			public const string Georgia = "^GE\\d{2}[A-Z]{2}\\d{16}$";
			public const string Gibraltar = "^GI\\d{2}[A-Z]{4}[A-Z0-9]{15}$";
			public const string Greenland = "^GL\\d{16}$";
			public const string Greece = "^GR\\d{9}[A-Z0-9]{16}$";
			public const string Guatemala = "^GT\\d{2}[A-Z0-9]{24}$";
			public const string Croatia = "^HR\\d{19}$";
			public const string Hungary = "^HU\\d{26}$";
			public const string Ireland = "^IE\\d{2}[A-Z]{4}\\d{14}$";
			public const string Israel = "^IL\\d{21}$";
			public const string Iraq = "^IQ\\d{2}[A-Z]{4}\\d{15}$";
			public const string Iceland = "^IS\\d{24}$";
			public const string Italy = "^IT\\d{2}[A-Z]{1}\\d{10}[A-Z0-9]{12}$";
			public const string Jordan = "^JO\\d{2}[A-Z]{4}\\d{4}[A-Z0-9]{18}$";
			public const string Kuwait = "^KW\\d{2}[A-Z]{4}[A-Z0-9]{22}$";
			public const string Kazakhstan = "^KZ\\d{5}[A-Z0-9]{13}$";
			public const string Lebanon = "^LB\\d{6}[A-Z0-9]{20}$";
			public const string SaintLucia = "^LC\\d{2}[A-Z]{4}[A-Z0-9]{24}$";
			public const string Liechtenstein = "^LI\\d{7}[A-Z0-9]{12}$";
			public const string Lithuania = "^LT\\d{18}$";
			public const string Luxembourg = "^LU\\d{5}[A-Z0-9]{13}$";
			public const string Latvia = "^LV\\d{2}[A-Z]{4}[A-Z0-9]{13}$";
			public const string Monaco = "^MC\\d{12}[A-Z0-9]{11}\\d{2}$";
			public const string Moldova = "^MD\\d{2}[A-Z0-9]{20}$";
			public const string Montenegro = "^ME\\d{20}$";
			public const string Macedonia = "^MK\\d{5}[A-Z0-9]{10}\\d{2}$";
			public const string Mauritania = "^MR\\d{25}$";
			public const string Malta = "^MT\\d{2}[A-Z]{4}\\d{5}[A-Z0-9]{18}$";
			public const string Mauritius = "^MU\\d{2}[A-Z]{4}\\d{19}[A-Z]{3}$";
			public const string Netherlands = "^NL\\d{2}[A-Z]{4}\\d{10}$";
			public const string Norway = "^NO\\d{13}$";
			public const string Pakistan = "^PK\\d{2}[A-Z]{4}[A-Z0-9]{16}$";
			public const string Poland = "^PL\\d{26}$";
			public const string Palestine = "^PS\\d{2}[A-Z]{4}[A-Z0-9]{21}$";
			public const string Portugal = "^PT\\d{23}$";
			public const string Qatar = "^QA\\d{2}[A-Z]{4}[A-Z0-9]{21}$";
			public const string Romania = "^RO\\d{2}[A-Z]{4}[A-Z0-9]{16}$";
			public const string Serbia = "^RS\\d{20}$";
			public const string SaudiArabia = "^SA\\d{4}[A-Z0-9]{18}$";
			public const string Seychelles = "^SC\\d{2}[A-Z]{4}\\d{20}[A-Z]{3}$";
			public const string Sweden = "^SE\\d{22}$";
			public const string Slovenia = "^SI\\d{17}$";
			public const string Slovakia = "^SK\\d{22}$";
			public const string SanMarino = "^SM\\d{2}[A-Z]{1}\\d{10}[A-Z0-9]{12}$";
			public const string SaoTomeAndPrincipe = "^ST\\d{23}$";
			public const string ElSalvador = "^SV\\d{2}[A-Z]{4}\\d{20}$";
			public const string TimorLeste = "^TL\\d{21}$";
			public const string Tunisia = "^TN\\d{22}$";
			public const string Turkey = "^TR\\d{8}[A-Z0-9]{16}$";
			public const string Ukraine = "^UA\\d{8}[A-Z0-9]{19}$";
			public const string VaticanCityState = "^VA\\d{20}$";
			public const string VirginIslands = "^VG\\d{2}[A-Z]{4}\\d{16}$";
			public const string Kosovo = "^XK\\d{18}$";
		}
	}
}