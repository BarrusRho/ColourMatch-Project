using System.Collections.Generic;
using UnityEngine;

namespace ColourMatch
{
    [CreateAssetMenu(fileName = "LogChannels", menuName = "ColourMatch/LogChannels")]
    public class LogChannelsSO : ScriptableObject
    {
        public List<LogChannel> LogChannels;
    }
} 
