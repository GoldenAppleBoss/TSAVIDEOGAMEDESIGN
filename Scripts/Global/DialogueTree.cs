using System.Collections.Generic;
using UnityEngine;  // Add this line to use JsonUtility

namespace GlobalClasses
{
     [System.Serializable]
     public class DialogueNode
     {
          public string Id;
          public string Text;
          public List<DialogueBranch> Choices = new List<DialogueBranch>();
     }

     [System.Serializable]
     public class DialogueBranch
     {
          public string Id;
          public string Text;
          public string NextDialogueId;
     }

     [System.Serializable]
     public class DialogueNpc
     {
          public string NpcId;
          public string NpcName;
          public List<DialogueNode> DialogueLines = new List<DialogueNode>();
     }

     public class Dialogue
     {
          public List<DialogueNpc> Root = new List<DialogueNpc>();

          // Constructor to load the Dialogue from JSON
          public Dialogue(string jsonData)
          {
               DialogueContainer container = JsonUtility.FromJson<DialogueContainer>(jsonData); // Add this line
               Root = container.Root;
          }
     }

     [System.Serializable]
     public class DialogueContainer
     {
          public List<DialogueNpc> Root;
     }

     [System.Serializable]
     public class DialogueHistory
     {
          public DialogueNode CurrentNode;
          public List<DialogueNode> PreviousNodes = new List<DialogueNode>();
     }

}
