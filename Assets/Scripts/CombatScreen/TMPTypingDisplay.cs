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
        public char spaceReplacement = '·';
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
                char displayChar = originText[i] == ' ' ? spaceReplacement : originText[i];
                if (i >= typedText.Length) richText += $"<color=#{untypedColor.ToHexString()}>{displayChar}";
                else if (originText[i] == typedText[i]) richText += $"<color=#{correctlyTypedColor.ToHexString()}>{displayChar}";
                else richText += $"<color=#{incorretlyTypedColor.ToHexString()}>{displayChar}";
            }
            return richText;
        }


    }
}