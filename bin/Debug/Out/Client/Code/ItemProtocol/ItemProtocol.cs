//************************************************************
// Auto Generated Code By ProtocolTool
// Copyright(c) Cao ChunYang  All rights reserved.
//************************************************************

using System;
using System.Collections.Generic;

class ItemProtocol
{

	 /**>
	 * 使用道具
	 @ param uint id (作用对象)
	 @ param List<EItemStruct1> item (使用个数)
	 **/
	public void Send_SceneUseItem1(uint id , List<EItemStruct1> item)
	{
		//public uint id;
		//public List item;
	}

	 /**>
	 * 使用道具
	 @ param uint id (作用对象)
	 @ param List<EItemStruct2> item (使用个数)
	 **/
	public void Send_SceneUseItem2(uint id , List<EItemStruct2> item)
	{
		//public uint id;
		//public List item;
	}

	 /**>
	 * 同步翅膀信息
	 @ param uint itemId (翅膀Id)
	 @ param uint stage (翅膀阶数)
	 @ param uint progress (翅膀升级进度)
	 @ param string level (翅膀强化等级)
	 @ param List<EItemStruct2> item (属性)
	 **/
	private void Receive_SceneSyncWing(uint itemId , uint stage , uint progress , string level , List<EItemStruct2> item)
	{
		//public uint itemId;
		//public uint stage;
		//public uint progress;
		//public string level;
		//public List item;
	}

	 /**>
	 * 翅膀升阶进度
	 @ param uint itemId (翅膀Id)
	 @ param List<EItemStruct1> item (翅膀升级进度)
	 **/
	private void Receive_SceneSyncWingStageProgress(uint itemId , List<EItemStruct1> item)
	{
		//public uint itemId;
		//public List item;
	}

}
