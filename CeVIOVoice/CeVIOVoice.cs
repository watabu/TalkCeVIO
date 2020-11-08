using CeVIO.Talk.RemoteService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeVIOCtrl
{
    class CeVIOController
    {
        static void Main(string[] args)
        {
            foreach(string text in args){
                Talk(text);
            }
        }
 

        internal static void Talk(string text)
       {
            ServiceControl.StartHost(false);
            Talker talker = new Talker();
            talker.Cast = "さとうささら";
            SpeakingState state = talker.Speak(text);
            state.Wait();
        }
    }
}