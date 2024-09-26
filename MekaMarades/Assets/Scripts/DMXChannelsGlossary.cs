using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DMXChannelsGlossary
{
    public const byte LIGHT_ADRESS = 1 - 1;
    public const byte DIMMER_ADRESS = 13 - 1;

    // Light Channels
    public const byte LIGHT_PAN_CHANNEL = LIGHT_ADRESS + 1;
    public const byte LIGHT_TILT_CHANNEL = LIGHT_ADRESS + 3;
    public const byte LIGHT_COLOR_CHANNEL = LIGHT_ADRESS + 5;
    public const byte LIGHT_GOBO_CHANNEL = LIGHT_ADRESS + 6;
    public const byte LIGHT_STROBE_CHANNEL = LIGHT_ADRESS + 7;
    public const byte LIGHT_DIMMER_CHANNEL = LIGHT_ADRESS + 8;

    // Dimmer Channels
    public const byte DIMMER_1_CHANNEL = DIMMER_ADRESS + 1;
    public const byte DIMMER_2_CHANNEL = DIMMER_ADRESS + 2;
    public const byte DIMMER_3_CHANNEL = DIMMER_ADRESS + 3;
    public const byte DIMMER_4_CHANNEL = DIMMER_ADRESS + 4;
}
