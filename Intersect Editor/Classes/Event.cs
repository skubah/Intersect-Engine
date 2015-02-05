﻿using System;
using System.Collections.Generic;

namespace Intersect_Editor.Classes
{
    public class Event
    {
        public string MyName = "New Event";
        public int SpawnX;
        public int SpawnY;
        public int Deleted;
        public int PageCount = 1;
        public List<EventPage> MyPages = new List<EventPage>();
        
        public Event(int x, int y)
        {
            SpawnX = x;
            SpawnY = y;
            MyPages.Add(new EventPage());
        }
        public Event(ByteBuffer myBuffer)
        {
            MyName = myBuffer.ReadString();
            SpawnX = myBuffer.ReadInteger();
            SpawnY = myBuffer.ReadInteger();
            Deleted = myBuffer.ReadInteger();
            PageCount = myBuffer.ReadInteger();
            for (var i = 0; i < PageCount; i++)
            {
                MyPages.Add(new EventPage(myBuffer));
            }
        }
        public byte[] EventData()
        {
            var myBuffer = new ByteBuffer();
            myBuffer.WriteString(MyName);
            myBuffer.WriteInteger(SpawnX);
            myBuffer.WriteInteger(SpawnY);
            myBuffer.WriteInteger(Deleted);
            myBuffer.WriteInteger(PageCount);
            for (var i = 0; i < PageCount; i++)
            {
                MyPages[i].WriteBytes(myBuffer);
            }
            return myBuffer.ToArray();
        }
    }

    public class EventPage
    {
        public EventConditions MyConditions;
        public int MovementType;
        public int MovementSpeed;
        public int MovementFreq;
        public int Passable;
        public int Layer;
        public int Trigger;
        public int GraphicType;
        public string Graphic;
        public int Graphicx;
        public int Graphicy;
        public int HideName;
        public List<CommandList> CommandLists = new List<CommandList>();

        public EventPage()
        {
            MyConditions = new EventConditions();
            MovementType = 0;
            MovementSpeed = 2;
            MovementFreq = 2;
            Passable = 0;
            Layer = 1;
            Trigger = 0;
            GraphicType = 0;
            Graphic = "";
            Graphicx = -1;
            Graphicy = -1;
            HideName = 0;
            CommandLists.Add(new CommandList());
        }

        public EventPage(ByteBuffer curBuffer)
        {
            MyConditions = new EventConditions();
            MyConditions.Load(curBuffer);
            MovementType = curBuffer.ReadInteger();
            MovementSpeed = curBuffer.ReadInteger();
            MovementFreq = curBuffer.ReadInteger();
            Passable = curBuffer.ReadInteger();
            Layer = curBuffer.ReadInteger();
            Trigger = curBuffer.ReadInteger();
            GraphicType = curBuffer.ReadInteger();
            Graphic = curBuffer.ReadString();
            Graphicx = curBuffer.ReadInteger();
            Graphicy = curBuffer.ReadInteger();
            HideName = curBuffer.ReadInteger();
            var x = curBuffer.ReadInteger();
            for (var i = 0; i < x; i++)
            {
                CommandLists.Add(new CommandList(curBuffer));
            }

        }

        public void WriteBytes(ByteBuffer myBuffer)
        {
            MyConditions.WriteBytes(myBuffer);
            myBuffer.WriteInteger(MovementType);
            myBuffer.WriteInteger(MovementSpeed);
            myBuffer.WriteInteger(MovementFreq);
            myBuffer.WriteInteger(Passable);
            myBuffer.WriteInteger(Layer);
            myBuffer.WriteInteger(Trigger);
            myBuffer.WriteInteger(GraphicType);
            myBuffer.WriteString(Graphic);
            myBuffer.WriteInteger(Graphicx);
            myBuffer.WriteInteger(Graphicy);
            myBuffer.WriteInteger(HideName);
            myBuffer.WriteInteger(CommandLists.Count);
            foreach (var t in CommandLists)
            {
                t.WriteBytes(myBuffer);
            }
        }

    }

    public class EventConditions
    {
        public int Switch1;
        public bool Switch1Val;
        public int Switch2;
        public bool Switch2Val;

        public void WriteBytes(ByteBuffer myBuffer)
        {
            myBuffer.WriteInteger(Switch1);
            myBuffer.WriteInteger(Convert.ToInt32(Switch1Val));
            myBuffer.WriteInteger(Switch2);
            myBuffer.WriteInteger(Convert.ToInt32(Switch1Val));
        }

        public void Load(ByteBuffer myBuffer)
        {
            Switch1 = myBuffer.ReadInteger();
            Switch1Val = Convert.ToBoolean(myBuffer.ReadInteger());
            Switch2 = myBuffer.ReadInteger();
            Switch2Val = Convert.ToBoolean(myBuffer.ReadInteger());
        }
    }

    public class CommandList
    {
        public List<EventCommand> Commands = new List<EventCommand>();

        public CommandList()
        {

        }

        public CommandList(ByteBuffer myBuffer)
        {
            var y = myBuffer.ReadInteger();
            for (var i = 0; i < y; i++)
            {
                Commands.Add(new EventCommand());
                Commands[i].Type = myBuffer.ReadInteger();
                if (Commands[i].Type != 4)
                {
                    for (var x = 0; x < 6; x++)
                    {
                        Commands[i].Strs[x] = myBuffer.ReadString();
                        Commands[i].Ints[x] = myBuffer.ReadInteger();
                    }
                }
                else
                {
                    Commands[i].MyConditions.Load(myBuffer);
                    for (var x = 0; x < 6; x++)
                    {
                        Commands[i].Strs[x] = myBuffer.ReadString();
                        Commands[i].Ints[x] = myBuffer.ReadInteger();
                    }
                }
            }
        }

        public void WriteBytes(ByteBuffer myBuffer)
        {
            myBuffer.WriteInteger(Commands.Count);
            foreach (var t in Commands)
            {
                myBuffer.WriteInteger(t.Type);
                if (t.Type != 4)
                {
                    for (var x = 0; x < 6; x++)
                    {
                        myBuffer.WriteString(t.Strs[x]);
                        myBuffer.WriteInteger(t.Ints[x]);
                    }
                }
                else
                {
                    t.MyConditions.WriteBytes(myBuffer);
                    for (var x = 0; x < 6; x++)
                    {
                        myBuffer.WriteString(t.Strs[x]);
                        myBuffer.WriteInteger(t.Ints[x]);
                    }
                }
            }
        }
    }

    public class EventCommand
    {
        public int Type;
        public EventConditions MyConditions;
        public string[] Strs = new string[6];
        public int[] Ints = new int[6];
        public EventCommand()
        {
            MyConditions = new EventConditions();
            for (var i = 0; i < 6; i++)
            {
                Strs[i] = "";
                Ints[i] = 0;
            }
        }
    }
}
