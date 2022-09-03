using MIETAPI.Orioks;
using MIETAPI.Orioks.Models;

OrioksClient orioksClient = new()
{
    ApiToken = Environment.GetEnvironmentVariable("MIEToken", EnvironmentVariableTarget.Machine)
};

SemesterInfo semester = await orioksClient.GetSemesterInfo();

Console.WriteLine(semester.CurrentWeekNumber);
Console.WriteLine(semester.CurrentWeekType);

Console.WriteLine(string.Join('\n', await orioksClient.GetGroups()));

Console.WriteLine(string.Join('\n', await orioksClient.GetPairsTimings()));

await orioksClient.GetSchedule("845");