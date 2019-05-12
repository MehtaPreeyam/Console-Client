using System;
using System.Collections.Generic;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// This alerts ally chat when stronghold is getting lost
    /// </summary>

    public class Outpost : ChatBot
    {
        private int count;
        private int timeping;
        private bool Controlled = false;
        private bool Vanilla_Controlled = false;
        private bool Hero_Controlled = false;
        private bool Trainee_Controlled = false;
        private bool Cosmonaut_Controlled = false;
        private double outpost_percent = 100.0;
        private bool UNDER_ATTACK = false;
        private string FACTION_NAME = "BadRep";

        /// <summary>
        /// This alerts ally chat when stronghold is getting lost
        /// </summary>

        public override void GetText(string text)
        {
            text = GetVerbatim(text);
            if(text.Contains("BadRep have lost the "))
            {
                Controlled = false;
                string[] spl = text.Split(' ');
                if (spl[5].Equals("Vanilla"))
                    Vanilla_Controlled = false;
                if (spl[5].Equals("Trainee"))
                    Trainee_Controlled = false;
                if (spl[5].Equals("Hero"))
                    Hero_Controlled = false;
                if (spl[5].Equals("Cosmonaut"))
                    Cosmonaut_Controlled = false;
            }
            if (text.Contains("Outpost: "))
            {
                string inside = (text.Split('[', ']')[1]);
                double percent = double.Parse(inside.Substring(0, inside.Length - 1));
                //Console.WriteLine(percent);
                if (Controlled)
                {
                    if (text[0] == 'V' && Vanilla_Controlled)
                    {
                        if (text.Contains("UNDER ATTACK"))
                            UNDER_ATTACK = true;
                        outpost_percent = percent;
                    }
                    if (text[0] == 'T' && Trainee_Controlled)
                    {
                        if (text.Contains("UNDER ATTACK"))
                            UNDER_ATTACK = true;
                        outpost_percent = percent;
                    }
                    if (text[0] == 'H' && Hero_Controlled)
                    {
                        if (text.Contains("UNDER ATTACK"))
                            UNDER_ATTACK = true;
                        outpost_percent = percent;
                    }
                    if (text[0] == 'C' && Cosmonaut_Controlled)
                    {
                        if (text.Contains("UNDER ATTACK"))
                            UNDER_ATTACK = true;
                        outpost_percent = percent;
                    }
                }
                string faction = "";
                if(text.Contains("("))
                    faction = text.Split('(', ')')[1];
                //Console.WriteLine(faction);
                if (faction.Equals(FACTION_NAME) && text.Contains("CONTROLLED"))
                {
                    UNDER_ATTACK = false;
                    outpost_percent = percent;
                    Controlled = true;
                    if (text[0] == 'V')
                        Vanilla_Controlled = true;
                    if (text[0] == 'T')
                        Trainee_Controlled = true;
                    if (text[0] == 'H')
                        Hero_Controlled = true;
                    if (text[0] == 'C')
                        Cosmonaut_Controlled = true;
                }
                if(text.Contains("Vanilla") && Vanilla_Controlled)
                {
                    if(text.Contains("SECURING") || text.Contains("CAPTURING") || text.Contains("IDLE"))
                        UNDER_ATTACK = false;
                }
                if (text.Contains("Trainee") && Trainee_Controlled)
                {
                    if (text.Contains("SECURING") || text.Contains("CAPTURING") || text.Contains("IDLE"))
                        UNDER_ATTACK = false;
                }
                if (text.Contains("Hero") && Hero_Controlled)
                {
                    if (text.Contains("SECURING") || text.Contains("CAPTURING") || text.Contains("IDLE"))
                        UNDER_ATTACK = false;
                }
                if (text.Contains("Cosmonaut") && Cosmonaut_Controlled)
                {
                    if (text.Contains("SECURING") || text.Contains("CAPTURING") || text.Contains("IDLE") || text.Contains("CONTESTED"))
                        UNDER_ATTACK = false;
                }
            }
        }
        public Outpost(int timeping)
        {
            count = 0;
            this.timeping = timeping;
        }
        public override void Update()
        {
            count++;
            if (count == timeping)
            {
                SendText("/outpost");
                if (Controlled && UNDER_ATTACK)
                {
                    SendText("Outpost has dropped to " + outpost_percent + "%, HOLD THE DOOOOR");
                }
                count = 0;
            }
        }
    }
}
