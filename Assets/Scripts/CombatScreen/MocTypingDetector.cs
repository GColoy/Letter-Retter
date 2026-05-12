using UnityEngine;
using UnityEngine.Assemblies;

namespace CombatScreen
{
    class MocTypingDetector : TypingDetector
    {
        public string totype;
        public int index;
        public string next;

        public override string get_latest_keys()
        {
            if (next == null || next == "") return "";
            string send = next;
            next = "";
            return send;
        }

        public void type_next_key()
        {
            if (index >= totype.Length) index = 0;
            next = totype.Substring(index, 1);
            index += 1;
        }
    }
}