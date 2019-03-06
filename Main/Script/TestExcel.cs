using System;
using System.Collections.Generic;
namespace SuperHero.Config
{
	public class AttributeData
	{
		private int mId;
		private int mValue;
		public int Id
		{
			get{ return mId; }
			set{ mId = value; }
		}
		public int Value
		{
			get{ return mValue; }
			set{ mValue = value; }
		}
	}
	public class TestExcelProperty
	{
		private static string Path = "ConfigAssets/TestExcel";
		private int mId;
		private string mName;
		private string mRemark;
		private int[] mPrice;
		private AttributeData mAttribute;
		private string mModel;
		public int Id
		{
			get{ return mId; }
			set{ mId = value; }
		}
		public string Name
		{
			get{ return mName; }
			set{ mName = value; }
		}
		public string Remark
		{
			get{ return mRemark; }
			set{ mRemark = value; }
		}
		public int[] Price
		{
			get{ return mPrice; }
			set{ mPrice = value; }
		}
		public AttributeData Attribute
		{
			get{ return mAttribute; }
			set{ mAttribute = value; }
		}
		public string Model
		{
			get{ return mModel; }
			set{ mModel = value; }
		}
		//public static TestExcelProperty Read(int id)
			//{
			//	var config = TestExcel.Instance.Config;
			//	int count = config.Count;
			//	for(int i = 0; i < count; i++)
				//	{
				//		if(config[i].Id == id)
					//		{
					//			return config[i];
				//		}
			//	}
			//	return null;
		//}
	}
}
