namespace RocketLeague.ReplayLib.Console
{
    public class Program
    {
        public static void Main()
        {
            var file = "/Users/razfriman/Desktop/f635c7e8-51ce-43cb-b43e-cb46a574fc70.replay";
            for (var i = 0; i < 100; i++)
            {
                var replayReader = new ReplayReader();
                var replay = replayReader.Deserialize(file);
                System.Console.WriteLine(i);
            }
            
            System.Console.WriteLine("Done");
        }
    }
}