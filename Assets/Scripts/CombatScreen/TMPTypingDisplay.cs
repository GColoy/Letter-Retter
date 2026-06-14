using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace CombatScreen
{
    class TMPTypingDisplay : TypingDisplay
    {
        public Color untypedColor;
        public Color correctlyTypedColor;
        public Color incorretlyTypedColor;
        public TMP_Text textMesh;
        string originText;

        public override void initializeText(string text)
        {
            originText = text;
            displayProgress("");
        }

        public override void displayProgress(string text)
        {
            textMesh.SetText(createRichText(text));
        }

        string createRichText(string typedText)
        {
            string richText = "";
            for (int i = 0; i < originText.Length; i++)
            {
                if (i >= typedText.Length) richText += $"<color=#{untypedColor.ToHexString()}>{originText[i]}";
                else if (originText[i] == typedText[i]) richText += $"<color=#{correctlyTypedColor.ToHexString()}>{originText[i]}";
                else richText += $"<color=#{incorretlyTypedColor.ToHexString()}>{originText[i]}";
            }
            return richText;
        }


    }
}