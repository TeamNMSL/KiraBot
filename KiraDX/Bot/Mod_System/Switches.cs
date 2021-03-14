using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mirai_CSharp;
using Mirai_CSharp.Models;

namespace KiraDX.Bot.Mod_System
{
  public static   class Switches
    {
        public static void SwitchOn(GroupMsg g, IGroupMessageEventArgs e) {
            try
            {
                if (!File.Exists($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira"))
                {
                    File.Create($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira").Close();
                }
                //string cfg = File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira");
                string cfg = Users.Info.GetGroupConfig(g.fromGroup).switches;
                if (cfg.ToLower().Contains("[locked]"))
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"本群模块已被管理团队锁定，无法切换状态");
                    return;
                }
                if (e.Sender.Permission==GroupPermission.Administrator|| e.Sender.Permission == GroupPermission.Owner||g.fromAccount==1848200159||g.fromAccount==G.BotList.Laffy || g.fromAccount == G.BotList.Miffy || g.fromAccount == G.BotList.Soffy)
                {
                    // /k mod enable cao
                    string[] cmd = g.msg.Split(' ');
                    string[] FuncList = Users.cfgs.FunctionList.Split(';');
                    
                    foreach (var item in FuncList)
                    {
                        if (item == cmd[3])
                        {

                            if (File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}0.kira").Contains(item))
                            {
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"{item}模块在全局被设置为关闭，无法修改");
                                return;
                            }

                            cfg =cfg.Replace(item, " ");
                            
                            File.WriteAllText($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira", cfg);
                            Users.Info.GroupInfo[g.fromGroup].switches = cfg;
                            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"已开启{item}模块");
                            return;
                        }
                    }
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"没有找到模块{cmd[3]}，可能大小写问题？");
                }
                else
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你冇权限啦！w");
                    return;
                }
                
            }
            catch (Exception ex)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, ex.Message);
                return;
            }

        }

        public static void SwitchOff(GroupMsg g, IGroupMessageEventArgs e)
        {
            try
            {
                if (!File.Exists($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira"))
                {
                    File.Create($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira");
                }
                //string cfg = File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira");
                string cfg = Users.Info.GetGroupConfig(g.fromGroup).switches;
                if (cfg.ToLower().Contains("[locked]"))
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"本群模块已被管理团队锁定，无法切换状态");
                    return;
                }
                if (e.Sender.Permission == GroupPermission.Administrator || e.Sender.Permission == GroupPermission.Owner || g.fromAccount == 1848200159 || g.fromAccount == G.BotList.Laffy || g.fromAccount == G.BotList.Miffy || g.fromAccount == G.BotList.Soffy)
                {
                    if (!File.Exists($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira"))
                    {
                        File.Create($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira").Close();
                    }
                    // /k mod disable cao
                    string[] cmd = g.msg.Split(' ');
                    string[] FuncList = File.ReadAllText($"{G.path.Apppath}FunctionList.Kira").Split(';');
                    foreach (var item in FuncList)
                    {
                        if (item == cmd[3])
                        {
                            if (File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}0.kira").Contains(item))
                            {
                                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"{item}模块在全局被设置为关闭，无法修改");
                                return;
                            }
                            cfg += $"\n{item}";
                            File.WriteAllText($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira", cfg);
                            Users.Info.GroupInfo[g.fromGroup].switches = cfg;
                            KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"已关闭{item}模块");
                            return;
                        }
                    }
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, $"没有找到模块{cmd[3]}，可能大小写问题？");
                }
                else
                {
                    KiraPlugin.SendGroupMessage(g.s, g.fromGroup, "你冇权限啦！w");
                    return;
                }
                
            }
            catch (Exception ex)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, ex.Message);
                return;
            }
        }
        public static void SwitchShow(GroupMsg g, IGroupMessageEventArgs e)
        {
            try
            {
                if (!File.Exists($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira"))
                {
                    File.Create($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira").Close();
                }
                string rt = "[KIRADX模块列表]\n";
                string[] FuncList = Users.cfgs.FunctionList.Split(';');
                foreach (var item in FuncList)
                {
                    if (!item.StartsWith("*"))
                    {
                        if (BotFunc.FuncSwith(g, item))
                        {
                            rt += $"{item} On\n";
                        }
                        else
                        {
                            rt += $"{item} Off\n";
                        }
                    }

                    
                }
                if (File.Exists($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira"))
                {
                    string cfg = File.ReadAllText($"{G.path.Apppath}{G.path.SwitchData}{g.fromGroup}.kira");
                    if (cfg.ToLower().Contains("[locked]"))
                    {
                        rt += "本群模块开关已被锁定，无法编辑";
                    }
                }
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, rt);
            }
            catch (Exception  ex)
            {
                KiraPlugin.SendGroupMessage(g.s, g.fromGroup, ex.Message);
                return;
            }
        }
    }
}
