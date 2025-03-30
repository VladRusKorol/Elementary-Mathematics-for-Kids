using System.Collections.Generic;
using System.Linq.Expressions;

internal class Program
{

    private static void Main(string[] args)
    {
        int countPlayers;
        int countLaps;
        int maxNumber;
        int minNumber;
        List<LapResult> laps = new List<LapResult>();

        Console.Write("Введите количество игроков:  "); countPlayers = Int32.Parse(Console.ReadLine());
        Console.Write("Введите количество кругов задач: "); countLaps = Int32.Parse(Console.ReadLine());
        Console.Write("Введите минимальное число: "); minNumber = Int32.Parse(Console.ReadLine());
        Console.Write("Введите максимальное число: "); maxNumber = Int32.Parse(Console.ReadLine());
       
       

        List<PlayerResult> massPlayersName = new List<PlayerResult>();

        for (int i = 0; i < countPlayers; i++) 
        { 
            Console.Write($"Введите имя {i+1}-го игрока:  "); 
            string playerName = Console.ReadLine();
            PlayerResult playerResult = new PlayerResult();
            playerResult.playerName = playerName;
            playerResult.maxtimeResultName = 0;
            massPlayersName.Add(playerResult);           
            
        } 

 RestartFlag:

        System.Console.Clear();
        System.Threading.Thread.Sleep(1000);
        Console.WriteLine( "Начинаем игру...удачи");
        System.Threading.Thread.Sleep(1000);
        System.Console.Clear();

        for (int i = 0; i < countPlayers; i++) 
        {
            Game(i, ref countLaps, massPlayersName, laps, minNumber, maxNumber);
        }

        System.Console.Clear();
        System.Threading.Thread.Sleep(2000);
        Console.WriteLine("Результаты\n");

        foreach (PlayerResult playerResult in massPlayersName)
        {
            List<LapResult> playerLaps = laps.Where(x => x.playerName == playerResult.playerName).ToList();
            Console.WriteLine($"Игрок {playerResult.playerName} || мин время ответа {Math.Round(playerResult.mintimeResultName, 2, MidpointRounding.ToZero)}|| макс время ответа {Math.Round(playerResult.maxtimeResultName, 2, MidpointRounding.ToZero)}|| среднее время ответа {Math.Round(playerResult.avgtimeResultName, 2, MidpointRounding.ToZero)}|| правильных ответов {playerResult.countGoodResult} || неправильных ответов {playerResult.countBadResult}\n");
            for (int i = 0; i < playerLaps.Count; i++)
            {
                LapResult lap = playerLaps[i];
                if (lap.isTrueResponse) Console.ForegroundColor = ConsoleColor.Green;
                else Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"имя:{lap.playerName}|круг:{lap.lapNumber + 1}|выражение:{lap.expression}|твой ответ:{lap.playerResponse}|ответ:{lap.systemResponse}|время:{Math.Round(lap.duration, 2, MidpointRounding.ToZero)} сек|\n");
                Console.ResetColor();
            }
        }        
        Console.ResetColor();

        Console.Write("Повторим игру с установленными настройками?(y/n):    ");
        var v = Console.ReadLine();

        if (v =="y") 
        {
            System.Console.Clear();
            laps.Clear();
            ClearSummary(massPlayersName);
            goto RestartFlag;
        }

        Environment.Exit(0);
    }

    public static void Game(int playerIndex,ref int countLaps, List<PlayerResult> massPlayersName, List<LapResult> laps, int minNumber, int maxNumber)
    {

        for (int i = 0; i < countLaps; i++)
        {
            int firstNumber, lastNumber;
            char symbol = Random.Shared.Next(1, 3) == 1 ? '+' : '-';
            do
            {
                firstNumber = Random.Shared.Next(minNumber, maxNumber+1);
                lastNumber = Random.Shared.Next(minNumber, maxNumber+1);
            } while (firstNumber - lastNumber < 0);

            int systemResult = symbol switch
            {
                '+' => firstNumber + lastNumber,
                '-' => firstNumber - lastNumber,
                _ => throw new NotImplementedException()
            };

            Console.Write($"Игрок {massPlayersName[playerIndex].playerName} введи ответ на эту задачу:   {firstNumber} {symbol} {lastNumber} = ");

            DateTime startTimestamp = DateTime.Now;

            int playerResult = Int32.Parse(Console.ReadLine());

            DateTime endTimestamp = DateTime.Now;

            double duration = (endTimestamp - startTimestamp).TotalSeconds;

            laps.Add(new LapResult
            {
                playerName = massPlayersName[playerIndex].playerName,
                lapNumber = i,
                expression = $"{ firstNumber } { symbol } { lastNumber }",
                playerResponse = playerResult,
                systemResponse = systemResult,
                isTrueResponse = playerResult == systemResult ? true : false,
                duration = duration 
            });

            var currentPlaytr = massPlayersName[playerIndex];
            
            if (i == 0)
            {
                currentPlaytr.maxtimeResultName = duration;
                currentPlaytr.mintimeResultName = duration;
            }
            else if (currentPlaytr.maxtimeResultName < duration)
            {
                currentPlaytr.maxtimeResultName = duration;
            }
            else if (currentPlaytr.mintimeResultName > duration)
            {
                currentPlaytr.mintimeResultName = duration;
            }

            if(playerResult == systemResult)
            {
                currentPlaytr.countGoodResult += 1;
            }
            else
            {
                currentPlaytr.countBadResult += 1;
            }

            currentPlaytr.avgtimeResultName += duration;

            massPlayersName[playerIndex] = currentPlaytr;

            System.Console.Clear();
        }

        var currentMustPlaytr = massPlayersName[playerIndex];
        currentMustPlaytr.avgtimeResultName = currentMustPlaytr.avgtimeResultName / countLaps;
        massPlayersName[playerIndex] = currentMustPlaytr;
    }

    public static void ClearSummary(List<PlayerResult> massPlayersName)
    {
        for (int i = 0; i < massPlayersName.Count; i++)
        {
            var tmp = massPlayersName[i];
            tmp.mintimeResultName = 0;
            tmp.maxtimeResultName = 0;
            tmp.avgtimeResultName = 0;
            tmp.countGoodResult = 0;
            tmp.countBadResult = 0;
            massPlayersName[i] = tmp;
        }
    }

}

public struct LapResult
{
    public string playerName;
    public int lapNumber;
    public string expression; 
    public int playerResponse;
    public int systemResponse;
    public bool isTrueResponse;
    public double duration;
};

public struct PlayerResult
{
    public string playerName;
    public double mintimeResultName;
    public double maxtimeResultName;
    public double avgtimeResultName;
    public int countGoodResult;
    public int countBadResult;
}

