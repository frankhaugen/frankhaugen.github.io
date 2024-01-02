<Query Kind="Statements">
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

	[XmlRoot(ElementName = "HOP")]
	public class HOP
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "ORIGIN")]
		public string ORIGIN { get; set; }
		[XmlElement(ElementName = "ALPHA")]
		public string ALPHA { get; set; }
		[XmlElement(ElementName = "AMOUNT")]
		public string AMOUNT { get; set; }
		[XmlElement(ElementName = "USE")]
		public string USE { get; set; }
		[XmlElement(ElementName = "TIME")]
		public string TIME { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "FORM")]
		public string FORM { get; set; }
		[XmlElement(ElementName = "BETA")]
		public string BETA { get; set; }
		[XmlElement(ElementName = "HSI")]
		public string HSI { get; set; }
		[XmlElement(ElementName = "DISPLAY_AMOUNT")]
		public string DISPLAY_AMOUNT { get; set; }
		[XmlElement(ElementName = "INVENTORY")]
		public string INVENTORY { get; set; }
		[XmlElement(ElementName = "DISPLAY_TIME")]
		public string DISPLAY_TIME { get; set; }
	}

	[XmlRoot(ElementName = "HOPS")]
	public class HOPS
	{
		[XmlElement(ElementName = "HOP")]
		public List<HOP> HOP { get; set; }
	}

	[XmlRoot(ElementName = "FERMENTABLE")]
	public class FERMENTABLE
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "AMOUNT")]
		public string AMOUNT { get; set; }
		[XmlElement(ElementName = "YIELD")]
		public string YIELD { get; set; }
		[XmlElement(ElementName = "COLOR")]
		public string COLOR { get; set; }
		[XmlElement(ElementName = "ADD_AFTER_BOIL")]
		public string ADD_AFTER_BOIL { get; set; }
		[XmlElement(ElementName = "ORIGIN")]
		public string ORIGIN { get; set; }
		[XmlElement(ElementName = "SUPPLIER")]
		public string SUPPLIER { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "COARSE_FINE_DIFF")]
		public string COARSE_FINE_DIFF { get; set; }
		[XmlElement(ElementName = "MOISTURE")]
		public string MOISTURE { get; set; }
		[XmlElement(ElementName = "DIASTATIC_POWER")]
		public string DIASTATIC_POWER { get; set; }
		[XmlElement(ElementName = "PROTEIN")]
		public string PROTEIN { get; set; }
		[XmlElement(ElementName = "MAX_IN_BATCH")]
		public string MAX_IN_BATCH { get; set; }
		[XmlElement(ElementName = "RECOMMEND_MASH")]
		public string RECOMMEND_MASH { get; set; }
		[XmlElement(ElementName = "IBU_GAL_PER_LB")]
		public string IBU_GAL_PER_LB { get; set; }
		[XmlElement(ElementName = "DISPLAY_AMOUNT")]
		public string DISPLAY_AMOUNT { get; set; }
		[XmlElement(ElementName = "INVENTORY")]
		public string INVENTORY { get; set; }
		[XmlElement(ElementName = "POTENTIAL")]
		public string POTENTIAL { get; set; }
		[XmlElement(ElementName = "DISPLAY_COLOR")]
		public string DISPLAY_COLOR { get; set; }
	}

	[XmlRoot(ElementName = "FERMENTABLES")]
	public class FERMENTABLES
	{
		[XmlElement(ElementName = "FERMENTABLE")]
		public List<FERMENTABLE> FERMENTABLE { get; set; }
	}

	[XmlRoot(ElementName = "MISC")]
	public class MISC
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "USE")]
		public string USE { get; set; }
		[XmlElement(ElementName = "AMOUNT")]
		public string AMOUNT { get; set; }
		[XmlElement(ElementName = "TIME")]
		public string TIME { get; set; }
		[XmlElement(ElementName = "AMOUNT_IS_WEIGHT")]
		public string AMOUNT_IS_WEIGHT { get; set; }
		[XmlElement(ElementName = "USE_FOR")]
		public string USE_FOR { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "DISPLAY_AMOUNT")]
		public string DISPLAY_AMOUNT { get; set; }
		[XmlElement(ElementName = "INVENTORY")]
		public string INVENTORY { get; set; }
		[XmlElement(ElementName = "DISPLAY_TIME")]
		public string DISPLAY_TIME { get; set; }
		[XmlElement(ElementName = "BATCH_SIZE")]
		public string BATCH_SIZE { get; set; }
	}

	[XmlRoot(ElementName = "MISCS")]
	public class MISCS
	{
		[XmlElement(ElementName = "MISC")]
		public List<MISC> MISC { get; set; }
	}

	[XmlRoot(ElementName = "YEAST")]
	public class YEAST
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "FORM")]
		public string FORM { get; set; }
		[XmlElement(ElementName = "AMOUNT")]
		public string AMOUNT { get; set; }
		[XmlElement(ElementName = "AMOUNT_IS_WEIGHT")]
		public string AMOUNT_IS_WEIGHT { get; set; }
		[XmlElement(ElementName = "LABORATORY")]
		public string LABORATORY { get; set; }
		[XmlElement(ElementName = "PRODUCT_ID")]
		public string PRODUCT_ID { get; set; }
		[XmlElement(ElementName = "MIN_TEMPERATURE")]
		public string MIN_TEMPERATURE { get; set; }
		[XmlElement(ElementName = "MAX_TEMPERATURE")]
		public string MAX_TEMPERATURE { get; set; }
		[XmlElement(ElementName = "FLOCCULATION")]
		public string FLOCCULATION { get; set; }
		[XmlElement(ElementName = "ATTENUATION")]
		public string ATTENUATION { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "BEST_FOR")]
		public string BEST_FOR { get; set; }
		[XmlElement(ElementName = "MAX_REUSE")]
		public string MAX_REUSE { get; set; }
		[XmlElement(ElementName = "TIMES_CULTURED")]
		public string TIMES_CULTURED { get; set; }
		[XmlElement(ElementName = "ADD_TO_SECONDARY")]
		public string ADD_TO_SECONDARY { get; set; }
		[XmlElement(ElementName = "DISPLAY_AMOUNT")]
		public string DISPLAY_AMOUNT { get; set; }
		[XmlElement(ElementName = "DISP_MIN_TEMP")]
		public string DISP_MIN_TEMP { get; set; }
		[XmlElement(ElementName = "DISP_MAX_TEMP")]
		public string DISP_MAX_TEMP { get; set; }
		[XmlElement(ElementName = "INVENTORY")]
		public string INVENTORY { get; set; }
		[XmlElement(ElementName = "CULTURE_DATE")]
		public string CULTURE_DATE { get; set; }
	}

	[XmlRoot(ElementName = "YEASTS")]
	public class YEASTS
	{
		[XmlElement(ElementName = "YEAST")]
		public YEAST YEAST { get; set; }
	}

	[XmlRoot(ElementName = "WATER")]
	public class WATER
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "AMOUNT")]
		public string AMOUNT { get; set; }
		[XmlElement(ElementName = "CALCIUM")]
		public string CALCIUM { get; set; }
		[XmlElement(ElementName = "BICARBONATE")]
		public string BICARBONATE { get; set; }
		[XmlElement(ElementName = "SULFATE")]
		public string SULFATE { get; set; }
		[XmlElement(ElementName = "CHLORIDE")]
		public string CHLORIDE { get; set; }
		[XmlElement(ElementName = "SODIUM")]
		public string SODIUM { get; set; }
		[XmlElement(ElementName = "MAGNESIUM")]
		public string MAGNESIUM { get; set; }
		[XmlElement(ElementName = "PH")]
		public string PH { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "DISPLAY_AMOUNT")]
		public string DISPLAY_AMOUNT { get; set; }
	}

	[XmlRoot(ElementName = "WATERS")]
	public class WATERS
	{
		[XmlElement(ElementName = "WATER")]
		public WATER WATER { get; set; }
	}

	[XmlRoot(ElementName = "STYLE")]
	public class STYLE
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "CATEGORY")]
		public string CATEGORY { get; set; }
		[XmlElement(ElementName = "CATEGORY_NUMBER")]
		public string CATEGORY_NUMBER { get; set; }
		[XmlElement(ElementName = "STYLE_LETTER")]
		public string STYLE_LETTER { get; set; }
		[XmlElement(ElementName = "STYLE_GUIDE")]
		public string STYLE_GUIDE { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "OG_MIN")]
		public string OG_MIN { get; set; }
		[XmlElement(ElementName = "OG_MAX")]
		public string OG_MAX { get; set; }
		[XmlElement(ElementName = "FG_MIN")]
		public string FG_MIN { get; set; }
		[XmlElement(ElementName = "FG_MAX")]
		public string FG_MAX { get; set; }
		[XmlElement(ElementName = "IBU_MIN")]
		public string IBU_MIN { get; set; }
		[XmlElement(ElementName = "IBU_MAX")]
		public string IBU_MAX { get; set; }
		[XmlElement(ElementName = "COLOR_MIN")]
		public string COLOR_MIN { get; set; }
		[XmlElement(ElementName = "COLOR_MAX")]
		public string COLOR_MAX { get; set; }
		[XmlElement(ElementName = "CARB_MIN")]
		public string CARB_MIN { get; set; }
		[XmlElement(ElementName = "CARB_MAX")]
		public string CARB_MAX { get; set; }
		[XmlElement(ElementName = "ABV_MAX")]
		public string ABV_MAX { get; set; }
		[XmlElement(ElementName = "ABV_MIN")]
		public string ABV_MIN { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "PROFILE")]
		public string PROFILE { get; set; }
		[XmlElement(ElementName = "INGREDIENTS")]
		public string INGREDIENTS { get; set; }
		[XmlElement(ElementName = "EXAMPLES")]
		public string EXAMPLES { get; set; }
		[XmlElement(ElementName = "DISPLAY_OG_MIN")]
		public string DISPLAY_OG_MIN { get; set; }
		[XmlElement(ElementName = "DISPLAY_OG_MAX")]
		public string DISPLAY_OG_MAX { get; set; }
		[XmlElement(ElementName = "DISPLAY_FG_MIN")]
		public string DISPLAY_FG_MIN { get; set; }
		[XmlElement(ElementName = "DISPLAY_FG_MAX")]
		public string DISPLAY_FG_MAX { get; set; }
		[XmlElement(ElementName = "DISPLAY_COLOR_MIN")]
		public string DISPLAY_COLOR_MIN { get; set; }
		[XmlElement(ElementName = "DISPLAY_COLOR_MAX")]
		public string DISPLAY_COLOR_MAX { get; set; }
		[XmlElement(ElementName = "OG_RANGE")]
		public string OG_RANGE { get; set; }
		[XmlElement(ElementName = "FG_RANGE")]
		public string FG_RANGE { get; set; }
		[XmlElement(ElementName = "IBU_RANGE")]
		public string IBU_RANGE { get; set; }
		[XmlElement(ElementName = "CARB_RANGE")]
		public string CARB_RANGE { get; set; }
		[XmlElement(ElementName = "COLOR_RANGE")]
		public string COLOR_RANGE { get; set; }
		[XmlElement(ElementName = "ABV_RANGE")]
		public string ABV_RANGE { get; set; }
	}

	[XmlRoot(ElementName = "EQUIPMENT")]
	public class EQUIPMENT
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "BOIL_SIZE")]
		public string BOIL_SIZE { get; set; }
		[XmlElement(ElementName = "BATCH_SIZE")]
		public string BATCH_SIZE { get; set; }
		[XmlElement(ElementName = "TUN_VOLUME")]
		public string TUN_VOLUME { get; set; }
		[XmlElement(ElementName = "TUN_WEIGHT")]
		public string TUN_WEIGHT { get; set; }
		[XmlElement(ElementName = "TUN_SPECIFIC_HEAT")]
		public string TUN_SPECIFIC_HEAT { get; set; }
		[XmlElement(ElementName = "TOP_UP_WATER")]
		public string TOP_UP_WATER { get; set; }
		[XmlElement(ElementName = "TRUB_CHILLER_LOSS")]
		public string TRUB_CHILLER_LOSS { get; set; }
		[XmlElement(ElementName = "EVAP_RATE")]
		public string EVAP_RATE { get; set; }
		[XmlElement(ElementName = "BOIL_TIME")]
		public string BOIL_TIME { get; set; }
		[XmlElement(ElementName = "CALC_BOIL_VOLUME")]
		public string CALC_BOIL_VOLUME { get; set; }
		[XmlElement(ElementName = "LAUTER_DEADSPACE")]
		public string LAUTER_DEADSPACE { get; set; }
		[XmlElement(ElementName = "TOP_UP_KETTLE")]
		public string TOP_UP_KETTLE { get; set; }
		[XmlElement(ElementName = "HOP_UTILIZATION")]
		public string HOP_UTILIZATION { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "DISPLAY_BOIL_SIZE")]
		public string DISPLAY_BOIL_SIZE { get; set; }
		[XmlElement(ElementName = "DISPLAY_BATCH_SIZE")]
		public string DISPLAY_BATCH_SIZE { get; set; }
		[XmlElement(ElementName = "DISPLAY_TUN_VOLUME")]
		public string DISPLAY_TUN_VOLUME { get; set; }
		[XmlElement(ElementName = "DISPLAY_TUN_WEIGHT")]
		public string DISPLAY_TUN_WEIGHT { get; set; }
		[XmlElement(ElementName = "DISPLAY_TOP_UP_WATER")]
		public string DISPLAY_TOP_UP_WATER { get; set; }
		[XmlElement(ElementName = "DISPLAY_TRUB_CHILLER_LOSS")]
		public string DISPLAY_TRUB_CHILLER_LOSS { get; set; }
		[XmlElement(ElementName = "DISPLAY_LAUTER_DEADSPACE")]
		public string DISPLAY_LAUTER_DEADSPACE { get; set; }
		[XmlElement(ElementName = "DISPLAY_TOP_UP_KETTLE")]
		public string DISPLAY_TOP_UP_KETTLE { get; set; }
	}

	[XmlRoot(ElementName = "MASH_STEP")]
	public class MASH_STEP
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "INFUSE_AMOUNT")]
		public string INFUSE_AMOUNT { get; set; }
		[XmlElement(ElementName = "STEP_TIME")]
		public string STEP_TIME { get; set; }
		[XmlElement(ElementName = "STEP_TEMP")]
		public string STEP_TEMP { get; set; }
		[XmlElement(ElementName = "RAMP_TIME")]
		public string RAMP_TIME { get; set; }
		[XmlElement(ElementName = "END_TEMP")]
		public string END_TEMP { get; set; }
		[XmlElement(ElementName = "DESCRIPTION")]
		public string DESCRIPTION { get; set; }
		[XmlElement(ElementName = "WATER_GRAIN_RATIO")]
		public string WATER_GRAIN_RATIO { get; set; }
		[XmlElement(ElementName = "DECOCTION_AMT")]
		public string DECOCTION_AMT { get; set; }
		[XmlElement(ElementName = "INFUSE_TEMP")]
		public string INFUSE_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_STEP_TEMP")]
		public string DISPLAY_STEP_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_INFUSE_AMT")]
		public string DISPLAY_INFUSE_AMT { get; set; }
	}

	[XmlRoot(ElementName = "MASH_STEPS")]
	public class MASH_STEPS
	{
		[XmlElement(ElementName = "MASH_STEP")]
		public List<MASH_STEP> MASH_STEP { get; set; }
	}

	[XmlRoot(ElementName = "MASH")]
	public class MASH
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "GRAIN_TEMP")]
		public string GRAIN_TEMP { get; set; }
		[XmlElement(ElementName = "TUN_TEMP")]
		public string TUN_TEMP { get; set; }
		[XmlElement(ElementName = "SPARGE_TEMP")]
		public string SPARGE_TEMP { get; set; }
		[XmlElement(ElementName = "PH")]
		public string PH { get; set; }
		[XmlElement(ElementName = "TUN_WEIGHT")]
		public string TUN_WEIGHT { get; set; }
		[XmlElement(ElementName = "TUN_SPECIFIC_HEAT")]
		public string TUN_SPECIFIC_HEAT { get; set; }
		[XmlElement(ElementName = "EQUIP_ADJUST")]
		public string EQUIP_ADJUST { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "DISPLAY_GRAIN_TEMP")]
		public string DISPLAY_GRAIN_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_TUN_TEMP")]
		public string DISPLAY_TUN_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_SPARGE_TEMP")]
		public string DISPLAY_SPARGE_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_TUN_WEIGHT")]
		public string DISPLAY_TUN_WEIGHT { get; set; }
		[XmlElement(ElementName = "MASH_STEPS")]
		public MASH_STEPS MASH_STEPS { get; set; }
	}

	[XmlRoot(ElementName = "RECIPE")]
	public class RECIPE
	{
		[XmlElement(ElementName = "NAME")]
		public string NAME { get; set; }
		[XmlElement(ElementName = "VERSION")]
		public string VERSION { get; set; }
		[XmlElement(ElementName = "TYPE")]
		public string TYPE { get; set; }
		[XmlElement(ElementName = "BREWER")]
		public string BREWER { get; set; }
		[XmlElement(ElementName = "ASST_BREWER")]
		public string ASST_BREWER { get; set; }
		[XmlElement(ElementName = "BATCH_SIZE")]
		public string BATCH_SIZE { get; set; }
		[XmlElement(ElementName = "BOIL_SIZE")]
		public string BOIL_SIZE { get; set; }
		[XmlElement(ElementName = "BOIL_TIME")]
		public string BOIL_TIME { get; set; }
		[XmlElement(ElementName = "EFFICIENCY")]
		public string EFFICIENCY { get; set; }
		[XmlElement(ElementName = "HOPS")]
		public HOPS HOPS { get; set; }
		[XmlElement(ElementName = "FERMENTABLES")]
		public FERMENTABLES FERMENTABLES { get; set; }
		[XmlElement(ElementName = "MISCS")]
		public MISCS MISCS { get; set; }
		[XmlElement(ElementName = "YEASTS")]
		public YEASTS YEASTS { get; set; }
		[XmlElement(ElementName = "WATERS")]
		public WATERS WATERS { get; set; }
		[XmlElement(ElementName = "STYLE")]
		public STYLE STYLE { get; set; }
		[XmlElement(ElementName = "EQUIPMENT")]
		public EQUIPMENT EQUIPMENT { get; set; }
		[XmlElement(ElementName = "MASH")]
		public MASH MASH { get; set; }
		[XmlElement(ElementName = "NOTES")]
		public string NOTES { get; set; }
		[XmlElement(ElementName = "TASTE_NOTES")]
		public string TASTE_NOTES { get; set; }
		[XmlElement(ElementName = "TASTE_RATING")]
		public string TASTE_RATING { get; set; }
		[XmlElement(ElementName = "OG")]
		public string OG { get; set; }
		[XmlElement(ElementName = "FG")]
		public string FG { get; set; }
		[XmlElement(ElementName = "CARBONATION")]
		public string CARBONATION { get; set; }
		[XmlElement(ElementName = "FERMENTATION_STAGES")]
		public string FERMENTATION_STAGES { get; set; }
		[XmlElement(ElementName = "PRIMARY_AGE")]
		public string PRIMARY_AGE { get; set; }
		[XmlElement(ElementName = "PRIMARY_TEMP")]
		public string PRIMARY_TEMP { get; set; }
		[XmlElement(ElementName = "SECONDARY_AGE")]
		public string SECONDARY_AGE { get; set; }
		[XmlElement(ElementName = "SECONDARY_TEMP")]
		public string SECONDARY_TEMP { get; set; }
		[XmlElement(ElementName = "TERTIARY_AGE")]
		public string TERTIARY_AGE { get; set; }
		[XmlElement(ElementName = "AGE")]
		public string AGE { get; set; }
		[XmlElement(ElementName = "AGE_TEMP")]
		public string AGE_TEMP { get; set; }
		[XmlElement(ElementName = "CARBONATION_USED")]
		public string CARBONATION_USED { get; set; }
		[XmlElement(ElementName = "DATE")]
		public string DATE { get; set; }
		[XmlElement(ElementName = "EST_OG")]
		public string EST_OG { get; set; }
		[XmlElement(ElementName = "EST_FG")]
		public string EST_FG { get; set; }
		[XmlElement(ElementName = "EST_COLOR")]
		public string EST_COLOR { get; set; }
		[XmlElement(ElementName = "IBU")]
		public string IBU { get; set; }
		[XmlElement(ElementName = "IBU_METHOD")]
		public string IBU_METHOD { get; set; }
		[XmlElement(ElementName = "EST_ABV")]
		public string EST_ABV { get; set; }
		[XmlElement(ElementName = "ABV")]
		public string ABV { get; set; }
		[XmlElement(ElementName = "ACTUAL_EFFICIENCY")]
		public string ACTUAL_EFFICIENCY { get; set; }
		[XmlElement(ElementName = "CALORIES")]
		public string CALORIES { get; set; }
		[XmlElement(ElementName = "DISPLAY_BATCH_SIZE")]
		public string DISPLAY_BATCH_SIZE { get; set; }
		[XmlElement(ElementName = "DISPLAY_BOIL_SIZE")]
		public string DISPLAY_BOIL_SIZE { get; set; }
		[XmlElement(ElementName = "DISPLAY_OG")]
		public string DISPLAY_OG { get; set; }
		[XmlElement(ElementName = "DISPLAY_FG")]
		public string DISPLAY_FG { get; set; }
		[XmlElement(ElementName = "DISPLAY_PRIMARY_TEMP")]
		public string DISPLAY_PRIMARY_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_SECONDARY_TEMP")]
		public string DISPLAY_SECONDARY_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_TERTIARY_TEMP")]
		public string DISPLAY_TERTIARY_TEMP { get; set; }
		[XmlElement(ElementName = "DISPLAY_AGE_TEMP")]
		public string DISPLAY_AGE_TEMP { get; set; }
	}

	[XmlRoot(ElementName = "RECIPES")]
	public class RECIPES
	{
		[XmlElement(ElementName = "RECIPE")]
		public List<RECIPE> RECIPE { get; set; }
	}
