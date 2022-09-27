using MIETAPI.Orioks;
using MIETAPI.Orioks.Models;
using MIETAPI.Orioks.Models.Schedule;

OrioksClient orioksClient = new()
{
    ApiToken = Environment.GetEnvironmentVariable("MIEToken", EnvironmentVariableTarget.Machine)
};

SemesterInfo semester = await orioksClient.GetSemesterInfo();

Console.WriteLine(semester.CurrentWeekNumber);
Console.WriteLine(semester.CurrentWeekType);

Console.WriteLine(string.Join('\n', await orioksClient.GetGroups()));

Console.WriteLine(string.Join('\n', await orioksClient.GetPairsTimings()));

Schedule schedule = await orioksClient.GetSchedule("1600");

for (int i = 0; i < 4; i++)
{
    Week w = schedule.GetForWeek(i);
    for (int j = 0; j < 7; j++)
    {
        foreach (Subject subject in w.GetDay(j))
        {
            Console.WriteLine(subject);
            Console.WriteLine("----------------------");
        }
    }
    Console.WriteLine("----------------------");
    Console.WriteLine("----------------------");
}