using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;

namespace PLAgentDll
{
	internal class XmlControl
	{
		public struct agent_defined_status
		{
			public string status_id;

			public string status_name;
		}

		private static XmlTextReader xtr;

		private static ILog XmlLog;

		public static void init()
		{
			XmlControl.XmlLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			XmlControl.XmlLog.Info("xml class init success！");
		}

		public static DataSet Read(string strXml)
		{
			byte[] array = new byte[1024];
			DataSet dataSet = new DataSet();
			StringReader reader = new StringReader(strXml);
			dataSet.ReadXml(reader);
			return dataSet;
		}

		public static DataView Read(string strXml, string Where)
		{
			DataSet dataSet = new DataSet();
			StringReader reader = new StringReader(strXml);
			dataSet.ReadXml(reader);
			return new DataView(dataSet.Tables[0])
			{
				RowFilter = Where
			};
		}

		public static object Query(string strXml, string Where, string Field)
		{
			DataSet dataSet = new DataSet();
			StringReader reader = new StringReader(strXml);
			dataSet.ReadXml(reader);
			return new DataView(dataSet.Tables[0])
			{
				RowFilter = Where
			}[0][Field].ToString();
		}

		public static string[] Query(string strXml, string Where, string[] Field)
		{
			DataSet dataSet = new DataSet();
			StringReader stringReader = new StringReader(strXml);
			dataSet.ReadXml(strXml);
			DataView dataView = new DataView(dataSet.Tables[0]);
			dataView.RowFilter = Where;
			List<string> list = new List<string>();
			for (int i = 0; i < Field.Length; i++)
			{
				string property = Field[i];
				list.Add(dataView[0][property].ToString());
			}
			return list.ToArray();
		}

		public static AgentEvent ReadOneXml(string strXml)
		{
			AgentEvent result = default(AgentEvent);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(strXml);
			XmlElement documentElement = xmlDocument.DocumentElement;
			result = XmlControl.OutputElementNameValue(documentElement, ref result);
			return result;
		}

		private static void InitAgentEvents(ref AgentEvent newAgentEvent)
		{
			newAgentEvent.agentID = "";
			newAgentEvent.callerID = "";
			newAgentEvent.calledID = "";
			newAgentEvent.cuID = "";
			newAgentEvent.deAgentEventType = AgentEventType.AGENT_EVENT_Unknow;
			newAgentEvent.eEventQualifier = (EventQualifier)0;
			newAgentEvent.eventMsg = "";
			newAgentEvent.extenNum = "";
			newAgentEvent.reason = "";
			newAgentEvent.retCode = -1;
			newAgentEvent.agent_call_uuid = "";
		}

