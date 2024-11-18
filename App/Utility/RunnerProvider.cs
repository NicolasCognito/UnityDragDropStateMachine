using Fusion;
 
 namespace App.Utils
 {
     public class RunnerProvider : NetworkBehaviour
     {
         private static RunnerProvider _instance;

         private static RunnerProvider Instance
         {
             get
             {
                 if (_instance == null || _instance.Runner.IsRunning == false)
                 {
                     _instance = FindObjectOfType<RunnerProvider>();
                 }
                 return _instance;
             }
         }
 
         public static NetworkRunner ActiveRunner => Instance.Runner;
     }
 }