		public static AgentEvent OutputElementNameValue(XmlElement root, ref AgentEvent tmpAgentEvents)
		{
			string text = root.Name.ToLower();
			if (text != null)
			{
				if (!(text == "response"))
				{
					if (!(text == "event"))
					{
						if (text == "define")
						{
							tmpAgentEvents.deAgentEventType = AgentEventType.AGENT_EVENT_MYDEFINE;
						}
					}
					else
					{
						tmpAgentEvents.deAgentEventType = AgentEventType.AGENT_EVENT_EVENT;
					}
				}
				else
				{
					tmpAgentEvents.deAgentEventType = AgentEventType.AGENT_EVENT_RESPONSE;
				}
			}
			foreach (XmlNode xmlNode in root)
			{
				if (xmlNode is XmlElement)
				{
					XmlElement xmlElement = xmlNode as XmlElement;
					text = xmlElement.Name.ToLower();
					switch (text)
					{
					case "agentid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agentID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "extennum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.extenNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "status_map":
						tmpAgentEvents.agentDefineStatus = new Dictionary<string, string>();
						break;
					case "website_map":
						tmpAgentEvents.agentWebSiteInfo = new Dictionary<string, string>();
						break;
					case "ivr_list":
						tmpAgentEvents.ivr_list = new Dictionary<string, string>();
						break;
					case "queue_list":
						tmpAgentEvents.queue_list = new Dictionary<string, string>();
						break;
					case "statusmaxnumlist":
						tmpAgentEvents.status_max_num_list = new Dictionary<string, string>();
						break;
					case "ivr_profile_list":
						tmpAgentEvents.ivr_profile_list = new Dictionary<string, string>();
						break;
					case "agentgroup_list":
						tmpAgentEvents.group_list = new Dictionary<string, string>();
						break;
					case "report_statis_list":
						tmpAgentEvents.report_statis_info_map = new Dictionary<string, string>();
						break;
					case "retcode":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.retCode = Convert.ToInt32(xmlElement.ChildNodes[0].Value);
						}
						break;
					case "reason":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.reason = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "cuid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.cuID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "hangup_reason":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.hangupReason = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "callerid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.callerID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "calledid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.calledID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "destagentid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.destAgentID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentdata":
					{
						string[] array = new string[xmlElement.Attributes.Count];
						for (int i = 0; i < xmlElement.Attributes.Count; i++)
						{
							array[i] = xmlElement.Attributes[i].Value;
						}
						tmpAgentEvents.agentInfoList.Add(array);
						break;
					}
					case "agentexten":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agentExten = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentgroupid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agentGroupID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentname":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agentName = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "autoposttreatment":
						if (xmlElement.HasChildNodes)
						{
							if (xmlElement.ChildNodes[0].Value == "true")
							{
								tmpAgentEvents.autoPostTreatment = true;
							}
							else
							{
								tmpAgentEvents.autoPostTreatment = false;
							}
						}
						break;
					case "is_bind_exten":
						if (xmlElement.HasChildNodes)
						{
							if (xmlElement.ChildNodes[0].Value == "1")
							{
								tmpAgentEvents.bindExten = true;
							}
							else
							{
								tmpAgentEvents.bindExten = false;
							}
						}
						break;
					case "bindexten":
						if (xmlElement.HasChildNodes)
						{
							if (xmlElement.ChildNodes[0].Value == "true")
							{
								tmpAgentEvents.bindExten = true;
							}
							else
							{
								tmpAgentEvents.bindExten = false;
							}
						}
						break;
					case "gradeswitch":
						if (xmlElement.HasChildNodes)
						{
							if (xmlElement.ChildNodes[0].Value == "true")
							{
								tmpAgentEvents.gradeSwitch = true;
							}
							else
							{
								tmpAgentEvents.gradeSwitch = false;
							}
						}
						break;
					case "initstatus":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.initStatus = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "roleid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.roleID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "salt_key":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.salt_key = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "did_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.DID_Num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "status":
					{
						string text2 = "";
						string text3 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "status_id"))
								{
									if (text == "status_name")
									{
										text3 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text2 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (text2 != "" && text3 != "")
						{
							tmpAgentEvents.agentDefineStatus.Add(text2, text3);
						}
						continue;
					}
					case "website":
					{
						string text4 = "";
						string text5 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "website_name"))
								{
									if (text == "website_url")
									{
										text5 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text4 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (text4 != "" && text5 != "")
						{
							tmpAgentEvents.agentWebSiteInfo.Add(text4, text5);
						}
						continue;
					}
					case "ivr":
					{
						string text6 = "";
						string text7 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "ivr_num"))
								{
									if (text == "ivr_name")
									{
										text7 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text6 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (text6 != "" && text7 != "")
						{
							tmpAgentEvents.ivr_list.Add(text6, text7);
						}
						continue;
					}
					case "ivr_profile":
					{
						string text8 = "";
						string text9 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "ivr_profile_num"))
								{
									if (text == "ivr_profile_name")
									{
										text9 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text8 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (string.Empty != text8 && string.Empty != text9)
						{
							tmpAgentEvents.ivr_profile_list.Add(text8, text9);
						}
						continue;
					}
					case "queue":
					{
						string text10 = "";
						string text11 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "queue_num"))
								{
									if (text == "queue_name")
									{
										text11 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text10 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (text10 != "" && text11 != "")
						{
							tmpAgentEvents.queue_list.Add(text10, text11);
						}
						continue;
					}
					case "agentgroup":
					{
						string text12 = "";
						string text13 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "agentgroup_num"))
								{
									if (text == "agentgroup_name")
									{
										text13 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text12 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (text12 != "" && text13 != "")
						{
							tmpAgentEvents.group_list.Add(text12, text13);
						}
						continue;
					}
					case "statusmaxnuminfo":
					{
						string text14 = "";
						string text15 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							if (text != null)
							{
								if (!(text == "agentgroupname"))
								{
									if (text == "statusmaxnum")
									{
										text15 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
								else
								{
									text14 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								}
							}
						}
						if (text14 != "" && text15 != "")
						{
							tmpAgentEvents.status_max_num_list.Add(text14, text15);
						}
						continue;
					}
					case "report_statis_info":
					{
						string text16 = "";
						string text17 = "";
						string text18 = "";
						string text19 = "";
						string text20 = "";
						string text21 = "";
						string text22 = "";
						for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
						{
							text = xmlElement.ChildNodes[i].Name.ToLower();
							switch (text)
							{
							case "todaycallin":
								text16 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							case "todayabandon":
								text17 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							case "todaycallinanswer":
								text18 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							case "todaycallout":
								text19 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							case "todaycalloutanswer":
								text20 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							case "todayenterqueue":
								text21 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							case "todayenterqueueother":
								text22 = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
								break;
							}
						}
						if (text16 == "")
						{
							text16 = "0";
						}
						if (text17 == "")
						{
							text17 = "0";
						}
						if (text18 == "")
						{
							text18 = "0";
						}
						if (text19 == "")
						{
							text19 = "0";
						}
						if (text20 == "")
						{
							text20 = "0";
						}
						if (text21 == "")
						{
							text21 = "0";
						}
						if (text22 == "")
						{
							text22 = "0";
						}
						tmpAgentEvents.report_statis_info_map.Add("todaycallin", text16);
						tmpAgentEvents.report_statis_info_map.Add("todayabandon", text17);
						tmpAgentEvents.report_statis_info_map.Add("todaycallinanswer", text18);
						tmpAgentEvents.report_statis_info_map.Add("todaycallout", text19);
						tmpAgentEvents.report_statis_info_map.Add("todaycalloutanswer", text20);
						tmpAgentEvents.report_statis_info_map.Add("todayenterqueue", text21);
						tmpAgentEvents.report_statis_info_map.Add("todayenterqueueother", text22);
						continue;
					}
					case "status_change_agentid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.status_change_agentid = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "status_before":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.status_before = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "status_after":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.status_after = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "exception_reason":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.exception_reason = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customernum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.callerID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "callcenternum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.calledID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "accessnumname":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.access_num_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "areaid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.area_id = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "areaname":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.area_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customergrade":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.cust_grade = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "relation_uuid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.relation_uuid = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "ctichannelmarkstr":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.makeStr = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "outextraparamsfromivr":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.outExtraParamsFromIvr = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "date":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.todayDate = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "hangupactiveflag":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.HangupActiveFlag = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "hangupcause":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.hangupReason = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "hangupuuid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.hangupUuid = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agent_group_name":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_group_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentgroupname":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_group_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "calledagentnum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.calledAgentNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "calleragentnum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.callerAgentNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "access_numbers_str":
						if (xmlElement.HasChildNodes)
						{
							string text23 = xmlElement.ChildNodes[0].Value ?? "";
							tmpAgentEvents.accessNumbers = text23.Split(new char[]
							{
								','
							});
						}
						break;
					case "consulteragentnum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.consulterAgentNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "targetagentnum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.destAgentID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "original_call_type":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.call_type = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "specificagentnum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.specificAgentNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "queue_num":
					case "queuenum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.queue_num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "transfee_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.transfee_num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "noansweredalarmflag":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.no_answered_alram_flag = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentuuid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_call_uuid = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "filepath":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.filePath = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "isevaluated":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.isEvaluated = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "evaluatestatus":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.evaluateStatus = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "evaluatedefaultresult":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.evaluate_default_result = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customeruuid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customerUuid = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "evaluatescore":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.evaluateScore = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "predictcustomername":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.predictCustomerName = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "predictcustomerremark":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.predictCustomerRemark = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "predictcustomerforeignid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.predictCustomerForeignId = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "calleenum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.calleeNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "callernum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.callerNum = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_foreign_id":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_foreign_id = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "access_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.access_num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_num_format_local":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_num_format_local = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_num_format_national":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_num_format_national = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_num_format_e164":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_num_format_e164 = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_num_phone_type":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_num_phone_type = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "transfer_time":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.transfer_time = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agent_list":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_list = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentgroupnum":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_group_num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "current_time":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.current_time = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "start_talking_time":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.start_talking_time = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "call_type":
					case "calltype":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.call_type = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "queuenumlststr":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.queueNumLstStr = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "targetagentgroupnamelist ":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agentGroupNameLstStr = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "enter_queue_time":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.enter_queue_time = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "queue_name":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.queue_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "queue_customer_amount":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.queue_customer_amount = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "early_queue_enter_time":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.early_queue_enter_time = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "early_queue_enter_time_all":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.early_queue_enter_time_all = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_enter_channel":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_enter_channel = (xmlElement.ChildNodes[0].Value ?? "");
						}
						else
						{
							tmpAgentEvents.customer_enter_channel = "";
						}
						break;
					case "customer_type":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_type = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_status":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_status = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.callerID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "callcenter_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.calledID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "customer_uuid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.customer_uuid = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "heartbeat_timeout":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.heartbeat_timeout = Convert.ToInt32(xmlElement.ChildNodes[0].Value);
						}
						break;
					case "agentmobile":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_mobile = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agentemail":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_email = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "targetagentgroupname":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agent_group_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "targetagentgroupid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.agentGroupID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "exclusive_agent_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.exclusive_agent_num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "exclusive_queue_num":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.exclusive_queue_num = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "role_map":
						tmpAgentEvents.agent_role_and_right = new List<Agent_Role_And_Right_Struct>();
						break;
					case "online_agents":
						tmpAgentEvents.agent_online = new List<Agent_Online_Struct>();
						break;
					case "leg_list":
						tmpAgentEvents.leg_info_list = new List<Leg_Info_Struct>();
						break;
					case "relation_list":
						tmpAgentEvents.relation_info_list = new List<Relation_Info_Struct>();
						break;
					case "customer_list":
						tmpAgentEvents.customer_info_list = new List<Customer_Info_Struct>();
						break;
					case "queue_statis_list":
						tmpAgentEvents.queue_statis_list = new List<Queue_Statis_Struct>();
						break;
					case "apply_agent_list":
						tmpAgentEvents.apply_change_status_list = new List<Apply_Change_Status>();
						break;
					case "applyagentid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.apply_agent_id = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "applyagentname":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.apply_agent_name = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "targetstatus":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.targetStatus = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "applytime":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.applyTime = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "applytype":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.applyType = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "approveresult":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.approveResult = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "approvetime":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.approveTime = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "timeouttype":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.timeoutType = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "uuida":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.bridge_uuidA = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "uuidb":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.bridge_uuidB = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					case "agent":
						if (xmlElement.HasChildNodes)
						{
							Agent_Online_Struct item = default(Agent_Online_Struct);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								switch (text)
								{
								case "agent_name":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.agentName = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.agentNum = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_status":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.agentStatus = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "bind_exten":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										if (xmlElement.ChildNodes[i].FirstChild.Value == "true")
										{
											item.bindExten = true;
										}
										else
										{
											item.bindExten = false;
										}
									}
									break;
								case "agentgroup_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.agentgroup_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_group_name":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.agentgroup_name = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "flag":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.flag = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "role_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.roleNum = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "status_change_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.status_change_time = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "customer_enter_channel":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.customer_enter_channel = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "start_talking_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item.start_talking_time = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								}
							}
							if (tmpAgentEvents.agent_online == null)
							{
								tmpAgentEvents.agent_online = new List<Agent_Online_Struct>();
							}
							tmpAgentEvents.agent_online.Add(item);
						}
						continue;
					case "leg":
						if (xmlElement.HasChildNodes)
						{
							Leg_Info_Struct item2 = default(Leg_Info_Struct);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								if (text != null)
								{
									if (!(text == "agent_num"))
									{
										if (!(text == "end_num"))
										{
											if (!(text == "end_is_outbound"))
											{
												if (!(text == "leg_status"))
												{
													if (!(text == "leg_uuid"))
													{
														if (text == "lgr")
														{
															if (xmlElement.ChildNodes[i].HasChildNodes)
															{
																item2.lgr = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
															}
														}
													}
													else if (xmlElement.ChildNodes[i].HasChildNodes)
													{
														item2.leg_uuid = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
													}
												}
												else if (xmlElement.ChildNodes[i].HasChildNodes)
												{
													item2.leg_status = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
												}
											}
											else if (xmlElement.ChildNodes[i].HasChildNodes)
											{
												if (xmlElement.ChildNodes[i].FirstChild.Value == "true")
												{
													item2.end_is_outbound = true;
												}
												else
												{
													item2.end_is_outbound = false;
												}
											}
										}
										else if (xmlElement.ChildNodes[i].HasChildNodes)
										{
											item2.end_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
										}
									}
									else if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item2.agentNum = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
							}
							tmpAgentEvents.leg_info_list.Add(item2);
						}
						continue;
					case "relation":
						if (xmlElement.HasChildNodes)
						{
							Relation_Info_Struct item3 = default(Relation_Info_Struct);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								if (text != null)
								{
									if (!(text == "callee_uuid"))
									{
										if (!(text == "caller_uuid"))
										{
											if (text == "relation_name")
											{
												if (xmlElement.ChildNodes[i].HasChildNodes)
												{
													item3.relation_name = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
												}
											}
										}
										else if (xmlElement.ChildNodes[i].HasChildNodes)
										{
											item3.caller_uuid = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
										}
									}
									else if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item3.callee_uuid = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
								}
							}
							tmpAgentEvents.relation_info_list.Add(item3);
						}
						continue;
					case "role":
						if (xmlElement.HasChildNodes)
						{
							Agent_Role_And_Right_Struct item4 = default(Agent_Role_And_Right_Struct);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								switch (text)
								{
								case "role_id":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item4.role_id = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "role_name":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item4.role_name = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "controled_agent_group_lst":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item4.controled_agent_group_lst = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "controled_queue_lst":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item4.controled_queue_lst = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "rights_of_bargein":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_bargein = true;
									}
									else
									{
										item4.rights_of_bargein = false;
									}
									break;
								case "rights_of_callout":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_callout = true;
									}
									else
									{
										item4.rights_of_callout = false;
									}
									break;
								case "rights_of_eavesdrop":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_eavesdrop = true;
									}
									else
									{
										item4.rights_of_eavesdrop = false;
									}
									break;
								case "rights_of_intercept":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_forcehangup = true;
									}
									else
									{
										item4.rights_of_forcehangup = false;
									}
									break;
								case "rights_of_view_agent_group_info":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_view_agent_group_info = true;
									}
									else
									{
										item4.rights_of_view_agent_group_info = false;
									}
									break;
								case "rights_of_view_cdr":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_view_cdr = true;
									}
									else
									{
										item4.rights_of_view_cdr = false;
									}
									break;
								case "rights_of_view_queue_info":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_view_queue_info = true;
									}
									else
									{
										item4.rights_of_view_queue_info = false;
									}
									break;
								case "rights_of_view_queue_message":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_view_queue_message = true;
									}
									else
									{
										item4.rights_of_view_queue_message = false;
									}
									break;
								case "rights_of_view_recording":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_view_recording = true;
									}
									else
									{
										item4.rights_of_view_recording = false;
									}
									break;
								case "rights_of_whisper":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_whisper = true;
									}
									else
									{
										item4.rights_of_whisper = false;
									}
									break;
								case "rights_of_force_change_status_idle":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_force_change_status_idle = true;
									}
									else
									{
										item4.rights_of_force_change_status_idle = false;
									}
									break;
								case "rights_of_force_change_status_busy":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_force_change_status_busy = true;
									}
									else
									{
										item4.rights_of_force_change_status_busy = false;
									}
									break;
								case "rights_of_force_change_status_leave":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.rights_of_force_change_status_leave = true;
									}
									else
									{
										item4.rights_of_force_change_status_leave = false;
									}
									break;
								case "role_right1":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.role_right1 = true;
									}
									else
									{
										item4.role_right1 = false;
									}
									break;
								case "role_right2":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.role_right2 = true;
									}
									else
									{
										item4.role_right2 = false;
									}
									break;
								case "role_right3":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.role_right3 = true;
									}
									else
									{
										item4.role_right3 = false;
									}
									break;
								case "role_right4":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.role_right4 = true;
									}
									else
									{
										item4.role_right4 = false;
									}
									break;
								case "role_right5":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.role_right5 = true;
									}
									else
									{
										item4.role_right5 = false;
									}
									break;
								case "group_role_right1":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.group_role_right1 = true;
									}
									else
									{
										item4.group_role_right1 = false;
									}
									break;
								case "group_role_right2":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.group_role_right2 = true;
									}
									else
									{
										item4.group_role_right2 = false;
									}
									break;
								case "group_role_right3":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.group_role_right3 = true;
									}
									else
									{
										item4.group_role_right3 = false;
									}
									break;
								case "group_role_right4":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.group_role_right4 = true;
									}
									else
									{
										item4.group_role_right4 = false;
									}
									break;
								case "group_role_right5":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.group_role_right5 = true;
									}
									else
									{
										item4.group_role_right5 = false;
									}
									break;
								case "queue_role_right1":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.queue_role_right1 = true;
									}
									else
									{
										item4.queue_role_right1 = false;
									}
									break;
								case "queue_role_right2":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.queue_role_right2 = true;
									}
									else
									{
										item4.queue_role_right2 = false;
									}
									break;
								case "queue_role_right3":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.queue_role_right3 = true;
									}
									else
									{
										item4.queue_role_right3 = false;
									}
									break;
								case "queue_role_right4":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.queue_role_right4 = true;
									}
									else
									{
										item4.queue_role_right4 = false;
									}
									break;
								case "queue_role_right5":
									if (xmlElement.ChildNodes[i].FirstChild.Value == "1")
									{
										item4.queue_role_right5 = true;
									}
									else
									{
										item4.queue_role_right5 = false;
									}
									break;
								}
							}
							tmpAgentEvents.agent_role_and_right.Add(item4);
						}
						continue;
					case "customer":
						if (xmlElement.HasChildNodes)
						{
							Customer_Info_Struct item5 = default(Customer_Info_Struct);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								switch (text)
								{
								case "call_type":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.call_type = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "callcenter_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.callcenter_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "customer_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.customer_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "customer_status":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.customer_status = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "customer_type":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.customer_type = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "customer_uuid":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.customer_uuid = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "enter_queue_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.enter_queue_time = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "queue_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.queue_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "customer_enter_channel":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.customer_enter_channel = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "exclusive_agent_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.exclusive_agent_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "exclusive_queue_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item5.exclusive_queue_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								}
							}
							if (tmpAgentEvents.customer_info_list == null)
							{
								tmpAgentEvents.customer_info_list = new List<Customer_Info_Struct>();
							}
							tmpAgentEvents.customer_info_list.Add(item5);
						}
						continue;
					case "queue_statis_info":
						if (xmlElement.HasChildNodes)
						{
							Queue_Statis_Struct item6 = default(Queue_Statis_Struct);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								switch (text)
								{
								case "queue_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.queue_num = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "queue_name":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.queue_name = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "min_enter_queue_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.min_enter_queue_time = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agentnumlststr":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agentNumLstStr = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "queue_wait_people_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.queue_wait_people_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_idle_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_idle_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_talking_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_talking_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_busy_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_busy_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_leave_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_leave_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_offline_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_offline_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_online_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_online_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_no_exten_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_no_exten_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_post_treatment":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_post_treatment = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_callout_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_callout_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_calling_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_calling_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_ring_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_ring_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_hold_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_hold_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_mute_amount":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.agent_mute_amount = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "early_queue_enter_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.early_queue_enter_time = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "early_queue_enter_time_all":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item6.early_queue_enter_time_all = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								}
							}
							if (tmpAgentEvents.queue_statis_list == null)
							{
								tmpAgentEvents.queue_statis_list = new List<Queue_Statis_Struct>();
							}
							tmpAgentEvents.queue_statis_list.Add(item6);
						}
						continue;
					case "apply_agent_info":
						if (xmlElement.HasChildNodes)
						{
							Apply_Change_Status item7 = default(Apply_Change_Status);
							for (int i = 0; i < xmlElement.ChildNodes.Count; i++)
							{
								text = xmlElement.ChildNodes[i].Name.ToLower();
								switch (text)
								{
								case "agent_group_name":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.applyAgentGroupName = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "agent_group_num":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.applyAgentGroupID = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "applyagentid":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.applyAgentID = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "applyagentname":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.agentName = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "apply_status":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.applyStateStr = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "apply_target_status":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.targetStatus = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "apply_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.applyTime = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "approve_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.approveTime = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "status_change_time":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.executeTime = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								case "applytype":
									if (xmlElement.ChildNodes[i].HasChildNodes)
									{
										item7.applyType = (xmlElement.ChildNodes[i].FirstChild.Value ?? "");
									}
									break;
								}
							}
							if (tmpAgentEvents.apply_change_status_list == null)
							{
								tmpAgentEvents.apply_change_status_list = new List<Apply_Change_Status>();
							}
							tmpAgentEvents.apply_change_status_list.Add(item7);
						}
						continue;
					case "cmdtype":
					case "eventtype":
						text = xmlElement.ChildNodes[0].Value.ToLower();
						switch (text)
						{
						case "response_timeout":
							tmpAgentEvents.eEventQualifier = EventQualifier.Sys_ResponseTimeOut;
							break;
						case "login":
							tmpAgentEvents.eEventQualifier = EventQualifier.Login_Status;
							break;
						case "logout":
							tmpAgentEvents.eEventQualifier = EventQualifier.LogOut_Status;
							break;
						case "signin":
							tmpAgentEvents.eEventQualifier = EventQualifier.SignIn_Status;
							break;
						case "signout":
							tmpAgentEvents.eEventQualifier = EventQualifier.SignOut_Status;
							break;
						case "kickout":
							tmpAgentEvents.eEventQualifier = EventQualifier.Sys_KickOut;
							break;
						case "offline":
							tmpAgentEvents.eEventQualifier = EventQualifier.Offline_Status;
							break;
						case "disconnect":
							tmpAgentEvents.eEventQualifier = EventQualifier.Socket_Disconnect;
							break;
						case "bridge":
							tmpAgentEvents.eEventQualifier = EventQualifier.Bridge_NormalCall;
							break;
						case "callee_answer":
							tmpAgentEvents.eEventQualifier = EventQualifier.Callee_Answered;
							break;
						case "answer":
							tmpAgentEvents.eEventQualifier = EventQualifier.Answered_NormalCall;
							break;
						case "callin":
							tmpAgentEvents.eEventQualifier = EventQualifier.CallIn_NormalCall;
							break;
						case "callout":
							tmpAgentEvents.eEventQualifier = EventQualifier.CallOut_NormalCall;
							break;
						case "callout_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.CallOut_Ring;
							break;
						case "hangup":
							tmpAgentEvents.eEventQualifier = EventQualifier.HangUp_NormalCall;
							break;
						case "hangup_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.HangUp_NormalCallResult;
							break;
						case "signin_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.SignIn_Ring_NormalCall;
							break;
						case "signin_play":
							tmpAgentEvents.eEventQualifier = EventQualifier.SignIn_Play_NormalCall;
							break;
						case "heartbeat":
							tmpAgentEvents.eEventQualifier = EventQualifier.Socket_HeartBeat;
							break;
						case "echo_test_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Check_ExtenStatus;
							break;
						case "echo_test":
							tmpAgentEvents.eEventQualifier = EventQualifier.Check_ExtenStatus;
							break;
						case "hold_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Hold_NormalCall;
							break;
						case "unhold_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.StopHold_NormalCall;
							break;
						case "mute_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Mute_NormalCall;
							break;
						case "unmute_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.StopMute_NormalCall;
							break;
						case "eavesdrop":
						case "eavesdrop_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Eavesdrop_Result;
							break;
						case "stoplisten":
							tmpAgentEvents.eEventQualifier = EventQualifier.StopListen_NormalCall;
							break;
						case "consult_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Consult_Call_In;
							break;
						case "consult":
						case "consult_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Consult_Call_Result;
							break;
						case "consult_cancel_result":
						case "consult_cancel":
							tmpAgentEvents.eEventQualifier = EventQualifier.Consult_Cancel_Result;
							break;
						case "intercept":
							tmpAgentEvents.eEventQualifier = EventQualifier.InterceptCall_NormalCall;
							break;
						case "interrupt":
							tmpAgentEvents.eEventQualifier = EventQualifier.Interrupt_NormalCall;
							break;
						case "forcedisconnect":
							tmpAgentEvents.eEventQualifier = EventQualifier.ForceDisconnect_NormalCall;
							break;
						case "getagentlist":
							tmpAgentEvents.eEventQualifier = EventQualifier.Monitor_Agent;
							break;
						case "getqueuelist":
							tmpAgentEvents.eEventQualifier = EventQualifier.Monitor_Queue;
							break;
						case "get_defined_status":
							tmpAgentEvents.eEventQualifier = EventQualifier.Sys_GetAgentDefineStatus;
							break;
						case "get_website_info":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Website_Info;
							break;
						case "agent_status_change":
						case "change_status":
							tmpAgentEvents.eEventQualifier = EventQualifier.Sys_AgentStatusChange;
							break;
						case "change_status_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Sys_AgentStatusChangeResult;
							break;
						case "exception":
							tmpAgentEvents.eEventQualifier = EventQualifier.Sys_ThrowException;
							break;
						case "common_callin_bridge_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.CallIn_NormalCall;
							break;
						case "manual_callout_agent_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.CallOut_Ring;
							break;
						case "preview_callout":
						case "manual_callout":
							tmpAgentEvents.eEventQualifier = EventQualifier.Manual_Callout;
							break;
						case "internal_call":
							tmpAgentEvents.eEventQualifier = EventQualifier.Internal_Call;
							break;
						case "internal_call_caller_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Internal_Caller_Ring;
							break;
						case "internal_call_called_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Internal_Call_CallIn;
							break;
						case "evaluate":
						case "evaluate_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Grade_NormalCall;
							break;
						case "evaluate_result_event":
							tmpAgentEvents.eEventQualifier = EventQualifier.Evaluate_Result_Event;
							break;
						case "predict_callout_bridge_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Predict_CallOut_Bridge_Ring;
							break;
						case "get_access_numbers":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Access_Numbers;
							break;
						case "threeway_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Three_Way_Call_Result;
							break;
						case "threeway_cancel_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Three_Way_Call_Cancel_Result;
							break;
						case "consult_transfer":
							tmpAgentEvents.eEventQualifier = EventQualifier.Consult_Transfer_Result;
							break;
						case "threeway":
							tmpAgentEvents.eEventQualifier = EventQualifier.Three_Way_Call_Result;
							break;
						case "threeway_cancel":
							tmpAgentEvents.eEventQualifier = EventQualifier.Three_Way_Call_Cancel_Result;
							break;
						case "threeway_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Three_Way_Call_In;
							break;
						case "whisper":
						case "whisper_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Whisper_Result;
							break;
						case "force_change_status":
						case "force_change_status_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Force_Change_Status_Result;
							break;
						case "bargein":
						case "bargein_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Bargein_Result;
							break;
						case "force_hangup_result":
						case "force_hangup":
							tmpAgentEvents.eEventQualifier = EventQualifier.Force_Hangup_Result;
							break;
						case "eavesdrop_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Eavesdrop_Ring_Myself;
							break;
						case "whisper_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Whisper_Ring_Myself;
							break;
						case "bargein_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Bargein_Ring_Myself;
							break;
						case "force_hangup_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Force_Hangup_Ring_Myself;
							break;
						case "blind_transfer":
						case "blind_transfer_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Transfer_Blind_Agent_NormalCall;
							break;
						case "blind_transfer_ring":
							tmpAgentEvents.eEventQualifier = EventQualifier.Transfer_Blind_Call_In;
							break;
						case "transfer_ivr":
						case "transfer_ivr_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Transfer_IVR_NormalCall;
							break;
						case "transfer_queue":
						case "transfer_queue_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Transfer_Queue_NormalCall;
							break;
						case "transfer_ivr_profile":
						case "transfer_ivr_profile_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Transfer_IVR_Profile_NormalCall;
							break;
						case "get_online_agents":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Online_Agents;
							break;
						case "get_ivr_list":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Ivr_List;
							break;
						case "get_queue_list":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Queue_List;
							break;
						case "get_ivr_profile_list":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Ivr_Profile_List;
							break;
						case "get_defined_role_rights":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Defined_Role_Rights;
							break;
						case "get_agentgroup_list":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Agent_Group_List;
							break;
						case "get_report_statis_info":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Report_Statis_Info;
							break;
						case "get_agents_of_queue":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Agents_Of_Queue;
							break;
						case "get_agents_of_agentgroup":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Agents_Of_Agent_Group;
							break;
						case "get_agents_monitor_info":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Agents_Monitor_Info;
							break;
						case "get_detail_call_info":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Detail_Call_Info;
							break;
						case "get_customer_of_queue":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Customer_Of_Queue;
							break;
						case "add_customer_to_queue":
							tmpAgentEvents.eEventQualifier = EventQualifier.Add_Customer_To_Queue;
							break;
						case "del_customer_from_queue":
							tmpAgentEvents.eEventQualifier = EventQualifier.Del_Customer_From_Queue;
							break;
						case "update_customer_of_queue":
							tmpAgentEvents.eEventQualifier = EventQualifier.Update_Customer_Of_Queue;
							break;
						case "consultee_hangup":
							tmpAgentEvents.eEventQualifier = EventQualifier.Consultee_Hangup_Call;
							break;
						case "threewayee_hangup":
							tmpAgentEvents.eEventQualifier = EventQualifier.Threewayee_Hangup_Call;
							break;
						case "warn_agent_resignin":
							tmpAgentEvents.eEventQualifier = EventQualifier.Warn_Agent_Resignin;
							break;
						case "warn_agent_force_change_status":
							tmpAgentEvents.eEventQualifier = EventQualifier.Warn_Agent_Force_Change_Status;
							break;
						case "blind_transfer_outbound_failed":
							tmpAgentEvents.eEventQualifier = EventQualifier.Blind_Transfer_Outbound_Failed;
							break;
						case "queue_transfer_outbound":
							tmpAgentEvents.eEventQualifier = EventQualifier.Queue_Transfer_Outbound;
							break;
						case "get_queue_statis":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Queue_Statis;
							break;
						case "get_all_queue_statis":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_All_Queue_Statis;
							break;
						case "get_customer_of_my_queue":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Customer_Of_My_Queue;
							break;
						case "get_agent_personal_info":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Agent_Personal_info;
							break;
						case "set_agent_personal_info":
							tmpAgentEvents.eEventQualifier = EventQualifier.Set_Agent_Personal_info;
							break;
						case "change_pswd":
							tmpAgentEvents.eEventQualifier = EventQualifier.Change_Pswd;
							break;
						case "apply_change_status":
						case "apply_change_status_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Apply_Change_Status;
							break;
						case "apply_change_status_distribute":
							tmpAgentEvents.eEventQualifier = EventQualifier.Apply_Change_Status_Distribute;
							break;
						case "approve_change_status_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Approve_Change_Status_Result;
							break;
						case "approve_change_status_distribute":
							tmpAgentEvents.eEventQualifier = EventQualifier.Approve_Change_Status_Distribute;
							break;
						case "get_agentgroup_status_max_num":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Agentgroup_Status_Max_Num;
							break;
						case "set_agentgroup_status_max_num":
							tmpAgentEvents.eEventQualifier = EventQualifier.Set_Agentgroup_Status_Max_Num;
							break;
						case "apply_or_approve_change_status_timeout_distribute":
							tmpAgentEvents.eEventQualifier = EventQualifier.Apply_or_Approve_Change_Status_Timeout_Distribute;
							break;
						case "cancel_apply_change_status":
						case "cancel_apply_change_status_result":
							tmpAgentEvents.eEventQualifier = EventQualifier.Apply_Change_Status_Cancel;
							break;
						case "cancel_apply_change_status_distribute":
							tmpAgentEvents.eEventQualifier = EventQualifier.Apply_Change_Status_Cancel_Distribute;
							break;
						case "get_change_status_apply_list":
							tmpAgentEvents.eEventQualifier = EventQualifier.Get_Change_Status_Apply_List;
							break;
						case "record_start":
							tmpAgentEvents.eEventQualifier = EventQualifier.Record_Start;
							break;
						case "record_stop":
							tmpAgentEvents.eEventQualifier = EventQualifier.Record_Stop;
							break;
						}
						break;
					case "executoragentid":
						if (xmlElement.HasChildNodes)
						{
							tmpAgentEvents.executorAgentID = (xmlElement.ChildNodes[0].Value ?? "");
						}
						break;
					}
					if (xmlNode.HasChildNodes)
					{
						XmlControl.OutputElementNameValue(xmlNode as XmlElement, ref tmpAgentEvents);
					}
				}
			}
			return tmpAgentEvents;
		}
	}
}
